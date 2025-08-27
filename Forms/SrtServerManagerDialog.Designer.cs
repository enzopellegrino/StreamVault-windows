namespace StreamVault.Forms
{
    partial class SrtServerManagerDialog
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
            groupBoxServers = new GroupBox();
            buttonTest = new Button();
            buttonDuplicate = new Button();
            buttonDelete = new Button();
            buttonEdit = new Button();
            buttonAddPreset = new Button();
            buttonAdd = new Button();
            listBoxServers = new ListBox();
            groupBoxDetails = new GroupBox();
            labelLastUsed = new Label();
            labelCreated = new Label();
            labelSrtUrl = new Label();
            label6 = new Label();
            checkBoxActive = new CheckBox();
            textBoxDescription = new TextBox();
            label5 = new Label();
            textBoxStreamKey = new TextBox();
            label4 = new Label();
            numericPort = new NumericUpDown();
            label3 = new Label();
            textBoxHost = new TextBox();
            label2 = new Label();
            textBoxName = new TextBox();
            label1 = new Label();
            buttonOK = new Button();
            buttonCancel = new Button();
            groupBoxServers.SuspendLayout();
            groupBoxDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericPort).BeginInit();
            SuspendLayout();
            // 
            // groupBoxServers
            // 
            groupBoxServers.Controls.Add(buttonTest);
            groupBoxServers.Controls.Add(buttonDuplicate);
            groupBoxServers.Controls.Add(buttonDelete);
            groupBoxServers.Controls.Add(buttonEdit);
            groupBoxServers.Controls.Add(buttonAddPreset);
            groupBoxServers.Controls.Add(buttonAdd);
            groupBoxServers.Controls.Add(listBoxServers);
            groupBoxServers.Location = new Point(12, 12);
            groupBoxServers.Name = "groupBoxServers";
            groupBoxServers.Size = new Size(300, 400);
            groupBoxServers.TabIndex = 0;
            groupBoxServers.TabStop = false;
            groupBoxServers.Text = "SRT Servers";
            // 
            // buttonTest
            // 
            buttonTest.Location = new Point(219, 340);
            buttonTest.Name = "buttonTest";
            buttonTest.Size = new Size(75, 23);
            buttonTest.TabIndex = 6;
            buttonTest.Text = "Test";
            buttonTest.UseVisualStyleBackColor = true;
            buttonTest.Click += ButtonTest_Click;
            // 
            // buttonDuplicate
            // 
            buttonDuplicate.Location = new Point(138, 340);
            buttonDuplicate.Name = "buttonDuplicate";
            buttonDuplicate.Size = new Size(75, 23);
            buttonDuplicate.TabIndex = 5;
            buttonDuplicate.Text = "Duplicate";
            buttonDuplicate.UseVisualStyleBackColor = true;
            buttonDuplicate.Click += ButtonDuplicate_Click;
            // 
            // buttonDelete
            // 
            buttonDelete.Location = new Point(57, 340);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(75, 23);
            buttonDelete.TabIndex = 4;
            buttonDelete.Text = "Delete";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += ButtonDelete_Click;
            // 
            // buttonEdit
            // 
            buttonEdit.Location = new Point(138, 311);
            buttonEdit.Name = "buttonEdit";
            buttonEdit.Size = new Size(75, 23);
            buttonEdit.TabIndex = 3;
            buttonEdit.Text = "Edit";
            buttonEdit.UseVisualStyleBackColor = true;
            buttonEdit.Click += ButtonEdit_Click;
            // 
            // buttonAddPreset
            // 
            buttonAddPreset.Location = new Point(219, 311);
            buttonAddPreset.Name = "buttonAddPreset";
            buttonAddPreset.Size = new Size(75, 23);
            buttonAddPreset.TabIndex = 2;
            buttonAddPreset.Text = "Presets ▼";
            buttonAddPreset.UseVisualStyleBackColor = true;
            buttonAddPreset.Click += ButtonAddPreset_Click;
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(57, 311);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(75, 23);
            buttonAdd.TabIndex = 1;
            buttonAdd.Text = "Add";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += ButtonAdd_Click;
            // 
            // listBoxServers
            // 
            listBoxServers.FormattingEnabled = true;
            listBoxServers.ItemHeight = 15;
            listBoxServers.Location = new Point(6, 22);
            listBoxServers.Name = "listBoxServers";
            listBoxServers.Size = new Size(288, 274);
            listBoxServers.TabIndex = 0;
            listBoxServers.SelectedIndexChanged += ListBoxServers_SelectedIndexChanged;
            // 
            // groupBoxDetails
            // 
            groupBoxDetails.Controls.Add(labelLastUsed);
            groupBoxDetails.Controls.Add(labelCreated);
            groupBoxDetails.Controls.Add(labelSrtUrl);
            groupBoxDetails.Controls.Add(label6);
            groupBoxDetails.Controls.Add(checkBoxActive);
            groupBoxDetails.Controls.Add(textBoxDescription);
            groupBoxDetails.Controls.Add(label5);
            groupBoxDetails.Controls.Add(textBoxStreamKey);
            groupBoxDetails.Controls.Add(label4);
            groupBoxDetails.Controls.Add(numericPort);
            groupBoxDetails.Controls.Add(label3);
            groupBoxDetails.Controls.Add(textBoxHost);
            groupBoxDetails.Controls.Add(label2);
            groupBoxDetails.Controls.Add(textBoxName);
            groupBoxDetails.Controls.Add(label1);
            groupBoxDetails.Location = new Point(318, 12);
            groupBoxDetails.Name = "groupBoxDetails";
            groupBoxDetails.Size = new Size(350, 400);
            groupBoxDetails.TabIndex = 1;
            groupBoxDetails.TabStop = false;
            groupBoxDetails.Text = "Server Details";
            // 
            // labelLastUsed
            // 
            labelLastUsed.AutoSize = true;
            labelLastUsed.ForeColor = SystemColors.GrayText;
            labelLastUsed.Location = new Point(6, 375);
            labelLastUsed.Name = "labelLastUsed";
            labelLastUsed.Size = new Size(70, 15);
            labelLastUsed.TabIndex = 14;
            labelLastUsed.Text = "Last Used: -";
            // 
            // labelCreated
            // 
            labelCreated.AutoSize = true;
            labelCreated.ForeColor = SystemColors.GrayText;
            labelCreated.Location = new Point(6, 355);
            labelCreated.Name = "labelCreated";
            labelCreated.Size = new Size(61, 15);
            labelCreated.TabIndex = 13;
            labelCreated.Text = "Created: -";
            // 
            // labelSrtUrl
            // 
            labelSrtUrl.AutoSize = true;
            labelSrtUrl.Font = new Font("Consolas", 9F, FontStyle.Bold);
            labelSrtUrl.ForeColor = Color.Blue;
            labelSrtUrl.Location = new Point(6, 330);
            labelSrtUrl.Name = "labelSrtUrl";
            labelSrtUrl.Size = new Size(42, 14);
            labelSrtUrl.TabIndex = 12;
            labelSrtUrl.Text = "srt://";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 315);
            label6.Name = "label6";
            label6.Size = new Size(54, 15);
            label6.TabIndex = 11;
            label6.Text = "SRT URL:";
            // 
            // checkBoxActive
            // 
            checkBoxActive.AutoSize = true;
            checkBoxActive.Location = new Point(95, 280);
            checkBoxActive.Name = "checkBoxActive";
            checkBoxActive.Size = new Size(59, 19);
            checkBoxActive.TabIndex = 10;
            checkBoxActive.Text = "Active";
            checkBoxActive.UseVisualStyleBackColor = true;
            // 
            // textBoxDescription
            // 
            textBoxDescription.Location = new Point(95, 170);
            textBoxDescription.Multiline = true;
            textBoxDescription.Name = "textBoxDescription";
            textBoxDescription.ScrollBars = ScrollBars.Vertical;
            textBoxDescription.Size = new Size(240, 100);
            textBoxDescription.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 173);
            label5.Name = "label5";
            label5.Size = new Size(70, 15);
            label5.TabIndex = 8;
            label5.Text = "Description:";
            // 
            // textBoxStreamKey
            // 
            textBoxStreamKey.Location = new Point(95, 130);
            textBoxStreamKey.Name = "textBoxStreamKey";
            textBoxStreamKey.Size = new Size(240, 23);
            textBoxStreamKey.TabIndex = 7;
            textBoxStreamKey.TextChanged += TextBox_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 133);
            label4.Name = "label4";
            label4.Size = new Size(72, 15);
            label4.TabIndex = 6;
            label4.Text = "Stream Key:";
            // 
            // numericPort
            // 
            numericPort.Location = new Point(95, 90);
            numericPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numericPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericPort.Name = "numericPort";
            numericPort.Size = new Size(120, 23);
            numericPort.TabIndex = 5;
            numericPort.Value = new decimal(new int[] { 9999, 0, 0, 0 });
            numericPort.ValueChanged += NumericPort_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 92);
            label3.Name = "label3";
            label3.Size = new Size(32, 15);
            label3.TabIndex = 4;
            label3.Text = "Port:";
            // 
            // textBoxHost
            // 
            textBoxHost.Location = new Point(95, 50);
            textBoxHost.Name = "textBoxHost";
            textBoxHost.Size = new Size(240, 23);
            textBoxHost.TabIndex = 3;
            textBoxHost.TextChanged += TextBox_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 53);
            label2.Name = "label2";
            label2.Size = new Size(35, 15);
            label2.TabIndex = 2;
            label2.Text = "Host:";
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(95, 22);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(240, 23);
            textBoxName.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 25);
            label1.Name = "label1";
            label1.Size = new Size(42, 15);
            label1.TabIndex = 0;
            label1.Text = "Name:";
            // 
            // buttonOK
            // 
            buttonOK.Location = new Point(512, 428);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(75, 23);
            buttonOK.TabIndex = 2;
            buttonOK.Text = "OK";
            buttonOK.UseVisualStyleBackColor = true;
            buttonOK.Click += ButtonOK_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(593, 428);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 3;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // SrtServerManagerDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(684, 463);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOK);
            Controls.Add(groupBoxDetails);
            Controls.Add(groupBoxServers);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SrtServerManagerDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "SRT Server Manager";
            groupBoxServers.ResumeLayout(false);
            groupBoxDetails.ResumeLayout(false);
            groupBoxDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericPort).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBoxServers;
        private ListBox listBoxServers;
        private Button buttonAdd;
        private Button buttonEdit;
        private Button buttonDelete;
        private Button buttonDuplicate;
        private Button buttonTest;
        private Button buttonAddPreset;
        private GroupBox groupBoxDetails;
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
        private Label labelCreated;
        private Label labelLastUsed;
        private Button buttonOK;
        private Button buttonCancel;
    }
}
