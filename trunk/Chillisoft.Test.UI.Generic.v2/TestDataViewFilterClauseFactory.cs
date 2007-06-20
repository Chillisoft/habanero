using Habanero.Generic;
using Habanero.Ui.Generic;
using Habanero.Util;
using NUnit.Framework;

namespace Chillisoft.Test.UI.Generic.v2
{
    /// <summary>
    /// Summary description for TestDataviewFilterClauseBuilder.
    /// </summary>
    [TestFixture]
    public class TestDataViewFilterClauseFactory
    {
        FilterClauseFactory itsFilterClauseFactory;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            itsFilterClauseFactory = new DataViewFilterClauseFactory();
        }

        [Test]
        public void TestEqualsWithString()
        {
            FilterClause filterClause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals, "testvalue");
            Assert.AreEqual("TestColumn = 'testvalue'", filterClause.GetFilterClauseString());
        }

        [Test]
        public void TestEqualsWithInteger()
        {
            FilterClause filterClause =
                itsFilterClauseFactory.CreateIntegerFilterClause("TestColumn", FilterClauseOperator.OpEquals, 12);
            Assert.AreEqual("TestColumn = 12", filterClause.GetFilterClauseString());
        }

        [Test]
        public void TestWithColumnNameMoreThanOneWord()
        {
            FilterClause filterClause =
                itsFilterClauseFactory.CreateIntegerFilterClause("Test Column", FilterClauseOperator.OpEquals, 12);
            Assert.AreEqual("[Test Column] = 12", filterClause.GetFilterClauseString());
        }

        [Test]
        public void TestLikeWithString()
        {
            FilterClause filterClause =
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
            FilterClause stringFilterClause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumnString", FilterClauseOperator.OpEquals,
                                                                "testvalue");
            FilterClause intFilterClause =
                itsFilterClauseFactory.CreateIntegerFilterClause("TestColumnInt", FilterClauseOperator.OpEquals, 12);
            FilterClause compositeClause =
                itsFilterClauseFactory.CreateCompositeFilterClause(stringFilterClause,
                                                                   FilterClauseCompositeOperator.OpAnd, intFilterClause);
            Assert.AreEqual("(TestColumnString = 'testvalue') and (TestColumnInt = 12)",
                            compositeClause.GetFilterClauseString());
        }

        [Test]
        public void TestCompositeWithOr()
        {
            FilterClause stringFilterClause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumnString", FilterClauseOperator.OpEquals,
                                                                "testvalue");
            FilterClause intFilterClause =
                itsFilterClauseFactory.CreateIntegerFilterClause("TestColumnInt", FilterClauseOperator.OpEquals, 12);
            FilterClause compositeClause =
                itsFilterClauseFactory.CreateCompositeFilterClause(stringFilterClause,
                                                                   FilterClauseCompositeOperator.OpOr, intFilterClause);
            Assert.AreEqual("(TestColumnString = 'testvalue') or (TestColumnInt = 12)",
                            compositeClause.GetFilterClauseString());
        }

        [Test]
        public void TestCompositeWithNullClauses()
        {
            FilterClause nullFilterClause = itsFilterClauseFactory.CreateNullFilterClause();
            FilterClause intFilterClause =
                itsFilterClauseFactory.CreateIntegerFilterClause("TestColumnInt", FilterClauseOperator.OpEquals, 12);
            FilterClause compositeClause =
                itsFilterClauseFactory.CreateCompositeFilterClause(nullFilterClause, FilterClauseCompositeOperator.OpOr,
                                                                   intFilterClause);
            Assert.AreEqual("TestColumnInt = 12", compositeClause.GetFilterClauseString());
        }

        [Test]
        public void TestEqualsWithSingleQuote()
        {
            FilterClause filterClause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals,
                                                                "test'value");
            Assert.AreEqual("TestColumn = 'test''value'", filterClause.GetFilterClauseString());
        }
    }
}