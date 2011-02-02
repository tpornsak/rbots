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


namespace CameraTrendnetAForge
{
    public partial class FormPilot : Form
    {
        // statistics length
        private const int statLength = 15;
        // current statistics index
        private int statIndex = 0;
        // ready statistics values
        private int statReady = 0;

        // fire gun variable, 0: No fire, 1: Fire
        private byte gunFire = 0x00;

        // length of command
        private const int cmdLength = 12;
        private int mousePrevX = 0;
        private int mousePrevY = 0;
        private bool mouseMoveInitFlag = false;

        private double scaleMouseHeight = 0;
        private double scaleMouseWidth = 0;
        

        // statistics array
        private int[] statCount = new int[statLength];

        string cameraURL = "http://192.168.2.107/img/video.mjpeg";
        public FormPilot()
        {
            
            InitializeComponent();
            InitializeCamera();
            InitializeSerialPort();
            scaleMouseHeight = (double)(trackBarElPos.Maximum - trackBarElPos.Minimum) / mechCamera.Height;
            scaleMouseWidth = (double)(trackBarAzPos.Maximum - trackBarAzPos.Minimum) / mechCamera.Width;
        }

        private void InitializeSerialPort()
        {
            try
            {
                serialPortMech.Open();
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
                //mjpegSource.Login = "ben";
                //mjpegSource.Password = "r4d4r4";
               // mjpegSource.RequestTimeout = 2000;

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

        private void mechCamera_Click(object sender, EventArgs e)
        {

        }

        private void FormTrendnet_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (mechCamera.VideoSource != null)
                {
                   mechCamera.SignalToStop();
                   mechCamera.WaitForStop();
                }
                if (serialPortMech.IsOpen)
                {
                    serialPortMech.Close();
                }
            }
            catch (Exception ex)
            {
                textBoxDebug.AppendText(ex.Message.ToString() + System.Environment.NewLine);
            }
        }

        private void mechCamera_NewFrame(object sender, ref Bitmap image)
        {
            DateTime now = DateTime.Now;
            Graphics g = Graphics.FromImage(image);

            // paint current time
            SolidBrush brush = new SolidBrush(Color.Red);
            g.DrawString(now.ToString(), this.Font, brush, new PointF(5, 5));

            // draw cross-hairs
            Pen pR = new Pen(Color.Red);
            
            g.DrawLine(pR, 275.0f, 240.0f, 325.0f, 240.0f);
            g.DrawLine(pR, 300.0f, 215.0f, 300.0f, 265.0f);


            // must dispose !
            brush.Dispose();
            pR.Dispose();

            g.Dispose();
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
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            updateTurret();   
        }

        private void updateTurret()
        {
            textBoxElPos.Text = trackBarElPos.Value.ToString();
            textBoxElSpeed.Text = trackBarElSpeed.Value.ToString();
            textBoxAzPos.Text = trackBarAzPos.Value.ToString();
            textBoxAzSpeed.Text = trackBarAzSpeed.Value.ToString();
            //short goalElPos = (short)(1023 * trackBarElPos.Value / 300);
            //short goalElSpeed = (short)(1023 * trackBarElSpeed.Value / 114);
            //short goalAzPos = (short)(1023 * trackBarAzPos.Value / 300);
            //short goalAzSpeed = (short)(1023 * trackBarAzSpeed.Value / 114);
            short goalElPos = (short) trackBarElPos.Value;
            short goalElSpeed = (short) trackBarElSpeed.Value;
            short goalAzPos = (short) trackBarAzPos.Value;
            short goalAzSpeed = (short) trackBarAzSpeed.Value;
            byte goalElPosLow = (byte)(goalElPos & 0xff);
            byte goalElPosHigh = (byte)(goalElPos >> 8);
            byte goalAzPosLow = (byte)(goalAzPos & 0xff);
            byte goalAzPosHigh = (byte)(goalAzPos >> 8);
            byte goalElSpeedLow = (byte)(goalElSpeed & 0xff);
            byte goalElSpeedHigh = (byte)(goalElSpeed >> 8);
            byte goalAzSpeedLow = (byte)(goalAzSpeed & 0xff);
            byte goalAzSpeedHigh = (byte)(goalAzSpeed >> 8);

            byte[] cmdBytes = new byte[cmdLength] { 0x2A, 0x00, 0x00, goalElPosLow, goalElPosHigh, goalElSpeedLow, goalElSpeedHigh,
                                             goalAzPosLow, goalAzPosHigh, goalAzSpeedLow, goalAzSpeedHigh,  gunFire};
            // reset gun to zero
            gunFire = 0x00;

            if (serialPortMech.IsOpen)
            {
                serialPortMech.Write(cmdBytes, 0, cmdLength);
            }


           // textBoxDebug.AppendText(cmdBytes[0].ToString());

           // for(int i=1;i < cmdLength;i++)
           //     textBoxDebug.AppendText("," + cmdBytes[i].ToString());
           //  textBoxDebug.AppendText(System.Environment.NewLine);
            

        }

        private void trackBarElSpeed_ValueChanged(object sender, EventArgs e)
        {
            textBoxElSpeed.Text = trackBarElSpeed.Value.ToString();   
        }

        private void trackBarAzPos_ValueChanged(object sender, EventArgs e)
        {
            updateTurret(); 
        }

        private void trackBarAzSpeed_ValueChanged(object sender, EventArgs e)
        {
            textBoxAzSpeed.Text = trackBarAzSpeed.Value.ToString();
        }

        private void buttonFire_Click(object sender, EventArgs e)
        {
            gunFire = 0x01;
            updateTurret();
        }

        private void checkBoxMouseControl_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void FormPilot_MouseMove(object sender, MouseEventArgs mouseCurrent)
        {
            
        }

        private void FormPilot_MouseClick(object sender, MouseEventArgs e)
        {
            if (checkBoxMouseControl.Checked)
            {
                // if left mouse was clicked, fire
                if (e.Button == MouseButtons.Left)
                {
                    gunFire = 0x01;
                    updateTurret();
                }

            }
            
        }

        private void mechCamera_MouseMove(object sender, MouseEventArgs mouseCurrent)
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

        private void mechCamera_Click_1(object sender, EventArgs e)
        {
   

        }
    }
}
