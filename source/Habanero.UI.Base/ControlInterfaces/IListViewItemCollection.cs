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

using System.Collections;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Represents the collection of items in a ListView control or assigned to a ListViewGroup
    /// </summary>
    public interface IListViewItemCollection
    {
    //    ///// <summary>
    //    ///// Adds the specified STR text.
    //    ///// </summary>
    //    ///// <param name="strText">STR text.</param>
    //    ///// <returns></returns>
    //    //IListViewItem Add(string strText);

    //    /// <summary>
    //    /// Adds the specified obj list view item.
    //    /// </summary>
    //    /// <param name="objListViewItem">Obj list view item.</param>
    //    /// <returns></returns>
    //    IListViewItem Add(IListViewItem objListViewItem);

    //    ///// <summary>
    //    ///// Adds the specified text.
    //    ///// </summary>
    //    ///// <param name="strText">Text.</param>
    //    ///// <param name="imageIndex">Image index.</param>
    //    ///// <returns></returns>
    //    //IListViewItem Add(string strText, int imageIndex);

    //    ///// <summary>
    //    ///// Removes the specified list view item.
    //    ///// </summary>
    //    ///// <param name="objListViewItem">list view item.</param>
    //    //void Remove(IListViewItem objListViewItem);

    //    ///// <summary>
    //    ///// Get the index the of the specified list view item.
    //    ///// </summary>
    //    ///// <param name="objListViewItem">Obj list view item.</param>
    //    ///// <returns></returns>
    //    //int IndexOf(IListViewItem objListViewItem);

    //    ///// <summary>
    //    ///// Clears this list items.
    //    ///// </summary>
    //    //void Clear();

    //    ///// <summary>
    //    ///// Gets an enumerator.
    //    ///// </summary>
    //    ///// <returns></returns>
    //    //IEnumerator GetEnumerator();

    //    ///// <summary>
    //    ///// Gets the <see cref="IListViewItem"/> at the specified int index.
    //    ///// </summary>
    //    ///// <value></value>
    //    //IListViewItem this[int intIndex] { get; }

    //    ///// <summary>
    //    /////
    //    ///// </summary>
    //    ///// <param name="index"></param>
    //    //void RemoveAt(int index);

    //    ///// <summary>Creates a new item and inserts it into the collection at the specified index.</summary>
    //    ///// <returns>The <see cref="IListViewItem"></see> that was inserted into the collection.</returns>
    //    ///// <param name="index">The zero-based index location where the item is inserted. </param>
    //    ///// <param name="text">The text to display for the item. </param>
    //    ///// <exception cref="T:System.ArgumentOutOfRangeException">The index parameter is less than 0 or greater than the value of the <see cref="P:Gizmox.WebGUI.Forms.ListView.ListViewItemCollection.Count"></see> property of the <see cref="T:Gizmox.WebGUI.Forms.ListView.ListViewItemCollection"></see>. </exception>
    //    //IListViewItem Insert(int index, string text);

    //    ///// <summary>Inserts an existing <see cref="IListView"></see> into the collection at the specified index.</summary>
    //    ///// <returns>The <see cref="IListView"></see> that was inserted into the collection.</returns>
    //    ///// <param name="item">The <see cref="IListView"></see> that represents the item to insert. </param>
    //    ///// <param name="index">The zero-based index location where the item is inserted. </param>
    //    ///// <exception cref="T:System.ArgumentOutOfRangeException">The index parameter is less than 0 or greater than the value of the <see cref="P:Gizmox.WebGUI.Forms.ListView.ListViewItemCollection.Count"></see> property of the <see cref="T:Gizmox.WebGUI.Forms.ListView.ListViewItemCollection"></see>. </exception>
    //    //IListViewItem Insert(int index, IListViewItem item);

    //    ///// <summary>Creates a new item with the specified image index and inserts it into the collection at the specified index.</summary>
    //    ///// <returns>The <see cref="IListView"></see> that was inserted into the collection.</returns>
    //    ///// <param name="imageIndex">The index of the image to display for the item. </param>
    //    ///// <param name="index">The zero-based index location where the item is inserted. </param>
    //    ///// <param name="text">The text to display for the item. </param>
    //    ///// <exception cref="T:System.ArgumentOutOfRangeException">The index parameter is less than 0 or greater than the value of the <see cref="P:Gizmox.WebGUI.Forms.ListView.IListViewItemCollection.Count"></see> property of the <see cref="T:Gizmox.WebGUI.Forms.ListView.IListViewItemCollection"></see>. </exception>
    //    //IListViewItem Insert(int index, string text, int imageIndex);

    //    ///// <summary>Creates a new item with the specified text and image and inserts it in the collection at the specified index.</summary>
    //    ///// <returns>The <see cref="IListView"></see> added to the collection.</returns>
    //    ///// <param name="imageKey">The key of the image to display for the item.</param>
    //    ///// <param name="index">The zero-based index location where the item is inserted. </param>
    //    ///// <param name="text">The text of the <see cref="IListView"></see>.</param>
    //    ///// <exception cref="T:System.ArgumentOutOfRangeException">The index parameter is less than 0 or greater than the value of the <see cref="P:Gizmox.WebGUI.Forms.ListView.ListViewItemCollection.Count"></see> property of the <see cref="T:Gizmox.WebGUI.Forms.ListView.ListViewItemCollection"></see>. </exception>
    //    //IListViewItem Insert(int index, string text, string imageKey);

    //    ///// <summary>Creates a new item with the specified key, text, and image, and inserts it in the collection at the specified index.</summary>
    //    ///// <returns>The <see cref="IListView"></see> added to the collection.</returns>
    //    ///// <param name="imageIndex">The index of the image to display for the item.</param>
    //    ///// <param name="key">The <see cref="e"></see> of the item.</param>
    //    ///// <param name="index">The zero-based index location where the item is inserted</param>
    //    ///// <param name="text">The text of the item.</param>
    //    ///// <exception cref="T:System.ArgumentOutOfRangeException">The index parameter is less than 0 or greater than the value of the <see cref="P:Gizmox.WebGUI.Forms.ListView.ListViewItemCollection.Count"></see> property of the <see cref="T:Gizmox.WebGUI.Forms.ListView.ListViewItemCollection"></see>. </exception>
    //    //IListViewItem Insert(int index, string key, string text, int imageIndex);

    //    ///// <summary>Creates a new item with the specified key, text, and image, and adds it to the collection at the specified index.</summary>
    //    ///// <returns>The <see cref="IListView"></see> added to the collection.</returns>
    //    ///// <param name="imageKey">The key of the image to display for the item.</param>
    //    ///// <param name="key">The <see cref=""></see> of the item. </param>
    //    ///// <param name="index">The zero-based index location where the item is inserted.</param>
    //    ///// <param name="text">The text of the item.</param>
    //    ///// <exception cref="T:System.ArgumentOutOfRangeException">The index parameter is less than 0 or greater than the value of the <see cref=ListView.ListViewItemCollection.Count"></see> property of the <see cref="ListViewItemCollection"></see>. </exception>
    //    //IListViewItem Insert(int index, string key, string text, string imageKey);

    //    ///// <summary>
    //    /////
    //    ///// </summary>
    //    ///// <param name="value"></param>
    //    //bool Contains(object value);

    //    ///// <summary>
    //    /////
    //    ///// </summary>
    //    ///// <param name="value"></param>
    //    //int IndexOf(object value);
    //    int Count { get; }

    //    /// <summary>
    //    /// Gets or sets the item at the specified index within the collection
    //    /// </summary>
    //    IListViewItem this[int index] { get; }
    }
}