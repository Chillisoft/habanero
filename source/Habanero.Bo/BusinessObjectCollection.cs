//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using System.Security.Permissions;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Comparer;
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
	public class BusinessObjectCollection<TBusinessObject> : List<TBusinessObject>, IEnumerable<TBusinessObject>, IBusinessObjectCollection 
		where TBusinessObject : BusinessObject
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
        /// Default constructor. 
        /// The classdef will be implied from TBusinessObject and the Current Database Connection will be used.
        /// </summary>
        public BusinessObjectCollection()
            : this(null, null)
        {
        }

        /// <summary>
        /// Use this constructor if you will only know TBusinessObject at run time - BusinessObject will be the generic type
        /// and the objects in the collection will be determined from the classDef passed in.
        /// </summary>
        /// <param name="classDef">The classdef of the objects to be contained in this collection</param>
        public BusinessObjectCollection(ClassDef classDef) 
            :this (classDef, null)
        {
        }

        /// <summary>
        /// Constructor to initialise a new collection with a
        /// class definition provided by an existing business object
        /// </summary>
        /// <param name="bo">The business object whose class definition
        /// is used to initialise the collection</param>
        public BusinessObjectCollection(TBusinessObject bo)
            : this(null, bo)
        {
        }

        /// <summary>
        /// Constructor to initialise a new collection with a specified Database Connection.
        /// This Database Connection will be used to load the collection.
        /// </summary>
        /// <param name="databaseConnection">The Database Connection to used to load the collection</param>
        public BusinessObjectCollection(IDatabaseConnection databaseConnection)
            : this(null, null)
        {
            _sampleBo.SetDatabaseConnection(databaseConnection);
        }

        private BusinessObjectCollection(ClassDef classDef, TBusinessObject sampleBo)
        {
            if (classDef == null)
            {
                if (sampleBo == null)
                {
                    _boClassDef = ClassDef.ClassDefs[typeof (TBusinessObject)];
                } else
                {
                    _boClassDef = sampleBo.ClassDef;
                }
            } else
            {
                _boClassDef = classDef;
            }
            if (sampleBo != null)
            {
                _sampleBo = sampleBo;
            } else
            {
                if (_boClassDef == null)
                {
                    throw new UserException(String.Format("A business object collection is " +
                        "being created for the type '{0}', but no class definitions have " +
                        "been loaded for this type.", typeof(TBusinessObject).Name));
                }
                _sampleBo = _boClassDef.CreateNewBusinessObject();
            }
            _lookupTable = new Hashtable();
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
        public void FireBusinessObjectAdded(TBusinessObject bo)
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
        public void FireBusinessObjectRemoved(TBusinessObject bo)
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
        public new void Add(TBusinessObject bo)
        {
            base.Add(bo);
            _lookupTable.Add(bo.ID.ToString(), bo);
            bo.Deleted += BusinessObjectDeletedHandler;
            this.FireBusinessObjectAdded(bo);
        }

        /// <summary>
        /// Handles the event of a business object being deleted
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectDeletedHandler(object sender, BOEventArgs e)
        {
            this.Remove((TBusinessObject)e.BusinessObject);
        }

        /// <summary>
        /// Copies the business objects in one collection across to this one
        /// </summary>
        /// <param name="col">The collection to copy from</param>
        public void Add(BusinessObjectCollection<TBusinessObject> col)
        {
			Add((List<TBusinessObject>) col);
			//foreach (TBusinessObject bo in col)
			//{
			//    this.Add(bo);
			//}
        }

        /// <summary>
        /// Adds the businessobjects from col into this collecction
        /// </summary>
        /// <param name="col"></param>
        public void Add(List<TBusinessObject> col)
        {
            foreach (TBusinessObject bo in col)
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
            IDatabaseConnection boDatabaseConnection = _sampleBo.GetDatabaseConnection();
            //Create the SqlStatement for the _sampleBo's database connection
        	ISqlStatement refreshSql = CreateLoadSqlStatement(_sampleBo, _boClassDef, 
                _criteriaExpression, _limit, _extraSearchCriteriaLiteral);
        	using (IDataReader dr = boDatabaseConnection.LoadDataReader(refreshSql, _orderByClause))
            {
                try
                {
                    TBusinessObject lTempBusObj;
                    lTempBusObj = (TBusinessObject)_boClassDef.CreateNewBusinessObject(boDatabaseConnection); 
                    //lTempBusObj = (TBusinessObject)_boClassDef.InstantiateBusinessObject();
					//lTempBusObj = (BusinessObjectBase)Activator.CreateInstance(_boClassDef.ClassType, true);
					while (dr.Read())
                    {
                        //Load Business OBject from the data reader
                        Add((TBusinessObject)BOLoader.Instance.GetBusinessObject(lTempBusObj, dr));
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

    	internal static ISqlStatement CreateLoadSqlStatement(BusinessObject businessObject, ClassDef classDef, IExpression criteriaExpression, int limit, string extraSearchCriteriaLiteral)
    	{
    	    IDatabaseConnection boDatabaseConnection = businessObject.GetDatabaseConnection();
    		ISqlStatement refreshSql = new SqlStatement(boDatabaseConnection);
			refreshSql.Statement.Append(businessObject.GetSelectSql(limit));
			if (criteriaExpression != null)
    		{
    			refreshSql.AppendWhere();
				SqlCriteriaCreator creator = new SqlCriteriaCreator(criteriaExpression, classDef, boDatabaseConnection);
    			creator.AppendCriteriaToStatement(refreshSql);
    		}
			if (!String.IsNullOrEmpty(extraSearchCriteriaLiteral))
    		{
    			refreshSql.AppendWhere();
				refreshSql.Statement.Append(extraSearchCriteriaLiteral);
    		}

            if (limit > 0)
            {
                string limitClause = boDatabaseConnection.GetLimitClauseForEnd(limit);
                if (!String.IsNullOrEmpty(limitClause)) refreshSql.Statement.Append(" " + limitClause);
            }
    		return refreshSql;
    	}

    	#region Load Methods

		/// <summary>
        /// Loads the entire collection for the type of object.
        /// </summary>
        public void LoadAll()
        {
			LoadAll("");
        }

		/// <summary>
		/// Loads the entire collection for the type of object,
		/// loaded in the order specified. 
		/// To load the collection in any order use the LoadAll() method.
		/// </summary>
		/// <param name="orderByClause">The order-by clause</param>
		public void LoadAll(string orderByClause)
		{
			Load("", orderByClause);
		}

        /// <summary>
        /// Loads business objects that match the search criteria provided,
        /// loaded in the order specified.  
        /// Use empty quotes, (or the LoadAll method) to load the
        /// entire collection for the type of object.
        /// </summary>
        /// <param name="searchCriteria">The search criteria</param>
        /// <param name="orderByClause">The order-by clause</param>
        public void Load(string searchCriteria, string orderByClause)
        {
            Load(searchCriteria, orderByClause, "");
        }
		
		/// <summary>
		/// Loads business objects that match the search criteria provided in
		/// an expression, loaded in the order specified
		/// </summary>
		/// <param name="searchExpression">The search expression</param>
		/// <param name="orderByClause">The order-by clause</param>
		public void Load(IExpression searchExpression, string orderByClause)
		{
			Load(searchExpression, orderByClause, "");
		}

        /// <summary>
        /// Loads business objects that match the search criteria provided
		/// and an extra criteria literal,
        /// loaded in the order specified
        /// </summary>
        /// <param name="searchCriteria">The search criteria</param>
        /// <param name="orderByClause">The order-by clause</param>
        /// <param name="extraSearchCriteriaLiteral">Extra search criteria</param>
        /// TODO ERIC - what is the last one?
        public void Load(string searchCriteria, string orderByClause, string extraSearchCriteriaLiteral)
        {
            LoadWithLimit(searchCriteria, orderByClause, extraSearchCriteriaLiteral, -1);
        }

        /// <summary>
        /// Loads business objects that match the search criteria provided in
		/// an expression and an extra criteria literal, 
		/// loaded in the order specified
        /// </summary>
		/// <param name="searchExpression">The search expression</param>
        /// <param name="orderByClause">The order-by clause</param>
        /// <param name="extraSearchCriteriaLiteral">Extra search criteria</param>
        /// TODO ERIC - what is the last one?
		public void Load(IExpression searchExpression, string orderByClause, string extraSearchCriteriaLiteral)
        {
			LoadWithLimit(searchExpression, orderByClause, extraSearchCriteriaLiteral, -1);
        }

		/// <summary>
		/// Loads business objects that match the search criteria provided, 
		/// loaded in the order specified, 
		/// and limiting the number of objects loaded
		/// </summary>
		/// <param name="searchCriteria">The search criteria</param>
		/// <param name="orderByClause">The order-by clause</param>
		/// <param name="limit">The limit</param>
		public void LoadWithLimit(string searchCriteria, string orderByClause, int limit)
		{
			LoadWithLimit(searchCriteria, orderByClause, "", limit);
		}

		/// <summary>
		/// Loads business objects that match the search criteria provided in
		/// an expression, loaded in the order specified, 
		/// and limiting the number of objects loaded
		/// </summary>
		/// <param name="searchExpression">The search expression</param>
		/// <param name="orderByClause">The order-by clause</param>
		/// <param name="limit">The limit</param>
		public void LoadWithLimit(IExpression searchExpression, string orderByClause, int limit)
		{
			LoadWithLimit(searchExpression, orderByClause, "", limit);
		}

		/// <summary>
		/// Loads business objects that match the search criteria provided
		/// and an extra criteria literal, 
		/// loaded in the order specified, 
		/// and limiting the number of objects loaded
		/// </summary>
		/// <param name="searchCriteria">The search expression</param>
		/// <param name="orderByClause">The order-by clause</param>
		/// <param name="extraSearchCriteriaLiteral">Extra search criteria</param>
		/// <param name="limit">The limit</param>
		public void LoadWithLimit(string searchCriteria, string orderByClause, string extraSearchCriteriaLiteral, int limit)
		{
			IExpression criteriaExpression = null;
			if (searchCriteria.Length > 0)
			{
				criteriaExpression = Expression.CreateExpression(searchCriteria);
			}
			LoadWithLimit(criteriaExpression, orderByClause, extraSearchCriteriaLiteral, limit);
		}

		/// <summary>
		/// Loads business objects that match the search criteria provided in
		/// an expression and an extra criteria literal, 
		/// loaded in the order specified, 
		/// and limiting the number of objects loaded
		/// </summary>
		/// <param name="searchExpression">The search expression</param>
		/// <param name="orderByClause">The order-by clause</param>
		/// <param name="extraSearchCriteriaLiteral">Extra search criteria</param>
		/// <param name="limit">The limit</param>
		public void LoadWithLimit(IExpression searchExpression, string orderByClause, string extraSearchCriteriaLiteral, int limit)
		{
			_criteriaExpression = searchExpression;
			_orderByClause = orderByClause;
			_extraSearchCriteriaLiteral = extraSearchCriteriaLiteral;
			_limit = limit;
			Refresh();
		}

		#endregion

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
            TBusinessObject boToRemove = this[index];
            base.RemoveAt(index);
			_lookupTable.Remove(boToRemove.ID.ToString());
			boToRemove.Deleted -= BusinessObjectDeletedHandler;
            this.FireBusinessObjectRemoved(boToRemove);
        }

        /// <summary>
        /// Removes the specified business object from the collection
        /// </summary>
        /// <param name="bo">The business object to remove</param>
        public new bool Remove(TBusinessObject bo)
        {
			bool removed = base.Remove(bo);
            _lookupTable.Remove(bo.ID.ToString());
        	bo.Deleted -= BusinessObjectDeletedHandler;
            this.FireBusinessObjectRemoved(bo);
        	return removed;
        }

        ///// <summary>
        ///// Indicates whether the collection contains the specified 
        ///// business object
        ///// </summary>
        ///// <param name="bo">The business object</param>
        ///// <returns>Returns true if contained</returns>
        //public bool Contains(TBusinessObject bo)
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
                foreach (TBusinessObject child in this)
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
            foreach (TBusinessObject child in this)
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
            foreach (TBusinessObject child in this)
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
        public TBusinessObject Find(string key)
        {
            if (_lookupTable.ContainsKey(key))
            {
                TBusinessObject bo = (TBusinessObject)_lookupTable[key];
                if (this.Contains(bo))
                    return (TBusinessObject)_lookupTable[key];
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
        public TBusinessObject FindByGuid(Guid searchTerm)
        {
//            string formattedSearchItem = searchTerm.ToString();
//            formattedSearchItem.Replace("{", "");
//            formattedSearchItem.Replace("}", "");
//            formattedSearchItem.Insert(0, _boClassDef.PrimaryKeyDef.KeyName + "=");

            string formattedSearchItem = string.Format("{0}={1}", _boClassDef.PrimaryKeyDef.KeyName, searchTerm);

            if (_lookupTable.ContainsKey(formattedSearchItem))
            {
                return (TBusinessObject)_lookupTable[formattedSearchItem];
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
        public BusinessObjectCollection<TBusinessObject> Intersection(BusinessObjectCollection<TBusinessObject> col2)
        {
            BusinessObjectCollection<TBusinessObject> intersectionCol = new BusinessObjectCollection<TBusinessObject>();
            foreach (TBusinessObject businessObjectBase in this)
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
        public BusinessObjectCollection<TBusinessObject> Union(BusinessObjectCollection<TBusinessObject> col2)
        {
            BusinessObjectCollection<TBusinessObject> unionCol = new BusinessObjectCollection<TBusinessObject>();
            foreach (TBusinessObject businessObjectBase in this)
            {
                unionCol.Add(businessObjectBase);
            }
            foreach (TBusinessObject businessObjectBase in col2)
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
        public BusinessObjectCollection<TBusinessObject> Clone()
        {
            BusinessObjectCollection<TBusinessObject> clonedCol = new BusinessObjectCollection<TBusinessObject>(_boClassDef);
            foreach (TBusinessObject businessObjectBase in this)
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
			if (!typeof(DestType).IsSubclassOf(typeof(TBusinessObject)) &&
				!typeof(TBusinessObject).IsSubclassOf(typeof(DestType)) &&
				!typeof(TBusinessObject).Equals(typeof(DestType)))
			{
				throw new InvalidCastException(String.Format("Cannot cast a collection of type '{0}' to " +
					  "a collection of type '{1}'.", typeof(TBusinessObject).Name, typeof(DestType).Name));
			}
			foreach (TBusinessObject businessObject in this)
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
                Sort(ClassDef.GetPropDef(propertyName).GetPropertyComparer<TBusinessObject>());
            }
            else
            {
                Sort(new ReflectedPropertyComparer<TBusinessObject>(propertyName));
            }

            if (!isAscending)
            {
                Reverse();
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
        public List<TBusinessObject> GetSortedList(string propertyName, bool isAscending)
        {
            List<TBusinessObject> list = new List<TBusinessObject>(this.Count);
            foreach (TBusinessObject o in this)
            {
                list.Add(o);
            }
            list.Sort(ClassDef.GetPropDef(propertyName).GetPropertyComparer<TBusinessObject>());
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
        public BusinessObjectCollection<TBusinessObject> GetSortedCollection(string propertyName, bool isAscending)
        {
            //test
            BusinessObjectCollection<TBusinessObject> sortedCol = new BusinessObjectCollection<TBusinessObject>();
            foreach (TBusinessObject bo in GetSortedList(propertyName, isAscending))
            {
                sortedCol.Add(bo);
            }
            return sortedCol;
        }

        /// <summary>
        /// Returns the business object collection as a List
        /// </summary>
        /// <returns>Returns an IList object</returns>
        public List<TBusinessObject> GetList()
        {
            List<TBusinessObject> list = new List<TBusinessObject>(this.Count);
            foreach (TBusinessObject o in this)
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
            foreach (TBusinessObject bo in this)
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
			return this.IndexOf((TBusinessObject) item);
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
    		this.Insert(index, (TBusinessObject)item);
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
    		set { base[index] = (TBusinessObject)value; }
    	}

		///<summary>
    	///Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
    	///</summary>
    	///
    	///<param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
    	///<exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		void IBusinessObjectCollection.Add(BusinessObject item)
    	{
    		this.Add((TBusinessObject) item);
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
    		return this.Contains((TBusinessObject) item);
    	}

    	///<summary>
    	///Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
    	///</summary>
    	///
    	///<param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
    	///<param name="arrayIndex">The zero-based index in array at which copying begins.</param>
    	///<exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
    	///<exception cref="T:System.ArgumentNullException">array is null.</exception>
    	///<exception cref="T:System.ArgumentException">array is multidimensional.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is greater than the available space from arrayIndex to the end of the destination array.-or-Type TBusinessObject cannot be cast automatically to the type of the destination array.</exception>
		void IBusinessObjectCollection.CopyTo(BusinessObject[] array, int arrayIndex)
    	{
    		TBusinessObject[] thisArray = new TBusinessObject[array.LongLength];
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
    		return this.Remove((TBusinessObject) item);
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

		//public new IEnumerator<TBusinessObject> GetEnumerator()
		//{
		//    return base.GetEnumerator();
		//}

    }
}
