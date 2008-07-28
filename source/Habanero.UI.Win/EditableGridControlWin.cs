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
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Base.FilterControl;
using Habanero.UI.Base.Grid;

namespace Habanero.UI.Win
{
    public class EditableGridControlWin : UserControlWin, IEditableGridControl
    {
        private readonly IControlFactory _controlFactory;
        private readonly IEditableGrid _grid;
        private readonly EditableGridControlManager _editableGridManager;
        private IEditableGridButtonsControl _buttons;
        private IFilterControl _filterControl;
        private string _orderBy;
        private string _additionalSearchCriteria;

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
            //TODO copy rest from readonly version
        }

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
                BusinessObjectCollection<BusinessObject> collection =
                    new BusinessObjectCollection<BusinessObject>(this.ClassDef);
                string searchClause = _filterControl.GetFilterClause().GetFilterClauseString("%", "'");
                if (!string.IsNullOrEmpty(AdditionalSearchCriteria))
                {
                    if (!string.IsNullOrEmpty(searchClause))
                    {
                        searchClause += " AND ";
                    }
                    searchClause += AdditionalSearchCriteria;
                }
                collection.Load(searchClause, OrderBy);
                SetBusinessObjectCollection(collection);
            }
            else
            {
                this.Grid.ApplyFilter(_filterControl.GetFilterClause());
            }
        }

        public IGridBase Grid
        {
            get { return _grid; }
        }

        public void Initialise(IClassDef classDef)
        {
            _editableGridManager.Initialise(classDef);
            
        }

        public void Initialise(IClassDef classDef, string uiDefName)
        {
            _editableGridManager.Initialise(classDef, uiDefName);
        }


        public string UiDefName
        {
            get { return _editableGridManager.UiDefName; }
            set { _editableGridManager.UiDefName = value; }
        }

        public IClassDef ClassDef
        {
            get { return _editableGridManager.ClassDef; }
            set { _editableGridManager.ClassDef = value; }
        }

        /// <summary>
        /// Sets the business object collection to display.  Loading of
        /// the collection needs to be done before it is assigned to the
        /// grid.  This method assumes a default ui definition is to be
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

        public IEditableGridButtonsControl Buttons
        {
            get { return _buttons; }
        }

        /// <summary>
        /// returns the filter control for the readonly grid
        /// </summary>
        public IFilterControl FilterControl
        {
            get { return _filterControl; }
        }

        /// <summary>
        /// gets and sets the filter modes for the grid i.e. Filter or search <see cref="FilterModes"/>
        /// </summary>
        public FilterModes FilterMode
        {
            get { return _filterControl.FilterMode; }
            set { _filterControl.FilterMode=value; }
        }


        /// <summary>
        /// Gets and sets the default order by clause used for loading the grid when the <see cref="FilterMode"/>
        /// is Search see <see cref="FilterModes"/>
        /// </summary>
        public string OrderBy
        {
            get { return _orderBy; }
            set { _orderBy = value; }
        }

        /// <summary>
        /// Gets and sets the standard search criteria used for loading the grid when the <see cref="FilterMode"/>
        /// is Search see <see cref="FilterModes"/>. This search criteria will be And (ed) to any search criteria returned
        /// by the FilterControl.
        /// </summary>
        public string AdditionalSearchCriteria
        {
            get { return _additionalSearchCriteria; }
            set { _additionalSearchCriteria = value; }
        }

        private void Buttons_CancelClicked(object sender, EventArgs e)
        {
            this._grid.RejectChanges();
        }

        private void Buttons_SaveClicked(object sender, EventArgs e)
        {
            this._grid.SaveChanges();
        }
    }
}