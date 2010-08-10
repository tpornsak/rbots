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
    public partial class FormTrendnet : Form
    {
        // statistics length
        private const int statLength = 15;
        // current statistics index
        private int statIndex = 0;
        // ready statistics values
        private int statReady = 0;
        // statistics array
        private int[] statCount = new int[statLength];

        string cameraURL = "http://192.168.2.16/cgi/mjpg/mjpeg.cgi";
        public FormTrendnet()
        {
            InitializeComponent();
            InitializeCamera();
        }

        private void InitializeCamera()
        {
            // create video source
            MJPEGStream mjpegSource = new MJPEGStream(cameraURL);
            textBoxDebug.AppendText("Camera URL" + System.Environment.NewLine);
            textBoxDebug.AppendText(cameraURL + System.Environment.NewLine);
            mjpegSource.Login = "bloftin";
            mjpegSource.Password = "r4d4r4";
            // open it
            OpenVideo(mjpegSource);
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
            if (mechCamera.VideoSource != null)
            {
                mechCamera.SignalToStop();
                mechCamera.WaitForStop();
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
    }
}
