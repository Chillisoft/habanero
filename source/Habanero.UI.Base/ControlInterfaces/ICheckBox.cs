//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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

namespace Habanero.UI.Base
{
    /// <summary>
    /// Represents a CheckBox control
    /// </summary>
    public interface ICheckBox:IControlHabanero
    {
        /// <summary>
        /// Gets or sets whether the CheckBox is checked
        /// </summary>
        bool Checked { get; set; }

        /// <summary>
        /// Gets or sets the horizontal and vertical alignment of the
        /// check mark on a CheckBox control
        /// </summary>
        ContentAlignment CheckAlign { get; set; }

        /// <summary>
        /// The event that is raised when the <see cref="Checked"/> property is changed.
        /// </summary>
        event EventHandler CheckedChanged;
    }
}
