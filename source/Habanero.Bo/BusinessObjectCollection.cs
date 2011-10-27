//  ---------------------------------------------------------------------------------
//   Copyright (C) 2007-2010 Chillisoft Solutions
//   
//   This file is part of the Habanero framework.
//   
//       Habanero is a free framework: you can redistribute it and/or modify
//       it under the terms of the GNU Lesser General Public License as published by
//       the Free Software Foundation, either version 3 of the License, or
//       (at your option) any later version.
//   
//       The Habanero framework is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU Lesser General Public License for more details.
//   
//       You should have received a copy of the GNU Lesser General Public License
//       along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//  ---------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Comparer;
using Habanero.Util;

namespace Habanero.BO
{
    /// <summary>
    /// This is an interface used for loading BusinessObjects from the Database.
    /// </summary>
    internal interface IBusinessObjectCollectionInternal : IBusinessObjectCollection
    {
        void AddInternal(IBusinessObject businessObject);
        bool Loading { get; set; }
        void FireRefreshedEvent();
        void ClearCurrentCollection();
    }
    /// <summary>
    /// This should never be used by a client application it is used by the DB Loading to clear collections
    /// via the <see cref="IBusinessObjectCollectionInternal"/>
    /// etc and it is used by testing applications to call same methods.
    /// I.e. This is a Hack_ used so that we can call methods that we do not want as public methods
    /// on the Collection and Relationship but which we need to be able to call during Loading and/or Unit Testing.
    /// </summary>
    public static class BOColLoaderHelper
    {
        /// <summary>
        /// Calls the <see cref="IBusinessObjectCollectionInternal"/>'s ClearCurrentCollectionMethod
        /// </summary>
        /// <param name="col">A collection that implements the IBusinessObjectCollectionInternal</param>
        public static void ClearCurrentCollection(IBusinessObjectCollection col)
        {
            IBusinessObjectCollectionInternal internalBOCol = (IBusinessObjectCollectionInternal) col;
            internalBOCol.ClearCurrentCollection();
        }
        /// <summary>
        /// Calls the <see cref="IBusinessObjectCollectionInternal"/>'s FireRefreshedEvent
        /// </summary>
        /// <param name="col">A collection that implements the IBusinessObjectCollectionInternal</param>
        public static void FireRefreshedEvent(IBusinessObjectCollection col)
        {
            IBusinessObjectCollectionInternal internalBOCol = (IBusinessObjectCollectionInternal) col;
            internalBOCol.FireRefreshedEvent();
        }
        /// <summary>
        /// Calls the <see cref="IBusinessObjectCollectionInternal"/>'s Initialise
        /// </summary>
        /// <param name="relationship">A relationship that implements the IRelationshipForLoading</param>
        public static void Initialise(IRelationship relationship)
        {
            IRelationshipForLoading internalRelForLoading = (IRelationshipForLoading)relationship;
            internalRelForLoading.Initialise();
        }
        /// <summary>
        /// Calls the <see cref="IBusinessObjectCollectionInternal"/>'s Loading Property and returns result
        /// </summary>
        /// <param name="col">A collection that implements the IBusinessObjectCollectionInternal</param>
        public static bool GetLoading(IBusinessObjectCollection col)
        {
            IBusinessObjectCollectionInternal internalBOCol = (IBusinessObjectCollectionInternal) col;
            return internalBOCol.Loading;
        }

        /// <summary>
        /// Sets the <see cref="IBusinessObjectCollectionInternal"/>'s Loading Property and returns result
        /// </summary>
        /// <param name="col">A collection that implements the IBusinessObjectCollectionInternal</param>
        /// <param name="loading">The loading value to be set</param>
        public static void SetLoading(IBusinessObjectCollection col, bool loading)
        {
            IBusinessObjectCollectionInternal internalBOCol = (IBusinessObjectCollectionInternal) col;
            internalBOCol.Loading = loading;
        }
    }

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
        : IList<TBusinessObject>, IBusinessObjectCollectionInternal, ISerializable
        where TBusinessObject : class, IBusinessObject, new()
    {
        private const string COUNT = "Count";
        private const string CLASS_NAME = "ClassName";
        private const string ASSEMBLY_NAME = "AssemblyName";
        private const string CREATED_COUNT = "CreatedCount";
        private const string PERSISTED_COUNT = "PersistedCount";
        private const string ADDED_COUNT = "AddedCount";
        private const string REMOVED_COUNT = "RemovedCount";
        private const string MARKEDFORDELETE_COUNT = "MarkedForDeleteCount";
        private const string BUSINESS_OBJECT = "bo";
        private const string CREATED_BUSINESS_OBJECT = "createdbo";
        private const string PERSISTED_BUSINESS_OBJECT = "persistedbo";
        private const string ADDED_BUSINESS_OBJECT = "addedbo";
        private const string REMOVED_BUSINESS_OBJECT = "removedbo";
        private const string MARKEDFORDELETE_BUSINESS_OBJECT = "markedfordeletebo";
        protected readonly List<TBusinessObject> _boCol = new List<TBusinessObject>();

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

        /// <summary> Collection of Business Objects Removed From this collection </summary>
        protected readonly List<TBusinessObject> _removedBusinessObjects = new List<TBusinessObject>();

        private readonly List<TBusinessObject> _addedBusinessObjects = new List<TBusinessObject>();
        private readonly List<TBusinessObject> _markedForDeleteBusinessObjects = new List<TBusinessObject>();
        private EventHandler<BOEventArgs> _savedEventHandler;
        private EventHandler<BOEventArgs> _deletedEventHandler;
        private EventHandler<BOEventArgs> _restoredEventHandler;
        private EventHandler<BOEventArgs> _markForDeleteEventHandler;
        private EventHandler<BOEventArgs> _updatedEventHandler;
        private EventHandler<BOPropUpdatedEventArgs> _boPropUpdatedEventHandler;
        private EventHandler<BOEventArgs> _boIDUpdatedEventHandler;

        private ISelectQuery _selectQuery;

        /// <summary>
        /// Default constructor. 
        /// The <see cref="IClassDef"/> will be implied from <typeparamref name="TBusinessObject"/> and the Current Database Connection will be used.
        /// </summary>
        public BusinessObjectCollection() : this(null, null)
        {
        }

        /// <summary>
        /// Use this constructor if you will only know <typeparamref name="TBusinessObject"/> at run time - <see cref="BusinessObject"/> will be the generic type
        /// and the objects in the collection will be determined from the <see cref="IClassDef"/> passed in.
        /// </summary>
        /// <param name="classDef">The <see cref="IClassDef"/> of the objects to be contained in this collection</param>
        public BusinessObjectCollection(IClassDef classDef) : this(classDef, null)
        {
        }

        /// <summary>
        /// Constructor to initialize a new collection with a
        /// class definition provided by an existing business object
        /// </summary>
        /// <param name="bo">The business object whose class definition
        /// is used to initialize the collection</param>
        [Obsolete("Please initialize with a ClassDef instead.  This option will be removed in later versions of Habanero.")]
        public BusinessObjectCollection(TBusinessObject bo) : this(null, bo)
        {
        }


        private BusinessObjectCollection(IClassDef classDef, TBusinessObject sampleBo)
        {
            Initialise(classDef, sampleBo);
            SetupEventHandlers();
        }

        private void SetupEventHandlers() {
            this._savedEventHandler = SavedEventHandler;
            this._deletedEventHandler = DeletedEventHandler;
            this._restoredEventHandler = RestoredEventHandler;
            this._markForDeleteEventHandler = MarkForDeleteEventHandler;
            this._updatedEventHandler = UpdatedEventHandler;
            this._boPropUpdatedEventHandler = BOPropUpdatedEventHandler;
            this._boIDUpdatedEventHandler = BOIDUpdatedEventHandler;
        }

        private void Initialise(IClassDef classDef, TBusinessObject sampleBo)
        {
            _boClassDef = classDef ?? (sampleBo != null ? sampleBo.ClassDef : null);
            KeyObjectHashTable = new Hashtable();
        }

        /// <summary>
        /// Reconstitutes the collection from a stream that was serialized.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected BusinessObjectCollection(SerializationInfo info, StreamingContext context)
        {
            int count = info.GetInt32(COUNT);
            
            Type classType = Util.TypeLoader.LoadType(info.GetString(ASSEMBLY_NAME), info.GetString(CLASS_NAME));
            this.Initialise(ClassDefinition.ClassDef.ClassDefs[classType], null);
            SetupEventHandlers();
            for (int i = 0; i < count; i++)
            {
                TBusinessObject businessObject = (TBusinessObject) info.GetValue(BUSINESS_OBJECT + i, typeof (TBusinessObject));
                this.AddWithoutEvents(businessObject);
                RegisterBOEvents(businessObject);
            }
            int removedCount = info.GetInt32(REMOVED_COUNT);
            for (int i = 0; i < removedCount; i++)
            {
                TBusinessObject businessObject = (TBusinessObject)info.GetValue(REMOVED_BUSINESS_OBJECT + i, typeof(TBusinessObject));
                this.RemovedBusinessObjects.Add(businessObject);
                //RegisterBOEvents(businessObject);
            }
            int markedForDeleteCount = info.GetInt32(MARKEDFORDELETE_COUNT);
            for (int i = 0; i < markedForDeleteCount; i++)
            {
                TBusinessObject businessObject = (TBusinessObject)info.GetValue(MARKEDFORDELETE_BUSINESS_OBJECT + i, typeof(TBusinessObject));
                this.MarkedForDeleteBusinessObjects.Add(businessObject);
                //RegisterBOEvents(businessObject);
            }
            var boManager = BORegistry.BusinessObjectManager;
            int createdCount = info.GetInt32(CREATED_COUNT);
            for (int i = 0; i < createdCount; i++)
            {
                Guid createdID = (Guid)info.GetValue(CREATED_BUSINESS_OBJECT + i, typeof (Guid));
                this.AddCreatedBusinessObject((TBusinessObject)boManager[createdID]);
            }
            int persistedCount = info.GetInt32(PERSISTED_COUNT);
            for (int i = 0; i < persistedCount; i++)
            {
                Guid persistedID = (Guid)info.GetValue(PERSISTED_BUSINESS_OBJECT + i, typeof (Guid));
                this.AddToPersistedCollection((TBusinessObject)boManager[persistedID]);
            }
            int addedCount = info.GetInt32(ADDED_COUNT);
            for (int i = 0; i < addedCount; i++)
            {
                Guid addedID = (Guid)info.GetValue(ADDED_BUSINESS_OBJECT + i, typeof(Guid));
                this.AddedBusinessObjects.Add((TBusinessObject)boManager[addedID]);
            }
        }


        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            lock (KeyObjectHashTable)
            {
                info.AddValue(CLASS_NAME, this.ClassDef.ClassName);
                info.AddValue(ASSEMBLY_NAME, this.ClassDef.AssemblyName);
                info.AddValue(COUNT, _boCol.Count);
                int count = 0;
                foreach (TBusinessObject businessObject in this)
                {
                    info.AddValue(BUSINESS_OBJECT + count, businessObject);
                    count++;
                }
                info.AddValue(REMOVED_COUNT, this.RemovedBusinessObjects.Count);
                int removedCount = 0;
                foreach (TBusinessObject removedBusinessObject in this.RemovedBusinessObjects)
                {
                    info.AddValue(REMOVED_BUSINESS_OBJECT + removedCount, removedBusinessObject);
                    removedCount++;
                }
                info.AddValue(MARKEDFORDELETE_COUNT, this.MarkedForDeleteBusinessObjects.Count);
                int markedForDeleteCount = 0;
                foreach (TBusinessObject markedForDeleteBusinessObject in this.MarkedForDeleteBusinessObjects)
                {
                    info.AddValue(MARKEDFORDELETE_BUSINESS_OBJECT + markedForDeleteCount, markedForDeleteBusinessObject);
                    markedForDeleteCount++;
                }
                info.AddValue(CREATED_COUNT, this.CreatedBusinessObjects.Count);
                int createdCount = 0;
                foreach (TBusinessObject createdBusinessObject in this.CreatedBusinessObjects)
                {
                    info.AddValue(CREATED_BUSINESS_OBJECT + createdCount, createdBusinessObject.ID.ObjectID);
                    createdCount++;
                }
                info.AddValue(PERSISTED_COUNT, this.PersistedBusinessObjects.Count);
                int persistedCount = 0;
                foreach (TBusinessObject persistedBusinessObject in this.PersistedBusinessObjects)
                {
                    info.AddValue(PERSISTED_BUSINESS_OBJECT + persistedCount, persistedBusinessObject.ID.ObjectID);
                    persistedCount++;
                }
                info.AddValue(ADDED_COUNT, this.AddedBusinessObjects.Count);
                int addedCount = 0;
                foreach (TBusinessObject addedBusinessObject in this.AddedBusinessObjects)
                {
                    info.AddValue(ADDED_BUSINESS_OBJECT + addedCount, addedBusinessObject.ID.ObjectID);
                    addedCount++;
                }
            }
        }


        #region Events and event handlers

        private readonly IDictionary<EventHandler<BOEventArgs>, EventHandler<BOEventArgs<TBusinessObject>>>
            _updatedHandlers = new Dictionary<EventHandler<BOEventArgs>, EventHandler<BOEventArgs<TBusinessObject>>>();

        private readonly IDictionary<EventHandler<BOEventArgs>, EventHandler<BOEventArgs<TBusinessObject>>>
            _addedHandlers = new Dictionary<EventHandler<BOEventArgs>, EventHandler<BOEventArgs<TBusinessObject>>>();

        private readonly IDictionary<EventHandler<BOEventArgs>, EventHandler<BOEventArgs<TBusinessObject>>>
            _removedHandlers = new Dictionary<EventHandler<BOEventArgs>, EventHandler<BOEventArgs<TBusinessObject>>>();

        private readonly IDictionary<EventHandler<BOEventArgs>, EventHandler<BOEventArgs<TBusinessObject>>>
            _idUpdatedHandlers = new Dictionary<EventHandler<BOEventArgs>, EventHandler<BOEventArgs<TBusinessObject>>>();

        private readonly
            IDictionary<EventHandler<BOPropUpdatedEventArgs>, EventHandler<BOPropUpdatedEventArgs<TBusinessObject>>>
            _propertyUpdatedHandlers =
                new Dictionary
                    <EventHandler<BOPropUpdatedEventArgs>, EventHandler<BOPropUpdatedEventArgs<TBusinessObject>>>();

        private event EventHandler<BOEventArgs<TBusinessObject>> _BusinessObjectRemoved;
        private event EventHandler<BOEventArgs<TBusinessObject>> _businessObjectAdded;
        private event EventHandler<BOEventArgs<TBusinessObject>> _businessObjectUpdated;
        private event EventHandler<BOEventArgs<TBusinessObject>> _BusinessObjectIDUpdated;
        private event EventHandler<BOPropUpdatedEventArgs<TBusinessObject>> _BusinessObjectPropertyUpdated;

        /// <summary>
        /// Event Fires whenever the Collection is Refreshed.
        /// </summary>
        public event EventHandler CollectionRefreshed;

        /// <summary>
        /// Handles the event of a business object being added
        /// </summary>
        event EventHandler<BOEventArgs> IBusinessObjectCollection.BusinessObjectAdded
        {
            add
            {
                EventHandler<BOEventArgs<TBusinessObject>> addedHandler =
                    new EventHandler<BOEventArgs<TBusinessObject>>(value);
                _addedHandlers[value] = addedHandler;
                _businessObjectAdded += addedHandler;
            }
            remove
            {
                if (_addedHandlers.ContainsKey(value))
                {
                    _businessObjectAdded -= _addedHandlers[value];
                    _addedHandlers.Remove(value);
                }
            }
        }

        /// <summary>
        /// Handles the event of a business object being added
        /// </summary>
        public event EventHandler<BOEventArgs<TBusinessObject>> BusinessObjectAdded
        {
            add { _businessObjectAdded += value; }
            remove { _businessObjectAdded -= value; }
        }

        /// <summary>
        /// Handles the event of a business object being removed
        /// </summary>
        event EventHandler<BOEventArgs> IBusinessObjectCollection.BusinessObjectRemoved
        {
            add
            {
                EventHandler<BOEventArgs<TBusinessObject>> removedHandler =
                    new EventHandler<BOEventArgs<TBusinessObject>>(value);
                _removedHandlers[value] = removedHandler;
                _BusinessObjectRemoved += removedHandler;
            }
            remove
            {
                if (_removedHandlers.ContainsKey(value))
                {
                    _BusinessObjectRemoved -= _removedHandlers[value];
                    _removedHandlers.Remove(value);
                }
            }
        }

        /// <summary>
        /// Handles the event of a business object being removed
        /// </summary>
        public event EventHandler<BOEventArgs<TBusinessObject>> BusinessObjectRemoved
        {
            add { _BusinessObjectRemoved += value; }
            remove { _BusinessObjectRemoved -= value; }
        }


        /// <summary>
        /// Handles the event of any business object in this collection being Updated(i.e the BO is saved, or edits are canceled).
        /// See the <see cref="IBusinessObject"/>.<see cref="IBusinessObject.Updated"/> event.
        /// </summary>
        event EventHandler<BOEventArgs> IBusinessObjectCollection.BusinessObjectUpdated
        {
            add
            {
                EventHandler<BOEventArgs<TBusinessObject>> updatedHandler =
                    new EventHandler<BOEventArgs<TBusinessObject>>(value);
                _updatedHandlers[value] = updatedHandler;
                _businessObjectUpdated += updatedHandler;
            }
            remove
            {
                if (_updatedHandlers.ContainsKey(value))
                {
                    _businessObjectUpdated -= _updatedHandlers[value];
                    _updatedHandlers.Remove(value);
                }
            }
        }

        /// <summary>
        /// Handles the event of any business object in this collection being Updated(i.e the BO is saved, or edits are canceled).
        /// See the <see cref="IBusinessObject"/>.<see cref="IBusinessObject.Updated"/> event.
        /// </summary>
        public event EventHandler<BOEventArgs<TBusinessObject>> BusinessObjectUpdated
        {
            add { _businessObjectUpdated += value; }
            remove { _businessObjectUpdated -= value; }
        }

        /// <summary>
        /// Handles the event of any business object in this collection being edited (i.e. a property value is changed).
        /// See the <see cref="IBusinessObject"/>.<see cref="IBusinessObject.PropertyUpdated"/> event.
        /// </summary>
        event EventHandler<BOPropUpdatedEventArgs> IBusinessObjectCollection.BusinessObjectPropertyUpdated
        {
            add
            {
                EventHandler<BOPropUpdatedEventArgs<TBusinessObject>> propertyUpdatedHandler =
                    new EventHandler<BOPropUpdatedEventArgs<TBusinessObject>>(value);
                _propertyUpdatedHandlers[value] = propertyUpdatedHandler;
                _BusinessObjectPropertyUpdated += propertyUpdatedHandler;
            }
            remove
            {
                if (_propertyUpdatedHandlers.ContainsKey(value))
                {
                    _BusinessObjectPropertyUpdated -= _propertyUpdatedHandlers[value];
                    _propertyUpdatedHandlers.Remove(value);
                }
            }
        }

        /// <summary>
        /// Handles the event of any business object in this collection being edited (i.e. a property value is changed).
        /// See the <see cref="IBusinessObject"/>.<see cref="IBusinessObject.PropertyUpdated"/> event.
        /// </summary>
        public event EventHandler<BOPropUpdatedEventArgs<TBusinessObject>> BusinessObjectPropertyUpdated
        {
            add { _BusinessObjectPropertyUpdated += value; }
            remove { _BusinessObjectPropertyUpdated -= value; }
        }


        /// <summary>
        /// Handles the event when a <c>BusinessObject</c> in the collection has an ID that is Updated(i.e one of the properties of the ID is edited).
        /// </summary>
        event EventHandler<BOEventArgs> IBusinessObjectCollection.BusinessObjectIDUpdated
        {
            add
            {
                EventHandler<BOEventArgs<TBusinessObject>> idUpdatedHandler =
                    new EventHandler<BOEventArgs<TBusinessObject>>(value);
                _idUpdatedHandlers[value] = idUpdatedHandler;
                _BusinessObjectIDUpdated += idUpdatedHandler;
            }
            remove
            {
                if (_idUpdatedHandlers.ContainsKey(value))
                {
                    _BusinessObjectIDUpdated -= _idUpdatedHandlers[value];
                    _idUpdatedHandlers.Remove(value);
                }
            }
        }

        /// <summary>
        /// Handles the event when a <c>BusinessObject</c> in the collection has an ID that is Updated(i.e one of the properties of the ID is edited).
        /// </summary>
        public event EventHandler<BOEventArgs<TBusinessObject>> BusinessObjectIDUpdated
        {
            add { _BusinessObjectIDUpdated += value; }
            remove { _BusinessObjectIDUpdated -= value; }
        }

        /// <summary>
        /// Calls the <see cref="BusinessObjectAdded"/>() handler
        /// </summary>
        /// <param name="bo">The business object added</param>
        private void FireBusinessObjectAdded(TBusinessObject bo)
        {
            if (_businessObjectAdded != null)
            {
                _businessObjectAdded(this, new BOEventArgs<TBusinessObject>(bo));
            }
        }

        // ReSharper disable UnusedMember.Local
        //This is used by the BusinessObjectLoader when it has finished loading the Collection
        // ReSharper disable UnusedMember.Global
        void IBusinessObjectCollectionInternal.FireRefreshedEvent()
        {
            if (CollectionRefreshed != null)
            {
                CollectionRefreshed(this, new EventArgs());
            }
        }

        // ReSharper restore UnusedMember.Global 
        // ReSharper restore UnusedMember.Local
        /// <summary>
        /// Calls the <see cref="BusinessObjectRemoved"/>() handler
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
            if (_businessObjectUpdated != null)
            {
                _businessObjectUpdated(this, new BOEventArgs<TBusinessObject>(bo));
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

        // ReSharper disable VirtualMemberNeverOverriden.Global
        /// <summary>
        /// Adds a business object to the collection
        /// </summary>
        /// <param name="bo">The business object to add</param>
        /// <exception cref="ArgumentNullException"><c>bo</c> is null.</exception>
        public virtual void Add(TBusinessObject bo)
        {
            if (bo == null) throw new ArgumentNullException("bo");
            bool addSuccessful;
            lock (KeyObjectHashTable)
            {
                addSuccessful = AddInternal(bo);
            }
            if (addSuccessful) this.FireBusinessObjectAdded(bo);
        }

        // ReSharper restore VirtualMemberNeverOverriden.Global
        /// <summary>
        /// Adds a business object to the collection of business objects without raising any events.
        /// But still verifies the object does not exist in Created, removed etc collections
        /// </summary>
        /// <param name="bo">The business object to be added to the collection</param>
        /// <returns>a value indicating whether the object was added to the collection or not.</returns>
        protected virtual bool AddInternal(TBusinessObject bo)
        {
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
            if (bo.ID != null) if (KeyObjectHashTable.Contains(bo.ID.ObjectID)) return;
            _boCol.Add(bo);
            if (bo.ID != null) KeyObjectHashTable.Add(bo.ID.ObjectID, bo);
            RegisterBOEvents(bo);
        }

        private void RegisterBOEvents(TBusinessObject businessObject)
        {
            businessObject.Saved += _savedEventHandler;
            businessObject.Deleted += _deletedEventHandler;
            businessObject.Restored += _restoredEventHandler;
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

        private void MarkForDeleteEventHandler(object sender, BOEventArgs e)
        {
            bool removeSuccessful;
            TBusinessObject bo;
            lock (KeyObjectHashTable)
            {
                bo = e.BusinessObject as TBusinessObject;
                if (bo == null) return;
                if (this.MarkedForDeleteBusinessObjects.Contains(bo)) return;
                if (bo.Status.IsNew)
                {
                    this.CreatedBusinessObjects.Remove(bo);
                }
                else
                {
                    this.MarkedForDeleteBusinessObjects.Add(bo);
                }

                _boCol.Remove(bo);
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
                this.CreatedBusinessObjects.Remove(bo);
                this.PersistedBusinessObjects.Remove(bo);
                this.RemovedBusinessObjects.Remove(bo);
                this.MarkedForDeleteBusinessObjects.Remove(bo);
                if (bo.Status.IsDeleted) this.AddedBusinessObjects.Remove(bo);
                DeRegisterBOEvents(bo);
            }
            if (fireEvent) FireBusinessObjectRemoved(bo);
        }

        /// <summary>
        /// Handles the event of a Business object being restored.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    //ReflectionUtilities.SetPrivatePropertyValue(this, "Loading", true);
                    var boColInternal = ((IBusinessObjectCollectionInternal)this);
                    boColInternal.Loading = true;
                    addEventRequired = this.AddInternal(bo);
                    boColInternal.Loading = false;
                    //ReflectionUtilities.SetPrivatePropertyValue(this, "Loading", false);
                }
            }
            if (addEventRequired) FireBusinessObjectAdded(bo);
        }

        /// <summary>
        /// Handles the event of the Business object becoming invalid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    if ((criteria == null) || (criteria.IsMatch(bo)))
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
            Add((IList<TBusinessObject>) col);
        }

        /// <summary>
        /// Adds the business objects from col into this collection
        /// </summary>
        /// <param name="col"></param>
        public void Add(IEnumerable<TBusinessObject> col)
        {
            if (col == null) throw new ArgumentNullException("col");
            foreach (TBusinessObject bo in col)
            {
                this.Add(bo);
            }
        }

        ///<summary>
        /// Adds the specified business objects to this collection
        ///</summary>
        ///<param name="businessObjects">A parameter array of business objects to add to the collection</param>
        public virtual void Add(params TBusinessObject[] businessObjects)
        {
            Add(new List<TBusinessObject>(businessObjects));
        }

        /// <summary>
        /// Refreshes the business objects in the collection
        /// </summary>
        [ReflectionPermission(SecurityAction.Demand)]
        public virtual void Refresh()
        {
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(this);
        }

        void IBusinessObjectCollectionInternal.AddInternal(IBusinessObject businessObject)
        {
            this.AddInternal((TBusinessObject) businessObject);
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
        /// To load the collection in any order use the <see cref="LoadAll()"/> method.
        /// </summary>
        /// <param name="orderByClause">The order-by clause</param>
        public void LoadAll(string orderByClause)
        {
            Load("", orderByClause);
        }

        /// <summary>
        /// Loads business objects that match the search criteria provided,
        /// loaded in the order specified.  
        /// Use empty quotes, (or the <see cref="LoadAll(string)"/> method) to load the
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
        public virtual void LoadWithLimit(string searchCriteria, string orderByClause, int limit)
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
        public virtual void LoadWithLimit(Criteria searchExpression, string orderByClause, int limit)
        {
            this.SelectQuery.Criteria = searchExpression;

            this.SelectQuery.OrderCriteria = QueryBuilder.CreateOrderCriteria(this.ClassDef, orderByClause);
            if (limit > -1) this.SelectQuery.Limit = limit;

            Refresh();
        }

        /// <summary>
        /// Loads business objects that match the search criteria provided, 
        /// loaded in the order specified, and limiting the number of objects loaded. 
        /// The limited list of TBusinessObjects specified as follows:
        /// If you want record 6 to 15 then 
        /// <paramref name="firstRecordToLoad"/> will be set to 5 (this is zero based) and 
        /// <paramref name="numberOfRecordsToLoad"/> will be set to 10. 
        /// This will load 10 records, starting at record 6 of the ordered set (ordered by the <paramref name="orderByClause"/>).
        /// If there are fewer than 15 records in total, then the remaining records after record 6 will be returned. 
        /// </summary>
        /// <remarks>
        /// As a design decision, we have elected for the <paramref name="firstRecordToLoad"/> to be zero based since this is consistent with the limit clause in used by <c>MySql</c> etc.
        /// Also, the <paramref name="numberOfRecordsToLoad"/> returns the specified number of records unless its value is '-1' where it will 
        /// return all the remaining records from the specified <paramref name="firstRecordToLoad"/>.
        /// If you give '0' as the value for the <paramref name="numberOfRecordsToLoad"/> parameter, it will load zero records.
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
        public virtual void LoadWithLimit(Criteria searchCriteria, IOrderCriteria orderByClause,
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
        /// The limited list of TBusinessObjects specified as follows:
        /// If you want record 6 to 15 then
        /// <paramref name="firstRecordToLoad"/> will be set to 5 (this is zero based) and 
        /// <paramref name="numberOfRecordsToLoad"/> will be set to 10.
        /// This will load 10 records, starting at record 6 of the ordered set (ordered by the <paramref name="orderByClause"/>).
        /// If there are fewer than 15 records in total, then the remaining records after record 6 will be returned. 
        /// </summary>
        /// <remarks>
        /// As a design decision, we have elected for the <paramref name="firstRecordToLoad"/> to be zero based since this is consistent with the limit clause in used by <c>MySql</c> etc.
        /// Also, the <paramref name="numberOfRecordsToLoad"/> returns the specified number of records unless its value is '-1' where it will 
        /// return all the remaining records from the specified <paramref name="firstRecordToLoad"/>.
        /// If you give '0' as the value for the <paramref name="numberOfRecordsToLoad"/> parameter, it will load zero records.
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
        public virtual void LoadWithLimit(string searchCriteria, string orderByClause,
                                  int firstRecordToLoad, int numberOfRecordsToLoad, out int totalNoOfRecords)
        {
            Criteria criteria = null;
            if (!Utilities.IsNull(searchCriteria) && searchCriteria.Length > 0)
            {
                criteria = CriteriaParser.CreateCriteria(searchCriteria);
                QueryBuilder.PrepareCriteria(this.ClassDef, criteria);
            }
            //If there is no orderByClause then we create an order by clause using the ObjectsID
            //This seems a bit strange but is required due to the fact that the load with Limit must be 
            // able to return a Set of objects from the database in exactly the same order else the 
            // paging from one page to another does not make sense.
            if(string.IsNullOrEmpty(orderByClause))
            {
                var classDefCol = Habanero.BO.ClassDefinition.ClassDef.ClassDefs;
                var primaryKeyDef = ClassDefHelper.GetPrimaryKeyDef(this.ClassDef, classDefCol);
                orderByClause = PrimaryKeyAsOrderByClause(primaryKeyDef);
            }
            IOrderCriteria orderCriteria = QueryBuilder.CreateOrderCriteria(this.ClassDef, orderByClause);
            LoadWithLimit(criteria, orderCriteria, firstRecordToLoad, numberOfRecordsToLoad, out totalNoOfRecords);
        }


        private static string PrimaryKeyAsOrderByClause(IPrimaryKeyDef primaryKeyDef)
        {
            string toStringValue = "";

            foreach (IPropDef lPropDef in primaryKeyDef)
            {
                toStringValue = StringUtilities.AppendMessage(toStringValue, lPropDef.PropertyName, ", ");
            }
            return toStringValue;
        }
        #endregion

        int IList.Add(object value)
        {
            this.Add((TBusinessObject) value);
            return this.Count - 1;
        }

        bool IList.Contains(object value)
        {
            return _boCol.Contains((TBusinessObject) value);
        }

        int IList.IndexOf(object value)
        {
            return _boCol.IndexOf((TBusinessObject) value);
        }

        void IList.Insert(int index, object value)
        {
            _boCol.Insert(index, (TBusinessObject) value);
        }

        void IList.Remove(object value)
        {
            _boCol.Remove((TBusinessObject) value);
        }

        object IList.this[int index]
        {
            get { return _boCol[index]; }
            set { _boCol[index] = (TBusinessObject)value; }
        }

        /// <summary>
        /// Clears the collection
        /// </summary>
        public virtual void Clear()
        {
            lock (KeyObjectHashTable)
            {
                DeRegisterBoEventsForAllBusinessObjects();
                _boCol.Clear();
                KeyObjectHashTable.Clear();
                this.PersistedBusinessObjects.Clear();
                this.CreatedBusinessObjects.Clear();
                this.RemovedBusinessObjects.Clear();
                this.AddedBusinessObjects.Clear();
                this.MarkedForDeleteBusinessObjects.Clear();
                this.TimeLastLoaded = null;
            }
        }

        ///<summary>
        ///Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        ///</summary>
        ///<returns>
        ///true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        ///</returns>
        ///<param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public bool Contains(TBusinessObject item)
        {
            return _boCol.Contains(item);
        }

        ///<summary>
        ///Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        ///</summary>
        ///<param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        ///<param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        ///<exception cref="T:System.ArgumentNullException"><paramref name="array" /> is null.</exception>
        ///<exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex" /> is less than 0.</exception>
        ///<exception cref="T:System.ArgumentException"><paramref name="array" /> is multidimensional.-or-<paramref name="arrayIndex" /> is equal to or greater than the length of <paramref name="array" />.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.-or-Type <typeparamref name="TBusinessObject"/> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
        public void CopyTo(TBusinessObject[] array, int arrayIndex)
        {
            _boCol.CopyTo(array, arrayIndex);
        }

        ///<summary>
        ///Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.
        ///</summary>
        ///<returns>
        ///The index of <paramref name="item" /> if found in the list; otherwise, -1.
        ///</returns>
        ///<param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        public int IndexOf(TBusinessObject item)
        {
            return _boCol.IndexOf(item);
        }

        ///<summary>
        ///Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified index.
        ///</summary>
        ///<param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        ///<param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        ///<exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.</exception>
        ///<exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
        public void Insert(int index, TBusinessObject item)
        {
            _boCol.Insert(index, item);
        }

        /// <summary>
        /// Removes the business object at the index position specified
        /// </summary>
        /// <param name="index">The index position to remove from</param>
        public virtual void RemoveAt(int index)
        {
            lock (KeyObjectHashTable)
            {
                TBusinessObject boToRemove = this[index];
                Remove(boToRemove);
            }
        }

        ///<summary>
        ///Gets or sets the element at the specified index.
        ///</summary>
        ///<returns>
        ///The element at the specified index.
        ///</returns>
        ///<param name="index">The zero-based index of the element to get or set.</param>
        ///<exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.</exception>
        ///<exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
        public TBusinessObject this[int index]
        {
            get { return _boCol[index]; }
            set { _boCol[index] = value; }
        }

        /// <summary>
        /// Removes the specified business object from the collection
        /// </summary>
        /// <param name="bo">The business object to remove</param>
        public virtual bool Remove(TBusinessObject bo)
        {
            bool fireEvent;
            bool success = RemoveInternal(bo, out fireEvent);
            if (fireEvent) FireBusinessObjectRemoved(bo);
            return success;
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an
        /// <see cref="T:System.Array"/>, starting at a particular 
        /// <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the 
        /// destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. 
        /// The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins. </param>
        /// <exception cref="System.ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero. </exception>
        /// <exception cref="System.ArgumentException"><paramref name="array"/> is multidimensional. 
        /// -or- <paramref name="index"/> is equal to or greater than the length of <paramref name="array"/>. 
        /// -or-  The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than 
        /// the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>. </exception>
        /// <exception cref="System.ArgumentException">The type of the source 
        /// <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to
        /// the type of the destination <paramref name="array"/>. </exception>
        /// <filterpriority>2</filterpriority>
        public void CopyTo(Array array, int index)
        {
            if (object.ReferenceEquals(array, null))
            {
                throw new ArgumentNullException(
                    "array", "Null array reference"
                    );
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "index","Index is out of range"
                    );
            }

            if (array.Rank > 1)
            {
                throw new ArgumentException(
                     "Array is multi-dimensional","array"
                    );
            }

            foreach (TBusinessObject o in this)
            {
                array.SetValue(o, index);
                index++;
            }
        }

        ///<summary>
        ///Gets the number of elements contained in the <see cref="T:System.Collections.ICollection" />.
        ///</summary>
        ///<returns>
        ///The number of elements contained in the <see cref="T:System.Collections.ICollection" />.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public int Count
        {
            get { return _boCol.Count; }
        }

        private readonly object _mSyncRoot = new object();

        object ICollection.SyncRoot
        {
            get { return _mSyncRoot; }
        }

        bool ICollection.IsSynchronized
        {
            get { return false;}
        }

        int ICollection<TBusinessObject>.Count
        {
            get { return _boCol.Count; }
        }

        bool ICollection<TBusinessObject>.IsReadOnly
        {
            get { return false; }
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        ///<summary>
        /// Clears only the current collection i.e. the persisted, removed, added and created lists 
        ///    are retained.
        ///</summary>
        /// Note_: This is used by reflection by the collection loader.
// ReSharper disable UnusedPrivateMember
// ReSharper disable UnusedMember.Global
        void IBusinessObjectCollectionInternal.ClearCurrentCollection()
        {
            foreach (TBusinessObject businessObject in this)
            {
                DeRegisterBOEvents(businessObject);
            }
            _boCol.Clear();
            KeyObjectHashTable.Clear();
        }
// ReSharper restore UnusedPrivateMember
// ReSharper restore UnusedMember.Global

        /// <summary>
        /// Removes the specified business object from the collection. This is used when refreshing
        /// a collection so that any overridden behavior (from overriding Remove) is not applied
        /// when loading and refreshing.
        /// </summary>
        /// <param name="businessObject"></param>
        /// <param name="fireEvent">Indicates whether to fire a removed event after calling this method.</param>
        /// <returns></returns>
        private bool RemoveInternal(TBusinessObject businessObject, out bool fireEvent)
        {
            fireEvent = false;
            bool removed = _boCol.Remove(businessObject);
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
            errorMessage = string.Empty;
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
        /// <exception cref="HabaneroDeveloperException">A collection's select query cannot be set to null</exception>
        public ISelectQuery SelectQuery
        {
			get { return _selectQuery ?? (_selectQuery = QueryBuilder.CreateSelectQuery(ClassDef)); }
        	set
            {
                if (value == null)
                {
                    throw new HabaneroDeveloperException
                        ("A collection's select query cannot be set to null",
                         "A collection's select query cannot be set to null");
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
        public virtual TBusinessObject Find(Guid key)

        {
            if (KeyObjectHashTable.ContainsKey(key))
            {
                TBusinessObject bo = (TBusinessObject) KeyObjectHashTable[key];
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
        /// specified <see cref="Guid"/> ID
        /// </summary>
        /// <param name="searchTerm">The <see cref="Guid"/> to search for</param>
        /// <returns>Returns the business object if found, or null if not
        /// found</returns>
        [Obsolete("Please use Find(Guid key) instead")]
        public TBusinessObject FindByGuid(Guid searchTerm)
        {
            //string formattedSearchItem = searchTerm.ToString();
            if (KeyObjectHashTable.ContainsKey(searchTerm))
            {
                return (TBusinessObject) KeyObjectHashTable[searchTerm];
            }
            return null;
        }

        /// <summary>
        /// Returns the class definition of the collection
        /// </summary>
        public IClassDef ClassDef
        {
			get { return _boClassDef ?? (_boClassDef = ClassDefinition.ClassDef.ClassDefs[typeof (TBusinessObject)]); }
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
				(ClassDef);
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
        /// <typeparam name="DestType">The <c>BusinessObject</c> type for this collection's objects to be cast to. </typeparam>
        /// <returns>Returns the cloned copy</returns>
        /// <exception cref="InvalidCastException">Cannot cast a collection of type <typeparamref name="TBusinessObject"/>
        /// to a collection of type <typeparamref name="DestType"/>.</exception>
        public BusinessObjectCollection<DestType> Clone<DestType>() where DestType : BusinessObject, new()
        {
			BusinessObjectCollection<DestType> clonedCol = new BusinessObjectCollection<DestType>(ClassDef);
            if (!typeof (DestType).IsSubclassOf(typeof (TBusinessObject))
                && !typeof (TBusinessObject).IsSubclassOf(typeof (DestType))
                && !typeof (TBusinessObject).Equals(typeof (DestType)))
            {
                throw new InvalidCastException
                    (String.Format
                         ("Cannot cast a collection of type '{0}' to a collection of type '{1}'.",
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
        public virtual void Sort(string propertyName, bool isBoProperty, bool isAscending)
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
                _boCol.Reverse();
            }
        }

        ///<summary>
        /// Sorts the Collection using the comparer delegate
        ///</summary>
        ///<param name="comparer">The Delegate used to sort</param>
        public virtual void Sort(IComparer<TBusinessObject> comparer)
        {
            _boCol.Sort(comparer);
        }

        /// <summary>
        /// Sorts the Collection by the Order Criteria Set up during the Loading of this collection.
        /// For <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/>'s this will be the 
        /// Order Criteria set up in the orderBy in the <see cref="IClassDef"/> for 
        /// the <typeparamref name="TBusinessObject"/> type (i.e. in the <c>ClassDef.xml</c>).
        /// </summary>
        public virtual void Sort()
        {
            if (this.SelectQuery.OrderCriteria.Fields.Count > 0)
            {
                _boCol.Sort(new StronglyTypedComperer<TBusinessObject>(this.SelectQuery.OrderCriteria));
            }
        }

        /// <summary>
        /// Sorts the collection by the property specified. The second parameter
        /// indicates whether this property is a business object property or
        /// whether it is a property defined in the code.  For example, a full name
        /// would be a code-calculated property that is not itself a business
        /// object property, even though it uses the BO properties of first name
        /// and surname, and the argument would thus be set as false.
        /// </summary>
        /// <param name="comparer">The property name to sort on</param>
        /// <exception cref="ArgumentNullException"><c>comparer</c> is null.</exception>
        void IBusinessObjectCollection.Sort(IComparer comparer)
        {
            if (comparer == null) throw new ArgumentNullException("comparer");
            _boCol.Sort(new StronglyTypedComperer<TBusinessObject>(comparer));
        }


        /// <summary>
        /// Returns a list containing all the objects sorted by the property
        /// name and in the order specified
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <param name="isAscending">True for ascending, false for descending
        /// </param>
        /// <returns>Returns a sorted list</returns>
        public virtual List<TBusinessObject> GetSortedList(string propertyName, bool isAscending)
        {
            List<TBusinessObject> list = new List<TBusinessObject>(_boCol.Count);
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
        public virtual BusinessObjectCollection<TBusinessObject> GetSortedCollection(string propertyName, bool isAscending)
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
        /// Returns the business object collection as an <see cref="IList"/>.
        /// </summary>
        /// <returns>Returns an <see cref="IList"/> object</returns>
        public virtual List<TBusinessObject> GetList()
        {
            List<TBusinessObject> list = new List<TBusinessObject>(_boCol.Count);
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

        /// <summary>
        /// Adds all the Business objects to the <paramref name="transaction"/> including the 
        /// Business objects in the Added, Created, Removed and Marked for deleted collections.
        /// And then commits the <paramref name="transaction"/> to the DataSource.
        /// </summary>
        /// <param name="transaction"></param>
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
        public virtual void CancelEdits()
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
                    removed = _boCol.Remove(bo);
                    KeyObjectHashTable.Remove(bo.ID.ToString());
                    this.RemovedBusinessObjects.Remove(bo);
                }
                if (removed)
                {
                    this.FireBusinessObjectRemoved(bo);
                }
            }
        }

        /// <summary>
        /// The <see cref="DateTime"/> that the Collection was loaded.
        /// This is used to determine whether the Collection should be Reloaded when 
        /// the MultipleRelationship's get <c>BusinessObjectCollection</c> is called.
        /// </summary>
        /// <summary>
        /// The <see cref="DateTime"/> that the Collection was loaded.
        /// This is used to determine whether the Collection should be Reloaded when 
        /// the MultipleRelationship's get <c>BusinessObjectCollection</c> is called.
        /// </summary>
        public DateTime? TimeLastLoaded { get; set; }

        /// <summary>
        /// Restores all the business objects to their last persisted state, that
        /// is their state and values at the time they were last saved to the database
        /// </summary>
        [Obsolete("Should use Cancel Edits")]
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
            get { return _boCol[index]; }
            set { _boCol[index] = (TBusinessObject) value; }
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
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentNullException">Array is null.</exception>
        /// <exception cref="T:System.ArgumentException">Array is multidimensional or <paramref name="arrayIndex"/>
        /// is equal to or greater than the length of array.-or-The number of elements in
        /// the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is 
        /// greater than the available space from <paramref name="arrayIndex"/> to the end of the destination array, or 
        /// Type T cannot be cast automatically to the type of the destination array.</exception>
        void IBusinessObjectCollection.CopyTo(IBusinessObject[] array, int arrayIndex)
        {
            TBusinessObject[] thisArray = new TBusinessObject[array.LongLength];
            this.CopyTo(thisArray, arrayIndex);
            //array = thisArray;
            int count = _boCol.Count;
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
        bool IList.IsReadOnly
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
        /// The list of business objects that have been created via this collection (see <see cref="CreateBusinessObject"/>) and have not
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
        /// Hack: This method was created returning a type <see cref="IList"/> to overcome problems with 
        ///   <see cref="BusinessObjectCollection{T}"/> being a generic collection.
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
        ///   collection but have not necessarily been persisted to the database.
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
        ///   collection but have not necessarily been persisted to the database.
        /// </summary>
        IList IBusinessObjectCollection.AddedBusinessObjects
        {
            get { return this.AddedBusinessObjects; }
        }

        /// <summary>
        /// The list of business objects that have been added to this Business Object 
        /// collection (<see cref="BusinessObjectCollection{TBusinessObject}.Add(TBusinessObject)"/>) and have not
        /// yet been persisted.
        /// </summary>
        public List<TBusinessObject> AddedBusinessObjects
        {
            get { return _addedBusinessObjects; }
        }

        private Hashtable KeyObjectHashTable { get; set; }

// ReSharper disable UnusedPrivateMember 
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        //This method is used by reflection only
        /// <summary>
        /// This property is set to true while loading the collection from the datastore so as to 
        /// prevent certain checks being done (e.g. Adding persisted business objects to a collection.
        /// </summary>
        bool IBusinessObjectCollectionInternal.Loading { get; set; }

// ReSharper restore UnusedAutoPropertyAccessor.Global
// ReSharper restore UnusedPrivateMember

        ///<summary>
        /// This property is used to return the total number of records available for paging.
        /// It is set internally by the loader when the collection is being loaded.
        ///</summary>
        public int TotalCountAvailableForPaging { get; set; }

        #endregion

        /// <summary>
        /// Creates a business object of type <typeparamref name="TBusinessObject"/>
        /// Adds this BO to the <see cref="CreatedBusinessObjects"/> list. When the object is saved it will
        /// be added to the actual Business Object collection.
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
        /// Creates a new <typeparamref name="TBusinessObject"/> for this <c>BusinessObjectCollection</c>.
        /// The new <typeparamref name="TBusinessObject"/> is not added into the collection.
        /// </summary>
        /// <returns>A new <typeparamref name="TBusinessObject"/></returns>
        /// <exception cref="HabaneroDeveloperException">A <c>BusinessObject</c> of type <typeparamref name="TBusinessObject"/> cannot be created.</exception>
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
                        ("An attempt was made to create a {0} with a customized class def. Please add a constructor that takes a ClassDef as a parameter to the business object class of type {1}.",
                         className, className);
                    throw new HabaneroDeveloperException("There was a problem creating a " + className, msg, ex);
                }
            }
            return newBO;
        }

        /// <summary>
        /// Adds a new <c>BusinessObject</c> to the collection. If you call this you must
        /// call <see cref="FireBusinessObjectAdded"/> to raise the event (outside of the
        /// lock) This should be called from inside a lock
        /// </summary>
        /// <param name="newBO"></param>
        private void AddCreatedBusinessObject(TBusinessObject newBO)
        {
            if (!this.CreatedBusinessObjects.Contains(newBO))
            {
                CreatedBusinessObjects.Add(newBO);
            }
            if (this.Contains(newBO)) return;

            AddWithoutEvents(newBO);
        }

        /// <summary>
        /// Creates a business object of type <typeparamref name="TBusinessObject"/>
        /// Adds this BO to the <see cref="CreatedBusinessObjects"/> list. When the object is saved it will
        /// be added to the actual <see cref="BusinessObject"/> collection.
        /// </summary>
        /// <returns></returns>
        IBusinessObject IBusinessObjectCollection.CreateBusinessObject()
        {
            return CreateBusinessObject();
        }

        ///<summary>
        /// Marks the business object as MarkedForDeletion and places the object
        ///</summary>
        ///<param name="businessObject"></param>
        public virtual void MarkForDelete(TBusinessObject businessObject)
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
        public virtual void MarkForDeleteAt(int index)
        {
            TBusinessObject boToMark = this[index];
            MarkForDelete(boToMark);
        }

        ///<summary>
        ///Returns an enumerator that iterates through the collection.
        ///</summary>
        ///<returns>
        ///A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>1</filterpriority>
        public IEnumerator<TBusinessObject> GetEnumerator()
        {
            return _boCol.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        ///<summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, 
        /// and returns the first occurrence within the entire <see cref="System.Collections.Generic.List{TBusinessObject}"/>.
        ///</summary>
        ///<param name="match">The <see cref="System.Predicate{TBusinessObject}"/> delegate that defines the conditions of the element to search for.</param>
        ///<returns>The first element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type TBusinessObject.</returns>
        /// <exception cref="System.ArgumentNullException">match is null.</exception>
        public TBusinessObject Find(System.Predicate<TBusinessObject> match)
        {
            return _boCol.Find(match);
        }
        ///<summary>
        /// Searches for all elements that match the conditions defined by the specified predicate, 
        /// and returns a List of all items found in the entire <see cref="System.Collections.Generic.List{TBusinessObject}"/>.
        ///</summary>
        ///<param name="match">The <see cref="System.Predicate{TBusinessObject}"/> delegate that defines the conditions of the element to search for.</param>
        ///<returns>The Elements that match the conditions defined by the specified predicate.</returns>
        /// <exception cref="System.ArgumentNullException">match is null.</exception>
        public List<TBusinessObject> FindAll(System.Predicate<TBusinessObject> match)
        {
            return _boCol.FindAll(match);
        }
        ///<summary>
        /// Loops through each item in the Collection and Applies the Action
        ///</summary>
        ///<param name="action"></param>
        public void ForEach(System.Action<TBusinessObject> action)
        {
            _boCol.ForEach(action);
        }
        /// <summary>
        ///  Removes all the items that Match the Predicate.
        /// </summary>
        /// <param name="match">The <see cref="System.Predicate{TBusinessObject}"/>
        /// delegate that defines the conditions of the element to remove</param>
        public void RemoveAll(System.Predicate<TBusinessObject> match)
        {
            List<TBusinessObject> businessObjects = _boCol.FindAll(match);
            foreach (TBusinessObject businessObject in businessObjects)
            {
                this.Remove(businessObject);
            }
        }

        ///<summary>
        /// Copies the elements of the 
        /// <see cref="BusinessObjectCollection{TBusinessObject}"/> to a new array.
        ///</summary>
        ///<returns>An array containing copies of the elements of the 
        ///<see cref="BusinessObjectCollection{TBusinessObject}"/>.</returns>
        public TBusinessObject[] ToArray()
        {
            return _boCol.ToArray();
        }

        ///<summary>
        /// Converts the elements in the current 
        /// <see cref="BusinessObjectCollection{TBusinessObject}"/> to another type, and
        /// returns a list containing the converted elements.
        ///</summary>
        ///<param name="converter">A <see cref="System.Converter{TInput,TOutput}"/> delegate
        ///that converts each element from one type to another type.</param>
        ///<typeparam name="TOutput">The type of the elements of the target array.
        ///</typeparam>
        ///<returns>A <see cref="System.Collections.Generic.List{T}"/> of the target type
        ///containing the converted elements from the current 
        ///<see cref="BusinessObjectCollection{TBusinessObject}"/>.</returns>
        /// <exception cref="System.ArgumentNullException">converter is null.</exception>
        public List<TOutput> ConvertAll<TOutput>(Converter<TBusinessObject, TOutput> converter)
        {
            return _boCol.ConvertAll(converter);
        }
       
        ///<summary>
        /// Determines whether the <see cref="BusinessObjectCollection{TBusinessObject}"/> contains elements that match the conditions defined by the specified predicate.
        ///</summary>
        ///<param name="match">The <see cref="System.Predicate{TBusinessObject}"/> delegate that defines the conditions of the elements to search for.</param>
        ///<returns>
        /// true if the <see cref="BusinessObjectCollection{TBusinessObject}"/> contains one or more elements that match the conditions defined by the specified predicate; otherwise, false.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">match is null.</exception>
        public bool Exists(Predicate<TBusinessObject> match)
        {
            return _boCol.Exists(match);
        }

        ///<summary>
        /// Adds the elements of the specified collection to the end of the <see cref="BusinessObjectCollection{TBusinessObject}"/>.
        ///</summary>
        ///<param name="collection">The collection whose elements should be added to the end of the <see cref="BusinessObjectCollection{TBusinessObject}"/>. 
        /// The collection itself cannot be null, but it can contain elements that are null, if type <typeparamref name="TBusinessObject"/> is a reference type.</param>
        /// <exception cref="System.ArgumentNullException">collection is null.</exception>
        public void AddRange(IEnumerable<TBusinessObject> collection)
        {
            _boCol.AddRange(collection);
        }

        /// <summary>
        /// Gives you a strongly typed enumrable of these objects for iterating over. 
        /// If the objects are not of type T this with throw an <see cref="InvalidCastException"/>
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}" /> which is a shallow copy of the objects in this collection. </returns>
        IEnumerable<T> IBusinessObjectCollection.AsEnumerable<T>()
        {
            return this.Select(bo => (T) (object) bo);
        }


    }
}