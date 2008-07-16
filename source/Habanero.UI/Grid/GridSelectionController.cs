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
using Habanero.Base;
using Habanero.BO;
using Habanero.Util;

namespace Habanero.UI.Grid
{
    /// <summary>
    /// Delegate for setting the business object
    /// </summary>
    /// <param name="bo">The business object</param>
    public delegate void SetBusinessObjectDelegate(IBusinessObject bo);

    ///<summary>
    /// Controls the ability of the grid to keep track of it's currently selected Business Object
    ///</summary>
    public class GridSelectionController
    {

        private readonly GridBase _grid;
        private readonly DelayedMethodCall _itemSelectedMethodCaller;
        private VoidMethodWithSender _delayedItemSelectedMethod;
        private readonly List<SetBusinessObjectDelegate> _itemSelectedDelegates;
        //private int _oldRowNumber = -1;
        private IBusinessObject _currentBusinessObject;
		
        ///<summary>
        /// Initialises this GridSelectionController for the specified GridBase
        ///</summary>
        ///<param name="grid">The GridBase to control the selection for.</param>
        public GridSelectionController(GridBase grid)
        {
            _itemSelectedMethodCaller = new DelayedMethodCall(500, this);
            _grid = grid;
            _grid.CurrentCellChanged += CurrentCellChangedHandler;
            _grid.CollectionChanged += CollectionChangedHandler;
            _grid.FilterUpdated += GridFilterUpdatedHandler;
            _itemSelectedDelegates = new List<SetBusinessObjectDelegate>();
        }
        
        ///<summary>
        /// Returns the grid which this selection controller is controlling.
        ///</summary>
        public GridBase Grid
        {
            get { return _grid; }
        }

        ///<summary>
        /// Fires after all of the other ItemSelected Delegates and is delayed by about 500ms.
        ///</summary>
        public VoidMethodWithSender DelayedItemSelected
        {
            get { return _delayedItemSelectedMethod; }
            set { _delayedItemSelectedMethod = value; }
        }

        /// <summary>
        /// Gets or sets the single selected business object denoted by 
        /// where the current selected cell is. Returns null if none are selected.
        /// </summary>
        public BusinessObject SelectedBusinessObject
        {
            get { return _grid.GetSelectedBusinessObject(); }
            set { _grid.SetSelectedBusinessObject(value); }
        }

        ///<summary>
        /// Refreshes the current selection and fires the ItemSelected event if the 
        /// Selected Business Object has changed.
        ///</summary>
        public void RefreshSelection()
        {
            FireItemSelectedIfCurrentBusinessObjectChanged();
        }

        /// <summary>
        /// Forces the ItemSelected event to fire.
        /// </summary>
        public void Reselect()
        {
            FireItemSelected();
        }

        /// <summary>
        /// Adds another delegate to those of the selected item
        /// </summary>
        /// <param name="boDelegate">The delegate to add</param>
        public void AddItemSelectedDelegate(SetBusinessObjectDelegate boDelegate)
        {
            _itemSelectedDelegates.Add(boDelegate);
        }

        /// <summary>
        /// Calls the item selected handler for each of the selected item's
        /// delegates
        /// </summary>
        private void FireItemSelected()
        {
            IBusinessObject businessObject = SelectedBusinessObject;
            _currentBusinessObject = businessObject;
            if (businessObject != null)
            {
                foreach (SetBusinessObjectDelegate selectedDelegate in _itemSelectedDelegates)
                {
                    selectedDelegate(businessObject);
                }
            }
            if (_delayedItemSelectedMethod != null)
            {
                _itemSelectedMethodCaller.Call(_delayedItemSelectedMethod);
            }
        }
        
        #region Event Handling

        /// <summary>
        /// Handles the event of the grid filter being updated
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void GridFilterUpdatedHandler(object sender, EventArgs e)
        {
            //_oldRowNumber = -1;
            FireItemSelectedIfCurrentBusinessObjectChanged();
        }

        /// <summary>
        /// Handles the event of the data provider being updated
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void CollectionChangedHandler(object sender, EventArgs e)
        {
            //_oldRowNumber = -1;
            FireItemSelectedIfCurrentBusinessObjectChanged();
        }

        /// <summary>
        /// Handles the event of the current cell being changed
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void CurrentCellChangedHandler(object sender, EventArgs e)
        {
            FireItemSelectedIfCurrentBusinessObjectChanged();
        }

        /// <summary>
        /// Creates an item selected event if the current row has changed
        /// </summary>
        private void FireItemSelectedIfCurrentBusinessObjectChanged()
        {
            //int rowIndex = -1;
            //if (_grid.CurrentCell != null)
            //{
            //    rowIndex = _grid.CurrentCell.RowIndex;
            //}
            IBusinessObject businessObject = SelectedBusinessObject;
            //if (_oldRowNumber != rowIndex)
            if (businessObject != _currentBusinessObject)
            {
                //_oldRowNumber = rowIndex;
                FireItemSelected();
            }
        }

        #endregion //Event Handling
    }
}
