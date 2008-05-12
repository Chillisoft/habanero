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
using System.Windows.Forms;

namespace Habanero.UI
{
    /// <summary>
    /// Manages a layout with multiple columns
    /// </summary>
    public class ColumnLayoutManager : UI.LayoutManager
    {
        private UI.ControlCollection _controls;
        private int _numCols;

        /// <summary>
        /// Constructor to initialise a new layout manager
        /// </summary>
        /// <param name="managedControl">The control to manage</param>
        public ColumnLayoutManager(Control managedControl) : base(managedControl)
        {
            _controls = new UI.ControlCollection();
            _numCols = 1;
        }

        /// <summary>
        /// Add a control to those being managed in the layout
        /// </summary>
        /// <param name="control">The control to add</param>
        /// <returns>Returns the control after it has been added</returns>
        public Control AddControl(Control control)
        {
            _controls.Add(control);
            this.ManagedControl.Controls.Add(control);
            RefreshControlPositions();
            return control;
        }

        /// <summary>
        /// Remove the specified control from those being managed in the layout
        /// </summary>
        /// <param name="control">The control to be removed</param>
        public void RemoveControl(Control control)
        {
            _controls.Remove(control);
            this.ManagedControl.Controls.Remove(control);
            RefreshControlPositions();
        }

        /// <summary>
        /// Updates the positions of the controls in the layout
        /// </summary>
        protected override void RefreshControlPositions()
        {
            double colWidth = (this.ManagedControl.Width - (2*this.BorderSize) - ((_numCols - 1)*this.GapSize))/
                              _numCols;
            int currentCol = 0;
            int currentTop = this.BorderSize;
            int highestControlheight = 0;
            foreach (Control control in _controls)
            {
                control.Width = Convert.ToInt32(Math.Round(colWidth));
                control.Left =
                    Convert.ToInt32(Math.Round(this.BorderSize + (currentCol*colWidth) + (currentCol*this.GapSize)));
                control.Top = currentTop;
                if (highestControlheight < control.Height)
                {
                    highestControlheight = control.Height;
                }
                currentCol = (currentCol + 1)%_numCols;
                if (currentCol == 0)
                {
                    currentTop = currentTop + highestControlheight + this.GapSize;
                    highestControlheight = 0;
                }
            }
        }

        /// <summary>
        /// Sets the number of columns in the layout
        /// </summary>
        /// <param name="numCols">The number of columns to set to</param>
        public void SetColumns(int numCols)
        {
            _numCols = numCols;
        }
    }
}