using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace CameraTrendnet
{
    public partial class FormCamera : Form
    {
        public FormCamera()
        {
            InitializeComponent();
            string cameraURL = "http://192.168.2.15/cgi/jpg/image.cgi";
            byte[] buffer = new byte[100000];
            int read, total = 0;

            System.Net.HttpWebRequest req = (HttpWebRequest)WebRequest.Create(cameraURL);
            req.Credentials = new NetworkCredential("bloftin", "r4d4r4");

            WebResponse response = req.GetResponse();
            Stream stream = response.GetResponseStream();

            while ((read = stream.Read(buffer, total, 1000)) != 0)
            {
                total += read;
            }

            Bitmap bmp = (Bitmap)Bitmap.FromStream(new MemoryStream(buffer, 0, total));

            pictureBoxCamera.Image = bmp;

            textBoxLog.AppendText(response.Headers.ToString());

        }
    }
}
