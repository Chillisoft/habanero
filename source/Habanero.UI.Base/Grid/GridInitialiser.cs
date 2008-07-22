//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base.Grid;

namespace Habanero.UI.Base
{
    public class GridInitialiser : IGridInitialiser
    {
        private readonly IGridControl _gridControl;
        private readonly IControlFactory _controlFactory;
        private bool _isInitialised = false;

        public GridInitialiser(IGridControl gridControl, IControlFactory controlFactory)
        {
            _gridControl = gridControl;
            _controlFactory = controlFactory;
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
                //Try to get the id column from the grid. If there is no id column or if the id column
                // is not set up with a header then an error should be thrown. This looks like checking if 
                // column is null and throwing the error would achieve this objective.
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

        public void InitialiseGrid(IClassDef classDef)
        {
            InitialiseGrid(classDef, "default");
        }

        public void InitialiseGrid(IClassDef classDef, string uiDefName)
        {
//            if (_isInitialised) throw new GridBaseSetUpException("You cannot initialise the grid more than once");

            UIGrid gridDef = GetGridDef((ClassDef) classDef, uiDefName);
            SetUpGridColumns((ClassDef) classDef, gridDef);
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
            //foreach (UIGridColumn gridColDef in gridDef)
            //{
            //    IDataGridViewColumn col = CreateColumn(gridColDef.PropertyName, gridColDef.GetHeading());
            //    //col.ReadOnly = true;
            //    col.HeaderText = gridColDef.GetHeading();
            //    col.Name = gridColDef.PropertyName;
            //    col.DataPropertyName = gridColDef.PropertyName;
            //    col.Visible = true;
            //    col.Width = gridColDef.Width;
            //    col.SortMode = DataGridViewColumnSortMode.Automatic;
            //    PropDef propDef = GetPropDef(classDef, gridColDef);
            //    if (propDef != null) col.ValueType = propDef.PropertyType;
            //    //this._grid.Columns.Add(col);
            //}
            foreach (UIGridColumn gridColDef in gridDef)
            {
                IDataGridViewColumn col;
                if (gridColDef.GridControlTypeName == "DataGridViewComboBoxColumn")
                {
                    IDataGridViewComboBoxColumn comboBoxCol = _controlFactory.CreateDataGridViewComboBoxColumn();
                    ////this._gridControl.Grid.Columns.Add(comboBoxCol);

                    //IPropDef propDef = GetPropDef(classDef, gridColDef);
                    //ILookupList source = propDef.LookupList;  //TODO: what if lookuplist is null?
                    //    //(ILookupList)_dataTable.Columns[colNum].ExtendedProperties["LookupList"];
                    //if (source != null)
                    //{
                    //    DataTable table = new DataTable();
                    //    table.Columns.Add("id");
                    //    table.Columns.Add("str");

                    //    table.LoadDataRow(new object[] { "", "" }, true);
                    //    foreach (KeyValuePair<string, object> pair in source.GetLookupList())
                    //    {
                    //        table.LoadDataRow(new object[] { pair.Value, pair.Key }, true);
                    //    }

                    //    comboBoxCol.DataSource = table;
                    //    comboBoxCol.ValueMember = "str";
                    //    comboBoxCol.DisplayMember = "str";
                    //}
                    //comboBoxCol.DataPropertyName = gridColDef.PropertyName; //dataColumn.ColumnName;
                    col = comboBoxCol;
                    this._gridControl.Grid.Columns.Add(col);
                }
                else if (gridColDef.GridControlTypeName == "DataGridViewCheckBoxColumn")
                {
                    col = _controlFactory.CreateDataGridViewCheckBoxColumn();
                    this._gridControl.Grid.Columns.Add(col);
                }
                else
                {
                    col = CreateColumn(gridColDef.PropertyName, gridColDef.GetHeading());
                }
//                IDataGridViewColumn col = _controlFactory.CreateDataGridViewCheckBoxColumn();
                //col.ReadOnly = true;
                col.HeaderText = gridColDef.GetHeading();
                col.Name = gridColDef.PropertyName;
                col.DataPropertyName = gridColDef.PropertyName;
                col.Visible = true;
                col.Width = gridColDef.Width;
                col.SortMode = DataGridViewColumnSortMode.Automatic;
                //IPropDef propDef = GetPropDef(classDef, gridColDef);
                //if (propDef != null) col.ValueType = propDef.PropertyType;
                Type propertyType = classDef.GetPropertyType(gridColDef.PropertyName);
                if (propertyType != typeof(object))
                {
                    col.ValueType = propertyType;
                }
                SetupColumnWithDefParameters(col, gridColDef);
//                this._gridControl.Grid.Columns.Add(col);
            }
        }

        private static void SetupColumnWithDefParameters(IDataGridViewColumn col, UIGridColumn gridColDef)
        {
            string dateFormat = gridColDef.GetParameterValue("dateFormat") as string;
            if (dateFormat != null)
            {
                col.DefaultCellStyle.Format = dateFormat;
            }
        }

        private IPropDef GetPropDef(ClassDef classDef, UIGridColumn gridColumn)
        {
            IPropDef propDef = null;
            if (classDef.PropDefColIncludingInheritance.Contains(gridColumn.PropertyName))
            {
                propDef = classDef.PropDefColIncludingInheritance[gridColumn.PropertyName];
            }
            return propDef;
        }
    }
}