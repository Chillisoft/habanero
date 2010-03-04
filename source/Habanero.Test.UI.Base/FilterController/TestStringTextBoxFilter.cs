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
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.FilterController
{
    [TestFixture]
    public abstract class TestStringTextBoxFilter
    {
        protected abstract IControlFactory GetControlFactory();

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            string propertyName = TestUtil.GetRandomString();
            const FilterClauseOperator filterClauseOperator = FilterClauseOperator.OpGreaterThan;
            
            //---------------Execute Test ----------------------
            StringTextBoxFilter filter = new StringTextBoxFilter(GetControlFactory(), propertyName, filterClauseOperator);

            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(ITextBox), filter.Control);
            Assert.AreEqual(propertyName, filter.PropertyName);
            Assert.AreEqual(filterClauseOperator, filter.FilterClauseOperator);
            Assert.IsInstanceOf(typeof(DataViewNullFilterClause), filter.GetFilterClause(new DataViewFilterClauseFactory()));
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
