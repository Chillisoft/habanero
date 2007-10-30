//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System.Data;
using Habanero.Base;
using Habanero.UI.Grid;
using NUnit.Framework;

namespace Habanero.Test.UI.Generic
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