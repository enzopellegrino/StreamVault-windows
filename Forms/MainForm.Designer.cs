namespace StreamVault.Forms
{
    partial class MainForm
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
            this.groupBoxMonitor = new System.Windows.Forms.GroupBox();
            this.buttonRefreshMonitors = new System.Windows.Forms.Button();
            this.comboBoxMonitor = new System.Windows.Forms.ComboBox();
            this.labelMonitor = new System.Windows.Forms.Label();
            this.groupBoxVirtualMonitor = new System.Windows.Forms.GroupBox();
            this.buttonMultiStream = new System.Windows.Forms.Button();
            this.labelVirtualMonitorStatus = new System.Windows.Forms.Label();
            this.buttonRemoveVirtualMonitor = new System.Windows.Forms.Button();
            this.buttonCreateVirtualMonitor = new System.Windows.Forms.Button();
            this.groupBoxSettings = new System.Windows.Forms.GroupBox();
            this.labelBitrateUnit = new System.Windows.Forms.Label();
            this.labelFpsUnit = new System.Windows.Forms.Label();
            this.numericBitrate = new System.Windows.Forms.NumericUpDown();
            this.labelBitrate = new System.Windows.Forms.Label();
            this.numericFps = new System.Windows.Forms.NumericUpDown();
            this.labelFps = new System.Windows.Forms.Label();
            this.textBoxSrtUrl = new System.Windows.Forms.TextBox();
            this.labelSrtUrl = new System.Windows.Forms.Label();
            this.groupBoxStreaming = new System.Windows.Forms.GroupBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.buttonStartStop = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBoxMonitor.SuspendLayout();
            this.groupBoxVirtualMonitor.SuspendLayout();
            this.groupBoxSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericBitrate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericFps)).BeginInit();
            this.groupBoxStreaming.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxMonitor
            // 
            this.groupBoxMonitor.Controls.Add(this.buttonRefreshMonitors);
            this.groupBoxMonitor.Controls.Add(this.comboBoxMonitor);
            this.groupBoxMonitor.Controls.Add(this.labelMonitor);
            this.groupBoxMonitor.Location = new System.Drawing.Point(12, 12);
            this.groupBoxMonitor.Name = "groupBoxMonitor";
            this.groupBoxMonitor.Size = new System.Drawing.Size(460, 80);
            this.groupBoxMonitor.TabIndex = 0;
            this.groupBoxMonitor.TabStop = false;
            this.groupBoxMonitor.Text = "Monitor Selection";
            // 
            // buttonRefreshMonitors
            // 
            this.buttonRefreshMonitors.Location = new System.Drawing.Point(379, 35);
            this.buttonRefreshMonitors.Name = "buttonRefreshMonitors";
            this.buttonRefreshMonitors.Size = new System.Drawing.Size(75, 23);
            this.buttonRefreshMonitors.TabIndex = 2;
            this.buttonRefreshMonitors.Text = "Refresh";
            this.buttonRefreshMonitors.UseVisualStyleBackColor = true;
            this.buttonRefreshMonitors.Click += new System.EventHandler(this.ButtonRefreshMonitors_Click);
            // 
            // comboBoxMonitor
            // 
            this.comboBoxMonitor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMonitor.FormattingEnabled = true;
            this.comboBoxMonitor.Location = new System.Drawing.Point(70, 35);
            this.comboBoxMonitor.Name = "comboBoxMonitor";
            this.comboBoxMonitor.Size = new System.Drawing.Size(303, 23);
            this.comboBoxMonitor.TabIndex = 1;
            // 
            // labelMonitor
            // 
            this.labelMonitor.AutoSize = true;
            this.labelMonitor.Location = new System.Drawing.Point(6, 38);
            this.labelMonitor.Name = "labelMonitor";
            this.labelMonitor.Size = new System.Drawing.Size(51, 15);
            this.labelMonitor.TabIndex = 0;
            this.labelMonitor.Text = "Monitor:";
            // 
            // groupBoxVirtualMonitor
            // 
            this.groupBoxVirtualMonitor.Controls.Add(this.buttonMultiStream);
            this.groupBoxVirtualMonitor.Controls.Add(this.labelVirtualMonitorStatus);
            this.groupBoxVirtualMonitor.Controls.Add(this.buttonRemoveVirtualMonitor);
            this.groupBoxVirtualMonitor.Controls.Add(this.buttonCreateVirtualMonitor);
            this.groupBoxVirtualMonitor.Location = new System.Drawing.Point(12, 98);
            this.groupBoxVirtualMonitor.Name = "groupBoxVirtualMonitor";
            this.groupBoxVirtualMonitor.Size = new System.Drawing.Size(460, 80);
            this.groupBoxVirtualMonitor.TabIndex = 1;
            this.groupBoxVirtualMonitor.TabStop = false;
            this.groupBoxVirtualMonitor.Text = "Virtual Monitors & Multi-Stream";
            // 
            // labelVirtualMonitorStatus
            // 
            this.labelVirtualMonitorStatus.AutoSize = true;
            this.labelVirtualMonitorStatus.Location = new System.Drawing.Point(6, 55);
            this.labelVirtualMonitorStatus.Name = "labelVirtualMonitorStatus";
            this.labelVirtualMonitorStatus.Size = new System.Drawing.Size(39, 15);
            this.labelVirtualMonitorStatus.TabIndex = 2;
            this.labelVirtualMonitorStatus.Text = "Ready";
            // 
            // buttonRemoveVirtualMonitor
            // 
            this.buttonRemoveVirtualMonitor.BackColor = System.Drawing.Color.IndianRed;
            this.buttonRemoveVirtualMonitor.ForeColor = System.Drawing.Color.White;
            this.buttonRemoveVirtualMonitor.Location = new System.Drawing.Point(135, 22);
            this.buttonRemoveVirtualMonitor.Name = "buttonRemoveVirtualMonitor";
            this.buttonRemoveVirtualMonitor.Size = new System.Drawing.Size(120, 25);
            this.buttonRemoveVirtualMonitor.TabIndex = 1;
            this.buttonRemoveVirtualMonitor.Text = "Remove Virtual";
            this.buttonRemoveVirtualMonitor.UseVisualStyleBackColor = false;
            this.buttonRemoveVirtualMonitor.Click += new System.EventHandler(this.ButtonRemoveVirtualMonitor_Click);
            // 
            // buttonCreateVirtualMonitor
            // 
            this.buttonCreateVirtualMonitor.BackColor = System.Drawing.Color.LightGreen;
            this.buttonCreateVirtualMonitor.Location = new System.Drawing.Point(6, 22);
            this.buttonCreateVirtualMonitor.Name = "buttonCreateVirtualMonitor";
            this.buttonCreateVirtualMonitor.Size = new System.Drawing.Size(120, 25);
            this.buttonCreateVirtualMonitor.TabIndex = 0;
            this.buttonCreateVirtualMonitor.Text = "Create Virtual";
            this.buttonCreateVirtualMonitor.UseVisualStyleBackColor = false;
            this.buttonCreateVirtualMonitor.Click += new System.EventHandler(this.ButtonCreateVirtualMonitor_Click);
            // 
            // buttonMultiStream
            // 
            this.buttonMultiStream.BackColor = System.Drawing.Color.LightBlue;
            this.buttonMultiStream.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.buttonMultiStream.Location = new System.Drawing.Point(270, 22);
            this.buttonMultiStream.Name = "buttonMultiStream";
            this.buttonMultiStream.Size = new System.Drawing.Size(180, 25);
            this.buttonMultiStream.TabIndex = 3;
            this.buttonMultiStream.Text = "🎥 Multi-Monitor Streaming";
            this.buttonMultiStream.UseVisualStyleBackColor = false;
            this.buttonMultiStream.Click += new System.EventHandler(this.ButtonMultiStream_Click);
            // 
            // groupBoxSettings
            // 
            this.groupBoxSettings.Controls.Add(this.labelBitrateUnit);
            this.groupBoxSettings.Controls.Add(this.labelFpsUnit);
            this.groupBoxSettings.Controls.Add(this.numericBitrate);
            this.groupBoxSettings.Controls.Add(this.labelBitrate);
            this.groupBoxSettings.Controls.Add(this.numericFps);
            this.groupBoxSettings.Controls.Add(this.labelFps);
            this.groupBoxSettings.Controls.Add(this.textBoxSrtUrl);
            this.groupBoxSettings.Controls.Add(this.labelSrtUrl);
            this.groupBoxSettings.Location = new System.Drawing.Point(12, 184);
            this.groupBoxSettings.Name = "groupBoxSettings";
            this.groupBoxSettings.Size = new System.Drawing.Size(460, 140);
            this.groupBoxSettings.TabIndex = 2;
            this.groupBoxSettings.TabStop = false;
            this.groupBoxSettings.Text = "Streaming Settings";
            // 
            // labelBitrateUnit
            // 
            this.labelBitrateUnit.AutoSize = true;
            this.labelBitrateUnit.Location = new System.Drawing.Point(190, 108);
            this.labelBitrateUnit.Name = "labelBitrateUnit";
            this.labelBitrateUnit.Size = new System.Drawing.Size(36, 15);
            this.labelBitrateUnit.TabIndex = 7;
            this.labelBitrateUnit.Text = "kbps";
            // 
            // labelFpsUnit
            // 
            this.labelFpsUnit.AutoSize = true;
            this.labelFpsUnit.Location = new System.Drawing.Point(190, 79);
            this.labelFpsUnit.Name = "labelFpsUnit";
            this.labelFpsUnit.Size = new System.Drawing.Size(22, 15);
            this.labelFpsUnit.TabIndex = 6;
            this.labelFpsUnit.Text = "fps";
            // 
            // numericBitrate
            // 
            this.numericBitrate.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericBitrate.Location = new System.Drawing.Point(70, 106);
            this.numericBitrate.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.numericBitrate.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericBitrate.Name = "numericBitrate";
            this.numericBitrate.Size = new System.Drawing.Size(114, 23);
            this.numericBitrate.TabIndex = 5;
            this.numericBitrate.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // labelBitrate
            // 
            this.labelBitrate.AutoSize = true;
            this.labelBitrate.Location = new System.Drawing.Point(6, 108);
            this.labelBitrate.Name = "labelBitrate";
            this.labelBitrate.Size = new System.Drawing.Size(44, 15);
            this.labelBitrate.TabIndex = 4;
            this.labelBitrate.Text = "Bitrate:";
            // 
            // numericFps
            // 
            this.numericFps.Location = new System.Drawing.Point(70, 77);
            this.numericFps.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericFps.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericFps.Name = "numericFps";
            this.numericFps.Size = new System.Drawing.Size(114, 23);
            this.numericFps.TabIndex = 3;
            this.numericFps.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // labelFps
            // 
            this.labelFps.AutoSize = true;
            this.labelFps.Location = new System.Drawing.Point(6, 79);
            this.labelFps.Name = "labelFps";
            this.labelFps.Size = new System.Drawing.Size(27, 15);
            this.labelFps.TabIndex = 2;
            this.labelFps.Text = "FPS:";
            // 
            // textBoxSrtUrl
            // 
            this.textBoxSrtUrl.Location = new System.Drawing.Point(70, 30);
            this.textBoxSrtUrl.Name = "textBoxSrtUrl";
            this.textBoxSrtUrl.Size = new System.Drawing.Size(384, 23);
            this.textBoxSrtUrl.TabIndex = 1;
            // 
            // labelSrtUrl
            // 
            this.labelSrtUrl.AutoSize = true;
            this.labelSrtUrl.Location = new System.Drawing.Point(6, 33);
            this.labelSrtUrl.Name = "labelSrtUrl";
            this.labelSrtUrl.Size = new System.Drawing.Size(53, 15);
            this.labelSrtUrl.TabIndex = 0;
            this.labelSrtUrl.Text = "SRT URL:";
            // 
            // groupBoxStreaming
            // 
            this.groupBoxStreaming.Controls.Add(this.labelStatus);
            this.groupBoxStreaming.Controls.Add(this.buttonStartStop);
            this.groupBoxStreaming.Location = new System.Drawing.Point(12, 330);
            this.groupBoxStreaming.Name = "groupBoxStreaming";
            this.groupBoxStreaming.Size = new System.Drawing.Size(460, 80);
            this.groupBoxStreaming.TabIndex = 3;
            this.groupBoxStreaming.TabStop = false;
            this.groupBoxStreaming.Text = "Streaming Control";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(150, 35);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(39, 15);
            this.labelStatus.TabIndex = 1;
            this.labelStatus.Text = "Ready";
            // 
            // buttonStartStop
            // 
            this.buttonStartStop.BackColor = System.Drawing.Color.LightGreen;
            this.buttonStartStop.Location = new System.Drawing.Point(6, 25);
            this.buttonStartStop.Name = "buttonStartStop";
            this.buttonStartStop.Size = new System.Drawing.Size(120, 35);
            this.buttonStartStop.TabIndex = 0;
            this.buttonStartStop.Text = "Start Streaming";
            this.buttonStartStop.UseVisualStyleBackColor = false;
            this.buttonStartStop.Click += new System.EventHandler(this.ButtonStartStop_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 422);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(484, 22);
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(98, 17);
            this.toolStripStatusLabel.Text = "StreamVault v1.0";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 480);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.groupBoxStreaming);
            this.Controls.Add(this.groupBoxSettings);
            this.Controls.Add(this.groupBoxVirtualMonitor);
            this.Controls.Add(this.groupBoxMonitor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StreamVault - Screen Capture to SRT";
            this.groupBoxMonitor.ResumeLayout(false);
            this.groupBoxMonitor.PerformLayout();
            this.groupBoxVirtualMonitor.ResumeLayout(false);
            this.groupBoxVirtualMonitor.PerformLayout();
            this.groupBoxSettings.ResumeLayout(false);
            this.groupBoxSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericBitrate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericFps)).EndInit();
            this.groupBoxStreaming.ResumeLayout(false);
            this.groupBoxStreaming.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxMonitor;
        private System.Windows.Forms.ComboBox comboBoxMonitor;
        private System.Windows.Forms.Label labelMonitor;
        private System.Windows.Forms.GroupBox groupBoxVirtualMonitor;
        private System.Windows.Forms.Button buttonMultiStream;
        private System.Windows.Forms.Label labelVirtualMonitorStatus;
        private System.Windows.Forms.Button buttonRemoveVirtualMonitor;
        private System.Windows.Forms.Button buttonCreateVirtualMonitor;
        private System.Windows.Forms.GroupBox groupBoxSettings;
        private System.Windows.Forms.TextBox textBoxSrtUrl;
        private System.Windows.Forms.Label labelSrtUrl;
        private System.Windows.Forms.GroupBox groupBoxStreaming;
        private System.Windows.Forms.Button buttonStartStop;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.NumericUpDown numericFps;
        private System.Windows.Forms.Label labelFps;
        private System.Windows.Forms.NumericUpDown numericBitrate;
        private System.Windows.Forms.Label labelBitrate;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Button buttonRefreshMonitors;
        private System.Windows.Forms.Label labelBitrateUnit;
        private System.Windows.Forms.Label labelFpsUnit;
    }
}
