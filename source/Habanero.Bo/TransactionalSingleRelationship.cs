using System;
using Habanero.Base;

namespace Habanero.BO
{
    internal abstract class TransactionalSingleRelationship : ITransactional
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
    }

    internal class TransactionalSingleRelationship_Added : TransactionalSingleRelationship
    {
        public TransactionalSingleRelationship_Added(ISingleRelationship singleRelationship) : base(singleRelationship)
        {}

        protected override void UpdateCollections()
        {
            RelationshipBase relationshipBase = (RelationshipBase)Relationship;
            IMultipleRelationship reverseRelationship = (IMultipleRelationship)relationshipBase.GetReverseRelationship(Relationship.GetRelatedObject());
            reverseRelationship.BusinessObjectCollection.AddedBusinessObjects.Remove(Relationship.OwningBO);
        
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
            IMultipleRelationship reverseRelationship = (IMultipleRelationship)relationshipBase.GetReverseRelationship(relationshipBase.RemovedBOInternal);
            IBusinessObjectCollection businessObjectCollection = reverseRelationship.BusinessObjectCollection;
            businessObjectCollection.RemovedBusinessObjects.Remove(Relationship.OwningBO);
            businessObjectCollection.PersistedBusinessObjects.Remove(Relationship.OwningBO);
        }
    }
}