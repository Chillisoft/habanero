//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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

using System;
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
        public void Test_Search_EqualsWithString()
        {         

            //---------------Execute Test ----------------------
            IFilterClause filterClause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals, "testvalue");
            //---------------Test Result -----------------------
            Assert.AreEqual("TestColumn = 'testvalue'", filterClause.GetFilterClauseString("%",""));
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_Search_EqualsWithInteger()
        {
            //---------------Execute Test ----------------------
            IFilterClause filterClause =
                itsFilterClauseFactory.CreateIntegerFilterClause("TestColumn", FilterClauseOperator.OpEquals, 12);
            //---------------Test Result -----------------------
            Assert.AreEqual("TestColumn = 12", filterClause.GetFilterClauseString("%", ""));
        }
        public enum PurchaseOrderStatus
        {
            Processed
        }
        [Test]
        public void Test_FilterClause_WithEnum()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IFilterClause filterClause =
    itsFilterClauseFactory.CreateEnumFilterClause("TestColumn", FilterClauseOperator.OpEquals, PurchaseOrderStatus.Processed);

            //---------------Test Result -----------------------
            string expectedFilterClause = string.Format("TestColumn = '{0}'", PurchaseOrderStatus.Processed);
            Assert.AreEqual(expectedFilterClause, filterClause.GetFilterClauseString("%", "'"));
        }
        [Test]
        public void Test_Search_LikeWithString()
        {
            IFilterClause filterClause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpLike, "testvalue");
            Assert.AreEqual("TestColumn like '%testvalue%'", filterClause.GetFilterClauseString("%", ""));
        }

        [Test]
        public void Test_Search_DateEquals()
        {
            DateTime filterValue = DateTime.Now.AddDays(-2);
            //---------------Execute Test ----------------------
            IFilterClause filterClause =
                itsFilterClauseFactory.CreateDateFilterClause("TestColumn", FilterClauseOperator.OpEquals, filterValue);
            //---------------Test Result -----------------------
            string expectedFilterClause = string.Format("TestColumn = '{0}'", filterValue.ToString("dd MMM yyyy HH:mm:ss"));
            Assert.AreEqual(expectedFilterClause, filterClause.GetFilterClauseString("%", "'"));
        }








        [Test]
        public void TestDateEquals()
        {
            DateTime filterValue = DateTime.Now.AddDays(-2);
            //---------------Execute Test ----------------------
            IFilterClause filterClause =
                itsFilterClauseFactory.CreateDateFilterClause("TestColumn", FilterClauseOperator.OpEquals, filterValue);
            //---------------Test Result -----------------------
            string expectedFilterClause = string.Format("TestColumn = #{0}#", filterValue.ToString("dd MMM yyyy HH:mm:ss"));
            Assert.AreEqual(expectedFilterClause, filterClause.GetFilterClauseString("%", "#"));
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
        public void TestEqualsWithSingleQuote()
        {
            IFilterClause filterClause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals,
                                                                "test'value");
            Assert.AreEqual("TestColumn = 'test''value'", filterClause.GetFilterClauseString());
        }
    }
}