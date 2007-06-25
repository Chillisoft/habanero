using Habanero.Base;
using Habanero.Ui.Grid;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Ui.Generic
{
    /// <summary>
    /// Summary description for TestDataviewFilterClauseBuilder.
    /// </summary>
    [TestFixture]
    public class TestDataViewFilterClauseFactory
    {
        IFilterClauseFactory itsFilterClauseFactory;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            itsFilterClauseFactory = new DataViewFilterClauseFactory();
        }

        [Test]
        public void TestEqualsWithString()
        {
            IFilterClause filterClause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals, "testvalue");
            Assert.AreEqual("TestColumn = 'testvalue'", filterClause.GetFilterClauseString());
        }

        [Test]
        public void TestEqualsWithInteger()
        {
            IFilterClause filterClause =
                itsFilterClauseFactory.CreateIntegerFilterClause("TestColumn", FilterClauseOperator.OpEquals, 12);
            Assert.AreEqual("TestColumn = 12", filterClause.GetFilterClauseString());
        }

        [Test]
        public void TestWithColumnNameMoreThanOneWord()
        {
            IFilterClause filterClause =
                itsFilterClauseFactory.CreateIntegerFilterClause("Test Column", FilterClauseOperator.OpEquals, 12);
            Assert.AreEqual("[Test Column] = 12", filterClause.GetFilterClauseString());
        }

        [Test]
        public void TestLikeWithString()
        {
            IFilterClause filterClause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpLike, "testvalue");
            Assert.AreEqual("TestColumn like '*testvalue*'", filterClause.GetFilterClauseString());
        }

        [
            Test,
                ExpectedException(typeof (HabaneroArgumentException),
                    "The argument 'clauseOperator' is not valid. Operator Like is not supported for non string operands")]
        public void TestLikeWithNonString()
        {
            itsFilterClauseFactory.CreateIntegerFilterClause("TestColumn", FilterClauseOperator.OpLike, 11);
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

        [Test]
        public void TestEqualsWithSingleQuote()
        {
            IFilterClause filterClause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals,
                                                                "test'value");
            Assert.AreEqual("TestColumn = 'test''value'", filterClause.GetFilterClauseString());
        }
    }
}