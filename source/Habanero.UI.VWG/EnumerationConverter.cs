//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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

using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    ///<summary>
    /// Class that provides utility to convert enumerations that differ between VWG and Win.
    ///</summary>
    public class EnumerationConverter
    {
        ///<summary>
        /// Converts Habanero.UI.Base.HorizontalAlignment to Gizmox.WebGUI.Forms.HorizontalAlignment
        ///</summary>
        ///<param name="alignment">A Habanero.UI.Base Horizontal Alignment</param>
        ///<returns>The equivalent VWG Horizontal Alignment</returns>
        public static Gizmox.WebGUI.Forms.HorizontalAlignment HorizontalAlignmentToVWG(HorizontalAlignment alignment)
        {
            if (alignment == HorizontalAlignment.Right)
            {
                return Gizmox.WebGUI.Forms.HorizontalAlignment.Right;
            }
            if (alignment == HorizontalAlignment.Center)
            {
                return Gizmox.WebGUI.Forms.HorizontalAlignment.Center;
            }
            return Gizmox.WebGUI.Forms.HorizontalAlignment.Left;
        }

        ///<summary>
        /// Converts Gizmox.WebGUI.Forms.HorizontalAlignment to Converts Habanero.UI.Base.HorizontalAlignment
        ///</summary>
        ///<param name="alignment">A Gizmox.WebGUI.Forms Horizontal Alignment</param>
        ///<returns>The equivalent Habanero Horizontal Alignment</returns>
        public static HorizontalAlignment HorizontalAlignmentToHabanero(Gizmox.WebGUI.Forms.HorizontalAlignment alignment)
        {
            if (alignment == Gizmox.WebGUI.Forms.HorizontalAlignment.Right)
            {
                return HorizontalAlignment.Right;
            }
            if (alignment == Gizmox.WebGUI.Forms.HorizontalAlignment.Center)
            {
                return HorizontalAlignment.Center;
            }
            return HorizontalAlignment.Left;
        }
    }
}