//using System;
//using System.Drawing;
//using System.Windows.Forms;
//using Habanero.Generic;
//using log4net;
//using Noogen.WinForms;

//namespace Habanero.Ui.Grid
//{
//    /// <summary>
//    /// Summary description for DataGridComboBoxColumn.
//    /// </summary>
//    // This example shows how to create your own column style that
//    // hosts a control, in this case, a DateTimePicker.
//    public class DataGridComboBoxColumn : DataGridColumnStyle
//    {
//        private static readonly ILog log = LogManager.GetLogger("Habanero.Ui.Generic.DataGridComboBoxColumn");
//        private ComboBox itsComboBox = new NComboBox();
//        // The isEditing field tracks whether or not the user is
//        // editing data with the hosted control.
//        private bool isEditing;
//        private StringGuidPairCollection itsLookupList;

//        public DataGridComboBoxColumn(StringGuidPairCollection col) : base()
//        {
//            //log.Debug("DataGridComboBoxColumn created.");
//            NComboBox cbx = new Noogen.WinForms.NComboBox();
//            cbx.DisableEntryNotInList = true;
//            cbx.AutoComplete = true;
//            cbx.CharacterCasing = CharacterCasing.Normal;
//            cbx.ShowDropDownDuringInput = true;
//            itsComboBox = cbx;
//            itsComboBox.Visible = false;
//            this.SetLookupList(col);
//        }

//        public void SetLookupList(StringGuidPairCollection col)
//        {
//            itsLookupList = col;
//            itsComboBox.Items.Clear();
//            foreach (StringGuidPair pair in col)
//            {
//                itsComboBox.Items.Add(pair);
//            }
//        }

//        protected override void Abort(int rowNum)
//        {
//            isEditing = false;
//            itsComboBox.SelectedIndexChanged -= new EventHandler(ComboBoxValueChanged);
//            Invalidate();
//        }

//        protected override bool Commit(CurrencyManager dataSource, int rowNum)
//        {
//            itsComboBox.Bounds = Rectangle.Empty;
//            itsComboBox.SelectedIndexChanged -= new EventHandler(ComboBoxValueChanged);

//            if (!isEditing)
//            {
//                return true;
//            }

//            isEditing = false;

//            try
//            {
//                String value = itsComboBox.SelectedItem.ToString();
//                SetColumnValueAtRow(dataSource, rowNum, value);
//            }
//            catch (Exception)
//            {
//                log.Info("Error setting column value.  Aborting and returning false");
//                Abort(rowNum);
//                return false;
//            }
//            Invalidate();
//            return true;
//        }

//        protected override void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly,
//                                     string instantText, bool cellIsVisible)
//        {
//            String value;
//            //if (itsComboBox.SelectedIndex != -1) {
//            //	value = itsComboBox.SelectedItem.ToString() ;
//            //} else {
//            value = Convert.ToString(GetColumnValueAtRow(source, rowNum));
//            //}

//            if (cellIsVisible)
//            {
//                itsComboBox.Bounds = new Rectangle(bounds.X + 2, bounds.Y + 2, bounds.Width - 4, bounds.Height - 4);
//                itsComboBox.SelectedItem = this.itsLookupList.FindByValue(value);
//                itsComboBox.Visible = true;
//                itsComboBox.BringToFront();
//                itsComboBox.SelectedIndexChanged += new EventHandler(ComboBoxValueChanged);
//            }
//            else
//            {
//                itsComboBox.SelectedItem = this.itsLookupList.FindByValue(value);
//                itsComboBox.Visible = false;
//            }

//            if (itsComboBox.Visible)
//            {
//                DataGridTableStyle.DataGrid.Invalidate(bounds);
//            }
//        }

//        protected override Size GetPreferredSize(
//            Graphics g,
//            object value)
//        {
//            return new Size(100, itsComboBox.PreferredHeight + 4);
//        }

//        protected override int GetMinimumHeight()
//        {
//            return itsComboBox.PreferredHeight + 4;
//        }

//        protected override int GetPreferredHeight(Graphics g,
//                                                  object value)
//        {
//            return itsComboBox.PreferredHeight + 4;
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
//            String str = Convert.ToString(GetColumnValueAtRow(source, rowNum));
//            Rectangle rect = bounds;
//            g.FillRectangle(backBrush, rect);
//            rect.Offset(0, 2);
//            rect.Height -= 2;
//            g.DrawString(str, this.DataGridTableStyle.DataGrid.Font, foreBrush, rect);
//        }

//        protected override void SetDataGridInColumn(DataGrid value)
//        {
//            base.SetDataGridInColumn(value);
//            if (itsComboBox.Parent != null)
//            {
//                itsComboBox.Parent.Controls.Remove(itsComboBox);
//            }
//            if (value != null)
//            {
//                value.Controls.Add(itsComboBox);
//            }
//        }

//        private void ComboBoxValueChanged(object sender, EventArgs e)
//        {
//            //log.Debug("Combo box value changed to " + itsComboBox.SelectedItem.ToString());
//            this.isEditing = true;
//            base.ColumnStartedEditing(itsComboBox);
//        }
//    }
//}