namespace StreamVault.Forms
{
    partial class VirtualMonitorDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelPresets = new System.Windows.Forms.Label();
            this.comboBoxPresets = new System.Windows.Forms.ComboBox();
            this.labelWidth = new System.Windows.Forms.Label();
            this.numericWidth = new System.Windows.Forms.NumericUpDown();
            this.labelHeight = new System.Windows.Forms.Label();
            this.numericHeight = new System.Windows.Forms.NumericUpDown();
            this.labelRefreshRate = new System.Windows.Forms.Label();
            this.numericRefreshRate = new System.Windows.Forms.NumericUpDown();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRefreshRate)).BeginInit();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(12, 15);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(42, 15);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Name:";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(100, 12);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(280, 23);
            this.textBoxName.TabIndex = 1;
            // 
            // labelPresets
            // 
            this.labelPresets.AutoSize = true;
            this.labelPresets.Location = new System.Drawing.Point(12, 50);
            this.labelPresets.Name = "labelPresets";
            this.labelPresets.Size = new System.Drawing.Size(47, 15);
            this.labelPresets.TabIndex = 2;
            this.labelPresets.Text = "Preset:";
            // 
            // comboBoxPresets
            // 
            this.comboBoxPresets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPresets.FormattingEnabled = true;
            this.comboBoxPresets.Location = new System.Drawing.Point(100, 47);
            this.comboBoxPresets.Name = "comboBoxPresets";
            this.comboBoxPresets.Size = new System.Drawing.Size(280, 23);
            this.comboBoxPresets.TabIndex = 3;
            this.comboBoxPresets.SelectedIndexChanged += new System.EventHandler(this.ComboBoxPresets_SelectedIndexChanged);
            // 
            // labelWidth
            // 
            this.labelWidth.AutoSize = true;
            this.labelWidth.Location = new System.Drawing.Point(12, 85);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(42, 15);
            this.labelWidth.TabIndex = 4;
            this.labelWidth.Text = "Width:";
            // 
            // numericWidth
            // 
            this.numericWidth.Location = new System.Drawing.Point(100, 82);
            this.numericWidth.Maximum = new decimal(new int[] {
            7680,
            0,
            0,
            0});
            this.numericWidth.Minimum = new decimal(new int[] {
            640,
            0,
            0,
            0});
            this.numericWidth.Name = "numericWidth";
            this.numericWidth.Size = new System.Drawing.Size(120, 23);
            this.numericWidth.TabIndex = 5;
            this.numericWidth.Value = new decimal(new int[] {
            1920,
            0,
            0,
            0});
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Location = new System.Drawing.Point(240, 85);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(46, 15);
            this.labelHeight.TabIndex = 6;
            this.labelHeight.Text = "Height:";
            // 
            // numericHeight
            // 
            this.numericHeight.Location = new System.Drawing.Point(292, 82);
            this.numericHeight.Maximum = new decimal(new int[] {
            4320,
            0,
            0,
            0});
            this.numericHeight.Minimum = new decimal(new int[] {
            480,
            0,
            0,
            0});
            this.numericHeight.Name = "numericHeight";
            this.numericHeight.Size = new System.Drawing.Size(88, 23);
            this.numericHeight.TabIndex = 7;
            this.numericHeight.Value = new decimal(new int[] {
            1080,
            0,
            0,
            0});
            // 
            // labelRefreshRate
            // 
            this.labelRefreshRate.AutoSize = true;
            this.labelRefreshRate.Location = new System.Drawing.Point(12, 120);
            this.labelRefreshRate.Name = "labelRefreshRate";
            this.labelRefreshRate.Size = new System.Drawing.Size(82, 15);
            this.labelRefreshRate.TabIndex = 8;
            this.labelRefreshRate.Text = "Refresh Rate:";
            // 
            // numericRefreshRate
            // 
            this.numericRefreshRate.Location = new System.Drawing.Point(100, 117);
            this.numericRefreshRate.Maximum = new decimal(new int[] {
            240,
            0,
            0,
            0});
            this.numericRefreshRate.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericRefreshRate.Name = "numericRefreshRate";
            this.numericRefreshRate.Size = new System.Drawing.Size(120, 23);
            this.numericRefreshRate.TabIndex = 9;
            this.numericRefreshRate.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(224, 190);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 10;
            this.buttonOK.Text = "Create";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(305, 190);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 11;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // labelInfo
            // 
            this.labelInfo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.labelInfo.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelInfo.Location = new System.Drawing.Point(12, 150);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(368, 30);
            this.labelInfo.TabIndex = 12;
            this.labelInfo.Text = "Note: Virtual monitor creation requires appropriate drivers (e.g., IddSampleDriv" +
    "er) to be installed on the system.";
            // 
            // VirtualMonitorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 225);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.numericRefreshRate);
            this.Controls.Add(this.labelRefreshRate);
            this.Controls.Add(this.numericHeight);
            this.Controls.Add(this.labelHeight);
            this.Controls.Add(this.numericWidth);
            this.Controls.Add(this.labelWidth);
            this.Controls.Add(this.comboBoxPresets);
            this.Controls.Add(this.labelPresets);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VirtualMonitorDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create Virtual Monitor";
            ((System.ComponentModel.ISupportInitialize)(this.numericWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRefreshRate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelPresets;
        private System.Windows.Forms.ComboBox comboBoxPresets;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.NumericUpDown numericWidth;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.NumericUpDown numericHeight;
        private System.Windows.Forms.Label labelRefreshRate;
        private System.Windows.Forms.NumericUpDown numericRefreshRate;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelInfo;
    }
}
