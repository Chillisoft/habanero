using System;
using Habanero.Base;
using Habanero.BO;

namespace Habanero.UI.Base
{
    /// <summary>
    /// This class provides mapping from a business object collection to a
    /// user interface ListBox.  This mapper is used at code level when
    /// you are explicitly providing a business object collection.
    /// </summary>
    public class ListBoxCollectionManager
    {
        private readonly IControlFactory _controlFactory;
        //private ListBoxRightClickController _listBoxRightClickController;


        /// <summary>
        /// Constructor to create a new collection ListBox mapper object.
        /// </summary>
        /// <param name="listBox">The ListBox object to map</param>
        /// <param name="controlFactory">The control factory used to create controls</param>
        public ListBoxCollectionManager(IListBox listBox, IControlFactory controlFactory)
        {
            if (listBox == null) throw new ArgumentNullException("listBox");
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            Control = listBox;
            _controlFactory = controlFactory;
            Control.SelectedIndexChanged += delegate { FireBusinessObjectSelected(); };
            this.AutoSelectFirstItem = true;
        }

        /// <summary>
        /// Sets the collection being represented to a specific collection
        /// of business objects
        /// </summary>
        /// <param name="collection">The collection to represent</param>
        /// <param name="includeBlank">Whether to a put a blank item at the
        /// top of the list</param>
        public void SetCollection(IBusinessObjectCollection collection, bool includeBlank)
        {
            if (Collection != null)
            {
                Collection.BusinessObjectAdded -= BusinessObjectAddedHandler;
                Collection.BusinessObjectRemoved -= BusinessObjectRemovedHandler;
            }
            Collection = collection;
            //TODO _Port: SetupListBoxRightClickController();
            SetListBoxCollection(Control, Collection);
            if (Collection == null) return;
            Collection.BusinessObjectAdded += BusinessObjectAddedHandler;
            Collection.BusinessObjectRemoved += BusinessObjectRemovedHandler;

        }
        /// <summary>
        /// Event Occurs when a business object is selected
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectSelected;
                    
        
        private void FireBusinessObjectSelected()
        {
            if (this.BusinessObjectSelected != null)
            {
                this.BusinessObjectSelected(this, new BOEventArgs(this.SelectedBusinessObject));
            }
        }

        //private void SetupListBoxRightClickController()
        //{
        //    _listBoxRightClickController = new ListBoxRightClickController(_listBox, _collection.ClassDef);
        //    _listBoxRightClickController.NewObjectCreated += NewListBoxObjectCreatedHandler;
        //}

        //private void NewListBoxObjectCreatedHandler(IBusinessObject businessObject)
        //{
        //    _collection.Add(businessObject);
        //    _listBox.SelectedItem = businessObject;
        //}

        /////<summary>
        ///// The controller used to handle the right-click pop-up form behaviour
        /////</summary>
        //public ListBoxRightClickController ListBoxRightClickController
        //{
        //    get { return _listBoxRightClickController; }
        //    set { _listBoxRightClickController = value; }
        //}

        /// <summary>
        /// This handler is called when a business object has been removed from
        /// the collection - it subsequently removes the item from the ListBox
        /// list as well.
        /// </summary>
        /// <param name="sender">The object that notified of the change</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectRemovedHandler(object sender, BOEventArgs e)
        {
            Control.Items.Remove(e.BusinessObject);
        }

        /// <summary>
        /// This handler is called when a business object has been added to
        /// the collection - it subsequently adds the item to the ListBox
        /// list as well.
        /// </summary>
        /// <param name="sender">The object that notified of the change</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectAddedHandler(object sender, BOEventArgs e)
        {
            Control.Items.Add(e.BusinessObject);
        }

        /// <summary>
        /// Returns the business object, in object form, that is currently 
        /// selected in the ListBox list, or null if none is selected
        /// </summary>
        public IBusinessObject SelectedBusinessObject
        {
            get
            {
                if (NoItemSelected() || NullItemSelected()) return null;

                return (BusinessObject)Control.SelectedItem;
            }
            set
            {
                Control.SelectedItem = ContainsValue(value) ? value : null;
            }
        }

        private bool ContainsValue(IBusinessObject value)
        {
            return (value != null && Control.Items.Contains(value));
        }

        private bool NullItemSelected()
        {
            return Control.SelectedItem is string && (Control.SelectedItem == null || (string)Control.SelectedItem == "");
        }

        private bool NoItemSelected()
        {
            return Control.SelectedIndex == -1;
        }

        /// <summary>
        /// Returns the ListBox control
        /// </summary>
        public IListBox Control { get; private set; }

        /// <summary>
        /// Returns the control factory used to generate controls
        /// such as the label
        /// </summary>
        public IControlFactory ControlFactory
        {
            get { return _controlFactory; }
        }

        /// <summary>
        /// Returns the collection used to populate the items shown in the ListBox
        /// </summary>
        public IBusinessObjectCollection Collection { get; private set; }

        /// <summary>
        /// Set the list of objects in the ListBox to a specific collection of
        /// business objects.<br/>
        /// Important: If you are changing the business object collection,
        /// use the SetBusinessObjectCollection method instead, which will call this method
        /// automatically.
        /// </summary>
        /// <param name="cbx">The ListBox being controlled</param>
        /// <param name="col">The business object collection used to populate the items list</param>
        protected void SetListBoxCollection(IListBox cbx, IBusinessObjectCollection col)
        {

            cbx.Items.Clear();
            if (col == null) return;

            foreach (IBusinessObject businessObject in col)
            {
                cbx.Items.Add(businessObject);
            }
            if (col.Count > 0 && this.AutoSelectFirstItem) cbx.SelectedIndex = 0;
        }


        ///// <summary>
        ///// Sets up a handler so that right-clicking on the ListBox will
        ///// allow the user to create a new business object using a form that is
        ///// provided.  A tooltip is also added to indicate this possibility to
        ///// the user.
        ///// </summary>
        //public void SetupRightClickBehaviour()
        //{
        //    _listBoxRightClickController.SetupRightClickBehaviour();
        //}

        ///<summary>
        /// Clears all items in the Combo Box and sets the selected item and <see cref="Collection"/>
        /// to null
        ///</summary>
        public void Clear()
        {
            SetCollection (null, false);
        }
        /// <summary>Gets the number of rows displayed in the <see cref="IBOColSelectorControl"></see>.</summary>
        /// <returns>The number of rows in the <see cref="IBOColSelectorControl"></see>.</returns>
        public int NoOfItems
        {
            get { return Control.Items.Count; }
        }
        


        /// <summary>
        /// Gets and sets whether this selector autoselects the first item or not when a new collection is set.
        /// </summary>
        public bool AutoSelectFirstItem { get; set; }

        /// <summary>
        /// Returns the business object at the specified row number
        /// </summary>
        /// <param name="row">The row number in question</param>
        /// <returns>Returns the busines object at that row, or null
        /// if none is found</returns>
        public IBusinessObject GetBusinessObjectAtRow(int row)
        {
            if (IndexOutOfRange(row)) return null;
            return (IBusinessObject)Control.Items[row];
        }

        private bool IndexOutOfRange(int row)
        {
            return row < 0 || row >= NoOfItems;
        }
    }
}