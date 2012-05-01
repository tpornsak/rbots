namespace FormVLC
{
    partial class FormVLC
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
            this.panelVLC = new System.Windows.Forms.Panel();
            this.timerInput = new System.Windows.Forms.Timer(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.buttonRight = new System.Windows.Forms.CheckBox();
            this.buttonLeft = new System.Windows.Forms.CheckBox();
            this.buttonDown = new System.Windows.Forms.CheckBox();
            this.buttonUp = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.rightMotor = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.leftMotor = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonLeftShoulder = new System.Windows.Forms.CheckBox();
            this.buttonRightShoulder = new System.Windows.Forms.CheckBox();
            this.buttonStart = new System.Windows.Forms.CheckBox();
            this.buttonBack = new System.Windows.Forms.CheckBox();
            this.buttonRightStick = new System.Windows.Forms.CheckBox();
            this.buttonLeftStick = new System.Windows.Forms.CheckBox();
            this.buttonY = new System.Windows.Forms.CheckBox();
            this.buttonX = new System.Windows.Forms.CheckBox();
            this.buttonB = new System.Windows.Forms.CheckBox();
            this.buttonA = new System.Windows.Forms.CheckBox();
            this.lblNotConnected = new System.Windows.Forms.Label();
            this.ddlController = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rightTriggerPosition = new System.Windows.Forms.ProgressBar();
            this.label6 = new System.Windows.Forms.Label();
            this.leftTriggerPosition = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.y2position = new System.Windows.Forms.ProgressBar();
            this.x2position = new System.Windows.Forms.ProgressBar();
            this.y1Position = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.x1Position = new System.Windows.Forms.ProgressBar();
            this.comboBoxSerial = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.serialPortMech = new System.IO.Ports.SerialPort(this.components);
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rightMotor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftMotor)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelVLC
            // 
            this.panelVLC.Location = new System.Drawing.Point(37, 12);
            this.panelVLC.Name = "panelVLC";
            this.panelVLC.Size = new System.Drawing.Size(78, 67);
            this.panelVLC.TabIndex = 0;
            // 
            // timerInput
            // 
            this.timerInput.Enabled = true;
            this.timerInput.Interval = 50;
            this.timerInput.Tick += new System.EventHandler(this.timerInput_Tick);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.buttonRight);
            this.groupBox4.Controls.Add(this.buttonLeft);
            this.groupBox4.Controls.Add(this.buttonDown);
            this.groupBox4.Controls.Add(this.buttonUp);
            this.groupBox4.Location = new System.Drawing.Point(570, 440);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(208, 114);
            this.groupBox4.TabIndex = 21;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "D-Pad";
            // 
            // buttonRight
            // 
            this.buttonRight.AutoSize = true;
            this.buttonRight.Location = new System.Drawing.Point(125, 54);
            this.buttonRight.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRight.Name = "buttonRight";
            this.buttonRight.Size = new System.Drawing.Size(63, 21);
            this.buttonRight.TabIndex = 3;
            this.buttonRight.Text = "Right";
            this.buttonRight.UseVisualStyleBackColor = true;
            // 
            // buttonLeft
            // 
            this.buttonLeft.AutoSize = true;
            this.buttonLeft.Location = new System.Drawing.Point(21, 54);
            this.buttonLeft.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLeft.Name = "buttonLeft";
            this.buttonLeft.Size = new System.Drawing.Size(54, 21);
            this.buttonLeft.TabIndex = 2;
            this.buttonLeft.Text = "Left";
            this.buttonLeft.UseVisualStyleBackColor = true;
            // 
            // buttonDown
            // 
            this.buttonDown.AutoSize = true;
            this.buttonDown.Location = new System.Drawing.Point(75, 86);
            this.buttonDown.Margin = new System.Windows.Forms.Padding(4);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(65, 21);
            this.buttonDown.TabIndex = 1;
            this.buttonDown.Text = "Down";
            this.buttonDown.UseVisualStyleBackColor = true;
            // 
            // buttonUp
            // 
            this.buttonUp.AutoSize = true;
            this.buttonUp.Location = new System.Drawing.Point(75, 23);
            this.buttonUp.Margin = new System.Windows.Forms.Padding(4);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(48, 21);
            this.buttonUp.TabIndex = 0;
            this.buttonUp.Text = "Up";
            this.buttonUp.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.rightMotor);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.leftMotor);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Location = new System.Drawing.Point(178, 440);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(384, 117);
            this.groupBox3.TabIndex = 20;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Vibration Motors";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(32, 86);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 14;
            this.button1.Text = "&Vibrate";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // rightMotor
            // 
            this.rightMotor.DecimalPlaces = 1;
            this.rightMotor.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.rightMotor.Location = new System.Drawing.Point(203, 57);
            this.rightMotor.Margin = new System.Windows.Forms.Padding(4);
            this.rightMotor.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.rightMotor.Name = "rightMotor";
            this.rightMotor.Size = new System.Drawing.Size(139, 22);
            this.rightMotor.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(25, 57);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(168, 17);
            this.label8.TabIndex = 1;
            this.label8.Text = "Medium Frequency Motor";
            // 
            // leftMotor
            // 
            this.leftMotor.DecimalPlaces = 1;
            this.leftMotor.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.leftMotor.Location = new System.Drawing.Point(205, 25);
            this.leftMotor.Margin = new System.Windows.Forms.Padding(4);
            this.leftMotor.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.leftMotor.Name = "leftMotor";
            this.leftMotor.Size = new System.Drawing.Size(136, 22);
            this.leftMotor.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 25);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(144, 17);
            this.label7.TabIndex = 0;
            this.label7.Text = "Low Frequency Motor";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonLeftShoulder);
            this.groupBox1.Controls.Add(this.buttonRightShoulder);
            this.groupBox1.Controls.Add(this.buttonStart);
            this.groupBox1.Controls.Add(this.buttonBack);
            this.groupBox1.Controls.Add(this.buttonRightStick);
            this.groupBox1.Controls.Add(this.buttonLeftStick);
            this.groupBox1.Controls.Add(this.buttonY);
            this.groupBox1.Controls.Add(this.buttonX);
            this.groupBox1.Controls.Add(this.buttonB);
            this.groupBox1.Controls.Add(this.buttonA);
            this.groupBox1.Location = new System.Drawing.Point(178, 85);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(195, 347);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Digital Inputs";
            // 
            // buttonLeftShoulder
            // 
            this.buttonLeftShoulder.AutoSize = true;
            this.buttonLeftShoulder.Location = new System.Drawing.Point(9, 138);
            this.buttonLeftShoulder.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLeftShoulder.Name = "buttonLeftShoulder";
            this.buttonLeftShoulder.Size = new System.Drawing.Size(115, 21);
            this.buttonLeftShoulder.TabIndex = 19;
            this.buttonLeftShoulder.Text = "Left Shoulder";
            this.buttonLeftShoulder.UseVisualStyleBackColor = true;
            // 
            // buttonRightShoulder
            // 
            this.buttonRightShoulder.AutoSize = true;
            this.buttonRightShoulder.Location = new System.Drawing.Point(9, 166);
            this.buttonRightShoulder.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRightShoulder.Name = "buttonRightShoulder";
            this.buttonRightShoulder.Size = new System.Drawing.Size(124, 21);
            this.buttonRightShoulder.TabIndex = 18;
            this.buttonRightShoulder.Text = "Right Shoulder";
            this.buttonRightShoulder.UseVisualStyleBackColor = true;
            // 
            // buttonStart
            // 
            this.buttonStart.AutoSize = true;
            this.buttonStart.Location = new System.Drawing.Point(9, 194);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(4);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(105, 21);
            this.buttonStart.TabIndex = 17;
            this.buttonStart.Text = "Start Button";
            this.buttonStart.UseVisualStyleBackColor = true;
            // 
            // buttonBack
            // 
            this.buttonBack.AutoSize = true;
            this.buttonBack.Location = new System.Drawing.Point(9, 223);
            this.buttonBack.Margin = new System.Windows.Forms.Padding(4);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(106, 21);
            this.buttonBack.TabIndex = 16;
            this.buttonBack.Text = "Back Button";
            this.buttonBack.UseVisualStyleBackColor = true;
            // 
            // buttonRightStick
            // 
            this.buttonRightStick.AutoSize = true;
            this.buttonRightStick.Location = new System.Drawing.Point(8, 279);
            this.buttonRightStick.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRightStick.Name = "buttonRightStick";
            this.buttonRightStick.Size = new System.Drawing.Size(97, 21);
            this.buttonRightStick.TabIndex = 15;
            this.buttonRightStick.Text = "Right Stick";
            this.buttonRightStick.UseVisualStyleBackColor = true;
            // 
            // buttonLeftStick
            // 
            this.buttonLeftStick.AutoSize = true;
            this.buttonLeftStick.Location = new System.Drawing.Point(9, 251);
            this.buttonLeftStick.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLeftStick.Name = "buttonLeftStick";
            this.buttonLeftStick.Size = new System.Drawing.Size(88, 21);
            this.buttonLeftStick.TabIndex = 14;
            this.buttonLeftStick.Text = "Left Stick";
            this.buttonLeftStick.UseVisualStyleBackColor = true;
            // 
            // buttonY
            // 
            this.buttonY.AutoSize = true;
            this.buttonY.Location = new System.Drawing.Point(8, 111);
            this.buttonY.Margin = new System.Windows.Forms.Padding(4);
            this.buttonY.Name = "buttonY";
            this.buttonY.Size = new System.Drawing.Size(84, 21);
            this.buttonY.TabIndex = 13;
            this.buttonY.Text = "Y-button";
            this.buttonY.UseVisualStyleBackColor = true;
            // 
            // buttonX
            // 
            this.buttonX.AutoSize = true;
            this.buttonX.Location = new System.Drawing.Point(8, 82);
            this.buttonX.Margin = new System.Windows.Forms.Padding(4);
            this.buttonX.Name = "buttonX";
            this.buttonX.Size = new System.Drawing.Size(84, 21);
            this.buttonX.TabIndex = 12;
            this.buttonX.Text = "X-button";
            this.buttonX.UseVisualStyleBackColor = true;
            // 
            // buttonB
            // 
            this.buttonB.AutoSize = true;
            this.buttonB.Location = new System.Drawing.Point(8, 54);
            this.buttonB.Margin = new System.Windows.Forms.Padding(4);
            this.buttonB.Name = "buttonB";
            this.buttonB.Size = new System.Drawing.Size(85, 21);
            this.buttonB.TabIndex = 11;
            this.buttonB.Text = "B-Button";
            this.buttonB.UseVisualStyleBackColor = true;
            // 
            // buttonA
            // 
            this.buttonA.AutoSize = true;
            this.buttonA.Location = new System.Drawing.Point(8, 26);
            this.buttonA.Margin = new System.Windows.Forms.Padding(4);
            this.buttonA.Name = "buttonA";
            this.buttonA.Size = new System.Drawing.Size(85, 21);
            this.buttonA.TabIndex = 10;
            this.buttonA.Text = "A-Button";
            this.buttonA.UseVisualStyleBackColor = true;
            // 
            // lblNotConnected
            // 
            this.lblNotConnected.AutoSize = true;
            this.lblNotConnected.ForeColor = System.Drawing.Color.Red;
            this.lblNotConnected.Location = new System.Drawing.Point(471, 35);
            this.lblNotConnected.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNotConnected.Name = "lblNotConnected";
            this.lblNotConnected.Size = new System.Drawing.Size(173, 17);
            this.lblNotConnected.TabIndex = 22;
            this.lblNotConnected.Text = "Controller Is Disconnected";
            // 
            // ddlController
            // 
            this.ddlController.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlController.FormattingEnabled = true;
            this.ddlController.Items.AddRange(new object[] {
            "Player 1",
            "Player 2",
            "Player 3",
            "Player 4"});
            this.ddlController.Location = new System.Drawing.Point(287, 35);
            this.ddlController.Margin = new System.Windows.Forms.Padding(4);
            this.ddlController.Name = "ddlController";
            this.ddlController.Size = new System.Drawing.Size(175, 24);
            this.ddlController.TabIndex = 21;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(191, 35);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 17);
            this.label9.TabIndex = 20;
            this.label9.Text = "Controller";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rightTriggerPosition);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.leftTriggerPosition);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.y2position);
            this.groupBox2.Controls.Add(this.x2position);
            this.groupBox2.Controls.Add(this.y1Position);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.x1Position);
            this.groupBox2.Location = new System.Drawing.Point(427, 85);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(395, 341);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Analogue Inputs";
            // 
            // rightTriggerPosition
            // 
            this.rightTriggerPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rightTriggerPosition.Location = new System.Drawing.Point(8, 299);
            this.rightTriggerPosition.Margin = new System.Windows.Forms.Padding(4);
            this.rightTriggerPosition.Name = "rightTriggerPosition";
            this.rightTriggerPosition.Size = new System.Drawing.Size(379, 28);
            this.rightTriggerPosition.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 279);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "Right Trigger";
            // 
            // leftTriggerPosition
            // 
            this.leftTriggerPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.leftTriggerPosition.Location = new System.Drawing.Point(8, 247);
            this.leftTriggerPosition.Margin = new System.Windows.Forms.Padding(4);
            this.leftTriggerPosition.MarqueeAnimationSpeed = 25;
            this.leftTriggerPosition.Name = "leftTriggerPosition";
            this.leftTriggerPosition.Size = new System.Drawing.Size(379, 28);
            this.leftTriggerPosition.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 228);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "Left Trigger";
            // 
            // y2position
            // 
            this.y2position.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.y2position.Location = new System.Drawing.Point(8, 196);
            this.y2position.Margin = new System.Windows.Forms.Padding(4);
            this.y2position.MarqueeAnimationSpeed = 25;
            this.y2position.Name = "y2position";
            this.y2position.Size = new System.Drawing.Size(379, 28);
            this.y2position.TabIndex = 7;
            // 
            // x2position
            // 
            this.x2position.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.x2position.Location = new System.Drawing.Point(8, 144);
            this.x2position.Margin = new System.Windows.Forms.Padding(4);
            this.x2position.MarqueeAnimationSpeed = 25;
            this.x2position.Name = "x2position";
            this.x2position.Size = new System.Drawing.Size(379, 28);
            this.x2position.TabIndex = 6;
            // 
            // y1Position
            // 
            this.y1Position.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.y1Position.Location = new System.Drawing.Point(8, 91);
            this.y1Position.Margin = new System.Windows.Forms.Padding(4);
            this.y1Position.Name = "y1Position";
            this.y1Position.Size = new System.Drawing.Size(379, 28);
            this.y1Position.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 176);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 17);
            this.label4.TabIndex = 4;
            this.label4.Text = "Y-Axis 2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 123);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "X-Axis 2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 71);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Y-Axis 1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "X-Axis 1";
            // 
            // x1Position
            // 
            this.x1Position.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.x1Position.Location = new System.Drawing.Point(8, 39);
            this.x1Position.Margin = new System.Windows.Forms.Padding(4);
            this.x1Position.Name = "x1Position";
            this.x1Position.Size = new System.Drawing.Size(379, 28);
            this.x1Position.TabIndex = 0;
            // 
            // comboBoxSerial
            // 
            this.comboBoxSerial.FormattingEnabled = true;
            this.comboBoxSerial.Location = new System.Drawing.Point(992, 35);
            this.comboBoxSerial.Name = "comboBoxSerial";
            this.comboBoxSerial.Size = new System.Drawing.Size(152, 24);
            this.comboBoxSerial.TabIndex = 24;
            this.comboBoxSerial.SelectedIndexChanged += new System.EventHandler(this.comboBoxSerial_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(894, 38);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 17);
            this.label10.TabIndex = 25;
            this.label10.Text = "Serial Port";
            // 
            // serialPortMech
            // 
            this.serialPortMech.BaudRate = 38400;
            this.serialPortMech.PortName = "COM5";
            // 
            // FormVLC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1231, 607);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.comboBoxSerial);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblNotConnected);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ddlController);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.panelVLC);
            this.Name = "FormVLC";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormVLC_FormClosing);
            this.Load += new System.EventHandler(this.FormVLC_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rightMotor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftMotor)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelVLC;
        private System.Windows.Forms.Timer timerInput;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox buttonRight;
        private System.Windows.Forms.CheckBox buttonLeft;
        private System.Windows.Forms.CheckBox buttonDown;
        private System.Windows.Forms.CheckBox buttonUp;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown rightMotor;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown leftMotor;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox buttonLeftShoulder;
        private System.Windows.Forms.CheckBox buttonRightShoulder;
        private System.Windows.Forms.CheckBox buttonStart;
        private System.Windows.Forms.CheckBox buttonBack;
        private System.Windows.Forms.CheckBox buttonRightStick;
        private System.Windows.Forms.CheckBox buttonLeftStick;
        private System.Windows.Forms.CheckBox buttonY;
        private System.Windows.Forms.CheckBox buttonX;
        private System.Windows.Forms.CheckBox buttonB;
        private System.Windows.Forms.CheckBox buttonA;
        private System.Windows.Forms.Label lblNotConnected;
        private System.Windows.Forms.ComboBox ddlController;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar rightTriggerPosition;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ProgressBar leftTriggerPosition;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ProgressBar y2position;
        private System.Windows.Forms.ProgressBar x2position;
        private System.Windows.Forms.ProgressBar y1Position;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar x1Position;
        private System.Windows.Forms.ComboBox comboBoxSerial;
        private System.Windows.Forms.Label label10;
        private System.IO.Ports.SerialPort serialPortMech;
    }
}

