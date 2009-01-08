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

using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

//using log4net;

namespace Habanero.BO
{

    /// <summary>
    /// Manages a relationship where the relationship owner relates to one
    /// other object
    /// </summary>
    public interface ISingleRelationship : IRelationship {
        /// <summary>
        /// Sets the related object to that provided
        /// </summary>
        /// <param name="relatedObject">The object to relate to</param>
        void SetRelatedObject(IBusinessObject relatedObject);

        ///<summary>
        /// Returns the related object for the single relationship.
        ///</summary>
        ///<returns>returns the related business object</returns>
        IBusinessObject GetRelatedObject();

        /// <summary>
        /// Indicates whether the related object has been specified
        /// </summary>
        /// <returns>Returns true if related object exists</returns>
        bool HasRelatedObject();

        bool OwningBOHasForeignKey { get; set; }

    }

    public abstract class SingleRelationshipBase : Relationship
    {
        protected SingleRelationshipBase(IBusinessObject owningBo, RelationshipDef lRelDef, BOPropCol lBOPropCol) : base(owningBo, lRelDef, lBOPropCol) {}
        internal abstract IBusinessObject RemovedBOInternal { get; }
    }
    /// <summary>
    /// Manages a relationship where the relationship owner relates to one
    /// other object
    /// </summary>
    public class SingleRelationship<TBusinessObject> : SingleRelationshipBase, ISingleRelationship
        where TBusinessObject : class, IBusinessObject, new()
    {
        //TODO: Implement logging private static readonly ILog log = LogManager.GetLogger("Habanero.BO.SingleRelationship");
        private TBusinessObject _relatedBo;
        private IExpression _storedRelationshipExpression;
        private bool _isRemoved;
        private TBusinessObject _removedBO;
        
        /// <summary>
        /// Constructor to initialise a new relationship
        /// </summary>
        /// <param name="owningBo">The business object that owns the
        /// relationship</param>
        /// <param name="lRelDef">The relationship definition</param>
        /// <param name="lBOPropCol">The set of properties used to
        /// initialise the RelKey object</param>
        public SingleRelationship(IBusinessObject owningBo, RelationshipDef lRelDef, BOPropCol lBOPropCol)
            : base(owningBo, lRelDef, lBOPropCol)
        {
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
                if (IsRelatedBOCreatedOrDeleted()) return true;
                if (IsRelatedBOPropsDirty() || IsRemovedBOPropsDirty()) return true;
                if (MustPersistChild()) return _isRemoved || IsRelatedBODirty();
                return false;
            }
        }

        private bool IsRelatedBOCreatedOrDeleted() {
            return (_relatedBo != null && (_relatedBo.Status.IsNew || _relatedBo.Status.IsDeleted));
        }

        private bool MustPersistChild()
        {
            return this.RelationshipDef.RelationshipType == RelationshipType.Composition
                   || this.RelationshipDef.RelationshipType == RelationshipType.Aggregation;
        }

        private bool IsRemovedBOPropsDirty()
        {
            return IsRelatedPropsDirty(_removedBO);
        }

        private bool IsRelatedBOPropsDirty()
        {
            return IsRelatedPropsDirty(_relatedBo);
        }

        private bool IsRelatedPropsDirty(IBusinessObject bo) {
            if (bo == null) return false;
            foreach (IRelPropDef relPropDef in this.RelationshipDef.RelKeyDef)
            {
                if (bo.Props[relPropDef.RelatedClassPropName].IsDirty) return true;
            }
            return false;
        }

        internal bool IsRemoved
        { get { return _isRemoved; } }

        internal TBusinessObject RemovedBO
        { get { return _removedBO; } }

        internal override IBusinessObject RemovedBOInternal
        { get { return RemovedBO; } }

        public bool OwningBOHasForeignKey { get { return _relDef.OwningBOHasForeignKey; } set { _relDef.OwningBOHasForeignKey = value; } }

        /// <summary>
        /// Indicates whether the related object has been specified
        /// </summary>
        /// <returns>Returns true if related object exists</returns>
        public virtual bool HasRelatedObject()
        {
            return _relKey.HasRelatedObject();
        }

        ///<summary>
        /// Returns the related object for the single relationship.
        ///</summary>
        ///<returns>returns the related business object</returns>
        IBusinessObject ISingleRelationship.GetRelatedObject()
        {
            return GetRelatedObject();
        }

        ///<summary>
        /// Returns the related object for the single relationship.
        ///</summary>
        ///<returns>returns the related business object</returns>
        public virtual TBusinessObject GetRelatedObject()
        {
            if (RelatedBoForeignKeyHasChanged()) _relatedBo = null;

            IExpression newRelationshipExpression = _relKey.RelationshipExpression();
            if (_storedRelationshipExpression == null || (_storedRelationshipExpression != null && _storedRelationshipExpression.ExpressionString() != newRelationshipExpression.ExpressionString()))
            {
                _relatedBo = HasRelatedObject() ? Broker.GetRelatedBusinessObject(this) : null;
                _storedRelationshipExpression = newRelationshipExpression;
            }
            return _relatedBo;
        }

        private bool RelatedBoForeignKeyHasChanged()
        {
            if (_relatedBo != null)
            {
                foreach (IRelProp prop in this.RelKey)
                {
                    object relatedPropValue = _relatedBo.GetPropertyValue(prop.RelatedClassPropName);
                    if (prop.BOProp.Value == null)
                    {
                        if (relatedPropValue == null) continue;
                        return true;
                    }
                    if (prop.BOProp.Value.Equals(relatedPropValue)) continue;
                    return true;
                }
            }
            return false;
        }

        void ISingleRelationship.SetRelatedObject(IBusinessObject relatedObject)
        {
            SetRelatedObject((TBusinessObject) relatedObject);
        }

        /// <summary>
        /// Sets the related object to that provided
        /// </summary>
        /// <param name="relatedObject">The object to relate to</param>
        public virtual void SetRelatedObject(TBusinessObject relatedObject)
        {
            if (_relatedBo == null) GetRelatedObject();
            if (_relatedBo == relatedObject) return;

            if (relatedObject != null) RelationshipDef.CheckCanAddChild(relatedObject);
            if (_relatedBo != null) RelationshipDef.CheckCanRemoveChild(_relatedBo);

            RemoveFromReverseRelationship(_relatedBo);
            AddToReverseRelationship(relatedObject);

            _relatedBo = relatedObject;            

            UpdatedForeignKeyAndStoredRelationshipExpression();
        }

        private void AddToReverseRelationship(TBusinessObject relatedObject) {
            if (relatedObject == null) return;
            
            IRelationship reverseRelationship = GetReverseRelationship(relatedObject);
            if (reverseRelationship == null) return;

            reverseRelationship.RelationshipDef.CheckCanAddChild(this.OwningBO);
            _relatedBo = relatedObject;

            AddToMultipleReverseRelationship(reverseRelationship);
            AddToSingleReverseRelationship(reverseRelationship);
        }

        private void AddToSingleReverseRelationship(IRelationship reverseRelationship) {
            ISingleRelationship singleRelationship = reverseRelationship as ISingleRelationship;
            if (singleRelationship != null)
            {
                singleRelationship.SetRelatedObject(this.OwningBO);
            }
        }

        private void AddToMultipleReverseRelationship(IRelationship reverseRelationship) {
            MultipleRelationshipBase multipleRelationship = reverseRelationship as MultipleRelationshipBase;
            if (multipleRelationship != null) 
            {
                multipleRelationship.GetLoadedBOColInternal().Add(this.OwningBO);
            }
        }

        private void RemoveFromReverseRelationship(TBusinessObject previousRelatedBO) {
            if (previousRelatedBO == null)
            {
                _isRemoved = false;
                _removedBO = null;
                return;
            }

            IRelationship reverseRelationship = GetReverseRelationship(previousRelatedBO);
            if (reverseRelationship != null)
            {
                reverseRelationship.RelationshipDef.CheckCanRemoveChild(this.OwningBO);
                _relatedBo = null;
                UpdatedForeignKeyAndStoredRelationshipExpression();

                RemoveFromMultipleReverseRelationship(reverseRelationship);
                RemoveFromSingleReverseRelationship(reverseRelationship);
            }
            _isRemoved = true;
            _removedBO = previousRelatedBO;
        }

        private void RemoveFromSingleReverseRelationship(IRelationship reverseRelationship) {
            ISingleRelationship singleReverseRelationship = reverseRelationship as ISingleRelationship;
            if (singleReverseRelationship != null)
            {
                RelationshipUtils.CheckCorrespondingSingleRelationshipsAreValid(this, singleReverseRelationship);
                singleReverseRelationship.SetRelatedObject(null);
            }
        }

        private void RemoveFromMultipleReverseRelationship(IRelationship reverseRelationship) {
            MultipleRelationshipBase multipleReverseRelationship = reverseRelationship as MultipleRelationshipBase;
            if (multipleReverseRelationship != null)
            {
                IBusinessObjectCollection colInternal = multipleReverseRelationship.GetLoadedBOColInternal();
                if (colInternal.Contains(this.OwningBO)) colInternal.Remove(this.OwningBO);
            }
        }

        private void UpdatedForeignKeyAndStoredRelationshipExpression()
        {
            if (this.RelationshipDef.RelationshipType == RelationshipType.Association && this.OwningBOHasForeignKey)
            {
                foreach (RelProp relProp in _relKey)
                {
                    object relatedObjectValue = _relatedBo == null
                                                    ? null
                                                    : _relatedBo.GetPropertyValue(relProp.RelatedClassPropName);
                    _owningBo.SetPropertyValue(relProp.OwnerPropertyName, relatedObjectValue);
                }
            }
            _storedRelationshipExpression = _relKey.RelationshipExpression();
        }

        private bool IsRelatedBODirty() { return _relatedBo != null && _relatedBo.Status.IsDirty ; }

        private bool MustAddToDirtyBusinessObjects()
        {
            return IsRelatedBODirty() &&
                   ((this.RelationshipDef.RelationshipType == RelationshipType.Aggregation
                    || this.RelationshipDef.RelationshipType == RelationshipType.Composition)
                   || _relatedBo.Status.IsNew
                   || _relatedBo.Status.IsDeleted);
        }

        private bool MustAddRemovedBOToDirtyBusinessObjects() { return _isRemoved && (this.RelationshipDef.RelationshipType == RelationshipType.Aggregation
                    || this.RelationshipDef.RelationshipType == RelationshipType.Composition); }

        protected override void DoInitialisation()
        {
            // do nothing
        }

        internal override void UpdateRelationshipAsPersisted() { 
            _isRemoved = false;
            _removedBO = null;
        }

        internal override void DereferenceChildren(TransactionCommitter committer)
        {
            DereferenceChild(committer, GetRelatedObject());
        }

        internal override void DeleteChildren(TransactionCommitter committer) {
            DeleteChild(committer, GetRelatedObject());
        }

        internal override void AddDirtyChildrenToTransactionCommitter(TransactionCommitter transactionCommitter) {
            foreach (TBusinessObject businessObject in GetDirtyChildren())
            {
                transactionCommitter.AddBusinessObject(businessObject);
            }
            if (this.RelationshipDef.RelationshipType == RelationshipType.Association)
            {
                if (IsRelatedBOPropsDirty())
                {
                    ISingleRelationship reverseRelationship = GetReverseRelationship(_relatedBo) as ISingleRelationship;
                    if (reverseRelationship != null)
                    {
                        transactionCommitter.AddTransaction(new TransactionalSingleRelationship_Added(reverseRelationship));
                    }
                } else if (IsRemoved)
                {
                    ISingleRelationship reverseRelationship = GetReverseRelationship(_removedBO) as ISingleRelationship;
                    if (reverseRelationship != null)
                    {
                        transactionCommitter.AddTransaction(new TransactionalSingleRelationship_Removed(reverseRelationship));
                    }
                }
            }
        }

        internal IList<TBusinessObject> GetDirtyChildren()
        {
            IList<TBusinessObject> dirtyChildren = new List<TBusinessObject>();
            if (MustAddToDirtyBusinessObjects()) dirtyChildren.Add(_relatedBo);
            if (MustAddRemovedBOToDirtyBusinessObjects()) dirtyChildren.Add(_removedBO);
            return dirtyChildren;
        }
    }

}