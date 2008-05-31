using System;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    public class GridInitialiser
    {
        private readonly IGridControl _gridControl;
        private bool _isInitialised = false;

        public GridInitialiser(IGridControl gridControl)
        {
            _gridControl = gridControl;
        }

        public void InitialiseGrid(ClassDef classDef)
        {
            _gridControl.Initialise(classDef, "default");
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