// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using Habanero.Base;

namespace Habanero.BO
{
    /// <summary>
    /// An <see cref="ITransactional"/> object for persisting a single relationship.
    /// This is used for single association relationships where the relationship has changed 
    /// and you wish the changed relationship to be persisted to the database.
    /// </summary>
    public abstract class TransactionalSingleRelationship : ITransactional
    {
        private readonly IRelationship _relationship;
        private readonly IBusinessObject _relatedBO;
        private readonly string _transactionID;
        /// <summary>
        /// Constructor for <see cref="TransactionalSingleRelationship"/>
        /// </summary>
        /// <param name="relationship"></param>
        /// <param name="relatedBO"></param>
        protected TransactionalSingleRelationship(IRelationship relationship, IBusinessObject relatedBO)
        {
            if (relatedBO == null) throw new ArgumentNullException("relatedBO");
            _transactionID = Guid.NewGuid().ToString();
            _relationship = relationship;
            _relatedBO = relatedBO;
        }
        /// <summary>
        /// Returns the Relationship for this <see cref="TransactionalSingleRelationship"/>
        /// </summary>
        public IRelationship Relationship { get { return _relationship; } }
        /// <summary>
        /// The Related <see cref="IBusinessObject"/> for this <see cref="TransactionalSingleRelationship"/>
        /// </summary>
        protected IBusinessObject RelatedBO
        {
            get { return _relatedBO; }
        }

        ///<summary>
        /// This is the ID that uniquely identifies this item of the transaction.
        /// This must be the same for the lifetime of the transaction object. 
        /// This assumption is relied upon for certain optimisations in the Transaction Comitter.
        ///</summary>
        ///<returns>The ID that uniquely identifies this item of the transaction. In the case of business objects the object Id.
        /// for non business objects that no natural id exists for the particular transactional item a guid that uniquely identifies 
        /// transactional item should be generated. This is used by the transaction committer to ensure that the transactional item
        /// is not added twice in error.</returns>
        public string TransactionID()
        {
            return _transactionID;
        }

        ///<summary>
        /// Updates the business object as committed
        ///</summary>
        public void UpdateStateAsCommitted()
        {
            BusinessObject businessObject = (BusinessObject)RelatedBO;
            foreach (IRelPropDef relPropDef in _relationship.RelationshipDef.RelKeyDef)
            {
                businessObject.Props[relPropDef.RelatedClassPropName].BackupPropValue();
            }
            UpdateCollections();
            ((Relationship)Relationship).UpdateRelationshipAsPersisted();
            //businessObject.UpdateDirtyStatusFromProperties();
        }
        /// <summary>
        /// Update the Underlying relationship collection e.g. The added or removed Business Object Collections are updated.
        /// </summary>
        protected abstract void UpdateCollections();

        ///<summary>
        /// updates the object as rolled back
        ///</summary>
        public void UpdateAsRolledBack() { }

    }
    /// <summary>
    /// A <see cref="TransactionalSingleRelationship"/> for an item that is in the Added Business Object collection.
    /// </summary>
    public class TransactionalSingleRelationship_Added : TransactionalSingleRelationship
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="singleRelationship"></param>
        /// <param name="relatedBO"></param>
        protected internal TransactionalSingleRelationship_Added(IRelationship singleRelationship, IBusinessObject relatedBO)
            : base(singleRelationship, relatedBO)
        {}

        /// <summary>
        /// Update the Underlying relationship collection e.g. The added or removed Business Object Collections are updated.
        /// </summary>
        protected override void UpdateCollections()
        {
//            RelationshipBase relationshipBase = (RelationshipBase)Relationship;
            IMultipleRelationship relationship = Relationship as IMultipleRelationship;
            if (relationship != null)
            {
                relationship.BusinessObjectCollection.AddedBusinessObjects.Remove(this.RelatedBO);
            }
        }
    }
    /// <summary>
    /// A <see cref="TransactionalSingleRelationship"/> for an item that is in the Removed Business Object collection.
    /// </summary>
    public class TransactionalSingleRelationship_Removed : TransactionalSingleRelationship
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="singleRelationship"></param>
        /// <param name="relatedBO"></param>
        protected internal TransactionalSingleRelationship_Removed(IRelationship singleRelationship, IBusinessObject relatedBO)
            : base(singleRelationship, relatedBO)
        {}

        /// <summary>
        /// Update the Underlying relationship collection e.g. The added or removed Business Object Collections are updated.
        /// </summary>
        protected override void UpdateCollections()
        {
//            SingleRelationshipBase relationshipBase = (SingleRelationshipBase)Relationship;
            IMultipleRelationship relationship = Relationship as IMultipleRelationship;
            if (relationship != null)
            {
                IBusinessObjectCollection businessObjectCollection = relationship.BusinessObjectCollection;
                businessObjectCollection.RemovedBusinessObjects.Remove(this.RelatedBO);
                businessObjectCollection.PersistedBusinessObjects.Remove(this.RelatedBO);
            }
        }
    }
}