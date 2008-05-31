using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Base.FilterControl;

namespace Habanero.UI.WebGUI
{
    public class ReadOnlyGridControlGiz : ControlGiz, IReadOnlyGridControl, System.ComponentModel.ISupportInitialize
    {
        private readonly IControlFactory _controlFactory;
        private readonly ReadOnlyGridGiz _grid;
        private readonly IReadOnlyGridButtonsControl _buttons;
        private IBusinessObjectEditor _businessObjectEditor;
        private IBusinessObjectCreator _businessObjectCreator;
        private IBusinessObjectDeletor _businessObjectDeletor;
        private string _uiDefName = "";
        private ClassDef _classDef;
        private readonly IFilterControl _filterControl;
        private bool _isInitialised = false;
        private readonly GridInitialiser _gridInitialiser;

        //private IBusinessObjectCollection _collection;

        /// <summary>
        /// Constructor to initialise a new grid
        /// </summary>
        public ReadOnlyGridControlGiz()
        {
      

            _controlFactory = new ControlFactoryGizmox();
            _filterControl = new FilterControlGiz(_controlFactory);
            _grid = new ReadOnlyGridGiz();
            _buttons = _controlFactory.CreateReadOnlyGridButtonsControl();
            _gridInitialiser = new GridInitialiser(this);
            //InitialiseGrid();
            InitialiseButtons();
            InitialiseFilterControl();

            BorderLayoutManager borderLayoutManager = new BorderLayoutManagerGiz(this, _controlFactory);
            borderLayoutManager.AddControl(_grid, BorderLayoutManager.Position.Centre);
            borderLayoutManager.AddControl(_buttons, BorderLayoutManager.Position.South);
            borderLayoutManager.AddControl(_filterControl, BorderLayoutManager.Position.North);
            FilterMode = FilterModes.Filter;
            _grid.Name = "GridControl";

        }

        private void InitialiseFilterControl()
        {
            _filterControl.Filter +=_filterControl_OnFilter;
        }

        private void _filterControl_OnFilter(object sender, EventArgs e)
        {
            this.Grid.CurrentPage = 1;
            if (FilterMode == FilterModes.Search)
            {
                BusinessObjectCollection<BusinessObject> collection = new BusinessObjectCollection<BusinessObject>(this.ClassDef);
                collection.Load(_filterControl.GetFilterClause().GetFilterClauseString("%", "'"), "");
                SetBusinessObjectCollection(collection);
            }
            else
            {
                this.Grid.ApplyFilter(_filterControl.GetFilterClause());
            }
        }

        private void InitialiseButtons()
        {
            
            _buttons.AddClicked += Buttons_AddClicked;
            _buttons.EditClicked += Buttons_EditClicked;
            _buttons.DeleteClicked += Buttons_DeleteClicked;
            _buttons.Name = "ButtonControl";

        }

        /// <summary>
        /// initiliase the grid to the with the 'default' UIdef.
        /// </summary>
        public void Initialise(ClassDef classDef)
        {
            
            _gridInitialiser.InitialiseGrid(classDef);
            _isInitialised = true;
           
        }

        public void Initialise(ClassDef classDef, string uiDefName)
        {
            if (classDef == null) throw new ArgumentNullException("classDef");
            if (uiDefName == null) throw new ArgumentNullException("uiDefName");


            _classDef = classDef;
            _uiDefName = uiDefName;

            _gridInitialiser.InitialiseGrid(classDef, uiDefName);
            _isInitialised = true;

        }

        




        private void Buttons_DeleteClicked(object sender, EventArgs e)
        {
            if (this.Grid.GetBusinessObjectCollection() == null)
            {
                throw new GridDeveloperException("You cannot call delete since the grid has not been set up");
            }
            IBusinessObject selectedBo = SelectedBusinessObject;

            if (selectedBo != null)
            {
                _grid.SelectedBusinessObject = null;
                try
                {
                    _businessObjectDeletor.DeleteBusinessObject(selectedBo);
                }
                catch (Exception ex)
                {
                    GlobalRegistry.UIExceptionNotifier.Notify(ex, "There was a problem deleting", "Problem Deleting");
                }
            }
        }
        public delegate void RefreshGridDelegate();
        private void Buttons_EditClicked(object sender, EventArgs e)
        {
            if (this.Grid.GetBusinessObjectCollection() == null)
            {
                throw new GridDeveloperException("You cannot call edit since the grid has not been set up");
            }
            IBusinessObject selectedBo = SelectedBusinessObject;
            if (selectedBo != null)
            {
                if (_businessObjectEditor != null)
                {
                    _businessObjectEditor.EditObject(selectedBo, _uiDefName, delegate
                                                                                 {
                                                                                     this.Grid.RefreshGrid();
                                                                                 });
                }
            }
        }

        private void Buttons_AddClicked(object sender, EventArgs e)
        {
            if (this.Grid.GetBusinessObjectCollection() == null)
            {
                throw new GridDeveloperException("You cannot call add since the grid has not been set up");
            }
            IBusinessObject newBo;
            if (_businessObjectCreator == null)
            {
                throw new GridDeveloperException("You cannot call add as there is no business object creator set up for the grid");
            }
            newBo = _businessObjectCreator.CreateBusinessObject();
            if (_businessObjectEditor != null && newBo != null)
            {
                _businessObjectEditor.EditObject(newBo, _uiDefName, delegate(IBusinessObject bo) 
                    { this.Grid.SelectedBusinessObject= (BusinessObject) bo; });
            }
        }


        /// <summary>
        /// Returns the grid object held. This property can be used to
        /// access a range of functionality for the grid
        /// (eg. myGridWithButtons.Grid.AddBusinessObject(...)).
        /// </summary>
        public IGridBase Grid
        {
            get { return _grid; }
        }

        /// <summary>
        /// Gets or sets the single selected business object (null if none are selected)
        /// denoted by where the current selected cell is
        /// </summary>
        public BusinessObject SelectedBusinessObject
        {
            get { return _grid.SelectedBusinessObject; }
            set { _grid.SelectedBusinessObject = value; }
        }

        /// <summary>
        /// Returns the button control held. This property can be used
        /// to access a range of functionality for the button control
        /// (eg. myGridWithButtons.Buttons.AddButton(...)).
        /// </summary>
        public IReadOnlyGridButtonsControl Buttons
        {
            get { return _buttons; }
        }

        public IBusinessObjectEditor BusinessObjectEditor
        {
            get { return _businessObjectEditor; }
            set { _businessObjectEditor = value; }
        }

        public IBusinessObjectCreator BusinessObjectCreator
        {
            get { return _businessObjectCreator; }
            set { _businessObjectCreator = value; }
        }

        public IBusinessObjectDeletor BusinessObjectDeletor
        {
            get { return _businessObjectDeletor; }
            set { _businessObjectDeletor = value; }
        }

        public string UiDefName
        {
            get { return _uiDefName; }
            set { _uiDefName = value; }
        }

        public ClassDef ClassDef
        {
            get { return _classDef; }
            set { _classDef = value; }
        }

        public IFilterControl FilterControl
        {
            get { return _filterControl; }
        }

        public bool IsInitialised
        {
            get { return _isInitialised; }
        }

        public FilterModes FilterMode
        {
            get { return this._filterControl.FilterMode; }
            set { this._filterControl.FilterMode = value; }
        }


        public void SetBusinessObjectCollection(IBusinessObjectCollection boCollection)
        {
            if (boCollection == null )
            {
                _grid.SetBusinessObjectCollection(null);
                this.Buttons.Enabled = false;
                this.FilterControl.Enabled = false;
                return;
            }
            if (_classDef == null)
            {
                Initialise(boCollection.ClassDef);
            }else
            {
                if (_classDef != boCollection.ClassDef)
                {
                    throw new ArgumentException(
                        "You cannot call set collection for a collection that has a different class def than is initialised");
                }
            }
            if (this.BusinessObjectCreator  is DefaultBOCreator )
            {
                this.BusinessObjectCreator = new DefaultBOCreator(boCollection);
            }
            if (this.BusinessObjectCreator == null) this.BusinessObjectCreator = new DefaultBOCreator(boCollection);
            if (this.BusinessObjectEditor == null ) this.BusinessObjectEditor = new DefaultBOEditor(_controlFactory);
            if (this.BusinessObjectDeletor == null) this.BusinessObjectDeletor = new DefaultBODeletor();

            _grid.SetBusinessObjectCollection(boCollection);

            this.Buttons.Enabled = true;
            this.FilterControl.Enabled = true;
        }

        ///<summary>
        ///Signals the object that initialization is starting.
        ///</summary>
        ///
        public void BeginInit()
        {
            ((System.ComponentModel.ISupportInitialize)this.Grid).BeginInit();
        }

        ///<summary>
        ///Signals the object that initialization is complete.
        ///</summary>
        ///
        public void EndInit()
        {
            ((System.ComponentModel.ISupportInitialize)this.Grid).EndInit();
        }
    }
}