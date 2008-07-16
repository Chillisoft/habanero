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

using System;
using System.Windows.Forms;

namespace Habanero.UI.Grid
{
    /// <summary>
    /// Represents a column in data grid view
    /// </summary>
    public class DataGridViewNumericUpDownColumn : DataGridViewColumn
    {
        /// <summary>
        /// Constructor to initialise a new column
        /// </summary>
        public DataGridViewNumericUpDownColumn()
            : base(new NumericUpDownCell())
        {
        }

        /// <summary>
        /// Gets and sets the cell template
        /// </summary>
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(NumericUpDownCell)))
                {
                    throw new InvalidCastException("Must be a NumericUpDownCell");
                }
                base.CellTemplate = value;
            }
        }
    }
}