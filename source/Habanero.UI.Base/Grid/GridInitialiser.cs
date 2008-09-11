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
    /// <summary>
    /// Initialises the structure of a grid.  If a ClassDef is provided, the grid
    /// is initialised using the UI definition provided for that class.  If no
    /// ClassDef is provided, it is assumed that the grid will be set up in code
    /// by the developer.
    /// </summary>
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
        /// Initialises the grid without a ClassDef. This is typically used where the columns are set up manually
        /// for purposes such as adding a column with images to indicate the state of the object or adding a
        /// column with buttons/links.
        /// <br/>
        /// The grid must already have at least one column added. At least one column must be a column with the name
        /// "ID", which is used to synchronise the grid with the business objects.
        /// </summary>
        /// <exception cref="GridBaseInitialiseException">Thrown in the case where the columns
        /// have not already been defined for the grid</exception>
        /// <exception cref="GridBaseSetUpException">Thrown in the case where the grid has already been initialised</exception>
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

        /// <summary>
        /// Initialises the grid with the default UI definition for the class,
        /// as provided in the ClassDef
        /// </summary>
        /// <param name="classDef">The ClassDef used to initialise the grid</param>
        public void InitialiseGrid(IClassDef classDef)
        {
            InitialiseGrid(classDef, "default");
        }

        /// <summary>
        /// Initialises the grid with a specified alternate UI definition for the class,
        /// as provided in the ClassDef
        /// </summary>
        /// <param name="classDef">The Classdef used to initialise the grid</param>
        /// <param name="uiDefName">The name of the UI definition</param>
        public void InitialiseGrid(IClassDef classDef, string uiDefName)
        {
            //if (_isInitialised) throw new GridBaseSetUpException("You cannot initialise the grid more than once");

            UIGrid gridDef = GetGridDef((ClassDef) classDef, uiDefName);
            SetUpGridColumns((ClassDef) classDef, gridDef);
            _gridControl.UiDefName = uiDefName;
            _gridControl.ClassDef = classDef;

            _isInitialised = true;
           
        }

        /// <summary>
        /// Gets the value indicating whether the grid has been initialised already
        /// </summary>
        public bool IsInitialised
        {
            get { return _isInitialised; }
        }

        /// <summary>
        /// Gets the grid that is being initialised
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
            IDataGridViewColumn col = CreateStandardColumn("ID", "ID");
            col.Width = 0;
            col.Visible = false;
            col.ReadOnly = true;
            col.DataPropertyName = "ID";
            col.ValueType = typeof(string);
        }

        private IDataGridViewColumn CreateStandardColumn(string columnName, string columnHeader)
        {
            int colIndex = this._gridControl.Grid.Columns.Add(columnName, columnHeader);
            return this._gridControl.Grid.Columns[colIndex];
        }

        private IDataGridViewColumn CreateCustomColumn(UIGridColumn columnDef)
        {
            IDataGridViewColumn newColumn;

            if (columnDef.GridControlType != null)
            {
                newColumn = _controlFactory.CreateDataGridViewColumn(columnDef.GridControlType);
            }
            else
            {
                newColumn = _controlFactory.CreateDataGridViewColumn(columnDef.GridControlTypeName, columnDef.GridControlAssemblyName);
            }

            _gridControl.Grid.Columns.Add(newColumn);
            return newColumn;
        }

        private void CreateColumnForUIDef(ClassDef classDef, UIGrid gridDef)
        {
            //foreach (UIGridColumn gridColDef in gridDef)
            //{
            //    IDataGridViewColumn col = CreateStandardColumn(gridColDef.PropertyName, gridColDef.GetHeading());
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
                    //this._gridControl.Grid.Columns.Add(comboBoxCol);

                    IPropDef propDef = GetPropDef(classDef, gridColDef);
                    ILookupList source = propDef.LookupList;
                    //(ILookupList)_dataTable.Columns[colNum].ExtendedProperties["LookupList"];
                    if (source != null)
                    {
                        DataTable table = new DataTable();
                        table.Columns.Add("id");
                        table.Columns.Add("str");

                        table.LoadDataRow(new object[] {"", ""}, true);
                        foreach (KeyValuePair<string, object> pair in source.GetLookupList())
                        {
                            table.LoadDataRow(new object[] {pair.Value, pair.Key}, true);
                        }

                        comboBoxCol.DataSource = table;
                        //Bug: This null check has been placed because of a Gizmox bug 
                        //  We posted this at: http://www.visualwebgui.com/Forums/tabid/364/forumid/29/threadid/12420/scope/posts/Default.aspx
                        //  It is causing a StackOverflowException on ValueMember because the DataSource is still null
                        if (comboBoxCol.DataSource != null)
                        {
                            comboBoxCol.ValueMember = "str";
                            comboBoxCol.DisplayMember = "str";
                        }
                    }
                    comboBoxCol.DataPropertyName = gridColDef.PropertyName; //dataColumn.ColumnName;
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
                    //col = CreateStandardColumn(gridColDef.PropertyName, gridColDef.GetHeading());
                    col = CreateCustomColumn(gridColDef);
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