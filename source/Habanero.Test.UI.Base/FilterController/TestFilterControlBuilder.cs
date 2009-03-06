//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.FilterController
{
    [TestFixture]
    public class TestFilterControlBuilderVWG : TestFilterControlBuilder
    {
        protected override IControlFactory GetControlFactory() { return new ControlFactoryVWG();  }
    }

    [TestFixture]
    public class TestFilterControlBuilder
    {
        protected virtual IControlFactory GetControlFactory() { return new ControlFactoryWin(); }


        [Test]
        public void Test_BuildFilterControl_Simple()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());
            string propName = TestUtil.GetRandomString();
            FilterDef filterDef = CreateFilterDef_1Property(propName);

            //---------------Execute Test ----------------------
            IFilterControl filterControl = builder.BuildFilterControl(filterDef);
            
            //---------------Test Result -----------------------
            Assert.IsNotNull(filterControl);
            Assert.AreEqual(FilterModes.Filter, filterControl.FilterMode);
            Assert.AreEqual(1, filterControl.FilterControls.Count);
            Assert.IsNotNull(filterControl.GetChildControl(propName));
            Assert.IsInstanceOfType(typeof(StringTextBoxFilter), filterControl.FilterControls[0]);

            //---------------Tear Down -------------------------          
        }


        [Test]
        public void Test_BuildFilterControl_TwoProperties_CheckPropNames()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());
            string testprop1 = TestUtil.GetRandomString();
            string testprop2 = TestUtil.GetRandomString();
            FilterDef filterDef = CreateFilterDef_2Properties(testprop1, testprop2);

            //---------------Execute Test ----------------------
            IFilterControl filterControl = builder.BuildFilterControl(filterDef);
            
            //---------------Test Result -----------------------
            Assert.AreEqual(2, filterControl.FilterControls.Count);
            Assert.AreEqual(testprop1, filterControl.FilterControls[0].PropertyName);
            Assert.AreEqual(testprop2, filterControl.FilterControls[1].PropertyName);
            
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void Test_BuildFilterControl_TwoProperties_DifferentTypes()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());
            FilterDef filterDef = CreateFilterDef_2PropertiesWithType("StringTextBoxFilter", "BoolCheckBoxFilter");
            
            //---------------Execute Test ----------------------
            IFilterControl filterControl = builder.BuildFilterControl(filterDef);
            
            //---------------Test Result -----------------------
            Assert.AreEqual(2, filterControl.FilterControls.Count);
            Assert.IsInstanceOfType(typeof(StringTextBoxFilter), filterControl.FilterControls[0]);
            Assert.IsInstanceOfType(typeof(ITextBox), filterControl.FilterControls[0].Control);    
            Assert.IsInstanceOfType(typeof(BoolCheckBoxFilter), filterControl.FilterControls[1]);
            Assert.IsInstanceOfType(typeof(ICheckBox), filterControl.FilterControls[1].Control);
            
            //---------------Tear Down -------------------------          
        }


        [Test]  
        public void Test_BuildFilterControl_AlreadyConstructedFilterControl()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());
            const string filterType = "Habanero.Test.UI.Base.FilterController.SimpleFilter";
            const string filterTypeAssembly = "Habanero.Test.UI.Base";
            FilterDef filterDef = CreateFilterDef_1PropertyWithTypeAndAssembly(filterType, filterTypeAssembly);

            //---------------Execute Test ----------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            builder.BuildFilterControl(filterDef, filterControl);
            
            //---------------Test Result -----------------------
            Assert.AreEqual(1, filterControl.FilterControls.Count);
            Assert.IsInstanceOfType(typeof(SimpleFilter), filterControl.FilterControls[0]);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_BuildFilterControl_PreviouslyBuiltFilterControl()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());
            FilterDef filterDef =CreateFilterDef_1Property();

            //---------------Execute Test ----------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            builder.BuildFilterControl(filterDef, filterControl);
            builder.BuildFilterControl(filterDef, filterControl);
            
            //---------------Test Result -----------------------
            Assert.AreEqual(1, filterControl.FilterControls.Count);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_BuildFilterControl_FilterMode_FilterIsDefault()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());
            FilterDef filterDef = CreateFilterDef_1Property();
        
            //---------------Execute Test ----------------------
            IFilterControl filterControl = builder.BuildFilterControl(filterDef);

            //---------------Test Result -----------------------
            Assert.AreEqual(FilterModes.Filter, filterControl.FilterMode);
            
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_BuildFilterControl_FilterMode_Search()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());
            FilterDef filterDef = CreateFilterDef_1Property();
        
            //---------------Execute Test ----------------------
            filterDef.FilterMode = FilterModes.Search;
            IFilterControl filterControl = builder.BuildFilterControl(filterDef);

            //---------------Test Result -----------------------
            Assert.AreEqual(FilterModes.Search, filterControl.FilterMode);
            
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_BuildFilterControl_Layout_0Columns_UsesFlowLayout()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());
            FilterDef filterDef = CreateFilterDef_1Property();
          
            //---------------Execute Test ----------------------
            filterDef.Columns = 0;
            IFilterControl filterControl = builder.BuildFilterControl(filterDef);

            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(FlowLayoutManager), filterControl.LayoutManager);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_BuildFilterControl_Layout_1OrMoreColumns_UsesGridLayout()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());
            FilterDef filterDef = CreateFilterDef_1Property();
          
            //---------------Execute Test ----------------------
            filterDef.Columns = 3;
            IFilterControl filterControl = builder.BuildFilterControl(filterDef);

            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(GridLayoutManager), filterControl.LayoutManager);
            GridLayoutManager layoutManager = (GridLayoutManager) filterControl.LayoutManager;
            Assert.AreEqual(6, layoutManager.Columns.Count);
            Assert.AreEqual(1, layoutManager.Rows.Count);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_BuildFilterControl_Layout_1OrMoreColumns_UsesGridLayout_MoreThanOneRow()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());
            FilterDef filterDef = CreateFilterDef_3Properties();

            //---------------Execute Test ----------------------
            filterDef.Columns = 2;
            IFilterControl filterControl = builder.BuildFilterControl(filterDef);

            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(GridLayoutManager), filterControl.LayoutManager);
            GridLayoutManager layoutManager = (GridLayoutManager) filterControl.LayoutManager;
            Assert.AreEqual(4, layoutManager.Columns.Count);
            Assert.AreEqual(2, layoutManager.Rows.Count);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_BuildCustomFilter_FilterClauseOperator()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());

            const FilterClauseOperator op = FilterClauseOperator.OpLessThanOrEqualTo;
            FilterPropertyDef filterPropertyDef1 = CreateFilterPropertyDef(op);

            //---------------Execute Test ----------------------
            ICustomFilter customFilter = builder.BuildCustomFilter(filterPropertyDef1);

            //---------------Test Result -----------------------
            Assert.AreEqual(op, customFilter.FilterClauseOperator);

            //---------------Tear Down -------------------------          
        }


        [Test]
        public void Test_BuildCustomFilter_SetParametersViaReflection()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());

            Dictionary<string, string> parameters = new Dictionary<string, string> { { "IsChecked", "true" } };

            FilterPropertyDef filterPropertyDef =
                new FilterPropertyDef(TestUtil.GetRandomString(), TestUtil.GetRandomString(), "BoolCheckBoxFilter", "", FilterClauseOperator.OpEquals, parameters);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ICustomFilter customFilter = builder.BuildCustomFilter(filterPropertyDef);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(BoolCheckBoxFilter), customFilter);
            BoolCheckBoxFilter checkBoxFilter = (BoolCheckBoxFilter)customFilter;
            Assert.IsTrue(checkBoxFilter.IsChecked);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_BuildCustomFilter_SetParametersViaReflection_InvalidProperty()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());
            string parameterName = TestUtil.GetRandomString();
            Dictionary<string, string> parameters = new Dictionary<string, string> { { parameterName, TestUtil.GetRandomString()} };

            string propertyName = TestUtil.GetRandomString();
            const string filterType = "BoolCheckBoxFilter";
            FilterPropertyDef filterPropertyDef =
                new FilterPropertyDef(propertyName, TestUtil.GetRandomString(), filterType, "", FilterClauseOperator.OpEquals, parameters);
            //---------------Execute Test ----------------------
            try
            {
                builder.BuildCustomFilter(filterPropertyDef);
                Assert.Fail("Error should have occured because a parameter didn't exist.");

                //---------------Test Result -----------------------
            }
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains(
                    string.Format("The property '{0}' was not found on a filter of type '{1}' for property '{2}'", 
                    parameterName, filterType, propertyName), ex.Message);
            }
        }

        [Test]
        public void Test_BuildCustomFilter_CustomAssembly()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());

            FilterPropertyDef filterPropertyDef1 =
                CreateFilterPropertyDefWithType("Habanero.Test.UI.Base.FilterController.SimpleFilter", "Habanero.Test.UI.Base");
            //---------------Execute Test ----------------------
            ICustomFilter customFilter = builder.BuildCustomFilter(filterPropertyDef1);

            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(SimpleFilter), customFilter);
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_TestToHookIntoSimpleFilterEvents()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            SimpleFilter simpleFilter = new SimpleFilter(GetControlFactory(), "SomeName", FilterClauseOperator.OpEquals);
            simpleFilter.ValueChanged += delegate { };
            //---------------Test Result -----------------------
        }

        private static FilterDef CreateFilterDef_1Property()
        {
            return CreateFilterDef_1Property(TestUtil.GetRandomString());
         
        }
        
        private static FilterDef CreateFilterDef_1Property(string propName)
        {
            return new FilterDef(new List<FilterPropertyDef> { CreateFilterPropertyDef(propName) });
        }

        private static FilterPropertyDef CreateFilterPropertyDef()
        {
            return CreateFilterPropertyDef(TestUtil.GetRandomString());
        }

        private static FilterPropertyDef CreateFilterPropertyDef(string propName) {
            return CreateFilterPropertyDef(propName, FilterClauseOperator.OpEquals);
        }    

        private static FilterPropertyDef CreateFilterPropertyDef(FilterClauseOperator op)
        {
            return CreateFilterPropertyDef(TestUtil.GetRandomString(), op);
        }

        private static FilterPropertyDef CreateFilterPropertyDef(string propName, FilterClauseOperator filterClauseOperator)
        {
            return CreateFilterPropertyDef(propName, "StringTextBoxFilter", "", filterClauseOperator);
        }         
        
        private static FilterPropertyDef CreateFilterPropertyDef(string propName, string filterType, string filterTypeAssembly, FilterClauseOperator filterClauseOperator)
        {
            return new FilterPropertyDef(propName, TestUtil.GetRandomString(), filterType, filterTypeAssembly, filterClauseOperator, null);
        }  
        
        private static FilterDef CreateFilterDef_1PropertyWithTypeAndAssembly(string filterType, string filterTypeAssembly)
        {
            return new FilterDef(new List<FilterPropertyDef> { CreateFilterPropertyDefWithType(filterType, filterTypeAssembly) });
        }

        private static FilterPropertyDef CreateFilterPropertyDefWithType(string filterType, string filterTypeAssembly)
        {
            return CreateFilterPropertyDef(TestUtil.GetRandomString(), filterType, filterTypeAssembly,
                                           FilterClauseOperator.OpEquals);
        }


        private static FilterDef CreateFilterDef_2Properties(string propName1, string propName2)
        {
            const string filterType = "StringTextBoxFilter";
            return CreateFilterDef_2Properties(propName1, filterType, propName2, filterType);
        }


        private static FilterDef CreateFilterDef_2PropertiesWithType(string filterType1, string filterType2)
        {
            return CreateFilterDef_2Properties(TestUtil.GetRandomString(), filterType1, TestUtil.GetRandomString(), filterType2);
        }

        private static FilterDef CreateFilterDef_2Properties(string propName1, string filterType1, string propName2, string filterType2) {
            return new FilterDef(new List<FilterPropertyDef> {
                                         CreateFilterPropertyDef(propName1, filterType1, "", FilterClauseOperator.OpEquals), 
                                         CreateFilterPropertyDef(propName2, filterType2, "", FilterClauseOperator.OpEquals)
                                     });
        }
        
        private static FilterDef CreateFilterDef_3Properties()
        {
            FilterPropertyDef filterPropertyDef1 = CreateFilterPropertyDef();
            FilterPropertyDef filterPropertyDef2 = CreateFilterPropertyDef();
            FilterPropertyDef filterPropertyDef3 = CreateFilterPropertyDef();
            return new FilterDef(new List<FilterPropertyDef> { filterPropertyDef1, filterPropertyDef2, filterPropertyDef3 });
        }

    }
    
    internal class SimpleFilter : ICustomFilter
    {
        private readonly IControlHabanero _textBox;
#pragma warning disable 168
        public SimpleFilter(IControlFactory controlFactory, string propertyName, FilterClauseOperator filterClauseOperator)
#pragma warning restore 168
        {
            _textBox = controlFactory.CreateTextBox();
        }

        ///<summary>
        /// The control that has been constructed by this Control Manager.
        ///</summary>
        public IControlHabanero Control { get { return _textBox; } }

        ///<summary>
        /// Returns the filter clause for this control
        ///</summary>
        ///<param name="filterClauseFactory"></param>
        ///<returns></returns>
        public IFilterClause GetFilterClause(IFilterClauseFactory filterClauseFactory) { throw new NotImplementedException(); }

        ///<summary>
        /// Clears the <see cref="IDateRangeComboBox"/> of its value
        ///</summary>
        public void Clear() { throw new NotImplementedException(); }

        /// <summary>
        /// Event handler that fires when the value in the Filter control changes
        /// </summary>
        public event EventHandler ValueChanged;

        ///<summary>
        /// The name of the property being filtered by.
        ///</summary>
        public string PropertyName { get { throw new NotImplementedException(); } }

        ///<summary>
        /// Returns the operator <see cref="ICustomFilter.FilterClauseOperator"/> e.g.OpEquals to be used by for creating the Filter Clause.
        ///</summary>
        public FilterClauseOperator FilterClauseOperator { get { throw new NotImplementedException(); } }
    }
    

}
