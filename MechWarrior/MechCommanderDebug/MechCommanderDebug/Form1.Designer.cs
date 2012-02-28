namespace WindowsFormsApplication1
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxHeader = new System.Windows.Forms.TextBox();
            this.textBoxRVert = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxRHorz = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxLVert = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxLHorz = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxButton = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxExt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxChecksum = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.serialPortMech = new System.IO.Ports.SerialPort(this.components);
            this.textBoxDebug = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Header";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // textBoxHeader
            // 
            this.textBoxHeader.Location = new System.Drawing.Point(12, 53);
            this.textBoxHeader.Name = "textBoxHeader";
            this.textBoxHeader.ReadOnly = true;
            this.textBoxHeader.Size = new System.Drawing.Size(42, 20);
            this.textBoxHeader.TabIndex = 1;
            this.textBoxHeader.Text = "255";
            // 
            // textBoxRVert
            // 
            this.textBoxRVert.Location = new System.Drawing.Point(69, 53);
            this.textBoxRVert.Name = "textBoxRVert";
            this.textBoxRVert.Size = new System.Drawing.Size(42, 20);
            this.textBoxRVert.TabIndex = 3;
            this.textBoxRVert.Text = "128";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(69, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "R Vert";
            // 
            // textBoxRHorz
            // 
            this.textBoxRHorz.Location = new System.Drawing.Point(128, 53);
            this.textBoxRHorz.Name = "textBoxRHorz";
            this.textBoxRHorz.Size = new System.Drawing.Size(42, 20);
            this.textBoxRHorz.TabIndex = 5;
            this.textBoxRHorz.Text = "128";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(128, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "R Horz";
            // 
            // textBoxLVert
            // 
            this.textBoxLVert.Location = new System.Drawing.Point(187, 53);
            this.textBoxLVert.Name = "textBoxLVert";
            this.textBoxLVert.Size = new System.Drawing.Size(42, 20);
            this.textBoxLVert.TabIndex = 7;
            this.textBoxLVert.Text = "128";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(187, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "L Vert";
            // 
            // textBoxLHorz
            // 
            this.textBoxLHorz.Location = new System.Drawing.Point(246, 53);
            this.textBoxLHorz.Name = "textBoxLHorz";
            this.textBoxLHorz.Size = new System.Drawing.Size(42, 20);
            this.textBoxLHorz.TabIndex = 9;
            this.textBoxLHorz.Text = "128";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(246, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "L Horz";
            // 
            // textBoxButton
            // 
            this.textBoxButton.Location = new System.Drawing.Point(305, 53);
            this.textBoxButton.Name = "textBoxButton";
            this.textBoxButton.Size = new System.Drawing.Size(42, 20);
            this.textBoxButton.TabIndex = 11;
            this.textBoxButton.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(305, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Button";
            // 
            // textBoxExt
            // 
            this.textBoxExt.Location = new System.Drawing.Point(362, 53);
            this.textBoxExt.Name = "textBoxExt";
            this.textBoxExt.Size = new System.Drawing.Size(42, 20);
            this.textBoxExt.TabIndex = 13;
            this.textBoxExt.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(362, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Extended";
            // 
            // textBoxChecksum
            // 
            this.textBoxChecksum.Location = new System.Drawing.Point(420, 53);
            this.textBoxChecksum.Name = "textBoxChecksum";
            this.textBoxChecksum.ReadOnly = true;
            this.textBoxChecksum.Size = new System.Drawing.Size(42, 20);
            this.textBoxChecksum.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(420, 27);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Checksum";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 104);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // serialPortMech
            // 
            this.serialPortMech.BaudRate = 38400;
            this.serialPortMech.PortName = "COM11";
            // 
            // textBoxDebug
            // 
            this.textBoxDebug.Location = new System.Drawing.Point(22, 173);
            this.textBoxDebug.Multiline = true;
            this.textBoxDebug.Name = "textBoxDebug";
            this.textBoxDebug.Size = new System.Drawing.Size(399, 77);
            this.textBoxDebug.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 146);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Debug Log";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 272);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxDebug);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxChecksum);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxExt);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxLHorz);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxLVert);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxRHorz);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxRVert);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxHeader);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Mech Commander";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxHeader;
        private System.Windows.Forms.TextBox textBoxRVert;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxRHorz;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxLVert;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxLHorz;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxExt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxChecksum;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button1;
        private System.IO.Ports.SerialPort serialPortMech;
        private System.Windows.Forms.TextBox textBoxDebug;
        private System.Windows.Forms.Label label9;
    }
}

