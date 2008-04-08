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


using Habanero.DB;
using Habanero.UI.Base;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Provides a control form listing database settings
    /// </summary>
    public class DbConnectionControl : System.Windows.Forms.UserControl
    {
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PortTextBox;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.TextBox UserNameTextBox;
        private System.Windows.Forms.TextBox DatabaseTextBox;
        private System.Windows.Forms.TextBox ServerTextBox;
        private System.Windows.Forms.TextBox VendorTextBox;

        /// <summary> 
        /// Required designer variable
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// Constructor to initialise a new connection control
        /// </summary>
        public DbConnectionControl()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
        }

        /// <summary> 
        /// Cleans up any resources being used
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.PortTextBox = new System.Windows.Forms.TextBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.UserNameTextBox = new System.Windows.Forms.TextBox();
            this.DatabaseTextBox = new System.Windows.Forms.TextBox();
            this.ServerTextBox = new System.Windows.Forms.TextBox();
            this.VendorTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(16, 176);
            this.label6.Name = "label6";
            this.label6.TabIndex = 35;
            this.label6.Text = "Port:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(16, 144);
            this.label5.Name = "label5";
            this.label5.TabIndex = 34;
            this.label5.Text = "Password:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(16, 112);
            this.label4.Name = "label4";
            this.label4.TabIndex = 33;
            this.label4.Text = "User name:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(16, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 23);
            this.label3.TabIndex = 32;
            this.label3.Text = "Database:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 23);
            this.label2.TabIndex = 31;
            this.label2.Text = "Server:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 23);
            this.label1.TabIndex = 30;
            this.label1.Text = "Vendor";
            // 
            // PortTextBox
            // 
            this.PortTextBox.Location = new System.Drawing.Point(120, 176);
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.TabIndex = 29;
            this.PortTextBox.Text = "3306";
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(120, 144);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.TabIndex = 28;
            this.PasswordTextBox.Text = "";
            // 
            // UserNameTextBox
            // 
            this.UserNameTextBox.Location = new System.Drawing.Point(120, 112);
            this.UserNameTextBox.Name = "UserNameTextBox";
            this.UserNameTextBox.TabIndex = 27;
            this.UserNameTextBox.Text = "root";
            // 
            // DatabaseTextBox
            // 
            this.DatabaseTextBox.Location = new System.Drawing.Point(120, 80);
            this.DatabaseTextBox.Name = "DatabaseTextBox";
            this.DatabaseTextBox.TabIndex = 26;
            this.DatabaseTextBox.Text = "";
            // 
            // ServerTextBox
            // 
            this.ServerTextBox.Location = new System.Drawing.Point(120, 48);
            this.ServerTextBox.Name = "ServerTextBox";
            this.ServerTextBox.TabIndex = 25;
            this.ServerTextBox.Text = "localhost";
            // 
            // VendorTextBox
            // 
            this.VendorTextBox.Enabled = false;
            this.VendorTextBox.Location = new System.Drawing.Point(120, 16);
            this.VendorTextBox.Name = "VendorTextBox";
            this.VendorTextBox.TabIndex = 24;
            this.VendorTextBox.Text = "MySql";
            // 
            // UserControl1
            // 
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PortTextBox);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.UserNameTextBox);
            this.Controls.Add(this.DatabaseTextBox);
            this.Controls.Add(this.ServerTextBox);
            this.Controls.Add(this.VendorTextBox);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(240, 216);
            this.ResumeLayout(false);
        }

        /// <summary>
        /// Returns a database configuration with the settings specified
        /// in the control
        /// </summary>
        /// <returns>Returns a new DatabaseConfig object</returns>
        public DatabaseConfig GetDatabaseConfig()
        {
            return
                new DatabaseConfig(VendorTextBox.Text, ServerTextBox.Text, DatabaseTextBox.Text, UserNameTextBox.Text,
                                   PasswordTextBox.Text, PortTextBox.Text);
        }

        #endregion
    }
}