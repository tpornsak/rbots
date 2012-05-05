using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AForge.Video;
using AForge.Video.DirectShow;
using System.Threading;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Imaging.Textures;
using System.IO;


namespace CameraTrendnetAForge
{
    public partial class FormPilot : Form
    {
        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetTextCallback(string text);
        delegate void SetPictureCallback(ref Bitmap image);
        private delegate void UIUpdater();
        private bool portOpen = false;
        private bool zoomSpeed = false;
        private bool centerFlag = false;

        private AutoResetEvent are = new AutoResetEvent(false);

        // buffers to stor IR 'radar' data, save full sweep before overwriting
        const int bufferSize = 20;
        int idxBufferRight = 0;
        double [] bufferRightIR    = new double[bufferSize];
        double[] bufferLeftIR = new double[bufferSize];
        float[] bufferPhi = new float[bufferSize];
        int [] bufferRightIRPos    = new int[bufferSize];
        int[] bufferLeftIRPos = new int[bufferSize];
        int currentAz = 0;



        // variables to store for label for serial rx, needed
        // since we cannot edit labels inside rx interrupt
        double cmLeftIR  = 0.0;
        double cmRightIR = 0.0;
        double cmFrontIR = 0.0;
        double cmBackIR  = 0.0;

        double cmServoLeftIR  = 0.0;
        double cmServiRightIR = 0.0;

        byte servoPosRight = 0;
        byte servoPosLeft = 0;

        double bearing   = 0.0;

        byte returnCheckSum = 0x00;

        // create filter
        RotateBilinear filterRotate = new RotateBilinear(90.0,true);
        // create filter
        CannyEdgeDetector filterCanny = new CannyEdgeDetector();
        // create filters sequence
        FiltersSequence filter = new AForge.Imaging.Filters.FiltersSequence();

        Grayscale filterGS = new Grayscale(0.2125, 0.7154, 0.0721);
        Mirror filterMirror = new Mirror(false, true);

        // hitpoints
        byte hitPoints = 20;

        // Target plate that was hit
        byte targetPlate = 0;

        //
        private int latchButton = 0; // keep track of button latching for commander protocol

        private const float deg2rad = (float) (Math.PI / 180.0);

        // Match timer variables
        private int minutes = 0;
        private int seconds = 0;

        // statistics length
        private const int statLength = 15;
        // current statistics index
        private int statIndex = 0;
        // ready statistics values
        private int statReady = 0;

        // fire gun variable, 0: No fire, 1: Fire
        private byte gunFire = 0x00;

        private byte [] inByte = {0x00};
        private byte [] readBytes;

        // stop iscmdLeg = 0x01, forwad is cmdLeg = 0x02, 
        private byte cmdLeg = 0x00;

        // length of command
        private int cmdLength = 12;
        private int mousePrevX = 0;
        private int mousePrevY = 0;
        private bool mouseMoveInitFlag = false;
        private bool armed = false;

        private double scaleMouseHeight = 0;
        private double scaleMouseWidth = 0;
        

        // statistics array
        private int[] statCount = new int[statLength];
        //string cameraURL = "http://192.168.1.86/img/video.mjpeg";
        //string cameraURL = "http://192.168.2.175/img/video.mjpeg";
        //string cameraURL = "http://192.168.0.103:8080/live.flv";
        //string cameraURL = "http://192.168.1.16:8080/videofeed";
        string cameraURL = "http://192.168.0.56:8080/videofeed";
        //string cameraURL = "http://192.168.0.102:8080/ipcam";
        Thread t;

        public FormPilot()
        {
            
            InitializeComponent();
            filter.Add(new RotateBicubic(90.0, true));
            //filter.Add(new Grayscale(0.299, 0.587, 0.114));
            //filter.Add(new Grayscale(0.2125, 0.7154, 0.0721)); 
            //filter.Add(new CannyEdgeDetector());
            
            InitializeCamera();
            InitializeSerialPort();
            scaleMouseHeight = (double)(trackBarElPos.Maximum - trackBarElPos.Minimum) / pictureBoxMech.Height;
            scaleMouseWidth = (double)(trackBarAzPos.Maximum - trackBarAzPos.Minimum) / pictureBoxMech.Width;

            for (int i = 0; i < bufferRightIR.Length; i++)
            {
                bufferRightIR[i] = 0.0;
                bufferRightIRPos[i] = 120;

                bufferLeftIR[i] = 0.0;
                bufferLeftIRPos[i] = 0;

                bufferPhi[i] = 0;
            }

           
        }

        private void InitializeSerialPort()
        {
            try
            {
                serialPortMech.Open();
                portOpen = true;
                t = new Thread(new ThreadStart(MonitorPort));
                //serialPortMech.WriteTimeout = 4000;
                //serialPortMech.Ti
            }
            catch (Exception ex)
            {
                textBoxDebug.AppendText(ex.Message.ToString() + Environment.NewLine);
            }
        }
        private void InitializeCamera()
        {
            try
            {
                // create video source
                MJPEGStream mjpegSource = new MJPEGStream(cameraURL);
                textBoxDebug.AppendText("Camera URL" + System.Environment.NewLine);
                textBoxDebug.AppendText(cameraURL + System.Environment.NewLine);
                mjpegSource.Login = "robotacronym";
                mjpegSource.Password = "robotacronym";
                //mjpegSource.RequestTimeout = 5000;

                //mjpegSource.SeparateConnectionGroup = true;
                // open it
                OpenVideo(mjpegSource);
            }
            catch (Exception ex)
            {
                textBoxDebug.AppendText(ex.Message.ToString() + Environment.NewLine);
            }
        }

        // Open video source
        private void OpenVideo(IVideoSource source)
        {
            // set busy cursor
            this.Cursor = Cursors.WaitCursor;

            // stop current video source
            mechCamera.SignalToStop();
            mechCamera.WaitForStop();

            // start new video source
            mechCamera.VideoSource = source;
            mechCamera.Start();

            // reset statistics
            statIndex = statReady = 0;

            // start timer
            timer.Start();

         

            this.Cursor = Cursors.Default;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void UIUpdate()
        {
           // textBox1.Text = serialData;
        }

        private void MonitorPort()
        {
            try
            {
                //All this thread does is write the serial data to the UI.
                //This simple example is not written to prevent data loss!
                while (portOpen)
                {
                    this.Invoke(new UIUpdater(UIUpdate));
                    Thread.Sleep(100);
                }
            }
            finally
            {
                //Signal that the thread is terminating
                are.Set();
            }
        }

        private void FormTrendnet_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (mechCamera.VideoSource != null)
                {
                    mechCamera.SignalToStop();
                    //mechCamera.WaitForStop();
                    //mechCamera.Stop();
                }
                if (serialPortMech.IsOpen)
                {
                    //serialPortMech.Close();
                    //Tell MonitorPort() to stop updating the UI
                    portOpen = false;

                    //Wait for MonitorPort() to signal that it's done
                    are.WaitOne();

                    serialPortMech.Close();
                }
            }
            catch (Exception ex)
            {
                textBoxDebug.AppendText(ex.Message.ToString() + System.Environment.NewLine);
            }
        }


        // InvokeRequired required compares the thread ID of the
        private void SetPicture(ref Bitmap image)
        {
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.mechCamera.InvokeRequired)
            {
                SetPictureCallback d = new SetPictureCallback(SetPicture);
                this.Invoke(d, new object[] { image });
            }
            else
            {
                try
                {

                    DateTime now = DateTime.Now;
                    Bitmap img2 = (Bitmap)filter.Apply(image);
                    Graphics g = Graphics.FromImage(img2);
                    //filterCanny.ApplyInPlace(image);

                    // draw cross-hairs
                    Pen pR = new Pen(Color.Red);

                    // draw center point
                    Pen pG = new Pen(Color.Green);


                    if (!armed)
                    {
                        Font armFont = new Font("arial", 16.0f);
                        Color clr = Color.Red;
                        SolidBrush br = new SolidBrush(clr);


                        g.DrawString("NOT ARMED", armFont, br, 10.0f / 4, 240.0f / 4);
                    }

                    g.DrawEllipse(pG, 88.0f - 3.0f, 72.0f - 3.0f, 20.0f, 20.0f);
                    //g.DrawLine(pR, 137.0f, 120.0f, 173.0f, 120.0f);
                    //g.DrawLine(pR, 150.0f, 107.0f, 150.0f, 133.0f);
                    g.DrawRectangle(pR, 366.0f / 4, 257.0f / 4, 20.0f / 2, 20.0f / 2);
                    //filterCanny.ApplyInPlace(img);
                    //pictureBoxMech.Image = img2;
                    //mechCamera.BackgroundImage = img;
                    // mechCamera.

                    pR.Dispose();
                    pG.Dispose();

                    g.Dispose();
                }
                catch (Exception ex)
                {
                   textBoxDebug.AppendText(ex.Message.ToString() + System.Environment.NewLine);
                }
            }
        }

        private void mechCamera_NewFrame(object sender, ref Bitmap image)
        {
            try
            {
                // DateTime now = DateTime.Now;
                // add filters to the sequence
                //SetPicture(ref image);
                //filter.Add(new RotateBicubic(90.0, true));
                //filter.Add(new CannyEdgeDetector());
               // Bitmap img2 = (Bitmap)image.Clone();
                // apply the filter sequence
                Bitmap img2 = (Bitmap)filter.Apply(image);
                //Bitmap grayImage = new Bitmap( filterGS.Apply(image));
                //Bitmap newImage = filterRotate.Apply(image);
                //filterCanny.GaussianSize = 3; 
                //filterCanny.ApplyInPlace(image);
                //filterMirror.ApplyInPlace(image);
                //Bitmap img = (Bitmap)image.Clone();
                //Bitmap img = (Bitmap) image.Clone();
                //img = (Bitmap) filterRotate.Apply(img);
                //img = (Bitmap) filterGS.Apply(img);
                //Bitmap img2 = (Bitmap)img.Clone();
                //img2 = (Bitmap) filterGS.Apply(img);
                //filterCanny.ApplyInPlace(img);
                //do processing here
                //pictureBox1.Image = img;
                // Derive BitMap object using Image instance, so that you can avoid the issue
                //"a graphics object cannot be created from an image that has an indexed pixel format"
                Bitmap img = new Bitmap(new Bitmap(img2));

                Graphics g = Graphics.FromImage(img);
                //filterCanny.ApplyInPlace(image);

                // draw cross-hairs
                Pen pR = new Pen(Color.Red);
                // draw center point
               // Pen pG = new Pen(Color.Green);


                if (!armed)
                {
                    Font armFont = new Font("arial", 16.0f);
                    Color clr = Color.Red;
                    SolidBrush br = new SolidBrush(clr);


                    g.DrawString("NOT ARMED", armFont, br, 10.0f / 4, 240.0f / 4);
                }

                //g.DrawLine(pR, 137.0f, 120.0f, 173.0f, 120.0f);
                //g.DrawLine(pR, 150.0f, 107.0f, 150.0f, 133.0f);
                //g.DrawRectangle(pR, 366.0f / 4, 257.0f / 4, 20.0f / 2, 20.0f / 2);
                g.DrawRectangle(pR, (366.0f / 4.0f)-10.0f, (252.0f / 4.0f) + 10.0f, 20.0f / 2, 20.0f / 2);   //176x144
               //g.DrawRectangle(pR, (366.0f / 4.0f) - 20.0f, (252.0f / 4.0f) - 10.0f, 20.0f / 2, 20.0f / 2);
                //g.DrawRectangle(pR, (366.0f / 4.0f) * 1.8f -20.0f, (252.0f / 4.0f) * 1.8f + 15.0f, 20.0f / 2, 20.0f / 2);   // 320x240
                
                //g.DrawEllipse(pG, 88.0f - 3.0f, 72.0f - 3.0f, 6.0f, 6.0f);
                //filterCanny.ApplyInPlace(img);
                pictureBoxMech.Image = img;
                //mechCamera.BackgroundImage = img;
                // mechCamera.

                pR.Dispose();
                //pG.Dispose();
                g.Dispose();

                //img2.Dispose();
                //img.Dispose();
                //grayImage.Dispose();
                //updateData();
                
                //Calling the UI thread using the Dispatcher to update the 'Image' WPF control         
                
                //  .Dispatcher.BeginInvoke(new ThreadStart(delegate
                //{
                //    pictureBoxMech.Image = img2; /*frameholder is the name of the 'Image' WPF control*/
                //}));
            }
            catch (Exception ex)
            {
                int i = 1;
               // textBoxDebug.AppendText(ex.Message.ToString() + System.Environment.NewLine);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //webBrowserCam.Refresh();

            IVideoSource videoSource = mechCamera.VideoSource;

            if (videoSource != null)
            {
                // get number of frames for the last second
                statCount[statIndex] = videoSource.FramesReceived;

                // increment indexes
                if (++statIndex >= statLength)
                    statIndex = 0;
                if (statReady < statLength)
                    statReady++;

                float fps = 0;

                // calculate average value
                for (int i = 0; i < statReady; i++)
                {
                    fps += statCount[i];
                }
                fps /= statReady;

                statCount[statIndex] = 0;

                toolStripLabelFPS.Text = fps.ToString("F2") + " fps";
            }

            //textBoxFocus.AppendText(this.ActiveControl.ToString() + Environment.NewLine);
            updateData();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            textBoxElPos.Text = trackBarElPos.Value.ToString();
            updateTurret();
            //pictureBoxMech.Focus();
            
        }

        private void updateTurret()
        {
            try
            {
               // textBoxElPos.Text = trackBarElPos.Value.ToString();
               // textBoxElSpeed.Text = trackBarElSpeed.Value.ToString();
               //textBoxAzPos.Text = trackBarAzPos.Value.ToString();
               // textBoxAzSpeed.Text = trackBarAzSpeed.Value.ToString();
               // //short goalElPos = (short)(1023 * trackBarElPos.Value / 300);
               // //short goalElSpeed = (short)(1023 * trackBarElSpeed.Value / 114);
               // //short goalAzPos = (short)(1023 * trackBarAzPos.Value / 300);
               // //short goalAzSpeed = (short)(1023 * trackBarAzSpeed.Value / 114);
                if (zoomSpeed)
                {
                    trackBarAzSpeed.Value = 50;
                }
                else
                {
                    trackBarAzSpeed.Value = 150;
                }

               short goalElPos = (short) ( trackBarElPos.Maximum - trackBarElPos.Value + trackBarElPos.Minimum ) ;
               short goalElSpeed = (short)trackBarElSpeed.Value;
               short goalAzPos = (short)(trackBarAzPos.Maximum - trackBarAzPos.Value + trackBarAzPos.Minimum);
               //short goalAzPos = 900;  //testing the CCW limit in AX12;
               //short goalAzPos = (short)(trackBarAzPos.Value);
               short goalAzSpeed = (short)trackBarAzSpeed.Value;
               byte goalElPosLow = (byte)(goalElPos & 0xff);
               byte goalElPosHigh = (byte)(goalElPos >> 8);
               byte goalAzPosLow = (byte)(goalAzPos & 0xff);
               byte goalAzPosHigh = (byte)(goalAzPos >> 8);

               currentAz = trackBarAzPos.Value; 
               byte goalElSpeedLow = (byte)(goalElSpeed & 0xff);
               byte goalElSpeedHigh = (byte)(goalElSpeed >> 8);
               byte goalAzSpeedLow = (byte)(goalAzSpeed & 0xff);
               byte goalAzSpeedHigh = (byte)(goalAzSpeed >> 8);

               ////byte[] cmdBytes = new byte[cmdLength] { 0x2A, cmdLeg, 0x00, goalElPosLow, goalElPosHigh, goalElSpeedLow, goalElSpeedHigh,
               // //                             goalAzPosLow, goalAzPosHigh, goalAzSpeedLow, goalAzSpeedHigh,  gunxsFire};

               // byte[] cmdBytes = new byte[8] { 0xFF, 0xC8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
               // cmdBytes[7] = (byte) (0xFF - (byte) (cmdBytes[1] + cmdBytes[2] + cmdBytes[3] + cmdBytes[4] + cmdBytes[5]));
               // textBoxDebug.AppendText("Commands!!!: " + cmdBytes[7].ToString());
               // // reset gun to zero
               // gunFire = 0x00;

               // // reset legcmd to zero
               // cmdLeg = 0x00;

               // if (serialPortMech.IsOpen)
               // {
               //     serialPortMech.Write(cmdBytes, 0, cmdLength);
               // }


                // textBoxDebug.AppendText(cmdBytes[0].ToString());

                // for(int i=1;i < cmdLength;i++)
                //     textBoxDebug.AppendText("," + cmdBytes[i].ToString());
                //  textBoxDebug.AppendText(System.Environment.NewLine);

                byte byteHeader = 0xFF;
                byte byteLVert = 0x80;
                byte byteLHorz = 0x80;
                byte byteRVert = 0x80;
                byte byteRHorz = 0x80;
                byte byteButton = 0x00;
                byte byteExt = 0x00;

                //if (latchButton == 0)
                //{
                //    byteButton = 0x00;
                //}
                //else if (latchButton == 1)
                //{
                    
                //    byteButton = 0x00;
                //    latchButton = 2;
                //}
                //else if (latchButton == 2)
                //{
                //    byteButton = 0x80;
                //    latchButton = 0;
                //}


                switch (cmdLeg)
                {
                    // Forward Case
                    case 0x05:
                        byteLVert = 0xFD;  //  253 decimal - see commander Protocol
                        byteLHorz = 0x80;  //  80 decimal  - neutral position
                        break;
                    // Reverse Case
                    case 0x06:
                        byteLVert = 0x03;  //  3 decimal   - see commander Protocol
                        byteLHorz = 0x80;  //  80 decimal  - neutral position
                        break;
                    // Turn Right Case
                    case 0x01:
                        byteLVert = 0x80;  //  3 decimal   - see commander Protocol
                        byteLHorz = 0xFD;  //  80 decimal  - neutral position
                        break;
                    // Turn Left Case
                    case 0x03:
                        byteLVert = 0x80;  //  3 decimal   - see commander Protocol
                        byteLHorz = 0x03;  //  80 decimal  - neutral position
                        break;
                    // Stop Case
                    case 0x07:
                        byteLVert = 0x80;  //  3 decimal   - see commander Protocol
                        byteLHorz = 0x80;  //  80 decimal  - neutral position
                        break;
                    // Strafe Left
                    case 0x04:
                        byteLVert = 0x80;  //  3 decimal   - see commander Protocol
                        byteLHorz = 0x03;  //  80 decimal  - neutral position
                        byteButton = 0x80; // to strafe left top butoon
                        //latchButton = 1;
                        break;
                    // Strafe right
                    case 0x02:
                        byteLVert = 0x80;  //  3 decimal   - see commander Protocol
                        byteLHorz = 0xFD;  //  80 decimal  - neutral position
                        byteButton = 0x80; // to strafe left top butoon
                        //qlatchButton = 1;
                        break;

                }
                // Calculate the checksum + display it
                //byte byteChecksum = (byte)(0xFF - (byte)(byteRVert + byteRHorz + byteLVert + byteLHorz + byteButton + byteExt) % 256);
                byte byteChecksum = (byte)~(byteRVert + byteRHorz + byteLVert + byteLHorz + byteButton + byteExt  + goalAzPosLow + goalAzPosHigh + goalElPosLow + goalElPosHigh + gunFire + goalAzSpeedLow + goalAzSpeedHigh + goalElSpeedLow + goalElSpeedHigh) ;

                textBoxFocus.AppendText(byteChecksum.ToString() + Environment.NewLine);

                //goalAzPosLow = 0; goalAzPosHigh = 0; goalElPosLow = 0; goalElPosHigh = 0;
                // make sure these bytes are are not 0xFF, if so change to 0xFE
                // this is because our start of message uses 0xFF
                if (goalAzPosLow == 0xFF) { goalAzPosLow = 0xFE; }
                if (goalElPosLow == 0xFF) { goalElPosLow = 0xFE; }
                byte[] cmdBytes = new byte[17] { byteHeader, byteRVert, byteRHorz, byteLVert, byteLHorz, byteButton, byteExt, goalAzPosLow, goalAzPosHigh, goalElPosLow, goalElPosHigh, gunFire, goalAzSpeedLow, goalAzSpeedHigh, goalElSpeedLow, goalElSpeedHigh, byteChecksum };
                //byte[] cmdBytes = new byte[8] { byteHeader, byteRVert, byteRHorz, byteLVert, byteLHorz, byteButton, byteExt, byteChecksum };
                //byte[] cmdBytes = new byte[9] { byteHeader, byteRVert, byteRHorz, byteLVert, byteLHorz, byteButton, byteExt, goalAzPosLow, byteChecksum };
                cmdLength = 17;
                if (serialPortMech.IsOpen)
                {
                    serialPortMech.Write(cmdBytes, 0, cmdLength);
                    //if (cmdLeg == 0x02 || cmdLeg == 0x04)
                    //{
                    //    byteButton = 0x00;
                    //    latchButton = 2;
                    //    serialPortMech.Write(cmdBytes, 0, cmdLength);
                    //}
                }

                
                gunFire = 0x00;
                panelGunOrientation.Refresh();

            }
            catch (Exception ex)
            {
                textBoxDebug.AppendText(ex.Message.ToString() + Environment.NewLine);
            }
        }

        private void trackBarElSpeed_ValueChanged(object sender, EventArgs e)
        {
            textBoxElSpeed.Text = trackBarElSpeed.Value.ToString();
            //mechCamera.Focus();
            pictureBoxMech.Focus();
        }

        private void trackBarAzPos_ValueChanged(object sender, EventArgs e)
        {
            textBoxAzPos.Text = trackBarAzPos.Value.ToString();
            updateTurret();
            //mechCamera.Focus();
            pictureBoxMech.Focus();
        }

        private void trackBarAzSpeed_ValueChanged(object sender, EventArgs e)
        {
            textBoxAzSpeed.Text = trackBarAzSpeed.Value.ToString();
            //mechCamera.Focus();
            pictureBoxMech.Focus();
        }

        private void buttonFire_Click(object sender, EventArgs e)
        {
            if (armed)
            {
                gunFire = 0x01;
                updateTurret();
            }
            //mechCamera.Focus();
            pictureBoxMech.Focus();
        }

        private void checkBoxMouseControl_CheckedChanged(object sender, EventArgs e)
        {
            pictureBoxMech.Focus();
            //pictureBoxMech.Focus();
        }

        private void FormPilot_MouseMove(object sender, MouseEventArgs mouseCurrent)
        {
            
        }

        private void FormPilot_MouseClick(object sender, MouseEventArgs e)
        {
            //if (checkBoxMouseControl.Checked)
            //{
            //    // if left mouse was clicked, fire
            //    if (e.Button == MouseButtons.Left)
            //    {
            //        gunFire = 0x01;
            //        updateTurret();
            //    }

            //}
            textBoxDebug.AppendText("pictureBoxMech_Click_1" + System.Environment.NewLine);
            
        }

        private void pictureBoxMech_MouseMove(object sender, MouseEventArgs mouseCurrent)
        {
            // if mouse control box is checked then move turret based on mouse movements
            if (checkBoxMouseControl.Checked)
            {
                // if mouse is moved, move the turret, ignore first move to get position start
                if (!mouseMoveInitFlag)
                {
                    mouseMoveInitFlag = true;
                    mousePrevX = mouseCurrent.X;
                    mousePrevY = mouseCurrent.Y;
                }
                else
                {
                    // for now just get sign ofmovement
                    
                    //int dirX = Math.Sign(mouseCurrent.X - mousePrevX);
                    //int dirY = Math.Sign(mouseCurrent.Y - mousePrevY);
                    //int newElPos = trackBarElPos.Value + dirY;
                    //int newAzPos = trackBarAzPos.Value + dirX;
                    //textBoxDebug.AppendText(mouseCurrent.X.ToString() + "," + mouseCurrent.Y.ToString() + "," + mousePrevX.ToString() + "," +
                    //mousePrevY.ToString() + "," + dirX.ToString() + "," + dirY.ToString() + "," + newElPos.ToString() + "," + newAzPos.ToString() + System.Environment.NewLine);
                    int newElPos = (int)((double)mouseCurrent.Y * scaleMouseHeight + (double)trackBarElPos.Minimum);
                    int newAzPos = (int)((double)mouseCurrent.X * scaleMouseWidth + (double)trackBarAzPos.Minimum);

                    //cap values
                    if (newElPos > trackBarElPos.Maximum)
                        newElPos = trackBarElPos.Maximum;
                    if (newAzPos > trackBarAzPos.Maximum)
                        newAzPos = trackBarAzPos.Maximum;
                    if (newElPos < trackBarElPos.Minimum)
                        newElPos = trackBarElPos.Minimum;
                    if (newAzPos < trackBarAzPos.Minimum)
                        newAzPos = trackBarAzPos.Minimum;

                    trackBarElPos.Value = newElPos;
                    trackBarAzPos.Value = newAzPos;
                    //mousePrevX = mouseCurrent.X;
                    //mousePrevY = mouseCurrent.Y;        



                    updateTurret();

                }

            }

        }

        private void pictureBoxMech_Click_1(object sender, EventArgs e)
        {
            pictureBoxMech.Focus();

        }

        private void FormPilot_KeyDown(object sender, KeyEventArgs e)
        {
            textBoxDebug.AppendText("FormPilot_KeyDown" + System.Environment.NewLine);

            if (e.KeyData == Keys.W)
            {
                cmdLeg = 0x05;
            }
            else if (e.KeyData == Keys.D)
            {
                cmdLeg = 0x01;
            }
            else if (e.KeyData == Keys.A)
            {
                cmdLeg = 0x03;
            }
            else if (e.KeyData == Keys.S)
            {
                cmdLeg = 0x06;
            }
            else if (e.KeyData == Keys.E)
            {
                cmdLeg = 0x02;
            }
            else if (e.KeyData == Keys.Q)
            {
                cmdLeg = 0x04;
            }
            else if (e.KeyData == Keys.X)
            {
                cmdLeg = 0x07;
            }
            // If ready to arm check box is checked and key P was pressed,
            // then arm the MechWarrior
            else if (e.KeyData == Keys.P)
            {
                if (checkBoxArm.Checked)
                {
                    armed = true;
                    labelArm.Text = "Armed";
                    labelArm.ForeColor = Color.Red;
                }
            }
            // DISARM MechWarrior if O key is pressed
            else if (e.KeyData == Keys.O)
            {
                armed = false;
                labelArm.Text = "Not Armed";
                labelArm.ForeColor = Color.Green;
                checkBoxArm.Checked = false;


            }
            else if (e.KeyData == Keys.Space)
            {
                if (checkBoxMouseControl.Checked)
                {
                    checkBoxMouseControl.Checked = false;
                }
                else
                {
                    checkBoxMouseControl.Checked = true;

                }
            }
            else if (e.KeyCode == Keys.L)
            {
                timerMatchClock.Start();
            }
            else if (e.KeyCode == Keys.Z)
            {
                zoomSpeed = !zoomSpeed;
            }
            else if (e.KeyCode == Keys.C)
            {
                centerFlag = true;
            }
            else
            {
                cmdLeg = 0x00;
            }
            updateTurret();
        }

        private void pictureBoxMech_KeyDown(object sender, KeyEventArgs e)
        {
            
            textBoxDebug.AppendText("pictureBoxMech_KeyDown" + System.Environment.NewLine);

        }

        private void textBoxDebug_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyData == Keys.W)
            //{
            //    cmdLeg = 0x02;
            //}
            //else if (e.KeyData == Keys.Space)
            //{
            //    cmdLeg = 0x01;
            //}

            //updateTurret();
            textBoxDebug.AppendText("textBoxDebug_KeyDown" + System.Environment.NewLine);
        }

        private void buttonFire_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyData == Keys.W)
            //{
            //    cmdLeg = 0x02;
            //}
            //else if (e.KeyData == Keys.Space)
            //{
            //    cmdLeg = 0x01;
            //}

            //updateTurret();
            textBoxDebug.AppendText("buttonFire_KeyDown" + System.Environment.NewLine);

        }

        private void trackBarElPos_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void trackBarElSpeed_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyData == Keys.W)
            //{
            //    cmdLeg = 0x02;
            //}
            //else if (e.KeyData == Keys.Space)
            //{
            //    cmdLeg = 0x01;
            //}

            //updateTurret();
            textBoxDebug.AppendText("trackBarElSpeed_KeyDown" + System.Environment.NewLine);
        }

        private void trackBarElPos_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyData == Keys.W)
            //{
            //    cmdLeg = 0x02;
            //}
            //else if (e.KeyData == Keys.Space)
            //{
            //    cmdLeg = 0x01;
            //}

            //updateTurret();
            textBoxDebug.AppendText("Fire trackBarElPos_KeyDown" + System.Environment.NewLine);
        }

        private void trackBarAzPos_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyData == Keys.W)
            //{
            //    cmdLeg = 0x02;
            //}
            //else if (e.KeyData == Keys.Space)
            //{
            //    cmdLeg = 0x01;
            //}

            //updateTurret();
            textBoxDebug.AppendText("Fire trackBarElPos_KeyDown" + System.Environment.NewLine);

        }

        private void trackBarAzSpeed_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyData == Keys.W)
            //{
            //    cmdLeg = 0x02;
            //}
            //else if (e.KeyData == Keys.S)
            //{
            //    cmdLeg = 0x01;
            //}

            //updateTurret();
            textBoxDebug.AppendText("Fire trackBarElPos_KeyDown" + System.Environment.NewLine);

        }

        private void pictureBoxMech_MouseDown(object sender, MouseEventArgs e)
        {

            textBoxDebug.AppendText("X Pos: " + e.X.ToString() + ", Y Pos: " + e.Y.ToString() + System.Environment.NewLine);

            if (e.Button ==  MouseButtons.Right)
            {
                pictureBoxMech.Focus();
                checkBoxMouseControl.Checked = true;
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (checkBoxMouseControl.Checked && armed)
                {

                    gunFire = 0x01;
                    updateTurret();

                }

            }

            textBoxDebug.AppendText("pictureBoxMech_MouseDown" + System.Environment.NewLine);
        }

        private void checkBoxArm_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBoxArm.Checked)
            {
                labelArm.Text = "READY TO ARM";
                labelArm.ForeColor = Color.MediumBlue;
                pictureBoxMech.Focus();
            }
            else
            {
                armed = false;
                labelArm.Text = "NOT ARMED";
                labelArm.ForeColor = Color.Green;
                pictureBoxMech.Focus();


            }


        }

        private void labelPKeyArm_Click(object sender, EventArgs e)
        {
            //pictureBoxMech.Focus();
        }

        private void labelArm_Click(object sender, EventArgs e)
        {
           // pictureBoxMech.Focus();
        }

        private void timerMatchClock_Tick(object sender, EventArgs e)
        {
            seconds++;
            // reset seconds and increment minutes
            if (seconds >= 60)
            {
                seconds = 0;
                minutes++;
            }
            labelSeconds.Text = seconds.ToString();
            labelMinutes.Text = minutes.ToString();

        }

        private void panelGunOrientation_Paint(object sender, PaintEventArgs e)
        {
            float cm2pixel = 450 / 311;
            float mechWidth = 53.4f;    // 36.83 cm
            float mechHalfWidth = 26.7f;
            const int center = 225;   // middle of panel
            const int offsetIRX = 16;   // offset from center of mech body to IR in pixels, 11 cm
            const int offsetIRY = 7;    // offset from center of mech body to IR in pixels, 11 cm
            const int boxIRwidth = 4;
            const float offsetAZ = 150.0f;  //   AX-12 offset in degrees, ie. if AZ motor is at 90 deg, than it is forward facing to mech body
            const int offsetBearing = 180;  // offset in degrees from North to Local arean reference
            const float offsetRservo = -20.0f; // 90.0f;
            const float offsetLservo = 90.0f; // -75.0f;

            // draw gun orientation to panel
            Graphics grfx = panelGunOrientation.CreateGraphics();
            Pen greenPen = new Pen(Color.Green);
            Pen bluePen = new Pen(Color.Blue);
            Pen redPen = new Pen(Color.Red);
            
            redPen.Width = 5;
            bluePen.Width = 5;
            greenPen.Width = 5;
            Pen blackPen = new Pen(Color.Black);
            int xp2 = center;
            int yp2 = center; // -(int)mechHalfWidth;
            const float lineLength = 90.0f;  // lenth in pixels of how long gun viz is
            float bearingLineLength = mechHalfWidth;
            
            const float bit2deg = 1024.0f / 300.0f;
            float theta = trackBarAzPos.Value / bit2deg;  // azimuth angle in degrees
            //float phi = theta - 150;
            float phi = theta -offsetAZ;
            // convert to radians
            phi *= deg2rad;
            float x1 = lineLength * (float) Math.Sin(phi);
            float y1 = lineLength * (float) Math.Cos(phi);
            float xp1 = x1 + xp2;
            float yp1 = yp2 - y1;

            // draw bearing - consider moving to draw on video also
            float phiBearing = (float)bearing - offsetBearing;


            // Set world transform of graphics object to translate.
            grfx.TranslateTransform(center,center);

            // Then to rotate, prepending rotation matrix.
            grfx.RotateTransform(phiBearing);

            // Set world transform of graphics object to translate.
            grfx.TranslateTransform(-center, -center);

            // draw mech body as rectangle
            grfx.DrawRectangle(blackPen, center - (int)mechHalfWidth, center - (int)mechHalfWidth, (int)mechWidth,(int) mechWidth);

            // draw gun barrel, update based on orientation
            grfx.DrawLine(redPen,xp1,yp1,xp2,yp2);

            // draw 25 cm ring around mech body for detecting stuff around
            float ringDetect = 25.0f * cm2pixel * 4.0f;
            grfx.DrawEllipse(blackPen, center - ringDetect / 2, center - ringDetect/2, ringDetect, ringDetect);

            phiBearing *= deg2rad;   //convert to radians
            x1 = 0.0f; //*(float)Math.Sin(phiBearing);
            y1 = bearingLineLength;// *(float)Math.Cos(phiBearing);
            xp1 = x1 + center;
            yp1 = center - y1;
            grfx.DrawLine(bluePen, xp1, yp1, center, center);

            for (int i = 0; i < bufferRightIR.Length; i++)
            {
                // draw right sweeping sensor
                float cmServiRightIRTemp = (float)bufferRightIR[i];
                int servoPosRightTemp = bufferRightIRPos[i];
                if (cmServiRightIRTemp < 150.0 && cmServiRightIRTemp > 0.0)// && servoPosRightTemp < 110 && servoPosRightTemp > 40)
                {

                    float servoRphi = (float)servoPosRightTemp - offsetRservo;
                    servoRphi *= deg2rad;  //convert to rad
                    //x1 = (float)cmServiRightIRTemp * cm2pixel * (float)Math.Sin(servoRphi);
                    //y1 = (float)cmServiRightIRTemp * cm2pixel * (float)Math.Cos(servoRphi);
                    //float x2 = x1 * (float)Math.Cos(-bufferPhi[i]) - y1 * (float)Math.Sin(-bufferPhi[i]);
                    //float y2 = x1 * (float)Math.Sin(-bufferPhi[i]) + y1 * (float)Math.Cos(-bufferPhi[i]);

                    //x1 = (float)cmServiRightIRTemp * cm2pixel * (float)Math.Cos(bufferPhi[i] + servoRphi);
                    //y1 = (float)cmServiRightIRTemp * cm2pixel * (float)Math.Sin(bufferPhi[i] + servoRphi);
                    
                    x1 = (float)(cmServiRightIRTemp * cm2pixel + mechHalfWidth) * (float)Math.Cos(servoRphi - 90 * deg2rad - bufferPhi[i]);
                    y1 = (float)(cmServiRightIRTemp * cm2pixel  + mechHalfWidth ) * (float)Math.Sin(servoRphi - 90 * deg2rad - bufferPhi[i]);

                    xp1 = x1 + center;// +mechHalfWidth; // +6;
                    yp1 = center - y1;// -mechHalfWidth;
                    //yp1 = center - y2 - 5 * cm2pixel;
                    blackPen.Width = 5;

                    // save coordinates into buffer
                    grfx.DrawEllipse(blackPen, xp1, yp1, 2.0f, 2.0f);

                    
                }

                // draw left sweeping sensor
                float cmServoLeftIRTemp = (float)bufferLeftIR[i];
                int servoPosLeftTemp = bufferLeftIRPos[i];
                if (cmServoLeftIRTemp < 150.0 && cmServoLeftIRTemp > 0.0)// && servoPosLeftTemp < 170 && servoPosLeftTemp > 100)
                {

                    float servoLphi = (float)servoPosLeftTemp - offsetLservo;
                    servoLphi *= deg2rad;  //convert to rad
                    //x1 = (float)cmServoLeftIRTemp * cm2pixel * (float)Math.Sin(servoLphi);
                    //y1 = (float)cmServoLeftIRTemp * cm2pixel * (float)Math.Cos(servoLphi);
                    //float x2L = x1 * (float)Math.Cos(bufferPhi[i]) - y1 * (float)Math.Sin(bufferPhi[i]);
                    //float y2L = x1 * (float)Math.Sin(bufferPhi[i]) + y1 * (float)Math.Cos(bufferPhi[i]);

                    //x1 = (float)cmServoLeftIRTemp * cm2pixel * (float)Math.Cos(bufferPhi[i] + servoLphi);
                    //y1 = (float)cmServoLeftIRTemp * cm2pixel * (float)Math.Sin(bufferPhi[i] + servoLphi);

                    x1 = (float)(cmServoLeftIRTemp * cm2pixel + mechHalfWidth) * (float)Math.Cos(servoLphi + 180 * deg2rad - bufferPhi[i]);
                    y1 = (float)(cmServoLeftIRTemp * cm2pixel + mechHalfWidth) * (float)Math.Sin(servoLphi + 180 * deg2rad - bufferPhi[i]);

                    //xp1 = x2L - center - 6;
                    //yp1 = center - y2L - 5 * cm2pixel;
                    xp1 = x1 + center;// -mechHalfWidth;
                    yp1 = center - y1;// -mechHalfWidth;
                    //blackPen.Width = 5;

                    // save coordinates into buffer
                    grfx.DrawEllipse(greenPen, xp1, yp1, 2.0f, 2.0f);


                }
            }


            


            // draw right IR sesnor reading, red, blue, green based on distance
            if(cmRightIR < 30.0  && cmRightIR > 0.0) {
                grfx.DrawRectangle(redPen, center + offsetIRX + (int)(cmRightIR * cm2pixel), center - offsetIRY, boxIRwidth, boxIRwidth);
            }
            else if (cmRightIR < 48.0 && cmRightIR > 30.0)
            {
                grfx.DrawRectangle(bluePen, center + offsetIRX + (int)(cmRightIR * cm2pixel), center - offsetIRY, boxIRwidth, boxIRwidth);
            }
            else if (cmRightIR < 150 && cmRightIR > 48.0)
            {
                grfx.DrawRectangle(greenPen, center + offsetIRX + (int)(cmRightIR * cm2pixel), center - offsetIRY, boxIRwidth, boxIRwidth);
            }

            // draw left IR sesnor reading, red, blue, green based on distance
            if (cmLeftIR < 30.0 && cmLeftIR > 0.0)
            {
                grfx.DrawRectangle(redPen, center - offsetIRX - (int)(cmLeftIR * cm2pixel), center - offsetIRY, boxIRwidth, boxIRwidth);
            }
            else if (cmLeftIR < 48.0 && cmLeftIR > 30.0)
            {
                grfx.DrawRectangle(bluePen, center - offsetIRX - (int)(cmLeftIR * cm2pixel), center - offsetIRY, boxIRwidth, boxIRwidth);
            }
            else if (cmLeftIR < 150.0 && cmLeftIR > 48.0)
            {
                grfx.DrawRectangle(greenPen, center - offsetIRX - (int)(cmLeftIR * cm2pixel), center - offsetIRY, boxIRwidth, boxIRwidth);
            }

            // draw front IR sesnor reading, red, blue, green based on distance
            if (cmFrontIR < 30.0 && cmFrontIR > 0.0)
            {
                grfx.DrawRectangle(redPen, center + offsetIRY, center - offsetIRX - (int)(cmFrontIR * cm2pixel), boxIRwidth, boxIRwidth);
            }
            else if (cmFrontIR < 48.0 && cmFrontIR > 30.0)
            {
                grfx.DrawRectangle(bluePen, center + offsetIRY, center - offsetIRX - (int)(cmFrontIR * cm2pixel), boxIRwidth, boxIRwidth);
            }
            else if (cmFrontIR < 150.0 && cmFrontIR > 48.0)
            {
                grfx.DrawRectangle(greenPen, center + offsetIRY, center - offsetIRX - (int)(cmFrontIR * cm2pixel), boxIRwidth, boxIRwidth);
            }

            // draw back IR sesnor reading, red, blue, green based on distance
            if (cmBackIR < 30.0 && cmBackIR > 0.0)
            {
                grfx.DrawRectangle(redPen, center + offsetIRY, center + offsetIRX + (int)(cmBackIR * cm2pixel), boxIRwidth, boxIRwidth);
            }
            else if (cmBackIR < 48.0 && cmBackIR > 30.0)
            {
                grfx.DrawRectangle(bluePen, center + offsetIRY, center + offsetIRX + (int)(cmBackIR * cm2pixel), boxIRwidth, boxIRwidth);
            }
            else if (cmBackIR < 150.0 && cmBackIR > 48.0)
            {
                grfx.DrawRectangle(greenPen, center + offsetIRY, center + offsetIRX + (int)(cmBackIR * cm2pixel), boxIRwidth, boxIRwidth);
            }

            // draw Servo Right IR sesnor reading, red, blue, green based on distance
            //if (cmServiRightIR < 30.0 && cmServiRightIR > 0.0)
            //{
            //    grfx.DrawRectangle(redPen, center + offsetIRY, center + offsetIRX + (int)(cmServiRightIR * cm2pixel), boxIRwidth, boxIRwidth);
            //}l
            //else if (cmServiRightIR < 48.0 && cmServiRightIR > 30.0)
            //{
            //    grfx.DrawRectangle(yellowPen, center + offsetIRY, center + offsetIRX + (int)(cmServiRightIR * cm2pixel), boxIRwidth, boxIRwidth);
            //}
            //else if (cmServiRightIR < 150.0 && cmServiRightIR > 48.0)
            //{
            //    grfx.DrawRectangle(greenPen, center + offsetIRY, center + offsetIRX + (int)(cmServiRightIR * cm2pixel), boxIRwidth, boxIRwidth);
            //}
            

            

            //   draw last target hit
            if (labelTargetPlate.Text == "3")
            {
                // front
                grfx.DrawLine(redPen, 0, 2, 450, 2);
            }
            else if (labelTargetPlate.Text == "4")
            {
                // right
                grfx.DrawLine(redPen, 448, 0, 448, 450);
            }
            else if (labelTargetPlate.Text == "2")
            {
                // left
                grfx.DrawLine(redPen, 2, 0, 2, 450);
            }
            else if (labelTargetPlate.Text == "1")
            {
                // back
                grfx.DrawLine(redPen, 0, 448, 450, 448);
            }


            redPen.Dispose();
            grfx.Dispose();
            
        }
        private void SetLabel(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.labelHitPoints.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetLabel);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.labelHitPoints.Text = text;
            }
        }

        private void SetLabelBearing(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.labelBearingInt.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetLabelBearing);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.labelBearingInt.Text = text;
            }
        }

        private void SetLabelLeftIR(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.labelLeftIR.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetLabelLeftIR);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.labelLeftIR.Text = text;
            }
        }

        private void SetLabelRightIR(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.labelRightIR.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetLabelRightIR);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.labelRightIR.Text = text;
            }
        }

        private void SetLabelFrontIR(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.labelFrontIR.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetLabelFrontIR);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.labelFrontIR.Text = text;
            }
        }

        private void SetLabelBackIR(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.labelBackIR.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetLabelBackIR);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.labelBackIR.Text = text;
            }
        }

        private void SetLabelServoRIR(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.labelServoR.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetLabelServoRIR);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.labelServoR.Text = text;
            }
        }

        private void SetLabelServoLIR(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.labelServoL.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetLabelServoLIR);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.labelServoL.Text = text;
            }
        }

        private void SetLabelServoRPosIR(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.labelServoRPos.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetLabelServoRPosIR);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.labelServoRPos.Text = text;
            }
        }

        private void SetLabelServoLPosIR(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.labelServoLPos.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetLabelServoLPosIR);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.labelServoLPos.Text = text;
            }
        }

        private void SetLabelTargetPlate(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.labelTargetPlate.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetLabelTargetPlate);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.labelTargetPlate.Text = text;
            }
        }

        private void SetLabelChecksum(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBoxFocus.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetLabelChecksum);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBoxFocus.AppendText("Return Check: " + text + System.Environment.NewLine) ;
            }
        }


        private void updateData()
        {
            //labelBearingInt.Text = String.Format("{0:f0}", bearing);
            //labelLeftIR.Text = String.Format("{0:f0}", cmLeftIR);
            //labelRightIR.Text = String.Format("{0:f0}", cmRightIR);
            //labelFrontIR.Text = String.Format("{0:f0}", cmFrontIR);
            //labelBackIR.Text = String.Format("{0:f0}", cmBackIR);
            //labelServoR.Text = String.Format("{0:f0}", cmServiRightIR);
            //labelServoRPos.Text = servoPosRight.ToString();
            //labelHitPoints.Text = hitPoints.ToString();
            //labelTargetPlate.Text = targetPlate.ToString();

            SetLabel(hitPoints.ToString());
            SetLabelBearing(String.Format("{0:f0}", bearing));
            SetLabelLeftIR(String.Format("{0:f0}", cmLeftIR));
            SetLabelRightIR(String.Format("{0:f0}", cmRightIR));
            SetLabelFrontIR(String.Format("{0:f0}", cmFrontIR));
            SetLabelBackIR(String.Format("{0:f0}", cmBackIR));
            SetLabelServoRIR(String.Format("{0:f0}", cmServiRightIR));
            SetLabelServoRPosIR(servoPosRight.ToString());
            SetLabelServoLIR(String.Format("{0:f0}", cmServoLeftIR));
            SetLabelServoLPosIR(servoPosLeft.ToString());
            SetLabelTargetPlate(targetPlate.ToString());
            SetLabelChecksum(returnCheckSum.ToString());

            

        }
        double gp2d12_range(short value)
        {

            if (value < 3)
                return -1;  //invalid value

            return (6787.0 / ((double)value - 3.0)) - 4.0;
        }

        private void serialPortMech_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            // read from bot
            try
            {
                if (serialPortMech.BytesToRead > 22) // if new data is available
                {
                    byte[] recBytes = new byte[23];
                    serialPortMech.Read(recBytes, 0, 23);
                    //for (int k = 0; k < 12; k++)
                    //{
                    //    textBoxDebug.AppendText(recBytes[k].ToString());
                    //}
                    if (recBytes[0] == '*' && recBytes[1] == '*')
                    {
                        hitPoints   = recBytes[2];
                        targetPlate = recBytes[3];

                        byte hbyte,lbyte;
                        hbyte = recBytes[4];
                        lbyte = recBytes[5];
                           
                        int bearingInt = ((hbyte<<8)+lbyte)/10;               // Calculate full bearing
                        int bearingDec = ((hbyte << 8) + lbyte) % 10;         // Calculate decimal place of bearing
                        bearing = bearingInt + bearingDec;

                        byte[] bytesLeftIR = new byte[2];
                        bytesLeftIR[0] = recBytes[8];   // low byte of left analog Sharp IR sensor reading
                        bytesLeftIR[1] = recBytes[9];    // high byte of left analog Sharp IR sensor reading

                        byte[] bytesRightIR = new byte[2];
                        bytesRightIR[0] = recBytes[10];  // low byte of right analog Sharp IR sensor reading
                        bytesRightIR[1] = recBytes[11];  // high byte of right analog Sharp IR sensor reading

                        byte[] bytesFrontIR = new byte[2];
                        bytesFrontIR[0] = recBytes[12];   // low byte of front analog Sharp IR sensor reading
                        bytesFrontIR[1] = recBytes[13];    // high byte of front analog Sharp IR sensor reading

                        byte[] bytesBackIR = new byte[2];
                        bytesBackIR[0] = recBytes[14];   // low byte of front analog Sharp IR sensor reading
                        bytesBackIR[1] = recBytes[15];    // high byte of front analog Sharp IR sensor reading

                        byte[] bytesServoRightIR = new byte[2];
                        bytesServoRightIR[0] = recBytes[16];   // low byte of front analog Sharp IR sensor reading
                        bytesServoRightIR[1] = recBytes[17];    // high byte of front analog Sharp IR sensor reading

                        servoPosRight = recBytes[18];

                        byte[] bytesServoLeftIR = new byte[2];
                        bytesServoLeftIR[0] = recBytes[19];    // low byte of front analog Sharp IR sensor reading
                        bytesServoLeftIR[1] = recBytes[20];    // high byte of front analog Sharp IR sensor reading

                        servoPosLeft = recBytes[21];
                        returnCheckSum = recBytes[22];

                        short leftIRInt  = BitConverter.ToInt16(bytesLeftIR,0);  // convert from 2 bytes to int
                        short rightIRInt = BitConverter.ToInt16(bytesRightIR, 0); // convert from 2 bytes to int
                        short frontIRInt = BitConverter.ToInt16(bytesFrontIR, 0); // convert from 2 bytes to int
                        short backIRInt  = BitConverter.ToInt16(bytesBackIR, 0); // convert from 2 bytes to int

                        short servoRightIRInt = BitConverter.ToInt16(bytesServoRightIR, 0); // convert from 2 bytes to int
                        short servoLeftIRInt = BitConverter.ToInt16(bytesServoLeftIR, 0); // convert from 2 bytes to int

                        double VOLTS_PER_UNIT = 0.0049;

                        double voltsLeft  = (double)leftIRInt  * VOLTS_PER_UNIT;
                        double voltsRight = (double)rightIRInt * VOLTS_PER_UNIT;
                        //double voltsFront = (double)frontIRInt * VOLTS_PER_UNIT;
                        double voltsBack  = (double)backIRInt  * VOLTS_PER_UNIT;

                        double voltsServoRight = (double)servoRightIRInt * VOLTS_PER_UNIT;
                        double voltsServoLeft = (double)servoLeftIRInt * VOLTS_PER_UNIT;

                        cmLeftIR  = 60.495 * Math.Pow(voltsLeft, -1.1904);
                        cmRightIR = 60.495 * Math.Pow(voltsRight, -1.1904);
                        //cmFrontIR = 60.495 * Math.Pow(voltsFront, -1.1904);
                        cmFrontIR = gp2d12_range(frontIRInt);
                        cmBackIR  = 60.495 * Math.Pow(voltsBack, -1.1904);

                        cmServiRightIR = 60.495 * Math.Pow(voltsServoRight, -1.1904);
                        cmServoLeftIR = 60.495 * Math.Pow(voltsServoLeft, -1.1904);


                        bufferRightIR[idxBufferRight] = cmServiRightIR;
                        bufferLeftIR[idxBufferRight] = cmServoLeftIR;
                        bufferRightIRPos[idxBufferRight] = (int)servoPosRight;
                        bufferLeftIRPos[idxBufferRight] = (int)servoPosLeft;
                        const float bit2deg = 1024.0f / 300.0f;
                        const float offsetAZ = 150.0f;
                        float theta = currentAz / bit2deg;  // azimuth angle in degrees
                        bufferPhi[idxBufferRight] = (theta - offsetAZ)*deg2rad;
                        idxBufferRight++;
                        if(idxBufferRight >= bufferSize)
                        {
                            idxBufferRight = 0;
                        }
                        
                    }
                    updateData();
                    serialPortMech.DiscardInBuffer();
                    
                }
                
                //string strTargetBoards = serialPortMech.ReadLine();
               // string strHitPanel = serialPortMech.ReadLine();

                //if (strHitpoints != null)
                //{
                //    hitPoints = int.Parse(strHitpoints);
                //    labelHitPoints.Text = hitPoints.ToString();     
                //}

                //if (strHitPanel != null)
                //{
                //    hitPoints = int.Parse(strHitPanel);
                //    labelTargetPlate.Text = hitPoints.ToString();
                //}
            }
            catch (Exception ex)
            {
                textBoxDebug.AppendText(ex.Message.ToString() + Environment.NewLine);
            }

            //updateData();
            panelGunOrientation.Invalidate();
           
        }
    }
}
