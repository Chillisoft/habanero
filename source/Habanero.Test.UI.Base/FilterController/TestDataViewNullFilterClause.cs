using Habanero.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.FilterController
{
    [TestFixture]
    public class TestDataViewNullFilterClause//:TestBase
    {
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            //base.SetupTest();
        }
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }
        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
        }
        [Test]
        public void TestCreateNullFilterClause()
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory itsFilterClauseFactory = new DataViewFilterClauseFactory();
            //---------------Execute Test ----------------------
            IFilterClause filterClause =
                itsFilterClauseFactory.CreateNullFilterClause();
            //---------------Test Result -----------------------
            Assert.AreEqual("", filterClause.GetFilterClauseString());
        }

    }
}
