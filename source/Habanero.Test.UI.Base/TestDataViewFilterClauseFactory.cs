//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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

using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
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
            Test, ExpectedException(typeof (HabaneroArgumentException), ExpectedMessage = 
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