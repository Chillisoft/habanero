// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Util;

namespace Habanero.UI.Base
{
    /// <summary>
    /// This manager groups common logic for IEditableGridControl objects.
    /// Do not use this object in working code.
    /// </summary>
    public class GridBaseManager
    {
        private readonly IGridBase _gridBase;
        private IDataSetProvider _dataSetProvider;
        private IBusinessObjectCollection _boCol;
        private DataView _dataTableDefaultView;
        /// <summary>
        /// Occurs when a business object is selected
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectSelected;
        /// <summary>
        /// Handler for the CollectionChanged Event
        /// </summary>
        public event EventHandler CollectionChanged;

        private GridLoaderDelegate _gridLoader;
        private readonly EventHandler _gridBase_OnSelectionChangedHandler;
        private bool _fireBusinessObjectSelectedEvent;

        ///<summary>
        /// Constructor
        ///</summary>
        ///<param name="gridBase"></param>
        ///<param name="uiDefName"></param>
        public GridBaseManager(IGridBase gridBase, string uiDefName)
        {
            _gridBase = gridBase;
            UiDefName = uiDefName;
            _gridBase.AutoGenerateColumns = false;
            _gridLoader = DefaultGridLoader;
            _gridBase.AllowUserToAddRows = false;

            _gridBase_OnSelectionChangedHandler = GridBase_OnSelectionChanged;
            _gridBase.SelectionChanged += _gridBase_OnSelectionChangedHandler;
            AutoSelectFirstItem = true;
        }

        private void GridBase_OnSelectionChanged(object sender, EventArgs e)
        {
            try
            {
                FireBusinessObjectSelected();
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        ///<summary>
        /// Constructor
        ///</summary>
        ///<param name="gridBase"></param>
        public GridBaseManager(IGridBase gridBase) : this(gridBase, "default")
        {
        }

        private void FireBusinessObjectSelected()
        {
            if (this.BusinessObjectSelected != null && _fireBusinessObjectSelectedEvent)
            {
                this.BusinessObjectSelected(this, new BOEventArgs(this.SelectedBusinessObject));
            }
        }

        /// <summary>
        /// See <see cref="IGridBase"/>.<see cref="IBusinessObjectCollection"/>
        /// </summary>
        public void SetBusinessObjectCollection(IBusinessObjectCollection col)
        {
            if (_gridBase.Columns.Count <= 0)
            {
                if (col == null) return;
                throw new GridBaseSetUpException
                    ("You cannot call SetBusinessObjectCollection if the grid's columns have not been set up");
            }
            _boCol = col;
            if (_boCol == null)
            {
                SelectedBusinessObject = null;
                _gridLoader(_gridBase, null);
                FireCollectionChanged();
                return;
            }
            //Hack: this is to overcome a bug_ in Gizmox where the grid was freezing after delete
            // but should not cause a problem with win since it removed the currently selected item which is the deleted item
            col.BusinessObjectRemoved += delegate { SelectedBusinessObject = null; };

            _gridLoader(_gridBase, _boCol);

            if (_gridBase.Rows.Count > 0)
            {
                if (!IsFirstRowSelected())
                {
                    try
                    {
                        _gridBase.SelectionChanged -= _gridBase_OnSelectionChangedHandler;
                        _fireBusinessObjectSelectedEvent = false;
                        SelectedBusinessObject = null;
                        _fireBusinessObjectSelectedEvent = false;
                        _gridBase.Rows[0].Selected = AutoSelectFirstItem;
                    }
                    finally
                    {
                        _gridBase.SelectionChanged += _gridBase_OnSelectionChangedHandler;
                        _fireBusinessObjectSelectedEvent = true;
                    }

                }
                if (AutoSelectFirstItem)
                {
                    FireBusinessObjectSelected();
                }
            }
            FireCollectionChanged();
        }

        private bool IsFirstRowSelected()
        {
            return _gridBase.Rows[0].Selected;
        }

        /// <summary>
        /// See <see cref="IGridBase.DataSetProvider"/>
        /// </summary>
        public IDataSetProvider DataSetProvider
        {
            get { return _dataSetProvider; }
        }

        /// <summary>
        /// Sets the default grid loader
        /// </summary>
        public void DefaultGridLoader(IGridBase gridBase, IBusinessObjectCollection boCol)
        {
            if (boCol == null)
            {
                gridBase.DataSource = null;
                return;
            }
            DataView table = GetDataTable(boCol);
            try
            {
                gridBase.SelectionChanged -= _gridBase_OnSelectionChangedHandler;
                _fireBusinessObjectSelectedEvent = false;
                gridBase.DataSource = table;
                if (!AutoSelectFirstItem) gridBase.SelectedBusinessObject = null;
            }
            finally
            {
                gridBase.SelectionChanged += _gridBase_OnSelectionChangedHandler;
                _fireBusinessObjectSelectedEvent = true;
            }
        }

        /// <summary>
        /// Returns a DataView based on the <see cref="IBusinessObjectCollection"/> defined by <paramref name="boCol"/>.
        /// The Columns in the <see cref="DataView"/> will be the collumns defined in the Grids <see cref="UiDefName"/>
        /// </summary>
        /// <param name="boCol">The collection that the DataView is based on</param>
        /// <returns></returns>
        protected DataView GetDataTable(IBusinessObjectCollection boCol)
        {
            _dataSetProvider = _gridBase.CreateDataSetProvider(boCol);
            if (this.ClassDef == null || this.ClassDef != _boCol.ClassDef)
            {
                this.ClassDef = _boCol.ClassDef;
            }
            IUIDef uiDef = ((ClassDef) this.ClassDef).GetUIDef(UiDefName);
            if (uiDef == null)
            {
                throw new ArgumentException
                    (String.Format
                         ("You cannot Get the data for the grid {0} since the uiDef {1} cannot be found for the classDef {2}",
                          this._gridBase.Name, UiDefName, ((ClassDef) this.ClassDef).ClassName));
            }
            DataTable dataTable = _dataSetProvider.GetDataTable(uiDef.UIGrid);
            _dataTableDefaultView = dataTable.DefaultView;
            return this._dataTableDefaultView;
        }

        /// <summary>
        /// See <see cref="IBOColSelectorControl.SelectedBusinessObject"/>
        /// </summary>
        public IBusinessObject SelectedBusinessObject
        {
            get
            {
//                int rownum = -1;
//                for (int i = 0; i < _gridBase.Rows.Count; i++)
//                    if (_gridBase.Rows[i].Selected) rownum = i;
                for (int i = _gridBase.Rows.Count - 1; i >= 0; i--)
                {
                    if (_gridBase.Rows[i].Selected)
                    {
                        return this.GetBusinessObjectAtRow(i);
                    }
                }
                return null;
            }
            set
            {
                //TODO_Port: RemovePreviouslySelectedBusinessObjectsEventHandler();
                if (_boCol == null && value != null)
                {
                    throw new GridBaseInitialiseException
                        ("You cannot call SelectedBusinessObject if the collection is not set");
                }
                IDataGridViewRowCollection gridRows = _gridBase.Rows;
                try
                {
                    _gridBase.SelectionChanged -= _gridBase_OnSelectionChangedHandler;
                    _fireBusinessObjectSelectedEvent = false;
                    ClearAllSelectedRows(gridRows);
                    if (value == null)
                    {
                        _gridBase.CurrentCell = null;
                        return;
                    }
                }
                finally
                {
                    _fireBusinessObjectSelectedEvent = true;
                    _gridBase.SelectionChanged += _gridBase_OnSelectionChangedHandler;
                }
                BusinessObject bo = (BusinessObject) value;
                bool boFoundAndHighlighted = false;
                int rowNum = 0;
                foreach (IDataGridViewRow row in gridRows)
                {
                    if (GetRowObjectIDValue(row) == bo.ID.ObjectID)
                    {
                        gridRows[rowNum].Selected = true;
                        boFoundAndHighlighted = true;
                        _gridBase.ChangeToPageOfRow(rowNum);
                        break;
                    }
                    rowNum++;
                }

                //TODO: neither of these works in VWG (and they're needed)
                if (boFoundAndHighlighted && rowNum >= 0 && rowNum < gridRows.Count)
                {
                    if (_gridBase != null)
                    {
                        IDataGridViewRow row = _gridBase.Rows[rowNum];
                        if (row != null)
                        {
                            IDataGridViewCell cell = row.Cells[1];
                            if (cell != null && cell.RowIndex >= 0) _gridBase.CurrentCell = cell;
                        }
                    }
                    if (_gridBase != null)
                    if (_gridBase.CurrentRow != null && !_gridBase.CurrentRow.Displayed)
                    {
                        try
                        {
                            _gridBase.FirstDisplayedScrollingRowIndex = _gridBase.Rows.IndexOf(_gridBase.CurrentRow);
                            gridRows[rowNum].Selected = true; //Getting turned off for some reason
                        }
                        catch (InvalidOperationException)
                        {
                            //Do nothing - designed to catch error "No room is available to display rows"
                            //  when grid height is insufficient
                        }
                    }
                }
            }
        }


        //TODO_Port:
        //private void RemovePreviouslySelectedBusinessObjectsEventHandler()
        //{
        //    if (SelectedBusinessObject != null)
        //    {
        //        SelectedBusinessObject.Updated -= bo_Updated;
        //    }
        //}

        private static void ClearAllSelectedRows(IDataGridViewRowCollection gridRows)
        {
            for (int i = 0; i < gridRows.Count; i++)
            {
                IDataGridViewRow row = gridRows[i];
                row.Selected = false;
            }
        }

        ///// <summary>
        ///// Calls the BusinessObjectAdded() handler
        ///// </summary>
        ///// <param name="bo">The business object added</param>
        //private void FireBusinessObjectEdited(BusinessObject bo)
        //{
        //    _gridBase.SelectedBusinessObjectEdited(bo);
        //}

        /// <summary>
        /// See <see cref="IGridBase.SelectedBusinessObjects"/>
        /// </summary>
        public IList<BusinessObject> SelectedBusinessObjects
        {
            get
            {
                if (_boCol == null) return new List<BusinessObject>();
                BusinessObjectCollection<BusinessObject> busObjects = new BusinessObjectCollection<BusinessObject>
                    (this.ClassDef);
                foreach (IDataGridViewRow row in _gridBase.SelectedRows)
                {
                    BusinessObject businessObject = (BusinessObject) GetBusinessObjectAtRow(row.Index);
                    busObjects.Add(businessObject);
                }
                return busObjects;
            }
        }

        /// <summary>
        /// See <see cref="IGridBase.GridLoader"/>
        /// </summary>
        public GridLoaderDelegate GridLoader
        {
            get { return this._gridLoader; }
            set { this._gridLoader = value; }
        }

        /// <summary>
        /// See <see cref="IGridControl.UiDefName"/>
        /// </summary>
        public string UiDefName { get; set; }

        /// <summary>
        /// See <see cref="IGridControl.ClassDef"/>
        /// </summary>
        public IClassDef ClassDef { get; set; }

        private void FireCollectionChanged()
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// See <see cref="IBOColSelectorControl.BusinessObjectCollection"/>
        /// </summary>
        public IBusinessObjectCollection GetBusinessObjectCollection()
        {
            return _boCol;
        }

        /// <summary>
        /// See <see cref="IBOColSelectorControl.GetBusinessObjectAtRow"/>
        /// </summary>
        public IBusinessObject GetBusinessObjectAtRow(int rowIndex)
        {
            int i = 0;

            if (_gridBase.DataSource is DataView)
            {
                foreach (DataRowView dataRowView in _dataTableDefaultView)
                {
                    if (i++ == rowIndex)
                    {
                        Guid result = GetRowObjectIDValue(dataRowView);
                        return this._dataSetProvider.Find(result);
                    }
                }
            }
            else
            {
                IDataGridViewRow findRow = _gridBase.Rows[rowIndex];
                Guid value = GetRowObjectIDValue(findRow);
                return _boCol == null ? null : this._boCol.Find(value);
            }
            return null;
        }

        /// <summary>
        /// See <see cref="IGridBase.GetBusinessObjectRow"/>
        /// </summary>
        public IDataGridViewRow GetBusinessObjectRow(IBusinessObject businessObject)
        {
            if (businessObject == null) return null;
            Guid boIdGuid = businessObject.ID.ObjectID;
            foreach (IDataGridViewRow row in _gridBase.Rows)
            {
                if (GetRowObjectIDValue(row) == boIdGuid)
                {
                    return row;
                }
            }
            return null;
        }

        private Guid GetRowObjectIDValue(DataRowView dataRowView)
        {
            object idValue = dataRowView.Row[_dataSetProvider.IDColumnName];
            Guid result;
            StringUtilities.GuidTryParse(idValue.ToString(), out result);
            return result;
        }

        private Guid GetRowObjectIDValue(IDataGridViewRow row)
        {
            object idValue = row.Cells[IDColumnName].Value;
            if (idValue == null) return Guid.Empty;

            Guid result;
            StringUtilities.GuidTryParse(idValue.ToString(), out result);
            return result;
        }

        ///<summary>
        /// Returns the name of the column being used for tracking the business object identity.
        /// If a <see cref="IDataSetProvider"/> is used then it will be the <see cref="IDataSetProvider.IDColumnName"/>
        /// Else it will be "HABANERO_OBJECTID".
        ///</summary>
        public string IDColumnName
        {
            get { return _dataSetProvider != null ? _dataSetProvider.IDColumnName : "HABANERO_OBJECTID"; }
        }


        /// <summary>
        /// Gets and sets whether this selector autoselects the first item or not when a new collection is set.
        /// </summary>
        public bool AutoSelectFirstItem { get; set; }

        /// <summary>
        /// See <see cref="IBOColSelectorControl.Clear"/>
        /// </summary>
        public void Clear()
        {
            SetBusinessObjectCollection(null);
        }

        /// <summary>
        /// See <see cref="IGridBase.ApplyFilter"/>
        /// </summary>
        public void ApplyFilter(IFilterClause filterClause)
        {
            if (_dataTableDefaultView == null)
            {
                throw new GridBaseInitialiseException
                    ("You cannot apply filters as the collection for the grid has not been set");
            }
            var filterClauseString = filterClause != null ? filterClause.GetFilterClauseString() : null;
            try
            {
                _dataTableDefaultView.RowFilter = filterClauseString;
            }
            catch (Exception e)
            {
                throw new HabaneroApplicationException(
                    e.Message + Environment.NewLine + "An Error Occured while trying to Filter the grid with filterClause '" +
                    filterClauseString + "'", e);
                
            }
        }

        /// <summary>
        /// See <see cref="IGridBase.RefreshGrid"/>
        /// </summary>
        public void RefreshGrid()
        {
            _gridBase.CancelEdit();
            IBusinessObjectCollection col = this._gridBase.BusinessObjectCollection;
            IBusinessObject bo = this._gridBase.SelectedBusinessObject;
            SetBusinessObjectCollection(null);
            SetBusinessObjectCollection(col);
            SelectedBusinessObject = bo;
        }
    }

    /// <summary>
    /// Thrown when a failure occurs while setting up a grid
    /// </summary>
    public class GridBaseSetUpException : Exception
    {
        ///<summary>
        /// Base constructor with info and context for seraialisation
        ///</summary>
        ///<param name="info"></param>
        ///<param name="context"></param>
        public GridBaseSetUpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        ///<summary>
        /// Constructor with message and inner exception
        ///</summary>
        ///<param name="message"></param>
        ///<param name="innerException"></param>
        public GridBaseSetUpException(string message, Exception innerException) : base(message, innerException)
        {
        }

        ///<summary>
        /// Constructor with a basic message
        ///</summary>
        ///<param name="message"></param>
        public GridBaseSetUpException(string message) : base(message)
        {
        }

        ///<summary>
        /// Base constructor with no parameters
        ///</summary>
        public GridBaseSetUpException()
        {
        }
    }

    /// <summary>
    /// Thrown when a failure occurs on a grid, indicating that Habanero developers
    /// need to pay attention to aspects of the code
    /// </summary>
    public class GridDeveloperException : HabaneroDeveloperException
    {
        ///<summary>
        ///</summary>
        ///<param name="message"></param>
        public GridDeveloperException(string message) : base(message, "")
        {
        }

        ///<summary>
        ///</summary>
        ///<param name="message"></param>
        ///<param name="inner"></param>
        public GridDeveloperException(string message, Exception inner) : base(message, "", inner)
        {
        }

        ///<summary>
        ///</summary>
        ///<param name="info"></param>
        ///<param name="context"></param>
        public GridDeveloperException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        ///<summary>
        ///</summary>
        public GridDeveloperException()
        {
        }
    }

    /// <summary>
    /// Thrown when a failure occurred during the initialisation of a grid
    /// </summary>
    public class GridBaseInitialiseException : HabaneroDeveloperException
    {
        ///<summary>
        ///</summary>
        ///<param name="info"></param>
        ///<param name="context"></param>
        public GridBaseInitialiseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        ///<summary>
        ///</summary>
        ///<param name="message"></param>
        ///<param name="inner"></param>
        public GridBaseInitialiseException(string message, Exception inner) : base(message, "", inner)
        {
        }

        ///<summary>
        ///</summary>
        ///<param name="message"></param>
        public GridBaseInitialiseException(string message) : base(message, "")
        {
        }

        ///<summary>
        ///</summary>
        public GridBaseInitialiseException()
        {
        }
    }
}