using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base.Grid
{
    public class EditableGridControlManager
    {
        private readonly IEditableGridControl _gridControl;

        public EditableGridControlManager(IEditableGridControl gridControl)
        {
            _gridControl = gridControl;
        }

        public void SetUpGridColumns(UIGrid gridDef)
        {
            foreach (UIGridColumn gridColDef in gridDef)
            {
                int colIndex = _gridControl.Grid.Columns.Add(gridColDef.PropertyName, gridColDef.GetHeading());

                //IDataGridViewColumn col = CreateColumn(gridColDef.PropertyName, gridColDef.GetHeading());
                //col.ReadOnly = true;
                //col.HeaderText = gridColDef.GetHeading();
                //col.Name = gridColDef.PropertyName;
                //col.DataPropertyName = gridColDef.PropertyName;
                //col.Visible = true;
                //col.Width = gridColDef.Width;
                //((DataGridViewColumn)col).SortMode = DataGridViewColumnSortMode.Automatic;
                //PropDef propDef = GetPropDef(gridColDef);
                //if (propDef != null) col.ValueType = propDef.PropertyType;
                ////this._grid.Columns.Add(col);

            }
        }

        public void Initialise(ClassDef classDef)
        {
            UIGrid uiGrid = classDef.UIDefCol["default"].UIGrid;
            SetUpGridColumns(uiGrid);
        }

        public void Initialise(ClassDef classDef, string name)
        {
            UIGrid uiGrid = classDef.UIDefCol[name].UIGrid;
            SetUpGridColumns(uiGrid);
            
        }
    }
}
