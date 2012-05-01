using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using FormVLC.LibVlc;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Input = Microsoft.Xna.Framework.Input; // to provide shorthand to clear up ambiguities
namespace FormVLC
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct libvlc_exception_t
    {
        public int b_raised;
        public int i_code;
        [MarshalAs(UnmanagedType.LPStr)]
        public string psz_message;
    }

    

    public partial class FormVLC : Form
    {

        protected IntPtr instance = IntPtr.Zero;
        IntPtr player = IntPtr.Zero;
        IntPtr m_hMedia = IntPtr.Zero;
        IntPtr m_mp = IntPtr.Zero;
        String m_strVlcInstallDir;
        protected IntPtr m_hMediaPlayer;

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

        private void InitializeSerialPort()
        {
            try
            {
                // query the os for all the com ports and populate the combo box
                string [] ports = System.IO.Ports.SerialPort.GetPortNames();
                comboBoxSerial.Items.AddRange(ports);
                
                // Here is a hard-coded serial port, if it exists go ahead
                // and select it and open serial port, feature so user does not have 
                // to select it every time
                var comCheckExist = "COM5";
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
                lblNotConnected.Text = ex.Message.ToString();
            }
        }

        //static void Raise(ref libvlc_exception_t ex)
        //{
        //    if (LibVlc.libvlc_exception_raised(ref ex) != 0)
        //        MessageBox.Show(LibVlc.libvlc_exception_get_message(ref ex));
       // }
        public FormVLC()
        {
            InitializeComponent();
            InitializeSerialPort();
           // libvlc_exception_t ex = new libvlc_exception_t();
            //LibVlc.libvlc_exception_init(ref ex);
            m_strVlcInstallDir = QueryVlcInstallPath();

//            string[] args = new string[] {
 //                           "-I", 
  //              "dummy",  
//		        "--ignore-config", 
 //               "--no-osd",
 //               "--disable-screensaver",
               // "--ffmpeg-hw",/\
//		        "--plugin-path=" + m_strVlcInstallDir
                //"arena.mp4"
        
  //    };

            string[] args = new string[] 
             {
                "-I", 
                "dummy",
                //"http",  
		        //"--ignore-config", 
                //"--no-osd",
                //"--disable-screensaver",
                "--ffmpeg-hw",
                "--ffmpeg-fast",
		        "--plugin-path=./plugins", //,
                //"--rotate-angle=90",
                "--http-reconnect",
                //"--vout-filter=transform",
                //"--transform-type=90",
                //"--grayscale",
                "--video-on-top",
                "--width=1024",
                "--height=768",
                "--video-x=0",
                "--video-y=0",
                "--clock-synchro=0",
                "--video-filter=rotate{angle=90}",
                //"--video-filter=invert",
                //"--video-filter=edge{type=0}",
                //" --swscale-mode=0",
                //"--transform-type=vflip",
                "--high-priority",
                "--mjpeg-fps=30.0",
                "--network-caching=15"
                //"--ipv4"
             };
            
            instance = LibVlc.libvlc_new(args.Length, args);
            m_hMediaPlayer = LibVlc.libvlc_media_player_new(instance);
            LibVlc.libvlc_media_player_set_hwnd(m_hMediaPlayer, panelVLC.Handle);
            //String m_path = "arena.mp4";
            String m_path = "http://192.168.0.56:8080/videofeed";
            byte[] bytes = Encoding.UTF8.GetBytes(m_path);
            m_hMedia = LibVlc.libvlc_media_new_path(instance, bytes);
            m_mp = LibVlc.libvlc_media_player_new_from_media(m_hMedia);
            LibVlc.libvlc_media_player_play(m_mp);
           // instance = LibVlc.libvlc_new(0, null);
           // Raise(ref ex);

            //m_strVlcInstallDir = QueryVlcInstallPath();
            //m_iVlcHandle = VLC_Create();
            //VLC_Destroy(m_iVlcHandle);
            UISync.Init(this);
        }

        private string QueryVlcInstallPath()
        {
            // open registry
            RegistryKey regkeyVlcInstallPathKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\VideoLAN\VLC");
            if (regkeyVlcInstallPathKey == null)
                return "";
            return (string)regkeyVlcInstallPathKey.GetValue("InstallDir", "");
        }

        public enum libvlc_event_e
        {
            libvlc_MediaMetaChanged = 0,
            libvlc_MediaSubItemAdded,
            libvlc_MediaDurationChanged,
            libvlc_MediaParsedChanged,
            libvlc_MediaFreed,
            libvlc_MediaStateChanged,

            libvlc_MediaPlayerMediaChanged = 0x100,
            libvlc_MediaPlayerNothingSpecial,
            libvlc_MediaPlayerOpening,
            libvlc_MediaPlayerBuffering,
            libvlc_MediaPlayerPlaying,
            libvlc_MediaPlayerPaused,
            libvlc_MediaPlayerStopped,
            libvlc_MediaPlayerForward,
            libvlc_MediaPlayerBackward,
            libvlc_MediaPlayerEndReached,
            libvlc_MediaPlayerEncounteredError,
            libvlc_MediaPlayerTimeChanged,
            libvlc_MediaPlayerPositionChanged,
            libvlc_MediaPlayerSeekableChanged,
            libvlc_MediaPlayerPausableChanged,
            libvlc_MediaPlayerTitleChanged,
            libvlc_MediaPlayerSnapshotTaken,
            libvlc_MediaPlayerLengthChanged,

            libvlc_MediaListItemAdded = 0x200,
            libvlc_MediaListWillAddItem,
            libvlc_MediaListItemDeleted,
            libvlc_MediaListWillDeleteItem,

            libvlc_MediaListViewItemAdded = 0x300,
            libvlc_MediaListViewWillAddItem,
            libvlc_MediaListViewItemDeleted,
            libvlc_MediaListViewWillDeleteItem,

            libvlc_MediaListPlayerPlayed = 0x400,
            libvlc_MediaListPlayerNextItemSet,
            libvlc_MediaListPlayerStopped,

            libvlc_MediaDiscovererStarted = 0x500,
            libvlc_MediaDiscovererEnded,

            libvlc_VlmMediaAdded = 0x600,
            libvlc_VlmMediaRemoved,
            libvlc_VlmMediaChanged,
            libvlc_VlmMediaInstanceStarted,
            libvlc_VlmMediaInstanceStopped,
            libvlc_VlmMediaInstanceStatusInit,
            libvlc_VlmMediaInstanceStatusOpening,
            libvlc_VlmMediaInstanceStatusPlaying,
            libvlc_VlmMediaInstanceStatusPause,
            libvlc_VlmMediaInstanceStatusEnd,
            libvlc_VlmMediaInstanceStatusError,
        }

        static class LibVlc
        {
            #region core
            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern string libvlc_errmsg();

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern void libvlc_clearerr();

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr libvlc_new(int argc, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] argv);

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern void libvlc_release(IntPtr libvlc_instance_t);

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern void libvlc_retain(IntPtr p_instance);

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern int libvlc_add_intf(IntPtr p_instance, [MarshalAs(UnmanagedType.LPArray)] byte[] name);

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern void libvlc_set_exit_handler(IntPtr p_instance, IntPtr callback, IntPtr opaque);

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern void libvlc_wait(IntPtr p_instance);

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern void libvlc_set_user_agent(IntPtr p_instance, [MarshalAs(UnmanagedType.LPArray)] byte[] name, [MarshalAs(UnmanagedType.LPArray)] byte[] http);

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern string libvlc_get_version();

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern int libvlc_event_attach(IntPtr p_event_manager, libvlc_event_e i_event_type, IntPtr f_callback, IntPtr user_data);

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern void libvlc_event_detach(IntPtr p_event_manager, libvlc_event_e i_event_type, IntPtr f_callback, IntPtr user_data);

            //[DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            //[return: MarshalAs(UnmanagedType.AnsiBStr)]
            //public static extern string libvlc_event_type_name(libvlc_event_e event_type);

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern UInt32 libvlc_get_log_verbosity(IntPtr libvlc_instance_t);

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern void libvlc_set_log_verbosity(IntPtr libvlc_instance_t, UInt32 level);

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr libvlc_log_open(IntPtr libvlc_instance_t);

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern void libvlc_log_close(IntPtr libvlc_log_t);

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern UInt32 libvlc_log_count(IntPtr libvlc_log_t);

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern void libvlc_log_clear(IntPtr libvlc_log_t);

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr libvlc_log_get_iterator(IntPtr libvlc_log_t);

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern void libvlc_log_iterator_free(IntPtr libvlc_log_iterator_t);

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern Int32 libvlc_log_iterator_has_next(IntPtr libvlc_log_iterator_t);

           // [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
           // public static extern IntPtr libvlc_log_iterator_next(IntPtr libvlc_log_iterator_t, ref libvlc_log_message_t p_buffer);

            //[MinimalLibVlcVersion("1.2.0")]
            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr libvlc_audio_filter_list_get(IntPtr p_instance);

            //[MinimalLibVlcVersion("1.2.0")]
            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr libvlc_video_filter_list_get(IntPtr p_instance);

            //[MinimalLibVlcVersion("1.2.0")]
            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern void libvlc_module_description_list_release(IntPtr p_list);

            //[MinimalLibVlcVersion("1.2.0")]
            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern Int64 libvlc_clock();

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern void libvlc_media_player_set_media(IntPtr libvlc_media_player_t, IntPtr libvlc_media_t);

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern void libvlc_media_player_play(IntPtr libvlc_mediaplayer);

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr libvlc_media_player_new(IntPtr p_libvlc_instance);

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern void libvlc_media_player_set_hwnd(IntPtr libvlc_mediaplayer, IntPtr libvlc_drawable);

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr libvlc_media_new_path(IntPtr p_instance, [MarshalAs(UnmanagedType.LPArray)] byte[] psz_mrl);

            [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr libvlc_media_player_new_from_media(IntPtr libvlc_media);

            #endregion
        }

        private void FormVLC_FormClosing(object sender, FormClosingEventArgs e)
        {
            LibVlc.libvlc_release(instance);
            this.StopAllVibration();
            //this.Close();
        }

        private class UISync
        {
            private static ISynchronizeInvoke Sync;

            public static void Init(ISynchronizeInvoke sync)
            {
                Sync = sync;
            }

            public static void Execute(Action action)
            {
                Sync.BeginInvoke(action, null);
            }
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
                this.buttonA.Checked = (this.gamePadState.Buttons.A == Input.ButtonState.Pressed);
                this.buttonB.Checked = (this.gamePadState.Buttons.B == Input.ButtonState.Pressed);
                this.buttonX.Checked = (this.gamePadState.Buttons.X == Input.ButtonState.Pressed);
                this.buttonY.Checked = (this.gamePadState.Buttons.Y == Input.ButtonState.Pressed);
                this.buttonLeftShoulder.Checked = (this.gamePadState.Buttons.LeftShoulder == Input.ButtonState.Pressed);
                this.buttonRightShoulder.Checked = (this.gamePadState.Buttons.RightShoulder == Input.ButtonState.Pressed);
                this.buttonStart.Checked = (this.gamePadState.Buttons.Start == Input.ButtonState.Pressed);
                this.buttonBack.Checked = (this.gamePadState.Buttons.Back == Input.ButtonState.Pressed);
                this.buttonLeftStick.Checked = (this.gamePadState.Buttons.LeftStick == Input.ButtonState.Pressed);
                this.buttonRightStick.Checked = (this.gamePadState.Buttons.RightStick == Input.ButtonState.Pressed);
            }
            if (!this.gamePadState.DPad.Equals(this.previousState.DPad))
            {
                this.buttonUp.Checked = (this.gamePadState.DPad.Up == Input.ButtonState.Pressed);
                this.buttonDown.Checked = (this.gamePadState.DPad.Down == Input.ButtonState.Pressed);
                this.buttonLeft.Checked = (this.gamePadState.DPad.Left == Input.ButtonState.Pressed);
                this.buttonRight.Checked = (this.gamePadState.DPad.Right == Input.ButtonState.Pressed);
            }

            //Update the position of the thumb sticks
            //since the thumbsticks can return a number between -1.0 and +1.0 I had to shift (add 1.0)
            //and scale (mutiplication by 100/2, or 50) to get the numbers to be in the range of 0-100
            //for the progress bar
            this.x1Position.Value = (int)((this.gamePadState.ThumbSticks.Left.X + 1.0f) * 100.0f / 2.0f);
            this.y1Position.Value = (int)((this.gamePadState.ThumbSticks.Left.Y + 1.0f) * 100.0f / 2.0f);
            this.x2position.Value = (int)((this.gamePadState.ThumbSticks.Right.X + 1.0f) * 100.0f / 2.0f);
            this.y2position.Value = (int)((this.gamePadState.ThumbSticks.Right.Y + 1.0f) * 100.0f / 2.0f);

            //The triggers return a value between 0.0 and 1.0.  I only needed to scale these values for
            //the progress bar
            this.leftTriggerPosition.Value = (int)((this.gamePadState.Triggers.Left * 100));
            this.rightTriggerPosition.Value = (int)(this.gamePadState.Triggers.Right * 100);

            // send data states to mech over serial
            sendMechCommands();

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

                short goalElPos = (short)map((int)this.y2position.Value, 0, 100, 750, 545); //( trackBarElPos.Maximum - trackBarElPos.Value + trackBarElPos.Minimum ) ;
                short goalElSpeed = (short)50;  //trackBarElSpeed.Value;
                //short goalAzPos = (short)512;  //(trackBarAzPos.Maximum - trackBarAzPos.Value + trackBarAzPos.Minimum);
                // convert the joystick 0,100 values horz. into the full range of the azimuth Ax-12 servo
                short goalAzPos = (short)map((int)this.x2position.Value, 0, 100, 1023, 0);
                short goalAzSpeed = (short)100; //trackBarAzSpeed.Value;
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

                byteLVert = (byte)map((int)this.y1Position.Value, 0, 100, 0, 253);
                byteLHorz = (byte)map((int)this.x1Position.Value, 0, 100, 0, 253);
                if (this.buttonLeftShoulder.Checked)
                {
                    byteButton = 0x80;
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
                lblNotConnected.Text = ex.Message.ToString();
            }

        }
        private void timerInput_Tick(object sender, EventArgs e)
        {
            this.UpdateControllerState();
            this.CheckVibrationTimeout();
            // Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            //    this.Dispose();
        }

        

        private void FormVLC_Load(object sender, EventArgs e)
        {
            this.ddlController.SelectedIndex = 0;
            this.timerInput.Start();
        }
        private void CheckVibrationTimeout()
        {
            if (vibrationCountdown > 0)
            {
                --vibrationCountdown;
                if (vibrationCountdown == 0.0f)
                {
                    GamePad.SetVibration(playerIndex, 0.0f, 0.0f);
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            GamePad.SetVibration(playerIndex, (float)this.leftMotor.Value, (float)this.rightMotor.Value);
            vibrationCountdown = 30;
            

        }

        private void StopAllVibration()
        {
            GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
            GamePad.SetVibration(PlayerIndex.Two, 0.0f, 0.0f);
            GamePad.SetVibration(PlayerIndex.Three, 0.0f, 0.0f);
            GamePad.SetVibration(PlayerIndex.Four, 0.0f, 0.0f);

        }

        /// <summary>
        /// When a new controller is selected from the drop down
        /// update the player index and turn off all the vibration motors. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddlController_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.ddlController.SelectedIndex)
            {
                case 0: playerIndex = PlayerIndex.One; break;
                case 1: playerIndex = PlayerIndex.Two; break;
                case 2: playerIndex = PlayerIndex.Three; break;
                case 3: playerIndex = PlayerIndex.Four; break;
                default: playerIndex = PlayerIndex.One; break;
            }
            this.StopAllVibration();

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
                lblNotConnected.Text = ex.Message.ToString();
                //textBoxDebug.AppendText(ex.Message.ToString() + Environment.NewLine);
            }

        }
    }

}
