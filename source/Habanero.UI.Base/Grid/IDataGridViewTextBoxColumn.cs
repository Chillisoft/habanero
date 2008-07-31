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

using System.ComponentModel;

namespace Habanero.UI.Base
{

    public interface IDataGridViewTextBoxColumn
    {
        /// <summary>Gets or sets the maximum number of characters that can be entered into the text box.</summary>
        /// <returns>The maximum number of characters that can be entered into the text box; the default value is 32767.</returns>
        /// <exception cref="T:System.InvalidOperationException">The value of the CellTemplate property is null.</exception>
        [DefaultValue(0x7fff)]
        int MaxInputLength { get; set; }
    }
}