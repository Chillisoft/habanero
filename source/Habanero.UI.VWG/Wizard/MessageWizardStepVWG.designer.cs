// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using Gizmox.WebGUI.Forms;

namespace Habanero.UI.VWG
{
    partial class MessageWizardStepVWG
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
            this._uxMessageLabel = new Label();
            this.SuspendLayout();
            // 
            // _uxMessageLabel
            // 
            this._uxMessageLabel.Location = new System.Drawing.Point(3, 10);
            this._uxMessageLabel.Name = "_uxMessageLabel";
            this._uxMessageLabel.Size = new System.Drawing.Size(303, 123);
            this._uxMessageLabel.TabIndex = 0;
            this._uxMessageLabel.Text = "Please replace this message";
            // 
            // MessageWizardStep
            // 
            //this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._uxMessageLabel);
            this.Name = "MessageWizardStep";
            this.Size = new System.Drawing.Size(322, 300);
            this.ResumeLayout(false);

        }

        #endregion
        /// <summary>
        /// The label that is used to show the message in the wizard step
        /// </summary>
        protected Label _uxMessageLabel;
    }
}