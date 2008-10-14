using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.UI.Win
{
    ///<summary>
    /// Class that provides utility methods for the System.Windows.Forms.DockStyle class
    ///</summary>
    public static class DockStyleWin
    {
        ///<summary>
        /// Gets the Habanero dock style equivalent to the provided System.Windows.Forms dock style
        ///</summary>
        ///<param name="dockStyle">A System.Windows.Forms dock style.</param>
        ///<returns>The equivalent Habanero dock style.</returns>
        public static Base.DockStyle GetDockStyle(System.Windows.Forms.DockStyle dockStyle)
        {
            switch (dockStyle)
            {
                case System.Windows.Forms.DockStyle.Bottom: return Base.DockStyle.Bottom;
                case System.Windows.Forms.DockStyle.Left: return Base.DockStyle.Left;
                case System.Windows.Forms.DockStyle.Right: return Base.DockStyle.Right;
                case System.Windows.Forms.DockStyle.Top: return Base.DockStyle.Top;
                case System.Windows.Forms.DockStyle.Fill: return Base.DockStyle.Fill;
            }
            return (Base.DockStyle)dockStyle;
        }

        ///<summary>
        /// Gets the System.Windows.Forms dock style equivalent to the provided Habanero dock style
        ///</summary>
        ///<param name="dockStyle">A Habanero dock style.</param>
        ///<returns>The equivalent System.Windows.Forms dock style.</returns>
        public static System.Windows.Forms.DockStyle GetDockStyle(Base.DockStyle dockStyle)
        {
            switch (dockStyle)
            {
                case Base.DockStyle.Bottom: return System.Windows.Forms.DockStyle.Bottom;
                case Base.DockStyle.Top: return System.Windows.Forms.DockStyle.Top;
                case Base.DockStyle.Left: return System.Windows.Forms.DockStyle.Left;
                case Base.DockStyle.Right: return System.Windows.Forms.DockStyle.Right;
                case Base.DockStyle.Fill: return System.Windows.Forms.DockStyle.Fill;
            }
            return (System.Windows.Forms.DockStyle)dockStyle;
        }
    }
}
