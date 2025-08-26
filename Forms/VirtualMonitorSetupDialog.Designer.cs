namespace StreamVault.Forms
{
    partial class VirtualMonitorSetupDialog
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
            this.groupBoxMonitorSelection = new System.Windows.Forms.GroupBox();
            this.listBoxMonitors = new System.Windows.Forms.ListBox();
            this.groupBoxPreview = new System.Windows.Forms.GroupBox();
            this.pictureBoxPreview = new System.Windows.Forms.PictureBox();
            this.buttonRefreshPreview = new System.Windows.Forms.Button();
            this.groupBoxSettings = new System.Windows.Forms.GroupBox();
            this.textBoxMonitorName = new System.Windows.Forms.TextBox();
            this.labelMonitorName = new System.Windows.Forms.Label();
            this.textBoxResolution = new System.Windows.Forms.TextBox();
            this.labelResolution = new System.Windows.Forms.Label();
            this.checkBoxPrimary = new System.Windows.Forms.CheckBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelInstructions = new System.Windows.Forms.Label();
            this.groupBoxMonitorSelection.SuspendLayout();
            this.groupBoxPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
            this.groupBoxSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxMonitorSelection
            // 
            this.groupBoxMonitorSelection.Controls.Add(this.listBoxMonitors);
            this.groupBoxMonitorSelection.Location = new System.Drawing.Point(12, 50);
            this.groupBoxMonitorSelection.Name = "groupBoxMonitorSelection";
            this.groupBoxMonitorSelection.Size = new System.Drawing.Size(300, 200);
            this.groupBoxMonitorSelection.TabIndex = 0;
            this.groupBoxMonitorSelection.TabStop = false;
            this.groupBoxMonitorSelection.Text = "Available Monitors";
            // 
            // listBoxMonitors
            // 
            this.listBoxMonitors.FormattingEnabled = true;
            this.listBoxMonitors.ItemHeight = 15;
            this.listBoxMonitors.Location = new System.Drawing.Point(6, 22);
            this.listBoxMonitors.Name = "listBoxMonitors";
            this.listBoxMonitors.Size = new System.Drawing.Size(288, 169);
            this.listBoxMonitors.TabIndex = 0;
            this.listBoxMonitors.SelectedIndexChanged += new System.EventHandler(this.ListBoxMonitors_SelectedIndexChanged);
            // 
            // groupBoxPreview
            // 
            this.groupBoxPreview.Controls.Add(this.buttonRefreshPreview);
            this.groupBoxPreview.Controls.Add(this.pictureBoxPreview);
            this.groupBoxPreview.Location = new System.Drawing.Point(330, 50);
            this.groupBoxPreview.Name = "groupBoxPreview";
            this.groupBoxPreview.Size = new System.Drawing.Size(350, 280);
            this.groupBoxPreview.TabIndex = 1;
            this.groupBoxPreview.TabStop = false;
            this.groupBoxPreview.Text = "Monitor Preview";
            // 
            // pictureBoxPreview
            // 
            this.pictureBoxPreview.BackColor = System.Drawing.Color.Black;
            this.pictureBoxPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxPreview.Location = new System.Drawing.Point(6, 22);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new System.Drawing.Size(338, 220);
            this.pictureBoxPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxPreview.TabIndex = 0;
            this.pictureBoxPreview.TabStop = false;
            // 
            // buttonRefreshPreview
            // 
            this.buttonRefreshPreview.Location = new System.Drawing.Point(269, 248);
            this.buttonRefreshPreview.Name = "buttonRefreshPreview";
            this.buttonRefreshPreview.Size = new System.Drawing.Size(75, 25);
            this.buttonRefreshPreview.TabIndex = 1;
            this.buttonRefreshPreview.Text = "Refresh";
            this.buttonRefreshPreview.UseVisualStyleBackColor = true;
            this.buttonRefreshPreview.Click += new System.EventHandler(this.ButtonRefreshPreview_Click);
            // 
            // groupBoxSettings
            // 
            this.groupBoxSettings.Controls.Add(this.checkBoxPrimary);
            this.groupBoxSettings.Controls.Add(this.textBoxResolution);
            this.groupBoxSettings.Controls.Add(this.labelResolution);
            this.groupBoxSettings.Controls.Add(this.textBoxMonitorName);
            this.groupBoxSettings.Controls.Add(this.labelMonitorName);
            this.groupBoxSettings.Location = new System.Drawing.Point(12, 260);
            this.groupBoxSettings.Name = "groupBoxSettings";
            this.groupBoxSettings.Size = new System.Drawing.Size(300, 120);
            this.groupBoxSettings.TabIndex = 2;
            this.groupBoxSettings.TabStop = false;
            this.groupBoxSettings.Text = "Virtual Monitor Settings";
            // 
            // textBoxMonitorName
            // 
            this.textBoxMonitorName.Location = new System.Drawing.Point(6, 40);
            this.textBoxMonitorName.Name = "textBoxMonitorName";
            this.textBoxMonitorName.Size = new System.Drawing.Size(288, 23);
            this.textBoxMonitorName.TabIndex = 1;
            // 
            // labelMonitorName
            // 
            this.labelMonitorName.AutoSize = true;
            this.labelMonitorName.Location = new System.Drawing.Point(6, 22);
            this.labelMonitorName.Name = "labelMonitorName";
            this.labelMonitorName.Size = new System.Drawing.Size(87, 15);
            this.labelMonitorName.TabIndex = 0;
            this.labelMonitorName.Text = "Monitor Name:";
            // 
            // textBoxResolution
            // 
            this.textBoxResolution.Location = new System.Drawing.Point(6, 84);
            this.textBoxResolution.Name = "textBoxResolution";
            this.textBoxResolution.ReadOnly = true;
            this.textBoxResolution.Size = new System.Drawing.Size(150, 23);
            this.textBoxResolution.TabIndex = 3;
            // 
            // labelResolution
            // 
            this.labelResolution.AutoSize = true;
            this.labelResolution.Location = new System.Drawing.Point(6, 66);
            this.labelResolution.Name = "labelResolution";
            this.labelResolution.Size = new System.Drawing.Size(68, 15);
            this.labelResolution.TabIndex = 2;
            this.labelResolution.Text = "Resolution:";
            // 
            // checkBoxPrimary
            // 
            this.checkBoxPrimary.AutoSize = true;
            this.checkBoxPrimary.Location = new System.Drawing.Point(170, 86);
            this.checkBoxPrimary.Name = "checkBoxPrimary";
            this.checkBoxPrimary.Size = new System.Drawing.Size(114, 19);
            this.checkBoxPrimary.TabIndex = 4;
            this.checkBoxPrimary.Text = "Primary Monitor";
            this.checkBoxPrimary.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(525, 345);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 30);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(605, 345);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 30);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // labelInstructions
            // 
            this.labelInstructions.Location = new System.Drawing.Point(12, 9);
            this.labelInstructions.Name = "labelInstructions";
            this.labelInstructions.Size = new System.Drawing.Size(668, 35);
            this.labelInstructions.TabIndex = 5;
            this.labelInstructions.Text = "Select a physical monitor to use as a template for the virtual monitor. The virt" +
    "ual monitor will use the same resolution and can be configured with a custom na" +
    "me.";
            // 
            // VirtualMonitorSetupDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 387);
            this.Controls.Add(this.labelInstructions);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBoxSettings);
            this.Controls.Add(this.groupBoxPreview);
            this.Controls.Add(this.groupBoxMonitorSelection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VirtualMonitorSetupDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Virtual Monitor Setup";
            this.groupBoxMonitorSelection.ResumeLayout(false);
            this.groupBoxPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).EndInit();
            this.groupBoxSettings.ResumeLayout(false);
            this.groupBoxSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxMonitorSelection;
        private System.Windows.Forms.ListBox listBoxMonitors;
        private System.Windows.Forms.GroupBox groupBoxPreview;
        private System.Windows.Forms.PictureBox pictureBoxPreview;
        private System.Windows.Forms.Button buttonRefreshPreview;
        private System.Windows.Forms.GroupBox groupBoxSettings;
        private System.Windows.Forms.TextBox textBoxMonitorName;
        private System.Windows.Forms.Label labelMonitorName;
        private System.Windows.Forms.TextBox textBoxResolution;
        private System.Windows.Forms.Label labelResolution;
        private System.Windows.Forms.CheckBox checkBoxPrimary;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelInstructions;
    }
}
