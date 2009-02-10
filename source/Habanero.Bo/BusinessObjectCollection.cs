//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using System.Runtime.Serialization;
using System.Security.Permissions;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.Comparer;

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
    /// 
    ///   The business object co\llection differentiates between
    ///   - business objects deleted from it since it was last synchronised with the datastore.
    ///   - business objects added to it since it was last synchronised.
    ///   - business objects created by it since it was last synchronised.
    ///   - business objects removed from it since it was last synchronised.
    /// The Business Object collection maintains this list so as to be
    ///   able to store the state of this collection when it was last loaded or persisted
    ///   to the relevant datastore. This is necessary so that the collection can be
    ///   restored (in the case where a user selects to can edits to a collection.
    /// </summary>
    [Serializable]
    public class BusinessObjectCollection<TBusinessObject>
        : List<TBusinessObject>, IBusinessObjectCollection, ISerializable
        where TBusinessObject : class, IBusinessObject, new()
    {
        private const string COUNT = "Count";
        private const string CLASS_NAME = "ClassName";
        private const string ASSEMBLY_NAME = "AssemblyName";
        private const string CREATED_COUNT = "CreatedCount";
        private const string BUSINESS_OBJECT = "bo";
        private const string CREATED_BUSINESS_OBJECT = "createdbo";

        #region StronglyTypedComparer

        private class StronglyTypedComperer<T> : IComparer<T>
        {
            private readonly IComparer _comparer;

            public StronglyTypedComperer(IComparer comparer)
            {
                if (comparer == null) throw new ArgumentNullException("comparer");
                _comparer = comparer;
            }

            public int Compare(T x, T y)
            {
                return _comparer.Compare(x, y);
            }
        }

        #endregion//StronglyTypedComparer

        private IClassDef _boClassDef;
        private readonly List<TBusinessObject> _createdBusinessObjects = new List<TBusinessObject>();
        private readonly List<TBusinessObject> _persistedObjectsCollection = new List<TBusinessObject>();
        protected readonly List<TBusinessObject> _removedBusinessObjects = new List<TBusinessObject>();
        private readonly List<TBusinessObject> _addedBusinessObjects = new List<TBusinessObject>();
        private readonly List<TBusinessObject> _markedForDeleteBusinessObjects = new List<TBusinessObject>();

        private ISelectQuery _selectQuery;
        private readonly EventHandler<BOEventArgs> _savedEventHandler;
        private readonly EventHandler<BOEventArgs> _deletedEventHandler;
        private readonly EventHandler<BOEventArgs> _restoredEventHandler;
        private readonly EventHandler<BOEventArgs> _updateIDEventHandler;
        private readonly EventHandler<BOEventArgs> _markForDeleteEventHandler;
        private readonly EventHandler<BOEventArgs> _updatedEventHandler;
        private readonly EventHandler<BOPropUpdatedEventArgs> _boPropUpdatedEventHandler;
        private readonly EventHandler<BOEventArgs> _boIDUpdatedEventHandler;

        /// <summary>
        /// Default constructor. 
        /// The classdef will be implied from TBusinessObject and the Current Database Connection will be used.
        /// </summary>
        public BusinessObjectCollection() : this(null, null)
        {
        }

        /// <summary>
        /// Use this constructor if you will only know TBusinessObject at run time - BusinessObject will be the generic type
        /// and the objects in the collection will be determined from the classDef passed in.
        /// </summary>
        /// <param name="classDef">The classdef of the objects to be contained in this collection</param>
        public BusinessObjectCollection(IClassDef classDef) : this(classDef, null)
        {
        }

        /// <summary>
        /// Constructor to initialise a new collection with a
        /// class definition provided by an existing business object
        /// </summary>
        /// <param name="bo">The business object whose class definition
        /// is used to initialise the collection</param>
        public BusinessObjectCollection(TBusinessObject bo) : this(null, bo)
        {
        }


        private BusinessObjectCollection(IClassDef classDef, TBusinessObject sampleBo)
        {
            Initialise(classDef, sampleBo);
            this._savedEventHandler = SavedEventHandler;
            this._deletedEventHandler = DeletedEventHandler;
            this._restoredEventHandler = RestoredEventHandler;
            this._updateIDEventHandler = UpdateHashTable;
            this._markForDeleteEventHandler = MarkForDeleteEventHandler;
            this._updatedEventHandler = UpdatedEventHandler;
            this._boPropUpdatedEventHandler = BOPropUpdatedEventHandler;
            this._boIDUpdatedEventHandler = BOIDUpdatedEventHandler;
        }

        private void Initialise(IClassDef classDef, TBusinessObject sampleBo)
        {
            _boClassDef = classDef
                          ??
                          (sampleBo == null
                               ? ClassDefinition.ClassDef.ClassDefs[typeof (TBusinessObject)]
                               : sampleBo.ClassDef);
            KeyObjectHashTable = new Hashtable();
            _selectQuery = QueryBuilder.CreateSelectQuery(_boClassDef);
        }

        protected BusinessObjectCollection(SerializationInfo info, StreamingContext context)
        {
            int count = info.GetInt32(COUNT);
            int created_count = info.GetInt32(CREATED_COUNT);
            Type classType = Util.TypeLoader.LoadType(info.GetString(ASSEMBLY_NAME), info.GetString(CLASS_NAME));
            this.Initialise(ClassDefinition.ClassDef.ClassDefs[classType], null);
            for (int i = 0; i < count; i++)
            {
                this.AddWithoutEvents((TBusinessObject) info.GetValue(BUSINESS_OBJECT + i, typeof (TBusinessObject)));
            }
            for (int i = 0; i < created_count; i++)
            {
                this.AddCreatedBusinessObject
                    ((TBusinessObject) info.GetValue(CREATED_BUSINESS_OBJECT + i, typeof (TBusinessObject)));
            }
            _updateIDEventHandler = UpdateHashTable;
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
        /// Handles the event of any business object in this collection being Updated(i.e the BO is saved, or edits are cancelled).
        /// See the <see cref="IBusinessObject"/>.<see cref="IBusinessObject.Updated"/> event.
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectUpdated;

        /// <summary>
        /// Handles the event of any business object in this collection being edited (i.e. a property value is changed).
        /// See the <see cref="IBusinessObject"/>.<see cref="IBusinessObject.PropertyUpdated"/> event.
        /// </summary>
        public event EventHandler<BOPropUpdatedEventArgs> BusinessObjectPropertyUpdated;

        /// <summary>
        /// Handles the event when a BusinessObject in the collection has an ID that is Updated(i.e one of the properties of the ID is edited).
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectIDUpdated;

        ///// <summary>
        ///// Handles the event of any business object in this collection being edited
        ///// </summary>
        //public event EventHandler<BOEventArgs> BusinessObjectEdited;

        /// <summary>
        /// Calls the BusinessObjectAdded() handler
        /// </summary>
        /// <param name="bo">The business object added</param>
        private void FireBusinessObjectAdded(TBusinessObject bo)
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
        private void FireBusinessObjectRemoved(TBusinessObject bo)
        {
            if (this.BusinessObjectRemoved != null)
            {
                this.BusinessObjectRemoved(this, new BOEventArgs(bo));
            }
        }

        private void FireBusinessObjectUpdated(TBusinessObject bo)
        {
            if (this.BusinessObjectUpdated != null)
            {
                this.BusinessObjectUpdated(this, new BOEventArgs(bo));
            }
        }

        private void FireBusinessObjectPropertyUpdated(TBusinessObject bo, IBOProp boProp)
        {
            if (this.BusinessObjectPropertyUpdated != null)
            {
                this.BusinessObjectPropertyUpdated(this, new BOPropUpdatedEventArgs(bo, boProp));
            }
        }

        private void FireBusinessObjectIDUpdated(TBusinessObject bo)
        {
            if (this.BusinessObjectIDUpdated != null)
            {
                this.BusinessObjectIDUpdated(this, new BOEventArgs(bo));
            }
        }

        ///// <summary>
        ///// Calls the BusinessObjectEdited() handler
        ///// </summary>
        ///// <param name="bo">The business object that was edited</param>
        //public void FireBusinessObjectEdited(TBusinessObject bo)
        //{
        //    if (this.BusinessObjectEdited != null)
        //    {
        //        this.BusinessObjectEdited(this, new BOEventArgs(bo));
        //    }
        //}

        /// <summary>
        /// Adds a business object to the collection
        /// </summary>
        /// <param name="bo">The business object to add</param>
        public new virtual void Add(TBusinessObject bo)
        {
            if (bo == null) throw new ArgumentNullException("bo");
            lock (KeyObjectHashTable)
            {
                if (this.Contains(bo)) return;
                if (bo.Status.IsNew && !this.CreatedBusinessObjects.Contains(bo))
                {
                    AddCreatedBusinessObject(bo);
                }
                else
                {
                    if (bo.Status.IsDeleted) return;
                    AddWithoutEvents(bo);
                    if (!AddedBusinessObjects.Contains(bo) && !PersistedBusinessObjects.Contains(bo))
                    {
                        AddedBusinessObjects.Add(bo);
                    }
                    this.RemovedBusinessObjects.Remove(bo);
                    this.FireBusinessObjectAdded(bo);
                }
            }
        }

        void IBusinessObjectCollection.AddWithoutEvents(IBusinessObject businessObject)
        {
            AddWithoutEvents(businessObject as TBusinessObject);
            //AddToPersistedCollection(businessObject);
        }

        /// <summary>
        /// Allows the adding of business objects to the collection without
        /// this causing the added event to be fired.
        /// This is intended to be used for internal use only.
        /// </summary>
        /// <param name="businessObject"></param>
        private void AddToPersistedCollection(TBusinessObject businessObject)
        {
            this.PersistedBusinessObjects.Add(businessObject);
        }

        private void AddWithoutEvents(TBusinessObject bo)
        {
            if (bo == null) throw new ArgumentNullException("bo");

            base.Add(bo);
            lock (KeyObjectHashTable)
            {
                if (bo.ID != null) if (KeyObjectHashTable.Contains(bo.ID.ObjectID)) return;
                if (bo.ID != null) KeyObjectHashTable.Add(bo.ID.ObjectID, bo);
            }
            RegisterBOEvents(bo);
        }

        private void RegisterBOEvents(TBusinessObject businessObject)
        {
            businessObject.Saved += _savedEventHandler;
            businessObject.Deleted += _deletedEventHandler;
            businessObject.Restored += _restoredEventHandler;
            //TODO: businessObject.Updated += _updatedEventHandler;
            if (businessObject.ID != null) businessObject.IDUpdated += _updateIDEventHandler;
            businessObject.MarkedForDeletion += _markForDeleteEventHandler;
            businessObject.Updated += _updatedEventHandler;
            businessObject.PropertyUpdated += _boPropUpdatedEventHandler;
            businessObject.IDUpdated += _boIDUpdatedEventHandler;
        }

        private void DeRegisterBOEvents(TBusinessObject businessObject)
        {
            businessObject.Saved -= _savedEventHandler;
            businessObject.Restored -= _restoredEventHandler;
            businessObject.Deleted -= _deletedEventHandler;
            //TODO: businessObject.Updated -= _updatedEventHandler;
            if (businessObject.ID != null) businessObject.IDUpdated -= _updateIDEventHandler;
            businessObject.MarkedForDeletion -= _markForDeleteEventHandler;
            businessObject.Updated -= _updatedEventHandler;
            businessObject.PropertyUpdated -= _boPropUpdatedEventHandler;
            businessObject.IDUpdated -= _boIDUpdatedEventHandler;
        }

        private void DeRegisterBoEventsForAllBusinessObjects()
        {
            foreach (TBusinessObject businessObject in this)
            {
                DeRegisterBOEvents(businessObject);
            }
            foreach (TBusinessObject businessObject in this.RemovedBusinessObjects)
            {
                DeRegisterBOEvents(businessObject);
            }
            foreach (TBusinessObject businessObject in this.MarkedForDeleteBusinessObjects)
            {
                DeRegisterBOEvents(businessObject);
            }
        }

        /// <summary>
        /// Updates the lookup table when a primary key property has
        /// changed
        /// </summary>
        private void UpdateHashTable(object sender, BOEventArgs e)
        {
            string oldID = e.BusinessObject.ID.AsString_PreviousValue();
            if (KeyObjectHashTable.Contains(oldID))
            {
                BusinessObject bo = (BusinessObject) KeyObjectHashTable[oldID];
                KeyObjectHashTable.Remove(oldID);
                KeyObjectHashTable.Add(bo.ID.AsString_CurrentValue(), bo);
            }
        }

        private void MarkForDeleteEventHandler(object sender, BOEventArgs e)
        {
            TBusinessObject bo = e.BusinessObject as TBusinessObject;
            if (bo == null) return;
            if (this.MarkedForDeleteBusinessObjects.Contains(bo)) return;

            this.MarkedForDeleteBusinessObjects.Add(bo);
            base.Remove(bo);
            KeyObjectHashTable.Remove(bo.ID.AsString_CurrentValue());
            if (!this.RemovedBusinessObjects.Remove(bo))
            {
                this.FireBusinessObjectRemoved(bo);
            }
        }

        /// <summary>
        /// Handles the event of a business object being deleted
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void DeletedEventHandler(object sender, BOEventArgs e)
        {
            TBusinessObject bo = e.BusinessObject as TBusinessObject;
            if (bo == null) return;
            this.RemoveInternal(bo);
            this.PersistedBusinessObjects.Remove(bo);
            this.RemovedBusinessObjects.Remove(bo);
            this.MarkedForDeleteBusinessObjects.Remove(bo);
            if (bo.Status.IsDeleted) this.AddedBusinessObjects.Remove(bo);
            DeRegisterBOEvents(bo);
        }


        protected virtual void RestoredEventHandler(object sender, BOEventArgs e)
        {
            TBusinessObject bo = (TBusinessObject) e.BusinessObject;
            if (!this.MarkedForDeleteBusinessObjects.Remove(bo) || (this.Contains(bo))) return;
            if (this.AddedBusinessObjects.Contains(bo) && this.Contains(bo))
            {
                this.AddWithoutEvents(bo);
            }
            else
            {
                this.Add(bo);
            }
        }

        protected virtual void SavedEventHandler(object sender, BOEventArgs e)
        {
            TBusinessObject bo = (TBusinessObject) e.BusinessObject;
            if (bo == null) return;
            CreatedBusinessObjects.Remove(bo);
            if (this.RemovedBusinessObjects.Remove(bo)) return;
            if (!this.Contains(bo))
            {
                if (this.SelectQuery.Criteria.IsMatch(bo))
                {
                    Add(bo);
                }
            }
            if (!bo.Status.IsNew && !bo.Status.IsDeleted && !this.PersistedBusinessObjects.Contains(bo))
            {
                AddToPersistedCollection(bo);
            }
        }

        private void UpdatedEventHandler(object sender, BOEventArgs e)
        {
            TBusinessObject businessObject = (TBusinessObject) e.BusinessObject;
            if (!this.Contains(businessObject)) return;
            FireBusinessObjectUpdated(businessObject);
        }

        private void BOPropUpdatedEventHandler(object sender, BOPropUpdatedEventArgs propEventArgs)
        {
            TBusinessObject businessObject = (TBusinessObject) propEventArgs.BusinessObject;
            if (!this.Contains(businessObject)) return;
            FireBusinessObjectPropertyUpdated(businessObject, propEventArgs.Prop);
        }

        private void BOIDUpdatedEventHandler(object sender, BOEventArgs e)
        {
            TBusinessObject businessObject = (TBusinessObject) e.BusinessObject;
            if (!this.Contains(businessObject)) return;
            if (businessObject.ID == null) return;

            Guid previousObjectID = businessObject.ID.PreviousObjectID;
            if (KeyObjectHashTable.Contains(previousObjectID))
            {
                KeyObjectHashTable.Remove(previousObjectID);
                KeyObjectHashTable.Add(businessObject.ID.ObjectID, businessObject);
            }
            FireBusinessObjectIDUpdated(businessObject);
        }

        /// <summary>
        /// Copies the business objects in one collection across to this one
        /// </summary>
        /// <param name="col">The collection to copy from</param>
        public void Add(BusinessObjectCollection<TBusinessObject> col)
        {
            Add((List<TBusinessObject>) col);
        }

        /// <summary>
        /// Adds the business objects from col into this collection
        /// </summary>
        /// <param name="col"></param>
        public void Add(IEnumerable<TBusinessObject> col)
        {
            foreach (TBusinessObject bo in col)
            {
                this.Add(bo);
            }
        }

        ///<summary>
        /// Adds the specified business objects to this collection
        ///</summary>
        ///<param name="businessObjects">A parameter array of business objects to add to the collection</param>
        public void Add(params TBusinessObject[] businessObjects)
        {
            Add(new List<TBusinessObject>(businessObjects));
        }

        /// <summary>
        /// Refreshes the business objects in the collection
        /// </summary>
        [ReflectionPermission(SecurityAction.Demand)]
        public void Refresh()
        {
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(this);
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
            LoadWithLimit(searchCriteria, orderByClause, -1);
        }

        /// <summary>
        /// Loads business objects that match the search criteria provided in
        /// an expression, loaded in the order specified
        /// </summary>
        /// <param name="searchExpression">The search expression</param>
        /// <param name="orderByClause">The order-by clause</param>
        public void Load(Criteria searchExpression, string orderByClause)
        {
            LoadWithLimit(searchExpression, orderByClause, -1);
        }


        ///// <summary>
        ///// Loads business objects that match the search criteria provided
        ///// and an extra criteria literal,
        ///// loaded in the order specified
        ///// </summary>
        ///// <param name="searchCriteria">The search criteria</param>
        ///// <param name="orderByClause">The order-by clause</param>
        ///// <param name="extraSearchCriteriaLiteral">Extra search criteria</param>
        //public void Load(string searchCriteria, string orderByClause, string extraSearchCriteriaLiteral)
        //{
        //    LoadWithLimit(searchCriteria, orderByClause, extraSearchCriteriaLiteral, -1);
        //}

        ///// <summary>
        ///// Loads business objects that match the search criteria provided in
        ///// an expression and an extra criteria literal, 
        ///// loaded in the order specified
        ///// </summary>
        ///// <param name="searchExpression">The search expression</param>
        ///// <param name="orderByClause">The order-by clause</param>
        ///// <param name="extraSearchCriteriaLiteral">Extra search criteria</param>
        //public void Load(IExpression searchExpression, string orderByClause, string extraSearchCriteriaLiteral)
        //{
        //    LoadWithLimit(searchExpression, orderByClause, extraSearchCriteriaLiteral, -1);
        //}

//        /// <summary>
//        /// Loads business objects that match the search criteria provided, 
//        /// loaded in the order specified, 
//        /// and limiting the number of objects loaded
//        /// </summary>
//        /// <param name="searchCriteria">The search criteria</param>
//        /// <param name="orderByClause">The order-by clause</param>
//        /// <param name="limit">The limit</param>
//        public void LoadWithLimit(string searchCriteria, string orderByClause, int limit)
//        {
//            LoadWithLimit(searchCriteria, orderByClause, limit);
//        }
        /// <summary>
        /// Loads business objects that match the search criteria provided
        /// and an extra criteria literal, 
        /// loaded in the order specified, 
        /// and limiting the number of objects loaded
        /// </summary>
        /// <param name="searchCriteria">The search expression</param>
        /// <param name="orderByClause">The order-by clause</param>
        /// <param name="limit">The limit</param>
        public void LoadWithLimit(string searchCriteria, string orderByClause, int limit)
        {
            Criteria criteriaExpression = null;
            if (searchCriteria.Length > 0)
            {
                criteriaExpression = CriteriaParser.CreateCriteria(searchCriteria);
                QueryBuilder.PrepareCriteria(this.ClassDef, criteriaExpression);
            }
            LoadWithLimit(criteriaExpression, orderByClause, limit);
        }

        /// <summary>
        /// Loads business objects that match the search criteria provided, 
        /// loaded in the order specified, 
        /// and limiting the number of objects loaded. The limited number of objects is
        ///  limited as follows. If you want record 11 to 20 then firstRecordToLoad will be set to 
        ///  11 and the number of records to load will be set to 10 this will load 10 records (or fewer 
        ///   if there are less records returned) starting at record 11 ordered by the orderByClause.
        /// </summary>
        /// <param name="searchCriteria">The search criteria</param>
        /// <param name="orderByClause">The order-by clause</param>
        /// <param name="noOfRecords">The number of records to be loaded</param>
        /// <param name="firstRecordToLoad">The first record to load</param>
        public void LoadWithLimit(string searchCriteria, string orderByClause, int firstRecordToLoad, int noOfRecords)
        {
            Criteria criteriaExpression = null;
            if (searchCriteria.Length > 0)
            {
                criteriaExpression = CriteriaParser.CreateCriteria(searchCriteria);
                QueryBuilder.PrepareCriteria(this.ClassDef, criteriaExpression);
            }
            int totalRecords = 0;
            LoadWithLimit(criteriaExpression, orderByClause, firstRecordToLoad, noOfRecords, ref totalRecords);
        }

//        /// <summary>
//        /// Loads business objects that match the search criteria provided in
//        /// an expression, loaded in the order specified, 
//        /// and limiting the number of objects loaded
//        /// </summary>
//        /// <param name="searchExpression">The search expression</param>
//        /// <param name="orderByClause">The order-by clause</param>
//        /// <param name="limit">The limit</param>
//        public void LoadWithLimit(IExpression searchExpression, string orderByClause, int limit)
//        {
//            LoadWithLimit(searchExpression, orderByClause, limit);
//        }


        /// <summary>
        /// Loads business objects that match the search criteria provided in
        /// an expression and an extra criteria literal, 
        /// loaded in the order specified, 
        /// and limiting the number of objects loaded
        /// </summary>
        /// <param name="searchExpression">The search expression</param>
        /// <param name="orderByClause">The order-by clause</param>
        /// <param name="limit">The limit</param>
        public void LoadWithLimit(Criteria searchExpression, string orderByClause, int limit)
        {
            this.SelectQuery.Criteria = searchExpression;

            this.SelectQuery.OrderCriteria = QueryBuilder.CreateOrderCriteria(this.ClassDef, orderByClause);
            if (limit > -1) this.SelectQuery.Limit = limit;

            Refresh();
        }

        /// <summary>
        /// Loads business objects that match the search criteria provided in
        /// an expression and an extra criteria literal, 
        /// loaded in the order specified, 
        /// and limiting the number of objects loaded
        /// </summary>
        /// <param name="searchExpression">The search expression</param>
        /// <param name="orderByClause">The order-by clause</param>
        /// <param name="noOfRecords">The number of records to be loaded</param>
        /// <param name="firstRecordToLoad">The first record to load</param>
        /// <param name="totalNoOfRecords">The total number of records that exist</param>
        public void LoadWithLimit
            (Criteria searchExpression, string orderByClause, int firstRecordToLoad, int noOfRecords,
             ref int totalNoOfRecords)
        {
            this.SelectQuery.Criteria = searchExpression;
            this.SelectQuery.OrderCriteria = QueryBuilder.CreateOrderCriteria(this.ClassDef, orderByClause);
            if (firstRecordToLoad <= 0) this.SelectQuery.Limit = noOfRecords;
            if ((firstRecordToLoad > 0) && (noOfRecords > 0))
            {
                this.SelectQuery.FirstRecordToLoad = firstRecordToLoad;
                if (totalNoOfRecords <= 0)
                {
                    totalNoOfRecords = ((BusinessObjectLoaderDB) BORegistry.DataAccessor.BusinessObjectLoader).GetCount
                        (this.ClassDef, searchExpression);
                }
                int newLimit = totalNoOfRecords - firstRecordToLoad;
                this.SelectQuery.Limit = (newLimit > noOfRecords) ? noOfRecords : newLimit;
            }
            Refresh();
        }

        #endregion

        /// <summary>
        /// Clears the collection
        /// </summary>
        public new void Clear()
        {
            DeRegisterBoEventsForAllBusinessObjects();
            base.Clear();
            KeyObjectHashTable.Clear();
            this.PersistedBusinessObjects.Clear();
            this.CreatedBusinessObjects.Clear();
            this.RemovedBusinessObjects.Clear();
            this.AddedBusinessObjects.Clear();
            this.MarkedForDeleteBusinessObjects.Clear();
        }


        /// <summary>
        /// Removes the business object at the index position specified
        /// </summary>
        /// <param name="index">The index position to remove from</param>
        public new void RemoveAt(int index)
        {
            TBusinessObject boToRemove = this[index];
            Remove(boToRemove);
        }

        /// <summary>
        /// Removes the specified business object from the collection
        /// </summary>
        /// <param name="bo">The business object to remove</param>
        public new virtual bool Remove(TBusinessObject bo)
        {
            return RemoveInternal(bo);
        }

        ///<summary>
        /// Clears only the current collection i.e. the persisted, removed, added and created lists 
        ///    are retained.
        ///</summary>
        /// Note_: This is used by reflection by the collection loader.
// ReSharper disable UnusedPrivateMember
        internal void ClearCurrentCollection()
        {
            foreach (TBusinessObject businessObject in this)
            {
                DeRegisterBOEvents(businessObject);
            }
            base.Clear();
            KeyObjectHashTable.Clear();
        }

// ReSharper restore UnusedPrivateMember

        /// <summary>
        /// Removes the specified business object from the collection. This is used when refreshing
        /// a collection so that any overriden behaviour (from overriding Remove) is not applied
        /// when loading and refreshing.
        /// </summary>
        /// <param name="businessObject"></param>
        /// <returns></returns>
        internal bool RemoveInternal(TBusinessObject businessObject)
        {
            bool removed = base.Remove(businessObject);
            KeyObjectHashTable.Remove(businessObject.ID.ObjectID);

            if (!_removedBusinessObjects.Contains(businessObject)
                && !_markedForDeleteBusinessObjects.Contains(businessObject))
            {
                _removedBusinessObjects.Add(businessObject);
                this.FireBusinessObjectRemoved(businessObject);
            }
            RemoveCreatedBusinessObject(businessObject);
            RemoveAddedBusinessObject(businessObject);
            return removed;
        }

        private void RemoveCreatedBusinessObject(TBusinessObject businessObject)
        {
            if (!this.CreatedBusinessObjects.Remove(businessObject)) return;

            this.RemovedBusinessObjects.Remove(businessObject);
            DeRegisterBOEvents(businessObject);
        }

        private void RemoveAddedBusinessObject(TBusinessObject businessObject)
        {
            if (this.MarkedForDeleteBusinessObjects.Contains(businessObject)) return;

            if (!this.AddedBusinessObjects.Remove(businessObject)) return;

            this.RemovedBusinessObjects.Remove(businessObject);
            DeRegisterBOEvents(businessObject);
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
                    if (child.Status.IsDirty)
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


        ///<summary>
        /// The select query that is used to load this business object collection.
        ///</summary>
        public ISelectQuery SelectQuery
        {
            get { return _selectQuery; }
            set
            {
                if (value == null)
                {
                    throw new HabaneroDeveloperException
                        ("A collections select query cannot be set to null",
                         "A collections select query cannot be set to null");
                }
                _selectQuery = value;
            }
        }

        /// <summary>
        /// Finds a business object that has the key string specified.<br/>
        /// The format of the search term is strict, so that a Guid ID
        /// may be stored as "boIDname=########-####-####-####-############".
        /// In the case of such Guid ID's, rather use the FindByGuid() function.
        /// Composite primary keys may be stored otherwise, such as a
        /// concatenation of the key names.
        /// </summary>
        /// <param name="key">The object identifier as a Guid</param>
        /// <returns>Returns the business object if found, or null if not</returns>
        public TBusinessObject Find(Guid key)
        {
            if (KeyObjectHashTable.ContainsKey(key))
            {
                TBusinessObject bo = (TBusinessObject) KeyObjectHashTable[key];
                return this.Contains(bo) ? bo : null;
            }
//
//            foreach (TBusinessObject createdBusinessObject in _createdBusinessObjects)
//            {
//                if (createdBusinessObject.ID.AsString_CurrentValue() == key || createdBusinessObject.ID.AsString_LastPersistedValue() == key || createdBusinessObject.ID.AsString_PreviousValue() == key) return createdBusinessObject;
//            }
            return null;
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
            //string formattedSearchItem = searchTerm.ToString();
            if (KeyObjectHashTable.ContainsKey(searchTerm))
            {
                return (TBusinessObject)KeyObjectHashTable[searchTerm];
            }
            return null;
        }

        /// <summary>
        /// Returns the class definition of the collection
        /// </summary>
        public IClassDef ClassDef
        {
            get { return _boClassDef; }
            set { _boClassDef = value; }
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
            BusinessObjectCollection<TBusinessObject> clonedCol = new BusinessObjectCollection<TBusinessObject>
                (_boClassDef);
            foreach (TBusinessObject businessObjectBase in this)
            {
                clonedCol.Add(businessObjectBase);
            }
            foreach (TBusinessObject persistedBusinessObject in this.PersistedBusinessObjects)
            {
                clonedCol.AddToPersistedCollection(persistedBusinessObject);
            }
            foreach (TBusinessObject createdBusinessObject in this.CreatedBusinessObjects)
            {
                if (!clonedCol.Contains(createdBusinessObject))
                {
                    clonedCol.CreatedBusinessObjects.Add(createdBusinessObject);
                }
            }
            foreach (TBusinessObject removedBusinessObject in this.RemovedBusinessObjects)
            {
                if (!clonedCol.Contains(removedBusinessObject))
                {
                    clonedCol.RemovedBusinessObjects.Add(removedBusinessObject);
                }
            }
            foreach (TBusinessObject addedBusinessObject in this.AddedBusinessObjects)
            {
                if (!clonedCol.AddedBusinessObjects.Contains(addedBusinessObject))
                {
                    clonedCol.AddedBusinessObjects.Add(addedBusinessObject);
                }
            }
            return clonedCol;
        }

        /// <summary>
        /// Returns a new collection that is a copy of this collection
        /// </summary>
        /// <returns>Returns the cloned copy</returns>
        public BusinessObjectCollection<DestType> Clone<DestType>() where DestType : BusinessObject, new()
        {
            BusinessObjectCollection<DestType> clonedCol = new BusinessObjectCollection<DestType>(_boClassDef);
            if (!typeof (DestType).IsSubclassOf(typeof (TBusinessObject))
                && !typeof (TBusinessObject).IsSubclassOf(typeof (DestType))
                && !typeof (TBusinessObject).Equals(typeof (DestType)))
            {
                throw new InvalidCastException
                    (String.Format
                         ("Cannot cast a collection of type '{0}' to " + "a collection of type '{1}'.",
                          typeof (TBusinessObject).Name, typeof (DestType).Name));
            }
            foreach (TBusinessObject businessObject in this)
            {
                DestType obj = businessObject as DestType;
                if (obj != null)
                {
                    clonedCol.Add(obj);
                    clonedCol.AddToPersistedCollection(obj);
                }
            }
            foreach (TBusinessObject createdBusinessObject in this.CreatedBusinessObjects)
            {
                DestType obj = createdBusinessObject as DestType;
                if (obj != null)
                {
                    clonedCol.CreatedBusinessObjects.Add(obj);
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

        void IBusinessObjectCollection.Sort(IComparer comparer)
        {
            if (comparer == null) throw new ArgumentNullException("comparer");
            this.Sort(new StronglyTypedComperer<TBusinessObject>(comparer));
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
        public virtual void SaveAll()
        {
            ITransactionCommitter committer = BORegistry.DataAccessor.CreateTransactionCommitter();

            SaveAllInTransaction(committer);
        }

        protected virtual void SaveAllInTransaction(ITransactionCommitter transaction)
        {
            foreach (TBusinessObject bo in this)
            {
                if (bo.Status.IsDirty || bo.Status.IsNew)
                {
                    transaction.AddBusinessObject(bo);
                }
            }
            foreach (TBusinessObject bo in this._createdBusinessObjects)
            {
                transaction.AddBusinessObject(bo);
            }
            foreach (TBusinessObject businessObject in this.MarkedForDeleteBusinessObjects)
            {
                transaction.AddBusinessObject(businessObject);
            }
            transaction.CommitTransaction();
            CreatedBusinessObjects.Clear();
            RemovedBusinessObjects.Clear();
            this.MarkedForDeleteBusinessObjects.Clear();
        }

        /// <summary>
        /// Restores all the business objects to their last persisted state, that
        /// is their state and values at the time they were last saved to the database
        /// </summary>
        public void CancelEdits()
        {
            foreach (TBusinessObject bo in this.Clone())
            {
                bo.Restore();
            }
            while (this.MarkedForDeleteBusinessObjects.Count > 0)
            {
                TBusinessObject bo = this.MarkedForDeleteBusinessObjects[0];
                bo.Restore();
            }
            while (this.CreatedBusinessObjects.Count > 0)
            {
                TBusinessObject bo = this.CreatedBusinessObjects[0];
                this.CreatedBusinessObjects.Remove(bo);
                bo.Restore();
                this.RemoveInternal(bo);
                this.RemovedBusinessObjects.Remove(bo);
                DeRegisterBOEvents(bo);
            }
            while (this.RemovedBusinessObjects.Count > 0)
            {
                TBusinessObject bo = this.RemovedBusinessObjects[0];
                this.RemovedBusinessObjects.Remove(bo);
                bo.Restore();
                this.Add(bo);
            }
            while (this.AddedBusinessObjects.Count > 0)
            {
                TBusinessObject bo = this.AddedBusinessObjects[0];
                this.AddedBusinessObjects.Remove(bo);
                bo.Restore();
                bool removed = base.Remove(bo);
                KeyObjectHashTable.Remove(bo.ID.ToString());
                if (removed)
                {
                    this.FireBusinessObjectRemoved(bo);
                }
                this.RemovedBusinessObjects.Remove(bo);
            }
        }

        [Obsolete("Should use Cancel Edits")]
        /// <summary>
        /// Restores all the business objects to their last persisted state, that
        /// is their state and values at the time they were last saved to the database
        /// </summary>
        public void RestoreAll()
        {
            CancelEdits();
        }

        #region IBusinessObjectCollection Members

        /// <summary>
        /// Finds a business object that has the key string specified.<br/>
        /// The format of the search term is strict, so that a Guid ID
        /// may be stored as "boIDname=########-####-####-####-############".
        /// In the case of such Guid ID's, rather use the FindByGuid() function.
        /// Composite primary keys may be stored otherwise, such as a
        /// concatenation of the key names.
        /// </summary>
        /// <param name="key">The orimary key as a string</param>
        /// <returns>Returns the business object if found, or null if not</returns>
        IBusinessObject IBusinessObjectCollection.Find(Guid key)
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

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"></see>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
        /// <returns>
        /// The index of item if found in the list; otherwise, -1.
        /// </returns>
        int IBusinessObjectCollection.IndexOf(IBusinessObject item)
        {
            return this.IndexOf((TBusinessObject) item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"></see> at the specified index.
        /// </summary>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
        void IBusinessObjectCollection.Insert(int index, IBusinessObject item)
        {
            this.Insert(index, (TBusinessObject) item);
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
        /// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
        /// <returns>The element at the specified index.</returns>
        IBusinessObject IBusinessObjectCollection.this[int index]
        {
            get { return base[index]; }
            set { base[index] = (TBusinessObject) value; }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        void IBusinessObjectCollection.Add(IBusinessObject item)
        {
            this.Add((TBusinessObject) item);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
        /// </summary>
        /// <returns>
        /// True if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        bool IBusinessObjectCollection.Contains(IBusinessObject item)
        {
            return this.Contains((TBusinessObject) item);
        }

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
        void IBusinessObjectCollection.CopyTo(IBusinessObject[] array, int arrayIndex)
        {
            TBusinessObject[] thisArray = new TBusinessObject[array.LongLength];
            this.CopyTo(thisArray, arrayIndex);
            int count = Count;
            for (int index = 0; index < count; index++)
                array[arrayIndex + index] = thisArray[arrayIndex + index];
        }

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
        bool IBusinessObjectCollection.Remove(IBusinessObject item)
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

        /// <summary>
        /// Returns a list of the business objects that have been created via the
        ///   collection but have not been persisted to the database.
        /// </summary>
        IList IBusinessObjectCollection.CreatedBusinessObjects
        {
            get { return _createdBusinessObjects; }
        }

        /// <summary>
        /// The list of business objects that have been created via this collection (@see CreateBusinessObject) and have not
        /// yet been persisted.
        /// </summary>
        public List<TBusinessObject> CreatedBusinessObjects
        {
            get { return _createdBusinessObjects; }
        }

        /// <summary>
        /// Returns a collection representing the BusinessObjects that were last read or saved to the 
        ///    datastore. This collection thus represents an exact list of objects as per the database.
        ///    this collection excludes Created and Added BusinessObjects but includes Deleted and Removed Business
        ///    objects.
        /// Hack: This method was created to overcome the shortfall of using a Generic Collection.
        /// </summary>
        IList IBusinessObjectCollection.PersistedBusinessObjects
        {
            get { return _persistedObjectsCollection; }
        }

        /// <summary>
        /// Returns a collection representing the BusinessObjects that were last read or saved to the 
        ///    datastore. This collection thus represents an exact list of objects as per the database.
        ///    this collection excludes Created and Added BusinessObjects but includes Deleted and Removed Business
        ///    objects.
        /// </summary>
        public IList<TBusinessObject> PersistedBusinessObjects
        {
            get { return _persistedObjectsCollection; }
        }

        /// <summary>
        /// Returns a list of the business objects that are currently removed for the
        ///   collection but have not yet been persisted to the database.
        /// </summary>
        /// Hack: This method was created returning a type IList to overcome problems with 
        ///   BusinessObjectCollecion being a generic collection.
        IList IBusinessObjectCollection.RemovedBusinessObjects
        {
            get { return this.RemovedBusinessObjects; }
        }

        ///<summary>
        /// Returns a collection of business objects that have been removed from the collection
        /// but the collection has not yet been persisted.
        ///</summary>
        public List<TBusinessObject> RemovedBusinessObjects
        {
            get { return _removedBusinessObjects; }
        }


        /// <summary>
        /// Returns a list of the business objects that are currently marked for deletion for the
        ///   collection but have not cessarily been persisted to the database.
        /// </summary>
        IList IBusinessObjectCollection.MarkedForDeleteBusinessObjects
        {
            get { return this.MarkedForDeleteBusinessObjects; }
        }

        ///<summary>
        /// Returns a collection of business objects that have been marked for deletion from the collection
        /// but the collection has not yet been persisted.
        ///</summary>
        ///<exception cref="NotImplementedException"></exception>
        public List<TBusinessObject> MarkedForDeleteBusinessObjects
        {
            get { return _markedForDeleteBusinessObjects; }
        }

        /// <summary>
        /// Returns a list of the business objects that are currently added for the
        ///   collection but have not cessarily been persisted to the database.
        /// </summary>
        IList IBusinessObjectCollection.AddedBusinessObjects
        {
            get { return this.AddedBusinessObjects; }
        }

        /// <summary>
        /// The list of business objects that have been adde to this relationship 
        /// collection (<see cref="RelatedBusinessObjectCollection{TBusinessObject}.Add(TBusinessObject)"/>) and have not
        /// yet been persisted.
        /// </summary>
        public List<TBusinessObject> AddedBusinessObjects
        {
            get { return _addedBusinessObjects; }
        }

        private Hashtable KeyObjectHashTable { get; set; }

// ReSharper disable UnusedPrivateMember 
        //This method is used by reflection only
        /// <summary>
        /// This property is set to true while loading the collection from the datastore so as to 
        /// prevent certain checks being done (e.g. Adding persisted business objects to a collection.
        /// </summary>
        protected bool Loading { get; set; }

// ReSharper restore UnusedPrivateMember

        #endregion

        /// <summary>
        /// Creates a business object of type TBusinessObject
        /// Adds this BO to the CreatedBusinessObjects list. When the object is saved it will
        /// be added to the actual bo collection.
        /// </summary>
        /// <returns></returns>
        public virtual TBusinessObject CreateBusinessObject()
        {
            TBusinessObject newBO;
            if (this.ClassDef == ClassDefinition.ClassDef.ClassDefs[typeof (TBusinessObject)])
            {
                newBO = (TBusinessObject) Activator.CreateInstance(typeof (TBusinessObject));
            }
            else
            {
                //use the customised classdef instead of the default.
                try
                {
                    newBO =
                        (TBusinessObject)
                        Activator.CreateInstance(typeof (TBusinessObject), new object[] {this.ClassDef});
                }
                catch (MissingMethodException ex)
                {
                    string className = typeof (TBusinessObject).FullName;
                    string msg = string.Format
                        ("An attempt was made to create a {0} with a customised class def. Please add a constructor that takes a ClassDef as a parameter to the business object class of type {1}.",
                         className, className);
                    throw new HabaneroDeveloperException("There was a problem creating a " + className, msg, ex);
                }
            }
            AddCreatedBusinessObject(newBO);
            return newBO;
        }

        private void AddCreatedBusinessObject(TBusinessObject newBO)
        {
            CreatedBusinessObjects.Add(newBO);
            if (this.Contains(newBO)) return;

            AddWithoutEvents(newBO);
            this.FireBusinessObjectAdded(newBO);
        }

        /// <summary>
        /// Creates a business object of type TBusinessObject
        /// Adds this BO to the CreatedBusinessObjects list. When the object is saved it will
        /// be added to the actual bo collection.
        /// </summary>
        /// <returns></returns>
        IBusinessObject IBusinessObjectCollection.CreateBusinessObject()
        {
            return CreateBusinessObject();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            int count = 0;
            info.AddValue(COUNT, Count);
            info.AddValue(CREATED_COUNT, this.CreatedBusinessObjects.Count);
            info.AddValue(CLASS_NAME, this.ClassDef.ClassName);
            info.AddValue(ASSEMBLY_NAME, this.ClassDef.AssemblyName);
            foreach (TBusinessObject businessObject in this)
            {
                info.AddValue(BUSINESS_OBJECT + count, businessObject);
                count++;
            }
            int createdCount = 0;
            foreach (TBusinessObject createdBusinessObject in this.CreatedBusinessObjects)
            {
                info.AddValue(CREATED_BUSINESS_OBJECT + createdCount, createdBusinessObject);
                createdCount++;
            }
        }

        ///<summary>
        /// Marks the business object as MarkedForDeletion and places the 
        ///</summary>
        ///<param name="businessObject"></param>
        public void MarkForDelete(TBusinessObject businessObject)
        {
            businessObject.MarkForDelete();
        }

        ///<summary>
        /// Marks the business object as MarkedForDeletion and places the 
        ///</summary>
        ///<param name="index">The index position to remove from</param>if index does not exist in col
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void MarkForDeleteAt(int index)
        {
            TBusinessObject boToMark = this[index];
            MarkForDelete(boToMark);
        }
    }
}
