using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// Manages the layout of controls in a user interface by adding
    /// components in the manner of a horizontal text line that wraps to
    /// the next line.
    /// </summary>
    public class FlowLayoutManager : LayoutManager
    {
        private ControlCollection controls;
        private IList itsNewLinePositions;
        private Alignments alignment;
        private Point currentPos;
        private bool itsIsFlowDown = false;
        private IList itsGluePositions;

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
            controls = new ControlCollection();
            itsNewLinePositions = new ArrayList(3);
            itsGluePositions = new ArrayList(5);
        }

        /// <summary>
        /// Adds a control to the layout
        /// </summary>
        /// <param name="ctl">The control to add</param>
        /// <returns>Returns the control once it has been added</returns>
        public Control AddControl(Control ctl)
        {
            controls.Add(ctl);
            RefreshControlPositions();
            ctl.VisibleChanged += new EventHandler(ControlVisibleChangedHandler);
            ctl.Resize += new EventHandler(ControlResizedHandler);
            //if (alignment == Alignments.Right)
            //{
            //    this.ManagedControl.Controls.Clear();
            //    for (int i = controls.Count - 1; i >= 0; i--)
            //    {
            //        Control control = controls[i];
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
            controls.Remove(ctl);
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
            if (itsIsFlowDown)
            {
                foreach (Control control in controls)
                {
                    control.Width = this.ManagedControl.Width;
                }
            }
            currentPos = new Point(BorderSize, BorderSize);
            int rowStart = 0;
            int lastVisible = 0;
            int currentRowHeight = 0;
            int currentLine = 0;
            IList controlsInRow = new ArrayList();
            for (int i = 0; i < this.controls.Count; i++)
            {
                Control ctl = this.controls[i];
                if (currentLine < itsNewLinePositions.Count && (int) itsNewLinePositions[currentLine] == i)
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
                            if (this.controls[i - 1].Left > BorderSize && this.controls[i - 1].Visible &&
                                this.controls[i - 1].Width + this.controls[i].Width + BorderSize + BorderSize + GapSize <
                                this.ManagedControl.Width)
                            {
                                i--;
                                ctl = this.controls[i];
                            }
                        }
                        if (alignment == Alignments.Centre)
                        {
                            ShiftControlsRightForCentering(rowStart, i - 1);
                        }
                        MoveCurrentPosToNextRow(currentRowHeight);
                        currentRowHeight = 0;
                        if (alignment == Alignments.Right)
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
                    currentPos.X += ctl.Width + GapSize;
                    if (ctl.Height > currentRowHeight)
                    {
                        currentRowHeight = ctl.Height;
                    }
                    lastVisible = i;
                }
                if (alignment == Alignments.Centre)
                {
                    if ((i == this.controls.Count - 1) && (lastVisible >= rowStart))
                    {
                        ShiftControlsRightForCentering(rowStart, lastVisible);
                    }
                } 
            }
            if (alignment == Alignments.Right && rowStart == 0)
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
            currentPos.X = BorderSize;
            currentPos.Y += currentRowHeight + GapSize;
        }

        /// <summary>
        /// Calculates the control's position in the user interface
        /// </summary>
        /// <param name="ctl">The control in question</param>
        private void CalculateControlPosition(Control ctl)
        {
            if (alignment == Alignments.Right)
            {
                ctl.Left = ManagedControl.Width - currentPos.X - ctl.Width;
            }
            else
            {
                ctl.Left = currentPos.X;
            }
            ctl.Top = currentPos.Y;
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
                this.controls[ctlNum].Left += (ManagedControl.Width - this.controls[endControlNum].Right - BorderSize)/2;
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
            return (currentPos.X + ctl.Width >= ManagedControl.Width - BorderSize);
        }

        /// <summary>
        /// Edits the current alignment setting
        /// </summary>
        public Alignments Alignment
        {
            set
            {
                alignment = value;
                RefreshControlPositions();
            }
        }

        /// <summary>
        /// Edits the flow-down setting
        /// </summary>
        /// TODO ERIC - what is this?
        public bool FlowDown
        {
            set { itsIsFlowDown = value; }
        }

        /// <summary>
        /// Inserts a new line
        /// </summary>
        /// TODO ERIC - what is this?
        public void NewLine()
        {
            itsNewLinePositions.Add(this.controls.Count);
        }

        /// <summary>
        /// Adds "glue" to the current position, which fills the space
        /// between components
        /// </summary>
        /// TODO ERIC - double-check this
        public void AddGlue()
        {
            itsGluePositions.Add(this.controls.Count);
        }

        /// <summary>
        /// Indicates if the position specified has been "glued"
        /// </summary>
        /// <param name="pos">The position in question</param>
        /// <returns>Returns true if "glued", false if not</returns>
        /// TODO ERIC - review
        private bool IsGlueAtPosition(int pos)
        {
            foreach (int gluePosition in itsGluePositions)
            {
                if (gluePosition == pos) return true;
            }
            return false;
        }
    }
}