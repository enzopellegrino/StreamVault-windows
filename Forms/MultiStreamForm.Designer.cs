namespace StreamVault.Forms
{
    partial class MultiStreamForm
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
            this.groupBoxConfiguration = new System.Windows.Forms.GroupBox();
            this.buttonDebugFFmpeg = new System.Windows.Forms.Button();
            this.buttonSaveConfig = new System.Windows.Forms.Button();
            this.buttonTestChrome = new System.Windows.Forms.Button();
            this.buttonGenerateUrls = new System.Windows.Forms.Button();
            this.checkBoxAutoChrome = new System.Windows.Forms.CheckBox();
            this.textBoxChromeUrl = new System.Windows.Forms.TextBox();
            this.labelChromeUrl = new System.Windows.Forms.Label();
            this.numericBasePort = new System.Windows.Forms.NumericUpDown();
            this.labelBasePort = new System.Windows.Forms.Label();
            this.textBoxBaseHost = new System.Windows.Forms.TextBox();
            this.labelBaseHost = new System.Windows.Forms.Label();
            this.comboBoxSrtServers = new System.Windows.Forms.ComboBox();
            this.labelSrtServer = new System.Windows.Forms.Label();
            this.buttonManageSrtServers = new System.Windows.Forms.Button();
            this.buttonVirtualDesktops = new System.Windows.Forms.Button();
            this.groupBoxStreams = new System.Windows.Forms.GroupBox();
            this.dataGridViewStreams = new System.Windows.Forms.DataGridView();
            this.groupBoxControl = new System.Windows.Forms.GroupBox();
            this.buttonAddMonitor = new System.Windows.Forms.Button();
            this.buttonRefreshMonitors = new System.Windows.Forms.Button();
            this.buttonStartAll = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelStreams = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelChrome = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelStatus = new System.Windows.Forms.Label();
            this.groupBoxConfiguration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericBasePort)).BeginInit();
            this.groupBoxStreams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStreams)).BeginInit();
            this.groupBoxControl.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxConfiguration
            // 
            this.groupBoxConfiguration.Controls.Add(this.buttonVirtualDesktops);
            this.groupBoxConfiguration.Controls.Add(this.buttonManageSrtServers);
            this.groupBoxConfiguration.Controls.Add(this.comboBoxSrtServers);
            this.groupBoxConfiguration.Controls.Add(this.labelSrtServer);
            this.groupBoxConfiguration.Controls.Add(this.buttonSaveConfig);
            this.groupBoxConfiguration.Controls.Add(this.buttonTestChrome);
            this.groupBoxConfiguration.Controls.Add(this.buttonGenerateUrls);
            this.groupBoxConfiguration.Controls.Add(this.buttonDebugFFmpeg);
            this.groupBoxConfiguration.Controls.Add(this.checkBoxAutoChrome);
            this.groupBoxConfiguration.Controls.Add(this.textBoxChromeUrl);
            this.groupBoxConfiguration.Controls.Add(this.labelChromeUrl);
            this.groupBoxConfiguration.Controls.Add(this.numericBasePort);
            this.groupBoxConfiguration.Controls.Add(this.labelBasePort);
            this.groupBoxConfiguration.Controls.Add(this.textBoxBaseHost);
            this.groupBoxConfiguration.Controls.Add(this.labelBaseHost);
            this.groupBoxConfiguration.Location = new System.Drawing.Point(12, 12);
            this.groupBoxConfiguration.Name = "groupBoxConfiguration";
            this.groupBoxConfiguration.Size = new System.Drawing.Size(1180, 140);  // Aumentato da 120 a 140 per più spazio
            this.groupBoxConfiguration.TabIndex = 0;
            this.groupBoxConfiguration.TabStop = false;
            this.groupBoxConfiguration.Text = "Streaming Configuration";
            // 
            // buttonDebugFFmpeg
            // 
            this.buttonDebugFFmpeg.Location = new System.Drawing.Point(930, 85);  // Spostato per non sovrapporsi
            this.buttonDebugFFmpeg.Name = "buttonDebugFFmpeg";
            this.buttonDebugFFmpeg.Size = new System.Drawing.Size(100, 25);
            this.buttonDebugFFmpeg.TabIndex = 10;
            this.buttonDebugFFmpeg.Text = "Debug FFmpeg";
            this.buttonDebugFFmpeg.UseVisualStyleBackColor = true;
            this.buttonDebugFFmpeg.Click += new System.EventHandler(this.ButtonDebugFFmpeg_Click);
            // 
            // buttonSaveConfig
            // 
            this.buttonSaveConfig.Location = new System.Drawing.Point(820, 85);  // Spostato per non sovrapporsi
            this.buttonSaveConfig.Name = "buttonSaveConfig";
            this.buttonSaveConfig.Size = new System.Drawing.Size(100, 25);
            this.buttonSaveConfig.TabIndex = 9;
            this.buttonSaveConfig.Text = "Save Config";
            this.buttonSaveConfig.UseVisualStyleBackColor = true;
            this.buttonSaveConfig.Click += new System.EventHandler(this.ButtonSaveConfig_Click);
            // 
            // buttonTestChrome
            // 
            this.buttonTestChrome.Location = new System.Drawing.Point(710, 85);  // Spostato per non sovrapporsi
            this.buttonTestChrome.Name = "buttonTestChrome";
            this.buttonTestChrome.Size = new System.Drawing.Size(100, 25);
            this.buttonTestChrome.TabIndex = 8;
            this.buttonTestChrome.Text = "Test Chrome";
            this.buttonTestChrome.UseVisualStyleBackColor = true;
            this.buttonTestChrome.Click += new System.EventHandler(this.ButtonTestChrome_Click);
            // 
            // buttonGenerateUrls
            // 
            this.buttonGenerateUrls.Location = new System.Drawing.Point(580, 85);  // Spostato per non sovrapporsi
            this.buttonGenerateUrls.Name = "buttonGenerateUrls";
            this.buttonGenerateUrls.Size = new System.Drawing.Size(120, 25);  // Ridotto per dare più spazio
            this.buttonGenerateUrls.TabIndex = 7;
            this.buttonGenerateUrls.Text = "Generate URLs";
            this.buttonGenerateUrls.UseVisualStyleBackColor = true;
            this.buttonGenerateUrls.Click += new System.EventHandler(this.ButtonGenerateUrls_Click);
            // 
            // checkBoxAutoChrome
            // 
            this.checkBoxAutoChrome.AutoSize = true;
            this.checkBoxAutoChrome.Location = new System.Drawing.Point(15, 85);
            this.checkBoxAutoChrome.Name = "checkBoxAutoChrome";
            this.checkBoxAutoChrome.Size = new System.Drawing.Size(169, 19);
            this.checkBoxAutoChrome.TabIndex = 6;
            this.checkBoxAutoChrome.Text = "Auto-start Chrome on monitors";
            this.checkBoxAutoChrome.UseVisualStyleBackColor = true;
            // 
            // textBoxChromeUrl
            // 
            this.textBoxChromeUrl.Location = new System.Drawing.Point(280, 55);
            this.textBoxChromeUrl.Name = "textBoxChromeUrl";
            this.textBoxChromeUrl.Size = new System.Drawing.Size(290, 23);
            this.textBoxChromeUrl.TabIndex = 5;
            // 
            // labelChromeUrl
            // 
            this.labelChromeUrl.AutoSize = true;
            this.labelChromeUrl.Location = new System.Drawing.Point(280, 35);
            this.labelChromeUrl.Name = "labelChromeUrl";
            this.labelChromeUrl.Size = new System.Drawing.Size(76, 15);
            this.labelChromeUrl.TabIndex = 4;
            this.labelChromeUrl.Text = "Chrome URL:";
            // 
            // numericBasePort
            // 
            this.numericBasePort.Location = new System.Drawing.Point(145, 55);
            this.numericBasePort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericBasePort.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericBasePort.Name = "numericBasePort";
            this.numericBasePort.Size = new System.Drawing.Size(120, 23);
            this.numericBasePort.TabIndex = 3;
            this.numericBasePort.Value = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            // 
            // labelBasePort
            // 
            this.labelBasePort.AutoSize = true;
            this.labelBasePort.Location = new System.Drawing.Point(145, 35);
            this.labelBasePort.Name = "labelBasePort";
            this.labelBasePort.Size = new System.Drawing.Size(61, 15);
            this.labelBasePort.TabIndex = 2;
            this.labelBasePort.Text = "Base Port:";
            // 
            // textBoxBaseHost
            // 
            this.textBoxBaseHost.Location = new System.Drawing.Point(15, 55);
            this.textBoxBaseHost.Name = "textBoxBaseHost";
            this.textBoxBaseHost.Size = new System.Drawing.Size(120, 23);
            this.textBoxBaseHost.TabIndex = 1;
            // 
            // labelBaseHost
            // 
            this.labelBaseHost.AutoSize = true;
            this.labelBaseHost.Location = new System.Drawing.Point(15, 35);
            this.labelBaseHost.Name = "labelBaseHost";
            this.labelBaseHost.Size = new System.Drawing.Size(63, 15);
            this.labelBaseHost.TabIndex = 0;
            this.labelBaseHost.Text = "Base Host:";
            // 
            // labelSrtServer
            // 
            this.labelSrtServer.AutoSize = true;
            this.labelSrtServer.Location = new System.Drawing.Point(420, 35);
            this.labelSrtServer.Name = "labelSrtServer";
            this.labelSrtServer.Size = new System.Drawing.Size(71, 15);
            this.labelSrtServer.TabIndex = 20;
            this.labelSrtServer.Text = "SRT Server:";
            // 
            // comboBoxSrtServers
            // 
            this.comboBoxSrtServers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSrtServers.FormattingEnabled = true;
            this.comboBoxSrtServers.Location = new System.Drawing.Point(500, 32);
            this.comboBoxSrtServers.Name = "comboBoxSrtServers";
            this.comboBoxSrtServers.Size = new System.Drawing.Size(240, 23);
            this.comboBoxSrtServers.TabIndex = 21;
            this.comboBoxSrtServers.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSrtServers_SelectedIndexChanged);
            // 
            // buttonManageSrtServers
            // 
            this.buttonManageSrtServers.Location = new System.Drawing.Point(750, 32);
            this.buttonManageSrtServers.Name = "buttonManageSrtServers";
            this.buttonManageSrtServers.Size = new System.Drawing.Size(100, 23);
            this.buttonManageSrtServers.TabIndex = 22;
            this.buttonManageSrtServers.Text = "Manage...";
            this.buttonManageSrtServers.UseVisualStyleBackColor = true;
            this.buttonManageSrtServers.Click += new System.EventHandler(this.ButtonManageSrtServers_Click);
            // 
            // buttonVirtualDesktops
            // 
            this.buttonVirtualDesktops.Location = new System.Drawing.Point(980, 32);  // Spostato leggermente
            this.buttonVirtualDesktops.Name = "buttonVirtualDesktops";
            this.buttonVirtualDesktops.Size = new System.Drawing.Size(120, 23);
            this.buttonVirtualDesktops.TabIndex = 23;
            this.buttonVirtualDesktops.Text = "Virtual Desktops...";
            this.buttonVirtualDesktops.UseVisualStyleBackColor = true;
            this.buttonVirtualDesktops.Click += new System.EventHandler(this.ButtonVirtualDesktops_Click);
            // 
            // groupBoxStreams
            // 
            this.groupBoxStreams.Controls.Add(this.dataGridViewStreams);
            this.groupBoxStreams.Location = new System.Drawing.Point(12, 160);  // Adattato per il nuovo layout
            this.groupBoxStreams.Name = "groupBoxStreams";
            this.groupBoxStreams.Size = new System.Drawing.Size(1180, 400);  // Aumentato da 390 a 400
            this.groupBoxStreams.TabIndex = 1;
            this.groupBoxStreams.TabStop = false;
            this.groupBoxStreams.Text = "Active Streams";
            // 
            // dataGridViewStreams
            // 
            this.dataGridViewStreams.AllowUserToAddRows = false;
            this.dataGridViewStreams.AllowUserToDeleteRows = false;
            this.dataGridViewStreams.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewStreams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStreams.Location = new System.Drawing.Point(10, 25);
            this.dataGridViewStreams.Name = "dataGridViewStreams";
            this.dataGridViewStreams.ReadOnly = false;  // Cambiato per permettere i bottoni
            this.dataGridViewStreams.RowHeadersWidth = 51;
            this.dataGridViewStreams.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewStreams.Size = new System.Drawing.Size(1160, 360);  // Aumentato da 350 a 360
            this.dataGridViewStreams.TabIndex = 0;
            // 
            // groupBoxControl
            // 
            this.groupBoxControl.Controls.Add(this.labelStatus);
            this.groupBoxControl.Controls.Add(this.buttonAddMonitor);
            this.groupBoxControl.Controls.Add(this.buttonRefreshMonitors);
            this.groupBoxControl.Controls.Add(this.buttonStartAll);
            this.groupBoxControl.Location = new System.Drawing.Point(12, 570);  // Adattato per il nuovo layout
            this.groupBoxControl.Name = "groupBoxControl";
            this.groupBoxControl.Size = new System.Drawing.Size(1160, 80);  // Allargato da 760 a 1160
            this.groupBoxControl.TabIndex = 2;
            this.groupBoxControl.TabStop = false;
            this.groupBoxControl.Text = "Control";
            // 
            // buttonRefreshMonitors
            // 
            this.buttonRefreshMonitors.Location = new System.Drawing.Point(270, 25);
            this.buttonRefreshMonitors.Name = "buttonRefreshMonitors";
            this.buttonRefreshMonitors.Size = new System.Drawing.Size(120, 35);
            this.buttonRefreshMonitors.TabIndex = 2;
            this.buttonRefreshMonitors.Text = "Refresh Monitors";
            this.buttonRefreshMonitors.UseVisualStyleBackColor = true;
            this.buttonRefreshMonitors.Click += new System.EventHandler(this.ButtonRefreshMonitors_Click);
            // 
            // buttonAddMonitor
            // 
            this.buttonAddMonitor.BackColor = System.Drawing.Color.LightBlue;
            this.buttonAddMonitor.Location = new System.Drawing.Point(140, 25);
            this.buttonAddMonitor.Name = "buttonAddMonitor";
            this.buttonAddMonitor.Size = new System.Drawing.Size(120, 35);
            this.buttonAddMonitor.TabIndex = 1;
            this.buttonAddMonitor.Text = "Add Monitor";
            this.buttonAddMonitor.UseVisualStyleBackColor = false;
            this.buttonAddMonitor.Click += new System.EventHandler(this.ButtonAddMonitor_Click);
            // 
            // buttonStartAll
            // 
            this.buttonStartAll.BackColor = System.Drawing.Color.LightGreen;
            this.buttonStartAll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.buttonStartAll.Location = new System.Drawing.Point(15, 25);
            this.buttonStartAll.Name = "buttonStartAll";
            this.buttonStartAll.Size = new System.Drawing.Size(120, 35);
            this.buttonStartAll.TabIndex = 0;
            this.buttonStartAll.Text = "Start All Streams";
            this.buttonStartAll.UseVisualStyleBackColor = false;
            this.buttonStartAll.Click += new System.EventHandler(this.ButtonStartAll_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelStreams,
            this.toolStripStatusLabelChrome});
            this.statusStrip.Location = new System.Drawing.Point(0, 660);  // Adattato per il nuovo layout
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1200, 22);  // Allargato da 784 a 1200
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabelStreams
            // 
            this.toolStripStatusLabelStreams.Name = "toolStripStatusLabelStreams";
            this.toolStripStatusLabelStreams.Size = new System.Drawing.Size(98, 17);
            this.toolStripStatusLabelStreams.Text = "Active streams: 0";
            // 
            // toolStripStatusLabelChrome
            // 
            this.toolStripStatusLabelChrome.Name = "toolStripStatusLabelChrome";
            this.toolStripStatusLabelChrome.Size = new System.Drawing.Size(102, 17);
            this.toolStripStatusLabelChrome.Text = "Chrome: Checking...";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelStatus.Location = new System.Drawing.Point(280, 35);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(42, 15);
            this.labelStatus.TabIndex = 2;
            this.labelStatus.Text = "Ready";
            // 
            // MultiStreamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1210, 682);  // Adattato alle nuove dimensioni
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.groupBoxControl);
            this.Controls.Add(this.groupBoxStreams);
            this.Controls.Add(this.groupBoxConfiguration);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;  // Cambiato da FixedSingle a Sizable
            this.MaximizeBox = true;  // Cambiato da false a true
            this.MinimizeBox = true;  // Cambiato da false a true
            this.Name = "MultiStreamForm";
            this.Text = "StreamVault - Screen Capture & Streaming";
            this.groupBoxConfiguration.ResumeLayout(false);
            this.groupBoxConfiguration.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericBasePort)).EndInit();
            this.groupBoxStreams.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStreams)).EndInit();
            this.groupBoxControl.ResumeLayout(false);
            this.groupBoxControl.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxConfiguration;
        private System.Windows.Forms.TextBox textBoxBaseHost;
        private System.Windows.Forms.Label labelBaseHost;
        private System.Windows.Forms.NumericUpDown numericBasePort;
        private System.Windows.Forms.Label labelBasePort;
        private System.Windows.Forms.TextBox textBoxChromeUrl;
        private System.Windows.Forms.Label labelChromeUrl;
        private System.Windows.Forms.CheckBox checkBoxAutoChrome;
        private System.Windows.Forms.Button buttonGenerateUrls;
        private System.Windows.Forms.Button buttonTestChrome;
        private System.Windows.Forms.Button buttonSaveConfig;
        private System.Windows.Forms.Button buttonDebugFFmpeg;
        private System.Windows.Forms.ComboBox comboBoxSrtServers;
        private System.Windows.Forms.Label labelSrtServer;
        private System.Windows.Forms.Button buttonManageSrtServers;
        private System.Windows.Forms.Button buttonVirtualDesktops;
        private System.Windows.Forms.GroupBox groupBoxStreams;
        private System.Windows.Forms.DataGridView dataGridViewStreams;
        private System.Windows.Forms.GroupBox groupBoxControl;
        private System.Windows.Forms.Button buttonStartAll;
        private System.Windows.Forms.Button buttonAddMonitor;
        private System.Windows.Forms.Button buttonRefreshMonitors;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelStreams;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelChrome;
        private System.Windows.Forms.Label labelStatus;
    }
}
