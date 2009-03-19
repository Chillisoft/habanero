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
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

//using log4net;

namespace Habanero.BO
{
    /// <summary>
    /// Manages a relationship where the relationship owner relates to one
    /// other object
    /// </summary>
    public interface ISingleRelationship : IRelationship
    {
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
    }

    ///<summary>
    /// Provides a base class for managing single relationships
    ///</summary>
    public abstract class SingleRelationshipBase : Relationship
    {
        /// <summary>
        /// Constructs a Single Relationship with the Owning BO, the RelationshipDe and the Props that from the
        /// Owning BO.
        /// </summary>
        /// <param name="owningBo">The Business object that owns this relationship.</param>
        /// <param name="lRelDef">The <see cref="IRelationshipDef"/> that this relationship is for </param>
        /// <param name="lBOPropCol">The Collection of business objects properties (<see cref="BOPropCol"/> that is used to create the relKey</param>
        protected SingleRelationshipBase(IBusinessObject owningBo, RelationshipDef lRelDef, BOPropCol lBOPropCol)
            : base(owningBo, lRelDef, lBOPropCol)
        {
        }

        internal abstract IBusinessObject RemovedBOInternal { get; }

        ///<summary>
        /// Returns true if the Business object that owns this relationship has the foreign key and true otherwise.
        ///</summary>
        public abstract bool OwningBOHasForeignKey { get; set; }
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
        private Criteria _storedKeyCriteria;

        /// <summary>
        /// Indicates that the related BO has been changed. 
        /// This is fired any time that the related BO of the relationship is changed.
        /// </summary>
        public event EventHandler<BOEventArgs<TBusinessObject>> Updated;

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
                if (MustPersistChild()) return IsRemoved || IsRelatedBODirty();
                return false;
            }
        }

        private bool IsRelatedBOCreatedOrDeleted()
        {
            return (_relatedBo != null && (_relatedBo.Status.IsNew || _relatedBo.Status.IsDeleted));
        }

        private bool MustPersistChild()
        {
            return this.RelationshipDef.RelationshipType == RelationshipType.Composition
                   || this.RelationshipDef.RelationshipType == RelationshipType.Aggregation;
        }

        private bool IsRemovedBOPropsDirty()
        {
            return IsRelatedPropsDirty(RemovedBO);
        }

        private bool IsRelatedBOPropsDirty()
        {
            return IsRelatedPropsDirty(_relatedBo);
        }

        private bool IsRelatedPropsDirty(IBusinessObject bo)
        {
            if (bo == null) return false;
            foreach (IRelPropDef relPropDef in this.RelationshipDef.RelKeyDef)
            {
                if (bo.Props[relPropDef.RelatedClassPropName].IsDirty) return true;
            }
            return false;
        }

        internal bool IsRemoved { get; private set; }

        internal TBusinessObject RemovedBO { get; private set; }

        internal override IBusinessObject RemovedBOInternal
        {
            get { return RemovedBO; }
        }

        ///<summary>
        /// Returns true if the Business object that owns this relationship has the foreign key and true otherwise.
        ///</summary>
        public override bool OwningBOHasForeignKey
        {
            get { return _relDef.OwningBOHasForeignKey; }
            set { _relDef.OwningBOHasForeignKey = value; }
        }

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
            if (!HasRelatedObject())
            {
                _relatedBo = null;
                return null;
            }
//            if (!OwningBOHasForeignKey && (RelatedBoForeignKeyHasChanged() || _relatedBo == null))
            if ((RelatedBoForeignKeyHasChanged() || _relatedBo == null))
            {
                BusinessObjectCollection<TBusinessObject> relatedCol =
                    BusinessObjectManager.Instance.Find<TBusinessObject>(_relKey.Criteria);
                if (relatedCol.Count == 0)
                {
                    _relatedBo = null;
                }
                else if (relatedCol.Count == 1)
                {
                    _relatedBo = relatedCol[0] == _relatedBo ? null : relatedCol[0];
                }
                if (_relatedBo != null) return _relatedBo;
            }
            Criteria newKeyCriteria = _relKey.Criteria;
            if (_relatedBo != null && newKeyCriteria.IsMatch(_relatedBo, false))
            {
                return _relatedBo;
            }

            if (_storedKeyCriteria == null
                || (_storedKeyCriteria != null && ! _storedKeyCriteria.Equals(newKeyCriteria)))
            {
                if (HasRelatedObject())
                {
                    _relatedBo = Broker.GetRelatedBusinessObject(this);
                    _storedKeyCriteria = newKeyCriteria;
                }
                else
                {
                    _relatedBo = null;
                    _storedKeyCriteria = null;
                }
            }
            //If the object loaded from the databases current value does not match the relationship critieria then 
            // return null.
            if (_relatedBo != null && newKeyCriteria.IsMatch(_relatedBo, false))
            {
                return _relatedBo;
            }
            return null;
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

            _relatedBo = relatedObject;

            AddToReverseRelationship(relatedObject);

            UpdatedForeignKeyAndStoredRelationshipExpression();

            FireUpdatedEvent();
        }

        private void FireUpdatedEvent()
        {
            if (Updated != null) this.Updated(this, new BOEventArgs<TBusinessObject>(_relatedBo));
        }

        private void AddToReverseRelationship(TBusinessObject relatedObject)
        {
            if (relatedObject == null) return;

            IRelationship reverseRelationship = GetReverseRelationship(relatedObject);
            if (reverseRelationship == null) return;

            reverseRelationship.RelationshipDef.CheckCanAddChild(this.OwningBO);
            _relatedBo = relatedObject;

            AddToMultipleReverseRelationship(reverseRelationship);
            AddToSingleReverseRelationship(reverseRelationship);
        }

        private void AddToSingleReverseRelationship(IRelationship reverseRelationship)
        {
            ISingleRelationship singleRelationship = reverseRelationship as ISingleRelationship;

            if (singleRelationship == null) return;
            RelationshipUtils.CheckCorrespondingSingleRelationshipsAreValid
                (this, (SingleRelationshipBase) singleRelationship);
            singleRelationship.SetRelatedObject(this.OwningBO);
        }

        private void AddToMultipleReverseRelationship(IRelationship reverseRelationship)
        {
            MultipleRelationshipBase multipleRelationship = reverseRelationship as MultipleRelationshipBase;
            if (multipleRelationship != null)
            {
                multipleRelationship.GetLoadedBOColInternal().Add(this.OwningBO);
            }
        }

        private void RemoveFromReverseRelationship(TBusinessObject previousRelatedBO)
        {
            if (previousRelatedBO == null)
            {
                IsRemoved = false;
                RemovedBO = null;
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
            else if (!this.OwningBOHasForeignKey)
            {
                foreach (RelProp relProp in _relKey)
                {
                    _relatedBo.SetPropertyValue(relProp.RelatedClassPropName, null);
                }
            }
            IsRemoved = true;
            RemovedBO = previousRelatedBO;
        }

        private void RemoveFromSingleReverseRelationship(IRelationship reverseRelationship)
        {
            SingleRelationshipBase singleReverseRelationship = reverseRelationship as SingleRelationshipBase;
            if (singleReverseRelationship == null) return;

            RelationshipUtils.CheckCorrespondingSingleRelationshipsAreValid(this, singleReverseRelationship);
            ((ISingleRelationship) singleReverseRelationship).SetRelatedObject(null);
        }

        private void RemoveFromMultipleReverseRelationship(IRelationship reverseRelationship)
        {
            MultipleRelationshipBase multipleReverseRelationship = reverseRelationship as MultipleRelationshipBase;
            if (multipleReverseRelationship == null) return;
            IBusinessObjectCollection colInternal = multipleReverseRelationship.GetLoadedBOColInternal();
            if (colInternal.Contains(this.OwningBO)) colInternal.Remove(this.OwningBO);
        }

        private void UpdatedForeignKeyAndStoredRelationshipExpression()
        {
            if (this.OwningBOHasForeignKey)
            {
                foreach (RelProp relProp in _relKey)
                {
                    object relatedObjectValue = _relatedBo == null
                                                    ? null
                                                    : _relatedBo.GetPropertyValue(relProp.RelatedClassPropName);
                    _owningBo.SetPropertyValue(relProp.OwnerPropertyName, relatedObjectValue);
                }
            }
            else if (!HasReverseRelationshipDefined(this))
            {
                foreach (RelProp relProp in _relKey)
                {
                    object owningBOValue = _owningBo == null
                                               ? null
                                               : _owningBo.GetPropertyValue(relProp.OwnerPropertyName);
                    if (_relatedBo != null) _relatedBo.SetPropertyValue(relProp.RelatedClassPropName, owningBOValue);
                }
            }
            _storedKeyCriteria = _relKey.Criteria;
        }

        private bool IsRelatedBODirty()
        {
            return _relatedBo != null && _relatedBo.Status.IsDirty;
        }

        private bool MustAddToDirtyBusinessObjects()
        {
            return IsRelatedBODirty()
                   &&
                   ((this.RelationshipDef.RelationshipType == RelationshipType.Aggregation
                     || this.RelationshipDef.RelationshipType == RelationshipType.Composition)
                    || _relatedBo.Status.IsNew || _relatedBo.Status.IsDeleted);
        }

        private bool MustAddRemovedBOToDirtyBusinessObjects()
        {
            return IsRemoved
                   &&
                   (this.RelationshipDef.RelationshipType == RelationshipType.Aggregation
                    || this.RelationshipDef.RelationshipType == RelationshipType.Composition);
        }

        /// <summary>
        /// Do the initialisation of this relationship.
        /// </summary>
        protected override void DoInitialisation()
        {
            // do nothing
        }
        /// <summary>
        /// UpdateRelationshipAsPersisted
        /// </summary>
        internal override void UpdateRelationshipAsPersisted()
        {
            IsRemoved = false;
            RemovedBO = null;
        }

        /// <summary>
        /// DereferenceChildren
        /// </summary>
        /// <param name="committer"></param>
        internal override void DereferenceChildren(TransactionCommitter committer)
        {
            DereferenceChild(committer, GetRelatedObject());
        }
        /// <summary>
        /// Delete Children
        /// </summary>
        /// <param name="committer"></param>
        internal override void DeleteChildren(TransactionCommitter committer)
        {
            DeleteChild(committer, GetRelatedObject());
        }

        internal override void DereferenceRemovedChildren(TransactionCommitter committer)
        {
            //DO Nothing Single relationship does not store children;
        }

        internal override void DeleteMarkedForDeleteChildren(TransactionCommitter committer)
        {
            //Do nothing single relationship does not have children
        }

        internal override void CancelEdits()
        {
            //throw new NotImplementedException();
        }

        internal override void AddDirtyChildrenToTransactionCommitter(TransactionCommitter transactionCommitter)
        {
            foreach (TBusinessObject businessObject in GetDirtyChildren())
            {
                transactionCommitter.AddBusinessObject(businessObject);
            }
            if (this.RelationshipDef.RelationshipType == RelationshipType.Association)
            {
                if (IsRelatedBOPropsDirty())
                {
//                    ISingleRelationship reverseRelationship = GetReverseRelationship(_relatedBo) as ISingleRelationship;
//                    if (reverseRelationship != null)
//                    {
                        transactionCommitter.AddTransaction
                            (new TransactionalSingleRelationship_Added(this, _relatedBo));
//                    }
                }
                else if (IsRemoved)
                {
//                    ISingleRelationship reverseRelationship = GetReverseRelationship(RemovedBO) as ISingleRelationship;
//                    if (reverseRelationship != null)
//                    {
                        transactionCommitter.AddTransaction
                            (new TransactionalSingleRelationship_Removed(this, this.RemovedBO));
//                    }
                }
            }
        }

        internal IList<TBusinessObject> GetDirtyChildren()
        {
            IList<TBusinessObject> dirtyChildren = new List<TBusinessObject>();
            if (MustAddToDirtyBusinessObjects()) dirtyChildren.Add(_relatedBo);
            if (MustAddRemovedBOToDirtyBusinessObjects()) dirtyChildren.Add(RemovedBO);
            return dirtyChildren;
        }
    }
}