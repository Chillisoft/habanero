//using System;
//using System.Drawing;
//using System.Windows.Forms;

//namespace Habanero.Ui.Generic
//{
//    // This example shows how to create your own column style that
//    // hosts a control, in this case, a DateTimePicker.
//    public class DataGridDateTimePickerColumn : DataGridColumnStyle
//    {
//        private DateTimePicker itsDateTimePicker = new DateTimePicker();
//        // The isEditing field tracks whether or not the user is
//        // editing data with the hosted control.
//        private bool isEditing;

//        public DataGridDateTimePickerColumn() : base()
//        {
//            itsDateTimePicker.Visible = false;
//        }

//        protected override void Abort(int rowNum)
//        {
//            isEditing = false;
//            itsDateTimePicker.ValueChanged -= new EventHandler(TimePickerValueChanged);
//            Invalidate();
//        }

//        protected override bool Commit
//            (CurrencyManager dataSource, int rowNum)
//        {
//            itsDateTimePicker.Bounds = Rectangle.Empty;

//            itsDateTimePicker.ValueChanged -= new EventHandler(TimePickerValueChanged);

//            if (!isEditing)
//            {
//                return true;
//            }

//            isEditing = false;

//            try
//            {
//                DateTime value = itsDateTimePicker.Value;
//                SetColumnValueAtRow(dataSource, rowNum, value);
//            }
//            catch (Exception)
//            {
//                Abort(rowNum);
//                return false;
//            }

//            Invalidate();
//            return true;
//        }


//        protected override void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly,
//                                     string instantText, bool cellIsVisible)
//        {
//            object value;
//            value = GetColumnValueAtRow(source, rowNum);
//            if (DBNull.Value.Equals(value))
//            {
//                value = DateTime.Now;
//            }
//            if (cellIsVisible)
//            {
//                itsDateTimePicker.Bounds =
//                    new Rectangle(bounds.X + 2, bounds.Y + 2, bounds.Width - 4, bounds.Height - 4);
//                itsDateTimePicker.Value = Convert.ToDateTime(value);
//                itsDateTimePicker.Visible = true;
//                itsDateTimePicker.ValueChanged += new EventHandler(TimePickerValueChanged);
//            }
//            else
//            {
//                itsDateTimePicker.Value = Convert.ToDateTime(value);
//                itsDateTimePicker.Visible = false;
//            }

//            if (itsDateTimePicker.Visible)
//            {
//                DataGridTableStyle.DataGrid.Invalidate(bounds);
//            }
//        }

//        protected override Size GetPreferredSize(Graphics g, object value)
//        {
//            return new Size(100, itsDateTimePicker.PreferredHeight + 4);
//        }

//        protected override int GetMinimumHeight()
//        {
//            return itsDateTimePicker.PreferredHeight + 4;
//        }

//        protected override int GetPreferredHeight(Graphics g, object value)
//        {
//            return itsDateTimePicker.PreferredHeight + 4;
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
//            object value;
//            value = GetColumnValueAtRow(source, rowNum);
//            if (DBNull.Value.Equals(value))
//            {
//                value = DateTime.Now;
//            }

//            Rectangle rect = bounds;
//            g.FillRectangle(backBrush, rect);
//            rect.Offset(0, 2);
//            rect.Height -= 2;
//            g.DrawString(Convert.ToDateTime(value).ToString("d"), this.DataGridTableStyle.DataGrid.Font, foreBrush, rect);
//        }

//        protected override void SetDataGridInColumn(DataGrid value)
//        {
//            base.SetDataGridInColumn(value);
//            if (itsDateTimePicker.Parent != null)
//            {
//                itsDateTimePicker.Parent.Controls.Remove(itsDateTimePicker);
//            }
//            if (value != null)
//            {
//                value.Controls.Add(itsDateTimePicker);
//            }
//        }

//        private void TimePickerValueChanged(object sender, EventArgs e)
//        {
//            this.isEditing = true;
//            base.ColumnStartedEditing(itsDateTimePicker);
//        }
//    }
//}
