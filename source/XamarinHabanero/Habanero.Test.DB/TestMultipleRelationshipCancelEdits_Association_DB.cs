using Habanero.Test.BO.Relationship;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    [Ignore("Working on this")] //TODO Mark 24 Mar 2009: Ignored Tests - Working on this
    public class TestMultipleRelationshipCancelEdits_Association_DB : TestMultipleRelationshipCancelEdits_Association
    {
        protected override void SetupDataAccess()
        {
            TestUsingDatabase.SetupDBDataAccessor();
        }
    }
}