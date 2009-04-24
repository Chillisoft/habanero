using Habanero.Base;
using Habanero.BO;

namespace Habanero.DB
{
    internal class TransactionalSingleRelationship_Added_DB : TransactionalSingleRelationship_Added, ITransactionalDB
    {
        internal TransactionalSingleRelationship_Added_DB(IRelationship singleRelationship, IBusinessObject relatedBO)
            : base(singleRelationship, relatedBO)
        { }

        public virtual ISqlStatementCollection GetPersistSql()
        {
            UpdateStatementGenerator gen = new UpdateStatementGenerator(RelatedBO, DatabaseConnection.CurrentConnection);
            return gen.GenerateForRelationship(Relationship, RelatedBO);
        }
    }
}