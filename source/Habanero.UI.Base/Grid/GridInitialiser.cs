using System;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    public class GridInitialiser : IGridInitialiser
    {
        private readonly IGridControl _gridControl;
        private bool _isInitialised = false;

        public GridInitialiser(IGridControl gridControl)
        {
            _gridControl = gridControl;
        }

        /// <summary>
        /// Initialises the grid based with no classDef. This is used where the columns are set up manually.
        /// A typical case of when you would want to set the columns manually would be when the grid
        ///  requires alternate columns e.g. images to indicate the state of the object or buttons/links.
        /// The grid must already have at least one column added. At least one column must be a column with the name
        /// "ID" This column is used to synchronise the grid with the business objects.
        /// </summary>
        /// <exception cref="GridBaseInitialiseException"> in the case where the columns have not already been defined for the grid</exception>
        /// <exception cref="GridBaseSetUpException">in the case where the grid has already been initialised</exception>
        public void InitialiseGrid()
        {
            if (_isInitialised) throw new GridBaseSetUpException("You cannot initialise the grid more than once");
            if (_gridControl.Grid.Columns.Count == 0) throw new GridBaseInitialiseException("You cannot call initialise with no classdef since the ID column has not been added to the grid");
            try
            {
                IDataGridViewColumn column = _gridControl.Grid.Columns["ID"];
                string text = column.HeaderText;
            }
            catch (NullReferenceException ex)
            {
                throw new GridBaseInitialiseException(
                    "You cannot call initialise with no classdef since the ID column has not been added to the grid");
            }
            _isInitialised = true;
        }

        public void InitialiseGrid(ClassDef classDef)
        {
            InitialiseGrid(classDef, "default");
        }

        public void InitialiseGrid(ClassDef classDef, string uiDefName)
        {
            if (_isInitialised) throw new GridBaseSetUpException("You cannot initialise the grid more than once");

            UIGrid gridDef = GetGridDef(classDef, uiDefName);
            SetUpGridColumns(classDef, gridDef);
            _gridControl.UiDefName = uiDefName;
            _gridControl.ClassDef = classDef;

            _isInitialised = true;
           
        }

        public bool IsInitialised
        {
            get { return _isInitialised; }
        }

        /// <summary>
        /// returns the grid that this initialiser is initialising
        /// </summary>
        public IGridControl Grid
        {
            get { return _gridControl; }
        }

        private UIGrid GetGridDef(ClassDef classDef, string uiDefName)
        {
            UIDef uiDef = classDef.GetUIDef(uiDefName);
            if (uiDef == null)
            {
                throw new ArgumentException(
                    String.Format(
                        "You cannot initialise {0} because it does not contain a definition for UIDef {1} for the class def {2}",
                        this._gridControl.Grid.Name, uiDefName, classDef.ClassName));
            }
            UIGrid gridDef = uiDef.UIGrid;
            if (gridDef == null)
            {
                throw new ArgumentException(
                    String.Format(
                        "You cannot initialise {0} does not contain a grid definition for UIDef {1} for the class def {2}",
                        this._gridControl.Grid.Name, uiDefName, classDef.ClassName));
            }
            return gridDef;
        }



        private void SetUpGridColumns(ClassDef classDef, UIGrid gridDef)
        {
            this._gridControl.Grid.Columns.Clear();
            CreateIDColumn();
            CreateColumnForUIDef(classDef, gridDef);
        }

        private void CreateIDColumn()
        {
            IDataGridViewColumn col = CreateColumn("ID", "ID");
            col.Width = 0;
            col.Visible = false;
            col.ReadOnly = true;
            col.DataPropertyName = "ID";
            col.ValueType = typeof(string);
        }

        private IDataGridViewColumn CreateColumn(string columnName, string columnHeader)
        {
            int colIndex = this._gridControl.Grid.Columns.Add(columnName, columnHeader);
            return this._gridControl.Grid.Columns[colIndex];
        }

        private void CreateColumnForUIDef(ClassDef classDef, UIGrid gridDef)
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
                //TODO - do sorting
                //((DataGridViewColumn)col).SortMode = DataGridViewColumnSortMode.Automatic;
                PropDef propDef = GetPropDef(classDef, gridColDef);
                if (propDef != null) col.ValueType = propDef.PropertyType;
                //this._grid.Columns.Add(col);

            }
        }


        private PropDef GetPropDef(ClassDef classDef, UIGridColumn gridColumn)
        {
            PropDef propDef = null;
            if (classDef.PropDefColIncludingInheritance.Contains(gridColumn.PropertyName))
            {
                propDef = classDef.PropDefColIncludingInheritance[gridColumn.PropertyName];
            }
            return propDef;
        }
    }
}