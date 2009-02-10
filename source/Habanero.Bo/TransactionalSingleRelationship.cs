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
        private ISingleRelationship _singleRelationship;
        private string _transactionID;

        public TransactionalSingleRelationship(ISingleRelationship singleRelationship)
        {
            _transactionID = Guid.NewGuid().ToString();
            _singleRelationship = singleRelationship;

        }

        public ISingleRelationship Relationship { get { return _singleRelationship; } }

        public string TransactionID()
        {
            return _transactionID;
        }

        public void UpdateStateAsCommitted()
        {
            BusinessObject businessObject = (BusinessObject) _singleRelationship.OwningBO;
            foreach (IRelPropDef relPropDef in _singleRelationship.RelationshipDef.RelKeyDef)
            {
                businessObject.Props[relPropDef.OwnerPropertyName].BackupPropValue();
            }
            UpdateCollections();
            ((Relationship)Relationship).UpdateRelationshipAsPersisted();
            businessObject.UpdateDirtyStatusFromProperties();
        }

        protected abstract void UpdateCollections();

        public void UpdateAsRolledBack() { }

        public virtual ISqlStatementCollection GetPersistSql()
        {
            UpdateStatementGenerator gen = new UpdateStatementGenerator(Relationship.OwningBO, DatabaseConnection.CurrentConnection);
            return gen.GenerateForRelationship(Relationship);
        }
    }

    internal class TransactionalSingleRelationship_Added : TransactionalSingleRelationship
    {
        public TransactionalSingleRelationship_Added(ISingleRelationship singleRelationship) : base(singleRelationship)
        {}

        protected override void UpdateCollections()
        {
            RelationshipBase relationshipBase = (RelationshipBase)Relationship;
            IMultipleRelationship reverseRelationship = relationshipBase.GetReverseRelationship(Relationship.GetRelatedObject()) as IMultipleRelationship;
            if (reverseRelationship != null)
            {
                reverseRelationship.BusinessObjectCollection.AddedBusinessObjects.Remove(Relationship.OwningBO);
            }
        }

    }

    internal class TransactionalSingleRelationship_Removed : TransactionalSingleRelationship
    {
        public TransactionalSingleRelationship_Removed(ISingleRelationship singleRelationship)
            : base(singleRelationship)
        {}

        protected override void UpdateCollections()
        {
            SingleRelationshipBase relationshipBase = (SingleRelationshipBase)Relationship;
            IMultipleRelationship reverseRelationship = relationshipBase.GetReverseRelationship(relationshipBase.RemovedBOInternal) as IMultipleRelationship;
            if (reverseRelationship != null)
            {
                IBusinessObjectCollection businessObjectCollection = reverseRelationship.BusinessObjectCollection;
                businessObjectCollection.RemovedBusinessObjects.Remove(Relationship.OwningBO);
                businessObjectCollection.PersistedBusinessObjects.Remove(Relationship.OwningBO);
            }
        }
    }
}