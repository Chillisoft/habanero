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
using System.Collections.Generic;
using System.Linq;
using Habanero.Base;

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

        /// <summary>
        /// Handles the event of a Related Business Object <see cref="IBusinessObject"/> being changed
        /// </summary>
        event EventHandler RelatedBusinessObjectChanged;
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
        protected SingleRelationshipBase(IBusinessObject owningBo, IRelationshipDef lRelDef, IBOPropCol lBOPropCol)
            : base(owningBo, lRelDef, lBOPropCol)
        {
        }

        /// <summary>
        /// Removes the Business Object from the RelationshipBase.
        /// </summary>
        internal abstract IBusinessObject RemovedBOInternal { get; }

        ///<summary>
        /// Returns true if the Business object that owns this relationship has the foreign key and true otherwise.
        ///</summary>
        public abstract bool OwningBOHasForeignKey { get; set; }

        internal abstract void SetRelatedObjectFromMultiple(IBusinessObject relatedObject);
    }

    /// <summary>
    /// Manages a relationship where the relationship owner relates to one
    /// other object
    /// </summary>
    public class SingleRelationship<TBusinessObject> : SingleRelationshipBase, ISingleRelationship
        where TBusinessObject : class, IBusinessObject, new()
    {
        private TBusinessObject _relatedBo;
        private Criteria _storedKeyCriteria;

        /// <summary>
        /// Handles the event of a Related Business Object <see cref="IBusinessObject"/> being changed
        /// </summary>
        public event EventHandler RelatedBusinessObjectChanged;

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
        public SingleRelationship(IBusinessObject owningBo, IRelationshipDef lRelDef, IBOPropCol lBOPropCol)
            : base(owningBo, lRelDef, lBOPropCol)
        {
            this.RelKey.RelatedPropValueChanged += (sender, e) => FireRelatedBusinessObjectChangedEvent();
        }

        private void FireRelatedBusinessObjectChangedEvent()
        {
            if (RelatedBusinessObjectChanged == null) return;
            this.RelatedBusinessObjectChanged(this, new EventArgs());
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
            return RelKey.HasRelatedObject();
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
            return GetRelatedObject(false);
        }

        private TBusinessObject GetRelatedObject(bool isInternalAdd)
        {
            var newKeyCriteria = RelKey.Criteria;
            if (!HasRelatedObject())
            {
                _storedKeyCriteria = null;
                if (_relatedBo == null) return null;
                if (!newKeyCriteria.IsMatch(_relatedBo, false))
                {
                    SetRelatedBoReferenceInternal(null);
                }
                return _relatedBo;
            }

            if ((RelatedBoDoesNotMatchForeignKey() || _relatedBo == null))
            {
                var relatedBo = GetRelatedBusinessObjectFromBusinessObjectManager();
                SetRelatedBoReferenceInternal(relatedBo);
                if (RelKeyIsDirty()) AddToReverseRelationship(_relatedBo, isInternalAdd);
                if (_relatedBo != null) return _relatedBo;
            }
            if (_relatedBo != null && newKeyCriteria.IsMatch(_relatedBo, false))
            {
                return _relatedBo;
            }
            //Load Related Object from database 
            var criteriaHasChangedSinceLastLoad = _storedKeyCriteria == null ||
                                                  (_storedKeyCriteria != null &&
                                                   !_storedKeyCriteria.Equals(newKeyCriteria));
            if (criteriaHasChangedSinceLastLoad)
            {
                if (HasRelatedObject())
                {
                    // use non-generic one because of type parameters
                    var relatedBo =
                        (TBusinessObject)
                        BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObject((ISingleRelationship) this);

                    SetRelatedBoReferenceInternal(relatedBo);
                    if (RelKeyIsDirty()) AddToReverseRelationship(_relatedBo, isInternalAdd);
                    _storedKeyCriteria = newKeyCriteria;
                }
                else
                {
                    SetRelatedBoReferenceInternal(null);
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

        private bool RelKeyIsDirty()
        {
            var isRelKeyDirty = RelKey.Any(prop => prop.BOProp.IsDirty);
            return isRelKeyDirty;
        }

        private void SetRelatedBoReferenceInternal(TBusinessObject relatedBo)
        {
            UnRegisterForRelatedKeyPropChangeEvents();
            _relatedBo = relatedBo;
            RegisterForRelatedKeyPropChangeEvents();
        }

        private void RegisterForRelatedKeyPropChangeEvents()
        {
            if (!OwningBOHasForeignKey) return;
            ForEachRelatedBoKeyProp(boProp => boProp.Updated += RelatedBoKeyPropUpdated);
        }

        private void UnRegisterForRelatedKeyPropChangeEvents()
        {
            if (!OwningBOHasForeignKey) return;
            ForEachRelatedBoKeyProp(boProp => boProp.Updated -= RelatedBoKeyPropUpdated);
        }

        private void ForEachRelatedBoKeyProp(Action<IBOProp> action)
        {
            if (action == null) return;
            if (_relatedBo == null) return;
            foreach (IRelProp prop in this.RelKey)
            {
                var boProp = _relatedBo.Props[prop.RelatedClassPropName];
                action(boProp);
            }
        }

        private void RelatedBoKeyPropUpdated(object sender, BOPropEventArgs boPropEventArgs)
        {
            UpdateForeignKeyAndStoredRelationshipExpression();
        }

        private TBusinessObject GetRelatedBusinessObjectFromBusinessObjectManager()
        {
//            BusinessObjectCollection<TBusinessObject> relatedBOCol =
//                BusinessObjectManager.Instance.Find<TBusinessObject>(_relKey.Criteria);
//            TBusinessObject relatedBo = null;
//            if (relatedBOCol.Count == 1)
//            {
//                relatedBo = relatedBOCol[0] == relatedBo ? null : relatedBOCol[0];
//            }
			return (TBusinessObject)BORegistry.BusinessObjectManager.FindFirst<TBusinessObject>(RelKey.Criteria);
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
            IBusinessObject relatedObject = this.GetRelatedObject();
            if (relatedObject != null && !relatedObject.Status.IsDeleted)
            {
                if (!String.IsNullOrEmpty(this._relDef.PreventDeleteMessage))
                    message = this._relDef.PreventDeleteMessage.Replace("$(object)", this._owningBo.ToString());
                else
                    message =
                    string.Format(
                        "You cannot delete {0} identified by {1} or {2} since it is related to a Business Object via the {3} relationship",
                        this.OwningBO.ClassDef.ClassName, this.OwningBO.ID.AsString_CurrentValue(), this.OwningBO.ToString(),
                        this.RelationshipName);
                return false;
            }
            return true;
        }

        /// <summary>
        /// If the relationship is <see cref="IBusinessObject.MarkForDelete"/>.DeleteRelated then
        /// all the related objects and their relevant children will be marked for Delete.
        /// See <see cref="IRelationship.DeleteParentAction"/>
        /// </summary>
        public override void MarkForDelete()
        {
            if (this.DeleteParentAction != DeleteParentAction.DeleteRelated) return;
            TBusinessObject relatedObject = this.GetRelatedObject();
            if (relatedObject!= null && !relatedObject.Status.IsDeleted)
            {
                relatedObject.MarkForDelete();
            }
        }

        private bool RelatedBoDoesNotMatchForeignKey()
        {
            if (_relatedBo != null)
            {
                foreach (IRelProp prop in this.RelKey)
                {
                    object relatedPropValue = _relatedBo.GetPropertyValue(prop.RelatedClassPropName);
                    object propValue = prop.BOProp.Value;
                    if (propValue == null)
                    {
                        if (relatedPropValue == null) continue;
                        return true;
                    }
                    if (propValue.Equals(relatedPropValue)) continue;
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
            SetRelatedObjectInternal(relatedObject, false);
        }

        internal override void SetRelatedObjectFromMultiple(IBusinessObject relatedObject)
        {
            SetRelatedObjectInternal((TBusinessObject)relatedObject, true);
        }
        
        internal void SetRelatedObjectInternal(TBusinessObject relatedObject, bool isInternalAdd)
        {
            if (_relatedBo == null) GetRelatedObject(isInternalAdd);
            if (_relatedBo == relatedObject) return;

            if (relatedObject != null) RelationshipDef.CheckCanAddChild(relatedObject);
            if (_relatedBo != null) RelationshipDef.CheckCanRemoveChild(_relatedBo);

            RemoveFromReverseRelationship(_relatedBo);

            SetRelatedBoReferenceInternal(relatedObject);

            AddToReverseRelationship(relatedObject, isInternalAdd);

            UpdateForeignKeyAndStoredRelationshipExpression();

            FireUpdatedEvent();
        }

        private void FireUpdatedEvent()
        {
            if (Updated != null) this.Updated(this, new BOEventArgs<TBusinessObject>(_relatedBo));
        }

        private void AddToReverseRelationship(TBusinessObject relatedObject, bool isInternalAdd)
        {
            if (relatedObject == null) return;

            IRelationship reverseRelationship = GetReverseRelationship(relatedObject);
            if (reverseRelationship == null) return;
            //If related Object belongs to this relationship then you do not need to CheckCanAddChild.
            if (!reverseRelationship.RelKey.Criteria.IsMatch(this.OwningBO, false))
            {
                reverseRelationship.RelationshipDef.CheckCanAddChild(this.OwningBO);
            }

            SetRelatedBoReferenceInternal(relatedObject);

            AddToMultipleReverseRelationship(reverseRelationship, isInternalAdd);
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

        private void AddToMultipleReverseRelationship(IRelationship reverseRelationship, bool isInternalAdd)
        {
            MultipleRelationshipBase multipleRelationship = reverseRelationship as MultipleRelationshipBase;
            if (multipleRelationship != null)
            {
                IBusinessObjectCollectionInternal colInternal = multipleRelationship.GetLoadedBOColInternal();
                if (isInternalAdd) colInternal.AddInternal(this.OwningBO);
                else colInternal.Add(this.OwningBO);
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

                SetRelatedBoReferenceInternal(null);

                UpdateForeignKeyAndStoredRelationshipExpression();

                RemoveFromMultipleReverseRelationship(reverseRelationship);
                RemoveFromSingleReverseRelationship(reverseRelationship);
            }
            else if (!this.OwningBOHasForeignKey)
            {
				foreach (RelProp relProp in RelKey)
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

        private void UpdateForeignKeyAndStoredRelationshipExpression()
        {
        	var relKey = RelKey;
        	if (this.OwningBOHasForeignKey)
            {
				foreach (RelProp relProp in relKey)
                {
                    object relatedObjectValue = _relatedBo == null
                                                    ? null
                                                    : _relatedBo.GetPropertyValue(relProp.RelatedClassPropName);
                    _owningBo.SetPropertyValue(relProp.OwnerPropertyName, relatedObjectValue);
                }
            }
            else if (!HasReverseRelationshipDefined(this))
            {
				foreach (RelProp relProp in relKey)
                {
                    object owningBOValue = _owningBo == null
                                               ? null
                                               : _owningBo.GetPropertyValue(relProp.OwnerPropertyName);
                    if (_relatedBo != null) _relatedBo.SetPropertyValue(relProp.RelatedClassPropName, owningBOValue);
                }
            }
			_storedKeyCriteria = relKey.Criteria;
        }

        private bool IsRelatedBODirty()
        {
            return _relatedBo != null && _relatedBo.Status.IsDirty;
        }

        private bool MustAddToDirtyBusinessObjects()
        {
            return IsRelatedBODirty() && (IsRelationshipCompositionOrAggregation()
                                          || _relatedBo.Status.IsNew || _relatedBo.Status.IsDeleted);
        }

        private bool IsRelationshipCompositionOrAggregation()
        {
            return (IsRelationshipAggregation() || IsRelationshipComposition());
        }

        private bool IsRelationshipComposition()
        {
            return this.RelationshipDef.RelationshipType == RelationshipType.Composition;
        }

        private bool IsRelationshipAggregation()
        {
            return this.RelationshipDef.RelationshipType == RelationshipType.Aggregation;
        }

        private bool MustAddRemovedBOToDirtyBusinessObjects()
        {
            return IsRemoved && RemovedBO != null && RemovedBO.Status.IsNew == false && IsRelationshipCompositionOrAggregation();
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
            TBusinessObject businessObject = GetRelatedObject();
            if (businessObject != null) DereferenceChild(committer, businessObject);
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
            foreach (var businessObject in GetDirtyChildren())
            {
                if (this.OwningBOHasForeignKey)
                    transactionCommitter.InsertBusinessObject(businessObject);
                else
                    transactionCommitter.AddBusinessObject(businessObject);
            }
            if (this.RelationshipDef.RelationshipType == RelationshipType.Association)
            {
                if (IsRelatedBOPropsDirty() && _relatedBo != null && !_relatedBo.Status.IsNew)
                {
                    transactionCommitter.AddAddedChildBusinessObject(this, _relatedBo);
                }
                else if (IsRemoved)
                {
                    transactionCommitter.AddRemovedChildBusinessObject(this, RemovedBO);
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