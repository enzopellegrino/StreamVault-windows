namespace StreamVault.Forms
{
    partial class FFmpegDebugDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxInfo = new System.Windows.Forms.GroupBox();
            this.labelFFmpegPath = new System.Windows.Forms.Label();
            this.labelFFmpegVersion = new System.Windows.Forms.Label();
            this.labelFFmpegStatus = new System.Windows.Forms.Label();
            this.groupBoxTest = new System.Windows.Forms.GroupBox();
            this.buttonTestCapture = new System.Windows.Forms.Button();
            this.buttonTestSRT = new System.Windows.Forms.Button();
            this.buttonShowDevices = new System.Windows.Forms.Button();
            this.groupBoxOutput = new System.Windows.Forms.GroupBox();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonClearOutput = new System.Windows.Forms.Button();
            this.groupBoxInfo.SuspendLayout();
            this.groupBoxTest.SuspendLayout();
            this.groupBoxOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Controls.Add(this.labelFFmpegPath);
            this.groupBoxInfo.Controls.Add(this.labelFFmpegVersion);
            this.groupBoxInfo.Controls.Add(this.labelFFmpegStatus);
            this.groupBoxInfo.Location = new System.Drawing.Point(12, 12);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Size = new System.Drawing.Size(560, 100);
            this.groupBoxInfo.TabIndex = 0;
            this.groupBoxInfo.TabStop = false;
            this.groupBoxInfo.Text = "FFmpeg Information";
            // 
            // labelFFmpegStatus
            // 
            this.labelFFmpegStatus.AutoSize = true;
            this.labelFFmpegStatus.Location = new System.Drawing.Point(15, 25);
            this.labelFFmpegStatus.Name = "labelFFmpegStatus";
            this.labelFFmpegStatus.Size = new System.Drawing.Size(42, 15);
            this.labelFFmpegStatus.TabIndex = 0;
            this.labelFFmpegStatus.Text = "Status: Checking...";
            // 
            // labelFFmpegVersion
            // 
            this.labelFFmpegVersion.AutoSize = true;
            this.labelFFmpegVersion.Location = new System.Drawing.Point(15, 45);
            this.labelFFmpegVersion.Name = "labelFFmpegVersion";
            this.labelFFmpegVersion.Size = new System.Drawing.Size(51, 15);
            this.labelFFmpegVersion.TabIndex = 1;
            this.labelFFmpegVersion.Text = "Version: Unknown";
            // 
            // labelFFmpegPath
            // 
            this.labelFFmpegPath.AutoSize = true;
            this.labelFFmpegPath.Location = new System.Drawing.Point(15, 65);
            this.labelFFmpegPath.Name = "labelFFmpegPath";
            this.labelFFmpegPath.Size = new System.Drawing.Size(35, 15);
            this.labelFFmpegPath.TabIndex = 2;
            this.labelFFmpegPath.Text = "Path: Unknown";
            // 
            // groupBoxTest
            // 
            this.groupBoxTest.Controls.Add(this.buttonShowDevices);
            this.groupBoxTest.Controls.Add(this.buttonTestSRT);
            this.groupBoxTest.Controls.Add(this.buttonTestCapture);
            this.groupBoxTest.Location = new System.Drawing.Point(12, 118);
            this.groupBoxTest.Name = "groupBoxTest";
            this.groupBoxTest.Size = new System.Drawing.Size(560, 60);
            this.groupBoxTest.TabIndex = 1;
            this.groupBoxTest.TabStop = false;
            this.groupBoxTest.Text = "Test Commands";
            // 
            // buttonTestCapture
            // 
            this.buttonTestCapture.Location = new System.Drawing.Point(15, 25);
            this.buttonTestCapture.Name = "buttonTestCapture";
            this.buttonTestCapture.Size = new System.Drawing.Size(100, 25);
            this.buttonTestCapture.TabIndex = 0;
            this.buttonTestCapture.Text = "Test Capture";
            this.buttonTestCapture.UseVisualStyleBackColor = true;
            this.buttonTestCapture.Click += new System.EventHandler(this.ButtonTestCapture_Click);
            // 
            // buttonTestSRT
            // 
            this.buttonTestSRT.Location = new System.Drawing.Point(125, 25);
            this.buttonTestSRT.Name = "buttonTestSRT";
            this.buttonTestSRT.Size = new System.Drawing.Size(100, 25);
            this.buttonTestSRT.TabIndex = 1;
            this.buttonTestSRT.Text = "Test SRT";
            this.buttonTestSRT.UseVisualStyleBackColor = true;
            this.buttonTestSRT.Click += new System.EventHandler(this.ButtonTestSRT_Click);
            // 
            // buttonShowDevices
            // 
            this.buttonShowDevices.Location = new System.Drawing.Point(235, 25);
            this.buttonShowDevices.Name = "buttonShowDevices";
            this.buttonShowDevices.Size = new System.Drawing.Size(100, 25);
            this.buttonShowDevices.TabIndex = 2;
            this.buttonShowDevices.Text = "Show Devices";
            this.buttonShowDevices.UseVisualStyleBackColor = true;
            this.buttonShowDevices.Click += new System.EventHandler(this.ButtonShowDevices_Click);
            // 
            // groupBoxOutput
            // 
            this.groupBoxOutput.Controls.Add(this.textBoxOutput);
            this.groupBoxOutput.Location = new System.Drawing.Point(12, 184);
            this.groupBoxOutput.Name = "groupBoxOutput";
            this.groupBoxOutput.Size = new System.Drawing.Size(560, 300);
            this.groupBoxOutput.TabIndex = 2;
            this.groupBoxOutput.TabStop = false;
            this.groupBoxOutput.Text = "Debug Output";
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Font = new System.Drawing.Font("Consolas", 9F);
            this.textBoxOutput.Location = new System.Drawing.Point(15, 25);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ReadOnly = true;
            this.textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxOutput.Size = new System.Drawing.Size(530, 260);
            this.textBoxOutput.TabIndex = 0;
            // 
            // buttonClearOutput
            // 
            this.buttonClearOutput.Location = new System.Drawing.Point(12, 495);
            this.buttonClearOutput.Name = "buttonClearOutput";
            this.buttonClearOutput.Size = new System.Drawing.Size(100, 30);
            this.buttonClearOutput.TabIndex = 3;
            this.buttonClearOutput.Text = "Clear Output";
            this.buttonClearOutput.UseVisualStyleBackColor = true;
            this.buttonClearOutput.Click += new System.EventHandler(this.ButtonClearOutput_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(472, 495);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(100, 30);
            this.buttonClose.TabIndex = 4;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // FFmpegDebugDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 537);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonClearOutput);
            this.Controls.Add(this.groupBoxOutput);
            this.Controls.Add(this.groupBoxTest);
            this.Controls.Add(this.groupBoxInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FFmpegDebugDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FFmpeg Debug Console";
            this.groupBoxInfo.ResumeLayout(false);
            this.groupBoxInfo.PerformLayout();
            this.groupBoxTest.ResumeLayout(false);
            this.groupBoxOutput.ResumeLayout(false);
            this.groupBoxOutput.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxInfo;
        private System.Windows.Forms.Label labelFFmpegPath;
        private System.Windows.Forms.Label labelFFmpegVersion;
        private System.Windows.Forms.Label labelFFmpegStatus;
        private System.Windows.Forms.GroupBox groupBoxTest;
        private System.Windows.Forms.Button buttonShowDevices;
        private System.Windows.Forms.Button buttonTestSRT;
        private System.Windows.Forms.Button buttonTestCapture;
        private System.Windows.Forms.GroupBox groupBoxOutput;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonClearOutput;
    }
}
