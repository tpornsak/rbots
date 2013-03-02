using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Input = Microsoft.Xna.Framework.Input; // to provide shorthand to clear up ambiguities

using System.Threading;
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

        // variabes for the xbox controller
        float prevXpos = 0;
        float thumstickRight;
        float prevYpos = 0;
        float thumstickUp;
        int x1Position;
        int y1Position;
        int x2position;
        int y2position;
        int leftTriggerPosition;
        int rightTriggerPosition;
        bool buttonUp;
        bool buttonDown;
        bool buttonLeft;
        bool buttonRight;
        bool buttonA;
        bool buttonB;
        bool buttonX;
        bool buttonY;
        bool buttonLeftShoulder;
        bool buttonRightShoulder;
        bool buttonStart;
        bool buttonBack;
        bool buttonLeftStick;
        bool buttonRightStick;



        //To keep track of the current and previous state of the gamepad
        /// <summary>
        /// The current state of the controller
        /// </summary>
        GamePadState gamePadState;
        /// <summary>
        /// The previous state of the controller
        /// </summary>
        GamePadState previousState;

        /// <summary>
        /// Keeps track of the current controller
        /// </summary>
        PlayerIndex playerIndex = PlayerIndex.One;

        /// <summary>
        /// Counter for limiting the time for which the vibration motors are on.
        /// </summary>
        int vibrationCountdown = 0;

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
        //string cameraURL = "http://192.168.0.53:8080/videofeed";
        //string cameraURL = "http://192.168.0.102:8080/ipcam";
        Thread t;

        public FormPilot()
        {
            
            InitializeComponent();           
            //InitializeCamera();
            InitializeSerialPort();
            scaleMouseHeight = (double)(trackBarElPos.Maximum - trackBarElPos.Minimum) / webVideo.Height;
            scaleMouseWidth = (double)(trackBarAzPos.Maximum - trackBarAzPos.Minimum) / webVideo.Width;

            for (int i = 0; i < bufferRightIR.Length; i++)
            {
                bufferRightIR[i] = 0.0;
                bufferRightIRPos[i] = 120;

                bufferLeftIR[i] = 0.0;
                bufferLeftIRPos[i] = 0;

                bufferPhi[i] = 0;
            }

           
        }

        // Re-maps a number from one range to another. That is, a value of fromLow would get mapped to toLow, 
        // a value of fromHigh to toHigh, values in-between to values in-between, etc.
        // Does not constrain values to within the range, because out-of-range values are sometimes intended and useful. 
        // The constrain() function may be used either before or after this function, if limits to the ranges are desired.
        // Note that the "lower bounds" of either range may be larger or smaller than the "upper bounds" so the 
        // map() function may be used to reverse a range of numbers, for example
        // y = map(x, 1, 50, 50, 1);
        // The function also handles negative numbers well, so that this example
        // y = map(x, 1, 50, 50, -100);
        // is also valid and works well.
        //The map() function uses integer math so will not generate fractions, when the math might indicate
        // that it should do so. Fractional remainders are truncated, and are not rounded or averaged.
        //
        //Parameters
        // value: the number to map
        // fromLow: the lower bound of the value's current range
        // fromHigh: the upper bound of the value's current range
        // toLow: the lower bound of the value's target range
        // toHigh: the upper bound of the value's target range
        //
        // Returns
        // The mapped value.
        private int map(int x, int in_min, int in_max, int out_min, int out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        private float map(float x, float in_min, float in_max, float out_min, float out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }


        private void sendMechCommands()
        {
            try
            {
                //if (zoomSpeed)
                //{
                //    trackBarAzSpeed.Value = 50;
                //}
                //else
                //{
                //    trackBarAzSpeed.Value = 150;
                //}

                //short goalElPos = (short)map((int)this.y2position.Value, 0, 100, 750, 545); //( trackBarElPos.Maximum - trackBarElPos.Value + trackBarElPos.Minimum ) ;
                short goalElSpeed = (short)150;  //trackBarElSpeed.Value;
                //short goalAzPos = (short)512;  //(trackBarAzPos.Maximum - trackBarAzPos.Value + trackBarAzPos.Minimum);
                // convert the joystick 0,100 values horz. into the full range of the azimuth Ax-12 servo

                //joystick gains
                float kx = 5.0f;
                float ky = 5.0f;

                // joystick control code.
                float currX = prevXpos + kx * (float)Math.Pow((double)thumstickRight, 3.0); // x^3
                float currY = prevYpos + ky * (float)Math.Pow((double)thumstickUp, 3.0);  // x^3
                prevXpos = currX;
                prevYpos = currY;
                short goalAzPos = (short)map(currX, -100.0f, 100.0f, 1023.0f, 0.0f);
                //short goalAzPos = (short)map((int)this.x2position.Value, 0, 100, 1023, 0);
                short goalElPos = (short)map(currY, -100.0f, 100.0f, 750.0f, 545.0f);

                short goalAzSpeed = (short)150; //trackBarAzSpeed.Value;
                byte goalElPosLow = (byte)(goalElPos & 0xff);
                byte goalElPosHigh = (byte)(goalElPos >> 8);
                byte goalAzPosLow = (byte)(goalAzPos & 0xff);
                byte goalAzPosHigh = (byte)(goalAzPos >> 8);

                //currentAz = trackBarAzPos.Value; 
                byte goalElSpeedLow = (byte)(goalElSpeed & 0xff);
                byte goalElSpeedHigh = (byte)(goalElSpeed >> 8);
                byte goalAzSpeedLow = (byte)(goalAzSpeed & 0xff);
                byte goalAzSpeedHigh = (byte)(goalAzSpeed >> 8);


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

                byte cmdLeg = 0x07;
                byte gunFire = 0x00;
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

                byteLVert = (byte)map((int)this.y1Position, 0, 100, 0, 253);
                byteLHorz = (byte)map((int)this.x1Position, 0, 100, 0, 253);
                if (this.buttonLeftShoulder)
                {
                    byteButton = 0x80;
                }

                if (this.rightTriggerPosition > 0.0)
                {
                    gunFire = 0x01;
                }
                // Calculate the checksum + display it
                //byte byteChecksum = (byte)(0xFF - (byte)(byteRVert + byteRHorz + byteLVert + byteLHorz + byteButton + byteExt) % 256);
                byte byteChecksum = (byte)~(byteRVert + byteRHorz + byteLVert + byteLHorz + byteButton + byteExt + goalAzPosLow + goalAzPosHigh + goalElPosLow + goalElPosHigh + gunFire + goalAzSpeedLow + goalAzSpeedHigh + goalElSpeedLow + goalElSpeedHigh);

                //textBoxFocus.AppendText(byteChecksum.ToString() + Environment.NewLine);

                //goalAzPosLow = 0; goalAzPosHigh = 0; goalElPosLow = 0; goalElPosHigh = 0;
                // make sure these bytes are are not 0xFF, if so change to 0xFE
                // this is because our start of message uses 0xFF
                if (goalAzPosLow == 0xFF) { goalAzPosLow = 0xFE; }
                if (goalElPosLow == 0xFF) { goalElPosLow = 0xFE; }
                byte[] cmdBytes = new byte[17] { byteHeader, byteRVert, byteRHorz, byteLVert, byteLHorz, byteButton, byteExt, goalAzPosLow, goalAzPosHigh, goalElPosLow, goalElPosHigh, gunFire, goalAzSpeedLow, goalAzSpeedHigh, goalElSpeedLow, goalElSpeedHigh, byteChecksum };
                //byte[] cmdBytes = new byte[8] { byteHeader, byteRVert, byteRHorz, byteLVert, byteLHorz, byteButton, byteExt, byteChecksum };
                //byte[] cmdBytes = new byte[9] { byteHeader, byteRVert, byteRHorz, byteLVert, byteLHorz, byteButton, byteExt, goalAzPosLow, byteChecksum };
                int cmdLength = 17;
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


                //gunFire = 0x00;
                //panelGunOrientation.Refresh();

            }
            catch (Exception ex)
            {
                textBoxDebug.AppendText(ex.Message.ToString() + Environment.NewLine);
            }

        }


        private void InitializeSerialPort()
        {
            try
            {

                //    serialPortMech.Open();
                // query the os for all the com ports and populate the combo box
                string [] ports = System.IO.Ports.SerialPort.GetPortNames();
                comboBoxSerial.Items.AddRange(ports);
                
                // Here is a hard-coded serial port, if it exists go ahead
                // and select it and open serial port, feature so user does not have 
                // to select it every time
                var comCheckExist = "COM99";
                //var results = Array.FindAll(ports, s => s.Equals(comCheckExist));
                int idx = comboBoxSerial.FindString(comCheckExist);

                // if we have found our default serial port, go ahead and open it
                if (idx > 0 )
                {
                    serialPortMech.PortName = comCheckExist;
                    serialPortMech.Open();
                    comboBoxSerial.SelectedIndex = idx;
                }
            }
            catch (Exception ex)
            {
                textBoxDebug.AppendText(ex.Message.ToString() + Environment.NewLine);
           }
        }
        private void InitializeCamera()
        {

        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void UIUpdate()
        {
           // textBox1.Text = serialData;
        }

        //private void MonitorPort()
        //{
        //    try
        //    {
        //        //All this thread does is write the serial data to the UI.
        //        //This simple example is not written to prevent data loss!
        //        while (portOpen)
        //        {
        //            this.Invoke(new UIUpdater(UIUpdate));
        //            Thread.Sleep(100);
        //        }
        //    }
        //    finally
        //    {
        //        //Signal that the thread is terminating
        //        are.Set();
        //    }
        //}

        private void FormTrendnet_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (serialPortMech.IsOpen)
                {
                    //serialPortMech.Close();
                    //Tell MonitorPort() to stop updating the UI
                    portOpen = false;

                    //Wait for MonitorPort() to signal that it's done
                    //are.WaitOne();

                    serialPortMech.Close();
                }
            }
            catch (Exception ex)
            {
                textBoxDebug.AppendText(ex.Message.ToString() + System.Environment.NewLine);
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            textBoxElPos.Text = trackBarElPos.Value.ToString();
            //updateTurret();
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
            //pictureBoxMech.Focus();
        }

        private void trackBarAzPos_ValueChanged(object sender, EventArgs e)
        {
            textBoxAzPos.Text = trackBarAzPos.Value.ToString();
            //updateTurret();
            //mechCamera.Focus();
           // pictureBoxMech.Focus();
        }

        private void trackBarAzSpeed_ValueChanged(object sender, EventArgs e)
        {
            textBoxAzSpeed.Text = trackBarAzSpeed.Value.ToString();
            //mechCamera.Focus();
            //pictureBoxMech.Focus();
        }

        private void buttonFire_Click(object sender, EventArgs e)
        {
            if (armed)
            {
                gunFire = 0x01;
                //updateTurret();
            }
            //mechCamera.Focus();
           // pictureBoxMech.Focus();
        }

        private void checkBoxMouseControl_CheckedChanged(object sender, EventArgs e)
        {
            //pictureBoxMech.Focus();
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
        private void UpdateControllerState()
        {
            //Get the new gamepad state and save the old state.
            this.previousState = this.gamePadState;
            this.gamePadState = GamePad.GetState(this.playerIndex);
            //If the controller is not connected, let the user know
            //this.lblNotConnected.Visible = !this.gamePadState.IsConnected;
            //I personally prefer to only update the buttons if their state has been changed. 
            if (!this.gamePadState.Buttons.Equals(this.previousState.Buttons))
            {
                this.buttonA = (this.gamePadState.Buttons.A == Input.ButtonState.Pressed);
                this.buttonB = (this.gamePadState.Buttons.B == Input.ButtonState.Pressed);
                this.buttonX = (this.gamePadState.Buttons.X == Input.ButtonState.Pressed);
                this.buttonY = (this.gamePadState.Buttons.Y == Input.ButtonState.Pressed);
                this.buttonLeftShoulder = (this.gamePadState.Buttons.LeftShoulder == Input.ButtonState.Pressed);
                this.buttonRightShoulder = (this.gamePadState.Buttons.RightShoulder == Input.ButtonState.Pressed);
                this.buttonStart = (this.gamePadState.Buttons.Start == Input.ButtonState.Pressed);
                this.buttonBack = (this.gamePadState.Buttons.Back == Input.ButtonState.Pressed);
                this.buttonLeftStick = (this.gamePadState.Buttons.LeftStick == Input.ButtonState.Pressed);
                this.buttonRightStick = (this.gamePadState.Buttons.RightStick == Input.ButtonState.Pressed);
            }
            if (!this.gamePadState.DPad.Equals(this.previousState.DPad))
            {
                this.buttonUp = (this.gamePadState.DPad.Up == Input.ButtonState.Pressed);
                this.buttonDown = (this.gamePadState.DPad.Down == Input.ButtonState.Pressed);
                this.buttonLeft = (this.gamePadState.DPad.Left == Input.ButtonState.Pressed);
                this.buttonRight = (this.gamePadState.DPad.Right == Input.ButtonState.Pressed);
            }

            //Update the position of the thumb sticks
            //since the thumbsticks can return a number between -1.0 and +1.0 I had to shift (add 1.0)
            //and scale (mutiplication by 100/2, or 50) to get the numbers to be in the range of 0-100
            //for the progress bar
            this.x1Position = (int)((this.gamePadState.ThumbSticks.Left.X + 1.0f) * 100.0f / 2.0f);
            this.y1Position = (int)((this.gamePadState.ThumbSticks.Left.Y + 1.0f) * 100.0f / 2.0f);
            this.x2position = (int)((this.gamePadState.ThumbSticks.Right.X + 1.0f) * 100.0f / 2.0f);
            this.y2position = (int)((this.gamePadState.ThumbSticks.Right.Y + 1.0f) * 100.0f / 2.0f);
            thumstickRight = this.gamePadState.ThumbSticks.Right.X;
            thumstickUp = this.gamePadState.ThumbSticks.Right.Y;

            //The triggers return a value between 0.0 and 1.0.  I only needed to scale these values for
            //the progress bar
            this.leftTriggerPosition = (int)((this.gamePadState.Triggers.Left * 100));
            this.rightTriggerPosition = (int)(this.gamePadState.Triggers.Right * 100);

            // send data states to mech over serial
            sendMechCommands();

        }

        private void FormPilot_KeyDown(object sender, KeyEventArgs e)
        {
            textBoxDebug.AppendText("FormPilot_KeyDown" + System.Environment.NewLine);

            if (e.KeyData == System.Windows.Forms.Keys.W)
            {
                cmdLeg = 0x05;
            }
            else if (e.KeyData == System.Windows.Forms.Keys.D)
            {
                cmdLeg = 0x01;
            }
            else if (e.KeyData == System.Windows.Forms.Keys.A)
            {
                cmdLeg = 0x03;
            }
            else if (e.KeyData == System.Windows.Forms.Keys.S)
            {
                cmdLeg = 0x06;
            }
            else if (e.KeyData == System.Windows.Forms.Keys.E)
            {
                cmdLeg = 0x02;
            }
            else if (e.KeyData == System.Windows.Forms.Keys.Q)
            {
                cmdLeg = 0x04;
            }
            else if (e.KeyData == System.Windows.Forms.Keys.X)
            {
                cmdLeg = 0x07;
            }
            // If ready to arm check box is checked and key P was pressed,
            // then arm the MechWarrior
            else if (e.KeyData == System.Windows.Forms.Keys.P)
            {
                if (checkBoxArm.Checked)
                {
                    armed = true;
                    labelArm.Text = "Armed";
                    labelArm.ForeColor = System.Drawing.Color.Red;
                }
            }
            // DISARM MechWarrior if O key is pressed
            else if (e.KeyData == System.Windows.Forms.Keys.O)
            {
                armed = false;
                labelArm.Text = "Not Armed";
                labelArm.ForeColor = System.Drawing.Color.Green;
                checkBoxArm.Checked = false;


            }
            else if (e.KeyData == System.Windows.Forms.Keys.Space)
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
            else if (e.KeyCode == System.Windows.Forms.Keys.L)
            {
                timerMatchClock.Start();
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.Z)
            {
                zoomSpeed = !zoomSpeed;
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.C)
            {
                centerFlag = true;
            }
            else
            {
                cmdLeg = 0x00;
            }
            //updateTurret();
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


        private void checkBoxArm_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBoxArm.Checked)
            {
                labelArm.Text = "READY TO ARM";
                labelArm.ForeColor = System.Drawing.Color.MediumBlue;
                //pictureBoxMech.Focus();
            }
            else
            {
                armed = false;
                labelArm.Text = "NOT ARMED";
                labelArm.ForeColor = System.Drawing.Color.Green;
               // pictureBoxMech.Focus();


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
            Pen greenPen = new Pen(System.Drawing.Color.Green);
            Pen bluePen = new Pen(System.Drawing.Color.Blue);
            Pen redPen = new Pen(System.Drawing.Color.Red);
            
            redPen.Width = 5;
            bluePen.Width = 5;
            greenPen.Width = 5;
            Pen blackPen = new Pen(System.Drawing.Color.Black);
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

        private void comboBoxSerial_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (serialPortMech.IsOpen)
                {
                    // 1st close the serial port if it is open
                    serialPortMech.Close();
                }

                // grab the currently selected com port from the combo box and open it
                serialPortMech.PortName = (string)comboBoxSerial.SelectedItem;
                serialPortMech.Open();
            }
            catch (Exception ex)
            {
                textBoxDebug.AppendText(ex.Message.ToString() + Environment.NewLine);
            }
        }

        private void timerInput_Tick(object sender, EventArgs e)
        {
            this.UpdateControllerState();
        }

        private void FormPilot_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Graphics grfx = this.CreateGraphics();
            Pen redPen = new Pen(System.Drawing.Color.Red);
            grfx.DrawLine(redPen, 0, 0, 448, 450);
        }
    }
}
