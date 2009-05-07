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
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// Provides a combination of editable grid, filter and buttons used to edit a
    /// collection of business objects
    /// </summary>
    public class EditableGridControlVWG : UserControlVWG, IEditableGridControl
    {
        private readonly IControlFactory _controlFactory;
        private readonly IEditableGrid _grid;
        private readonly EditableGridControlManager _editableGridManager;

        ///<summary>
        /// Constructs a new instance of a <see cref="EditableGridControlVWG"/>.
        /// This uses the <see cref="IControlFactory"/> from the <see cref="GlobalUIRegistry"/> to construct the control.
        ///</summary>
        public EditableGridControlVWG() : this(GlobalUIRegistry.ControlFactory)
        {
        }
        
        ///<summary>
        /// Constructs a new instance of a <see cref="EditableGridControlVWG"/>.
        ///</summary>
        ///<param name="controlFactory">The <see cref="IControlFactory"/> to use to construct the control.</param>
        public EditableGridControlVWG(IControlFactory controlFactory)
        {
            if (controlFactory == null) throw new HabaneroArgumentException("controlFactory", 
                    "Cannot create an editable grid control if the control factory is null");
            _controlFactory = controlFactory;
            _editableGridManager = new EditableGridControlManager(this, controlFactory);
            _grid = _controlFactory.CreateEditableGrid();
            Buttons = _controlFactory.CreateEditableGridButtonsControl();
            FilterControl = _controlFactory.CreateFilterControl();
            InitialiseButtons();
            InitialiseFilterControl();
            BorderLayoutManager manager = controlFactory.CreateBorderLayoutManager(this);
            manager.AddControl(FilterControl, BorderLayoutManager.Position.North);
            manager.AddControl(_grid, BorderLayoutManager.Position.Centre);
            manager.AddControl(Buttons, BorderLayoutManager.Position.South);
            //TODO copy rest from readonly version
        }

        private void InitialiseButtons()
        {
            Buttons.CancelClicked += Buttons_CancelClicked;
            Buttons.SaveClicked += Buttons_SaveClicked;
        }

        private void InitialiseFilterControl()
        {
            FilterControl.Filter += _filterControl_OnFilter;
        }

        private void _filterControl_OnFilter(object sender, EventArgs e)
        {
            this.Grid.CurrentPage = 1;
            if (FilterMode == FilterModes.Search)
            {
                string searchClause = FilterControl.GetFilterClause().GetFilterClauseString("%", "'");
                if (!string.IsNullOrEmpty(AdditionalSearchCriteria))
                {
                    if (!string.IsNullOrEmpty(searchClause))
                    {
                        searchClause += " AND ";
                    }
                    searchClause += AdditionalSearchCriteria;
                }
                IBusinessObjectCollection collection = BORegistry.GetDataAccessor(ClassDef.ClassType).BusinessObjectLoader.
                    GetBusinessObjectCollection(this.ClassDef, searchClause, OrderBy);
                SetBusinessObjectCollection(collection);
            }
            else
            {
                this.Grid.ApplyFilter(FilterControl.GetFilterClause());
            }
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
            get { return _grid.UiDefName;  }
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
                _grid.SetBusinessObjectCollection(null);
                _grid.AllowUserToAddRows = false;
                this.Buttons.Enabled = false;
                this.FilterControl.Enabled = false;
                return;
            }
            if (this.ClassDef == null)
            {
                Initialise(boCollection.ClassDef);
            }
            else
            {
                if (this.ClassDef != boCollection.ClassDef)
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

            _grid.SetBusinessObjectCollection(boCollection);

            this.Buttons.Enabled = true;
            this.FilterControl.Enabled = true;
            _grid.AllowUserToAddRows = true;
        }

        ///<summary>
        /// Returns the <see cref="IBusinessObjectCollection"/> that has been set for this <see cref="IGridControl"/>.
        ///</summary>
        ///<returns>Returns the <see cref="IBusinessObjectCollection"/> that has been set for this <see cref="IGridControl"/>.</returns>
        public IBusinessObjectCollection GetBusinessObjectCollection()
        {
            return Grid.GetBusinessObjectCollection();
        }

        /// <summary>
        /// Gets the buttons control used to save and cancel changes
        /// </summary>
        public IEditableGridButtonsControl Buttons { get; private set; }

        /// <summary>
        /// Gets the filter control used to filter which rows are shown in the grid
        /// </summary>
        public IFilterControl FilterControl { get; private set; }

        /// <summary>
        /// Gets and sets the filter modes for the grid (i.e. filter or search). See <see cref="FilterModes"/>.
        /// </summary>
        public FilterModes FilterMode
        {
            get { return FilterControl.FilterMode; }
            set { FilterControl.FilterMode = value; }
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

        private void Buttons_CancelClicked(object sender, EventArgs e)
        {
            _grid.SelectedBusinessObject = null;
           this._grid.RejectChanges();
        }  
        
        private void Buttons_SaveClicked(object sender, EventArgs e)
        {
            _grid.SelectedBusinessObject = null;
            this._grid.SaveChanges();
        }
    }
}