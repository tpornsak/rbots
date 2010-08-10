using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GyroRealTime
{
    public partial class FormGyroRealTime : Form
    {
        ASCIIEncoding ascii;
        const int rawGridMaxX = 800;
        const int spaceX = 5;
        const int rawGridMaxY = 480;
        const int rawGridMinX = 200;
        const int rawGridMinY = 80;
        const int analogRange = 1024;
        int currentX = 0;
        float[] dashValuesGrid = { 10, 10};
        const int bufferSizeX = (rawGridMaxX-rawGridMinX)/spaceX;
        int[] bufferX = new int[bufferSizeX];
        int currentBuffer = 0;
        int rawGyroValue = 0;


        public FormGyroRealTime()
        {
            InitializeComponent();
            InitializeSerial();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

            
        }

        private void InitializeSerial()
        {


            try
            {
                // The encoding, ardunio uses ASCII encoding in serial comm
                ascii = new ASCIIEncoding();

                serialPortArduino.Open();
                serialPortArduino.Encoding = ascii;

            }
            catch (Exception ex)
            {
                if (serialPortArduino.IsOpen)
                {
                    serialPortArduino.Close();
                }

                textBoxDebug.AppendText(ex.Message.ToString());
            }
        }

        public void RefreshPlot()
        {
            this.Refresh();
        }

        private void serialPortArduino_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                string str = serialPortArduino.ReadLine();
                
                try
                {
                    if (str != null)
                    {
                        rawGyroValue = int.Parse(str);
                    }
                }
                catch (Exception ex)
                {
                    rawGyroValue = 0;
                }
                bufferX[currentBuffer] = rawGyroValue;
                currentBuffer += 1;
                if (currentBuffer >= bufferSizeX)
                {
                    currentBuffer = 0;
                    serialPortArduino.DiscardInBuffer();
                }

                this.Refresh();
                
            }
            catch (Exception ex)
            {
                if (serialPortArduino.IsOpen)
                {
                    serialPortArduino.Close();
                }

                textBoxDebug.AppendText(ex.Message.ToString());
            }

        }

        private void FormGyroRealTime_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (serialPortArduino.IsOpen)
            {
                serialPortArduino.Close();
            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (serialPortArduino.IsOpen)
            {
                serialPortArduino.Close();
            }
        }

        private void FormGyroRealTime_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics g;
                Bitmap drawing = null;

                drawing = new Bitmap(this.Width, this.Height, e.Graphics);
                g = Graphics.FromImage(drawing);

                // Draw the grid
                Pen gridPen = new Pen(Color.Black);
                gridPen.DashPattern = dashValuesGrid;
                // y axis
                for (int x = rawGridMinX; x <= rawGridMaxX; x += 100)
                {
                    g.DrawLine(gridPen, x, rawGridMaxY, x, rawGridMinY);
                }

                // x axis
                for (int y = rawGridMinY; y <= rawGridMaxY; y += 100)
                {
                    g.DrawLine(gridPen, rawGridMinX, y, rawGridMaxX, y);
                }

                // zero on y axis is (rawGridMaxY - rawGridMinY)/2
                float scale = (float)(rawGridMaxY - rawGridMinY) / (float)analogRange;
                for (int x = 0; x <= currentBuffer; x++)
                {
                    g.DrawEllipse(new Pen(Color.Blue), new Rectangle(currentX, (int)(bufferX[x] * scale) + rawGridMinY, 5, 5));
                    currentX += 5;

                    if (currentX > rawGridMaxX)
                    {
                        currentX = rawGridMinX;
                    }
                }

                e.Graphics.DrawImageUnscaled(drawing, 0, 0);
                g.Dispose();
                
            }
            catch (Exception ex)
            {
                textBoxDebug.AppendText(ex.Message.ToString());
            }
        }

        private void FormGyroRealTime_Load(object sender, EventArgs e)
        {

        }
    }
}
