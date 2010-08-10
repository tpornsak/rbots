using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace FormGyroRecord
{
    public partial class FormRecordGyro : Form
    {
        ASCIIEncoding ascii;
        StreamWriter fileOut;
        DateTime CurrTime;
        int rawGyroValue = 0;

        public FormRecordGyro()
        {
            InitializeComponent();
        }

        private void buttonOutputFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxOutputFile.Text = saveFileDialog1.FileName;
            } 
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            // Open Serial Port
            try
            {
                

                // open file to write
                if (textBoxOutputFile.Text == String.Empty)
                {
                     SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                    saveFileDialog1.RestoreDirectory = true;

                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        textBoxOutputFile.Text = saveFileDialog1.FileName;
                    }
                }

                fileOut = new StreamWriter(textBoxOutputFile.Text);

                // write Date/time for first line
                CurrTime = DateTime.Now;

                fileOut.WriteLine("% " + CurrTime.Date.ToLongDateString());
                fileOut.WriteLine("% Timestamp, Analog Value (0-1023)");
                
                // disable the button while recording, enable the stop
                buttonStart.Enabled = false;
                buttonStop.Enabled = true;

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

                if (fileOut != null)
                {
                    fileOut.Close();
                }

                textBoxDebug.AppendText(ex.Message.ToString());
            }

        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (serialPortArduino.IsOpen)
            {
                serialPortArduino.Close();
            }

            if (fileOut != null)
            {
                fileOut.Close();
            }

            // disable the button while recording, enable the stop
            buttonStart.Enabled = true;
            buttonStop.Enabled = false;
            
        }

        private void serialPortArduino_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                CurrTime = DateTime.Now;
                string str = serialPortArduino.ReadLine();
                
                str = str.Replace("\r", "");
                
                // build up time stamp
                string time = CurrTime.Hour.ToString() + ":" + CurrTime.Minute.ToString() + ":" +
                              CurrTime.Second.ToString() + "." + CurrTime.Millisecond.ToString() + ",";
                fileOut.WriteLine( time + str );
             
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

        private void FormRecordGyro_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPortArduino.IsOpen)
            {
                serialPortArduino.Close();
            }
            if (fileOut != null)
            {
                fileOut.Close();
            }
        }
    }
}
