using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        static int cmdLength = 8;

        public Form1()
        {
            InitializeComponent();
            InitializeSerialPort();
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                byte byteHeader = Byte.Parse(textBoxHeader.Text);
                byte byteRVert = Byte.Parse(textBoxRVert.Text);
                byte byteRHorz = Byte.Parse(textBoxRHorz.Text);
                byte byteLVert = Byte.Parse(textBoxLVert.Text);
                byte byteLHorz = Byte.Parse(textBoxLHorz.Text);
                byte byteButton = Byte.Parse(textBoxButton.Text);
                byte byteExt = Byte.Parse(textBoxExt.Text);

                // Calculate the checksum + display it
                byte byteChecksum = (byte)(0xFF - (byte)(byteRVert + byteRHorz + byteLVert + byteLHorz + byteButton + byteExt));
                textBoxChecksum.Text = byteChecksum.ToString();

                byte[] cmdBytes = new byte[8] { byteHeader, byteRVert, byteRHorz, byteLVert, byteLHorz, byteButton, byteExt, byteChecksum };
                if (serialPortMech.IsOpen)
                {
                    serialPortMech.Write(cmdBytes, 0, cmdLength);
                }
            }
            catch (Exception ex)
            {
                textBoxDebug.AppendText(ex.Message.ToString() + Environment.NewLine);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPortMech.IsOpen)
            {
                serialPortMech.Close();
            }
        }
    }
}
