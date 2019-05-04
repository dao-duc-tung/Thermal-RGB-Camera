namespace Chat_Edison_Win
{
    partial class fClient
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fClient));
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.txtMain = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtTemperatureThreshold = new System.Windows.Forms.TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnSendThermalCmd = new System.Windows.Forms.Button();
            this.btnSendRGBCmd = new System.Windows.Forms.Button();
            this.btnSendBothImageCmd = new System.Windows.Forms.Button();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblMODE = new System.Windows.Forms.Label();
            this.lblWarning = new System.Windows.Forms.Label();
            this.lblTemperature = new System.Windows.Forms.Label();
            this.cbIPServer = new System.Windows.Forms.ComboBox();
            this.btnNote = new System.Windows.Forms.Button();
            this.numBalanceTemperature = new System.Windows.Forms.NumericUpDown();
            this.btnAutoCapture = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblAutoCapture = new System.Windows.Forms.Label();
            this.btnAutoChangeMode = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.lblAutoMode = new System.Windows.Forms.Label();
            this.btnAddIPEdison = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBalanceTemperature)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(217, 207);
            this.txtPort.Name = "txtPort";
            this.txtPort.ReadOnly = true;
            this.txtPort.Size = new System.Drawing.Size(32, 20);
            this.txtPort.TabIndex = 26;
            this.txtPort.Text = "8888";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(185, 211);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Port";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.ForeColor = System.Drawing.Color.Red;
            this.btnDisconnect.Location = new System.Drawing.Point(255, 205);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(77, 50);
            this.btnDisconnect.TabIndex = 30;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // txtMain
            // 
            this.txtMain.Location = new System.Drawing.Point(12, 234);
            this.txtMain.Multiline = true;
            this.txtMain.Name = "txtMain";
            this.txtMain.ReadOnly = true;
            this.txtMain.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMain.Size = new System.Drawing.Size(237, 76);
            this.txtMain.TabIndex = 28;
            // 
            // btnConnect
            // 
            this.btnConnect.ForeColor = System.Drawing.Color.Blue;
            this.btnConnect.Location = new System.Drawing.Point(255, 261);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(77, 49);
            this.btnConnect.TabIndex = 27;
            this.btnConnect.Text = "CONNECT";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 211);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "IP Server";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(338, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(880, 660);
            this.pictureBox1.TabIndex = 34;
            this.pictureBox1.TabStop = false;
            // 
            // txtTemperatureThreshold
            // 
            this.txtTemperatureThreshold.ForeColor = System.Drawing.Color.Red;
            this.txtTemperatureThreshold.Location = new System.Drawing.Point(287, 379);
            this.txtTemperatureThreshold.Name = "txtTemperatureThreshold";
            this.txtTemperatureThreshold.Size = new System.Drawing.Size(45, 20);
            this.txtTemperatureThreshold.TabIndex = 35;
            this.txtTemperatureThreshold.Text = "38";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(12, 7);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(320, 194);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 37;
            this.pictureBox2.TabStop = false;
            // 
            // btnSendThermalCmd
            // 
            this.btnSendThermalCmd.ForeColor = System.Drawing.Color.Blue;
            this.btnSendThermalCmd.Location = new System.Drawing.Point(12, 316);
            this.btnSendThermalCmd.Name = "btnSendThermalCmd";
            this.btnSendThermalCmd.Size = new System.Drawing.Size(75, 51);
            this.btnSendThermalCmd.TabIndex = 38;
            this.btnSendThermalCmd.Text = "Capture Thermal Image";
            this.btnSendThermalCmd.UseVisualStyleBackColor = true;
            this.btnSendThermalCmd.Click += new System.EventHandler(this.btnSendThermalCmd_Click);
            // 
            // btnSendRGBCmd
            // 
            this.btnSendRGBCmd.Location = new System.Drawing.Point(12, 399);
            this.btnSendRGBCmd.Name = "btnSendRGBCmd";
            this.btnSendRGBCmd.Size = new System.Drawing.Size(85, 21);
            this.btnSendRGBCmd.TabIndex = 39;
            this.btnSendRGBCmd.Text = "Capture RGB Image";
            this.btnSendRGBCmd.UseVisualStyleBackColor = true;
            this.btnSendRGBCmd.Visible = false;
            this.btnSendRGBCmd.Click += new System.EventHandler(this.btnSendRGBCmd_Click);
            // 
            // btnSendBothImageCmd
            // 
            this.btnSendBothImageCmd.ForeColor = System.Drawing.Color.Blue;
            this.btnSendBothImageCmd.Location = new System.Drawing.Point(93, 316);
            this.btnSendBothImageCmd.Name = "btnSendBothImageCmd";
            this.btnSendBothImageCmd.Size = new System.Drawing.Size(75, 51);
            this.btnSendBothImageCmd.TabIndex = 40;
            this.btnSendBothImageCmd.Text = "Capture Both Image";
            this.btnSendBothImageCmd.UseVisualStyleBackColor = true;
            this.btnSendBothImageCmd.Click += new System.EventHandler(this.btnSendBothImageCmd_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new System.Drawing.Point(12, 426);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(320, 241);
            this.pictureBox3.TabIndex = 41;
            this.pictureBox3.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(12, 382);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 42;
            this.label4.Text = "Mode";
            // 
            // lblMODE
            // 
            this.lblMODE.AutoSize = true;
            this.lblMODE.ForeColor = System.Drawing.Color.Red;
            this.lblMODE.Location = new System.Drawing.Point(47, 382);
            this.lblMODE.Name = "lblMODE";
            this.lblMODE.Size = new System.Drawing.Size(13, 13);
            this.lblMODE.TabIndex = 43;
            this.lblMODE.Text = "0";
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarning.ForeColor = System.Drawing.Color.Red;
            this.lblWarning.Location = new System.Drawing.Point(-1, 669);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(341, 73);
            this.lblWarning.TabIndex = 44;
            this.lblWarning.Text = "WARNING";
            // 
            // lblTemperature
            // 
            this.lblTemperature.AutoSize = true;
            this.lblTemperature.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTemperature.ForeColor = System.Drawing.Color.Blue;
            this.lblTemperature.Location = new System.Drawing.Point(1021, 669);
            this.lblTemperature.Name = "lblTemperature";
            this.lblTemperature.Size = new System.Drawing.Size(194, 73);
            this.lblTemperature.TabIndex = 45;
            this.lblTemperature.Text = "99.99";
            // 
            // cbIPServer
            // 
            this.cbIPServer.FormattingEnabled = true;
            this.cbIPServer.Items.AddRange(new object[] {
            "192.168.1.117",
            "192.168.42.1",
            "192.168.4.176",
            "192.168.2.15"});
            this.cbIPServer.Location = new System.Drawing.Point(66, 207);
            this.cbIPServer.Name = "cbIPServer";
            this.cbIPServer.Size = new System.Drawing.Size(113, 21);
            this.cbIPServer.TabIndex = 46;
            // 
            // btnNote
            // 
            this.btnNote.ForeColor = System.Drawing.Color.Red;
            this.btnNote.Location = new System.Drawing.Point(243, 400);
            this.btnNote.Name = "btnNote";
            this.btnNote.Size = new System.Drawing.Size(89, 20);
            this.btnNote.TabIndex = 47;
            this.btnNote.Text = "NOTE";
            this.btnNote.UseVisualStyleBackColor = true;
            this.btnNote.Click += new System.EventHandler(this.btnNote_Click);
            // 
            // numBalanceTemperature
            // 
            this.numBalanceTemperature.DecimalPlaces = 2;
            this.numBalanceTemperature.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numBalanceTemperature.Location = new System.Drawing.Point(194, 400);
            this.numBalanceTemperature.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numBalanceTemperature.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.numBalanceTemperature.Name = "numBalanceTemperature";
            this.numBalanceTemperature.Size = new System.Drawing.Size(43, 20);
            this.numBalanceTemperature.TabIndex = 48;
            // 
            // btnAutoCapture
            // 
            this.btnAutoCapture.ForeColor = System.Drawing.Color.Red;
            this.btnAutoCapture.Location = new System.Drawing.Point(174, 316);
            this.btnAutoCapture.Name = "btnAutoCapture";
            this.btnAutoCapture.Size = new System.Drawing.Size(75, 51);
            this.btnAutoCapture.TabIndex = 49;
            this.btnAutoCapture.Text = "Auto Capture";
            this.btnAutoCapture.UseVisualStyleBackColor = true;
            this.btnAutoCapture.Click += new System.EventHandler(this.btnAutoCapture_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(227, 382);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 50;
            this.label3.Text = "Threshold";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(66, 382);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 51;
            this.label5.Text = "Auto Cap";
            // 
            // lblAutoCapture
            // 
            this.lblAutoCapture.AutoSize = true;
            this.lblAutoCapture.ForeColor = System.Drawing.Color.Red;
            this.lblAutoCapture.Location = new System.Drawing.Point(123, 382);
            this.lblAutoCapture.Name = "lblAutoCapture";
            this.lblAutoCapture.Size = new System.Drawing.Size(13, 13);
            this.lblAutoCapture.TabIndex = 52;
            this.lblAutoCapture.Text = "0";
            // 
            // btnAutoChangeMode
            // 
            this.btnAutoChangeMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAutoChangeMode.ForeColor = System.Drawing.Color.Red;
            this.btnAutoChangeMode.Location = new System.Drawing.Point(255, 316);
            this.btnAutoChangeMode.Name = "btnAutoChangeMode";
            this.btnAutoChangeMode.Size = new System.Drawing.Size(77, 51);
            this.btnAutoChangeMode.TabIndex = 53;
            this.btnAutoChangeMode.Text = "Auto Mode When Over Threshold";
            this.btnAutoChangeMode.UseVisualStyleBackColor = true;
            this.btnAutoChangeMode.Click += new System.EventHandler(this.btnAutoChangeMode_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(142, 382);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 13);
            this.label6.TabIndex = 54;
            this.label6.Text = "Auto Mode";
            // 
            // lblAutoMode
            // 
            this.lblAutoMode.AutoSize = true;
            this.lblAutoMode.ForeColor = System.Drawing.Color.Red;
            this.lblAutoMode.Location = new System.Drawing.Point(207, 382);
            this.lblAutoMode.Name = "lblAutoMode";
            this.lblAutoMode.Size = new System.Drawing.Size(13, 13);
            this.lblAutoMode.TabIndex = 55;
            this.lblAutoMode.Text = "0";
            // 
            // btnAddIPEdison
            // 
            this.btnAddIPEdison.Location = new System.Drawing.Point(103, 399);
            this.btnAddIPEdison.Name = "btnAddIPEdison";
            this.btnAddIPEdison.Size = new System.Drawing.Size(85, 21);
            this.btnAddIPEdison.TabIndex = 56;
            this.btnAddIPEdison.Text = "Add IP Edison";
            this.btnAddIPEdison.UseVisualStyleBackColor = true;
            this.btnAddIPEdison.Click += new System.EventHandler(this.btnAddIPEdison_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(341, 669);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(684, 73);
            this.label7.TabIndex = 57;
            this.label7.Text = "NHIỆT ĐỘ CAO NHẤT";
            // 
            // fClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1229, 749);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnAddIPEdison);
            this.Controls.Add(this.lblAutoMode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnAutoChangeMode);
            this.Controls.Add(this.lblAutoCapture);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnAutoCapture);
            this.Controls.Add(this.numBalanceTemperature);
            this.Controls.Add(this.btnNote);
            this.Controls.Add(this.cbIPServer);
            this.Controls.Add(this.lblTemperature);
            this.Controls.Add(this.lblWarning);
            this.Controls.Add(this.lblMODE);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.btnSendBothImageCmd);
            this.Controls.Add(this.btnSendRGBCmd);
            this.Controls.Add(this.btnSendThermalCmd);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.txtTemperatureThreshold);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.txtMain);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "fClient";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VMIG 2016";
            this.Load += new System.EventHandler(this.fClient_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBalanceTemperature)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.TextBox txtMain;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtTemperatureThreshold;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnSendThermalCmd;
        private System.Windows.Forms.Button btnSendRGBCmd;
        private System.Windows.Forms.Button btnSendBothImageCmd;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblMODE;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.Label lblTemperature;
        private System.Windows.Forms.ComboBox cbIPServer;
        private System.Windows.Forms.Button btnNote;
        private System.Windows.Forms.NumericUpDown numBalanceTemperature;
        private System.Windows.Forms.Button btnAutoCapture;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblAutoCapture;
        private System.Windows.Forms.Button btnAutoChangeMode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblAutoMode;
        private System.Windows.Forms.Button btnAddIPEdison;
        private System.Windows.Forms.Label label7;
    }
}

