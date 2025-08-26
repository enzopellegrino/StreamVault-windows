namespace StreamVault.Forms
{
    partial class VirtualMonitorListDialog
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
            this.labelMonitors = new System.Windows.Forms.Label();
            this.listBoxMonitors = new System.Windows.Forms.ListBox();
            this.labelDetails = new System.Windows.Forms.Label();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelMonitors
            // 
            this.labelMonitors.AutoSize = true;
            this.labelMonitors.Location = new System.Drawing.Point(12, 15);
            this.labelMonitors.Name = "labelMonitors";
            this.labelMonitors.Size = new System.Drawing.Size(105, 15);
            this.labelMonitors.TabIndex = 0;
            this.labelMonitors.Text = "Virtual Monitors:";
            // 
            // listBoxMonitors
            // 
            this.listBoxMonitors.FormattingEnabled = true;
            this.listBoxMonitors.ItemHeight = 15;
            this.listBoxMonitors.Location = new System.Drawing.Point(12, 35);
            this.listBoxMonitors.Name = "listBoxMonitors";
            this.listBoxMonitors.Size = new System.Drawing.Size(460, 139);
            this.listBoxMonitors.TabIndex = 1;
            this.listBoxMonitors.SelectedIndexChanged += new System.EventHandler(this.ListBoxMonitors_SelectedIndexChanged);
            // 
            // labelDetails
            // 
            this.labelDetails.BackColor = System.Drawing.SystemColors.Control;
            this.labelDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelDetails.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelDetails.Location = new System.Drawing.Point(12, 185);
            this.labelDetails.Name = "labelDetails";
            this.labelDetails.Size = new System.Drawing.Size(460, 80);
            this.labelDetails.TabIndex = 2;
            this.labelDetails.Text = "Select a virtual monitor to see details";
            this.labelDetails.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // buttonRemove
            // 
            this.buttonRemove.BackColor = System.Drawing.Color.IndianRed;
            this.buttonRemove.ForeColor = System.Drawing.Color.White;
            this.buttonRemove.Location = new System.Drawing.Point(316, 280);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonRemove.TabIndex = 3;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = false;
            this.buttonRemove.Click += new System.EventHandler(this.ButtonRemove_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(397, 280);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // VirtualMonitorListDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 315);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.labelDetails);
            this.Controls.Add(this.listBoxMonitors);
            this.Controls.Add(this.labelMonitors);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VirtualMonitorListDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manage Virtual Monitors";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelMonitors;
        private System.Windows.Forms.ListBox listBoxMonitors;
        private System.Windows.Forms.Label labelDetails;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonCancel;
    }
}
