using Habanero.BO;
using Habanero.DB;
using Habanero.Test.BO;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestBusinessObjectLookupListDB : TestBusinessObjectLookupList
    {
        protected override void SetupDataAccessor()
        {
            BORegistry.DataAccessor = new DataAccessorDB();
        }

        protected override void DeleteAllContactPeople()
        {
            ContactPersonTestBO.DeleteAllContactPeople();
        }
    }
}