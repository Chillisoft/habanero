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
