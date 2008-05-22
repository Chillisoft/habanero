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

using System;
using System.Data;
using Habanero.Base;
using Habanero.UI.Base.FilterControl;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// Summary description for TestDataViewFilterClauseBuilderWithStrings.
    /// </summary>
    [TestFixture]
    public class TestDataViewFilterClauseFactoryWithStrings
    {
        private DataView dv;
        private DataViewFilterClauseFactory filterClauseFactory;

        [TestFixtureSetUp]
        public void SetupDataView()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("h a");
            dt.Rows.Add(new object[] {"Peter"});
            dt.Rows.Add(new object[] {"Kelly"});
            dt.Rows.Add(new object[] {"Yorishma"});
            dt.Rows.Add(new object[] {"Doutch"});
            dv = dt.DefaultView;
            filterClauseFactory = new DataViewFilterClauseFactory();
        }

        [Test]
        public void TestEquals()
        {
            dv.RowFilter =
                filterClauseFactory.CreateStringFilterClause("h a", FilterClauseOperator.OpEquals, "Peter").
                    GetFilterClauseString();
            Assert.AreEqual(1, dv.Count);
        }

        [Test]
        public void TestLike()
        {
            dv.RowFilter =
                filterClauseFactory.CreateStringFilterClause("h a", FilterClauseOperator.OpLike, "e").
                    GetFilterClauseString();
            Assert.AreEqual(2, dv.Count);
        }

        [Test]
        public void Test_Search_CompositeEqualsWithAnd()
        {
            //---------------Set up test pack-------------------

            IFilterClause clause1 =
                filterClauseFactory.CreateStringFilterClause("h a", FilterClauseOperator.OpLike, "Peter");
            IFilterClause clause2 =
                filterClauseFactory.CreateStringFilterClause("h a", FilterClauseOperator.OpLike, "Kelly");
            IFilterClause compositeClause =
                filterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);

            //---------------Execute Test ----------------------

            string expectedFilterString = compositeClause.GetFilterClauseString("%","'");
            //---------------Test Result -----------------------
            Assert.AreEqual("([h a] like '%Peter%') and ([h a] like '%Kelly%')", expectedFilterString);
        }

        [Test]
        public void Test_Filter_CompositeEqualsWithAnd()
        {
            //---------------Set up test pack-------------------

            IFilterClause clause1 =
                filterClauseFactory.CreateStringFilterClause("h a", FilterClauseOperator.OpLike, "Peter");
            IFilterClause clause2 =
                filterClauseFactory.CreateStringFilterClause("h a", FilterClauseOperator.OpLike, "Kelly");
            IFilterClause compositeClause =
                filterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);

            //---------------Execute Test ----------------------

            string expectedFilterString = compositeClause.GetFilterClauseString();
            //---------------Test Result -----------------------
            Assert.AreEqual("([h a] like '*Peter*') and ([h a] like '*Kelly*')", expectedFilterString);
        }

        public void Test_Search_CompositeEqualsWithAnd_Date()
        {
            //---------------Set up test pack-------------------

            IFilterClause clause1 =
                filterClauseFactory.CreateDateFilterClause("h a", FilterClauseOperator.OpEquals, DateTime.Now);
            IFilterClause clause2 =
                filterClauseFactory.CreateDateFilterClause("h a", FilterClauseOperator.OpEquals, DateTime.Now);
            IFilterClause compositeClause =
                filterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);

            string expectedDateString = DateTime.Now.ToString("dd MMM yyyy HH:mm:ss");
            string expectedDateFilterClause = string.Format("([h a] = '{0}') and ([h a] = '{0}')", expectedDateString);
            //---------------Execute Test ----------------------

            string expectedFilterString = compositeClause.GetFilterClauseString("%", "'");
            //---------------Test Result -----------------------

            Assert.AreEqual(expectedDateFilterClause, expectedFilterString);
        }

        [Test]
        public void Test_Filter_CompositeEqualsWithAnd_Date()
        {
            //---------------Set up test pack-------------------

            IFilterClause clause1 =
                filterClauseFactory.CreateDateFilterClause("h a", FilterClauseOperator.OpEquals, DateTime.Now);
            IFilterClause clause2 =
                filterClauseFactory.CreateDateFilterClause("h a", FilterClauseOperator.OpEquals, DateTime.Now);
            IFilterClause compositeClause =
                filterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);

            string expectedDateString = DateTime.Now.ToString("dd MMM yyyy HH:mm:ss");
            string expectedDateFilterClause = string.Format("([h a] = #{0}#) and ([h a] = #{0}#)", expectedDateString);
            //---------------Execute Test ----------------------

            string filterString = compositeClause.GetFilterClauseString();
            //---------------Test Result -----------------------

            Assert.AreEqual(expectedDateFilterClause, filterString);
        }



        [Test]
        public void TestCompositeEqualsWithAnd()
        {
            IFilterClause clause1 =
                filterClauseFactory.CreateStringFilterClause("h a", FilterClauseOperator.OpEquals, "Peter");
            IFilterClause clause2 =
                filterClauseFactory.CreateStringFilterClause("h a", FilterClauseOperator.OpEquals, "Kelly");
            IFilterClause compositeClause =
                filterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);
            dv.RowFilter = compositeClause.GetFilterClauseString();
            Assert.AreEqual(0, dv.Count);
        }

        [Test]
        public void TestCompositeEqualsWithOr()
        {
            IFilterClause clause1 =
                filterClauseFactory.CreateStringFilterClause("h a", FilterClauseOperator.OpEquals, "Peter");
            IFilterClause clause2 =
                filterClauseFactory.CreateStringFilterClause("h a", FilterClauseOperator.OpEquals, "Kelly");
            IFilterClause compositeClause =
                filterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpOr, clause2);
            dv.RowFilter = compositeClause.GetFilterClauseString();
            Assert.AreEqual(2, dv.Count);
        }
    }
}