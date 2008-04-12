//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------


using Gizmox.WebGUI.Forms;

namespace Habanero.WebGUI.Wizard
{
    partial class WizardControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private readonly System.ComponentModel.IContainer components = null;

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
            this.uxNextButton = new Button();
            this.uxPreviousButton = new Button();
            this.pnlButtons = new Panel();
            this.pnlWizardStep = new Panel();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // uxNextButton
            // 
            this.uxNextButton.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.uxNextButton.Location = new System.Drawing.Point(260, 3);
            this.uxNextButton.Name = "uxNextButton";
            this.uxNextButton.Size = new System.Drawing.Size(75, 38);
            this.uxNextButton.TabIndex = 1;
            this.uxNextButton.Text = "Next";
            //this.uxNextButton.UseVisualStyleBackColor = true;
            this.uxNextButton.Click += new System.EventHandler(this.uxNextButton_Click);
            // 
            // uxPreviousButton
            // 
            this.uxPreviousButton.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.uxPreviousButton.Location = new System.Drawing.Point(179, 3);
            this.uxPreviousButton.Name = "uxPreviousButton";
            this.uxPreviousButton.Size = new System.Drawing.Size(75, 38);
            this.uxPreviousButton.TabIndex = 0;
            this.uxPreviousButton.Text = "Previous";
           // this.uxPreviousButton.UseVisualStyleBackColor = true;
            this.uxPreviousButton.Click += new System.EventHandler(this.uxPreviousButton_Click);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.uxPreviousButton);
            this.pnlButtons.Controls.Add(this.uxNextButton);
            this.pnlButtons.Dock = DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 239);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(338, 44);
            this.pnlButtons.TabIndex = 1;
            // 
            // pnlWizardStep
            // 
            this.pnlWizardStep.Dock = DockStyle.Fill;
            this.pnlWizardStep.Location = new System.Drawing.Point(0, 0);
            this.pnlWizardStep.Name = "pnlWizardStep";
            this.pnlWizardStep.Size = new System.Drawing.Size(338, 239);
            this.pnlWizardStep.TabIndex = 0;
            // 
            // WizardControl
            // 
            //this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlWizardStep);
            this.Controls.Add(this.pnlButtons);
            this.Name = "WizardControl";
            this.Size = new System.Drawing.Size(338, 283);
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Button uxNextButton;
        private Button uxPreviousButton;
        private Panel pnlButtons;
        private Panel pnlWizardStep;
    }
}
