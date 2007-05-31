using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// Manages the layout of controls in a user interface by assigning
    /// them to positions in a grid with rows and columns
    /// </summary>
    public class GridLayoutManager : LayoutManager
    {
        private ControlCollection controls;
        private Hashtable itsControlInfoTable;
        private Point currentPos;
        private int[] colWidths;
        private int[] rowHeights;
        private bool[] fixedColsBasedOnContents;
        private bool fixAllRowsBasedOnContents;
        private bool[] fixedRowsBasedOnContents;

        /// <summary>
        /// Constructor to initialise a new grid layout
        /// </summary>
        /// <param name="managedControl">The control to manage</param>
        public GridLayoutManager(Control managedControl) : base(managedControl)
        {
            controls = new ControlCollection();
            itsControlInfoTable = new Hashtable();
            this.SetGridSize(2, 2);
        }

        /// <summary>
        /// Sets the grid size as a number of rows and columns
        /// </summary>
        /// <param name="rows">The number of rows</param>
        /// <param name="cols">The number of columns</param>
        public void SetGridSize(int rows, int cols)
        {
            colWidths = new int[cols];
            fixedColsBasedOnContents = new bool[cols];
            for (int i = 0; i < colWidths.Length; i++)
            {
                colWidths[i] = -1;
                fixedColsBasedOnContents[i] = false;
            }
            rowHeights = new int[rows];
            fixedRowsBasedOnContents = new bool[rows];
            for (int i = 0; i < rowHeights.Length; i++)
            {
                rowHeights[i] = -1;
                fixedRowsBasedOnContents[i] = false;
            }
        }

        /// <summary>
        /// Returns the number of rows
        /// </summary>
        private int RowCount
        {
            get { return rowHeights.Length; }
        }

        /// <summary>
        /// Returns the number of columns
        /// </summary>
        /// TODO ERIC - rename this to ColumnCount
        private int ColCount
        {
            get { return colWidths.Length; }
        }

        /// <summary>
        /// Returns an IList object containing all the controls row by row
        /// </summary>
        public IList Rows
        {
            get
            {
                IList rows = new ArrayList();
                for (int i = 0; i < RowCount; i++)
                {
                    ControlCollection row = new ControlCollection();
                    for (int j = 0; j < ColCount; j++)
                    {
                        if ((i*ColCount + j) < this.controls.Count)
                        {
                            row.Add(this.controls[i*ColCount + j]);
                        }
                        else
                        {
                            row.Add(null);
                        }
                    }
                    rows.Add(row);
                }
                return rows;
            }
        }

        /// <summary>
        /// Returns an IList object containing all the controls column by column
        /// </summary>
        public IList Columns
        {
            get
            {
                IList cols = new ArrayList();
                for (int i = 0; i < ColCount; i++)
                {
                    ControlCollection col = new ControlCollection();
                    for (int j = 0; j < RowCount; j++)
                    {
                        if ((ColCount*j + i) < this.controls.Count)
                        {
                            col.Add(this.controls[ColCount*j + i]);
                        }
                        else
                        {
                            col.Add(null);
                        }
                    }
                    cols.Add(col);
                }
                return cols;
            }
        }

        /// <summary>
        /// Adds a control at position (1,1)
        /// </summary>
        /// <param name="control">The control to add</param>
        /// <returns>Returns the control once it has been added</returns>
        public Control AddControl(Control control)
        {
            return AddControl(control, 1, 1);
        }

        /// <summary>
        /// Adds a control at the row and column position specified
        /// </summary>
        /// <param name="control">The control to add</param>
        /// <param name="rowSpan">The row position to add to</param>
        /// <param name="colSpan">The column position to add to</param>
        /// <returns>Returns the control once it has been added</returns>
        public Control AddControl(Control control, int rowSpan, int colSpan)
        {
            if (control == null)
            {
                control = new Control();
                control.Visible = false;
            }
            int currentColNum = (this.controls.Count)%ColCount;
            int currentRowNum = (this.controls.Count)/ColCount;
            if (fixedColsBasedOnContents[currentColNum])
            {
                if (control.Width > colWidths[currentColNum])
                {
                    FixColumn(currentColNum, control.Width);
                }
            }
            if (fixAllRowsBasedOnContents)
            {
                if (control.Height > rowHeights[currentRowNum])
                {
                    FixRow(currentRowNum, control.Height);
                }
            }
            else if (fixedRowsBasedOnContents[currentRowNum])
            {
                if (control.Height > rowHeights[currentRowNum])
                {
                    FixRow(currentRowNum, control.Height);
                }
            }
            this.controls.Add(control);
            this.ManagedControl.Controls.Add(control);
            this.itsControlInfoTable.Add(control, new ControlInfo(control, colSpan, rowSpan));
            RefreshControlPositions();
            return control;
        }

        /// <summary>
        /// Updates the positions and settings of the controls in the interface
        /// </summary>
        protected override void RefreshControlPositions()
        {
            currentPos = new Point(BorderSize, BorderSize);
            for (int i = 0; i < controls.Count; i++)
            {
                int currentRow = i/ColCount;
                int currentCol = i%ColCount;
                Control ctl = this.controls[i];
                if ((i > 0) && (currentCol == 0))
                {
                    currentPos.X = BorderSize;
                    currentPos.Y += this.controls[i - 1].Height + GapSize;
                }
                ctl.Left = currentPos.X;
                ctl.Top = currentPos.Y;
                int width = 0;
                ControlInfo ctlInfo = (ControlInfo) itsControlInfoTable[ctl];
                for (int cols = currentCol; cols < Math.Min(this.ColCount, currentCol + ctlInfo.ColSpan); cols++)
                {
                    if (IsFixedColumn(cols))
                    {
                        width += colWidths[cols];
                    }
                    else
                    {
                        width += CalcColumnWidth();
                    }
                }
                width += (this.GapSize*(ctlInfo.ColSpan - 1));
                ctl.Width = width;

                int height = 0;
                for (int rows = currentRow; rows < Math.Min(this.RowCount, currentRow + ctlInfo.RowSpan); rows++)
                {
                    if (IsFixedRow(currentRow))
                    {
                        height += rowHeights[currentRow];
                    }
                    else
                    {
                        height += CalcRowHeight();
                    }
                }
                height += (this.GapSize*(ctlInfo.RowSpan - 1));
                ctl.Height = height;
                currentPos.X += ctl.Width + GapSize;
            }
        }

        /// <summary>
        /// Calculates the average row height
        /// </summary>
        /// <returns>Returns the average row height</returns>
        private int CalcRowHeight()
        {
            return (ManagedControl.Height - GetFixedHeightIncludingGaps())/GetNumVariableRows();
        }

        /// <summary>
        /// Calculates the average column height
        /// </summary>
        /// <returns>Returns the average column height</returns>
        private int CalcColumnWidth()
        {
            return (ManagedControl.Width - GetFixedWidthIncludingGaps())/GetNumVariableColumns();
        }

        /// <summary>
        /// Indicates whether the specified row has a fixed height
        /// </summary>
        /// <param name="rowNum">The row number in question</param>
        /// <returns>Returns true if fixed, false if not</returns>
        private bool IsFixedRow(int rowNum)
        {
            return rowHeights[rowNum] > -1;
        }

        /// <summary>
        /// Indicates whether the specified column has a fixed width
        /// </summary>
        /// <param name="colNum">The column number in question</param>
        /// <returns>Returns true if fixed, false if not</returns>
        private bool IsFixedColumn(int colNum)
        {
            return colWidths[colNum%ColCount] > -1;
        }

        /// <summary>
        /// Fixes the width of a column to a specified size
        /// </summary>
        /// <param name="colNum">The column in question</param>
        /// <param name="colWidth">The width to fix the column at</param>
        public void FixColumn(int colNum, int colWidth)
        {
            colWidths[colNum] = colWidth;
            RefreshControlPositions();
        }

        /// <summary>
        /// Fixes the height of a row to a specified size
        /// </summary>
        /// <param name="rowNum">The row in question</param>
        /// <param name="rowHeight">The height to fix the row at</param>
        public void FixRow(int rowNum, int rowHeight)
        {
            rowHeights[rowNum] = rowHeight;
            RefreshControlPositions();
        }

        /// <summary>
        /// Returns the total width of the fixed-width columns added together
        /// </summary>
        /// <returns>Returns the total width</returns>
        private int GetFixedWidth()
        {
            return GetFixedAmount(colWidths);
        }

        /// <summary>
        /// Returns the total height of the fixed-height rows added together
        /// </summary>
        /// <returns>Returns the total height</returns>
        /// TODO ERIC - not used
        private int GetFixedHeight()
        {
            return GetFixedAmount(rowHeights);
        }

        /// <summary>
        /// Returns the total width of the fixed-width columns added 
        /// together, including the gaps and borders
        /// </summary>
        /// <returns>Returns the total width</returns>
        public int GetFixedWidthIncludingGaps()
        {
            return GetFixedWidth() + (2 * BorderSize) + ((ColCount - 1) * GapSize);
        }
        
        /// <summary>
        /// Returns the total height of the fixed-height rows added 
        /// together, including the gaps and borders
        /// </summary>
        /// <returns>Returns the total height</returns>
        public int GetFixedHeightIncludingGaps()
        {
            return GetFixedAmount(rowHeights) + (2*BorderSize) + ((RowCount - 1)*GapSize);
        }

        /// <summary>
        /// Adds the values in the array provided, as long as the values are
        /// above -1.  This method is used to add up fixed-height/width items.
        /// </summary>
        /// <param name="arr">The array of values</param>
        /// <returns>Returns the total added value</returns>
        private int GetFixedAmount(int[] arr)
        {
            int total = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                int val = arr[i];
                if (val > -1)
                {
                    total += val;
                }
            }
            return total;
        }

        /// <summary>
        /// Counts the number of columns that have not been assigned a
        /// fixed width
        /// </summary>
        /// <returns>Returns the count</returns>
        private int GetNumVariableColumns()
        {
            return GetNumVariableEntries(colWidths);
        }

        /// <summary>
        /// Counts the number of rows that have not been assigned a
        /// fixed height
        /// </summary>
        /// <returns>Returns the count</returns>
        private int GetNumVariableRows()
        {
            return GetNumVariableEntries(rowHeights);
        }

        /// <summary>
        /// Counts the number of items in the array provided that have
        /// a value of -1.  This method is used to count the number of rows
        /// or columns that have not been assigned a fixed width/height
        /// </summary>
        /// <param name="arr">The array of sizes</param>
        /// <returns>Returns the count</returns>
        private int GetNumVariableEntries(int[] arr)
        {
            int num = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                int val = arr[i];
                if (val == -1)
                {
                    num++;
                }
            }
            return num;
        }

        /// <summary>
        /// Fixes a specified column's width based on current or future contents
        /// </summary>
        /// <param name="colNum">The column in question</param>
        public void FixColumnBasedOnContents(int colNum)
        {
            fixedColsBasedOnContents[colNum] = true;
        }

        /// <summary>
        /// Causes the fixed height of all the rows to be determined by the
        /// current or future contents
        /// </summary>
        /// TODO ERIC - a column equivalent?
        public void FixAllRowsBasedOnContents()
        {
            fixAllRowsBasedOnContents = true;
        }

        /// <summary>
        /// Fixes a specified row's height based on current or future contents
        /// </summary>
        /// <param name="rowNum">The row in question</param>
        public void FixRowBasedOnContents(int rowNum)
        {
            fixedRowsBasedOnContents[rowNum] = true;
        }

        /// <summary>
        /// Gets the fixed width set for a specified column.  The return
        /// value will be -1 if the width has not been fixed.
        /// </summary>
        /// <param name="colNum">The column in question</param>
        /// <returns>Returns the fixed width or -1</returns>
        /// TODO ERIC - add a row equivalent
        public int GetFixedColumnWidth(int colNum)
        {
            return this.colWidths[colNum];
        }

        /// <summary>
        /// Manages specific grid information for a control
        /// </summary>
        public class ControlInfo
        {
            private Control itsControl;
            private int itsColSpan;
            private readonly int itsRowSpan;

            /// <summary>
            /// Constructor to initialise a new instance.  Sets the spans 
            /// to (1,1)
            /// </summary>
            /// <param name="control">The control in question</param>
            public ControlInfo(Control control) : this(control, 1, 1)
            {
            }

            /// <summary>
            /// Constructor as before, but requiring the row and column
            /// spans to be specified
            /// </summary>
            public ControlInfo(Control control, int colSpan, int rowSpan)
            {
                itsControl = control;
                itsColSpan = colSpan;
                itsRowSpan = rowSpan;
            }

            /// <summary>
            /// Returns the control being represented
            /// </summary>
            public Control Control
            {
                get { return itsControl; }
            }

            /// <summary>
            /// Returns the row span (how many rows this cell spans across)
            /// </summary>
            public int RowSpan
            {
                get { return itsRowSpan; }
            }

            /// <summary>
            /// Returns the column span (how many columns this cell spans
            /// across)
            /// </summary>
            /// TODO ERIC - rename to ColumnSpan (Col means collection in
            /// some places)
            public int ColSpan
            {
                get { return itsColSpan; }
            }
        }
    }
}