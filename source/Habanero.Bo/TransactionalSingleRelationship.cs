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
using Habanero.BO.SqlGeneration;
using Habanero.DB;

namespace Habanero.BO
{
    internal abstract class TransactionalSingleRelationship : ITransactionalDB
    {
        private readonly IRelationship _relationship;
        private readonly IBusinessObject _relatedBO;
        private readonly string _transactionID;

        protected TransactionalSingleRelationship(IRelationship relationship, IBusinessObject relatedBO)
        {
            _transactionID = Guid.NewGuid().ToString();
            _relationship = relationship;
            _relatedBO = relatedBO;
        }

        public IRelationship Relationship { get { return _relationship; } }

        protected IBusinessObject RelatedBO
        {
            get { return _relatedBO; }
        }

        ///<summary>
        ///</summary>
        ///<returns>The ID that uniquelty identifies this item of the transaction. In the case of business objects the object Id.
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
            businessObject.UpdateDirtyStatusFromProperties();
        }

        protected abstract void UpdateCollections();

        ///<summary>
        /// updates the object as rolled back
        ///</summary>
        public void UpdateAsRolledBack() { }

        public virtual ISqlStatementCollection GetPersistSql()
        {
            UpdateStatementGenerator gen = new UpdateStatementGenerator(RelatedBO, DatabaseConnection.CurrentConnection);
            return gen.GenerateForRelationship(Relationship, RelatedBO);
        }
    }

    internal class TransactionalSingleRelationship_Added : TransactionalSingleRelationship
    {
        internal TransactionalSingleRelationship_Added(IRelationship singleRelationship, IBusinessObject relatedBO)
            : base(singleRelationship, relatedBO)
        {}

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

    internal class TransactionalSingleRelationship_Removed : TransactionalSingleRelationship
    {
        public TransactionalSingleRelationship_Removed(IRelationship singleRelationship , IBusinessObject relatedBO)
            : base(singleRelationship, relatedBO)
        {}

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