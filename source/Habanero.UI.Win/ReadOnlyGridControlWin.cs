using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Base.FilterControl;

namespace Habanero.UI.Win
{
    public class ReadOnlyGridControlWin : ControlWin, IReadOnlyGridControl
    {
        private IBusinessObjectEditor _BusinessObjectEditor;
        private IBusinessObjectCreator _BusinessObjectCreator;
        private IBusinessObjectDeletor _businessObjectDeletor;
        private ClassDef _classDef;
        private IFilterControl _filterControl;
        private string _uiDefName = "";
        private readonly ReadOnlyGridWin _grid;
        private readonly IGridInitialiser _gridInitialiser;
        private string _orderBy;
        private IControlFactory _controlFactory;


        public ReadOnlyGridControlWin()
        {
            _controlFactory = new ControlFactoryWin();
            _grid = new ReadOnlyGridWin();
            _grid.Name = "GridControl";
            _gridInitialiser = new GridInitialiser(this, _controlFactory);
        }

        /// <summary>
        /// initiliase the grid to the with the 'default' UIdef.
        /// </summary>
        public void Initialise(ClassDef classDef)
        {
            _gridInitialiser.InitialiseGrid(classDef);
        }

        public void Initialise(ClassDef classDef, string uiDefName)
        {
            _gridInitialiser.InitialiseGrid(classDef, uiDefName);
        }

        public IGridBase Grid
        {
            get { return _grid; }
        }

        public IBusinessObject SelectedBusinessObject
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Returns the button control held. This property can be used
        /// to access a range of functionality for the button control
        /// (eg. myGridWithButtons.Buttons.AddButton(...)).
        /// </summary>
        public IReadOnlyGridButtonsControl Buttons
        {
            get { throw new System.NotImplementedException(); }
        }

        public IBusinessObjectEditor BusinessObjectEditor
        {
            get { return _BusinessObjectEditor; }
            set { _BusinessObjectEditor = value; }
        }

        public IBusinessObjectCreator BusinessObjectCreator
        {
            get { return _BusinessObjectCreator; }
            set { _BusinessObjectCreator = value; }
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
            get { return _gridInitialiser.IsInitialised; }
        }

        public FilterModes FilterMode
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Gets and sets the default order by clause used for loading the grid when the <see cref="IReadOnlyGridControl.FilterMode"/>
        /// is Search see <see cref="FilterModes"/>
        /// </summary>
        public string OrderBy
        {
            get { return _orderBy; }
            set { _orderBy = value; }
        }


        public void SetBusinessObjectCollection(IBusinessObjectCollection boCollection)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Initialises the grid based with no classDef. This is used where the columns are set up manually.
        /// A typical case of when you would want to set the columns manually would be when the grid
        ///  requires alternate columns e.g. images to indicate the state of the object or buttons/links.
        /// The grid must already have at least one column added. At least one column must be a column with the name
        /// "ID" This column is used to synchronise the grid with the business objects.
        /// </summary>
        /// <exception cref="GridBaseInitialiseException"> in the case where the columns have not already been defined for the grid</exception>
        public void Initialise()
        {
            _gridInitialiser.InitialiseGrid();
        }


        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }
    }
}