using System;
using System.Collections;
using System.Windows.Forms;
using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;
using Chillisoft.UI.Generic.v2;
using Chillisoft.Util.v2;
using log4net;

namespace Chillisoft.UI.Application.v2
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
        public delegate void SetBusinessObjectDelegate(BusinessObjectBase bo);

        private static ILog log = LogManager.GetLogger("Chillisoft.UI.Application.v2.ReadOnlyGridWithButtons");
        public event EventHandler ItemSelected;
        public event EventHandler ItemActioned;

        private SimpleReadOnlyGrid itsGrid;
        private ReadOnlyGridButtonControl itsButtons;
        private DelayedMethodCall itsItemSelectedMethodCaller;
        //private SetGridDataProviderDelegate setGridDataProvider;
        //private BusinessObjectBaseCollection _collection;

        private int itsOldRowNumber = -1;
        private IGridDataProvider itsProvider;
        private IList itsItemSelectedDelegates;

        /// <summary>
        /// Constructor to initialise a new grid
        /// </summary>
        public ReadOnlyGridWithButtons()
        {
            itsItemSelectedMethodCaller = new DelayedMethodCall(500, this);
            BorderLayoutManager manager = new BorderLayoutManager(this);
            itsGrid = new SimpleReadOnlyGrid();
            itsGrid.Name = "GridControl";
            manager.AddControl(itsGrid, BorderLayoutManager.Position.Centre);
            itsButtons = new ReadOnlyGridButtonControl(itsGrid);
            itsButtons.Name = "ButtonControl";
            manager.AddControl(itsButtons, BorderLayoutManager.Position.South);

            itsGrid.CurrentCellChanged += new EventHandler(CurrentCellChangedHandler);
            itsGrid.DataProviderUpdated += new EventHandler(DataProviderUpdatedHandler);
            itsGrid.FilterUpdated += new EventHandler(GridFilterUpdatedHandler);
            itsItemSelectedDelegates = new ArrayList();
            //setGridDataProvider = new SetGridDataProviderDelegate(Grid.SetGridDataProvider) ;
        }

        /// <summary>
        /// Constructor to initialise the grid with a data provider, object
        /// editor and object creator
        /// </summary>
        /// <param name="dataProvider">The data provider</param>
        /// <param name="editor">The object editor</param>
        /// <param name="creator">The object creator</param>
        public ReadOnlyGridWithButtons(IGridDataProvider dataProvider, IObjectEditor editor, IObjectCreator creator)
            : this()
        {
            itsProvider = dataProvider;
            this.Grid.SetGridDataProvider(dataProvider);
            this.Buttons.ObjectEditor = editor;
            if (creator != null)
            {
                this.Buttons.ObjectCreator = creator;
            }
            else
            {
                this.Buttons.ObjectCreator = new DefaultBOCreator(itsProvider.GetCollection().ClassDef);
            }
        }

        /// <summary>
        /// Constructor to initialise a grid with a data provider and object
        /// editor
        /// </summary>
        /// <param name="dataProvider">The data provider</param>
        /// <param name="editor">The object editor</param>
        public ReadOnlyGridWithButtons(IGridDataProvider dataProvider, IObjectEditor editor)
            : this(dataProvider, editor, null)
        {
        }

        /// <summary>
        /// Constructor to initialise a grid with a data provider
        /// </summary>
        /// <param name="dataProvider">The data provider</param>
        public ReadOnlyGridWithButtons(IGridDataProvider dataProvider)
            : this()
        {
            itsProvider = dataProvider;
            this.Grid.SetGridDataProvider(dataProvider);
            this.Buttons.ObjectEditor = new DefaultBOEditor();
            this.Buttons.ObjectCreator = new DefaultBOCreator(itsProvider.ClassDef);
        }

        /// <summary>
        /// Handles the event of the grid filter being updated
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void GridFilterUpdatedHandler(object sender, EventArgs e)
        {
            //log.Debug("GridFilterUpdated - firing item selected");
            itsOldRowNumber = -1;
            FireItemSelectedIfCurrentRowChanged();
        }

        /// <summary>
        /// Handles the event of the data provider being updated
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void DataProviderUpdatedHandler(object sender, EventArgs e)
        {
            itsOldRowNumber = -1;
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
            if (itsGrid.CurrentCell != null)
            {
                if (itsOldRowNumber != itsGrid.CurrentCell.RowIndex)
                {
                    itsOldRowNumber = itsGrid.CurrentCell.RowIndex;
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
            itsItemSelectedDelegates.Add(boDelegate);
        }

        /// <summary>
        /// Calls the item selected handler for each of the selected item's
        /// delegates
        /// </summary>
        private void FireItemSelected()
        {
            if (this.GetSelectedObject() != null || this.GetSelectedObject() is BusinessObjectBase)
            {
                foreach (SetBusinessObjectDelegate selectedDelegate in itsItemSelectedDelegates)
                {
                    selectedDelegate((BusinessObjectBase) this.GetSelectedObject());
                }
            }
            itsItemSelectedMethodCaller.Call(new VoidMethodWithSender(DelayedItemSelected));
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
            get { return itsGrid; }
        }

        /// <summary>
        /// Returns the button control held. This property can be used
        /// to access a range of functionality for the button control
        /// (ie. myGridWithButtons.Buttons.AddButton("Delete", delegate(object sender, EventArgs e) {  }))
        /// </summary>
        public ReadOnlyGridButtonControl Buttons
        {
            get { return itsButtons; }
        }

        /// <summary>
        /// Sets the parent object
        /// </summary>
        /// <param name="parentObject">The parent object to set to</param>
        public void SetParentObject(object parentObject)
        {
            itsProvider.SetParentObject(parentObject);
            //BeginInvoke(setGridDataProvider, new object[] {itsProvider}); // needed to do the call on the Forms thread.  See info about STA thread model.

            this.Grid.SetGridDataProvider(itsProvider);
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
        public void SetBusinessObjectCollection(BusinessObjectBaseCollection boCollection)
        {
            itsProvider = new CollectionGridDataProvider(boCollection);
            this.Grid.SetGridDataProvider(itsProvider);
            this.Buttons.ObjectEditor = new DefaultBOEditor();
            this.Buttons.ObjectCreator = new DefaultBOCreator(itsProvider.ClassDef);
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
            itsOldRowNumber = -1;
            FireItemSelectedIfCurrentRowChanged();
        }

        /// <summary>
        /// Removes the specified object from the list
        /// </summary>
        /// <param name="objectToRemove">The object to remove</param>
        public void RemoveObject(object objectToRemove)
        {
            this.Grid.RemoveBusinessObject((BusinessObjectBase) objectToRemove);
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
            this.Grid.AddBusinessObject((BusinessObjectBase) objectToAdd);
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
        public BusinessObjectBaseCollection GetCollectionClone()
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