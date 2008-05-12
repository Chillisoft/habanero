using Habanero.Base;
using Habanero.UI.Base.FilterControl;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.FilterController
{
    [TestFixture]
    public class TestCompositeFilterClause
    {
        private IFilterClauseFactory itsFilterClauseFactory;

        [SetUp]
        public  void SetupTest()
        {
            //Runs every time that any testmethod is executed
            //base.SetupTest();
        }

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            itsFilterClauseFactory = new DataViewFilterClauseFactory();
        }
        [TearDown]
        public  void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
        }
        [Test]
        public void TestCompositeWithAnd()
        {
            IFilterClause stringFilterClause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumnString", FilterClauseOperator.OpEquals,
                                                                "testvalue");
            IFilterClause intFilterClause =
                itsFilterClauseFactory.CreateIntegerFilterClause("TestColumnInt", FilterClauseOperator.OpEquals, 12);
            IFilterClause compositeClause =
                itsFilterClauseFactory.CreateCompositeFilterClause(stringFilterClause,
                                                                   FilterClauseCompositeOperator.OpAnd, intFilterClause);
            Assert.AreEqual("(TestColumnString = 'testvalue') and (TestColumnInt = 12)",
                            compositeClause.GetFilterClauseString());
        }

        [Test]
        public void TestCompositeWithOr()
        {
            IFilterClause stringFilterClause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumnString", FilterClauseOperator.OpEquals,
                                                                "testvalue");
            IFilterClause intFilterClause =
                itsFilterClauseFactory.CreateIntegerFilterClause("TestColumnInt", FilterClauseOperator.OpEquals, 12);
            IFilterClause compositeClause =
                itsFilterClauseFactory.CreateCompositeFilterClause(stringFilterClause,
                                                                   FilterClauseCompositeOperator.OpOr, intFilterClause);
            Assert.AreEqual("(TestColumnString = 'testvalue') or (TestColumnInt = 12)",
                            compositeClause.GetFilterClauseString());
        }

        [Test]
        public void TestCompositeWithNullClauses()
        {
            IFilterClause nullFilterClause = itsFilterClauseFactory.CreateNullFilterClause();
            IFilterClause intFilterClause =
                itsFilterClauseFactory.CreateIntegerFilterClause("TestColumnInt", FilterClauseOperator.OpEquals, 12);
            IFilterClause compositeClause =
                itsFilterClauseFactory.CreateCompositeFilterClause(nullFilterClause, FilterClauseCompositeOperator.OpOr,
                                                                   intFilterClause);
            Assert.AreEqual("TestColumnInt = 12", compositeClause.GetFilterClauseString());
        }

    }
}
