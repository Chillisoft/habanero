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
using Habanero.Base;
using Habanero.BO;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Provides a combination of editable grid, filter and buttons used to edit a
    /// collection of business objects
    /// </summary>
    public class EditableGridControlWin : UserControlWin, IEditableGridControl
    {
        private readonly IEditableGridButtonsControl _buttons;
        private readonly IControlFactory _controlFactory;
        private readonly EditableGridControlManager _editableGridManager;
        private readonly IFilterControl _filterControl;
        private readonly IEditableGrid _grid;

        ///<summary>
        /// Constructs a new instance of a <see cref="EditableGridControlWin"/>.
        /// This uses the <see cref="IControlFactory"/> from the <see cref="GlobalUIRegistry"/> to construct the control.
        ///</summary>
        public EditableGridControlWin() : this(GlobalUIRegistry.ControlFactory)
        {
        }

        ///<summary>
        /// Constructs a new instance of a <see cref="EditableGridControlWin"/>.
        ///</summary>
        ///<param name="controlFactory">The <see cref="IControlFactory"/> to use to construct the control.</param>
        public EditableGridControlWin(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            _editableGridManager = new EditableGridControlManager(this, controlFactory);
            _grid = _controlFactory.CreateEditableGrid();
            _buttons = _controlFactory.CreateEditableGridButtonsControl();
            _filterControl = _controlFactory.CreateFilterControl();
            InitialiseButtons();
            InitialiseFilterControl();
            BorderLayoutManager manager = controlFactory.CreateBorderLayoutManager(this);
            manager.AddControl(_filterControl, BorderLayoutManager.Position.North);
            manager.AddControl(_grid, BorderLayoutManager.Position.Centre);
            manager.AddControl(_buttons, BorderLayoutManager.Position.South);
            this.Grid.BusinessObjectSelected += Grid_OnBusinessObjectSelected;
            //TODO copy rest from readonly version
        }

        /// <summary>
        /// Initiliases the grid structure using the default UI class definition (implicitly named "default")
        /// </summary>
        /// <param name="classDef">The class definition of the business objects shown in the grid</param>
        public void Initialise(IClassDef classDef)
        {
            _editableGridManager.Initialise(classDef);
        }

        /// <summary>
        /// Initialises the grid structure using the specified UI class definition
        /// </summary>
        /// <param name="classDef">The class definition of the business objects shown in the grid</param>
        /// <param name="uiDefName">The UI definition with the given name</param>
        public void Initialise(IClassDef classDef, string uiDefName)
        {
            _editableGridManager.Initialise(classDef, uiDefName);
        }

        /// <summary>
        /// Gets and sets the UI definition used to initialise the grid structure (the UI name is indicated
        /// by the "name" attribute on the UI element in the class definitions
        /// </summary>
        public string UiDefName
        {
            get { return _grid.UiDefName; }
            set { _grid.UiDefName = value; }
        }

        /// <summary>
        /// Gets and sets the class definition used to initialise the grid structure
        /// </summary>
        public IClassDef ClassDef
        {
            get { return _grid.ClassDef; }
            set { _grid.ClassDef = value; }
        }

        /// <summary>
        /// Returns the grid object held. This property can be used to
        /// access a range of functionality for the grid
        /// (eg. myGridWithButtons.Grid.AddBusinessObject(...)).
        /// </summary>    
        public IEditableGrid Grid
        {
            get { return _grid; }
        }

        /// <summary>
        /// Returns the editable grid object held. This property can be used to
        /// access a range of functionality for the grid
        /// (eg. myGridWithButtons.Grid.AddBusinessObject(...)).
        /// </summary>    
        IGridBase IGridControl.Grid
        {
            get { return _grid; }
        }

        /// <summary>
        /// Gets the button control, which contains a set of default buttons for
        /// editing the objects and can be customised
        /// </summary>
        IButtonGroupControl IGridControl.Buttons
        {
            get { return Buttons; }
        }

        ///<summary>
        /// Returns a Flag indicating whether this control has been initialised yet or not.
        ///</summary>
        public bool IsInitialised
        {
            get { return true; }
        }

        /// <summary>
        /// Gets and Sets the business object collection displayed in the grid.  This
        /// collection must be pre-loaded using the collection's Load() command or from the
        /// <see cref="IBusinessObjectLoader"/>.
        /// The default UI definition will be used, that is a 'ui' element 
        /// without a 'name' attribute.
        /// </summary>
        public IBusinessObjectCollection BusinessObjectCollection
        {
            get { return this.Grid.BusinessObjectCollection; }
            set { SetBusinessObjectCollection(value); }
        }

        /// <summary>
        /// Gets and sets the currently selected business object in the grid
        /// </summary>
        public IBusinessObject SelectedBusinessObject
        {
            get { return this.Grid.SelectedBusinessObject; }
            set { this.Grid.SelectedBusinessObject = value; }
        }

        /// <summary>
        /// Event Occurs when a business object is selected
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectSelected;

        private void Grid_OnBusinessObjectSelected(object sender, BOEventArgs e)
        {
            if (this.BusinessObjectSelected != null)
            {
                this.BusinessObjectSelected(this, new BOEventArgs(this.SelectedBusinessObject));
            }
        }

        /// <summary>
        /// Clears the business object collection and the rows in the data table
        /// </summary>
        public void Clear()
        {
            SetBusinessObjectCollection(null);
        }

        /// <summary>Gets the number of rows displayed in the <see cref="IBOSelectorControl"></see>.</summary>
        /// <returns>The number of rows in the <see cref="IBOSelectorControl"></see>.</returns>
        public int NoOfItems
        {
            get { return this.Grid.Rows.Count; }
        }


        /// <summary>
        /// Returns the business object at the specified row number
        /// </summary>
        /// <param name="row">The row number in question</param>
        /// <returns>Returns the busines object at that row, or null
        /// if none is found</returns>
        public IBusinessObject GetBusinessObjectAtRow(int row)
        {
            return this.Grid.GetBusinessObjectAtRow(row);
        }

        /// <summary>
        /// Sets the business object collection to display.  Loading of
        /// the collection needs to be done before it is assigned to the
        /// grid.  This method assumes a default UI definition is to be
        /// used, that is a 'ui' element without a 'name' attribute.
        /// </summary>
        /// <param name="boCollection">The new business object collection
        /// to be shown in the grid</param>
        public void SetBusinessObjectCollection(IBusinessObjectCollection boCollection)
        {
            if (boCollection == null)
            {
                //TODO: weakness where user could call _control.Grid.Set..(null) directly and bypass the disabling.
                _grid.BusinessObjectCollection = null;
                _grid.AllowUserToAddRows = false;
                Buttons.Enabled = false;
                FilterControl.Enabled = false;
                return;
            }
            if (ClassDef == null)
            {
                Initialise(boCollection.ClassDef);
            }
            else
            {
                if (ClassDef != boCollection.ClassDef)
                {
                    throw new ArgumentException(
                        "You cannot call set collection for a collection that has a different class def than is initialised");
                }
            }
            //if (this.BusinessObjectCreator is DefaultBOCreator)
            //{
            //    this.BusinessObjectCreator = new DefaultBOCreator(boCollection);
            //}
            //if (this.BusinessObjectCreator == null) this.BusinessObjectCreator = new DefaultBOCreator(boCollection);
            //if (this.BusinessObjectEditor == null) this.BusinessObjectEditor = new DefaultBOEditor(_controlFactory);
            //if (this.BusinessObjectDeletor == null) this.BusinessObjectDeletor = new DefaultBODeletor();

            _grid.BusinessObjectCollection = boCollection;

            Buttons.Enabled = true;
            FilterControl.Enabled = true;
            _grid.AllowUserToAddRows = true;
        }

        ///<summary>
        /// Returns the <see cref="IBusinessObjectCollection"/> that has been set for this <see cref="IGridControl"/>.
        ///</summary>
        ///<returns>Returns the <see cref="IBusinessObjectCollection"/> that has been set for this <see cref="IGridControl"/>.</returns>
        public IBusinessObjectCollection GetBusinessObjectCollection()
        {
            return Grid.BusinessObjectCollection;
        }

        /// <summary>
        /// Gets the buttons control used to save and cancel changes
        /// </summary>
        public IEditableGridButtonsControl Buttons
        {
            get { return _buttons; }
        }

        /// <summary>
        /// Gets the filter control used to filter which rows are shown in the grid
        /// </summary>
        public IFilterControl FilterControl
        {
            get { return _filterControl; }
        }

        /// <summary>
        /// Gets and sets the filter modes for the grid (i.e. filter or search). See <see cref="FilterModes"/>.
        /// </summary>
        public FilterModes FilterMode
        {
            get { return _filterControl.FilterMode; }
            set { _filterControl.FilterMode = value; }
        }

        /// <summary>
        /// Gets and sets the default order by clause used for loading the grid when the <see cref="FilterModes"/>
        /// is set to Search
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// Gets and sets the standard search criteria used for loading the grid when the <see cref="FilterModes"/>
        /// is set to Search. This search criteria will be appended with an AND to any search criteria returned
        /// by the FilterControl.
        /// </summary>
        public string AdditionalSearchCriteria { get; set; }

        private void InitialiseButtons()
        {
            _buttons.CancelClicked += Buttons_CancelClicked;
            _buttons.SaveClicked += Buttons_SaveClicked;
        }

        private void InitialiseFilterControl()
        {
            _filterControl.Filter += _filterControl_OnFilter;
        }

        private void _filterControl_OnFilter(object sender, EventArgs e)
        {
            //this.Grid.CurrentPage = 1;
            if (FilterMode == FilterModes.Search)
            {
                string searchClause = _filterControl.GetFilterClause().GetFilterClauseString("%", "'");
                if (!string.IsNullOrEmpty(AdditionalSearchCriteria))
                {
                    if (!string.IsNullOrEmpty(searchClause))
                    {
                        searchClause += " AND ";
                    }
                    searchClause += AdditionalSearchCriteria;
                }
                IBusinessObjectCollection collection = BORegistry.DataAccessor.BusinessObjectLoader.
                    GetBusinessObjectCollection(ClassDef, searchClause, OrderBy);
                SetBusinessObjectCollection(collection);
            }
            else
            {
                Grid.ApplyFilter(_filterControl.GetFilterClause());
            }
        }

        private void Buttons_CancelClicked(object sender, EventArgs e)
        {
            _grid.RejectChanges();
        }

        private void Buttons_SaveClicked(object sender, EventArgs e)
        {
            _grid.SaveChanges();
        }
    }
}