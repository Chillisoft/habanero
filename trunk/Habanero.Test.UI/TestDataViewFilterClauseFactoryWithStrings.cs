using System.Data;
using Habanero.Base;
using Habanero.Ui.Grid;
using NUnit.Framework;

namespace Habanero.Test.Ui.Generic
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