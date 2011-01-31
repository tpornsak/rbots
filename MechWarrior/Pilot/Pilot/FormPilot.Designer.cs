namespace CameraTrendnetAForge
{
    partial class FormPilot
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
            this.labelDebugLog = new System.Windows.Forms.Label();
            this.mechCamera = new AForge.Controls.VideoSourcePlayer();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelFPS = new System.Windows.Forms.ToolStripLabel();
            this.trackBarElPos = new System.Windows.Forms.TrackBar();
            this.labelElPos = new System.Windows.Forms.Label();
            this.textBoxElPos = new System.Windows.Forms.TextBox();
            this.serialPortMech = new System.IO.Ports.SerialPort(this.components);
            this.trackBarAzPos = new System.Windows.Forms.TrackBar();
            this.labelAzPos = new System.Windows.Forms.Label();
            this.textBoxAzPos = new System.Windows.Forms.TextBox();
            this.textBoxElSpeed = new System.Windows.Forms.TextBox();
            this.labelElSpeed = new System.Windows.Forms.Label();
            this.trackBarElSpeed = new System.Windows.Forms.TrackBar();
            this.textBoxAzSpeed = new System.Windows.Forms.TextBox();
            this.labelAzSpeed = new System.Windows.Forms.Label();
            this.trackBarAzSpeed = new System.Windows.Forms.TrackBar();
            this.buttonFire = new System.Windows.Forms.Button();
            this.checkBoxMouseControl = new System.Windows.Forms.CheckBox();
            this.webBrowserCam = new System.Windows.Forms.WebBrowser();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarElPos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAzPos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarElSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAzSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxDebug
            // 
            this.textBoxDebug.Location = new System.Drawing.Point(618, 358);
            this.textBoxDebug.Multiline = true;
            this.textBoxDebug.Name = "textBoxDebug";
            this.textBoxDebug.Size = new System.Drawing.Size(466, 134);
            this.textBoxDebug.TabIndex = 0;
            // 
            // labelDebugLog
            // 
            this.labelDebugLog.AutoSize = true;
            this.labelDebugLog.Location = new System.Drawing.Point(618, 342);
            this.labelDebugLog.Name = "labelDebugLog";
            this.labelDebugLog.Size = new System.Drawing.Size(60, 13);
            this.labelDebugLog.TabIndex = 1;
            this.labelDebugLog.Text = "Debug Log";
            // 
            // mechCamera
            // 
            this.mechCamera.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.mechCamera.Location = new System.Drawing.Point(12, 12);
            this.mechCamera.Margin = new System.Windows.Forms.Padding(0);
            this.mechCamera.Name = "mechCamera";
            this.mechCamera.Size = new System.Drawing.Size(600, 480);
            this.mechCamera.TabIndex = 2;
            this.mechCamera.Text = "videoSourcePlayer1";
            this.mechCamera.VideoSource = null;
            this.mechCamera.NewFrame += new AForge.Controls.VideoSourcePlayer.NewFrameHandler(this.mechCamera_NewFrame);
            this.mechCamera.Click += new System.EventHandler(this.mechCamera_Click_1);
            this.mechCamera.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mechCamera_MouseMove);
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelFPS});
            this.toolStrip1.Location = new System.Drawing.Point(0, 1041);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1100, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabelFPS
            // 
            this.toolStripLabelFPS.Name = "toolStripLabelFPS";
            this.toolStripLabelFPS.Size = new System.Drawing.Size(78, 22);
            this.toolStripLabelFPS.Text = "toolStripLabel1";
            // 
            // trackBarElPos
            // 
            this.trackBarElPos.Location = new System.Drawing.Point(746, 12);
            this.trackBarElPos.Maximum = 616;
            this.trackBarElPos.Minimum = 375;
            this.trackBarElPos.Name = "trackBarElPos";
            this.trackBarElPos.Size = new System.Drawing.Size(230, 45);
            this.trackBarElPos.TabIndex = 4;
            this.trackBarElPos.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarElPos.Value = 511;
            this.trackBarElPos.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // labelElPos
            // 
            this.labelElPos.AutoSize = true;
            this.labelElPos.Location = new System.Drawing.Point(671, 18);
            this.labelElPos.Name = "labelElPos";
            this.labelElPos.Size = new System.Drawing.Size(72, 13);
            this.labelElPos.TabIndex = 5;
            this.labelElPos.Text = "Elevation Pos";
            // 
            // textBoxElPos
            // 
            this.textBoxElPos.Location = new System.Drawing.Point(974, 15);
            this.textBoxElPos.Name = "textBoxElPos";
            this.textBoxElPos.ReadOnly = true;
            this.textBoxElPos.Size = new System.Drawing.Size(40, 20);
            this.textBoxElPos.TabIndex = 6;
            // 
            // serialPortMech
            // 
            this.serialPortMech.PortName = "COM9";
            // 
            // trackBarAzPos
            // 
            this.trackBarAzPos.Location = new System.Drawing.Point(746, 95);
            this.trackBarAzPos.Maximum = 790;
            this.trackBarAzPos.Minimum = 225;
            this.trackBarAzPos.Name = "trackBarAzPos";
            this.trackBarAzPos.Size = new System.Drawing.Size(230, 45);
            this.trackBarAzPos.TabIndex = 7;
            this.trackBarAzPos.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarAzPos.Value = 511;
            this.trackBarAzPos.ValueChanged += new System.EventHandler(this.trackBarAzPos_ValueChanged);
            // 
            // labelAzPos
            // 
            this.labelAzPos.AutoSize = true;
            this.labelAzPos.Location = new System.Drawing.Point(677, 99);
            this.labelAzPos.Name = "labelAzPos";
            this.labelAzPos.Size = new System.Drawing.Size(65, 13);
            this.labelAzPos.TabIndex = 8;
            this.labelAzPos.Text = "Azimuth Pos";
            // 
            // textBoxAzPos
            // 
            this.textBoxAzPos.Location = new System.Drawing.Point(974, 95);
            this.textBoxAzPos.Name = "textBoxAzPos";
            this.textBoxAzPos.ReadOnly = true;
            this.textBoxAzPos.Size = new System.Drawing.Size(40, 20);
            this.textBoxAzPos.TabIndex = 9;
            // 
            // textBoxElSpeed
            // 
            this.textBoxElSpeed.Location = new System.Drawing.Point(974, 56);
            this.textBoxElSpeed.Name = "textBoxElSpeed";
            this.textBoxElSpeed.ReadOnly = true;
            this.textBoxElSpeed.Size = new System.Drawing.Size(40, 20);
            this.textBoxElSpeed.TabIndex = 12;
            // 
            // labelElSpeed
            // 
            this.labelElSpeed.AutoSize = true;
            this.labelElSpeed.Location = new System.Drawing.Point(659, 58);
            this.labelElSpeed.Name = "labelElSpeed";
            this.labelElSpeed.Size = new System.Drawing.Size(85, 13);
            this.labelElSpeed.TabIndex = 11;
            this.labelElSpeed.Text = "Elevation Speed";
            // 
            // trackBarElSpeed
            // 
            this.trackBarElSpeed.Location = new System.Drawing.Point(746, 53);
            this.trackBarElSpeed.Maximum = 1023;
            this.trackBarElSpeed.Minimum = 1;
            this.trackBarElSpeed.Name = "trackBarElSpeed";
            this.trackBarElSpeed.Size = new System.Drawing.Size(230, 45);
            this.trackBarElSpeed.TabIndex = 10;
            this.trackBarElSpeed.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarElSpeed.Value = 75;
            this.trackBarElSpeed.ValueChanged += new System.EventHandler(this.trackBarElSpeed_ValueChanged);
            // 
            // textBoxAzSpeed
            // 
            this.textBoxAzSpeed.Location = new System.Drawing.Point(974, 137);
            this.textBoxAzSpeed.Name = "textBoxAzSpeed";
            this.textBoxAzSpeed.ReadOnly = true;
            this.textBoxAzSpeed.Size = new System.Drawing.Size(40, 20);
            this.textBoxAzSpeed.TabIndex = 15;
            // 
            // labelAzSpeed
            // 
            this.labelAzSpeed.AutoSize = true;
            this.labelAzSpeed.Location = new System.Drawing.Point(666, 141);
            this.labelAzSpeed.Name = "labelAzSpeed";
            this.labelAzSpeed.Size = new System.Drawing.Size(78, 13);
            this.labelAzSpeed.TabIndex = 14;
            this.labelAzSpeed.Text = "Azimuth Speed";
            // 
            // trackBarAzSpeed
            // 
            this.trackBarAzSpeed.Location = new System.Drawing.Point(746, 137);
            this.trackBarAzSpeed.Maximum = 1023;
            this.trackBarAzSpeed.Minimum = 1;
            this.trackBarAzSpeed.Name = "trackBarAzSpeed";
            this.trackBarAzSpeed.Size = new System.Drawing.Size(230, 45);
            this.trackBarAzSpeed.TabIndex = 13;
            this.trackBarAzSpeed.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarAzSpeed.Value = 75;
            this.trackBarAzSpeed.ValueChanged += new System.EventHandler(this.trackBarAzSpeed_ValueChanged);
            // 
            // buttonFire
            // 
            this.buttonFire.Location = new System.Drawing.Point(809, 258);
            this.buttonFire.Name = "buttonFire";
            this.buttonFire.Size = new System.Drawing.Size(90, 34);
            this.buttonFire.TabIndex = 16;
            this.buttonFire.Text = "Fire";
            this.buttonFire.UseVisualStyleBackColor = true;
            this.buttonFire.Click += new System.EventHandler(this.buttonFire_Click);
            // 
            // checkBoxMouseControl
            // 
            this.checkBoxMouseControl.AutoSize = true;
            this.checkBoxMouseControl.Location = new System.Drawing.Point(669, 188);
            this.checkBoxMouseControl.Name = "checkBoxMouseControl";
            this.checkBoxMouseControl.Size = new System.Drawing.Size(94, 17);
            this.checkBoxMouseControl.TabIndex = 17;
            this.checkBoxMouseControl.Text = "Mouse Control";
            this.checkBoxMouseControl.UseVisualStyleBackColor = true;
            this.checkBoxMouseControl.CheckedChanged += new System.EventHandler(this.checkBoxMouseControl_CheckedChanged);
            // 
            // webBrowserCam
            // 
            this.webBrowserCam.Location = new System.Drawing.Point(13, 506);
            this.webBrowserCam.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserCam.Name = "webBrowserCam";
            this.webBrowserCam.Size = new System.Drawing.Size(660, 500);
            this.webBrowserCam.TabIndex = 18;
            this.webBrowserCam.Url = new System.Uri("http://192.168.2.3/img/video.mjpeg", System.UriKind.Absolute);
            // 
            // FormPilot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 1066);
            this.Controls.Add(this.webBrowserCam);
            this.Controls.Add(this.checkBoxMouseControl);
            this.Controls.Add(this.buttonFire);
            this.Controls.Add(this.textBoxAzSpeed);
            this.Controls.Add(this.labelAzSpeed);
            this.Controls.Add(this.trackBarAzSpeed);
            this.Controls.Add(this.textBoxElSpeed);
            this.Controls.Add(this.labelElSpeed);
            this.Controls.Add(this.trackBarElSpeed);
            this.Controls.Add(this.textBoxAzPos);
            this.Controls.Add(this.labelAzPos);
            this.Controls.Add(this.trackBarAzPos);
            this.Controls.Add(this.textBoxElPos);
            this.Controls.Add(this.labelElPos);
            this.Controls.Add(this.trackBarElPos);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.mechCamera);
            this.Controls.Add(this.labelDebugLog);
            this.Controls.Add(this.textBoxDebug);
            this.Name = "FormPilot";
            this.Text = "Mech Warrior Vision";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTrendnet_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FormPilot_MouseClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormPilot_MouseMove);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarElPos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAzPos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarElSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAzSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxDebug;
        private System.Windows.Forms.Label labelDebugLog;
        private AForge.Controls.VideoSourcePlayer mechCamera;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabelFPS;
        private System.Windows.Forms.TrackBar trackBarElPos;
        private System.Windows.Forms.Label labelElPos;
        private System.Windows.Forms.TextBox textBoxElPos;
        private System.IO.Ports.SerialPort serialPortMech;
        private System.Windows.Forms.TrackBar trackBarAzPos;
        private System.Windows.Forms.Label labelAzPos;
        private System.Windows.Forms.TextBox textBoxAzPos;
        private System.Windows.Forms.TextBox textBoxElSpeed;
        private System.Windows.Forms.Label labelElSpeed;
        private System.Windows.Forms.TrackBar trackBarElSpeed;
        private System.Windows.Forms.TextBox textBoxAzSpeed;
        private System.Windows.Forms.Label labelAzSpeed;
        private System.Windows.Forms.TrackBar trackBarAzSpeed;
        private System.Windows.Forms.Button buttonFire;
        private System.Windows.Forms.CheckBox checkBoxMouseControl;
        private System.Windows.Forms.WebBrowser webBrowserCam;
    }
}

