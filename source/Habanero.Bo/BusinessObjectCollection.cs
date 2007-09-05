using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using System.Security.Permissions;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.CriteriaManager;
using Habanero.DB;
using Habanero.Base;

namespace Habanero.BO
{
    
    //public delegate void BusinessObjectEventHandler(Object sender, BOEventArgs e);

    /// <summary>
    /// Manages a collection of business objects.  This class also serves
    /// as a base class from which most types of business object collections
    /// can be derived.<br/>
    /// To create a collection of business objects, inherit from this 
    /// class. The business objects contained in this collection must
    /// inherit from BusinessObject.
    /// </summary>
	public class BusinessObjectCollection<T> : List<T>, IEnumerable<T>, IBusinessObjectCollection where T : BusinessObject
    {
        private ClassDef _boClassDef;
        private IExpression _criteriaExpression;
        private string _orderByClause;
        private BusinessObject _sampleBo;
        private string _extraSearchCriteriaLiteral = "";
        private int _limit = -1;
        private Hashtable _lookupTable;
        //private ArrayList _list;

        /// <summary>
        /// Default constructor. The classdef will be implied from T
        /// </summary>
        public BusinessObjectCollection()
            : this(ClassDef.ClassDefs[typeof(T)])
    {}

        /// <summary>
        /// Use this constructor if you will only know T at run time - BusinessObject will be the generic type
        /// and the objects in the collection will be determined from the classDef passed in.
        /// </summary>
        /// <param name="classDef">The classdef of the objects to be contained in this collection</param>
        public BusinessObjectCollection(ClassDef classDef)
        {
            _boClassDef = classDef;
            //_list = new ArrayList();
            _sampleBo = _boClassDef.CreateNewBusinessObject();
            _lookupTable = new Hashtable();
        }

        /// <summary>
        /// Constructor to initialise a new collection with a
        /// class definition provided by an existing business object
        /// </summary>
        /// <param name="bo">The business object whose class definition
        /// is used to initialise the collection</param>
        public BusinessObjectCollection(T bo)
            : this(bo.ClassDef)
        {
        }

        /// <summary>
        /// Handles the event of a business object being added
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectAdded;
 
        /// <summary>
        /// Handles the event of a business object being removed
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectRemoved;

        /// <summary>
        /// Calls the BusinessObjectAdded() handler
        /// </summary>
        /// <param name="bo">The business object added</param>
        public void FireBusinessObjectAdded(T bo)
        {
            if (this.BusinessObjectAdded != null)
            {
                this.BusinessObjectAdded(this, new BOEventArgs(bo));
            }

        }

        /// <summary>
        /// Calls the BusinessObjectRemoved() handler
        /// </summary>
        /// <param name="bo">The business object removed</param>
        public void FireBusinessObjectRemoved(T bo)
        {
            if (this.BusinessObjectRemoved != null)
            {
                this.BusinessObjectRemoved(this, new BOEventArgs(bo));
            }
        }

        /// <summary>
        /// Adds a business object to the collection
        /// </summary>
        /// <param name="bo">The business object to add</param>
        public void Add(T bo)
        {
            base.Add(bo);
            _lookupTable.Add(bo.ID.ToString(), bo);
            bo.Deleted += new EventHandler<BOEventArgs>(BusinessObjectDeletedHandler);
            this.FireBusinessObjectAdded(bo);
        }

        /// <summary>
        /// Handles the event of a business object being deleted
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectDeletedHandler(object sender, BOEventArgs e)
        {
            this.Remove((T)e.BusinessObject);
        }

        /// <summary>
        /// Copies the business objects in one collection across to this one
        /// </summary>
        /// <param name="col">The collection to copy from</param>
        public void Add(BusinessObjectCollection<T> col)
        {
			Add((List<T>) col);
			//foreach (T bo in col)
			//{
			//    this.Add(bo);
			//}
        }

        /// <summary>
        /// Adds the businessobjects from col into this collecction
        /// </summary>
        /// <param name="col"></param>
        public void Add(List<T> col)
        {
            foreach (T bo in col)
            {
                this.Add(bo);
            }
        }

        ///// <summary>
        ///// Provides a numerical indexing facility so that the objects
        ///// in the collection can be accessed with square brackets like an array
        ///// </summary>
        ///// <param name="index">The index position</param>
        ///// <returns>Returns the business object at the position specified</returns>
        //public BusinessObject this[int index]
        //{
        //    get { return (BusinessObject) _list[index]; }
        //}

        /// <summary>
        /// Refreshes the business objects in the collection
        /// </summary>
        [ReflectionPermission(SecurityAction.Demand)]
        public void Refresh()
        {
            Clear();
            ISqlStatement refreshSql = new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection());
            refreshSql.Statement.Append(_sampleBo.GetSelectSql(_limit));
            if (_criteriaExpression != null)
            {
                refreshSql.AppendWhere();
                SqlCriteriaCreator creator = new SqlCriteriaCreator(_criteriaExpression, _boClassDef);
                creator.AppendCriteriaToStatement(refreshSql);
            }
            if (_extraSearchCriteriaLiteral.Length > 0)
            {
                refreshSql.AppendWhere();
                refreshSql.Statement.Append(_extraSearchCriteriaLiteral);
            }


            using (IDataReader dr = DatabaseConnection.CurrentConnection.LoadDataReader(refreshSql, _orderByClause))
            {
                try
                {
                    T lTempBusObj;
					lTempBusObj = (T)_boClassDef.InstantiateBusinessObject();
					//lTempBusObj = (BusinessObjectBase)Activator.CreateInstance(_boClassDef.ClassType, true);
					while (dr.Read())
                    {
                        //Load Business OBject from the data reader
                        Add((T)BOLoader.Instance.GetBusinessObject(lTempBusObj, dr));
                    }
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                    {
                        dr.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Loads business objects that match the search criteria provided,
        /// loaded in the order specified.  Use empty quotes to load the
        /// entire collection for the type of object.
        /// </summary>
        /// <param name="searchCriteria">The search criteria</param>
        /// <param name="orderByClause">The order-by clause</param>
        public void Load(string searchCriteria, string orderByClause)
        {
            Load(searchCriteria, orderByClause, "");
        }

        /// <summary>
        /// Loads business objects that match the search criteria provided,
        /// loaded in the order specified
        /// </summary>
        /// <param name="searchCriteria">The search criteria</param>
        /// <param name="orderByClause">The order-by clause</param>
        /// <param name="extraSearchCriteriaLiteral">Extra search criteria</param>
        /// TODO ERIC - what is the last one?
        public void Load(string searchCriteria, string orderByClause, string extraSearchCriteriaLiteral)
        {
            if (searchCriteria.Length > 0)
            {
                _criteriaExpression = Expression.CreateExpression(searchCriteria);
            }
            _orderByClause = orderByClause;
            _extraSearchCriteriaLiteral = extraSearchCriteriaLiteral;
            Refresh();
        }

        /// <summary>
        /// Loads business objects that match the search criteria provided,
        /// loaded in the order specified, and limiting the number of objects
        /// loaded
        /// </summary>
        /// <param name="searchCriteria">The search criteria</param>
        /// <param name="orderByClause">The order-by clause</param>
        /// <param name="limit">The limit</param>
        public void LoadWithLimit(string searchCriteria, string orderByClause, int limit)
        {
            _limit = limit;
            Load(searchCriteria, orderByClause);
        }

        /// <summary>
        /// Loads business objects that match the search criteria provided in
        /// an expression, loaded in the order specified
        /// </summary>
        /// <param name="searchExpression">The search expression</param>
        /// <param name="orderByClause">The order-by clause</param>
        public void Load(IExpression searchExpression, string orderByClause)
        {
            _criteriaExpression = searchExpression;
            _orderByClause = orderByClause;
            Refresh();
        }

        ///// <summary>
        ///// Returns the business object at the index position specified
        ///// </summary>
        ///// <param name="index">The index position</param>
        ///// <returns>Returns the business object at that position</returns>
        //public BusinessObject item(int index)
        //{
        //    return (BusinessObject) _list[index];
        //}

        /// <summary>
        /// Clears the collection
        /// </summary>
        public new void Clear()
        {
            base.Clear();
            _lookupTable.Clear();
        }

        /// <summary>
        /// Removes the business object at the index position specified
        /// </summary>
        /// <param name="index">The index position to remove from</param>
        public new void RemoveAt(int index)
        {
            T boToRemove = this[index];
            _lookupTable.Remove(boToRemove.ID.ToString());
            base.RemoveAt(index);
            this.FireBusinessObjectRemoved(boToRemove);
        }

        /// <summary>
        /// Removes the specified business object from the collection
        /// </summary>
        /// <param name="bo">The business object to remove</param>
        public bool Remove(T bo)
        {
			bool removed = base.Remove(bo);
            _lookupTable.Remove(bo.ID.ToString());
            this.FireBusinessObjectRemoved(bo);
        	return removed;
        }

        ///// <summary>
        ///// Indicates whether the collection contains the specified 
        ///// business object
        ///// </summary>
        ///// <param name="bo">The business object</param>
        ///// <returns>Returns true if contained</returns>
        //public bool Contains(T bo)
        //{
        //    return Contains(bo);
        //}

        /// <summary>
        /// Indicates whether any of the business objects have been amended 
        /// since they were last persisted
        /// </summary>
        public bool IsDirty
        {
            get
            {
                foreach (T child in this)
                {
                    if (child.State.IsDirty)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Indicates whether all of the business objects in the collection
        /// have valid values
        /// </summary>
        /// <returns>Returns true if all are valid</returns>
        public bool IsValid()
        {
            foreach (T child in this)
            {
                if (!child.IsValid())
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Indicates whether all of the business objects in the collection
        /// have valid values, amending an error message if any object is
        /// invalid
        /// </summary>
        /// <param name="errorMessage">An error message to amend</param>
        /// <returns>Returns true if all are valid</returns>
        public bool IsValid(out string errorMessage)
        {
            errorMessage = "";
            foreach (T child in this)
            {
                if (!child.IsValid(out errorMessage))
                {
                    return false;
                }
            }
            return true;
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
        public T Find(string key)
        {
            if (_lookupTable.ContainsKey(key))
            {
                T bo = (T)_lookupTable[key];
                if (this.Contains(bo))
                    return (T)_lookupTable[key];
                else return null;
            }
            else
            {
                return null;
            }
//			foreach (BusinessObjectBase bo in this._list) {
//				if (bo.StrID() == strID) {
//					return bo;
//				}
//			}
//			return null;
        }

        /// <summary>
        /// Finds a business object in the collection that contains the
        /// specified Guid ID
        /// </summary>
        /// <param name="searchTerm">The Guid to search for</param>
        /// <returns>Returns the business object if found, or null if not
        /// found</returns>
        public T FindByGuid(Guid searchTerm)
        {
//            string formattedSearchItem = searchTerm.ToString();
//            formattedSearchItem.Replace("{", "");
//            formattedSearchItem.Replace("}", "");
//            formattedSearchItem.Insert(0, _boClassDef.PrimaryKeyDef.KeyName + "=");

            string formattedSearchItem = string.Format("{0}={1}", _boClassDef.PrimaryKeyDef.KeyName, searchTerm);

            if (_lookupTable.ContainsKey(formattedSearchItem))
            {
                return (T)_lookupTable[formattedSearchItem];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the class definition of the collection
        /// </summary>
        public ClassDef ClassDef
        {
            get { return _boClassDef; }
        }

        /// <summary>
        /// Returns a sample business object held by the collection, which is
        /// constructed from the class definition
        /// </summary>
        public BusinessObject SampleBo
        {
            get { return _sampleBo; }
        }

        /// <summary>
        /// Returns an intersection of the set of objects held in this
        /// collection with the set in another specified collection (an
        /// intersection refers to a set of objects held in common between
        /// two sets)
        /// </summary>
        /// <param name="col2">Another collection to intersect with</param>
        /// <returns>Returns a new collection containing the intersection</returns>
        public BusinessObjectCollection<T> Intersection(BusinessObjectCollection<T> col2)
        {
            BusinessObjectCollection<T> intersectionCol = new BusinessObjectCollection<T>();
            foreach (T businessObjectBase in this)
            {
                if (col2.Contains(businessObjectBase))
                {
                    intersectionCol.Add(businessObjectBase);
                }
            }
            return intersectionCol;
        }

        /// <summary>
        /// Returns a union of the set of objects held in this
        /// collection with the set in another specified collection (a
        /// union refers to a set of all objects held in either of two sets)
        /// </summary>
        /// <param name="col2">Another collection to unite with</param>
        /// <returns>Returns a new collection containing the union</returns>
        public BusinessObjectCollection<T> Union(BusinessObjectCollection<T> col2)
        {
            BusinessObjectCollection<T> unionCol = new BusinessObjectCollection<T>();
            foreach (T businessObjectBase in this)
            {
                unionCol.Add(businessObjectBase);
            }
            foreach (T businessObjectBase in col2)
            {
                if (!unionCol.Contains(businessObjectBase))
                {
                    unionCol.Add(businessObjectBase);
                }
            }

            return unionCol;
        }

        /// <summary>
        /// Returns a new collection that is a copy of this collection
        /// </summary>
        /// <returns>Returns the cloned copy</returns>
        public BusinessObjectCollection<T> Clone()
        {
            BusinessObjectCollection<T> clonedCol = new BusinessObjectCollection<T>(_boClassDef);
            foreach (T businessObjectBase in this)
            {
                clonedCol.Add(businessObjectBase);
            }
            return clonedCol;
        }

		/// <summary>
		/// Returns a new collection that is a copy of this collection
		/// </summary>
		/// <returns>Returns the cloned copy</returns>
		public BusinessObjectCollection<DestType> Clone<DestType>()
			where DestType : BusinessObject
		{
			BusinessObjectCollection<DestType> clonedCol = new BusinessObjectCollection<DestType>(_boClassDef);
			if (!typeof(DestType).IsSubclassOf(typeof(T)) &&
				!typeof(T).IsSubclassOf(typeof(DestType)) &&
				!typeof(T).Equals(typeof(DestType)))
			{
				throw new InvalidCastException(String.Format("Cannot cast a collection of type '{0}' to " +
					  "a collection of type '{1}'.", typeof(T).Name, typeof(DestType).Name));
			}
			foreach (T businessObject in this)
			{
				DestType obj = businessObject as DestType;
				if (obj != null)
				{
					clonedCol.Add(obj);
				}
			}
			return clonedCol;
		}

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
        public void Sort(string propertyName, bool isBoProperty, bool isAscending)
        {
            if (isBoProperty)
            {
                Sort(ClassDef.GetPropDef(propertyName).GetPropertyComparer<T>());
            }
            else
            {
                Sort(new PropertyComparer<T>(propertyName));
            }

            if (!isAscending)
            {
                Reverse();
            }
        }

        /// <summary>
        /// Compares two properties.  Used by Sort().
        /// </summary>
        private class PropertyComparer<T> : IComparer<T>

        {
            private string _propertyName;
        	private PropertyInfo _propInfo;

            /// <summary>
            /// Constructor to instantiate a new comparer
            /// </summary>
            /// <param name="propertyName">The property name to compare on</param>
            public PropertyComparer(string propertyName)
            {
                _propertyName = propertyName;
                _propInfo =typeof(T).GetProperty(_propertyName, BindingFlags.Public | BindingFlags.Instance);
            }

            public int Compare(T x, T y)
            {
                object x1 = _propInfo.GetValue(x, new object[] {});
                object y1 = _propInfo.GetValue(y, new object[] {});

                if (x1 == null && y1 == null)
                {
                    return 0;
                }
                else if (x1 == null)
                {
                    return -1;
                }
                else if (y1 == null)
                {
                    return 1;
                }

            	IComparer comparer = Comparer<T>.Default;
				return comparer.Compare(x1, y1);

				//if (x1 is string)
				//{
				//    return String.Compare((string)x1, (string)y1);
				//}

				//if (x1 is int)
				//{
				//    if ((int)x1 < (int)y1) return -1;
				//    if ((int)x1 > (int)y1) return 1;
				//    return 0;
				//}

				//if (x1 is double)
				//{
				//    if (Math.Abs((double)x1 - (double)y1) < 0.00001) return 0;
				//    if ((double)x1 < (double)y1) return -1;
				//    if ((double)x1 > (double)y1) return 1;
				//}

				//if (x1 is DateTime)
				//{
				//    return ((DateTime)x1).CompareTo(y1);
				//}

                //return 0;
            }
        }

        /// <summary>
        /// Returns a list containing all the objects sorted by the property
        /// name and in the order specified
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <param name="isAscending">True for ascending, false for descending
        /// </param>
        /// <returns>Returns a sorted list</returns>
        public List<T> GetSortedList(string propertyName, bool isAscending)
        {
            List<T> list = new List<T>(this.Count);
            foreach (T o in this)
            {
                list.Add(o);
            }
            list.Sort(ClassDef.GetPropDef(propertyName).GetPropertyComparer<T>());
            if (!isAscending)
            {
                list.Reverse();
            }
            return list;
        }

        /// <summary>
        /// Returns a copied business object collection with the objects sorted by 
        /// the property name and in the order specified
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <param name="isAscending">True for ascending, false for descending
        /// </param>
        /// <returns>Returns a sorted business object collection</returns>
        public BusinessObjectCollection<T> GetSortedCollection(string propertyName, bool isAscending)
        {
            //test
            BusinessObjectCollection<T> sortedCol = new BusinessObjectCollection<T>();
            foreach (T bo in GetSortedList(propertyName, isAscending))
            {
                sortedCol.Add(bo);
            }
            return sortedCol;
        }

        /// <summary>
        /// Returns the business object collection as a List
        /// </summary>
        /// <returns>Returns an IList object</returns>
        public List<T> GetList()
        {
            List<T> list = new List<T>(this.Count);
            foreach (T o in this)
            {
                list.Add(o);
            }
            return list;
        }

        /// <summary>
        /// Commits to the database all the business objects that are either
        /// new or have been altered since the last committal
        /// </summary>
        public void SaveAll()
        {
            Transaction t = new Transaction(DatabaseConnection.CurrentConnection);
            foreach (T bo in this)
            {
                if (bo.State.IsDirty || bo.State.IsNew)
                {
                    t.AddTransactionObject(bo);
                }
            }
            t.CommitTransaction();
		}

		#region IBusinessObjectCollection Members
		BusinessObject IBusinessObjectCollection.Find(string key)
		{
			return this.Find(key);
		}

    	/// <summary>
    	/// Returns a new collection that is a copy of this collection
    	/// </summary>
    	/// <returns>Returns the cloned copy</returns>
    	IBusinessObjectCollection IBusinessObjectCollection.Clone()
    	{
    		return this.Clone();
    	}

    	#endregion


		#region IBusinessObjectCollection Members

		///<summary>
    	///Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"></see>.
    	///</summary>
    	///
    	///<returns>
    	///The index of item if found in the list; otherwise, -1.
    	///</returns>
    	///
    	///<param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
		int IBusinessObjectCollection.IndexOf(BusinessObject item)
    	{
			return this.IndexOf((T) item);
    	}

    	///<summary>
    	///Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"></see> at the specified index.
    	///</summary>
    	///
    	///<param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
    	///<param name="index">The zero-based index at which item should be inserted.</param>
    	///<exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
    	///<exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
		void IBusinessObjectCollection.Insert(int index, BusinessObject item)
    	{
    		this.Insert(index, (T)item);
    	}

    	///<summary>
    	///Gets or sets the element at the specified index.
    	///</summary>
    	///
    	///<returns>
    	///The element at the specified index.
    	///</returns>
    	///
    	///<param name="index">The zero-based index of the element to get or set.</param>
    	///<exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
    	///<exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
		BusinessObject IBusinessObjectCollection.this[int index]
    	{
    		get { return base[index]; }
    		set { base[index] = (T)value; }
    	}

		///<summary>
    	///Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
    	///</summary>
    	///
    	///<param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
    	///<exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		void IBusinessObjectCollection.Add(BusinessObject item)
    	{
    		this.Add((T) item);
    	}

    	///<summary>
    	///Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
    	///</summary>
    	///
    	///<returns>
    	///true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
    	///</returns>
    	///
    	///<param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		bool IBusinessObjectCollection.Contains(BusinessObject item)
    	{
    		return this.Contains((T) item);
    	}

    	///<summary>
    	///Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
    	///</summary>
    	///
    	///<param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
    	///<param name="arrayIndex">The zero-based index in array at which copying begins.</param>
    	///<exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
    	///<exception cref="T:System.ArgumentNullException">array is null.</exception>
    	///<exception cref="T:System.ArgumentException">array is multidimensional.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is greater than the available space from arrayIndex to the end of the destination array.-or-Type T cannot be cast automatically to the type of the destination array.</exception>
		void IBusinessObjectCollection.CopyTo(BusinessObject[] array, int arrayIndex)
    	{
    		T[] thisArray = new T[array.LongLength];
			this.CopyTo(thisArray, arrayIndex);
    		int count = Count;
			for (int index = 0; index < count; index++)
				array[arrayIndex + index] = thisArray[arrayIndex + index];
    	}

    	///<summary>
    	///Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
    	///</summary>
    	///
    	///<returns>
    	///true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
    	///</returns>
    	///
    	///<param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
    	///<exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		bool IBusinessObjectCollection.Remove(BusinessObject item)
    	{
    		return this.Remove((T) item);
    	}

    	///<summary>
    	///Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
    	///</summary>
    	///
    	///<returns>
    	///true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.
    	///</returns>
    	///
		bool IBusinessObjectCollection.IsReadOnly
    	{
			get { return false; }
    	}

    	#endregion

		//#region IEnumerable<BusinessObject> Members

		/////<summary>
		/////Returns an enumerator that iterates through the collection.
		/////</summary>
		/////
		/////<returns>
		/////A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
		/////</returns>
		/////<filterpriority>1</filterpriority>
		//IEnumerator<BusinessObject> IBusinessObjectCollection.GetEnumerator()
		//{
		//    for (int i = 0; i < Count; i++)
		//    {
		//        yield return this[i];
		//    }
		//}

		//#endregion

		//public new IEnumerator<T> GetEnumerator()
		//{
		//    return base.GetEnumerator();
		//}

    }
}
