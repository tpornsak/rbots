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
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Imaging.Textures;


namespace CameraTrendnetAForge
{
    public partial class FormPilot : Form
    {

        // create filter
        Mirror filterMirror = new Mirror(false,true);

        // hitpoints
        private int hitPoints = 20;

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

        // stop iscmdLeg = 0x01, forwad is cmdLeg = 0x02, 
        private byte cmdLeg = 0x00;

        // length of command
        private const int cmdLength = 12;
        private int mousePrevX = 0;
        private int mousePrevY = 0;
        private bool mouseMoveInitFlag = false;
        private bool armed = false;

        private double scaleMouseHeight = 0;
        private double scaleMouseWidth = 0;
        

        // statistics array
        private int[] statCount = new int[statLength];
        string cameraURL = "http://192.168.1.86/img/video.mjpeg";
        //string cameraURL = "http://192.168.2.175/img/video.mjpeg";
        
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
                    //mechCamera.WaitForStop();
                    //mechCamera.Stop();
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
            filterMirror.ApplyInPlace(image);
            Graphics g = Graphics.FromImage(image);

            // draw cross-hairs
            Pen pR = new Pen(Color.Red);

            if (!armed)
            {
                Font armFont = new Font("arial",16.0f);
                Color clr = Color.Red;
                SolidBrush br = new SolidBrush(clr);
                

                g.DrawString("NOT ARMED", armFont, br, 10.0f/4, 240.0f/4);
            }
            
            //g.DrawLine(pR, 137.0f, 120.0f, 173.0f, 120.0f);
            //g.DrawLine(pR, 150.0f, 107.0f, 150.0f, 133.0f);
            g.DrawRectangle(pR, 366.0f/4, 257.0f/4 , 20.0f/2, 20.0f/2);

            
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

            textBoxFocus.AppendText(this.ActiveControl.ToString() + Environment.NewLine);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            updateTurret();
            mechCamera.Focus();
        }

        private void updateTurret()
        {
            try
            {
                textBoxElPos.Text = trackBarElPos.Value.ToString();
                textBoxElSpeed.Text = trackBarElSpeed.Value.ToString();
                textBoxAzPos.Text = trackBarAzPos.Value.ToString();
                textBoxAzSpeed.Text = trackBarAzSpeed.Value.ToString();
                //short goalElPos = (short)(1023 * trackBarElPos.Value / 300);
                //short goalElSpeed = (short)(1023 * trackBarElSpeed.Value / 114);
                //short goalAzPos = (short)(1023 * trackBarAzPos.Value / 300);
                //short goalAzSpeed = (short)(1023 * trackBarAzSpeed.Value / 114);
                short goalElPos = (short) ( trackBarElPos.Maximum - trackBarElPos.Value + trackBarElPos.Minimum ) ;
                short goalElSpeed = (short)trackBarElSpeed.Value;
                short goalAzPos = (short)(trackBarAzPos.Maximum - trackBarAzPos.Value + trackBarAzPos.Minimum);
               // short goalAzPos = (short)(trackBarAzPos.Value);
                short goalAzSpeed = (short)trackBarAzSpeed.Value;
                byte goalElPosLow = (byte)(goalElPos & 0xff);
                byte goalElPosHigh = (byte)(goalElPos >> 8);
                byte goalAzPosLow = (byte)(goalAzPos & 0xff);
                byte goalAzPosHigh = (byte)(goalAzPos >> 8);
                byte goalElSpeedLow = (byte)(goalElSpeed & 0xff);
                byte goalElSpeedHigh = (byte)(goalElSpeed >> 8);
                byte goalAzSpeedLow = (byte)(goalAzSpeed & 0xff);
                byte goalAzSpeedHigh = (byte)(goalAzSpeed >> 8);

                byte[] cmdBytes = new byte[cmdLength] { 0x2A, cmdLeg, 0x00, goalElPosLow, goalElPosHigh, goalElSpeedLow, goalElSpeedHigh,
                                             goalAzPosLow, goalAzPosHigh, goalAzSpeedLow, goalAzSpeedHigh,  gunFire};
                // reset gun to zero
                gunFire = 0x00;

                // reset legcmd to zero
                cmdLeg = 0x00;

                if (serialPortMech.IsOpen)
                {
                    serialPortMech.Write(cmdBytes, 0, cmdLength);
                }


                // textBoxDebug.AppendText(cmdBytes[0].ToString());

                // for(int i=1;i < cmdLength;i++)
                //     textBoxDebug.AppendText("," + cmdBytes[i].ToString());
                //  textBoxDebug.AppendText(System.Environment.NewLine);

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
            mechCamera.Focus();
        }

        private void trackBarAzPos_ValueChanged(object sender, EventArgs e)
        {
            updateTurret();
            mechCamera.Focus();
        }

        private void trackBarAzSpeed_ValueChanged(object sender, EventArgs e)
        {
            textBoxAzSpeed.Text = trackBarAzSpeed.Value.ToString();
            mechCamera.Focus();
        }

        private void buttonFire_Click(object sender, EventArgs e)
        {
            if (armed)
            {
                gunFire = 0x01;
                updateTurret();
            }
            mechCamera.Focus();
        }

        private void checkBoxMouseControl_CheckedChanged(object sender, EventArgs e)
        {
            mechCamera.Focus();
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
            textBoxDebug.AppendText("mechCamera_Click_1" + System.Environment.NewLine);
            
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
            mechCamera.Focus();
   
        }

        private void FormPilot_KeyDown(object sender, KeyEventArgs e)
        {
            textBoxDebug.AppendText("FormPilot_KeyDown" + System.Environment.NewLine);
        }

        private void mechCamera_KeyDown(object sender, KeyEventArgs e)
        {
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
             else
             {
                 cmdLeg = 0x00;
             }
            updateTurret();
            textBoxDebug.AppendText("mechCamera_KeyDown" + System.Environment.NewLine);

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

        private void mechCamera_MouseDown(object sender, MouseEventArgs e)
        {

            textBoxDebug.AppendText("X Pos: " + e.X.ToString() + ", Y Pos: " + e.Y.ToString() + System.Environment.NewLine);

            if (e.Button ==  MouseButtons.Right)
            {
                mechCamera.Focus();
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

            textBoxDebug.AppendText("mechCamera_MouseDown" + System.Environment.NewLine);
        }

        private void checkBoxArm_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBoxArm.Checked)
            {
                labelArm.Text = "READY TO ARM";
                labelArm.ForeColor = Color.MediumBlue;
                mechCamera.Focus();
            }
            else
            {
                armed = false;
                labelArm.Text = "NOT ARMED";
                labelArm.ForeColor = Color.Green;
                mechCamera.Focus();


            }


        }

        private void labelPKeyArm_Click(object sender, EventArgs e)
        {
            mechCamera.Focus();
        }

        private void labelArm_Click(object sender, EventArgs e)
        {
            mechCamera.Focus();
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
            // draw gun orientation to panel
            Graphics grfx = panelGunOrientation.CreateGraphics();
            Pen redPen = new Pen(Color.Red);
            redPen.Width = 5;
            Pen blackPen = new Pen(Color.Black);
            const int xp2 = 95;
            const int yp2 = 170;
            const float lineLength = 90.0f;
            
            const float bit2deg = 1024.0f / 300.0f;
            float theta = trackBarAzPos.Value / bit2deg;  // azimuth angle in degrees
            float phi = theta - 150;
            // convert to radians
            phi *= deg2rad;
            float x1 = lineLength * (float)Math.Sin(phi);
            float y1 = lineLength * (float) Math.Cos(phi);
            float xp1 = x1 + xp2;
            float yp1 = yp2 - y1;

            grfx.DrawRectangle(blackPen, 50, 150, 100, 40);

            // draw gun barrel, update based on orientation
            grfx.DrawLine(redPen,xp1,yp1,xp2,yp2);

            // draw last target hit
            if (labelTargetPlate.Text == "3")
            {
                // front
                grfx.DrawLine(redPen, 0, 2, 300, 2);
            }
            else if (labelTargetPlate.Text == "4")
            {
                // right
                grfx.DrawLine(redPen, 298, 0, 298, 300);
            }
            else if (labelTargetPlate.Text == "2")
            {
                // left
                grfx.DrawLine(redPen, 2, 0, 2, 300);
            }
            else if (labelTargetPlate.Text == "1")
            {
                // back
                grfx.DrawLine(redPen, 0, 298, 300, 298);
            }


            redPen.Dispose();
            grfx.Dispose();
            
        }

        private void serialPortMech_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            // read from bot
            try
            {
                if (serialPortMech.BytesToRead > 1)
                {
                    byte[] recBytes = new byte[2];
                    serialPortMech.Read(recBytes, 0, 2);

                    labelHitPoints.Text = recBytes[0].ToString();
                    labelTargetPlate.Text = recBytes[1].ToString();
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
           
        }
    }
}
