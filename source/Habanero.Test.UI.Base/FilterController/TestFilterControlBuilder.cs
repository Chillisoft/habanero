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
using System.Drawing;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.FilterController
{
    public abstract class TestFilterControlBuilder
    {
        protected abstract IControlFactory GetControlFactory();

        [Test]
        public void Test_BuildFilterControl_Simple()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());
            string propName = TestUtil.GetRandomString();
            FilterDef filterDef = CreateFilterDef_1Property(propName);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IFilterControl filterControl = builder.BuildFilterControl(filterDef);
            filterControl.Size = new Size(1000, 800);
            //---------------Test Result -----------------------
            Assert.IsNotNull(filterControl);
            Assert.AreEqual(1, filterControl.Controls.Count, "Always has GroupBox");
            Assert.AreEqual(FilterModes.Filter, filterControl.FilterMode);
            Assert.AreEqual(1, filterControl.FilterControls.Count);
            Assert.IsNotNull(filterControl.GetChildControl(propName));
            Assert.IsInstanceOf(typeof (StringTextBoxFilter), filterControl.FilterControls[0]);
            IPanel filterPanel = filterControl.FilterPanel;
            Assert.AreEqual(2, filterPanel.Controls.Count);
            IControlHabanero label = filterControl.FilterPanel.Controls[0];
            Assert.IsInstanceOf(typeof(ILabel), label);
            Assert.Greater(label.Width, 0);
            Assert.Greater(label.Height, 0);
            Assert.Greater(label.Left, 0);
            Assert.IsTrue(label.Visible);
            IControlHabanero textBox = filterControl.FilterPanel.Controls[1];
            Assert.IsInstanceOf(typeof(ITextBox), textBox);
            Assert.GreaterOrEqual(textBox.Left, label.Left + label.Width);
        }

        [Test]
        public void Test_ResizeControl_ShouldPlaceTextBoxInCorrectPosition()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());
            string propName = TestUtil.GetRandomString();
            FilterDef filterDef = CreateFilterDef_1Property(propName);
            IFilterControl filterControl = builder.BuildFilterControl(filterDef);
            //---------------Assert Precondition----------------
            IPanel filterPanel = filterControl.FilterPanel;
            Assert.AreEqual(2, filterPanel.Controls.Count);
            IControlHabanero label = filterControl.FilterPanel.Controls[0];
            Assert.IsInstanceOf(typeof(ILabel), label);
            Assert.GreaterOrEqual(label.Width, 0);
            Assert.GreaterOrEqual(label.Height, 0);
            Assert.GreaterOrEqual(label.Left, 0);
            Assert.IsTrue(label.Visible);
            IControlHabanero textBox = filterControl.FilterPanel.Controls[1];
            Assert.IsInstanceOf(typeof(ITextBox), textBox);
            Assert.LessOrEqual(textBox.Left, label.Left + label.Width);

            //---------------Execute Test ----------------------
            filterControl.Size = new Size(1000, 800);
            //---------------Test Result -----------------------

            Assert.GreaterOrEqual(textBox.Left, label.Left + label.Width);
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
            Assert.AreEqual(FilterClauseOperator.OpEquals, filterControl.FilterControls[0].FilterClauseOperator);
            Assert.AreEqual(FilterClauseOperator.OpLike, filterControl.FilterControls[1].FilterClauseOperator);
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
            Assert.IsInstanceOf(typeof (StringTextBoxFilter), filterControl.FilterControls[0]);
            Assert.IsInstanceOf(typeof (ITextBox), filterControl.FilterControls[0].Control);
            Assert.IsInstanceOf(typeof (BoolCheckBoxFilter), filterControl.FilterControls[1]);
            Assert.IsInstanceOf(typeof (ICheckBox), filterControl.FilterControls[1].Control);
        }

        [Test]
        public void Test_BuildFilterControl_AlreadyConstructedFilterControl()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());
            const string filterType = "Habanero.Test.UI.Base.FilterController.SimpleFilterStub";
            const string filterTypeAssembly = "Habanero.Test.UI.Base";
            FilterDef filterDef = CreateFilterDef_1PropertyWithTypeAndAssembly(filterType, filterTypeAssembly);

            //---------------Execute Test ----------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            builder.BuildFilterControl(filterDef, filterControl);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, filterControl.FilterControls.Count);
            Assert.IsInstanceOf(typeof (SimpleFilterStub), filterControl.FilterControls[0]);
        }

        [Test]
        public void Test_BuildFilterControl_PreviouslyBuiltFilterControl()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());
            FilterDef filterDef = CreateFilterDef_1Property();

            //---------------Execute Test ----------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            builder.BuildFilterControl(filterDef, filterControl);
            builder.BuildFilterControl(filterDef, filterControl);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, filterControl.FilterControls.Count);
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
            Assert.IsInstanceOf(typeof (FlowLayoutManager), filterControl.LayoutManager);
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
            Assert.IsInstanceOf(typeof (GridLayoutManager), filterControl.LayoutManager);
            GridLayoutManager layoutManager = (GridLayoutManager) filterControl.LayoutManager;
            Assert.AreEqual(6, layoutManager.Columns.Count);
            Assert.AreEqual(1, layoutManager.Rows.Count);
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
            Assert.IsInstanceOf(typeof (GridLayoutManager), filterControl.LayoutManager);
            GridLayoutManager layoutManager = (GridLayoutManager) filterControl.LayoutManager;
            Assert.AreEqual(4, layoutManager.Columns.Count);
            Assert.AreEqual(2, layoutManager.Rows.Count);
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
        }

        [Test]
        public void Test_BuildCustomFilter_SetParametersViaReflection()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());

            Dictionary<string, string> parameters = new Dictionary<string, string> {{"IsChecked", "true"}};

            FilterPropertyDef filterPropertyDef = new FilterPropertyDef
                (TestUtil.GetRandomString(), TestUtil.GetRandomString(), "BoolCheckBoxFilter", "",
                 FilterClauseOperator.OpEquals, parameters);
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            ICustomFilter customFilter = builder.BuildCustomFilter(filterPropertyDef);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof (BoolCheckBoxFilter), customFilter);
            BoolCheckBoxFilter checkBoxFilter = (BoolCheckBoxFilter) customFilter;
            Assert.IsTrue(checkBoxFilter.IsChecked);
        }

        [Test]
        public void Test_BuildCustomFilter_SetParametersViaReflection_InvalidProperty()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());
            string parameterName = TestUtil.GetRandomString();
            Dictionary<string, string> parameters = new Dictionary<string, string>
                                                        {{parameterName, TestUtil.GetRandomString()}};

            string propertyName = TestUtil.GetRandomString();
            const string filterType = "BoolCheckBoxFilter";
            FilterPropertyDef filterPropertyDef = new FilterPropertyDef
                (propertyName, TestUtil.GetRandomString(), filterType, "", FilterClauseOperator.OpEquals, parameters);
            //---------------Execute Test ----------------------
            try
            {
                builder.BuildCustomFilter(filterPropertyDef);
                Assert.Fail("Error should have occured because a parameter didn't exist.");

                //---------------Test Result -----------------------
            }
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains
                    (string.Format
                         ("The property '{0}' was not found on a filter of type '{1}' for property '{2}'", parameterName,
                          filterType, propertyName), ex.Message);
            }
        }

        [Test]
        public void Test_BuildCustomFilter_CustomAssembly()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());

            FilterPropertyDef filterPropertyDef1 = CreateFilterPropertyDefWithType
                ("Habanero.Test.UI.Base.FilterController.SimpleFilterStub", "Habanero.Test.UI.Base");
            //---------------Execute Test ----------------------
            ICustomFilter customFilter = builder.BuildCustomFilter(filterPropertyDef1);

            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof (SimpleFilterStub), customFilter);
        }

        [Test]
        public void Test_TestToHookIntoSimpleFilterEvents()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            SimpleFilterStub simpleFilterStub = new SimpleFilterStub
                (GetControlFactory(), "SomeName", FilterClauseOperator.OpEquals);
            simpleFilterStub.ValueChanged += delegate { };
        }

        private static FilterDef CreateFilterDef_1Property()
        {
            return CreateFilterDef_1Property(TestUtil.GetRandomString());
        }

        private static FilterDef CreateFilterDef_1Property(string propName)
        {
            return new FilterDef(new List<IFilterPropertyDef> {CreateFilterPropertyDef(propName)});
        }

        private static FilterPropertyDef CreateFilterPropertyDef()
        {
            return CreateFilterPropertyDef(TestUtil.GetRandomString());
        }

        private static FilterPropertyDef CreateFilterPropertyDef(string propName)
        {
            return CreateFilterPropertyDef(propName, FilterClauseOperator.OpEquals);
        }

        private static FilterPropertyDef CreateFilterPropertyDef(FilterClauseOperator op)
        {
            return CreateFilterPropertyDef(TestUtil.GetRandomString(), op);
        }

        private static FilterPropertyDef CreateFilterPropertyDef
            (string propName, FilterClauseOperator filterClauseOperator)
        {
            return CreateFilterPropertyDef(propName, "StringTextBoxFilter", "", filterClauseOperator);
        }

        private static FilterPropertyDef CreateFilterPropertyDef
            (string propName, string filterType, string filterTypeAssembly, FilterClauseOperator filterClauseOperator)
        {
            return new FilterPropertyDef
                (propName, TestUtil.GetRandomString(), filterType, filterTypeAssembly, filterClauseOperator, null);
        }

        private static FilterDef CreateFilterDef_1PropertyWithTypeAndAssembly
            (string filterType, string filterTypeAssembly)
        {
            return new FilterDef
                (new List<IFilterPropertyDef> {CreateFilterPropertyDefWithType(filterType, filterTypeAssembly)});
        }

        private static FilterPropertyDef CreateFilterPropertyDefWithType(string filterType, string filterTypeAssembly)
        {
            return CreateFilterPropertyDef
                (TestUtil.GetRandomString(), filterType, filterTypeAssembly, FilterClauseOperator.OpEquals);
        }

        private static FilterDef CreateFilterDef_2Properties(string propName1, string propName2)
        {
            const string filterType = "StringTextBoxFilter";
            return CreateFilterDef_2Properties(propName1, filterType, propName2, filterType);
        }

        private static FilterDef CreateFilterDef_2PropertiesWithType(string filterType1, string filterType2)
        {
            return CreateFilterDef_2Properties
                (TestUtil.GetRandomString(), filterType1, TestUtil.GetRandomString(), filterType2);
        }

        private static FilterDef CreateFilterDef_2Properties
            (string propName1, string filterType1, string propName2, string filterType2)
        {
            return new FilterDef
                (new List<IFilterPropertyDef>
                     {
                         CreateFilterPropertyDef(propName1, filterType1, "", FilterClauseOperator.OpEquals),
                         CreateFilterPropertyDef(propName2, filterType2, "", FilterClauseOperator.OpLike)
                     });
        }

        private static FilterDef CreateFilterDef_3Properties()
        {
            FilterPropertyDef filterPropertyDef1 = CreateFilterPropertyDef();
            FilterPropertyDef filterPropertyDef2 = CreateFilterPropertyDef();
            FilterPropertyDef filterPropertyDef3 = CreateFilterPropertyDef();
            return new FilterDef
                (new List<IFilterPropertyDef> {filterPropertyDef1, filterPropertyDef2, filterPropertyDef3});
        }
    }

    internal class SimpleFilterStub : ICustomFilter
    {
        private readonly IControlHabanero _textBox;
#pragma warning disable 168
        public SimpleFilterStub
            (IControlFactory controlFactory, string propertyName, FilterClauseOperator filterClauseOperator)
#pragma warning restore 168
        {
            _textBox = controlFactory.CreateTextBox();
        }

        ///<summary>
        /// The control that has been constructed by this Control Manager.
        ///</summary>
        public IControlHabanero Control
        {
            get { return _textBox; }
        }

        ///<summary>
        /// Returns the filter clause for this control
        ///</summary>
        ///<param name="filterClauseFactory"></param>
        ///<returns></returns>
        public IFilterClause GetFilterClause(IFilterClauseFactory filterClauseFactory)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        /// Clears the <see cref="IDateRangeComboBox"/> of its value
        ///</summary>
        public void Clear()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Event handler that fires when the value in the Filter control changes
        /// </summary>
        public event EventHandler ValueChanged;

        private void FireValueChanged()
        {
            ValueChanged(this, new EventArgs());
        }

        ///<summary>
        /// The name of the property being filtered by.
        ///</summary>
        public string PropertyName
        {
            get { throw new NotImplementedException(); }
        }

        ///<summary>
        /// Returns the operator <see cref="ICustomFilter.FilterClauseOperator"/> e.g.OpEquals to be used by for creating the Filter Clause.
        ///</summary>
        public FilterClauseOperator FilterClauseOperator
        {
            get { throw new NotImplementedException(); }
        }
    }
}