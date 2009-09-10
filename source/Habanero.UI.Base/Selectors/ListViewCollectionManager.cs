//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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

//using System;
//using System.Collections;
//using Habanero.Base;
//using Habanero.BO;
//using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Maps a ListView object in a user interface
    /// </summary>
    public class ListViewCollectionManager
    {
//        private readonly string _uiDefName;
//        private readonly IListView _listView;
//        private readonly ClassDef _classDef;
//        //private IBusinessObjectCollection _collection;
//        //private readonly Hashtable _listItemsHash;
//
//        /// <summary>
//        /// Constructor to initialise a new mapper.  Sets the UIDefName to
//        /// "default".
//        /// </summary>
//        /// <param name="listView">The ListView object to map</param>
//        /// <param name="classDef">the class defintion of the class that this controller is setting up the list view for</param>
//        public ListViewCollectionManager(IListView listView,ClassDef classDef)
//            : this(listView, classDef, "default")
//        {
// 
//        }
//
//        /// <summary>
//        /// Constructor as before, but allows a UIDefName to be specified
//        /// </summary>
//        /// <param name="listView">The ListView object to map</param>
//        /// <param name="classDef">the class defintion of the class that this controller is setting up the list view for</param>
//        /// <param name="uiDefName">The ui definition that the list view be set up for i.e. the properties that will be shown in the list view</param>
//        public ListViewCollectionManager(IListView listView,ClassDef classDef, string uiDefName)
//        {
//            _listView = listView;
//            _classDef = classDef;
//            _uiDefName = uiDefName;
//            //_listItemsHash = new Hashtable();
//        }
//
//        public IListView ListView
//        {
//            get { return _listView; }
//        }
//
//        public ClassDef ClassDef
//        {
//            get { return _classDef; }
//        }
//
//        public string UiDefName
//        {
//            get { return _uiDefName; }
//        }
//
//        /// <summary>
//        /// Returns the currently selected business object in the ListView
//        /// or null if none is selected
//        /// </summary>
//        //public IBusinessObject SelectedBusinessObject
//        //{
//        //    get
//        //    {
//        //        if (_listView.SelectedItems.Count == 1)
//        //        {
//        //            return (BusinessObject)_listView.SelectedItems[0].Tag;
//        //        }
//        //        else return null;
//        //    }
//        //}
//
//        /// <summary>
//        /// Specify the collection of objects to display and add these to the
//        /// ListView object
//        /// </summary>
//        /// <param name="collection">The collection of business objects</param>
//        //public void SetCollection(IBusinessObjectCollection collection)
//        //{
//
//        //    if (_collection != null)
//        //    {
//        //        _collection.BusinessObjectAdded -= new EventHandler<BOEventArgs>(BusinessObjectAddedHandler);
//        //        _collection.BusinessObjectRemoved -= new EventHandler<BOEventArgs>(BusinessObjectRemovedHandler);
//        //    }
//        //    _collection = collection;
//        //    SetListViewCollection(_listView, _collection);
//        //    //SetupRightClickBehaviour();
//        //    _collection.BusinessObjectAdded += new EventHandler<BOEventArgs>(BusinessObjectAddedHandler);
//        //    _collection.BusinessObjectRemoved += new EventHandler<BOEventArgs>(BusinessObjectRemovedHandler);
//        //}
//
//        ///// <summary>
//        ///// A handler that updates the display when a business object has been
//        ///// removed from the collection
//        ///// </summary>
//        ///// <param name="sender">The object that notified of the event</param>
//        ///// <param name="e">Attached arguments regarding the event</param>
//        //private void BusinessObjectRemovedHandler(object sender, BOEventArgs e)
//        //{
//        //    _listView.Items.Remove((ListViewItem)_listItemsHash[e.BusinessObject]);
//        //}
//
//        ///// <summary>
//        ///// A handler that updates the display when a business object has been
//        ///// added to the collection
//        ///// </summary>
//        ///// <param name="sender">The object that notified of the event</param>
//        ///// <param name="e">Attached arguments regarding the event</param>
//        //private void BusinessObjectAddedHandler(object sender, BOEventArgs e)
//        //{
//        //    _listView.Items.Add(CreateListViewItem(e.BusinessObject));
//        //}
//
//        /// <summary>
//        /// Creates a ListViewItem from the business object provided.  This
//        /// method is used by SetListViewCollection() to populate the ListView.
//        /// </summary>
//        /// <param name="bo">The business object to represent</param>
//        /// <returns>Returns a new ListViewItem</returns>
////        private IListViewItem CreateListViewItem(IBusinessObject bo)
////        {
//////           IListViewItem boItem = new ListViewItem(bo.ToString());
////            IListViewItem boItem = _listView.CreateListViewItem(bo.ToString());
//
////            boItem.Tag = bo;
////            _listItemsHash.Add(bo, boItem);
////            return boItem;
////        }
//
////        /// <summary>
////        /// Adds the business objects in the collection to the ListView. This
////        /// method is used by SetBusinessObjectCollection.
////        /// </summary>
////        /// <param name="listView">The ListView object to add to</param>
////        /// <param name="collection">The business object collection</param>
////        private void SetListViewCollection(IListView listView, IBusinessObjectCollection collection)
////        {
////            listView.Clear();
////            _listItemsHash.Clear();
////            listView.MultiSelect = true;
////            foreach (IBusinessObject bo in collection)
////            {
////                listView.Items.Add(CreateListViewItem(bo));
//
////            }
////        }
//        public void SetBusinessObjectCollection(IBusinessObjectCollection col)
//        {
//  
//        }
    }
}