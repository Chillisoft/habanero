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
using Habanero.Util;

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

        private readonly EventHandler<BOEventArgs> _savedEventHandler;
        private readonly EventHandler<BOEventArgs> _deletedEventHandler;
        private readonly EventHandler<BOEventArgs> _restoredEventHandler;
        private readonly EventHandler<BOEventArgs> _markForDeleteEventHandler;
        private readonly EventHandler<BOEventArgs> _updatedEventHandler;
        private readonly EventHandler<BOPropUpdatedEventArgs> _boPropUpdatedEventHandler;
        private readonly EventHandler<BOEventArgs> _boIDUpdatedEventHandler;

        private ISelectQuery _selectQuery;



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
        [Obsolete("Please initialise with a ClassDef instead.  This option will be removed in later versions of Habanero.")]
        public BusinessObjectCollection(TBusinessObject bo) : this(null, bo)
        {
        }


        private BusinessObjectCollection(IClassDef classDef, TBusinessObject sampleBo)
        {
            Initialise(classDef, sampleBo);
            this._savedEventHandler = SavedEventHandler;
            this._deletedEventHandler = DeletedEventHandler;
            this._restoredEventHandler = RestoredEventHandler;
            //this._updateIDEventHandler = UpdateHashTable;
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

        /// <summary>
        /// Reconstitutes the collection from a stream that was serialised.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
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
            //_updateIDEventHandler = UpdateHashTable;
        }

        #region Events and event handlers


        private readonly IDictionary<EventHandler<BOEventArgs>, EventHandler<BOEventArgs<TBusinessObject>>> _updatedHandlers = new Dictionary<EventHandler<BOEventArgs>, EventHandler<BOEventArgs<TBusinessObject>>>();
        private readonly IDictionary<EventHandler<BOEventArgs>, EventHandler<BOEventArgs<TBusinessObject>>> _addedHandlers = new Dictionary<EventHandler<BOEventArgs>, EventHandler<BOEventArgs<TBusinessObject>>>();
        private readonly IDictionary<EventHandler<BOEventArgs>, EventHandler<BOEventArgs<TBusinessObject>>> _removedHandlers = new Dictionary<EventHandler<BOEventArgs>, EventHandler<BOEventArgs<TBusinessObject>>>();
        private readonly IDictionary<EventHandler<BOEventArgs>, EventHandler<BOEventArgs<TBusinessObject>>> _idUpdatedHandlers = new Dictionary<EventHandler<BOEventArgs>, EventHandler<BOEventArgs<TBusinessObject>>>();
        private readonly IDictionary<EventHandler<BOPropUpdatedEventArgs>, EventHandler<BOPropUpdatedEventArgs<TBusinessObject>>> _propertyUpdatedHandlers = new Dictionary<EventHandler<BOPropUpdatedEventArgs>, EventHandler<BOPropUpdatedEventArgs<TBusinessObject>>>();

        private event EventHandler<BOEventArgs<TBusinessObject>> _BusinessObjectRemoved;
        private event EventHandler<BOEventArgs<TBusinessObject>> _BusinessObjectAdded;
        private event EventHandler<BOEventArgs<TBusinessObject>> _BusinessObjectUpdated;
        private event EventHandler<BOEventArgs<TBusinessObject>> _BusinessObjectIDUpdated;
        private event EventHandler<BOPropUpdatedEventArgs<TBusinessObject>> _BusinessObjectPropertyUpdated;

        /// <summary>
        /// Handles the event of a business object being added
        /// </summary>
        event EventHandler<BOEventArgs> IBusinessObjectCollection.BusinessObjectAdded
        {
            add {
                EventHandler<BOEventArgs<TBusinessObject>> addedHandler = new EventHandler<BOEventArgs<TBusinessObject>>(value);
                _addedHandlers[value] = addedHandler;
                _BusinessObjectAdded += addedHandler;
        }
            remove {
                _BusinessObjectAdded -= _addedHandlers[value];
                _addedHandlers.Remove(value);
            }
        }

        /// <summary>
        /// Handles the event of a business object being added
        /// </summary>
        public event EventHandler<BOEventArgs<TBusinessObject>> BusinessObjectAdded
        {
            add
            {
                _BusinessObjectAdded += value;
            }
            remove
            {
                _BusinessObjectAdded -=value;
            }
        }

        /// <summary>
        /// Handles the event of a business object being removed
        /// </summary>
        event EventHandler<BOEventArgs> IBusinessObjectCollection.BusinessObjectRemoved
        {
            add
            {
                EventHandler<BOEventArgs<TBusinessObject>> removedHandler = new EventHandler<BOEventArgs<TBusinessObject>>(value);
                _removedHandlers[value] = removedHandler;
                _BusinessObjectRemoved += removedHandler;
            }
            remove
            {
                _BusinessObjectRemoved -= _removedHandlers[value];
                _removedHandlers.Remove(value);
            }
        }
        
        /// <summary>
        /// Handles the event of a business object being removed
        /// </summary>
        public event EventHandler<BOEventArgs<TBusinessObject>> BusinessObjectRemoved
        {
            add
            {
                _BusinessObjectRemoved += value;
            }
            remove
            {
                _BusinessObjectRemoved -= value;
            }
        }


        /// <summary>
        /// Handles the event of any business object in this collection being Updated(i.e the BO is saved, or edits are cancelled).
        /// See the <see cref="IBusinessObject"/>.<see cref="IBusinessObject.Updated"/> event.
        /// </summary>
        event EventHandler<BOEventArgs> IBusinessObjectCollection.BusinessObjectUpdated
        {
            add
            {
                EventHandler<BOEventArgs<TBusinessObject>> updatedHandler = new EventHandler<BOEventArgs<TBusinessObject>>(value);
                _updatedHandlers[value] = updatedHandler;
                _BusinessObjectUpdated += updatedHandler;
            }
            remove
            {
                _BusinessObjectUpdated -= _updatedHandlers[value];
                _updatedHandlers.Remove(value);
            }
        }
        /// <summary>
        /// Handles the event of any business object in this collection being Updated(i.e the BO is saved, or edits are cancelled).
        /// See the <see cref="IBusinessObject"/>.<see cref="IBusinessObject.Updated"/> event.
        /// </summary>
        public event EventHandler<BOEventArgs<TBusinessObject>> BusinessObjectUpdated
        {
            add
            {
                _BusinessObjectUpdated += value;
            }
            remove
            {
                _BusinessObjectUpdated -= value;
            }
        }

        /// <summary>
        /// Handles the event of any business object in this collection being edited (i.e. a property value is changed).
        /// See the <see cref="IBusinessObject"/>.<see cref="IBusinessObject.PropertyUpdated"/> event.
        /// </summary>
        event EventHandler<BOPropUpdatedEventArgs> IBusinessObjectCollection.BusinessObjectPropertyUpdated
        {
            add
            {
                EventHandler<BOPropUpdatedEventArgs<TBusinessObject>> propertyUpdatedHandler = new EventHandler<BOPropUpdatedEventArgs<TBusinessObject>>(value);
                _propertyUpdatedHandlers[value] = propertyUpdatedHandler;
                _BusinessObjectPropertyUpdated += propertyUpdatedHandler;
            }
            remove
            {
                _BusinessObjectPropertyUpdated -= _propertyUpdatedHandlers[value];
                _propertyUpdatedHandlers.Remove(value);
            }
        }

        /// <summary>
        /// Handles the event of any business object in this collection being edited (i.e. a property value is changed).
        /// See the <see cref="IBusinessObject"/>.<see cref="IBusinessObject.PropertyUpdated"/> event.
        /// </summary>
        public event EventHandler<BOPropUpdatedEventArgs<TBusinessObject>> BusinessObjectPropertyUpdated
        {
            add
            {
                _BusinessObjectPropertyUpdated += value;
            }
            remove
            {
                _BusinessObjectPropertyUpdated -= value;
            }
        }


        /// <summary>
        /// Handles the event when a BusinessObject in the collection has an ID that is Updated(i.e one of the properties of the ID is edited).
        /// </summary>
        event EventHandler<BOEventArgs> IBusinessObjectCollection.BusinessObjectIDUpdated
        {
            add
            {
                EventHandler<BOEventArgs<TBusinessObject>> idUpdatedHandler = new EventHandler<BOEventArgs<TBusinessObject>>(value);
                _idUpdatedHandlers[value] = idUpdatedHandler;
                _BusinessObjectIDUpdated += idUpdatedHandler;
            }
            remove
            {
                _BusinessObjectIDUpdated -= _idUpdatedHandlers[value];
                _idUpdatedHandlers.Remove(value);
            }
        }

        /// <summary>
        /// Handles the event when a BusinessObject in the collection has an ID that is Updated(i.e one of the properties of the ID is edited).
        /// </summary>
        public event EventHandler<BOEventArgs<TBusinessObject>> BusinessObjectIDUpdated
        {
            add
            {
                _BusinessObjectIDUpdated += value;
            }
            remove
            {
                _BusinessObjectIDUpdated -= value;
            }
        }

        /// <summary>
        /// Calls the BusinessObjectAdded() handler
        /// </summary>
        /// <param name="bo">The business object added</param>
        private void FireBusinessObjectAdded(TBusinessObject bo)
        {
            if (_BusinessObjectAdded != null)
            {
                _BusinessObjectAdded(this, new BOEventArgs<TBusinessObject>(bo));
            }
        }

        /// <summary>
        /// Calls the BusinessObjectRemoved() handler
        /// </summary>
        /// <param name="bo">The business object removed</param>
        private void FireBusinessObjectRemoved(TBusinessObject bo)
        {
            if (this._BusinessObjectRemoved != null)
            {
                this._BusinessObjectRemoved(this, new BOEventArgs<TBusinessObject>(bo));
            }
        }

        private void FireBusinessObjectUpdated(TBusinessObject bo)
        {
            if (_BusinessObjectUpdated != null)
            {
                _BusinessObjectUpdated(this, new BOEventArgs<TBusinessObject>(bo));
            }
        }

        private void FireBusinessObjectPropertyUpdated(TBusinessObject bo, IBOProp boProp)
        {
            if (_BusinessObjectPropertyUpdated != null)
            {
                _BusinessObjectPropertyUpdated(this, new BOPropUpdatedEventArgs<TBusinessObject>(bo, boProp));
            }
        }

        private void FireBusinessObjectIDUpdated(TBusinessObject bo)
        {
            if (_BusinessObjectIDUpdated != null)
            {
                _BusinessObjectIDUpdated(this, new BOEventArgs<TBusinessObject>(bo));
            }
        }

        #endregion

        /// <summary>
        /// Adds a business object to the collection
        /// </summary>
        /// <param name="bo">The business object to add</param>
        public new virtual void Add(TBusinessObject bo)
        {
            if (bo == null) throw new ArgumentNullException("bo");
            bool addSuccessful;
            lock (KeyObjectHashTable)
            {
                addSuccessful = AddInternal(bo);
            }
            if (addSuccessful) this.FireBusinessObjectAdded(bo);
        }

        protected virtual bool AddInternal(TBusinessObject bo) {
            if (this.Contains(bo)) return false;
            if (bo.Status.IsNew && !this.CreatedBusinessObjects.Contains(bo))
            {
                AddCreatedBusinessObject(bo);
            }
            else
            {
                if (bo.Status.IsDeleted) return false;
                AddWithoutEvents(bo);
                if (!AddedBusinessObjects.Contains(bo) && !PersistedBusinessObjects.Contains(bo))
                {
                    AddedBusinessObjects.Add(bo);
                }
                this.RemovedBusinessObjects.Remove(bo);
            }
            return true;
        }

        void IBusinessObjectCollection.AddWithoutEvents(IBusinessObject businessObject)
        {
            lock (KeyObjectHashTable)
            {
                AddWithoutEvents(businessObject as TBusinessObject);
            }
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
            if (bo.ID != null) if (KeyObjectHashTable.Contains(bo.ID.ObjectID)) return;
            if (bo.ID != null) KeyObjectHashTable.Add(bo.ID.ObjectID, bo);
            RegisterBOEvents(bo);
        }

        private void RegisterBOEvents(TBusinessObject businessObject)
        {
            businessObject.Saved += _savedEventHandler;
            businessObject.Deleted += _deletedEventHandler;
            businessObject.Restored += _restoredEventHandler;
            //if (businessObject.ID != null) businessObject.IDUpdated += _updateIDEventHandler;
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
            //if (businessObject.ID != null) businessObject.IDUpdated -= _updateIDEventHandler;
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

        ///// <summary>
        ///// Updates the lookup table when a primary key property has
        ///// changed
        ///// </summary>
        //private void UpdateHashTable(object sender, BOEventArgs e)
        //{
        //    string oldID = e.BusinessObject.ID.AsString_PreviousValue();
        //    if (KeyObjectHashTable.Contains(oldID))
        //    {
        //        BusinessObject bo = (BusinessObject) KeyObjectHashTable[oldID];
        //        KeyObjectHashTable.Remove(oldID);
        //        KeyObjectHashTable.Add(bo.ID.AsString_CurrentValue(), bo);
        //    }
        //}

        private void MarkForDeleteEventHandler(object sender, BOEventArgs e)
        {
            bool removeSuccessful;
            TBusinessObject bo;
            lock (KeyObjectHashTable)
            {
                bo = e.BusinessObject as TBusinessObject;
                if (bo == null) return;
                if (this.MarkedForDeleteBusinessObjects.Contains(bo)) return;

                this.MarkedForDeleteBusinessObjects.Add(bo);
                base.Remove(bo);
                //KeyObjectHashTable.Remove(bo.ID.AsString_CurrentValue());
				KeyObjectHashTable.Remove(bo.ID.ObjectID);
                removeSuccessful = !this.RemovedBusinessObjects.Remove(bo);
            }
            if (removeSuccessful) this.FireBusinessObjectRemoved(bo);
        }

        /// <summary>
        /// Handles the event of a business object being deleted
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void DeletedEventHandler(object sender, BOEventArgs e)
        {
            bool fireEvent;
            TBusinessObject bo;
            lock (KeyObjectHashTable)
            {
                bo = e.BusinessObject as TBusinessObject;
                if (bo == null) return;
                this.RemoveInternal(bo, out fireEvent);
                this.PersistedBusinessObjects.Remove(bo);
                this.RemovedBusinessObjects.Remove(bo);
                this.MarkedForDeleteBusinessObjects.Remove(bo);
                if (bo.Status.IsDeleted) this.AddedBusinessObjects.Remove(bo);
                DeRegisterBOEvents(bo);
            }
            if (fireEvent) FireBusinessObjectRemoved(bo);
        }

        protected virtual void RestoredEventHandler(object sender, BOEventArgs e)
        {
            bool addEventRequired = false;
            TBusinessObject bo;
            lock (KeyObjectHashTable)
            {
                bo = e.BusinessObject as TBusinessObject;
                if (!this.MarkedForDeleteBusinessObjects.Remove(bo) || (this.Contains(bo))) return;
                if (this.AddedBusinessObjects.Contains(bo) && this.Contains(bo))
                {
                    this.AddWithoutEvents(bo);
                }
                else
                {
                   ReflectionUtilities.SetPrivatePropertyValue(this, "Loading", true);
                   addEventRequired =  this.AddInternal(bo);
                   ReflectionUtilities.SetPrivatePropertyValue(this, "Loading", false);
                }
            }
            if (addEventRequired) FireBusinessObjectAdded(bo);
        }

        protected virtual void SavedEventHandler(object sender, BOEventArgs e)
        {
            bool addEventRequired = false;
            TBusinessObject bo;
            lock (KeyObjectHashTable)
            {
                bo = e.BusinessObject as TBusinessObject;
                if (bo == null) return;
                CreatedBusinessObjects.Remove(bo);
                if (this.RemovedBusinessObjects.Remove(bo)) return;
                if (!this.Contains(bo))
                {
                    Criteria criteria = this.SelectQuery.Criteria;
                    if ((criteria == null ) || (criteria.IsMatch(bo)))
                    {
                        addEventRequired = AddInternal(bo);
                    }
                }
                if (!bo.Status.IsNew && !bo.Status.IsDeleted && !this.PersistedBusinessObjects.Contains(bo))
                {
                    AddToPersistedCollection(bo);
                }
            }
            if (addEventRequired) FireBusinessObjectAdded(bo);
        }

        private void UpdatedEventHandler(object sender, BOEventArgs e)
        {
            TBusinessObject businessObject;
            lock (KeyObjectHashTable)
            {
                businessObject = (TBusinessObject) e.BusinessObject;
                if (!this.Contains(businessObject)) return;
            }
            FireBusinessObjectUpdated(businessObject);

        }

        private void BOPropUpdatedEventHandler(object sender, BOPropUpdatedEventArgs propEventArgs)
        {
            TBusinessObject businessObject;
            lock (KeyObjectHashTable)
            {
                businessObject = propEventArgs.BusinessObject as TBusinessObject;
                
                if (!this.Contains(businessObject)) return;
            }
            FireBusinessObjectPropertyUpdated(businessObject, propEventArgs.Prop);
        }

        private void BOIDUpdatedEventHandler(object sender, BOEventArgs e)
        {
            TBusinessObject businessObject;
            lock (KeyObjectHashTable)
            {
                businessObject = (TBusinessObject) e.BusinessObject;
                if (!this.Contains(businessObject)) return;
                if (businessObject.ID == null) return;

                Guid previousObjectID = businessObject.ID.PreviousObjectID;
                if (KeyObjectHashTable.Contains(previousObjectID))
                {
                    KeyObjectHashTable.Remove(previousObjectID);
                    KeyObjectHashTable.Add(businessObject.ID.ObjectID, businessObject);
                }
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
        /// Loads business objects that match the search criteria provided, 
        /// loaded in the order specified, and limiting the number of objects loaded. 
        /// The limited list of <see cref="TBusinessObject"/>s specified as follows:
        /// If you want record 6 to 15 then 
        /// <see cref="firstRecordToLoad"/> will be set to 5 (this is zero based) and 
        /// <see cref="numberOfRecordsToLoad"/> will be set to 10.
        /// This will load 10 records, starting at record 6 of the ordered set (ordered by the <see cref="orderByClause"/>).
        /// If there are fewer than 15 records in total, then the remaining records after record 6 willbe returned. 
        /// </summary>
        /// <remarks>
        /// As a design decision, we have elected for the <see cref="firstRecordToLoad"/> to be zero based since this is consistent with the limit clause in used by MySql etc.
        /// Also, the <see cref="numberOfRecordsToLoad"/> returns the specified number of records unless its value is '-1' where it will 
        /// return all the remaining records from the specified <see cref="firstRecordToLoad"/>.
        /// If you give '0' as the value for the <see cref="numberOfRecordsToLoad"/> parameter, it will load zero records.
        /// </remarks>
        /// <example>
        /// The following code demonstrates how to loop through the invoices in the data store, 
        /// ten at a time, and print their details:
        /// <code>
        /// BusinessObjectCollection&lt;Invoice&gt; col = new BusinessObjectCollection&lt;Invoice&gt;();
        /// int interval = 10;
        /// int firstRecord = 0;
        /// int totalNoOfRecords = firstRecord + 1;
        /// while (firstRecord &lt; totalNoOfRecords)
        /// {
        ///     col.LoadWithLimit("", "InvoiceNo", firstRecord, interval, out totalNoOfRecords);
        ///     Debug.Print("The next {0} invoices:", interval);
        ///     col.ForEach(bo =&gt; Debug.Print(bo.ToString()));
        ///     firstRecord += interval;
        /// }</code>
        /// </example>
        /// <param name="searchCriteria">The search criteria</param>
        /// <param name="orderByClause">The order-by clause</param>
        /// <param name="firstRecordToLoad">The first record to load (NNB: this is zero based)</param>
        /// <param name="numberOfRecordsToLoad">The number of records to be loaded</param>
        /// <param name="totalNoOfRecords">The total number of records matching the criteria</param>
        public void LoadWithLimit(Criteria searchCriteria, OrderCriteria orderByClause, 
            int firstRecordToLoad, int numberOfRecordsToLoad, out int totalNoOfRecords)
        {
            this.SelectQuery.Criteria = searchCriteria;
            this.SelectQuery.OrderCriteria = orderByClause;
            this.SelectQuery.FirstRecordToLoad = firstRecordToLoad;
            this.SelectQuery.Limit = numberOfRecordsToLoad;
            Refresh();
            totalNoOfRecords = TotalCountAvailableForPaging;
        }

        /// <summary>
        /// Loads business objects that match the search criteria provided, 
        /// loaded in the order specified, and limiting the number of objects loaded. 
        /// The limited list of <see cref="TBusinessObject"/>s specified as follows:
        /// If you want record 6 to 15 then 
        /// <see cref="firstRecordToLoad"/> will be set to 5 (this is zero based) and 
        /// <see cref="numberOfRecordsToLoad"/> will be set to 10.
        /// This will load 10 records, starting at record 6 of the ordered set (ordered by the <see cref="orderByClause"/>).
        /// If there are fewer than 15 records in total, then the remaining records after record 6 willbe returned. 
        /// </summary>
        /// <remarks>
        /// As a design decision, we have elected for the <see cref="firstRecordToLoad"/> to be zero based since this is consistent with the limit clause in used by MySql etc.
        /// Also, the <see cref="numberOfRecordsToLoad"/> returns the specified number of records unless its value is '-1' where it will 
        /// return all the remaining records from the specified <see cref="firstRecordToLoad"/>.
        /// If you give '0' as the value for the <see cref="numberOfRecordsToLoad"/> parameter, it will load zero records.
        /// </remarks>
        /// <example>
        /// The following code demonstrates how to loop through the invoices in the data store, 
        /// ten at a time, and print their details:
        /// <code>
        /// BusinessObjectCollection&lt;Invoice&gt; col = new BusinessObjectCollection&lt;Invoice&gt;();
        /// int interval = 10;
        /// int firstRecord = 0;
        /// int totalNoOfRecords = firstRecord + 1;
        /// while (firstRecord &lt; totalNoOfRecords)
        /// {
        ///     col.LoadWithLimit("", "InvoiceNo", firstRecord, interval, out totalNoOfRecords);
        ///     Debug.Print("The next {0} invoices:", interval);
        ///     col.ForEach(bo =&gt; Debug.Print(bo.ToString()));
        ///     firstRecord += interval;
        /// }</code>
        /// </example>
        /// <param name="searchCriteria">The search criteria</param>
        /// <param name="orderByClause">The order-by clause</param>
        /// <param name="firstRecordToLoad">The first record to load (NNB: this is zero based)</param>
        /// <param name="numberOfRecordsToLoad">The number of records to be loaded</param>
        /// <param name="totalNoOfRecords">The total number of records matching the criteria</param>
        public void LoadWithLimit(string searchCriteria, string orderByClause, 
            int firstRecordToLoad, int numberOfRecordsToLoad, out int totalNoOfRecords)
        {
            Criteria criteria = null;
            if (searchCriteria.Length > 0)
            {
                criteria = CriteriaParser.CreateCriteria(searchCriteria);
                QueryBuilder.PrepareCriteria(this.ClassDef, criteria);
            }
            OrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(this.ClassDef, orderByClause);
            LoadWithLimit(criteria, orderCriteria, firstRecordToLoad, numberOfRecordsToLoad, out totalNoOfRecords);
        }

        #endregion

        /// <summary>
        /// Clears the collection
        /// </summary>
        public new void Clear()
        {
            lock (KeyObjectHashTable)
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
        }


        /// <summary>
        /// Removes the business object at the index position specified
        /// </summary>
        /// <param name="index">The index position to remove from</param>
        public new void RemoveAt(int index)
        {
            lock (KeyObjectHashTable)
            {
                TBusinessObject boToRemove = this[index];
                Remove(boToRemove);
            }

        }

        /// <summary>
        /// Removes the specified business object from the collection
        /// </summary>
        /// <param name="bo">The business object to remove</param>
        public new virtual bool Remove(TBusinessObject bo)
        {
            bool fireEvent;
            bool success = RemoveInternal(bo, out fireEvent);
            if (fireEvent) FireBusinessObjectRemoved(bo);
            return success;
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
        /// <param name="fireEvent">Indicates whether to fire a removed event after calling this method.</param>
        /// <returns></returns>
        private bool RemoveInternal(TBusinessObject businessObject, out bool fireEvent)
        {
            fireEvent = false;
            bool removed = base.Remove(businessObject);
            KeyObjectHashTable.Remove(businessObject.ID.ObjectID);

            if (!_removedBusinessObjects.Contains(businessObject)
                && !_markedForDeleteBusinessObjects.Contains(businessObject))
            {
                _removedBusinessObjects.Add(businessObject);
                fireEvent = true;
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
                TBusinessObject bo = (TBusinessObject)KeyObjectHashTable[key];
                return this.Contains(bo) ? bo : null;
            }
            return null;
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
        IBusinessObject IBusinessObjectCollection.Find(Guid key)
        {
            return Find(key);
        }

        /// <summary>
        /// Finds a business object in the collection that contains the
        /// specified Guid ID
        /// </summary>
        /// <param name="searchTerm">The Guid to search for</param>
        /// <returns>Returns the business object if found, or null if not
        /// found</returns>
        [Obsolete("Please use Find(Guid key) instead")]
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
                bo.CancelEdits();
            }
            while (this.MarkedForDeleteBusinessObjects.Count > 0)
            {
                TBusinessObject bo = this.MarkedForDeleteBusinessObjects[0];
                bo.CancelEdits();
            }
            while (this.CreatedBusinessObjects.Count > 0)
            {
                bool fireEvent;
                TBusinessObject bo;
                lock (KeyObjectHashTable)
                {
                    bo = this.CreatedBusinessObjects[0];
                    this.CreatedBusinessObjects.Remove(bo);
                    bo.CancelEdits();
                    
                    this.RemoveInternal(bo, out fireEvent);
                    this.RemovedBusinessObjects.Remove(bo);
                    DeRegisterBOEvents(bo);
                }
                if (fireEvent) FireBusinessObjectRemoved(bo);
            }
            while (this.RemovedBusinessObjects.Count > 0)
            {
                bool fireEvent;
                TBusinessObject bo;
                lock (KeyObjectHashTable)
                {
                    bo = this.RemovedBusinessObjects[0];
                    this.RemovedBusinessObjects.Remove(bo);
                    bo.CancelEdits();
                    fireEvent = this.AddInternal(bo);
                }
                if (fireEvent) FireBusinessObjectAdded(bo);
            }
            while (this.AddedBusinessObjects.Count > 0)
            {
                bool removed;
                TBusinessObject bo;
                lock (KeyObjectHashTable)
                {
                    bo = this.AddedBusinessObjects[0];
                    this.AddedBusinessObjects.Remove(bo);
                    bo.CancelEdits();
                    removed = base.Remove(bo);
                    KeyObjectHashTable.Remove(bo.ID.ToString());
                    this.RemovedBusinessObjects.Remove(bo);
                }
                if (removed)
                {
                    this.FireBusinessObjectRemoved(bo);
                }
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

        ///<summary>
        /// This property is used to return the total number of records available for paging.
        /// It is set internally by the loader when the collection is being loaded.
        ///</summary>
        public int TotalCountAvailableForPaging { get; set; }
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
            lock (KeyObjectHashTable)
            {
                newBO = CreateNewBusinessObject();
                AddCreatedBusinessObject(newBO);
            }
            FireBusinessObjectAdded(newBO);
            return newBO;
        }
        /// <summary>
        /// Creates a new <see cref="TBusinessObject"/> for this BusinessObjectCollection.
        /// The new BusinessObject is not added in to the collection.
        /// </summary>
        /// <returns>A new <see cref="TBusinessObject"/>.</returns>
        protected virtual TBusinessObject CreateNewBusinessObject()
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
                    newBO = (TBusinessObject)
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
            return newBO;
        }

        /// <summary>
        /// Adds a new bo to the collection.  If you call this you must call <see cref="FireBusinessObjectAdded"/> to raise the event (outside of the lock)
        /// This should be called from inside a lock
        /// </summary>
        /// <param name="newBO"></param>
        private void AddCreatedBusinessObject(TBusinessObject newBO)
        {
            CreatedBusinessObjects.Add(newBO);
            if (this.Contains(newBO)) return;

            AddWithoutEvents(newBO);
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

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            lock (KeyObjectHashTable)
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
        }

        ///<summary>
        /// Marks the business object as MarkedForDeletion and places the 
        ///</summary>
        ///<param name="businessObject"></param>
        public void MarkForDelete(TBusinessObject businessObject)
        {
            if (businessObject.Status.IsNew)
            {
                //Remove object from collection and created col and set state as permanently deleted
            }
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
