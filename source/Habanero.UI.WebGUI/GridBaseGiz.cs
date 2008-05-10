using System;
using System.Data;
using Gizmox.WebGUI.Forms;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{

    public class GridBaseGiz : DataGridView, IGridBase
    {
        public event EventHandler<BOEventArgs> BusinessObjectSelected;


        private readonly GridBaseManager _mngr;

        public GridBaseGiz()
        {
            _mngr = new GridBaseManager(this);
            this.SelectionChanged += delegate { FireBusinessObjectSelected(); };
        }
        private void FireBusinessObjectSelected()
        {
                if (this.BusinessObjectSelected != null)
                {
                    this.BusinessObjectSelected(this, new BOEventArgs(this.SelectedBusinessObject));
                }

        }


        public void SetCollection(IBusinessObjectCollection col)
        {
            _mngr.SetCollection(col);

        }

        public new IDataGridViewRowCollection Rows
        {
            get { return new DataGridViewRowCollectionGiz(base.Rows); }
        }

        public new IDataGridViewColumnCollection Columns
        {
            get { return new DataGridViewColumnCollectionGiz(base.Columns); }
        }

        public BusinessObject SelectedBusinessObject
        {
            get { return _mngr.SelectedBusinessObject; }
            set { _mngr.SelectedBusinessObject = value;
            this.FireBusinessObjectSelected();
        }
        }

        private class DataGridViewRowCollectionGiz : IDataGridViewRowCollection
        {
            private readonly DataGridViewRowCollection _rows;

            public DataGridViewRowCollectionGiz(DataGridViewRowCollection rows)
            {
                if (rows == null) throw new ArgumentNullException("rows");
                _rows = rows;
            }

            public int Count
            {
                get { return _rows.Count; }
            }

            public IDataGridViewRow this[int index]
            {
                get { return new DataGridViewRowGiz(_rows[index]); }
            }
        }
        private class DataGridViewColumnCollectionGiz : IDataGridViewColumnCollection
        {
            private readonly DataGridViewColumnCollection _columns;

            public DataGridViewColumnCollectionGiz(DataGridViewColumnCollection columns)
            {
                if (columns == null) throw new ArgumentNullException("columns");
                _columns = columns;
            }

            public int Count
            {
                get { return _columns.Count; }
            }
        }


        private class DataGridViewRowGiz : IDataGridViewRow
        {
            private readonly DataGridViewRow _dataGridViewRow;

            public DataGridViewRowGiz(DataGridViewRow dataGridViewRow)
            {
                _dataGridViewRow = dataGridViewRow;
            }

            public bool Selected
            {
                get { return _dataGridViewRow.Selected; }
                set { _dataGridViewRow.Selected = value; }
            }
        }
    }

}
