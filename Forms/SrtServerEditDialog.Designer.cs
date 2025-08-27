namespace StreamVault.Forms
{
    partial class SrtServerEditDialog
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
            label1 = new Label();
            textBoxName = new TextBox();
            label2 = new Label();
            textBoxHost = new TextBox();
            label3 = new Label();
            numericPort = new NumericUpDown();
            label4 = new Label();
            textBoxStreamKey = new TextBox();
            label5 = new Label();
            textBoxDescription = new TextBox();
            checkBoxActive = new CheckBox();
            label6 = new Label();
            labelSrtUrl = new Label();
            buttonOK = new Button();
            buttonCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)numericPort).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(42, 15);
            label1.TabIndex = 0;
            label1.Text = "Name:";
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(95, 12);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(280, 23);
            textBoxName.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 44);
            label2.Name = "label2";
            label2.Size = new Size(35, 15);
            label2.TabIndex = 2;
            label2.Text = "Host:";
            // 
            // textBoxHost
            // 
            textBoxHost.Location = new Point(95, 41);
            textBoxHost.Name = "textBoxHost";
            textBoxHost.Size = new Size(200, 23);
            textBoxHost.TabIndex = 3;
            textBoxHost.TextChanged += TextBox_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(310, 44);
            label3.Name = "label3";
            label3.Size = new Size(32, 15);
            label3.TabIndex = 4;
            label3.Text = "Port:";
            // 
            // numericPort
            // 
            numericPort.Location = new Point(348, 42);
            numericPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numericPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericPort.Name = "numericPort";
            numericPort.Size = new Size(70, 23);
            numericPort.TabIndex = 5;
            numericPort.Value = new decimal(new int[] { 9999, 0, 0, 0 });
            numericPort.ValueChanged += NumericPort_ValueChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 73);
            label4.Name = "label4";
            label4.Size = new Size(72, 15);
            label4.TabIndex = 6;
            label4.Text = "Stream Key:";
            // 
            // textBoxStreamKey
            // 
            textBoxStreamKey.Location = new Point(95, 70);
            textBoxStreamKey.Name = "textBoxStreamKey";
            textBoxStreamKey.Size = new Size(280, 23);
            textBoxStreamKey.TabIndex = 7;
            textBoxStreamKey.TextChanged += TextBox_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 102);
            label5.Name = "label5";
            label5.Size = new Size(70, 15);
            label5.TabIndex = 8;
            label5.Text = "Description:";
            // 
            // textBoxDescription
            // 
            textBoxDescription.Location = new Point(95, 99);
            textBoxDescription.Multiline = true;
            textBoxDescription.Name = "textBoxDescription";
            textBoxDescription.ScrollBars = ScrollBars.Vertical;
            textBoxDescription.Size = new Size(280, 80);
            textBoxDescription.TabIndex = 9;
            // 
            // checkBoxActive
            // 
            checkBoxActive.AutoSize = true;
            checkBoxActive.Location = new Point(95, 190);
            checkBoxActive.Name = "checkBoxActive";
            checkBoxActive.Size = new Size(59, 19);
            checkBoxActive.TabIndex = 10;
            checkBoxActive.Text = "Active";
            checkBoxActive.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 220);
            label6.Name = "label6";
            label6.Size = new Size(54, 15);
            label6.TabIndex = 11;
            label6.Text = "SRT URL:";
            // 
            // labelSrtUrl
            // 
            labelSrtUrl.AutoSize = true;
            labelSrtUrl.Font = new Font("Consolas", 9F, FontStyle.Bold);
            labelSrtUrl.ForeColor = Color.Blue;
            labelSrtUrl.Location = new Point(95, 220);
            labelSrtUrl.Name = "labelSrtUrl";
            labelSrtUrl.Size = new Size(42, 14);
            labelSrtUrl.TabIndex = 12;
            labelSrtUrl.Text = "srt://";
            // 
            // buttonOK
            // 
            buttonOK.Location = new Point(219, 250);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(75, 23);
            buttonOK.TabIndex = 13;
            buttonOK.Text = "OK";
            buttonOK.UseVisualStyleBackColor = true;
            buttonOK.Click += ButtonOK_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(300, 250);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 14;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // SrtServerEditDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(430, 285);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOK);
            Controls.Add(labelSrtUrl);
            Controls.Add(label6);
            Controls.Add(checkBoxActive);
            Controls.Add(textBoxDescription);
            Controls.Add(label5);
            Controls.Add(textBoxStreamKey);
            Controls.Add(label4);
            Controls.Add(numericPort);
            Controls.Add(label3);
            Controls.Add(textBoxHost);
            Controls.Add(label2);
            Controls.Add(textBoxName);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SrtServerEditDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edit SRT Server";
            ((System.ComponentModel.ISupportInitialize)numericPort).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBoxName;
        private Label label2;
        private TextBox textBoxHost;
        private Label label3;
        private NumericUpDown numericPort;
        private Label label4;
        private TextBox textBoxStreamKey;
        private Label label5;
        private TextBox textBoxDescription;
        private CheckBox checkBoxActive;
        private Label label6;
        private Label labelSrtUrl;
        private Button buttonOK;
        private Button buttonCancel;
    }
}
