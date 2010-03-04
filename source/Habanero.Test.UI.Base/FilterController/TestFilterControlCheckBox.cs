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

using Habanero.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestFilterControlCheckBox
    {
        #region Setup/Teardown

        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
        }

        #endregion
        protected abstract IControlFactory GetControlFactory();

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }



        public void TestAddCheckBox()
        {
            //---------------Set up test pack-------------------
            //IFilterClause nullClause = new DataViewNullFilterClause();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            //---------------Execute Test ----------------------
            const string labelName = "aa";
            IControlHabanero cb = filterControl.AddBooleanFilterCheckBox(labelName, "", true);

            //---------------Test Result -----------------------
            Assert.IsNotNull(cb);
            Assert.IsTrue(cb is ICheckBox);
           // ICheckBox cBox = (ICheckBox) cb;
           // Assert.AreEqual(labelName, cBox.Text);

            //---------------Tear Down -------------------------          
        }

        public void TestAddStringFilterCheckBox()
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            //---------------Execute Test ----------------------
            filterControl.AddBooleanFilterCheckBox("Test?", "TestColumn", true);
            //---------------Test Result -----------------------
            IFilterClause clause =
                filterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals, "true");

            Assert.AreEqual(clause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());

            //---------------Tear Down -------------------------          
        }

        public void TestGetCheckBoxFilterClause()
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            ICheckBox checkBox = filterControl.AddBooleanFilterCheckBox("Test?", "TestColumn", true);

            //---------------Execute Test ----------------------
            checkBox.Checked = false;
            string filterClauseString = filterControl.GetFilterClause().GetFilterClauseString();

            //---------------Test Result -----------------------
            IFilterClause clause =
                filterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals, "false");
            Assert.AreEqual(clause.GetFilterClauseString(), filterClauseString);

            //---------------Tear Down -------------------------          
        }

        public void TestTwoCheckBoxFilter()
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory itsFilterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            filterControl.AddBooleanFilterCheckBox("Test1?", "TestColumn1", true);
            filterControl.AddBooleanFilterCheckBox("Test2?", "TestColumn2", false);

            //---------------Execute Test ----------------------
            string filterClauseString = filterControl.GetFilterClause().GetFilterClauseString();

            //---------------Test Result -----------------------
            IFilterClause clause1 =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn1", FilterClauseOperator.OpEquals, "true");
            IFilterClause clause2 =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn2", FilterClauseOperator.OpEquals, "false");
            IFilterClause fullClause =
                itsFilterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);
            Assert.AreEqual(fullClause.GetFilterClauseString(), filterClauseString);

            //---------------Tear Down -------------------------          
        }

        
        }
}