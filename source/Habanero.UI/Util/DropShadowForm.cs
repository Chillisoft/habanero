using System;
using System.Security.Permissions;
using System.Windows.Forms;
using Habanero.UI.Base;

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
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Name = "DropShadowForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
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