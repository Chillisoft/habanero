using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Habanero.BO;
using Habanero.UI.Base;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Maps a ListView object in a user interface
    /// </summary>
    public class ListViewCollectionMapper
    {
        private string _uiDefName;
        private ListView _listView;
		private IBusinessObjectCollection _collection;
        private Hashtable _listItemsHash;

        /// <summary>
        /// Constructor to initialise a new mapper.  Sets the UIDefName to
        /// "default".
        /// </summary>
        /// <param name="listView">The ListView object to map</param>
        public ListViewCollectionMapper(ListView listView) : this(listView, "default")
        {

        }

        /// <summary>
        /// Constructor as before, but allows a UIDefName to be specified
        /// </summary>
        public ListViewCollectionMapper(ListView listView, string uiDefName)
        {
            Permission.Check(this);
            _listView = listView;
            _uiDefName = uiDefName;
            _listItemsHash = new Hashtable();
        }

        /// <summary>
        /// Returns the currently selected business object in the ListView
        /// or null if none is selected
        /// </summary>
        public BusinessObject SelectedBusinessObject
        {
            get {
                if (_listView.SelectedItems.Count == 1)
                {
                    return (BusinessObject)_listView.SelectedItems[0].Tag;
                } else return null;
            }
        }

        /// <summary>
        /// Specify the collection of objects to display and add these to the
        /// ListView object
        /// </summary>
        /// <param name="collection">The collection of business objects</param>
		public void SetCollection(IBusinessObjectCollection collection)
        {

            if (_collection != null)
            {
                _collection.BusinessObjectAdded -= new EventHandler<BOEventArgs>(BusinessObjectAddedHandler);
                _collection.BusinessObjectRemoved -= new EventHandler<BOEventArgs>(BusinessObjectRemovedHandler);
            }
            _collection = collection;
            SetListViewCollection(_listView, _collection);
            //SetupRightClickBehaviour();
            _collection.BusinessObjectAdded += new EventHandler<BOEventArgs>(BusinessObjectAddedHandler);
            _collection.BusinessObjectRemoved += new EventHandler<BOEventArgs>(BusinessObjectRemovedHandler);
        }

        /// <summary>
        /// A handler that updates the display when a business object has been
        /// removed from the collection
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectRemovedHandler(object sender, BOEventArgs e)
        {
            _listView.Items.Remove((ListViewItem) _listItemsHash[e.BusinessObject]);
        }

        /// <summary>
        /// A handler that updates the display when a business object has been
        /// added to the collection
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectAddedHandler(object sender, BOEventArgs e)
        {
            _listView.Items.Add(CreateListViewItem(e.BusinessObject));
        }

        /// <summary>
        /// Creates a ListViewItem from the business object provided.  This
        /// method is used by SetListViewCollection() to populate the ListView.
        /// </summary>
        /// <param name="bo">The business object to represent</param>
        /// <returns>Returns a new ListViewItem</returns>
        private ListViewItem CreateListViewItem(BusinessObject bo) {
            ListViewItem boItem = new ListViewItem(bo.ToString());
            boItem.Tag = bo;
            _listItemsHash.Add(bo, boItem);
            return boItem;
        }

        /// <summary>
        /// Adds the business objects in the collection to the ListView. This
        /// method is used by SetCollection.
        /// </summary>
        /// <param name="listView">The ListView object to add to</param>
        /// <param name="collection">The business object collection</param>
		private void SetListViewCollection(ListView listView, IBusinessObjectCollection collection)
        {
            listView.Clear();
            _listItemsHash.Clear();
            listView.MultiSelect = true;
            foreach (BusinessObject bo in collection) {
                listView.Items.Add(CreateListViewItem(bo));
               
            }
        }
    }
}