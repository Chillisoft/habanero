using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using System.Security.Permissions;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.CriteriaManager.v2;
using Chillisoft.Db.v2;
using Chillisoft.Generic.v2;

namespace Chillisoft.Bo.v2
{
    public delegate void BusinessObjectEventHandler(Object sender, BOEventArgs e);

    /// <summary>
    /// Manages a collection of business objects.  This class also serves
    /// as a base class from which most types of business object collections
    /// can be derived.<br/>
    /// To create a collection of business objects, inherit from this 
    /// class. The business objects contained in this collection must
    /// inherit from BusinessObjectBase.
    /// </summary>
    public class BusinessObjectCollection : ICollection
    {
        private ClassDef _boClassDef;
        private IExpression _criteriaExpression;
        private string _orderByClause;
        private BusinessObject _sampleBo;
        private string _extraSearchCriteriaLiteral = "";
        private int _limit = -1;
        private Hashtable _lookupTable;
        private ArrayList _list;

        /// <summary>
        /// Constructor to initialise a new collection with a
        /// class definition provided by an existing business object
        /// </summary>
        /// <param name="bo">The business object whose class definition
        /// is used to initialise the collection</param>
        public BusinessObjectCollection(BusinessObject bo) : this(bo.ClassDef)
        {
        }

        /// <summary>
        /// Constructor to initialise a new collection with a specified
        /// class definition
        /// </summary>
        /// <param name="lClassDef">The class definition</param>
        public BusinessObjectCollection(ClassDef lClassDef)
        {
            _list = new ArrayList();
            _boClassDef = lClassDef;
            _sampleBo = _boClassDef.CreateNewBusinessObject();
            _lookupTable = new Hashtable();
        }

        /// <summary>
        /// Handles the event of a business object being added
        /// </summary>
        public event BusinessObjectEventHandler BusinessObjectAdded;

        /// <summary>
        /// Handles the event of a business object being removed
        /// </summary>
        public event BusinessObjectEventHandler BusinessObjectRemoved;

        /// <summary>
        /// Calls the BusinessObjectAdded() handler
        /// </summary>
        /// <param name="bo">The business object added</param>
        public void FireBusinessObjectAdded(BusinessObject bo)
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
        public void FireBusinessObjectRemoved(BusinessObject bo)
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
        public void Add(BusinessObject bo)
        {
            _list.Add(bo);
            _lookupTable.Add(bo.StrID(), bo);
            bo.Deleted += new BusinessObjectUpdatedHandler(BusinessObjectDeletedHandler);
            this.FireBusinessObjectAdded(bo);
        }

        /// <summary>
        /// Handles the event of a business object being deleted
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectDeletedHandler(object sender, BOEventArgs e)
        {
            this.RemoveAt(e.BusinessObject);
        }

        /// <summary>
        /// Copies the business objects in one collection across to this one
        /// </summary>
        /// <param name="col">The collection to copy from</param>
        public void Add(BusinessObjectCollection col)
        {
            foreach (BusinessObject bo in col)
            {
                this.Add(bo);
            }
        }

        ///// <summary>
        ///// Adds the businessobjects from col into this collecction
        ///// </summary>
        ///// <param name="col"></param>
        //public void Add(Collection<BusinessObjectCollection> col)
        //{
        //    foreach (BusinessObjectCollection collection in col)
        //    {
        //        this.Add(collection);
        //    }
        //}

        /// <summary>
        /// Provides a numerical indexing facility so that the objects
        /// in the collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position</param>
        /// <returns>Returns the business object at the position specified</returns>
        public BusinessObject this[int index]
        {
            get { return (BusinessObject) _list[index]; }
        }

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
                SQLCriteriaCreator creator = new SQLCriteriaCreator(_criteriaExpression, _boClassDef);
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
                    BusinessObject lTempBusObj;
					lTempBusObj = _boClassDef.InstantiateBusinessObject();
					//lTempBusObj = (BusinessObjectBase)Activator.CreateInstance(_boClassDef.ClassType, true);
					while (dr.Read())
                    {
                        //Load Business OBject from the data reader
                        Add(lTempBusObj.GetBusinessObject(dr));
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
        /// TODO ERIC - review, what does a limit do?
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

        /// <summary>
        /// Returns the business object at the index position specified
        /// </summary>
        /// <param name="index">The index position</param>
        /// <returns>Returns the business object at that position</returns>
        public BusinessObject item(int index)
        {
            return (BusinessObject) _list[index];
        }

        /// <summary>
        /// Clears the collection
        /// </summary>
        public void Clear()
        {
            _list.Clear();
            _lookupTable.Clear();
        }

        /// <summary>
        /// Removes the business object at the index position specified
        /// </summary>
        /// <param name="index">The index position to remove from</param>
        public void RemoveAt(int index)
        {
            BusinessObject boToRemove = this[index];
            _lookupTable.Remove(boToRemove.StrID());
            _list.RemoveAt(index);
            this.FireBusinessObjectRemoved(boToRemove);
        }

        /// <summary>
        /// Removes the specified business object from the collection
        /// </summary>
        /// <param name="bo">The business object to remove</param>
        public void RemoveAt(BusinessObject bo)
        {
            _list.Remove(bo);
            _lookupTable.Remove(bo.StrID());
            this.FireBusinessObjectRemoved(bo);
        }

        /// <summary>
        /// Indicates whether the collection contains the specified 
        /// business object
        /// </summary>
        /// <param name="bo">The business object</param>
        /// <returns>Returns true if contained</returns>
        public bool Contains(BusinessObject bo)
        {
            return _list.Contains(bo);
        }

        /// <summary>
        /// Indicates whether any of the business objects have been amended 
        /// since they were last persisted
        /// </summary>
        public bool IsDirty
        {
            get
            {
                foreach (BusinessObject child in _list)
                {
                    if (child.IsDirty)
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
            foreach (BusinessObject child in _list)
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
            foreach (BusinessObject child in _list)
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
        public BusinessObject Find(string key)
        {
            if (_lookupTable.ContainsKey(key))
            {
                return (BusinessObject)_lookupTable[key];
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
        public BusinessObject FindByGuid(Guid searchTerm)
        {
//            string formattedSearchItem = searchTerm.ToString();
//            formattedSearchItem.Replace("{", "");
//            formattedSearchItem.Replace("}", "");
//            formattedSearchItem.Insert(0, _boClassDef.PrimaryKeyDef.KeyName + "=");

            string formattedSearchItem = _boClassDef.PrimaryKeyDef.KeyName + "=" + searchTerm;

            if (_lookupTable.ContainsKey(formattedSearchItem))
            {
                return (BusinessObject)_lookupTable[formattedSearchItem];
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
        public BusinessObjectCollection Intersection(BusinessObjectCollection col2)
        {
            BusinessObjectCollection intersectionCol = new BusinessObjectCollection(this.ClassDef);
            foreach (BusinessObject businessObjectBase in this)
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
        public BusinessObjectCollection Union(BusinessObjectCollection col2)
        {
            BusinessObjectCollection unionCol = new BusinessObjectCollection(this.ClassDef);
            foreach (BusinessObject businessObjectBase in this)
            {
                unionCol.Add(businessObjectBase);
            }
            foreach (BusinessObject businessObjectBase in col2)
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
        public BusinessObjectCollection Clone()
        {
            BusinessObjectCollection clonedCol = new BusinessObjectCollection(this.ClassDef);
            foreach (BusinessObject businessObjectBase in this)
            {
                clonedCol.Add(businessObjectBase);
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
        /// TODO ERIC - ascending/descending?
        public void Sort(string propertyName, bool isBoProperty)
        {
            if (isBoProperty)
            {
                _list.Sort(ClassDef.GetPropDef(propertyName).GetPropertyComparer());
            }
            else
            {
                _list.Sort(new PropertyComparer(propertyName));
            }
        }

        /// <summary>
        /// Compares two properties.  Used by Sort().
        /// </summary>
        private class PropertyComparer : IComparer
        {
            private string _propertyName;

            /// <summary>
            /// Constructor to instantiate a new comparer
            /// </summary>
            /// <param name="propertyName">The property name to compare on</param>
            public PropertyComparer(string propertyName)
            {
                _propertyName = propertyName;
            }

            /// <summary>
            /// Compares two objects
            /// </summary>
            /// <param name="bo1">The first object to compare</param>
            /// <param name="bo2">The second object to compare</param>
            /// <returns>Returns less than zero if bo1 less than bo2, zero if equal
            /// and above zero if bo1 is greater than bo2</returns>
            public int Compare(object bo1, object bo2)
            {
                PropertyInfo propInfo =
                    bo1.GetType().GetProperty(_propertyName, BindingFlags.Public | BindingFlags.Instance);
                object x = propInfo.GetValue(bo1, new object[] {});
                object y = propInfo.GetValue(bo2, new object[] {});

                if (x == null && y == null)
                {
                    return 0;
                }
                else if (x == null)
                {
                    return -1;
                }
                else if (y == null)
                {
                    return 1;
                }

                if (x is string)
                {
                    return String.Compare((string) x, (string) y);
                }

                if (x is int)
                {
                    if ((int) x < (int) y) return -1;
                    if ((int) x > (int) y) return 1;
                    return 0;
                }

                if (x is double)
                {
                    if (Math.Abs((double) x - (double) y) < 0.00001) return 0;
                    if ((double) x < (double) y) return -1;
                    if ((double) x > (double) y) return 1;
                }

                if (x is DateTime)
                {
                    return ((DateTime) x).CompareTo(y);
                }

                return 0;
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
        public IList GetSortedList(string propertyName, bool isAscending)
        {
            ArrayList list = new ArrayList(this.Count);
            foreach (object o in this)
            {
                list.Add(o);
            }
            list.Sort(ClassDef.GetPropDef(propertyName).GetPropertyComparer());
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
        public BusinessObjectCollection GetSortedCollection(string propertyName, bool isAscending)
        {
            //test
            BusinessObjectCollection sortedCol = new BusinessObjectCollection(this.ClassDef);
            foreach (BusinessObject bo in GetSortedList(propertyName, isAscending))
            {
                sortedCol.Add(bo);
            }
            return sortedCol;
        }

        /// <summary>
        /// Returns the business object collection as an IList object
        /// </summary>
        /// <returns>Returns an IList object</returns>
        /// TODO ERIC - doesn't this duplicate GetList()?
        public IList ToList()
        {
            IList list = new ArrayList(this.Count);
            foreach (object o in this)
            {
                list.Add(o);
            }
            return list;
        }

        /// <summary>
        /// Commits to the database all the business objects that are either
        /// new or have been altered since the last committal
        /// </summary>
        public void ApplyEdit()
        {
            Transaction t = new Transaction(DatabaseConnection.CurrentConnection);
            foreach (BusinessObject bo in this._list)
            {
                if (bo.IsDirty || bo.IsNew)
                {
                    t.AddTransactionObject(bo);
                }
            }
            t.CommitTransaction();
        }

        /// <summary>
        /// Not yet implemented
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// TODO ERIC - implement
        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the count of objects in the collection
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// Returns the collection object
        /// </summary>
        public object SyncRoot
        {
            get { return this; }
        }

        /// <summary>
        /// Returns false
        /// </summary>
        public bool IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// Returns this collection's enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        /// <summary>
        /// Returns the list of objects in the collection
        /// </summary>
        /// <returns>Returns an IList object</returns>
        public IList GetList()
        {
            return _list;
        }
    }
}
