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
using System.Collections.Generic;
using System.Drawing;

namespace Habanero.UI.Base.LayoutManagers
{
    /// <summary>
    /// Manages the layout of controls in a user interface by adding
    /// components in the manner of a horizontal text line that wraps to
    /// the next line.
    /// </summary>
    public class FlowLayoutManager : LayoutManager
    {

        private readonly List<IControlChilli> _controls;
        private readonly IList _newLinePositions;
        private Alignments _alignment;
        private Point _currentPos;
        //TODO: Removed Code did not affect tests
        //private bool _isFlowDown = false;
        private readonly IList _gluePositions;

        /// <summary>
        /// An enumeration that indicates whether a control should be
        /// placed to the left, right or centre of the other controls already
        /// placed
        /// </summary>
        public enum Alignments
        {
            ///<summary>
            /// Left alighn the controls
            ///</summary>
            Left = 0,
            ///<summary>
            /// Right align the controls
            ///</summary>
            Right = 1,
            /// <summary>
            /// Centre the controls
            /// </summary>
            Centre = 2
        }

        /// <summary>
        /// Constructor to initialise a new manager
        /// </summary>
        /// <param name="managedControl">The control to manage e.g. a Panel</param>
        public FlowLayoutManager(IControlChilli managedControl) : base(managedControl)
        {
            _controls = new List<IControlChilli>();
            _newLinePositions = new ArrayList(3);
            _gluePositions = new ArrayList(5);
        }

        /// <summary>
        /// Adds a control to the layout
        /// </summary>
        /// <param name="ctl">The control to add</param>
        /// <returns>Returns the control once it has been added</returns>
        public override IControlChilli AddControl(IControlChilli ctl)
        {
            _controls.Add(ctl);
            RefreshControlPositions();
            ctl.VisibleChanged += ControlVisibleChangedHandler;
            ctl.Resize += ControlResizedHandler;
            this.ManagedControl.Controls.Add(ctl);
            return ctl;
        }

        //TODO: This has been commented out since there are not tests for it.
        //If this is to be readded need Tests for Glue, rightaling, centre align and standard align
        ///// <summary>
        ///// Removes the specified control from the layout
        ///// </summary>
        ///// <param name="ctl">The control to remove</param>
        //public void RemoveControl(IControlChilli ctl)
        //{
        //    _controls.Remove(ctl);
        //    this.ManagedControl.Controls.Remove(ctl);
        //    RefreshControlPositions();
        //}

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
            ResetCurrentPosToTopLeft();
            int rowStart = 0;
            int lastVisible = 0;
            int currentRowHeight = 0;
            int currentLine = 0;
            IList controlsInRow = new ArrayList();
            for (int i = 0; i < this._controls.Count; i++)
            {
                IControlChilli ctl = GetControl(i);
                if (currentLine < _newLinePositions.Count && (int) _newLinePositions[currentLine] == i)
                {
                    MoveCurrentPosToStartOfNextRow(currentRowHeight);
                    currentLine++;
                }
                if (ctl.Visible)
                {
                    if (ControlDoesNotFitOnCurrentRow(ctl))
                    {
                        if (IsGluedToAnotherControl(i))
                        {
                            if (PreviousControlVisible(i) && BothControlsFitOnALine(i))
                            {
                                //Get the previous control as this is the one that this control is glued to
                                //E.g. a label and this needs to get moved to the new line.
                                i--;
                                ctl = GetControl(i);
                            }
                        }
                        if (_alignment == Alignments.Centre)
                        {
                            ShiftControlsRightForCentering(rowStart, i - 1);
                        }
                        MoveCurrentPosToStartOfNextRow(currentRowHeight);
                        currentRowHeight = InitialiseNewRow(i, out rowStart, controlsInRow);
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
                SetUpTabIndexForAlignmentRight(rowStart, controlsInRow);
            }
        }

        private static int InitialiseNewRow(int i, out int rowStart, IList controlsInRow)
        {
            int currentRowHeight;
            currentRowHeight = 0;
            controlsInRow.Clear();
            rowStart = i;
            return currentRowHeight;
        }

        private bool PreviousControlVisible(int i)
        {
            return GetPreviousControl(i).Visible;
        }

        private bool BothControlsFitOnALine(int i)
        {
            return GetPreviousControl(i).Width + GetControl(i).Width + BorderSize + BorderSize + GapSize <
                   this.ManagedControl.Width;
        }

        private IControlChilli GetPreviousControl(int currentPos)
        {
            return this._controls[currentPos - 1];
        }

        private static void SetUpTabIndexForAlignmentRight(int rowStart, IList controlsInRow)
        {
            for (int ctlCount = 0; ctlCount < controlsInRow.Count; ctlCount++)
            {
                IControlChilli controlInRow = (IControlChilli)controlsInRow[controlsInRow.Count - 1 - ctlCount];
                {
                    controlInRow.TabIndex = rowStart + ctlCount;
                }
            }
        }

        private IControlChilli GetControl(int position)
        {
            return this._controls[position];
        }

        private void ResetCurrentPosToTopLeft()
        {
            _currentPos = TopLeftPosition();
        }

        private Point TopLeftPosition()
        {
            return new Point(BorderSize, BorderSize);
        }

        /// <summary>
        /// Moves the current placement position to the beginning of the next row of items
        /// </summary>
        /// <param name="currentRowHeight">The current row height</param>
        private void MoveCurrentPosToStartOfNextRow(int currentRowHeight)
        {
            _currentPos.X = BorderSize;
            _currentPos.Y += currentRowHeight + GapSize;
        }

        /// <summary>
        /// Calculates the control's position in the user interface
        /// </summary>
        /// <param name="ctl">The control in question</param>
        private void CalculateControlPosition(IControlChilli ctl)
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
        private bool ControlDoesNotFitOnCurrentRow(IControlChilli ctl)
        {
            return (_currentPos.X + ctl.Width >= ManagedControl.Width - BorderSize);
        }

        /// <summary>
        /// Edits the current alignment setting.  If the controls are right aligned, the first added control
        /// will show at the rightmost point.
        /// <see cref="Alignments"/>
        /// </summary>
        public Alignments Alignment
        {
            set
            {
                _alignment = value;
                RefreshControlPositions();
            }
        }

        //TODO: This has been commented out since there are not tests for it.
        ///// <summary>
        ///// Changes the manager to flow down mode, which makes the controls flow vertically instead of 
        ///// horizontally.
        ///// </summary>
        //public bool FlowDown
        //{
        //    set { _isFlowDown = value; }
        //}

        /// <summary>
        /// Inserts a new line.  This is like a line break or carraige return for controls. The next control
        /// will start at the control's margin (depending on alignment).
        /// </summary>
        public void NewLine()
        {
            _newLinePositions.Add(this._controls.Count);
        }

        /// <summary>
        /// Adds glue that connects two controls together.  For example, if you've just added a label
        /// and want to ensure the textbox you're adding next is always next to the label, add glue
        /// between adding the label and adding the textbox.  In this way, if you resize the control such that
        /// the textbox doesn't fit on the line, both the label and the textbox will move to the next line together.
        /// This must be called immediatly after adding the label. The next control to be added will be the control the
        /// label is glued to.
        /// </summary>
        public void AddGlue()
        {
            _gluePositions.Add(this._controls.Count);
        }

        /// <summary>
        /// Checks for glue at a position (a position being a point between two controls.  
        /// See <see cref="AddGlue"/>
        /// </summary>
        /// <param name="pos">The position to check for glue</param>
        /// <returns></returns>
        private bool IsGluedToAnotherControl(int pos)
        {
            foreach (int gluePosition in _gluePositions)
            {
                if (gluePosition == pos) return true;
            }
            return false;
        }
    }
}