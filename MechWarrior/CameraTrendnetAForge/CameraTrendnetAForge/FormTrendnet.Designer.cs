namespace CameraTrendnetAForge
{
    partial class FormTrendnet
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
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxDebug
            // 
            this.textBoxDebug.Location = new System.Drawing.Point(618, 358);
            this.textBoxDebug.Multiline = true;
            this.textBoxDebug.Name = "textBoxDebug";
            this.textBoxDebug.Size = new System.Drawing.Size(358, 134);
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
            this.toolStrip1.Location = new System.Drawing.Point(0, 505);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(988, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabelFPS
            // 
            this.toolStripLabelFPS.Name = "toolStripLabelFPS";
            this.toolStripLabelFPS.Size = new System.Drawing.Size(78, 22);
            this.toolStripLabelFPS.Text = "toolStripLabel1";
            // 
            // FormTrendnet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(988, 530);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.mechCamera);
            this.Controls.Add(this.labelDebugLog);
            this.Controls.Add(this.textBoxDebug);
            this.Name = "FormTrendnet";
            this.Text = "Mech Warrior Vision";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTrendnet_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
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
    }
}

