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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{

    internal static class RelationshipUtils
    {
        /// <summary>
        /// Creates a <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/> with boType as its type parameter, using the Activator.
        /// </summary>
        /// <param name="boType">The type parameter to be used</param>
        /// <param name="relationship">The relationship that this <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/> is the collection for</param>
        /// <returns>The instantiated <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/></returns>
        public static IBusinessObjectCollection CreateRelatedBusinessObjectCollection(Type boType, IMultipleRelationship relationship)
        {
            IBusinessObjectCollection collection = CreateNewRelatedBusinessObjectCollection(boType, relationship);
            SetupCriteriaForRelationship(relationship, collection);
            return collection;
        }

        /// <summary>
        /// Creates a <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/> with boType as its type parameter, using the Activator.
        /// </summary>
        /// <param name="boType">The type parameter to be used</param>
        /// <param name="relationship">The relationship that this <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/> is the collection for</param>
        /// <returns>The instantiated <see cref="RelatedBusinessObjectCollection{TBusinessObject}"/></returns>
        internal static IBusinessObjectCollection CreateNewRelatedBusinessObjectCollection(Type boType, IRelationship relationship)
        {
//            Utilities.CheckTypeCanBeCreated(boType);
            Type relatedCollectionType = typeof(RelatedBusinessObjectCollection<>);
            relatedCollectionType = relatedCollectionType.MakeGenericType(boType);
            IBusinessObjectCollection collection = (IBusinessObjectCollection)Activator.CreateInstance(relatedCollectionType, relationship);
            return collection;
        }

        internal static void SetupCriteriaForRelationship(IMultipleRelationship relationship, IBusinessObjectCollection collection)
        {
            Criteria relationshipCriteria = Criteria.FromRelationship(relationship);

            OrderCriteria preparedOrderCriteria =
                QueryBuilder.CreateOrderCriteria(relationship.RelatedObjectClassDef, relationship.OrderCriteria.ToString());

            //QueryBuilder.PrepareCriteria(relationship.RelatedObjectClassDef, relationshipCriteria);
            collection.SelectQuery.Criteria = relationshipCriteria;
            collection.SelectQuery.OrderCriteria = preparedOrderCriteria;
        }

        internal static void CheckCorrespondingSingleRelationshipsAreValid(SingleRelationshipBase singleRelationship, SingleRelationshipBase singleReverseRelationship)
        {
            if (singleRelationship.OwningBOHasForeignKey && singleReverseRelationship.OwningBOHasForeignKey)
            {
                string message = String.Format("The corresponding single (one to one) relationships " +
                                               "{0} (on {1}) and {2} (on {3}) cannot both be configured as having the foreign key " 
                                               + "(OwningBOHasForeignKey). Please set this property to false on one of these relationships " 
                                               + "(In the ClassDefs.XML or in Firestarter.",
                                               singleRelationship.RelationshipName, singleRelationship.OwningBO.GetType().Name,
                                               singleReverseRelationship.RelationshipName,
                                               singleRelationship.RelatedObjectClassDef.ClassName);
                throw new HabaneroDeveloperException(message, "");
            }
        }
    }

    ///<summary>
    /// This provides a base class from which all relationship classes inherit.
    ///</summary>
    public abstract class RelationshipBase : IRelationship
    {
        protected IRelKey _relKey;

        ///<summary>
        /// The key that identifies this relationship i.e. the properties in the 
        /// source object and how they are related to properties in the related object.
        ///</summary>
        public abstract IRelKey RelKey { get; }

        /// <summary>
        /// The class Definition for the related object.
        /// </summary>
        public abstract IClassDef RelatedObjectClassDef { get; }

        ///<summary>
        /// Returns whether the relationship is dirty or not.
        /// A relationship is always dirty if it has Added, created, removed or deleted Related business objects.
        /// If the relationship is of type composition or aggregation then it is dirty if it has any 
        ///  related (children) business objects that are dirty.
        ///</summary>
        public abstract bool IsDirty { get; }

        /// <summary>
        /// Returns the relationship definition
        /// </summary>
        public abstract IRelationshipDef RelationshipDef { get; }
        /// <summary>
        /// Returns the relationship name
        /// </summary>
        public abstract string RelationshipName { get; }
        ///<summary>
        /// Is the relationship initialised.
        ///</summary>
        public abstract bool Initialised { get; }
        ///<summary>
        /// Returns the appropriate delete action when the parent is deleted.
        /// i.e. delete related objects, dereference related objects, prevent deletion.
        ///</summary>
        public abstract DeleteParentAction DeleteParentAction { get; }

        ///<summary>
        /// Returns the business object that owns this relationship e.g. Invoice has many lines
        /// the owning BO would be invoice.
        ///</summary>
        public abstract IBusinessObject OwningBO { get; }

        internal abstract void CancelEdits();

        internal abstract void AddDirtyChildrenToTransactionCommitter(TransactionCommitter committer);

        internal abstract void DereferenceChildren(TransactionCommitter committer);

        internal abstract void DeleteChildren(TransactionCommitter committer);

        internal abstract void DereferenceRemovedChildren(TransactionCommitter committer);

        internal abstract void DeleteMarkedForDeleteChildren(TransactionCommitter committer);

        /// <summary>
        /// Returns the reverse relationship for this relationship i.e. If invoice has invoice lines and you 
        /// can navigate from invoice lines to invoices then the invoicelines to invoice relationship is the
        /// reverse relationship of the invoice to invoicelines relationship and vica versa.
        /// </summary>
        /// <param name="bo">The related Business object (in the example the invoice lines)</param>
        /// <returns>The reverse relationship or null if no reverse relationship is set up.</returns>
        internal IRelationship GetReverseRelationship(IBusinessObject bo)
        {
            if (bo == null) return null;
            if (HasReverseRelationshipDefined(this))
            {
                foreach (IRelationship relationship in bo.Relationships)
                {
                    if (relationship.RelationshipName == this.RelationshipDef.ReverseRelationshipName)
                        return relationship;
                }
                throw new HabaneroDeveloperException(
                    String.Format(
                        "The relationship '{0}' on class '{1}' has a reverse relationship defined ('{2}'), but this " +
                        "relationship was not found on the related object. " +
                        "Please check that the reverse relationship is defined correctly " +
                        "and that it exists on the related class '{3}'",
                        this.RelationshipName, this.OwningBO.ClassDef.ClassName,
                        this.RelationshipDef.ReverseRelationshipName,
                        this.RelatedObjectClassDef.ClassName), "");
            }
            return null;
            //return FindRelationshipByRelationshipKey(bo);
        }
        /// <summary>
        /// Returns true if htis relationship has a reverse relationship defined. False otherwise.
        /// </summary>
        /// <param name="relationship"></param>
        /// <returns></returns>
        protected static bool HasReverseRelationshipDefined(IRelationship relationship)
        {
            return !String.IsNullOrEmpty(relationship.RelationshipDef.ReverseRelationshipName);
        }
    }
    
    /// <summary>
    /// Provides a super-class for relationships between business objects
    /// </summary>
    public abstract class Relationship : RelationshipBase
    {
        protected RelationshipDef _relDef;
        protected readonly IBusinessObject _owningBo;
        private bool _initialised;

        /// <summary>
        /// Constructor to initialise a new relationship
        /// </summary>
        /// <param name="owningBo">The business object from where the 
        /// relationship originates</param>
        /// <param name="lRelDef">The relationship definition</param>
        /// <param name="lBOPropCol">The set of properties used to
        /// initialise the RelKey object</param>
        protected Relationship(IBusinessObject owningBo, RelationshipDef lRelDef, BOPropCol lBOPropCol)
        {
            _relDef = lRelDef;
            _owningBo = owningBo;
            _relKey = _relDef.RelKeyDef.CreateRelKey(lBOPropCol);
        }

        /// <summary>
        /// Returns the relationship name
        /// </summary>
        public override string RelationshipName
        {
            get { return _relDef.RelationshipName; }
        }

        /// <summary>
        /// Returns the relationship definition
        /// </summary>
        public override IRelationshipDef RelationshipDef
        {
            get { return _relDef; }
        }

        ///<summary>
        /// Returns the appropriate delete action when the parent is deleted.
        /// i.e. delete related objects, dereference related objects, prevent deletion.
        ///</summary>
        public override DeleteParentAction DeleteParentAction
        {
            get { return _relDef.DeleteParentAction; }
        }


        ///<summary>
        /// Returns the business object that owns this relationship e.g. Invoice has many lines
        /// the owning BO would be invoice.
        ///</summary>
        public override IBusinessObject OwningBO
        {
            get { return _owningBo; }
        }

        ///<summary>
        /// The key that identifies this relationship i.e. the properties in the 
        /// source object and how they are related to properties in the related object.
        ///</summary>
        public override IRelKey RelKey
        {
            get { return _relKey; }
        }

        /// <summary>
        /// The class Definition for the related object.
        /// </summary>
        public override IClassDef RelatedObjectClassDef
        {
            get { return _relDef.RelatedObjectClassDef; }
        }

        //TODO: This should be temporary code and will b removed when define reverse relationships in Firestarter and classdefs.

        internal void Initialise()
        {
            if (_initialised) return;
            DoInitialisation();
            _initialised = true;
        }

        protected abstract void DoInitialisation();
        ///<summary>
        /// Is the relationship initialised or not.
        ///</summary>
        public override bool Initialised { get { return _initialised; } }

        protected void DereferenceChild(TransactionCommitter committer, IBusinessObject bo)
        {
            foreach (RelPropDef relPropDef in RelationshipDef.RelKeyDef)
            {
                bo.SetPropertyValue(relPropDef.RelatedClassPropName, null);
            }
            if (bo.Status.IsNew) return;
            committer.ExecuteTransactionToDataSource(committer.CreateTransactionalBusinessObject(bo));
        }
        
        protected void DeleteChild(TransactionCommitter committer, IBusinessObject bo)
        {
            if (bo == null) return;
            bo.Delete();
//            if (bo.Status.IsNew) return;
            committer.ExecuteTransactionToDataSource(committer.CreateTransactionalBusinessObject(bo));
        }

        internal abstract void UpdateRelationshipAsPersisted();
    }

    
}