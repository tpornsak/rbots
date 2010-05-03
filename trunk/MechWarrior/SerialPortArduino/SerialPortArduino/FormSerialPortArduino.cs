/* This is a simple c# program to demonstrate serial communication with
 * the Arduino Microcontroller using ASCII encoding.  The only other odd
 * thing is that the multiline textbox needs carriage return and line feed
 * and therefore we replace the carriage return "\r" with the NewLine type.
 * 
 * The arduino waits until it recieves an "A" and then replies with a "B".
 * Type A in the top left box and click the Write button.  If the Arduino is 
 * connected and the correct program running on it, it will write "B" in the
 * Read textbox
 * 
 * License: GPL 3
 * Author:  Ben Loftin
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SerialPortArduino
{
    public partial class FormSerialPortArduino : Form
    {
        public FormSerialPortArduino()
        {
            InitializeComponent();
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
            try
            {
                // The encoding, ardunio uses ASCII encoding in serial comm
                ASCIIEncoding ascii = new ASCIIEncoding();

                serialPortArduino.Open();
                // the arduino program serialResponse is expecting to 
                // receive the character 'A' before it sends its reply 'B'
                serialPortArduino.Write(textBoxWrite.Text);
                serialPortArduino.Encoding = ascii;
                string str = serialPortArduino.ReadLine();
                str = str.Replace("\r", Environment.NewLine);
                textBoxRead.AppendText(str);
                serialPortArduino.Close();
            }
            catch (Exception ex)
            {
                if (serialPortArduino.IsOpen)
                {
                    serialPortArduino.Close();
                }
                    
                textBoxRead.AppendText(ex.Message.ToString());
            }
        }
    }
}
