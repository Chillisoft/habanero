using System.Collections.Generic;
using Habanero.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.FilterController
{
    public abstract class TestMultiplePropStringTextBoxFilter
    {
        protected abstract IControlFactory GetControlFactory();

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            string propertyName = "";
            const FilterClauseOperator filterClauseOperator = FilterClauseOperator.OpGreaterThan;
            List<string> props = new List<string>{"prop1","prop2","prop3"};
            string name = propertyName;
           
            //---------------Execute Test ----------------------
            MultiplePropStringTextBoxFilter filter = new MultiplePropStringTextBoxFilter(GetControlFactory(), props, filterClauseOperator);
            props.ForEach(s => name = s + "/" + name);
            propertyName = name.Remove(name.Length - 1);
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
            const FilterClauseOperator filterClauseOperator = FilterClauseOperator.OpGreaterThan;
            List<string> props = new List<string> { "prop1", "prop2", "prop3" };
            MultiplePropStringTextBoxFilter filter = new MultiplePropStringTextBoxFilter(GetControlFactory(), props, filterClauseOperator);
            ITextBox textBox = (ITextBox) filter.Control;
            string text = TestUtil.GetRandomString();
            textBox.Text = text;

            //---------------Execute Test ----------------------

            IFilterClause filterClause = filter.GetFilterClause(new DataViewFilterClauseFactory());
            //---------------Test Result -----------------------

            Assert.AreEqual(string.Format("(({0} > '{3}') or ({1} > '{3}')) or ({2} > '{3}')", props[0],props[1],props[2], text), filterClause.GetFilterClauseString());
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestFilterClause_1Prop()
        {
            //---------------Set up test pack-------------------
            const FilterClauseOperator filterClauseOperator = FilterClauseOperator.OpGreaterThan;
            List<string> props = new List<string> { "prop1" };
            MultiplePropStringTextBoxFilter filter = new MultiplePropStringTextBoxFilter(GetControlFactory(), props, filterClauseOperator);
            ITextBox textBox = (ITextBox) filter.Control;
            string text = TestUtil.GetRandomString();
            textBox.Text = text;

            //---------------Execute Test ----------------------

            IFilterClause filterClause = filter.GetFilterClause(new DataViewFilterClauseFactory());
            //---------------Test Result -----------------------

            Assert.AreEqual(string.Format("{0} > '{1}'", props[0], text), filterClause.GetFilterClauseString());
            //---------------Tear Down -------------------------          
        }        
        
        [Test]
        public void TestFilterClause_2Prop()
        {
            //---------------Set up test pack-------------------
            const FilterClauseOperator filterClauseOperator = FilterClauseOperator.OpGreaterThan;
            List<string> props = new List<string> { "prop1","prop2" };
            MultiplePropStringTextBoxFilter filter = new MultiplePropStringTextBoxFilter(GetControlFactory(), props, filterClauseOperator);
            ITextBox textBox = (ITextBox) filter.Control;
            string text = TestUtil.GetRandomString();
            textBox.Text = text;

            //---------------Execute Test ----------------------

            IFilterClause filterClause = filter.GetFilterClause(new DataViewFilterClauseFactory());
            //---------------Test Result -----------------------

            Assert.AreEqual(string.Format("({0} > '{2}') or ({1} > '{2}')", props[0],props[1], text), filterClause.GetFilterClauseString());
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