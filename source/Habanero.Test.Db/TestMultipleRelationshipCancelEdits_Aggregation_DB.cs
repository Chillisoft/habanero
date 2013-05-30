using Habanero.Test.BO.Relationship;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestMultipleRelationshipCancelEdits_Aggregation_DB : TestMultipleRelationshipCancelEdits_Aggregation
    {
        protected override void SetupDataAccess()
        {
            TestUsingDatabase.SetupDBDataAccessor();
        }
    }
}