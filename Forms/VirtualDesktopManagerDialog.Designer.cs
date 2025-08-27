namespace StreamVault.Forms
{
    partial class VirtualDesktopManagerDialog
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
            groupBoxDriverStatus = new GroupBox();
            buttonRefresh = new Button();
            labelDriversPath = new Label();
            buttonInstallDriver = new Button();
            labelDriverStatus = new Label();
            labelAdminStatus = new Label();
            groupBoxDesktops = new GroupBox();
            labelSelectedInfo = new Label();
            buttonTestDesktop = new Button();
            buttonRemoveDesktop = new Button();
            labelDesktopCount = new Label();
            listBoxDesktops = new ListBox();
            groupBoxCreateDesktop = new GroupBox();
            buttonCreateDesktop = new Button();
            numericHeight = new NumericUpDown();
            label3 = new Label();
            numericWidth = new NumericUpDown();
            label2 = new Label();
            textBoxDesktopName = new TextBox();
            label1 = new Label();
            buttonClose = new Button();
            buttonTroubleshoot = new Button();
            groupBoxDriverStatus.SuspendLayout();
            groupBoxDesktops.SuspendLayout();
            groupBoxCreateDesktop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericHeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericWidth).BeginInit();
            SuspendLayout();
            // 
            // groupBoxDriverStatus
            // 
            groupBoxDriverStatus.Controls.Add(buttonRefresh);
            groupBoxDriverStatus.Controls.Add(labelDriversPath);
            groupBoxDriverStatus.Controls.Add(buttonInstallDriver);
            groupBoxDriverStatus.Controls.Add(labelDriverStatus);
            groupBoxDriverStatus.Controls.Add(labelAdminStatus);
            groupBoxDriverStatus.Location = new Point(12, 12);
            groupBoxDriverStatus.Name = "groupBoxDriverStatus";
            groupBoxDriverStatus.Size = new Size(560, 120);
            groupBoxDriverStatus.TabIndex = 0;
            groupBoxDriverStatus.TabStop = false;
            groupBoxDriverStatus.Text = "Virtual Display Driver Status";
            // 
            // buttonRefresh
            // 
            buttonRefresh.Location = new Point(479, 22);
            buttonRefresh.Name = "buttonRefresh";
            buttonRefresh.Size = new Size(75, 23);
            buttonRefresh.TabIndex = 4;
            buttonRefresh.Text = "Refresh";
            buttonRefresh.UseVisualStyleBackColor = true;
            buttonRefresh.Click += ButtonRefresh_Click;
            // 
            // labelDriversPath
            // 
            labelDriversPath.AutoSize = true;
            labelDriversPath.ForeColor = SystemColors.GrayText;
            labelDriversPath.Location = new Point(6, 95);
            labelDriversPath.Name = "labelDriversPath";
            labelDriversPath.Size = new Size(53, 15);
            labelDriversPath.TabIndex = 3;
            labelDriversPath.Text = "Drivers: ";
            // 
            // buttonInstallDriver
            // 
            buttonInstallDriver.Location = new Point(320, 45);
            buttonInstallDriver.Name = "buttonInstallDriver";
            buttonInstallDriver.Size = new Size(120, 30);
            buttonInstallDriver.TabIndex = 2;
            buttonInstallDriver.Text = "Install Driver";
            buttonInstallDriver.UseVisualStyleBackColor = true;
            buttonInstallDriver.Click += ButtonInstallDriver_Click;
            // 
            // labelDriverStatus
            // 
            labelDriverStatus.AutoSize = true;
            labelDriverStatus.Location = new Point(6, 50);
            labelDriverStatus.Name = "labelDriverStatus";
            labelDriverStatus.Size = new Size(85, 15);
            labelDriverStatus.TabIndex = 1;
            labelDriverStatus.Text = "Checking driver...";
            // 
            // labelAdminStatus
            // 
            labelAdminStatus.AutoSize = true;
            labelAdminStatus.Location = new Point(6, 25);
            labelAdminStatus.Name = "labelAdminStatus";
            labelAdminStatus.Size = new Size(116, 15);
            labelAdminStatus.TabIndex = 0;
            labelAdminStatus.Text = "Checking privileges...";
            // 
            // groupBoxDesktops
            // 
            groupBoxDesktops.Controls.Add(labelSelectedInfo);
            groupBoxDesktops.Controls.Add(buttonTestDesktop);
            groupBoxDesktops.Controls.Add(buttonRemoveDesktop);
            groupBoxDesktops.Controls.Add(labelDesktopCount);
            groupBoxDesktops.Controls.Add(listBoxDesktops);
            groupBoxDesktops.Location = new Point(12, 138);
            groupBoxDesktops.Name = "groupBoxDesktops";
            groupBoxDesktops.Size = new Size(350, 300);
            groupBoxDesktops.TabIndex = 1;
            groupBoxDesktops.TabStop = false;
            groupBoxDesktops.Text = "Virtual Desktops";
            // 
            // labelSelectedInfo
            // 
            labelSelectedInfo.AutoSize = true;
            labelSelectedInfo.ForeColor = SystemColors.GrayText;
            labelSelectedInfo.Location = new Point(6, 275);
            labelSelectedInfo.Name = "labelSelectedInfo";
            labelSelectedInfo.Size = new Size(118, 15);
            labelSelectedInfo.TabIndex = 4;
            labelSelectedInfo.Text = "No desktop selected";
            // 
            // buttonTestDesktop
            // 
            buttonTestDesktop.Location = new Point(188, 240);
            buttonTestDesktop.Name = "buttonTestDesktop";
            buttonTestDesktop.Size = new Size(75, 23);
            buttonTestDesktop.TabIndex = 3;
            buttonTestDesktop.Text = "Test";
            buttonTestDesktop.UseVisualStyleBackColor = true;
            buttonTestDesktop.Click += ButtonTestDesktop_Click;
            // 
            // buttonRemoveDesktop
            // 
            buttonRemoveDesktop.Location = new Point(269, 240);
            buttonRemoveDesktop.Name = "buttonRemoveDesktop";
            buttonRemoveDesktop.Size = new Size(75, 23);
            buttonRemoveDesktop.TabIndex = 2;
            buttonRemoveDesktop.Text = "Remove";
            buttonRemoveDesktop.UseVisualStyleBackColor = true;
            buttonRemoveDesktop.Click += ButtonRemoveDesktop_Click;
            // 
            // labelDesktopCount
            // 
            labelDesktopCount.AutoSize = true;
            labelDesktopCount.Location = new Point(6, 245);
            labelDesktopCount.Name = "labelDesktopCount";
            labelDesktopCount.Size = new Size(108, 15);
            labelDesktopCount.TabIndex = 1;
            labelDesktopCount.Text = "Virtual Desktops: 0";
            // 
            // listBoxDesktops
            // 
            listBoxDesktops.FormattingEnabled = true;
            listBoxDesktops.ItemHeight = 15;
            listBoxDesktops.Location = new Point(6, 22);
            listBoxDesktops.Name = "listBoxDesktops";
            listBoxDesktops.Size = new Size(338, 214);
            listBoxDesktops.TabIndex = 0;
            listBoxDesktops.SelectedIndexChanged += ListBoxDesktops_SelectedIndexChanged;
            // 
            // groupBoxCreateDesktop
            // 
            groupBoxCreateDesktop.Controls.Add(buttonCreateDesktop);
            groupBoxCreateDesktop.Controls.Add(numericHeight);
            groupBoxCreateDesktop.Controls.Add(label3);
            groupBoxCreateDesktop.Controls.Add(numericWidth);
            groupBoxCreateDesktop.Controls.Add(label2);
            groupBoxCreateDesktop.Controls.Add(textBoxDesktopName);
            groupBoxCreateDesktop.Controls.Add(label1);
            groupBoxCreateDesktop.Location = new Point(368, 138);
            groupBoxCreateDesktop.Name = "groupBoxCreateDesktop";
            groupBoxCreateDesktop.Size = new Size(204, 180);
            groupBoxCreateDesktop.TabIndex = 2;
            groupBoxCreateDesktop.TabStop = false;
            groupBoxCreateDesktop.Text = "Create New Desktop";
            // 
            // buttonCreateDesktop
            // 
            buttonCreateDesktop.Location = new Point(6, 140);
            buttonCreateDesktop.Name = "buttonCreateDesktop";
            buttonCreateDesktop.Size = new Size(192, 30);
            buttonCreateDesktop.TabIndex = 6;
            buttonCreateDesktop.Text = "Create Desktop";
            buttonCreateDesktop.UseVisualStyleBackColor = true;
            buttonCreateDesktop.Click += ButtonCreateDesktop_Click;
            // 
            // numericHeight
            // 
            numericHeight.Location = new Point(70, 105);
            numericHeight.Maximum = new decimal(new int[] { 4320, 0, 0, 0 });
            numericHeight.Minimum = new decimal(new int[] { 480, 0, 0, 0 });
            numericHeight.Name = "numericHeight";
            numericHeight.Size = new Size(120, 23);
            numericHeight.TabIndex = 5;
            numericHeight.Value = new decimal(new int[] { 1080, 0, 0, 0 });
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 107);
            label3.Name = "label3";
            label3.Size = new Size(46, 15);
            label3.TabIndex = 4;
            label3.Text = "Height:";
            // 
            // numericWidth
            // 
            numericWidth.Location = new Point(70, 75);
            numericWidth.Maximum = new decimal(new int[] { 7680, 0, 0, 0 });
            numericWidth.Minimum = new decimal(new int[] { 640, 0, 0, 0 });
            numericWidth.Name = "numericWidth";
            numericWidth.Size = new Size(120, 23);
            numericWidth.TabIndex = 3;
            numericWidth.Value = new decimal(new int[] { 1920, 0, 0, 0 });
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 77);
            label2.Name = "label2";
            label2.Size = new Size(42, 15);
            label2.TabIndex = 2;
            label2.Text = "Width:";
            // 
            // textBoxDesktopName
            // 
            textBoxDesktopName.Location = new Point(6, 40);
            textBoxDesktopName.Name = "textBoxDesktopName";
            textBoxDesktopName.Size = new Size(192, 23);
            textBoxDesktopName.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 22);
            label1.Name = "label1";
            label1.Size = new Size(42, 15);
            label1.TabIndex = 0;
            label1.Text = "Name:";
            // 
            // buttonClose
            // 
            buttonClose.Location = new Point(497, 452);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(75, 23);
            buttonClose.TabIndex = 3;
            buttonClose.Text = "Close";
            buttonClose.UseVisualStyleBackColor = true;
            buttonClose.Click += ButtonClose_Click;
            // 
            // buttonTroubleshoot
            // 
            buttonTroubleshoot.Location = new Point(400, 452);
            buttonTroubleshoot.Name = "buttonTroubleshoot";
            buttonTroubleshoot.Size = new Size(90, 23);
            buttonTroubleshoot.TabIndex = 4;
            buttonTroubleshoot.Text = "Troubleshoot";
            buttonTroubleshoot.UseVisualStyleBackColor = true;
            buttonTroubleshoot.Click += ButtonTroubleshoot_Click;
            // 
            // VirtualDesktopManagerDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 487);
            Controls.Add(buttonTroubleshoot);
            Controls.Add(buttonClose);
            Controls.Add(groupBoxCreateDesktop);
            Controls.Add(groupBoxDesktops);
            Controls.Add(groupBoxDriverStatus);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "VirtualDesktopManagerDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Virtual Desktop Manager";
            groupBoxDriverStatus.ResumeLayout(false);
            groupBoxDriverStatus.PerformLayout();
            groupBoxDesktops.ResumeLayout(false);
            groupBoxDesktops.PerformLayout();
            groupBoxCreateDesktop.ResumeLayout(false);
            groupBoxCreateDesktop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericHeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericWidth).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBoxDriverStatus;
        private Label labelAdminStatus;
        private Label labelDriverStatus;
        private Button buttonInstallDriver;
        private Label labelDriversPath;
        private Button buttonRefresh;
        private GroupBox groupBoxDesktops;
        private ListBox listBoxDesktops;
        private Label labelDesktopCount;
        private Button buttonRemoveDesktop;
        private Button buttonTestDesktop;
        private Label labelSelectedInfo;
        private GroupBox groupBoxCreateDesktop;
        private Label label1;
        private TextBox textBoxDesktopName;
        private Label label2;
        private NumericUpDown numericWidth;
        private Label label3;
        private NumericUpDown numericHeight;
        private Button buttonCreateDesktop;
        private Button buttonClose;
        private Button buttonTroubleshoot;
    }
}
