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
    /// The base class to be used for MultipleRelationships
    ///</summary>
    public abstract class MultipleRelationshipBase : Relationship
    {
        protected MultipleRelationshipBase(IBusinessObject owningBo, RelationshipDef lRelDef, BOPropCol lBOPropCol)
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
        protected RelatedBusinessObjectCollection<TBusinessObject> _boCol;

        /// <summary>
        /// Constructor to initialise a new relationship
        /// </summary>
        /// <param name="owningBo">The business object that owns the
        /// relationship</param>
        /// <param name="lRelDef">The relationship definition</param>
        /// <param name="lBOPropCol">The set of properties used to
        /// initialise the RelKey object</param>
        public MultipleRelationship(IBusinessObject owningBo, RelationshipDef lRelDef, BOPropCol lBOPropCol)
            : base(owningBo, lRelDef, lBOPropCol)
        {
            _boCol =
                (RelatedBusinessObjectCollection<TBusinessObject>)
                RelationshipUtils.CreateNewRelatedBusinessObjectCollection(_relDef.RelatedObjectClassType, this);
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

        protected bool HasDirtyEditingCollections
        {
            get
            {
                return (_boCol.CreatedBusinessObjects.Count > 0) || (_boCol.MarkedForDeleteBusinessObjects.Count > 0)
                       || (_boCol.RemovedBusinessObjects.Count > 0) || (_boCol.AddedBusinessObjects.Count > 0);
            }
        }

        IBusinessObjectCollection IMultipleRelationship.BusinessObjectCollection
        {
            get { return this.BusinessObjectCollection; }
        }

        ///<summary>
        /// The collection of business objects under the control of this relationship.
        ///</summary>
        public IBusinessObjectCollection CurrentBusinessObjectCollection
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
                BORegistry.DataAccessor.BusinessObjectLoader.Refresh(_boCol);
                return _boCol;
            }
        }

        protected override void DoInitialisation()
        {
            RelationshipUtils.SetupCriteriaForRelationship(this, _boCol);
        }

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
            get
            {
                return _relDef.OrderCriteria ?? new OrderCriteria();
            }
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
                addedChild.CancelEdits();
                _boCol.Remove(addedChild);
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
//            if (this.RelationshipDef.RelationshipType == RelationshipType.Association)
//            {
            if (!this.OwningBO.Status.IsDeleted)
            {
                foreach (TBusinessObject businessObject in _boCol.AddedBusinessObjects)
                {
//                    ISingleRelationship reverseRelationship =
//                        GetReverseRelationship(businessObject) as ISingleRelationship;
//                    if (reverseRelationship == null)
//                    {
//                        reverseRelationship = CreateReverseRelationship(businessObject);
//                    }
//                    if (reverseRelationship != null)
//                    {
                        transactionCommitter.AddTransaction
                            (new TransactionalSingleRelationship_Added(this, businessObject));
//                    }
                }
            }
            foreach (TBusinessObject businessObject in _boCol.RemovedBusinessObjects)
            {
//                ISingleRelationship reverseRelationship = GetReverseRelationship(businessObject) as ISingleRelationship;
//                if (reverseRelationship != null)
//                {
                    transactionCommitter.AddTransaction
                        (new TransactionalSingleRelationship_Removed(this, businessObject));
//                }
            }
        }

//        private ISingleRelationship CreateReverseRelationship(TBusinessObject businessObject)
//        {
//            RelKeyDef def = new RelKeyDef();
//            foreach (IRelProp prop in this.RelKey)
//            {
//                def.Add(new RelPropDef(businessObject.ClassDef.PropDefcol[prop.OwnerPropertyName],prop.OwnerPropertyName));
//            }
//            RelationshipDef relationshipDef = new SingleRelationshipDef("TempSingleReverseRelationship", businessObject.ClassDef.ClassType, def, false,
//                 DeleteParentAction.DoNothing);
//            return (ISingleRelationship) relationshipDef.CreateRelationship(businessObject, businessObject.Props);
//        }

        internal IList<TBusinessObject> GetDirtyChildren()
        {
            IList<TBusinessObject> dirtyChildren = new List<TBusinessObject>();
            if (!_owningBo.Status.IsDeleted)
            {
                foreach (TBusinessObject bo in _boCol.CreatedBusinessObjects)
                {
                    dirtyChildren.Add(bo);
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
    }
}