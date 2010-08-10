namespace GyroRealTime
{
    partial class FormGyroRealTime
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
            this.textBoxDebug = new System.Windows.Forms.TextBox();
            this.serialPortArduino = new System.IO.Ports.SerialPort(this.components);
            this.labelGyroPlot = new System.Windows.Forms.Label();
            this.labelDebug = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxDebug
            // 
            this.textBoxDebug.Location = new System.Drawing.Point(12, 80);
            this.textBoxDebug.Multiline = true;
            this.textBoxDebug.Name = "textBoxDebug";
            this.textBoxDebug.Size = new System.Drawing.Size(177, 400);
            this.textBoxDebug.TabIndex = 4;
            // 
            // serialPortArduino
            // 
            this.serialPortArduino.BaudRate = 115200;
            this.serialPortArduino.PortName = "COM8";
            this.serialPortArduino.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPortArduino_DataReceived);
            // 
            // labelGyroPlot
            // 
            this.labelGyroPlot.AutoSize = true;
            this.labelGyroPlot.Location = new System.Drawing.Point(204, 53);
            this.labelGyroPlot.Name = "labelGyroPlot";
            this.labelGyroPlot.Size = new System.Drawing.Size(75, 13);
            this.labelGyroPlot.TabIndex = 35;
            this.labelGyroPlot.Text = "Gyro Raw Plot";
            // 
            // labelDebug
            // 
            this.labelDebug.AutoSize = true;
            this.labelDebug.Location = new System.Drawing.Point(12, 53);
            this.labelDebug.Name = "labelDebug";
            this.labelDebug.Size = new System.Drawing.Size(39, 13);
            this.labelDebug.TabIndex = 36;
            this.labelDebug.Text = "Debug";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(15, 12);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 37;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // FormGyroRealTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 509);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.labelDebug);
            this.Controls.Add(this.labelGyroPlot);
            this.Controls.Add(this.textBoxDebug);
            this.Name = "FormGyroRealTime";
            this.Text = "Gyro Real Time";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGyroRealTime_FormClosing);
            this.Load += new System.EventHandler(this.FormGyroRealTime_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormGyroRealTime_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxDebug;
        private System.IO.Ports.SerialPort serialPortArduino;
        private System.Windows.Forms.Label labelGyroPlot;
        private System.Windows.Forms.Label labelDebug;
        private System.Windows.Forms.Button buttonClose;
    }
}

