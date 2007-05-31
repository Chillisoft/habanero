using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Chillisoft.Bo.v2;

namespace Chillisoft.UI.BOControls.v2
{
    /// <summary>
    /// Maps a ListView object in a user interface
    /// </summary>
    public class ListViewCollectionMapper
    {
        private string itsUIDefName;
        private ListView itsListView;
        private BusinessObjectBaseCollection itsCollection;
        private Hashtable itsListItemsHash;

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
            itsListView = listView;
            itsUIDefName = uiDefName;
            itsListItemsHash = new Hashtable();
        }

        /// <summary>
        /// Returns the currently selected business object in the ListView
        /// or null if none is selected
        /// </summary>
        public BusinessObjectBase SelectedBusinessObject
        {
            get {
                if (itsListView.SelectedItems.Count == 1)
                {
                    return (BusinessObjectBase)itsListView.SelectedItems[0].Tag;
                } else return null;
            }
        }

        /// <summary>
        /// Specify the collection of objects to display and add these to the
        /// ListView object
        /// </summary>
        /// <param name="collection">The collection of business objects</param>
        public void SetCollection(BusinessObjectBaseCollection collection) {

            if (itsCollection != null)
            {
                itsCollection.BusinessObjectAdded -= new BusinessObjectEventHandler(BusinessObjectAddedHandler);
                itsCollection.BusinessObjectRemoved -= new BusinessObjectEventHandler(BusinessObjectRemovedHandler);
            }
            itsCollection = collection;
            SetListViewCollection(itsListView, itsCollection);
            //SetupRightClickBehaviour();
            itsCollection.BusinessObjectAdded += new BusinessObjectEventHandler(BusinessObjectAddedHandler);
            itsCollection.BusinessObjectRemoved += new BusinessObjectEventHandler(BusinessObjectRemovedHandler);
        }

        /// <summary>
        /// A handler that updates the display when a business object has been
        /// removed from the collection
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectRemovedHandler(object sender, BusinessObjectEventArgs e)
        {
            itsListView.Items.Remove((ListViewItem) itsListItemsHash[e.BusinessObject]);
        }

        /// <summary>
        /// A handler that updates the display when a business object has been
        /// added to the collection
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectAddedHandler(object sender, BusinessObjectEventArgs e)
        {
            itsListView.Items.Add(CreateListViewItem(e.BusinessObject));
        }

        /// <summary>
        /// Creates a ListViewItem from the business object provided.  This
        /// method is used by SetListViewCollection() to populate the ListView.
        /// </summary>
        /// <param name="bo">The business object to represent</param>
        /// <returns>Returns a new ListViewItem</returns>
        private ListViewItem CreateListViewItem(BusinessObjectBase bo) {
            ListViewItem boItem = new ListViewItem(bo.ToString());
            boItem.Tag = bo;
            itsListItemsHash.Add(bo, boItem);
            return boItem;
        }

        /// <summary>
        /// Adds the business objects in the collection to the ListView. This
        /// method is used by SetCollection.
        /// </summary>
        /// <param name="listView">The ListView object to add to</param>
        /// <param name="collection">The business object collection</param>
        private void SetListViewCollection(ListView listView, BusinessObjectBaseCollection collection) {
            listView.Clear();
            itsListItemsHash.Clear();
            listView.MultiSelect = true;
            foreach (BusinessObjectBase bo in collection) {
                listView.Items.Add(CreateListViewItem(bo));
               
            }
        }
    }
}
