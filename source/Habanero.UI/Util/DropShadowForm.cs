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


using System;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Habanero.UI.Util
{
    /// <summary>
    /// Provides a form that has a drop shadow
    /// </summary>
    public class DropShadowForm : Form
    {
        private const int CS_DROPSHADOW = 0x00020000;

        /// <summary>
        /// Initialises the form
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // DropShadowForm
            // 
            this.AutoScaleBaseSize = new Size(5, 13);
            this.ClientSize = new Size(292, 266);
            this.Name = "DropShadowForm";
            this.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// Constructor to initialise a new instance of the form
        /// </summary>
        public DropShadowForm()
        {
        }

        /// <summary>
        /// Creates the necessary parameters for the form
        /// </summary>
        protected override CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode=true)]
            get
            {
                CreateParams parameters = base.CreateParams;

                if (DropShadowSupported)
                {
                    parameters.ClassStyle = (parameters.ClassStyle | CS_DROPSHADOW);
                }

                return parameters;
            }
        }

        /// <summary>
        /// Indicates whether a drop shadow is supported (ie. for Windows XP
        /// and above)
        /// </summary>
        public static bool DropShadowSupported
        {
            get { return IsWindowsXPOrAbove; }
        }

        /// <summary>
        /// Indicates whether the operating system is Windows XP or above
        /// </summary>
        public static bool IsWindowsXPOrAbove
        {
            get
            {
                OperatingSystem system = Environment.OSVersion;
                bool runningNT = system.Platform == PlatformID.Win32NT;

                return runningNT && system.Version.CompareTo(new Version(5, 1, 0, 0)) >= 0;
            }
        }
    }
}