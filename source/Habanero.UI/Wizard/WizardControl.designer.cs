namespace Habanero.UI.Wizard
{
    partial class WizardControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.uxNextButton = new System.Windows.Forms.Button();
            this.uxPreviousButton = new System.Windows.Forms.Button();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(5);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.uxNextButton);
            this.splitContainer1.Panel2.Controls.Add(this.uxPreviousButton);
            this.splitContainer1.Size = new System.Drawing.Size(338, 283);
            this.splitContainer1.SplitterDistance = 235;
            this.splitContainer1.TabIndex = 0;
            // 
            // uxNextButton
            // 
            this.uxNextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.uxNextButton.Location = new System.Drawing.Point(260, 3);
            this.uxNextButton.Name = "uxNextButton";
            this.uxNextButton.Size = new System.Drawing.Size(75, 38);
            this.uxNextButton.TabIndex = 1;
            this.uxNextButton.Text = "Next";
            this.uxNextButton.UseVisualStyleBackColor = true;
            this.uxNextButton.Click += new System.EventHandler(this.uxNextButton_Click);
            // 
            // uxPreviousButton
            // 
            this.uxPreviousButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.uxPreviousButton.Location = new System.Drawing.Point(179, 3);
            this.uxPreviousButton.Name = "uxPreviousButton";
            this.uxPreviousButton.Size = new System.Drawing.Size(75, 38);
            this.uxPreviousButton.TabIndex = 0;
            this.uxPreviousButton.Text = "Previous";
            this.uxPreviousButton.UseVisualStyleBackColor = true;
            this.uxPreviousButton.Click += new System.EventHandler(this.uxPreviousButton_Click);
            // 
            // WizardControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "WizardControl";
            this.Size = new System.Drawing.Size(338, 283);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button uxNextButton;
        private System.Windows.Forms.Button uxPreviousButton;
    }
}
