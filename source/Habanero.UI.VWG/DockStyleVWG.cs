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
namespace Habanero.UI.VWG
{
    ///<summary>
    /// Class that provides utility methods for the Gizmox.WebGUI.Forms.DockStyle class
    ///</summary>
    public static class DockStyleVWG
    {
        ///<summary>
        /// Gets the Habanero dock style equivalent to the provided Gizmox.WebGUI.Forms dock style
        ///</summary>
        ///<param name="dockStyle">A Gizmox.WebGUI.Forms dock style.</param>
        ///<returns>The equivalent Habanero dock style.</returns>
        public static Base.DockStyle GetDockStyle(Gizmox.WebGUI.Forms.DockStyle dockStyle)
        {
            switch (dockStyle)
            {
                case Gizmox.WebGUI.Forms.DockStyle.None: return Base.DockStyle.None;
                case Gizmox.WebGUI.Forms.DockStyle.Bottom: return Base.DockStyle.Bottom;
                case Gizmox.WebGUI.Forms.DockStyle.Left: return Base.DockStyle.Left;
                case Gizmox.WebGUI.Forms.DockStyle.Right: return Base.DockStyle.Right;
                case Gizmox.WebGUI.Forms.DockStyle.Top: return Base.DockStyle.Top;
                case Gizmox.WebGUI.Forms.DockStyle.Fill: return Base.DockStyle.Fill;
            }
            return (Base.DockStyle)dockStyle;
        }

        ///<summary>
        /// Gets the Gizmox.WebGUI.Forms dock style equivalent to the provided Habanero dock style
        ///</summary>
        ///<param name="dockStyle">A Habanero dock style.</param>
        ///<returns>The equivalent Gizmox.WebGUI.Forms dock style.</returns>
        public static Gizmox.WebGUI.Forms.DockStyle GetDockStyle(Base.DockStyle dockStyle)
        {
            switch (dockStyle)
            {
                case Base.DockStyle.None: return Gizmox.WebGUI.Forms.DockStyle.None;
                case Base.DockStyle.Bottom: return Gizmox.WebGUI.Forms.DockStyle.Bottom;
                case Base.DockStyle.Top: return Gizmox.WebGUI.Forms.DockStyle.Top;
                case Base.DockStyle.Left: return Gizmox.WebGUI.Forms.DockStyle.Left;
                case Base.DockStyle.Right: return Gizmox.WebGUI.Forms.DockStyle.Right;
                case Base.DockStyle.Fill: return Gizmox.WebGUI.Forms.DockStyle.Fill;
            }
            return (Gizmox.WebGUI.Forms.DockStyle)dockStyle;
        }
    }
}
