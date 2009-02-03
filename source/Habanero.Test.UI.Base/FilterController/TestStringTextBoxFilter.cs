using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.FilterController
{
    [TestFixture]
    public class TestStringTextBoxFilter
    {
        protected virtual IControlFactory GetControlFactory() { return new ControlFactoryWin(); }
        
        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            string propertyName = TestUtil.GetRandomString();
            const FilterClauseOperator filterClauseOperator = FilterClauseOperator.OpGreaterThan;
            
            //---------------Execute Test ----------------------
            StringTextBoxFilter filter = new StringTextBoxFilter(GetControlFactory(), propertyName, filterClauseOperator);

            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(ITextBox), filter.Control);
            Assert.AreEqual(propertyName, filter.PropertyName);
            Assert.AreEqual(filterClauseOperator, filter.FilterClauseOperator);
            Assert.IsInstanceOfType(typeof(DataViewNullFilterClause), filter.GetFilterClause(new DataViewFilterClauseFactory()));
        }

        [Test]
        public void TestFilterClause()
        {
            //---------------Set up test pack-------------------
            string propertyName = TestUtil.GetRandomString();
            const FilterClauseOperator filterClauseOperator = FilterClauseOperator.OpGreaterThan;
            StringTextBoxFilter filter = new StringTextBoxFilter(GetControlFactory(), propertyName, filterClauseOperator);
            ITextBox textBox = (ITextBox) filter.Control;
            string text = TestUtil.GetRandomString();
            textBox.Text = text;

            //---------------Execute Test ----------------------

            IFilterClause filterClause = filter.GetFilterClause(new DataViewFilterClauseFactory());
            //---------------Test Result -----------------------

            Assert.AreEqual(string.Format("{0} > '{1}'", propertyName, text), filterClause.GetFilterClauseString());
            //---------------Tear Down -------------------------          
        }

        [Test, Ignore("TODO:Peter")]
        public void TestValueChangedNotFiredIfFilterModeIsFilter()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            //---------------Tear Down -------------------------          
        }
    }
}
