//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using System.Collections;
using System.Collections.Generic;
using Habanero.BO.ClassDefinition;
using Habanero.BO.CriteriaManager;

namespace Habanero.BO
{
    /// <summary>
    /// Models a collection of business objects.  This interface has been provided to
    /// circumvent the strong typing of BusinessObjectCollection.
    /// </summary>
	public interface IBusinessObjectCollection : ICollection, IEnumerable //IList<BusinessObject>
	{
		/// <summary>
		/// Handles the event of a business object being added
		/// </summary>
		event EventHandler<BOEventArgs> BusinessObjectAdded;

		/// <summary>
		/// Handles the event of a business object being removed
		/// </summary>
		event EventHandler<BOEventArgs> BusinessObjectRemoved;

		/// <summary>
		/// Returns the class definition of the collection
		/// </summary>
		ClassDef ClassDef { get;}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
		/// </summary>
		/// <returns>
		/// True if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.
		/// </returns>
		///
		bool IsReadOnly { get; }

        /// <summary>
        /// Indicates whether any of the business objects have been amended 
        /// since they were last persisted
        /// </summary>
        bool IsDirty { get; }

		/// <summary>
		/// Returns a sample business object held by the collection, which is
		/// constructed from the class definition
		/// </summary>
		BusinessObject SampleBo
		{
			get;
		}

		/// <summary>
		/// Finds a business object that has the key string specified.<br/>
		/// Note: the format of the search term is strict, so that a Guid ID
		/// may be stored as "boIDname=########-####-####-####-############".
		/// In the case of such Guid ID's, rather use the FindByGuid() function.
		/// Composite primary keys may be stored otherwise, such as a
		/// concatenation of the key names.
		/// </summary>
		/// <param name="key">The orimary key as a string</param>
		/// <returns>Returns the business object if found, or null if not</returns>
		BusinessObject Find(string key);

		/// <summary>
		/// Returns a new collection that is a copy of this collection
		/// </summary>
		/// <returns>Returns the cloned copy</returns>
		IBusinessObjectCollection Clone();

		/// <summary>
		/// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"></see>.
		/// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
        /// <returns>
		/// The index of item if found in the list; otherwise, -1.
		/// </returns>
		int IndexOf(BusinessObject item);

		/// <summary>
		/// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"></see> at the specified index.
		/// </summary>
		/// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
		void Insert(int index, BusinessObject item);

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
		/// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
        /// <returns>The element at the specified index.</returns>
        BusinessObject this[int index] { get; set; }

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">
		/// The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		void Add(BusinessObject item);

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
		/// </summary>
		/// <returns>
		/// True if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
		/// </returns>
		/// <param name="item">The object to locate in the 
		/// <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		bool Contains(BusinessObject item);

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
		/// <exception cref="T:System.ArgumentNullException">Array is null.</exception>
		/// <exception cref="T:System.ArgumentException">Array is multidimensional or arrayIndex
		/// is equal to or greater than the length of array.-or-The number of elements in
		/// the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is 
		/// greater than the available space from arrayIndex to the end of the destination array, or 
		/// Type T cannot be cast automatically to the type of the destination array.</exception>
		void CopyTo(BusinessObject[] array, int arrayIndex);

		/// <summary>
		/// Removes the first occurrence of a specific object from the 
		/// <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        /// <returns>
        /// True if item was successfully removed from the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
        /// This method also returns false if item is not found in the original
        /// <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </returns>
        bool Remove(BusinessObject item);

		/// <summary>
		/// Clears the collection
		/// </summary>
		void Clear();

        /// <summary>
        /// Indicates whether all of the business objects in the collection
        /// have valid values, amending an error message if any object is
        /// invalid
        /// </summary>
        /// <param name="errorMessage">An error message to amend</param>
        /// <returns>Returns true if all are valid</returns>
        bool IsValid(out string errorMessage);

        /// <summary>
        /// Commits to the database all the business objects that are either
        /// new or have been altered since the last committal
        /// </summary>
        void SaveAll();

        /// <summary>
        /// Sorts the collection by the property specified. The second parameter
        /// indicates whether this property is a business object property or
        /// whether it is a property defined in the code.  For example, a full name
        /// would be a code-calculated property that is not itself a business
        /// object property, even though it uses the BO properties of first name
        /// and surname, and the argument would thus be set as false.
        /// </summary>
        /// <param name="propertyName">The property name to sort on</param>
        /// <param name="isBoProperty">Whether the property is a business
        /// object property</param>
        /// <param name="isAscending">Whether to sort in ascending order, set
        /// false for descending order</param>
        void Sort(string propertyName, bool isBoProperty, bool isAscending);

        /// <summary>
        /// Loads business objects that match the search criteria provided,
        /// loaded in the order specified.  
        /// Use empty quotes, (or the LoadAll method) to load the
        /// entire collection for the type of object.
        /// </summary>
        /// <param name="searchCriteria">The search criteria</param>
        /// <param name="orderByClause">The order-by clause</param>
        void Load(string searchCriteria, string orderByClause);

        /// <summary>
        /// Loads business objects that match the search criteria provided in
        /// an expression, loaded in the order specified
        /// </summary>
        /// <param name="searchExpression">The search expression</param>
        /// <param name="orderByClause">The order-by clause</param>
        void Load(IExpression searchExpression, string orderByClause);
	}
}