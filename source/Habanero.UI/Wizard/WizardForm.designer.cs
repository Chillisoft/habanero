namespace Habanero.UI.Wizard
{
    partial class WizardForm
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
            this.SuspendLayout();
            // 
            // _uxWizardControl
            // 
            this._uxWizardControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._uxWizardControl.Location = new System.Drawing.Point(0, 0);
            this._uxWizardControl.Name = "_uxWizardControl";
            this._uxWizardControl.Size = new System.Drawing.Size(379, 260);
            this._uxWizardControl.TabIndex = 0;
            this._uxWizardControl.WizardController = null;
            this._uxWizardControl.MessagePosted += new WizardControl.MessagePostedDelegate(this._uxWizardControl_MessagePosted);
            this._uxWizardControl.Finished += new WizardControl.FinishedDelegate(this._uxWizardControl_Finished);
            // 
            // WizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Controls.Add(this._uxWizardControl);
            this.Name = "WizardForm";
            this.Text = "WizardForm";
            this.ResumeLayout(false);

        }

        #endregion

        private WizardControl _uxWizardControl = new WizardControl( );

    }
}