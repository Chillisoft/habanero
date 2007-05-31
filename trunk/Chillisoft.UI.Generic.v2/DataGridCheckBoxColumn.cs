//using System;
//using System.Drawing;
//using System.Windows.Forms;
//using log4net;

//namespace Chillisoft.UI.Generic.v2
//{
//    /// <summary>
//    /// Summary description for DataGridComboBoxColumn.
//    /// </summary>
//    // This example shows how to create your own column style that
//    // hosts a control, in this case, a DateTimePicker.
//    public class DataGridCheckBoxColumn : DataGridColumnStyle
//    {
//        private static readonly ILog log = LogManager.GetLogger("CorChillisoft.Generic.DataGridCheckBoxColumn");
//        private CheckBox itsCheckBox = new CheckBox();
//        // The isEditing field tracks whether or not the user is
//        // editing data with the hosted control.
//        private bool isEditing;
//        private Brush outlineBrush = new SolidBrush(Color.Black);
//        private Brush insideBrush = new SolidBrush(Color.White);

//        public DataGridCheckBoxColumn() : base()
//        {
//            //log.Debug("DataGridCheckBoxColumn created.");
//            itsCheckBox.Visible = false;
//        }

//        protected override void Abort(int rowNum)
//        {
//            isEditing = false;
//            itsCheckBox.CheckedChanged -= new EventHandler(CheckBoxValueChanged);
//            Invalidate();
//        }

//        protected override bool Commit(CurrencyManager dataSource, int rowNum)
//        {
//            //log.Debug("Commit() called in DataGridCheckBoxColumn...");
//            itsCheckBox.Bounds = Rectangle.Empty;
//            itsCheckBox.CheckedChanged -= new EventHandler(CheckBoxValueChanged);

//            if (!isEditing)
//            {
//                return true;
//            }

//            isEditing = false;

//            try
//            {
//                bool value = itsCheckBox.Checked;
//                SetColumnValueAtRow(dataSource, rowNum, value);
//            }
//            catch (Exception)
//            {
//                log.Info("Error setting column value.  Aborting and returning false");
//                Abort(rowNum);
//                return false;
//            }

//            Invalidate();
//            //log.Debug("Commit() complete.  Returning true");
//            return true;
//        }

//        protected override void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly,
//                                     string instantText, bool cellIsVisible)
//        {
//            //log.Debug("Edit() called in DataGridCheckBoxColumn...");
//            object val = GetColumnValueAtRow(source, rowNum);
//            bool value;
//            if (DBNull.Value.Equals(val))
//            {
//                value = false;
//            }
//            else
//            {
//                value = Convert.ToBoolean(val);
//            }


//            //log.Debug("value is " + value);

//            if (cellIsVisible)
//            {
//                int midx = bounds.Width/2 - (6);
//                int midy = bounds.Height/2 - (6);
//                itsCheckBox.Bounds = new Rectangle(new Point(bounds.X + midx, bounds.Y + midy), new Size(12, 12));
//                itsCheckBox.Checked = value;
//                itsCheckBox.Visible = true;
//                itsCheckBox.BringToFront();
//                itsCheckBox.CheckedChanged += new EventHandler(CheckBoxValueChanged);
//            }
//            else
//            {
//                itsCheckBox.Checked = value;
//                itsCheckBox.Visible = false;
//            }

//            if (itsCheckBox.Visible)
//            {
//                DataGridTableStyle.DataGrid.Invalidate(bounds);
//            }
//            //log.Debug("Edit() complete...");
//        }

//        protected override Size GetPreferredSize(
//            Graphics g,
//            object value)
//        {
//            return new Size(itsCheckBox.Width, itsCheckBox.Height + 4);
//        }

//        protected override int GetMinimumHeight()
//        {
//            return itsCheckBox.Height + 4;
//        }

//        protected override int GetPreferredHeight(Graphics g,
//                                                  object value)
//        {
//            return itsCheckBox.Height + 4;
//        }

//        protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum)
//        {
//            Paint(g, bounds, source, rowNum, false);
//        }

//        protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum,
//                                      bool alignToRight)
//        {
//            Paint(g, bounds, source, rowNum, Brushes.Red, Brushes.Blue, alignToRight);
//        }

//        protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, Brush backBrush,
//                                      Brush foreBrush, bool alignToRight)
//        {
//            Rectangle backgroundRect = bounds;
//            g.FillRectangle(backBrush, backgroundRect);
//            object val = GetColumnValueAtRow(source, rowNum);
//            bool value;
//            if (DBNull.Value.Equals(val))
//            {
//                value = false;
//            }
//            else
//            {
//                value = Convert.ToBoolean(val);
//            }

//            int midx = bounds.Width/2 - (5);
//            int midy = bounds.Height/2 - (5);
//            Rectangle outlineRect = new Rectangle(new Point(bounds.X + midx, bounds.Y + midy), new Size(10, 10));
//            g.FillRectangle(outlineBrush, outlineRect);
//            if (!value)
//            {
//                Rectangle insideRect =
//                    new Rectangle(outlineRect.X + 1, outlineRect.Y + 1, outlineRect.Width - 2, outlineRect.Height - 2);
//                g.FillRectangle(insideBrush, insideRect);
//            }
//        }

//        protected override void SetDataGridInColumn(DataGrid value)
//        {
//            base.SetDataGridInColumn(value);
//            if (itsCheckBox.Parent != null)
//            {
//                itsCheckBox.Parent.Controls.Remove(itsCheckBox);
//            }
//            if (value != null)
//            {
//                value.Controls.Add(itsCheckBox);
//            }
//        }

//        private void CheckBoxValueChanged(object sender, EventArgs e)
//        {
//            //log.Debug("checkbox value changed to " + itsCheckBox.Checked.ToString());
//            this.isEditing = true;
//            base.ColumnStartedEditing(itsCheckBox);
//        }
//    }
//}