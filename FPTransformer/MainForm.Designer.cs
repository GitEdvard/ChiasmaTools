namespace FPTransformer
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
            this.QuitButton = new System.Windows.Forms.Button();
            this.TransformButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.InputFileTextBox = new System.Windows.Forms.TextBox();
            this.OutputFileTextBox = new System.Windows.Forms.TextBox();
            this.BrowseInputFileButton = new System.Windows.Forms.Button();
            this.InputFileOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.RPlotFileCheckBox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.RPlotFileTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // QuitButton
            // 
            this.QuitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.QuitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.QuitButton.Location = new System.Drawing.Point(690, 215);
            this.QuitButton.Name = "QuitButton";
            this.QuitButton.Size = new System.Drawing.Size(86, 24);
            this.QuitButton.TabIndex = 0;
            this.QuitButton.Text = "Quit";
            this.QuitButton.UseVisualStyleBackColor = true;
            this.QuitButton.Click += new System.EventHandler(this.QuitButton_Click);
            // 
            // TransformButton
            // 
            this.TransformButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TransformButton.Location = new System.Drawing.Point(12, 215);
            this.TransformButton.Name = "TransformButton";
            this.TransformButton.Size = new System.Drawing.Size(86, 24);
            this.TransformButton.TabIndex = 1;
            this.TransformButton.Text = "Transform";
            this.TransformButton.UseVisualStyleBackColor = true;
            this.TransformButton.Click += new System.EventHandler(this.TransformButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Input file:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Output file:";
            // 
            // InputFileTextBox
            // 
            this.InputFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.InputFileTextBox.Location = new System.Drawing.Point(74, 47);
            this.InputFileTextBox.Name = "InputFileTextBox";
            this.InputFileTextBox.Size = new System.Drawing.Size(615, 20);
            this.InputFileTextBox.TabIndex = 4;
            // 
            // OutputFileTextBox
            // 
            this.OutputFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputFileTextBox.Location = new System.Drawing.Point(74, 95);
            this.OutputFileTextBox.Name = "OutputFileTextBox";
            this.OutputFileTextBox.Size = new System.Drawing.Size(615, 20);
            this.OutputFileTextBox.TabIndex = 5;
            // 
            // BrowseInputFileButton
            // 
            this.BrowseInputFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseInputFileButton.Location = new System.Drawing.Point(710, 46);
            this.BrowseInputFileButton.Name = "BrowseInputFileButton";
            this.BrowseInputFileButton.Size = new System.Drawing.Size(66, 20);
            this.BrowseInputFileButton.TabIndex = 6;
            this.BrowseInputFileButton.Text = "Browse";
            this.BrowseInputFileButton.UseVisualStyleBackColor = true;
            this.BrowseInputFileButton.Click += new System.EventHandler(this.BrowseInputFileButton_Click);
            // 
            // InputFileOpenFileDialog
            // 
            this.InputFileOpenFileDialog.FileName = "openFileDialog1";
            this.InputFileOpenFileDialog.Filter = "Asc files|*.asc|All files|*.*";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(554, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Version:";
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Location = new System.Drawing.Point(605, 18);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(54, 13);
            this.VersionLabel.TabIndex = 8;
            this.VersionLabel.Text = "<Version>";
            // 
            // RPlotFileCheckBox
            // 
            this.RPlotFileCheckBox.AutoSize = true;
            this.RPlotFileCheckBox.Location = new System.Drawing.Point(15, 140);
            this.RPlotFileCheckBox.Name = "RPlotFileCheckBox";
            this.RPlotFileCheckBox.Size = new System.Drawing.Size(130, 17);
            this.RPlotFileCheckBox.TabIndex = 9;
            this.RPlotFileCheckBox.Text = "Create a r-plot file also";
            this.RPlotFileCheckBox.UseVisualStyleBackColor = true;
            this.RPlotFileCheckBox.CheckedChanged += new System.EventHandler(this.RPlotFileCheckBox_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 166);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "r-plot file:";
            // 
            // RPlotFileTextBox
            // 
            this.RPlotFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RPlotFileTextBox.Location = new System.Drawing.Point(74, 163);
            this.RPlotFileTextBox.Name = "RPlotFileTextBox";
            this.RPlotFileTextBox.ReadOnly = true;
            this.RPlotFileTextBox.Size = new System.Drawing.Size(615, 20);
            this.RPlotFileTextBox.TabIndex = 11;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.QuitButton;
            this.ClientSize = new System.Drawing.Size(788, 251);
            this.Controls.Add(this.RPlotFileTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.RPlotFileCheckBox);
            this.Controls.Add(this.VersionLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.BrowseInputFileButton);
            this.Controls.Add(this.OutputFileTextBox);
            this.Controls.Add(this.InputFileTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TransformButton);
            this.Controls.Add(this.QuitButton);
            this.Name = "MainForm";
            this.Text = "FPTransformer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button QuitButton;
        private System.Windows.Forms.Button TransformButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox InputFileTextBox;
        private System.Windows.Forms.TextBox OutputFileTextBox;
        private System.Windows.Forms.Button BrowseInputFileButton;
        private System.Windows.Forms.OpenFileDialog InputFileOpenFileDialog;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.CheckBox RPlotFileCheckBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox RPlotFileTextBox;
    }
}

