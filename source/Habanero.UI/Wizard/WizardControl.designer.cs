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
            this.uxNextButton = new System.Windows.Forms.Button();
            this.uxPreviousButton = new System.Windows.Forms.Button();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.pnlWizardStep = new System.Windows.Forms.Panel();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
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
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.uxPreviousButton);
            this.pnlButtons.Controls.Add(this.uxNextButton);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 239);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(338, 44);
            this.pnlButtons.TabIndex = 1;
            // 
            // pnlWizardStep
            // 
            this.pnlWizardStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWizardStep.Location = new System.Drawing.Point(0, 0);
            this.pnlWizardStep.Name = "pnlWizardStep";
            this.pnlWizardStep.Size = new System.Drawing.Size(338, 239);
            this.pnlWizardStep.TabIndex = 0;
            // 
            // WizardControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlWizardStep);
            this.Controls.Add(this.pnlButtons);
            this.Name = "WizardControl";
            this.Size = new System.Drawing.Size(338, 283);
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button uxNextButton;
        private System.Windows.Forms.Button uxPreviousButton;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Panel pnlWizardStep;
    }
}
