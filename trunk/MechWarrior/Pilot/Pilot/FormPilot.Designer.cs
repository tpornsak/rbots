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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxFocus = new System.Windows.Forms.TextBox();
            this.labelArm = new System.Windows.Forms.Label();
            this.checkBoxArm = new System.Windows.Forms.CheckBox();
            this.labelPKeyArm = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarElPos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAzPos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarElSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAzSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxDebug
            // 
            this.textBoxDebug.Enabled = false;
            this.textBoxDebug.Location = new System.Drawing.Point(734, 389);
            this.textBoxDebug.Multiline = true;
            this.textBoxDebug.Name = "textBoxDebug";
            this.textBoxDebug.Size = new System.Drawing.Size(452, 134);
            this.textBoxDebug.TabIndex = 0;
            this.textBoxDebug.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxDebug_KeyDown);
            // 
            // labelDebugLog
            // 
            this.labelDebugLog.AutoSize = true;
            this.labelDebugLog.Enabled = false;
            this.labelDebugLog.Location = new System.Drawing.Point(742, 358);
            this.labelDebugLog.Name = "labelDebugLog";
            this.labelDebugLog.Size = new System.Drawing.Size(60, 13);
            this.labelDebugLog.TabIndex = 1;
            this.labelDebugLog.Text = "Debug Log";
            // 
            // mechCamera
            // 
            this.mechCamera.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.mechCamera.Location = new System.Drawing.Point(24, 73);
            this.mechCamera.Margin = new System.Windows.Forms.Padding(0);
            this.mechCamera.Name = "mechCamera";
            this.mechCamera.Size = new System.Drawing.Size(640, 480);
            this.mechCamera.TabIndex = 2;
            this.mechCamera.Text = "videoSourcePlayer1";
            this.mechCamera.VideoSource = null;
            this.mechCamera.NewFrame += new AForge.Controls.VideoSourcePlayer.NewFrameHandler(this.mechCamera_NewFrame);
            this.mechCamera.Click += new System.EventHandler(this.mechCamera_Click_1);
            this.mechCamera.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mechCamera_KeyDown);
            this.mechCamera.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mechCamera_MouseDown);
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
            this.toolStrip1.Location = new System.Drawing.Point(0, 637);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1284, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabelFPS
            // 
            this.toolStripLabelFPS.Name = "toolStripLabelFPS";
            this.toolStripLabelFPS.Size = new System.Drawing.Size(86, 22);
            this.toolStripLabelFPS.Text = "toolStripLabel1";
            // 
            // trackBarElPos
            // 
            this.trackBarElPos.Location = new System.Drawing.Point(871, 74);
            this.trackBarElPos.Maximum = 839;
            this.trackBarElPos.Minimum = 400;
            this.trackBarElPos.Name = "trackBarElPos";
            this.trackBarElPos.Size = new System.Drawing.Size(230, 45);
            this.trackBarElPos.TabIndex = 4;
            this.trackBarElPos.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarElPos.Value = 679;
            this.trackBarElPos.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            this.trackBarElPos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.trackBarElPos_KeyDown);
            this.trackBarElPos.MouseMove += new System.Windows.Forms.MouseEventHandler(this.trackBarElPos_MouseMove);
            // 
            // labelElPos
            // 
            this.labelElPos.AutoSize = true;
            this.labelElPos.Enabled = false;
            this.labelElPos.Location = new System.Drawing.Point(796, 80);
            this.labelElPos.Name = "labelElPos";
            this.labelElPos.Size = new System.Drawing.Size(72, 13);
            this.labelElPos.TabIndex = 5;
            this.labelElPos.Text = "Elevation Pos";
            // 
            // textBoxElPos
            // 
            this.textBoxElPos.Enabled = false;
            this.textBoxElPos.Location = new System.Drawing.Point(1107, 73);
            this.textBoxElPos.Name = "textBoxElPos";
            this.textBoxElPos.ReadOnly = true;
            this.textBoxElPos.Size = new System.Drawing.Size(40, 20);
            this.textBoxElPos.TabIndex = 6;
            // 
            // serialPortMech
            // 
            this.serialPortMech.BaudRate = 115200;
            this.serialPortMech.PortName = "COM4";
            // 
            // trackBarAzPos
            // 
            this.trackBarAzPos.Location = new System.Drawing.Point(871, 157);
            this.trackBarAzPos.Maximum = 1023;
            this.trackBarAzPos.Name = "trackBarAzPos";
            this.trackBarAzPos.Size = new System.Drawing.Size(230, 45);
            this.trackBarAzPos.TabIndex = 7;
            this.trackBarAzPos.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarAzPos.Value = 511;
            this.trackBarAzPos.ValueChanged += new System.EventHandler(this.trackBarAzPos_ValueChanged);
            this.trackBarAzPos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.trackBarAzPos_KeyDown);
            // 
            // labelAzPos
            // 
            this.labelAzPos.AutoSize = true;
            this.labelAzPos.Enabled = false;
            this.labelAzPos.Location = new System.Drawing.Point(802, 161);
            this.labelAzPos.Name = "labelAzPos";
            this.labelAzPos.Size = new System.Drawing.Size(65, 13);
            this.labelAzPos.TabIndex = 8;
            this.labelAzPos.Text = "Azimuth Pos";
            // 
            // textBoxAzPos
            // 
            this.textBoxAzPos.Enabled = false;
            this.textBoxAzPos.Location = new System.Drawing.Point(1107, 158);
            this.textBoxAzPos.Name = "textBoxAzPos";
            this.textBoxAzPos.ReadOnly = true;
            this.textBoxAzPos.Size = new System.Drawing.Size(40, 20);
            this.textBoxAzPos.TabIndex = 9;
            // 
            // textBoxElSpeed
            // 
            this.textBoxElSpeed.Enabled = false;
            this.textBoxElSpeed.Location = new System.Drawing.Point(1107, 113);
            this.textBoxElSpeed.Name = "textBoxElSpeed";
            this.textBoxElSpeed.ReadOnly = true;
            this.textBoxElSpeed.Size = new System.Drawing.Size(40, 20);
            this.textBoxElSpeed.TabIndex = 12;
            // 
            // labelElSpeed
            // 
            this.labelElSpeed.AutoSize = true;
            this.labelElSpeed.Enabled = false;
            this.labelElSpeed.Location = new System.Drawing.Point(784, 120);
            this.labelElSpeed.Name = "labelElSpeed";
            this.labelElSpeed.Size = new System.Drawing.Size(85, 13);
            this.labelElSpeed.TabIndex = 11;
            this.labelElSpeed.Text = "Elevation Speed";
            // 
            // trackBarElSpeed
            // 
            this.trackBarElSpeed.Location = new System.Drawing.Point(871, 115);
            this.trackBarElSpeed.Maximum = 1023;
            this.trackBarElSpeed.Minimum = 1;
            this.trackBarElSpeed.Name = "trackBarElSpeed";
            this.trackBarElSpeed.Size = new System.Drawing.Size(230, 45);
            this.trackBarElSpeed.TabIndex = 10;
            this.trackBarElSpeed.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarElSpeed.Value = 75;
            this.trackBarElSpeed.ValueChanged += new System.EventHandler(this.trackBarElSpeed_ValueChanged);
            this.trackBarElSpeed.KeyDown += new System.Windows.Forms.KeyEventHandler(this.trackBarElSpeed_KeyDown);
            // 
            // textBoxAzSpeed
            // 
            this.textBoxAzSpeed.Enabled = false;
            this.textBoxAzSpeed.Location = new System.Drawing.Point(1107, 199);
            this.textBoxAzSpeed.Name = "textBoxAzSpeed";
            this.textBoxAzSpeed.ReadOnly = true;
            this.textBoxAzSpeed.Size = new System.Drawing.Size(40, 20);
            this.textBoxAzSpeed.TabIndex = 15;
            // 
            // labelAzSpeed
            // 
            this.labelAzSpeed.AutoSize = true;
            this.labelAzSpeed.Enabled = false;
            this.labelAzSpeed.Location = new System.Drawing.Point(791, 203);
            this.labelAzSpeed.Name = "labelAzSpeed";
            this.labelAzSpeed.Size = new System.Drawing.Size(78, 13);
            this.labelAzSpeed.TabIndex = 14;
            this.labelAzSpeed.Text = "Azimuth Speed";
            // 
            // trackBarAzSpeed
            // 
            this.trackBarAzSpeed.Location = new System.Drawing.Point(871, 199);
            this.trackBarAzSpeed.Maximum = 1023;
            this.trackBarAzSpeed.Minimum = 1;
            this.trackBarAzSpeed.Name = "trackBarAzSpeed";
            this.trackBarAzSpeed.Size = new System.Drawing.Size(230, 45);
            this.trackBarAzSpeed.TabIndex = 13;
            this.trackBarAzSpeed.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarAzSpeed.Value = 75;
            this.trackBarAzSpeed.ValueChanged += new System.EventHandler(this.trackBarAzSpeed_ValueChanged);
            this.trackBarAzSpeed.KeyDown += new System.Windows.Forms.KeyEventHandler(this.trackBarAzSpeed_KeyDown);
            // 
            // buttonFire
            // 
            this.buttonFire.Location = new System.Drawing.Point(163, 20);
            this.buttonFire.Name = "buttonFire";
            this.buttonFire.Size = new System.Drawing.Size(90, 34);
            this.buttonFire.TabIndex = 16;
            this.buttonFire.Text = "Fire";
            this.buttonFire.UseVisualStyleBackColor = true;
            this.buttonFire.Click += new System.EventHandler(this.buttonFire_Click);
            this.buttonFire.KeyDown += new System.Windows.Forms.KeyEventHandler(this.buttonFire_KeyDown);
            // 
            // checkBoxMouseControl
            // 
            this.checkBoxMouseControl.AutoSize = true;
            this.checkBoxMouseControl.Location = new System.Drawing.Point(54, 30);
            this.checkBoxMouseControl.Name = "checkBoxMouseControl";
            this.checkBoxMouseControl.Size = new System.Drawing.Size(94, 17);
            this.checkBoxMouseControl.TabIndex = 17;
            this.checkBoxMouseControl.Text = "Mouse Control";
            this.checkBoxMouseControl.UseVisualStyleBackColor = true;
            this.checkBoxMouseControl.CheckedChanged += new System.EventHandler(this.checkBoxMouseControl_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(742, 248);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Debug Focus";
            // 
            // textBoxFocus
            // 
            this.textBoxFocus.Enabled = false;
            this.textBoxFocus.Location = new System.Drawing.Point(734, 272);
            this.textBoxFocus.Multiline = true;
            this.textBoxFocus.Name = "textBoxFocus";
            this.textBoxFocus.Size = new System.Drawing.Size(420, 83);
            this.textBoxFocus.TabIndex = 19;
            // 
            // labelArm
            // 
            this.labelArm.AutoSize = true;
            this.labelArm.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.labelArm.ForeColor = System.Drawing.Color.Green;
            this.labelArm.Location = new System.Drawing.Point(465, 20);
            this.labelArm.Name = "labelArm";
            this.labelArm.Size = new System.Drawing.Size(209, 37);
            this.labelArm.TabIndex = 20;
            this.labelArm.Text = "NOT ARMED";
            this.labelArm.Click += new System.EventHandler(this.labelArm_Click);
            // 
            // checkBoxArm
            // 
            this.checkBoxArm.AutoSize = true;
            this.checkBoxArm.Location = new System.Drawing.Point(259, 30);
            this.checkBoxArm.Name = "checkBoxArm";
            this.checkBoxArm.Size = new System.Drawing.Size(94, 17);
            this.checkBoxArm.TabIndex = 21;
            this.checkBoxArm.Text = "Ready To Arm";
            this.checkBoxArm.UseVisualStyleBackColor = true;
            this.checkBoxArm.CheckStateChanged += new System.EventHandler(this.checkBoxArm_CheckStateChanged);
            // 
            // labelPKeyArm
            // 
            this.labelPKeyArm.AutoSize = true;
            this.labelPKeyArm.Location = new System.Drawing.Point(375, 20);
            this.labelPKeyArm.Name = "labelPKeyArm";
            this.labelPKeyArm.Size = new System.Drawing.Size(76, 13);
            this.labelPKeyArm.TabIndex = 22;
            this.labelPKeyArm.Text = "Press P to Arm";
            this.labelPKeyArm.Click += new System.EventHandler(this.labelPKeyArm_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(359, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Press O to DisArm";
            // 
            // FormPilot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 662);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelPKeyArm);
            this.Controls.Add(this.checkBoxArm);
            this.Controls.Add(this.labelArm);
            this.Controls.Add(this.textBoxFocus);
            this.Controls.Add(this.label1);
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
            this.Text = "RA";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTrendnet_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormPilot_KeyDown);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxFocus;
        private System.Windows.Forms.Label labelArm;
        private System.Windows.Forms.CheckBox checkBoxArm;
        private System.Windows.Forms.Label labelPKeyArm;
        private System.Windows.Forms.Label label2;
    }
}

