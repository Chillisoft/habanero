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
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Manages the layout of controls in a user interface by adding
    /// components in the manner of a horizontal text line that wraps to
    /// the next line.
    /// </summary>
    public class FlowLayoutManager : LayoutManager
    {
        private ControlCollection _controls;
        private IList _newLinePositions;
        private Alignments _alignment;
        private Point _currentPos;
        private bool _isFlowDown = false;
        private IList _gluePositions;

        /// <summary>
        /// An enumeration that indicates whether a control should be
        /// placed to the left, right or centre of the other controls already
        /// placed
        /// </summary>
        public enum Alignments
        {
            Left = 0,
            Right = 1,
            Centre = 2
        }

        /// <summary>
        /// Constructor to initialise a new manager
        /// </summary>
        /// <param name="managedControl">The control to manage</param>
        public FlowLayoutManager(Control managedControl) : base(managedControl)
        {
            _controls = new ControlCollection();
            _newLinePositions = new ArrayList(3);
            _gluePositions = new ArrayList(5);
        }

        /// <summary>
        /// Adds a control to the layout
        /// </summary>
        /// <param name="ctl">The control to add</param>
        /// <returns>Returns the control once it has been added</returns>
        public Control AddControl(Control ctl)
        {
            _controls.Add(ctl);
            RefreshControlPositions();
            ctl.VisibleChanged += new EventHandler(ControlVisibleChangedHandler);
            ctl.Resize += new EventHandler(ControlResizedHandler);
            //if (_alignment == Alignments.Right)
            //{
            //    this.ManagedControl.Controls.Clear();
            //    for (int i = _controls.Count - 1; i >= 0; i--)
            //    {
            //        Control control = _controls[i];
            //        this.ManagedControl.Controls.Add(control);
            //    }
            //}
            //else
            //{
            this.ManagedControl.Controls.Add(ctl);
            //}
            return ctl;
        }

        /// <summary>
        /// Removes the specified control from the layout
        /// </summary>
        /// <param name="ctl">The control to remove</param>
        public void RemoveControl(Control ctl)
        {
            _controls.Remove(ctl);
            this.ManagedControl.Controls.Remove(ctl);
            RefreshControlPositions();
        }

        /// <summary>
        /// A handler called when a control has had its visibility altered
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void ControlVisibleChangedHandler(Object sender, EventArgs e)
        {
            RefreshControlPositions();
        }

        /// <summary>
        /// A handler called when a control has been resized
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void ControlResizedHandler(Object sender, EventArgs e)
        {
            RefreshControlPositions();
        }

        /// <summary>
        /// Updates the layout and appearance of the managed controls
        /// </summary>
        protected override void RefreshControlPositions()
        {
            if (_isFlowDown)
            {
                foreach (Control control in _controls)
                {
                    control.Width = this.ManagedControl.Width;
                }
            }
            _currentPos = new Point(BorderSize, BorderSize);
            int rowStart = 0;
            int lastVisible = 0;
            int currentRowHeight = 0;
            int currentLine = 0;
            IList controlsInRow = new ArrayList();
            for (int i = 0; i < this._controls.Count; i++)
            {
                Control ctl = this._controls[i];
                if (currentLine < _newLinePositions.Count && (int) _newLinePositions[currentLine] == i)
                {
                    MoveCurrentPosToNextRow(currentRowHeight);
                    currentLine++;
                }
                if (ctl.Visible)
                {
                    if (DoesntFitOnCurrentRow(ctl))
                    {
                        if (IsGlueAtPosition(i))
                        {
                            if (this._controls[i - 1].Left > BorderSize && this._controls[i - 1].Visible &&
                                this._controls[i - 1].Width + this._controls[i].Width + BorderSize + BorderSize + GapSize <
                                this.ManagedControl.Width)
                            {
                                i--;
                                ctl = this._controls[i];
                            }
                        }
                        if (_alignment == Alignments.Centre)
                        {
                            ShiftControlsRightForCentering(rowStart, i - 1);
                        }
                        MoveCurrentPosToNextRow(currentRowHeight);
                        currentRowHeight = 0;
                        if (_alignment == Alignments.Right)
                        {
                            for (int ctlCount = 0; ctlCount < controlsInRow.Count; ctlCount++)
                            {
                                //Control controlInRow = (Control) controlsInRow[ctlCount];
                                Control controlInRow = (Control)controlsInRow[controlsInRow.Count - 1 - ctlCount];
                                {
                                    controlInRow.TabIndex = rowStart + ctlCount;
                                }
                            }
                        }
                        controlsInRow.Clear();
                        rowStart = i;

                    }
                    controlsInRow.Add(ctl);
                    CalculateControlPosition(ctl);
                    _currentPos.X += ctl.Width + GapSize;
                    if (ctl.Height > currentRowHeight)
                    {
                        currentRowHeight = ctl.Height;
                    }
                    lastVisible = i;
                }
                if (_alignment == Alignments.Centre)
                {
                    if ((i == this._controls.Count - 1) && (lastVisible >= rowStart))
                    {
                        ShiftControlsRightForCentering(rowStart, lastVisible);
                    }
                } 
            }
            if (_alignment == Alignments.Right && rowStart == 0)
            {
                for (int ctlCount = 0; ctlCount < controlsInRow.Count; ctlCount++)
                {
                    Control controlInRow = (Control)controlsInRow[controlsInRow.Count - 1 - ctlCount];
                    {
                        controlInRow.TabIndex = rowStart + ctlCount;
                    }
                }
            }
        }

        /// <summary>
        /// Moves the current placement position to the next row of items
        /// </summary>
        /// <param name="currentRowHeight">The current row height</param>
        private void MoveCurrentPosToNextRow(int currentRowHeight)
        {
            _currentPos.X = BorderSize;
            _currentPos.Y += currentRowHeight + GapSize;
        }

        /// <summary>
        /// Calculates the control's position in the user interface
        /// </summary>
        /// <param name="ctl">The control in question</param>
        private void CalculateControlPosition(Control ctl)
        {
            if (_alignment == Alignments.Right)
            {
                ctl.Left = ManagedControl.Width - _currentPos.X - ctl.Width;
            }
            else
            {
                ctl.Left = _currentPos.X;
            }
            ctl.Top = _currentPos.Y;
        }

        /// <summary>
        /// Shift controls right when centred is alignment is used
        /// </summary>
        /// <param name="startControlNum">The starting control number</param>
        /// <param name="endControlNum">The ending control number</param>
        private void ShiftControlsRightForCentering(int startControlNum, int endControlNum)
        {
            for (int ctlNum = startControlNum; ctlNum <= endControlNum; ctlNum++)
            {
                this._controls[ctlNum].Left += (ManagedControl.Width - this._controls[endControlNum].Right - BorderSize)/2;
            }
        }

        /// <summary>
        /// Informs if the specified control fails to fit on the current row
        /// of controls
        /// </summary>
        /// <param name="ctl">The control in question</param>
        /// <returns>Returns true if the item doesn't fit, false if it 
        /// does</returns>
        private bool DoesntFitOnCurrentRow(Control ctl)
        {
            return (_currentPos.X + ctl.Width >= ManagedControl.Width - BorderSize);
        }

        /// <summary>
        /// Edits the current alignment setting
        /// </summary>
        public Alignments Alignment
        {
            set
            {
                _alignment = value;
                RefreshControlPositions();
            }
        }

        /// <summary>
        /// Edits the flow-down setting
        /// </summary>
        /// TODO ERIC - what is this?
        public bool FlowDown
        {
            set { _isFlowDown = value; }
        }

        /// <summary>
        /// Inserts a new line
        /// </summary>
        /// TODO ERIC - what is this?
        public void NewLine()
        {
            _newLinePositions.Add(this._controls.Count);
        }

        /// <summary>
        /// Adds "glue" to the current position, which fills the space
        /// between components
        /// </summary>
        /// TODO ERIC - double-check these comments
        public void AddGlue()
        {
            _gluePositions.Add(this._controls.Count);
        }

        /// <summary>
        /// Indicates if the position specified has been "glued"
        /// </summary>
        /// <param name="pos">The position in question</param>
        /// <returns>Returns true if "glued", false if not</returns>
        /// TODO ERIC - review comments
        private bool IsGlueAtPosition(int pos)
        {
            foreach (int gluePosition in _gluePositions)
            {
                if (gluePosition == pos) return true;
            }
            return false;
        }
    }
}