namespace SerialPortArduino
{
    partial class FormSerialPortArduino
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBoxWrite = new System.Windows.Forms.TextBox();
            this.buttonWrite = new System.Windows.Forms.Button();
            this.labelRead = new System.Windows.Forms.Label();
            this.textBoxRead = new System.Windows.Forms.TextBox();
            this.serialPortArduino = new System.IO.Ports.SerialPort(this.components);
            this.SuspendLayout();
            // 
            // textBoxWrite
            // 
            this.textBoxWrite.Location = new System.Drawing.Point(12, 12);
            this.textBoxWrite.Name = "textBoxWrite";
            this.textBoxWrite.Size = new System.Drawing.Size(173, 20);
            this.textBoxWrite.TabIndex = 0;
            // 
            // buttonWrite
            // 
            this.buttonWrite.Location = new System.Drawing.Point(221, 8);
            this.buttonWrite.Name = "buttonWrite";
            this.buttonWrite.Size = new System.Drawing.Size(75, 23);
            this.buttonWrite.TabIndex = 1;
            this.buttonWrite.Text = "Write";
            this.buttonWrite.UseVisualStyleBackColor = true;
            this.buttonWrite.Click += new System.EventHandler(this.buttonWrite_Click);
            // 
            // labelRead
            // 
            this.labelRead.AutoSize = true;
            this.labelRead.Location = new System.Drawing.Point(12, 57);
            this.labelRead.Name = "labelRead";
            this.labelRead.Size = new System.Drawing.Size(33, 13);
            this.labelRead.TabIndex = 2;
            this.labelRead.Text = "Read";
            // 
            // textBoxRead
            // 
            this.textBoxRead.Location = new System.Drawing.Point(12, 83);
            this.textBoxRead.Multiline = true;
            this.textBoxRead.Name = "textBoxRead";
            this.textBoxRead.Size = new System.Drawing.Size(284, 248);
            this.textBoxRead.TabIndex = 3;
            // 
            // serialPortArduino
            // 
            this.serialPortArduino.PortName = "COM8";
            this.serialPortArduino.ReadTimeout = 2000;
            this.serialPortArduino.WriteTimeout = 2000;
            // 
            // FormSerialPortArduino
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 350);
            this.Controls.Add(this.textBoxRead);
            this.Controls.Add(this.labelRead);
            this.Controls.Add(this.buttonWrite);
            this.Controls.Add(this.textBoxWrite);
            this.Name = "FormSerialPortArduino";
            this.Text = "SerialPortArduino";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxWrite;
        private System.Windows.Forms.Button buttonWrite;
        private System.Windows.Forms.Label labelRead;
        private System.Windows.Forms.TextBox textBoxRead;
        private System.IO.Ports.SerialPort serialPortArduino;
    }
}

