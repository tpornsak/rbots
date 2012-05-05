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
            this.labelHitPoints = new System.Windows.Forms.Label();
            this.timerMatchClock = new System.Windows.Forms.Timer(this.components);
            this.labelMinutes = new System.Windows.Forms.Label();
            this.labelColon = new System.Windows.Forms.Label();
            this.labelSeconds = new System.Windows.Forms.Label();
            this.panelGunOrientation = new System.Windows.Forms.Panel();
            this.labelTargetPlate = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelBearingInt = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelLeftIR = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelRightIR = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelBackIR = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.labelFrontIR = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.labelServoR = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.labelServoL = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.labelServoLPos = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.labelServoRPos = new System.Windows.Forms.Label();
            this.pictureBoxMech = new System.Windows.Forms.PictureBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarElPos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAzPos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarElSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAzSpeed)).BeginInit();
            this.panelGunOrientation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMech)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxDebug
            // 
            this.textBoxDebug.Enabled = false;
            this.textBoxDebug.Location = new System.Drawing.Point(24, 569);
            this.textBoxDebug.Multiline = true;
            this.textBoxDebug.Name = "textBoxDebug";
            this.textBoxDebug.Size = new System.Drawing.Size(303, 65);
            this.textBoxDebug.TabIndex = 0;
            this.textBoxDebug.Visible = false;
            this.textBoxDebug.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxDebug_KeyDown);
            // 
            // labelDebugLog
            // 
            this.labelDebugLog.AutoSize = true;
            this.labelDebugLog.Enabled = false;
            this.labelDebugLog.Location = new System.Drawing.Point(21, 553);
            this.labelDebugLog.Name = "labelDebugLog";
            this.labelDebugLog.Size = new System.Drawing.Size(60, 13);
            this.labelDebugLog.TabIndex = 1;
            this.labelDebugLog.Text = "Debug Log";
            this.labelDebugLog.Visible = false;
            // 
            // mechCamera
            // 
            this.mechCamera.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.mechCamera.Location = new System.Drawing.Point(24, 83);
            this.mechCamera.Margin = new System.Windows.Forms.Padding(0);
            this.mechCamera.Name = "mechCamera";
            this.mechCamera.Size = new System.Drawing.Size(640, 480);
            this.mechCamera.TabIndex = 2;
            this.mechCamera.Text = "videoSourcePlayer1";
            this.mechCamera.VideoSource = null;
            this.mechCamera.Visible = false;
            this.mechCamera.NewFrame += new AForge.Controls.VideoSourcePlayer.NewFrameHandler(this.mechCamera_NewFrame);
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
            this.toolStrip1.Location = new System.Drawing.Point(0, 584);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1028, 25);
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
            this.trackBarElPos.Location = new System.Drawing.Point(781, 541);
            this.trackBarElPos.Maximum = 750;
            this.trackBarElPos.Minimum = 545;
            this.trackBarElPos.Name = "trackBarElPos";
            this.trackBarElPos.Size = new System.Drawing.Size(230, 45);
            this.trackBarElPos.TabIndex = 4;
            this.trackBarElPos.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarElPos.Value = 700;
            this.trackBarElPos.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            this.trackBarElPos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.trackBarElPos_KeyDown);
            this.trackBarElPos.MouseMove += new System.Windows.Forms.MouseEventHandler(this.trackBarElPos_MouseMove);
            // 
            // labelElPos
            // 
            this.labelElPos.AutoSize = true;
            this.labelElPos.Enabled = false;
            this.labelElPos.Location = new System.Drawing.Point(706, 547);
            this.labelElPos.Name = "labelElPos";
            this.labelElPos.Size = new System.Drawing.Size(72, 13);
            this.labelElPos.TabIndex = 5;
            this.labelElPos.Text = "Elevation Pos";
            // 
            // textBoxElPos
            // 
            this.textBoxElPos.Enabled = false;
            this.textBoxElPos.Location = new System.Drawing.Point(1017, 540);
            this.textBoxElPos.Name = "textBoxElPos";
            this.textBoxElPos.ReadOnly = true;
            this.textBoxElPos.Size = new System.Drawing.Size(40, 20);
            this.textBoxElPos.TabIndex = 6;
            // 
            // serialPortMech
            // 
            this.serialPortMech.BaudRate = 38400;
            this.serialPortMech.PortName = "COM20";
            this.serialPortMech.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPortMech_DataReceived);
            // 
            // trackBarAzPos
            // 
            this.trackBarAzPos.Location = new System.Drawing.Point(781, 624);
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
            this.labelAzPos.Location = new System.Drawing.Point(712, 628);
            this.labelAzPos.Name = "labelAzPos";
            this.labelAzPos.Size = new System.Drawing.Size(65, 13);
            this.labelAzPos.TabIndex = 8;
            this.labelAzPos.Text = "Azimuth Pos";
            // 
            // textBoxAzPos
            // 
            this.textBoxAzPos.Enabled = false;
            this.textBoxAzPos.Location = new System.Drawing.Point(1017, 625);
            this.textBoxAzPos.Name = "textBoxAzPos";
            this.textBoxAzPos.ReadOnly = true;
            this.textBoxAzPos.Size = new System.Drawing.Size(40, 20);
            this.textBoxAzPos.TabIndex = 9;
            // 
            // textBoxElSpeed
            // 
            this.textBoxElSpeed.Enabled = false;
            this.textBoxElSpeed.Location = new System.Drawing.Point(1017, 580);
            this.textBoxElSpeed.Name = "textBoxElSpeed";
            this.textBoxElSpeed.ReadOnly = true;
            this.textBoxElSpeed.Size = new System.Drawing.Size(40, 20);
            this.textBoxElSpeed.TabIndex = 12;
            // 
            // labelElSpeed
            // 
            this.labelElSpeed.AutoSize = true;
            this.labelElSpeed.Enabled = false;
            this.labelElSpeed.Location = new System.Drawing.Point(694, 587);
            this.labelElSpeed.Name = "labelElSpeed";
            this.labelElSpeed.Size = new System.Drawing.Size(85, 13);
            this.labelElSpeed.TabIndex = 11;
            this.labelElSpeed.Text = "Elevation Speed";
            // 
            // trackBarElSpeed
            // 
            this.trackBarElSpeed.Location = new System.Drawing.Point(781, 582);
            this.trackBarElSpeed.Maximum = 1023;
            this.trackBarElSpeed.Minimum = 1;
            this.trackBarElSpeed.Name = "trackBarElSpeed";
            this.trackBarElSpeed.Size = new System.Drawing.Size(230, 45);
            this.trackBarElSpeed.TabIndex = 10;
            this.trackBarElSpeed.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarElSpeed.Value = 150;
            this.trackBarElSpeed.ValueChanged += new System.EventHandler(this.trackBarElSpeed_ValueChanged);
            this.trackBarElSpeed.KeyDown += new System.Windows.Forms.KeyEventHandler(this.trackBarElSpeed_KeyDown);
            // 
            // textBoxAzSpeed
            // 
            this.textBoxAzSpeed.Enabled = false;
            this.textBoxAzSpeed.Location = new System.Drawing.Point(1017, 666);
            this.textBoxAzSpeed.Name = "textBoxAzSpeed";
            this.textBoxAzSpeed.ReadOnly = true;
            this.textBoxAzSpeed.Size = new System.Drawing.Size(40, 20);
            this.textBoxAzSpeed.TabIndex = 15;
            // 
            // labelAzSpeed
            // 
            this.labelAzSpeed.AutoSize = true;
            this.labelAzSpeed.Enabled = false;
            this.labelAzSpeed.Location = new System.Drawing.Point(701, 670);
            this.labelAzSpeed.Name = "labelAzSpeed";
            this.labelAzSpeed.Size = new System.Drawing.Size(78, 13);
            this.labelAzSpeed.TabIndex = 14;
            this.labelAzSpeed.Text = "Azimuth Speed";
            // 
            // trackBarAzSpeed
            // 
            this.trackBarAzSpeed.Location = new System.Drawing.Point(781, 666);
            this.trackBarAzSpeed.Maximum = 1023;
            this.trackBarAzSpeed.Minimum = 1;
            this.trackBarAzSpeed.Name = "trackBarAzSpeed";
            this.trackBarAzSpeed.Size = new System.Drawing.Size(230, 45);
            this.trackBarAzSpeed.TabIndex = 13;
            this.trackBarAzSpeed.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarAzSpeed.Value = 150;
            this.trackBarAzSpeed.ValueChanged += new System.EventHandler(this.trackBarAzSpeed_ValueChanged);
            this.trackBarAzSpeed.KeyDown += new System.Windows.Forms.KeyEventHandler(this.trackBarAzSpeed_KeyDown);
            // 
            // buttonFire
            // 
            this.buttonFire.Location = new System.Drawing.Point(163, 5);
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
            this.checkBoxMouseControl.Location = new System.Drawing.Point(54, 15);
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
            this.label1.Location = new System.Drawing.Point(359, 553);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Debug Focus";
            this.label1.Visible = false;
            // 
            // textBoxFocus
            // 
            this.textBoxFocus.Enabled = false;
            this.textBoxFocus.Location = new System.Drawing.Point(16, 405);
            this.textBoxFocus.Multiline = true;
            this.textBoxFocus.Name = "textBoxFocus";
            this.textBoxFocus.Size = new System.Drawing.Size(309, 32);
            this.textBoxFocus.TabIndex = 19;
            // 
            // labelArm
            // 
            this.labelArm.AutoSize = true;
            this.labelArm.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.labelArm.ForeColor = System.Drawing.Color.Green;
            this.labelArm.Location = new System.Drawing.Point(465, 5);
            this.labelArm.Name = "labelArm";
            this.labelArm.Size = new System.Drawing.Size(209, 37);
            this.labelArm.TabIndex = 20;
            this.labelArm.Text = "NOT ARMED";
            this.labelArm.Click += new System.EventHandler(this.labelArm_Click);
            // 
            // checkBoxArm
            // 
            this.checkBoxArm.AutoSize = true;
            this.checkBoxArm.Location = new System.Drawing.Point(259, 15);
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
            this.labelPKeyArm.Location = new System.Drawing.Point(375, 5);
            this.labelPKeyArm.Name = "labelPKeyArm";
            this.labelPKeyArm.Size = new System.Drawing.Size(76, 13);
            this.labelPKeyArm.TabIndex = 22;
            this.labelPKeyArm.Text = "Press P to Arm";
            this.labelPKeyArm.Click += new System.EventHandler(this.labelPKeyArm_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(359, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Press O to DisArm";
            // 
            // labelHitPoints
            // 
            this.labelHitPoints.AutoSize = true;
            this.labelHitPoints.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHitPoints.ForeColor = System.Drawing.Color.Blue;
            this.labelHitPoints.Location = new System.Drawing.Point(523, 595);
            this.labelHitPoints.Name = "labelHitPoints";
            this.labelHitPoints.Size = new System.Drawing.Size(36, 37);
            this.labelHitPoints.TabIndex = 24;
            this.labelHitPoints.Text = "0";
            // 
            // timerMatchClock
            // 
            this.timerMatchClock.Interval = 1000;
            this.timerMatchClock.Tick += new System.EventHandler(this.timerMatchClock_Tick);
            // 
            // labelMinutes
            // 
            this.labelMinutes.AutoSize = true;
            this.labelMinutes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMinutes.ForeColor = System.Drawing.Color.Blue;
            this.labelMinutes.Location = new System.Drawing.Point(292, 47);
            this.labelMinutes.Name = "labelMinutes";
            this.labelMinutes.Size = new System.Drawing.Size(24, 16);
            this.labelMinutes.TabIndex = 25;
            this.labelMinutes.Text = "00";
            // 
            // labelColon
            // 
            this.labelColon.AutoSize = true;
            this.labelColon.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelColon.ForeColor = System.Drawing.Color.Blue;
            this.labelColon.Location = new System.Drawing.Point(313, 45);
            this.labelColon.Name = "labelColon";
            this.labelColon.Size = new System.Drawing.Size(12, 16);
            this.labelColon.TabIndex = 26;
            this.labelColon.Text = ":";
            // 
            // labelSeconds
            // 
            this.labelSeconds.AutoSize = true;
            this.labelSeconds.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSeconds.ForeColor = System.Drawing.Color.Blue;
            this.labelSeconds.Location = new System.Drawing.Point(322, 47);
            this.labelSeconds.Name = "labelSeconds";
            this.labelSeconds.Size = new System.Drawing.Size(24, 16);
            this.labelSeconds.TabIndex = 27;
            this.labelSeconds.Text = "00";
            // 
            // panelGunOrientation
            // 
            this.panelGunOrientation.Controls.Add(this.textBoxFocus);
            this.panelGunOrientation.Location = new System.Drawing.Point(681, 73);
            this.panelGunOrientation.Name = "panelGunOrientation";
            this.panelGunOrientation.Size = new System.Drawing.Size(450, 450);
            this.panelGunOrientation.TabIndex = 28;
            this.panelGunOrientation.Paint += new System.Windows.Forms.PaintEventHandler(this.panelGunOrientation_Paint);
            // 
            // labelTargetPlate
            // 
            this.labelTargetPlate.AutoSize = true;
            this.labelTargetPlate.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTargetPlate.ForeColor = System.Drawing.Color.Blue;
            this.labelTargetPlate.Location = new System.Drawing.Point(571, 595);
            this.labelTargetPlate.Name = "labelTargetPlate";
            this.labelTargetPlate.Size = new System.Drawing.Size(36, 37);
            this.labelTargetPlate.TabIndex = 29;
            this.labelTargetPlate.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(527, 582);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Hits";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(575, 582);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 13);
            this.label4.TabIndex = 31;
            this.label4.Text = "TP";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(211, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 32;
            this.label5.Text = "Match Timer";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(456, 582);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 34;
            this.label6.Text = "Bearing";
            // 
            // labelBearingInt
            // 
            this.labelBearingInt.AutoSize = true;
            this.labelBearingInt.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBearingInt.ForeColor = System.Drawing.Color.Blue;
            this.labelBearingInt.Location = new System.Drawing.Point(452, 595);
            this.labelBearingInt.Name = "labelBearingInt";
            this.labelBearingInt.Size = new System.Drawing.Size(36, 37);
            this.labelBearingInt.TabIndex = 33;
            this.labelBearingInt.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(237, 582);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 36;
            this.label7.Text = "Left IR";
            // 
            // labelLeftIR
            // 
            this.labelLeftIR.AutoSize = true;
            this.labelLeftIR.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLeftIR.ForeColor = System.Drawing.Color.Blue;
            this.labelLeftIR.Location = new System.Drawing.Point(233, 595);
            this.labelLeftIR.Name = "labelLeftIR";
            this.labelLeftIR.Size = new System.Drawing.Size(36, 37);
            this.labelLeftIR.TabIndex = 35;
            this.labelLeftIR.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(370, 582);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 38;
            this.label8.Text = "Right IR";
            // 
            // labelRightIR
            // 
            this.labelRightIR.AutoSize = true;
            this.labelRightIR.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRightIR.ForeColor = System.Drawing.Color.Blue;
            this.labelRightIR.Location = new System.Drawing.Point(366, 595);
            this.labelRightIR.Name = "labelRightIR";
            this.labelRightIR.Size = new System.Drawing.Size(36, 37);
            this.labelRightIR.TabIndex = 37;
            this.labelRightIR.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(370, 642);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 13);
            this.label9.TabIndex = 42;
            this.label9.Text = "Back IR";
            // 
            // labelBackIR
            // 
            this.labelBackIR.AutoSize = true;
            this.labelBackIR.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBackIR.ForeColor = System.Drawing.Color.Blue;
            this.labelBackIR.Location = new System.Drawing.Point(366, 655);
            this.labelBackIR.Name = "labelBackIR";
            this.labelBackIR.Size = new System.Drawing.Size(36, 37);
            this.labelBackIR.TabIndex = 41;
            this.labelBackIR.Text = "0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(237, 642);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(45, 13);
            this.label11.TabIndex = 40;
            this.label11.Text = "Front IR";
            // 
            // labelFrontIR
            // 
            this.labelFrontIR.AutoSize = true;
            this.labelFrontIR.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFrontIR.ForeColor = System.Drawing.Color.Blue;
            this.labelFrontIR.Location = new System.Drawing.Point(233, 655);
            this.labelFrontIR.Name = "labelFrontIR";
            this.labelFrontIR.Size = new System.Drawing.Size(36, 37);
            this.labelFrontIR.TabIndex = 39;
            this.labelFrontIR.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(150, 582);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 13);
            this.label10.TabIndex = 44;
            this.label10.Text = "Servo R IR";
            // 
            // labelServoR
            // 
            this.labelServoR.AutoSize = true;
            this.labelServoR.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelServoR.ForeColor = System.Drawing.Color.Blue;
            this.labelServoR.Location = new System.Drawing.Point(146, 595);
            this.labelServoR.Name = "labelServoR";
            this.labelServoR.Size = new System.Drawing.Size(36, 37);
            this.labelServoR.TabIndex = 43;
            this.labelServoR.Text = "0";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(22, 582);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(58, 13);
            this.label13.TabIndex = 46;
            this.label13.Text = "Servo L IR";
            // 
            // labelServoL
            // 
            this.labelServoL.AutoSize = true;
            this.labelServoL.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelServoL.ForeColor = System.Drawing.Color.Blue;
            this.labelServoL.Location = new System.Drawing.Point(18, 595);
            this.labelServoL.Name = "labelServoL";
            this.labelServoL.Size = new System.Drawing.Size(36, 37);
            this.labelServoL.TabIndex = 45;
            this.labelServoL.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(22, 642);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 13);
            this.label12.TabIndex = 50;
            this.label12.Text = "Servo L Pos";
            // 
            // labelServoLPos
            // 
            this.labelServoLPos.AutoSize = true;
            this.labelServoLPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelServoLPos.ForeColor = System.Drawing.Color.Blue;
            this.labelServoLPos.Location = new System.Drawing.Point(18, 655);
            this.labelServoLPos.Name = "labelServoLPos";
            this.labelServoLPos.Size = new System.Drawing.Size(36, 37);
            this.labelServoLPos.TabIndex = 49;
            this.labelServoLPos.Text = "0";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(150, 642);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(67, 13);
            this.label15.TabIndex = 48;
            this.label15.Text = "Servo R Pos";
            // 
            // labelServoRPos
            // 
            this.labelServoRPos.AutoSize = true;
            this.labelServoRPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelServoRPos.ForeColor = System.Drawing.Color.Blue;
            this.labelServoRPos.Location = new System.Drawing.Point(146, 655);
            this.labelServoRPos.Name = "labelServoRPos";
            this.labelServoRPos.Size = new System.Drawing.Size(36, 37);
            this.labelServoRPos.TabIndex = 47;
            this.labelServoRPos.Text = "0";
            // 
            // pictureBoxMech
            // 
            this.pictureBoxMech.Location = new System.Drawing.Point(24, 83);
            this.pictureBoxMech.Name = "pictureBoxMech";
            this.pictureBoxMech.Size = new System.Drawing.Size(640, 480);
            this.pictureBoxMech.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxMech.TabIndex = 51;
            this.pictureBoxMech.TabStop = false;
            this.pictureBoxMech.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxMech_MouseDown);
            this.pictureBoxMech.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxMech_MouseMove);
            // 
            // FormPilot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 609);
            this.Controls.Add(this.trackBarElSpeed);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.trackBarElPos);
            this.Controls.Add(this.labelServoLPos);
            this.Controls.Add(this.labelElPos);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.textBoxElPos);
            this.Controls.Add(this.labelServoRPos);
            this.Controls.Add(this.trackBarAzPos);
            this.Controls.Add(this.labelAzPos);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBoxAzPos);
            this.Controls.Add(this.labelServoL);
            this.Controls.Add(this.labelElSpeed);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxElSpeed);
            this.Controls.Add(this.labelServoR);
            this.Controls.Add(this.trackBarAzSpeed);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.labelAzSpeed);
            this.Controls.Add(this.labelBackIR);
            this.Controls.Add(this.textBoxAzSpeed);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.labelFrontIR);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.labelRightIR);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.labelLeftIR);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.labelBearingInt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelTargetPlate);
            this.Controls.Add(this.panelGunOrientation);
            this.Controls.Add(this.labelSeconds);
            this.Controls.Add(this.labelColon);
            this.Controls.Add(this.labelMinutes);
            this.Controls.Add(this.labelHitPoints);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelPKeyArm);
            this.Controls.Add(this.checkBoxArm);
            this.Controls.Add(this.labelArm);
            this.Controls.Add(this.checkBoxMouseControl);
            this.Controls.Add(this.buttonFire);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxDebug);
            this.Controls.Add(this.labelDebugLog);
            this.Controls.Add(this.pictureBoxMech);
            this.Controls.Add(this.mechCamera);
            this.KeyPreview = true;
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
            this.panelGunOrientation.ResumeLayout(false);
            this.panelGunOrientation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMech)).EndInit();
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
        private System.Windows.Forms.Label labelHitPoints;
        private System.Windows.Forms.Timer timerMatchClock;
        private System.Windows.Forms.Label labelMinutes;
        private System.Windows.Forms.Label labelColon;
        private System.Windows.Forms.Label labelSeconds;
        private System.Windows.Forms.Panel panelGunOrientation;
        private System.Windows.Forms.Label labelTargetPlate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelBearingInt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelLeftIR;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelRightIR;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelBackIR;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label labelFrontIR;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label labelServoR;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label labelServoL;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelServoLPos;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label labelServoRPos;
        private System.Windows.Forms.PictureBox pictureBoxMech;
    }
}

