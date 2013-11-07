#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using System.Linq;

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
        IOrderCriteria OrderCriteria { get; }

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
        protected MultipleRelationshipBase(IBusinessObject owningBo, IRelationshipDef lRelDef, IBOPropCol lBOPropCol)
            : base(owningBo, lRelDef, lBOPropCol)
        {
        }

        internal abstract IBusinessObjectCollectionInternal GetLoadedBOColInternal();
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
        private readonly Lazy<RelatedBusinessObjectCollection<TBusinessObject>> _boCol;

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
            (IBusinessObject owningBo, IRelationshipDef lRelDef, IBOPropCol lBOPropCol, int timeOut)
            : base(owningBo, lRelDef, lBOPropCol)
        {
            _boCol = new Lazy<RelatedBusinessObjectCollection<TBusinessObject>>(() => 
				(RelatedBusinessObjectCollection<TBusinessObject>)
				RelationshipUtils.CreateRelatedBusinessObjectCollection(_relDef.RelatedObjectAssemblyName, _relDef.RelatedObjectClassName, this));
            TimeOut = timeOut;
        }

        internal override void DereferenceChildren(TransactionCommitter committer)
        {
            IBusinessObjectCollection col = BusinessObjectCollection;
            for (var i = col.Count - 1; i >= 0; i--)
            {
                var bo = col[i];
                DereferenceChild(committer, bo);
            }
        }

        internal override void DereferenceRemovedChildren(TransactionCommitter committer)
        {
            IList col = this.CurrentBusinessObjectCollection.RemovedBusinessObjects;
            for (int i = col.Count - 1; i >= 0; i--)
            {
                var bo = (IBusinessObject) col[i];
                DereferenceChild(committer, bo);
            }
        }

        internal override void DeleteMarkedForDeleteChildren(TransactionCommitter committer)
        {
            IList col = CurrentBusinessObjectCollection.MarkedForDeleteBusinessObjects;
            for (int i = col.Count - 1; i >= 0; i--)
            {
                var bo = (IBusinessObject) col[i];
                DeleteChild(committer, bo);
            }
        }

        internal override void DeleteChildren(TransactionCommitter committer)
        {
            IBusinessObjectCollection col = BusinessObjectCollection;
            for (var i = col.Count - 1; i >= 0; i--)
            {
                var businessObject = col[i];
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
            for (var i = collection.Count - 1; i >= 0; i--)
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
                    var currentCol = _boCol.Value;
                    return currentCol.PersistedBusinessObjects.Any(bo => bo.Status.IsDirty);
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
				if (!_boCol.IsValueCreated) return false;
				var currentCol = _boCol.Value;
            	return (currentCol.CreatedBusinessObjects.Count > 0) || (currentCol.MarkedForDeleteBusinessObjects.Count > 0)
					   || (currentCol.RemovedBusinessObjects.Count > 0) || (currentCol.AddedBusinessObjects.Count > 0);
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
			get { return _boCol.Value; }
        }

        /// <summary>
        /// Returns the collection for this relationship.  The collection is refreshed before
        /// it is returned.
        /// </summary>
        public BusinessObjectCollection<TBusinessObject> BusinessObjectCollection
        {
            get
            {
                var currentCol = this.CurrentBusinessObjectCollection;
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
			RelationshipUtils.SetupCriteriaForRelationship(this, _boCol.Value);
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
        internal override IBusinessObjectCollectionInternal GetLoadedBOColInternal()
        {
            return _boCol.Value;
        }

        ///<summary>
        /// Returns the <see cref="OrderCriteria"/> for this relationship (which determines how the 
        /// objects in the collection are ordered.
        ///</summary>
        public IOrderCriteria OrderCriteria
        {
            get { return _relDef.OrderCriteria ?? new OrderCriteria(); }
        }

        internal override void CancelEdits()
        {
			if (!_boCol.IsValueCreated) return;
			var currentCol = _boCol.Value;
        	foreach (TBusinessObject createdChild in currentCol.CreatedBusinessObjects.ToArray())
            {
                createdChild.CancelEdits();
				currentCol.RemoveInternal(createdChild);
            }
            var addedBusinessObjects = currentCol.AddedBusinessObjects.Except(currentCol.PersistedBusinessObjects);
            foreach (TBusinessObject addedChild in addedBusinessObjects.ToArray())
            {
				currentCol.Remove(addedChild);
                addedChild.CancelEdits();
            }
            foreach (TBusinessObject dirtyChild in GetDirtyChildren())
            {
                dirtyChild.CancelEdits();
            }
        }

        internal override void AddDirtyChildrenToTransactionCommitter(TransactionCommitter transactionCommitter)
        {
			if (!_boCol.IsValueCreated) return;
        	var currentCol = _boCol.Value;
        	foreach (TBusinessObject businessObject in GetDirtyChildren())
            {
                transactionCommitter.AddBusinessObject(businessObject);
            }
        	if (!this.OwningBO.Status.IsDeleted)
            {
                foreach (TBusinessObject businessObject in currentCol.AddedBusinessObjects)
                {
                    transactionCommitter.AddAddedChildBusinessObject(this, businessObject);
                }
            }
			foreach (TBusinessObject businessObject in currentCol.RemovedBusinessObjects)
            {
                transactionCommitter.AddRemovedChildBusinessObject(this, businessObject);
            }
        }

        internal IList<TBusinessObject> GetDirtyChildren()
        {
        	IList<TBusinessObject> dirtyChildren = new List<TBusinessObject>();
			if (!_boCol.IsValueCreated) return dirtyChildren;
        	var currentCol = _boCol.Value;
        	if (!_owningBo.Status.IsDeleted)
            {
                if (this.RelationshipDef.InsertParentAction == InsertParentAction.InsertRelationship)
                {
					foreach (TBusinessObject bo in currentCol.CreatedBusinessObjects)
                    {
                        dirtyChildren.Add(bo);
                    }
                }
            }
			foreach (TBusinessObject bo in currentCol.MarkedForDeleteBusinessObjects)
            {
                dirtyChildren.Add(bo);
            }
            if (this.RelationshipDef.RelationshipType == RelationshipType.Composition
                || this.RelationshipDef.RelationshipType == RelationshipType.Aggregation)
            {
				foreach (TBusinessObject bo in currentCol.RemovedBusinessObjects)
                {
                    dirtyChildren.Add(bo);
                }
				foreach (TBusinessObject bo in currentCol.AddedBusinessObjects)
                {
                    dirtyChildren.Add(bo);
                }
				foreach (TBusinessObject bo in currentCol.PersistedBusinessObjects)
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
            var noRelatedObjects = collection.Count;
            if (noRelatedObjects <= 0) return true;
            if (!String.IsNullOrEmpty(this._relDef.PreventDeleteMessage))
                message = String.Format(this._relDef.PreventDeleteMessage, this._owningBo.ToString());
            else
                message = string.Format(
                        "You cannot delete {0} identified by {1} or {2} since it is related to {3} Business Objects via the {4} relationship",
                        this._owningBo.ClassDef.ClassName, this._owningBo.ID.AsString_CurrentValue(), this._owningBo.ToString(),
                        noRelatedObjects,
                        this.RelationshipName);
            return false;
        }
    }
}