using System;
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Base.FilterControl;

namespace Habanero.UI.WebGUI
{
    public class ReadOnlyGridControlGiz : ControlGiz, IReadOnlyGridControl
    {
        private readonly IControlFactory _controlFactory;
        private readonly ReadOnlyGridGiz _grid;
        private readonly IReadOnlyGridButtonsControl _buttons;
        private IBusinessObjectEditor _businessObjectEditor;
        private IBusinessObjectCreator _businessObjectCreator;
        private IBusinessObjectDeletor _businessObjectDeletor;
        private string _uiDefName = "";
        private ClassDef _classDef;
        private bool _initialised;
        private IFilterControl _filterControl;
        //private readonly GridSelectionController _gridSelectionController;

        //private IBusinessObjectCollection _collection;

        /// <summary>
        /// Constructor to initialise a new grid
        /// </summary>
        public ReadOnlyGridControlGiz()
        {
      

            _controlFactory = new ControlFactoryGizmox();
            BorderLayoutManager borderLayoutManager = new BorderLayoutManagerGiz(this, _controlFactory);
            _grid = new ReadOnlyGridGiz();
            _grid.Name = "GridControl";
            borderLayoutManager.AddControl(_grid, BorderLayoutManager.Position.Centre);

            _buttons = _controlFactory.CreateReadOnlyGridButtonsControl();
            _buttons.AddClicked += Buttons_AddClicked;
            _buttons.EditClicked += Buttons_EditClicked;
            _buttons.DeleteClicked += Buttons_DeleteClicked;
            _buttons.Name = "ButtonControl";
            borderLayoutManager.AddControl(_buttons, BorderLayoutManager.Position.South);

            _filterControl = new FilterControlGiz(_controlFactory);
            borderLayoutManager.AddControl(_filterControl, BorderLayoutManager.Position.North);

            _filterControl.Filter += delegate
            {
                this.Grid.CurrentPage = 1;
                this.Grid.ApplyFilter(_filterControl.GetFilterClause());
            };
        }

        /// <summary>
        /// initiliase the grid to the with the 'default' UIdef.
        /// </summary>
        public void Initialise(ClassDef classDef)
        {
            this.Initialise(classDef, "default");
        }

        public void Initialise(ClassDef classDef, string uiDefName)
        {
            if (_initialised) throw new GridBaseSetUpException("You cannot initialise the grid more than once");
            if (classDef == null) throw new ArgumentNullException("classDef");
            if (uiDefName == null) throw new ArgumentNullException("uiDefName");


            _classDef = classDef;
            _uiDefName = uiDefName;

            UIGrid gridDef = GetGridDef(classDef);
            SetUpGridColumns(gridDef);
            _initialised = true;
        }

        private UIGrid GetGridDef(ClassDef classDef)
        {
            UIDef uiDef = _classDef.GetUIDef(_uiDefName);
            if (uiDef == null)
            {
                throw new ArgumentException(
                    String.Format(
                        "You cannot initialise {0} because it does not contain a definition for UIDef {1} for the class def {2}",
                        this._grid.Name, _uiDefName, classDef.ClassName));
            }
            UIGrid gridDef = uiDef.UIGrid;
            if (gridDef == null)
            {
                throw new ArgumentException(
                    String.Format(
                        "You cannot initialise {0} does not contain a grid definition for UIDef {1} for the class def {2}",
                        this._grid.Name, _uiDefName, classDef.ClassName));
            }
            return gridDef;
        }

        private void SetUpGridColumns(UIGrid gridDef)

        {
            this._grid.Columns.Clear();
            CreateIDColumn();
            CreateColumnForUIDef(gridDef);
        }

        private void CreateIDColumn()
        {
            IDataGridViewColumn col = CreateColumn("ID", "ID");
            col.Width = 0;
            col.Visible = false;
            col.ReadOnly = true;
            col.DataPropertyName = "ID";
            col.ValueType = typeof (string);
        }

        private IDataGridViewColumn CreateColumn(string columnName, string columnHeader)
        {
            int colIndex = this._grid.Columns.Add(columnName, columnHeader);
            return this._grid.Columns[colIndex];
        }

        private void CreateColumnForUIDef(UIGrid gridDef)
        {
            foreach (UIGridColumn gridColDef in gridDef)
            {
                IDataGridViewColumn col = CreateColumn(gridColDef.PropertyName, gridColDef.GetHeading());
                col.ReadOnly = true;
                col.HeaderText = gridColDef.GetHeading();
                col.Name = gridColDef.PropertyName;
                col.DataPropertyName = gridColDef.PropertyName;
                col.Visible = true;
                col.Width = gridColDef.Width;
                //TODO_Port: Figure out a generic way of doing setting sort
                // either by a custom cast or by moving enums to generalised
                //((DataGridViewColumn)col).SortMode = DataGridViewColumnSortMode.Automatic;
                PropDef propDef = GetPropDef(gridColDef);
                if (propDef != null) col.ValueType = propDef.PropertyType;
                //this._grid.Columns.Add(col);

            }
        }

        private PropDef GetPropDef(UIGridColumn gridColumn)
        {
            PropDef propDef = null;
            if (_classDef.PropDefColIncludingInheritance.Contains(gridColumn.PropertyName))
            {
                propDef = _classDef.PropDefColIncludingInheritance[gridColumn.PropertyName];
            }
            return propDef;
        }

        //        this.Columns.Clear();


        //int colNum = 1;
        //foreach (UIGridColumn gridColumn in uiGrid)
        //{
        //    dataColumn = _dataTable.Columns[colNum];
        //    PropDef propDef = null;
        //    if (classDef.PropDefColIncludingInheritance.Contains(gridColumn.PropertyName))
        //    {
        //        propDef = classDef.PropDefColIncludingInheritance[gridColumn.PropertyName];
        //    }
        //    if (gridColumn.GridControlType == typeof(DataGridViewComboBoxColumn))
        //    {
        //        DataGridViewComboBoxColumn comboBoxCol = new DataGridViewComboBoxColumn();
        //        ILookupList source =
        //            (ILookupList)_dataTable.Columns[colNum].ExtendedProperties["LookupList"];
        //        if (source != null) {
        //            DataTable table = new DataTable();
        //            table.Columns.Add("id");
        //            table.Columns.Add("str");

        //            table.LoadDataRow(new object[] {"", ""}, true);
        //            foreach (KeyValuePair<string, object> pair in source.GetLookupList()) {
        //                table.LoadDataRow(new object[] {pair.Value, pair.Key}, true);
        //            }
        //            comboBoxCol.DataSource = table;
        //            comboBoxCol.ValueMember = "str";
        //            comboBoxCol.DisplayMember = "str";
        //        }
        //        comboBoxCol.DataPropertyName = dataColumn.ColumnName;
        //        col = comboBoxCol;
        //    }
        //    else if (gridColumn.GridControlType == typeof(DataGridViewCheckBoxColumn))
        //    {
        //        DataGridViewCheckBoxColumn checkBoxCol = new DataGridViewCheckBoxColumn();
        //        col = checkBoxCol;
        //    }
        //    else if (gridColumn.GridControlType == typeof(DataGridViewDateTimeColumn))
        //    {
        //        DataGridViewDateTimeColumn dateTimeCol = new DataGridViewDateTimeColumn();
        //        col = dateTimeCol;
        //        _dateColumnIndices.Add(colNum, (string)gridColumn.GetParameterValue("dateFormat"));
        //    }
        //    else
        //    {
        //        col = (DataGridViewColumn)Activator.CreateInstance(gridColumn.GridControlType);
        //    }
        //    int width = (int)(dataColumn.ExtendedProperties["Width"]);
        //    col.Width = width;
        //    if (width == 0)
        //    {
        //        col.Visible = false;
        //    }
        //    col.ReadOnly = !gridColumn.Editable;
        //    col.HeaderText = dataColumn.Caption;
        //    col.Name = dataColumn.ColumnName;
        //    col.DataPropertyName = dataColumn.ColumnName;
        //    //col.MappingName = dataColumn.ColumnName;
        //    col.SortMode = DataGridViewColumnSortMode.Automatic;

        //    SetAlignment(col, gridColumn);
        //    if (CompulsoryColumnsBold && propDef != null && propDef.Compulsory)
        //    {
        //        Font newFont = new Font(DefaultCellStyle.Font, FontStyle.Bold);
        //        col.HeaderCell.Style.Font = newFont;
        //    }

        //    if (propDef != null && propDef.PropertyType == typeof(DateTime)
        //        && gridColumn.GridControlType != typeof(DataGridViewDateTimeColumn))
        //    {
        //        _dateColumnIndices.Add(colNum, (string)gridColumn.GetParameterValue("dateFormat"));
        //    }

        //    //if (propDef != null && propDef.PropertyName != gridColumn.GetHeading(classDef))
        //    //{
        //    //    foreach (BusinessObject bo in _collection)
        //    //    {
        //    //        BOProp boProp = bo.Props[propDef.PropertyName];
        //    //        if (!boProp.HasDisplayName())
        //    //        {
        //    //            boProp.DisplayName = gridColumn.GetHeading(classDef);
        //    //        }
        //    //    }
        //    //}

        //    Columns.Add(col);
        //    colNum++;

        private void Buttons_DeleteClicked(object sender, EventArgs e)
        {
            IBusinessObject selectedBo = SelectedBusinessObject;

            if (selectedBo != null)
            {
                _grid.SelectedBusinessObject = null;
                try
                {
                    _businessObjectDeletor.DeleteBusinessObject(selectedBo);
                }
                catch (Exception ex)
                {
                    GlobalRegistry.UIExceptionNotifier.Notify(ex, "There was a problem deleting", "Problem Deleting");
                }
            }
        }

        private void Buttons_EditClicked(object sender, EventArgs e)
        {
            IBusinessObject selectedBo = SelectedBusinessObject;
            if (selectedBo != null)
            {
                //TODO_Port: CheckEditorExists();
                _businessObjectEditor.EditObject(selectedBo, _uiDefName);
                //IBusinessObjectEditor objectEditor = new DefaultBOEditor(_controlFactory);
                //objectEditor.EditObject(selectedBo, "default");
                //				{
                //					_readOnlyGrid.RefreshRow(selectedBo) ;
                //				}
            }
        }

        private void Buttons_AddClicked(object sender, EventArgs e)
        {
            IBusinessObject newBo = _businessObjectCreator.CreateBusinessObject();
            _businessObjectEditor.EditObject(newBo, _uiDefName);
        }


        /// <summary>
        /// Returns the grid object held. This property can be used to
        /// access a range of functionality for the grid
        /// (eg. myGridWithButtons.Grid.AddBusinessObject(...)).
        /// </summary>
        public IReadOnlyGrid Grid
        {
            get { return _grid; }
        }

        /// <summary>
        /// Gets or sets the single selected business object (null if none are selected)
        /// denoted by where the current selected cell is
        /// </summary>
        public BusinessObject SelectedBusinessObject
        {
            get { return _grid.SelectedBusinessObject; }
            set { _grid.SelectedBusinessObject = value; }
        }

        /// <summary>
        /// Returns the button control held. This property can be used
        /// to access a range of functionality for the button control
        /// (eg. myGridWithButtons.Buttons.AddButton(...)).
        /// </summary>
        public IReadOnlyGridButtonsControl Buttons
        {
            get { return _buttons; }
        }

        public IBusinessObjectEditor BusinessObjectEditor
        {
            get { return _businessObjectEditor; }
            set { _businessObjectEditor = value; }
        }

        public IBusinessObjectCreator BusinessObjectCreator
        {
            get { return _businessObjectCreator; }
            set { _businessObjectCreator = value; }
        }

        public IBusinessObjectDeletor BusinessObjectDeletor
        {
            get { return _businessObjectDeletor; }
            set { _businessObjectDeletor = value; }
        }

        public string UiDefName
        {
            get { return _uiDefName; }
        }

        public ClassDef ClassDef
        {
            get { return _classDef; }
        }

        public IFilterControl FilterControl
        {
            get { return _filterControl; }
        }


        public void SetCollection(IBusinessObjectCollection boCollection)
        {
            if (_classDef == null)
            {
                Initialise(boCollection.ClassDef);
            }else
            {
                if (_classDef != boCollection.ClassDef)
                {
                    throw new ArgumentException(
                        "You cannot call set collection for a collection that has a different class def than is initialised");
                }
            }
            _grid.SetCollection(boCollection);
            this.BusinessObjectEditor = new DefaultBOEditor(_controlFactory);
            this.BusinessObjectCreator = new DefaultBOCreator(boCollection);
            this.BusinessObjectDeletor = new DefaultBODeletor();
        }
    }

    public class DataGridViewColumnGiz : DataGridViewColumn, IDataGridViewColumn
    {
        public object DataGridViewColumn
        {
            get { throw new NotImplementedException(); }
        }
    }
}