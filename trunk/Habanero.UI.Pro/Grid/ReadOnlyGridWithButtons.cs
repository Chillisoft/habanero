using System;
using System.Collections;
using System.Windows.Forms;
using Habanero.Bo;
using Habanero.Base;
using Habanero.Ui.Base;
using Habanero.Ui.Forms;
using Habanero.Ui.Grid;
using Habanero.Util;
using log4net;
using BusinessObject=Habanero.Bo.BusinessObject;

namespace Habanero.Ui.Grid
{
    /// <summary>
    /// Manages a read-only grid with buttons (ie. a grid that cannot be
    /// directly edited).  By default, an "Edit" and "Add" are added at 
    /// the bottom of the grid, which open up dialogs to edit the business
    /// object.<br/>
    /// To have further control of particular aspects of the buttons or
    /// grid, access the standard functionality through the Grid and
    /// Buttons properties 
    /// (ie. myGridWithButtons.Grid.SetGridDataProvider(...)).
    /// </summary>
    public class ReadOnlyGridWithButtons : UserControl, IObjectLister
    {
        /// <summary>
        /// Sets the business object delegate
        /// </summary>
        /// <param name="bo">The business object</param>
        public delegate void SetBusinessObjectDelegate(BusinessObject bo);

        private static ILog log = LogManager.GetLogger("Habanero.Ui.Grid.ReadOnlyGridWithButtons");
        public event EventHandler ItemSelected;
        public event EventHandler ItemActioned;

        private SimpleReadOnlyGrid _grid;
        private ReadOnlyGridButtonControl _buttons;
        private DelayedMethodCall _itemSelectedMethodCaller;
        //private SetGridDataProviderDelegate _setGridDataProvider;
        //private BusinessObjectBaseCollection _collection;

        private int _oldRowNumber = -1;
        private IGridDataProvider _provider;
        private IList _itemSelectedDelegates;

        /// <summary>
        /// Constructor to initialise a new grid.  Unless you plan to assign
        /// a data provider, editing form and object creation form yourself,
        /// rather use the other available constructors, such as
        /// ReadOnlyGridWithButtons(IGridDataProvider).
        /// </summary>
        public ReadOnlyGridWithButtons()
        {
            _itemSelectedMethodCaller = new DelayedMethodCall(500, this);
            BorderLayoutManager manager = new BorderLayoutManager(this);
            _grid = new SimpleReadOnlyGrid();
            _grid.Name = "GridControl";
            manager.AddControl(_grid, BorderLayoutManager.Position.Centre);
            _buttons = new ReadOnlyGridButtonControl(_grid);
            _buttons.Name = "ButtonControl";
            manager.AddControl(_buttons, BorderLayoutManager.Position.South);

            _grid.CurrentCellChanged += new EventHandler(CurrentCellChangedHandler);
            _grid.DataProviderUpdated += new EventHandler(DataProviderUpdatedHandler);
            _grid.FilterUpdated += new EventHandler(GridFilterUpdatedHandler);
            _itemSelectedDelegates = new ArrayList();
            //_setGridDataProvider = new SetGridDataProviderDelegate(Grid.SetGridDataProvider) ;
        }

        /// <summary>
        /// Constructor to initialise the grid with a data provider, object
        /// editor and object creator.  This constructor is suitable if you are
        /// using customised object creation and editing forms, otherwise it may
        /// be better to use ReadOnlyGridWithButtons(IGridDataProvider).
        /// </summary>
        /// <param name="dataProvider">The data provider to the grid.  An example
        /// usage would be "new CollectionGridDataProvider(yourBOCollection)".
        /// </param>
        /// <param name="editor">The form in which the user edits the object
        /// when "Edit" is clicked</param>
        /// <param name="creator">The form in which the user edits a new object
        /// when "Add" is clicked</param>
        public ReadOnlyGridWithButtons(IGridDataProvider dataProvider, IObjectEditor editor, IObjectCreator creator)
            : this()
        {
            _provider = dataProvider;
            this.Grid.SetGridDataProvider(dataProvider);
            this.Buttons.ObjectEditor = editor;
            if (creator != null)
            {
                this.Buttons.ObjectCreator = creator;
            }
            else
            {
                this.Buttons.ObjectCreator = new DefaultBOCreator(_provider.GetCollection().ClassDef);
            }
        }

        /// <summary>
        /// Constructor to initialise a grid with a data provider and object
        /// editor, setting the object creator to null.
        /// This constructor is suitable if you are
        /// using a customised object editing form, otherwise it may
        /// be better to use ReadOnlyGridWithButtons(IGridDataProvider).
        /// </summary>
        /// <param name="dataProvider">The data provider to the grid.  An example
        /// usage would be "new CollectionGridDataProvider(yourBOCollection)".
        /// </param>
        /// <param name="editor">The form in which the user edits the object
        /// when "Edit" is clicked</param>
        public ReadOnlyGridWithButtons(IGridDataProvider dataProvider, IObjectEditor editor)
            : this(dataProvider, editor, null)
        {
        }

        /// <summary>
        /// Constructor to initialise a grid with a data provider.  This
        /// constructor assigns the default object creation and editing forms
        /// to the grid (for use when the user clicks the Add or Edit buttons).
        /// </summary>
        /// <param name="dataProvider">The data provider to the grid.  An example
        /// usage would be "new CollectionGridDataProvider(yourBOCollection)".
        /// </param>
        public ReadOnlyGridWithButtons(IGridDataProvider dataProvider)
            : this()
        {
            _provider = dataProvider;
            this.Grid.SetGridDataProvider(dataProvider);
            this.Buttons.ObjectEditor = new DefaultBOEditor();
            this.Buttons.ObjectCreator = new DefaultBOCreator(_provider.ClassDef);
        }

        /// <summary>
        /// Handles the event of the grid filter being updated
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void GridFilterUpdatedHandler(object sender, EventArgs e)
        {
            //log.Debug("GridFilterUpdated - firing item selected");
            _oldRowNumber = -1;
            FireItemSelectedIfCurrentRowChanged();
        }

        /// <summary>
        /// Handles the event of the data provider being updated
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void DataProviderUpdatedHandler(object sender, EventArgs e)
        {
            _oldRowNumber = -1;
            FireItemSelectedIfCurrentRowChanged();
        }

        /// <summary>
        /// Handles the event of the current cell being changed
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void CurrentCellChangedHandler(object sender, EventArgs e)
        {
            FireItemSelectedIfCurrentRowChanged();
        }

        /// <summary>
        /// Creates an item selected event if the current row has changed
        /// </summary>
        private void FireItemSelectedIfCurrentRowChanged()
        {
            if (_grid.CurrentCell != null)
            {
                if (_oldRowNumber != _grid.CurrentCell.RowIndex)
                {
                    _oldRowNumber = _grid.CurrentCell.RowIndex;
                    FireItemSelected();
                }
            }
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
            if (this.GetSelectedObject() != null || this.GetSelectedObject() is BusinessObject)
            {
                foreach (SetBusinessObjectDelegate selectedDelegate in _itemSelectedDelegates)
                {
                    selectedDelegate((BusinessObject) this.GetSelectedObject());
                }
            }
            _itemSelectedMethodCaller.Call(new VoidMethodWithSender(DelayedItemSelected));
        }

        /// <summary>
        /// Creates a new item selected event
        /// </summary>
        /// <param name="sender">The sender</param>
        private void DelayedItemSelected(object sender)
        {
            if (this.ItemSelected != null)
            {
                this.ItemSelected(sender, new EventArgs());
            }
        }

        /// <summary>
        /// Creates an item actioned event
        /// </summary>
        private void FireItemActioned()
        {
            if (this.ItemActioned != null)
            {
                this.ItemActioned(this, new EventArgs());
            }
        }

        /// <summary>
        /// Returns the grid object held. This property can be used to
        /// access a range of functionality for the grid
        /// (ie. myGridWithButtons.Grid.SetGridDataProvider(...))
        /// </summary>
        public SimpleReadOnlyGrid Grid
        {
            get { return _grid; }
        }

        /// <summary>
        /// Returns the button control held. This property can be used
        /// to access a range of functionality for the button control
        /// (ie. myGridWithButtons.Buttons.AddButton("Delete", delegate(object sender, EventArgs e) {  }))
        /// </summary>
        public ReadOnlyGridButtonControl Buttons
        {
            get { return _buttons; }
        }

        /// <summary>
        /// Sets the parent object
        /// </summary>
        /// <param name="parentObject">The parent object to set to</param>
        public void SetParentObject(object parentObject)
        {
            _provider.SetParentObject(parentObject);
            //BeginInvoke(_setGridDataProvider, new object[] {_provider}); // needed to do the call on the Forms thread.  See info about STA thread model.

            this.Grid.SetGridDataProvider(_provider);
        }

        /// <summary>
        /// Sets the text of the action button
        /// </summary>
        /// <param name="text">The text to display on the button</param>
        public void SetActionButtonText(string text)
        {
            this.Buttons.AddButton(text, new EventHandler(SelectButtonClickedHandler));
        }

        /// <summary>
        /// Sets the business object collection being managed to that
        /// specified. This method would typically be used if either an
        /// unparameterised constructor was used to instantiate the
        /// grid, or if the business object collection is to be changed
        /// at some stage after instantiation.<br/>
        /// Alternatively, to have more specific control you could call
        /// *.Grid.SetGridDataProvider()
        /// and set the object editor and creator for the buttons as well,
        /// using *.Buttons.ObjectEditor and *.Buttons.ObjectCreator
        /// </summary>
        /// <param name="boCollection">The new business object collection
        /// to be shown in the grid. This collection must have been
        /// pre-loaded using the collection's Load() method</param>
        public void SetBusinessObjectCollection(BusinessObjectCollection<BusinessObject> boCollection)
        {
            _provider = new CollectionGridDataProvider(boCollection);
            this.Grid.SetGridDataProvider(_provider);
            this.Buttons.ObjectEditor = new DefaultBOEditor();
            this.Buttons.ObjectCreator = new DefaultBOCreator(_provider.ClassDef);
        }

        /// <summary>
        /// Returns the currently selected object
        /// </summary>
        /// <returns>Returns the currently selected object</returns>
        public object GetSelectedObject()
        {
            return this.Grid.SelectedBusinessObject;
        }

        /// <summary>
        /// Reselects the current row and creates a new item selected event
        /// if the current row has changed
        /// </summary>
        public void ReselectSelectedRow()
        {
            this.Grid.SelectedBusinessObject = null;
            _oldRowNumber = -1;
            FireItemSelectedIfCurrentRowChanged();
        }

        /// <summary>
        /// Removes the specified object from the list
        /// </summary>
        /// <param name="objectToRemove">The object to remove</param>
        public void RemoveObject(object objectToRemove)
        {
            this.Grid.RemoveBusinessObject((BusinessObject) objectToRemove);
            if (this.Grid.HasBusinessObjects)
            {
                FireItemSelected();
            }
        }

        /// <summary>
        /// Adds the specified object to the list
        /// </summary>
        /// <param name="objectToAdd">The object to add</param>
        public void AddObject(object objectToAdd)
        {
            this.Grid.AddBusinessObject((BusinessObject) objectToAdd);
        }

        /// <summary>
        /// Handles the event of the select button being pressed
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void SelectButtonClickedHandler(object sender, EventArgs e)
        {
            FireItemActioned();
        }

        /// <summary>
        /// Returns a cloned collection of the business objects in the grid
        /// </summary>
        /// <returns>Returns a business object collection</returns>
        public BusinessObjectCollection<BusinessObject> GetCollectionClone()
        {
            return this.Grid.GetCollectionClone();
        }

        /// <summary>
        /// Returns a list of the filtered business objects
        /// </summary>
        public IList FilteredBusinessObjects
        {
            get { return this.Grid.FilteredBusinessObjects; }
        }

    }
}