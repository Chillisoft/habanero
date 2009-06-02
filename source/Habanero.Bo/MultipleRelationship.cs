using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
    /// <summary>
    /// Manages a relationship where the relationship owner relates to several
    /// other objects
    /// </summary>
    public interface IMultipleRelationship : IRelationship
    {
        ///<summary>
        /// The criteria by which this relationship is ordered. I.e. by default all the
        /// related objects are loaded in this order.
        ///</summary>
        OrderCriteria OrderCriteria { get; }

        ///<summary>
        /// The collection of business objects that is managed by this relationship.
        ///</summary>
        IBusinessObjectCollection BusinessObjectCollection { get; }

        ///<summary>
        /// The collection of business objects under the control of this relationship.
        ///</summary>
        IBusinessObjectCollection CurrentBusinessObjectCollection { get; }
    }

    ///<summary>
    /// The base class to be used for MultipleRelationships.
    /// 
    ///</summary>
    public abstract class MultipleRelationshipBase : Relationship
    {
        /// <summary>
        /// Constrcutor for <see cref="MultipleRelationshipBase"/>
        /// </summary>
        /// <param name="owningBo">The <see cref="IBusinessObject"/> that owns this BO.</param>
        /// <param name="lRelDef">The <see cref="IRelationshipDef"/> that identifies  </param>
        /// <param name="lBOPropCol"></param>
        protected MultipleRelationshipBase(IBusinessObject owningBo, RelationshipDef lRelDef, IBOPropCol lBOPropCol)
            : base(owningBo, lRelDef, lBOPropCol)
        {
        }

        internal abstract IBusinessObjectCollection GetLoadedBOColInternal();


    }

    /// <summary>
    /// Manages a relationship where the relationship owner relates to several
    /// other objects
    /// </summary>
    public class MultipleRelationship<TBusinessObject> : MultipleRelationshipBase, IMultipleRelationship
        where TBusinessObject : class, IBusinessObject, new()
    {
        /// <summary>
        /// The timout in milliseconds. The collection will not be automatically refreshed from the DB if the timeout has not expired
        /// </summary>
        public int TimeOut { get; private set; }

        /// <summary> The collection storing the Related Business Objects. </summary>
        private RelatedBusinessObjectCollection<TBusinessObject> _boCol;

        /// <summary>
        /// Constructor to initialise a new relationship
        /// </summary>
        /// <param name="owningBo">The business object that owns the
        /// relationship</param>
        /// <param name="lRelDef">The relationship definition</param>
        /// <param name="lBOPropCol">The set of properties used to
        /// initialise the RelKey object</param>
        public MultipleRelationship(IBusinessObject owningBo, RelationshipDef lRelDef, IBOPropCol lBOPropCol)
            : this(owningBo, lRelDef, lBOPropCol, 0)
        {
        }

        /// <summary>
        /// Constructor to initialise a new relationship
        /// </summary>
        /// <param name="owningBo">The business object that owns the relationship</param>
        /// <param name="lRelDef">The relationship definition</param>
        /// <param name="lBOPropCol">The set of properties used to initialise the RelKey object</param>
        /// <param name="timeOut">The timeout between when the collection was last loaded.</param>
        public MultipleRelationship
            (IBusinessObject owningBo, RelationshipDef lRelDef, IBOPropCol lBOPropCol, int timeOut)
            : base(owningBo, lRelDef, lBOPropCol)
        {
            _boCol =
                (RelatedBusinessObjectCollection<TBusinessObject>)
                RelationshipUtils.CreateRelatedBusinessObjectCollection(_relDef.RelatedObjectClassType, this);
            TimeOut = timeOut;
        }

        internal override void DereferenceChildren(TransactionCommitter committer)
        {
            IBusinessObjectCollection col = BusinessObjectCollection;
            for (int i = col.Count - 1; i >= 0; i--)
            {
                IBusinessObject bo = col[i];
                DereferenceChild(committer, bo);
            }
        }

        internal override void DereferenceRemovedChildren(TransactionCommitter committer)
        {
            IList col = this.CurrentBusinessObjectCollection.RemovedBusinessObjects;
            for (int i = col.Count - 1; i >= 0; i--)
            {
                IBusinessObject bo = (IBusinessObject) col[i];
                DereferenceChild(committer, bo);
            }
        }

        internal override void DeleteMarkedForDeleteChildren(TransactionCommitter committer)
        {
            IList col = CurrentBusinessObjectCollection.MarkedForDeleteBusinessObjects;
            for (int i = col.Count - 1; i >= 0; i--)
            {
                IBusinessObject bo = (IBusinessObject) col[i];
                DeleteChild(committer, bo);
            }
        }

        internal override void DeleteChildren(TransactionCommitter committer)
        {
            IBusinessObjectCollection col = BusinessObjectCollection;
            for (int i = col.Count - 1; i >= 0; i--)
            {
                IBusinessObject businessObject = col[i];
                if (!businessObject.Status.IsNew)
                {
                    DeleteChild(committer, businessObject);
                }
            }
        }

        /// <summary>
        /// If the relationship is <see cref="IBusinessObject.MarkForDelete"/>.DeleteRelated then
        /// all the related objects and their relevant children will be marked for Delete.
        /// See <see cref="IRelationship.DeleteParentAction"/>
        /// </summary>
        public override void MarkForDelete()
        {
            if (this.RelationshipDef.DeleteParentAction != DeleteParentAction.DeleteRelated) return;
            IBusinessObjectCollection collection = this.BusinessObjectCollection;
            collection.Refresh();
            for (int i = collection.Count - 1; i >= 0; i--)
            {
                collection[i].MarkForDelete();
            }
        }

        ///<summary>
        /// Returns whether the relationship is dirty or not.
        /// A relationship is always dirty if it has Added, created, removed or deleted Related business objects.
        /// If the relationship is of type composition or aggregation then it is dirty if it has any 
        ///  related (children) business objects that are dirty.
        ///</summary>
        public override bool IsDirty
        {
            get
            {
                if (HasDirtyEditingCollections) return true;
                if (this.RelationshipDef.RelationshipType == RelationshipType.Aggregation
                    || RelationshipDef.RelationshipType == RelationshipType.Composition)
                {
                    foreach (IBusinessObject bo in _boCol.PersistedBusinessObjects)
                    {
                        if (bo.Status.IsDirty)
                        {
                            return true;
                        }
                    }
                }
                return false; // || 
            }
        }

        /// <summary>
        /// Are any of the Collections that store edits to this Relationship dirty.
        /// </summary>
        protected bool HasDirtyEditingCollections
        {
            get
            {
                return (_boCol.CreatedBusinessObjects.Count > 0) || (_boCol.MarkedForDeleteBusinessObjects.Count > 0)
                       || (_boCol.RemovedBusinessObjects.Count > 0) || (_boCol.AddedBusinessObjects.Count > 0);
            }
        }

        ///<summary>
        /// The collection of business objects that is managed by this relationship.
        ///</summary>
        IBusinessObjectCollection IMultipleRelationship.BusinessObjectCollection
        {
            get { return this.BusinessObjectCollection; }
        }

        ///<summary>
        /// The collection of business objects under the control of this relationship.
        /// This collection is not refreshed from the Database prior to being returned. I.e. This returns the 
        /// collection exactly in the state that it is in Memory.
        ///</summary>
        IBusinessObjectCollection IMultipleRelationship.CurrentBusinessObjectCollection
        {
            get { return CurrentBusinessObjectCollection; }
        }

        ///<summary>
        /// The collection of business objects under the control of this relationship.
        /// This collection is not refreshed from the Database prior to being returned. I.e. This returns the 
        /// collection exactly in the state that it is in Memory.
        ///</summary>
        public BusinessObjectCollection<TBusinessObject> CurrentBusinessObjectCollection
        {
            get { return _boCol; }
        }

        /// <summary>
        /// Returns the collection for this relationship.  The collection is refreshed before
        /// it is returned.
        /// </summary>
        public BusinessObjectCollection<TBusinessObject> BusinessObjectCollection
        {
            get
            {
                BusinessObjectCollection<TBusinessObject> currentCol = this.CurrentBusinessObjectCollection;
                if (TimeOutHasExpired(currentCol))
                {
                    RelationshipUtils.SetupCriteriaForRelationship(this, currentCol);
                    BORegistry.DataAccessor.BusinessObjectLoader.Refresh(currentCol);
                    return currentCol;
                }
                return currentCol;
            }
        }

        private bool TimeOutHasExpired(IBusinessObjectCollection currentCol)
        {
            if (this.TimeOut <= 0 || currentCol.TimeLastLoaded == null)
            {
                return true;
            }
            TimeSpan timeSinceLastLoad = DateTime.Now - currentCol.TimeLastLoaded.Value;
            return timeSinceLastLoad.TotalMilliseconds >= this.TimeOut;
        }

        /// <summary>
        /// Do the initialisation of this relationship.
        /// </summary>
        protected override void DoInitialisation()
        {
            RelationshipUtils.SetupCriteriaForRelationship(this, _boCol);
        }

        /// <summary>
        /// Updates the Relationship as Persisted
        /// </summary>
        internal override void UpdateRelationshipAsPersisted()
        {
        }

        /// <summary>
        /// Returns the underlying collection without refreshing it.
        /// </summary>
        /// <returns></returns>
        internal override IBusinessObjectCollection GetLoadedBOColInternal()
        {
            return _boCol;
        }

        ///<summary>
        /// Returns the <see cref="OrderCriteria"/> for this relationship (which determines how the 
        /// objects in the collection are ordered.
        ///</summary>
        public OrderCriteria OrderCriteria
        {
            get { return _relDef.OrderCriteria ?? new OrderCriteria(); }
        }

        internal override void CancelEdits()
        {
            foreach (TBusinessObject createdChild in _boCol.CreatedBusinessObjects.ToArray())
            {
                createdChild.CancelEdits();
                _boCol.RemoveInternal(createdChild);
//                createdChild.
            }
            foreach (TBusinessObject addedChild in _boCol.AddedBusinessObjects.ToArray())
            {
                _boCol.Remove(addedChild);
                addedChild.CancelEdits();
            }
            foreach (TBusinessObject dirtyChild in GetDirtyChildren())
            {
                dirtyChild.CancelEdits();
            }
        }

        internal override void AddDirtyChildrenToTransactionCommitter(TransactionCommitter transactionCommitter)
        {
            foreach (TBusinessObject businessObject in GetDirtyChildren())
            {
                transactionCommitter.AddBusinessObject(businessObject);
            }
            if (!this.OwningBO.Status.IsDeleted)
            {
                foreach (TBusinessObject businessObject in _boCol.AddedBusinessObjects)
                {
                    transactionCommitter.AddAddedChildBusinessObject(this, businessObject);
                }
            }
            foreach (TBusinessObject businessObject in _boCol.RemovedBusinessObjects)
            {
                transactionCommitter.AddRemovedChildBusinessObject(this, businessObject);
            }
        }

//        private ISingleRelationship CreateReverseRelationship(TBusinessObject businessObject)
//        {
//            RelKeyDef def = new RelKeyDef();
//            foreach (IRelProp prop in this.RelKey)
//            {
//                def.Add(new RelPropDef(businessObject.ClassDef.PropDefcol[prop.OwnerPropertyName],prop.OwnerPropertyName));
//            }
//            IRelationshipDef relationshipDef = new SingleRelationshipDef("TempSingleReverseRelationship", businessObject.ClassDef.ClassType, def, false,
//                 DeleteParentAction.DoNothing);
//            return (ISingleRelationship) relationshipDef.CreateRelationship(businessObject, businessObject.Props);
//        }

        internal IList<TBusinessObject> GetDirtyChildren()
        {
            IList<TBusinessObject> dirtyChildren = new List<TBusinessObject>();
            if (!_owningBo.Status.IsDeleted)
            {
                if (this.RelationshipDef.InsertParentAction == InsertParentAction.InsertRelationship)
                {
                    foreach (TBusinessObject bo in _boCol.CreatedBusinessObjects)
                    {
                        dirtyChildren.Add(bo);
                    }
                }
            }
            foreach (TBusinessObject bo in _boCol.MarkedForDeleteBusinessObjects)
            {
                dirtyChildren.Add(bo);
            }
            if (this.RelationshipDef.RelationshipType == RelationshipType.Composition
                || this.RelationshipDef.RelationshipType == RelationshipType.Aggregation)
            {
                foreach (TBusinessObject bo in _boCol.RemovedBusinessObjects)
                {
                    dirtyChildren.Add(bo);
                }
                foreach (TBusinessObject bo in _boCol.AddedBusinessObjects)
                {
                    dirtyChildren.Add(bo);
                }
                foreach (TBusinessObject bo in _boCol.PersistedBusinessObjects)
                {
                    if (bo.Status.IsDirty && !dirtyChildren.Contains(bo))
                    {
                        dirtyChildren.Add(bo);
                    }
                }
            }
            return dirtyChildren;
        }
        /// <summary>
        /// Is there anything in this relationship to prevent the business object from being deleted.
        /// e.g. if there are related business objects that are not marked as mark for delete.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool IsDeletable(out string message)
        {
            message = "";
            if (this.RelationshipDef.DeleteParentAction != DeleteParentAction.Prevent) return true;

            IBusinessObjectCollection collection = this.BusinessObjectCollection;
            int noRelatedObjects = collection.Count;
            if (noRelatedObjects <= 0) return true;
            message = string.Format(
                    "You cannot delete {0} Identified By {1} or {2} since it is related to {3} Business Objects via the {4} relationship",
                    this._owningBo.ClassDef.ClassName, this._owningBo.ID.AsString_CurrentValue(), this._owningBo.ToString(),
                    noRelatedObjects,
                    this.RelationshipName);
            return false;
        }
    }
}