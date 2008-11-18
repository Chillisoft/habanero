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

namespace Habanero.UI.Base
{
    /// <summary>
    /// Represents a small rectangular pop-up window that displays a brief
    /// description of a control's purpose when the user rests the pointer on the control
    /// </summary>
    public interface IToolTip
    {
        /// <summary>
        /// Associates ToolTip text with the specified control
        /// </summary>
        /// <param name="controlHabanero">The Control to associate the ToolTip text with</param>
        /// <param name="toolTipText">The ToolTip text to display when the pointer is on the control</param>
        void SetToolTip(IControlHabanero controlHabanero, string toolTipText);

        /// <summary>
        /// Retrieves the ToolTip text associated with the specified control
        /// </summary>
        /// <param name="controlHabanero">The Control for which to retrieve the ToolTip text</param>
        string GetToolTip(IControlHabanero controlHabanero);
    }
}