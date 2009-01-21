using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Win;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.FilterController
{
    [TestFixture]
    public class TestFilterControlBuilder
    {

        protected virtual IControlFactory GetControlFactory() { return new ControlFactoryWin(); }

        [Test]
        public void TestBuildSimple()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());

            FilterPropertyDef filterPropertyDef1 = new FilterPropertyDef("TestProp", "Test Prop:", "StringTextBoxFilter", "", null);
            FilterDef filterDef = new FilterDef(new List<FilterPropertyDef>() { filterPropertyDef1 });
            
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            IFilterControl filterControl = builder.BuildFilterControl(filterDef);
            //---------------Test Result -----------------------
            Assert.IsNotNull(filterControl);
            Assert.AreEqual(FilterModes.Filter, filterControl.FilterMode);
            Assert.AreEqual(1, filterControl.FilterControls.Count);
            Assert.IsNotNull(filterControl.GetChildControl("TestProp"));
            Assert.IsInstanceOfType(typeof(StringTextBoxFilter), filterControl.FilterControls[0]);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestBuildTwoProperties_CheckPropNames()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());

            string testprop1 = TestUtil.CreateRandomString();
            string testprop2 = TestUtil.CreateRandomString();
            FilterPropertyDef filterPropertyDef1 = new FilterPropertyDef(testprop1, TestUtil.CreateRandomString(), "StringTextBoxFilter", "", null);
            FilterPropertyDef filterPropertyDef2 = new FilterPropertyDef(testprop2, TestUtil.CreateRandomString(), "StringTextBoxFilter", "", null);
            FilterDef filterDef = new FilterDef(new List<FilterPropertyDef>() { filterPropertyDef1, filterPropertyDef2 });
            
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            IFilterControl filterControl = builder.BuildFilterControl(filterDef);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, filterControl.CountOfFilters);
            Assert.AreEqual(testprop1, filterControl.FilterControls[0].PropertyName);
            Assert.AreEqual(testprop2, filterControl.FilterControls[1].PropertyName);
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestBuildTwoProperties_DifferentTypes()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());

            FilterPropertyDef filterPropertyDef1 = 
                new FilterPropertyDef(TestUtil.CreateRandomString(), TestUtil.CreateRandomString(), "StringTextBoxFilter", "", null);
            FilterPropertyDef filterPropertyDef2 =
                new FilterPropertyDef(TestUtil.CreateRandomString(), TestUtil.CreateRandomString(), "BoolCheckBoxFilter", "", null);
            FilterDef filterDef = new FilterDef(new List<FilterPropertyDef> { filterPropertyDef1, filterPropertyDef2 });
            
            //---------------Assert PreConditions---------------            
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
        public void TestSetParametersViaReflection()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());

            Dictionary<string, string> parameters = new Dictionary<string, string> {{"IsChecked", "true"}};

            FilterPropertyDef filterPropertyDef =
                new FilterPropertyDef(TestUtil.CreateRandomString(), TestUtil.CreateRandomString(), "BoolCheckBoxFilter", "", parameters);
            FilterDef filterDef = new FilterDef(new List<FilterPropertyDef> { filterPropertyDef });
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            IFilterControl filterControl = builder.BuildFilterControl(filterDef);
            //---------------Test Result -----------------------
            BoolCheckBoxFilter checkBoxFilter = filterControl.FilterControls[0] as BoolCheckBoxFilter;
            Assert.IsNotNull(checkBoxFilter);
            Assert.IsTrue(checkBoxFilter.IsChecked);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSetParametersViaReflection_InvalidProperty()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());

            Dictionary<string, string> parameters = new Dictionary<string, string> {{"bob", "true"}};

            string propertyName = TestUtil.CreateRandomString();
            FilterPropertyDef filterPropertyDef =
                new FilterPropertyDef(propertyName, TestUtil.CreateRandomString(), "BoolCheckBoxFilter", "", parameters);
            FilterDef filterDef = new FilterDef(new List<FilterPropertyDef> { filterPropertyDef });
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            try
            {
                IFilterControl filterControl = builder.BuildFilterControl(filterDef);
                Assert.Fail("Error should have occured because a parameter didn't exist.");
            
            //---------------Test Result -----------------------
            }
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The property 'bob' was not found on a filter of type 'BoolCheckBoxFilter' for property '" + propertyName +"'", ex.Message);
            } 
        }

        [Test]
        public void TestBuild_CustomAssembly()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());

            //Habanero.Test.UI.Base.FilterController.TestFilterControlBuilder.SimpleFilter
            FilterPropertyDef filterPropertyDef1 =
                new FilterPropertyDef(TestUtil.CreateRandomString(), TestUtil.CreateRandomString(),
                    "Habanero.Test.UI.Base.FilterController.SimpleFilter", "Habanero.Test.UI.Base", null);         
            FilterDef filterDef = new FilterDef(new List<FilterPropertyDef> { filterPropertyDef1 });

            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            IFilterControl filterControl = builder.BuildFilterControl(filterDef);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, filterControl.FilterControls.Count);
            Assert.IsInstanceOfType(typeof(SimpleFilter), filterControl.FilterControls[0]);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestPopulateConstructedFilterControl()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            FilterPropertyDef filterPropertyDef1 =
               new FilterPropertyDef(TestUtil.CreateRandomString(), TestUtil.CreateRandomString(),
                   "Habanero.Test.UI.Base.FilterController.SimpleFilter", "Habanero.Test.UI.Base", null);
            FilterDef filterDef = new FilterDef(new List<FilterPropertyDef> { filterPropertyDef1 });

            //---------------Execute Test ----------------------
            builder.BuildFilterControl(filterDef, filterControl);
            
            //---------------Test Result -----------------------
            Assert.AreEqual(1, filterControl.FilterControls.Count);
            Assert.IsInstanceOfType(typeof(SimpleFilter), filterControl.FilterControls[0]);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestFilterMode_FilterIsDefault()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());
            FilterPropertyDef filterPropertyDef1 =
                new FilterPropertyDef(TestUtil.CreateRandomString(), TestUtil.CreateRandomString(),
                    "Habanero.Test.UI.Base.FilterController.SimpleFilter", "Habanero.Test.UI.Base", null);
            FilterDef filterDef = new FilterDef(new List<FilterPropertyDef> { filterPropertyDef1 });
        
            //---------------Execute Test ----------------------
            IFilterControl filterControl = builder.BuildFilterControl(filterDef);

            //---------------Test Result -----------------------
            Assert.AreEqual(FilterModes.Filter, filterControl.FilterMode);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestFilterMode_Search()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());
            FilterPropertyDef filterPropertyDef1 =
                new FilterPropertyDef(TestUtil.CreateRandomString(), TestUtil.CreateRandomString(),
                    "Habanero.Test.UI.Base.FilterController.SimpleFilter", "Habanero.Test.UI.Base", null);
            FilterDef filterDef = new FilterDef(new List<FilterPropertyDef> { filterPropertyDef1 });
        
            //---------------Execute Test ----------------------
            filterDef.FilterMode = FilterModes.Search;
            IFilterControl filterControl = builder.BuildFilterControl(filterDef);

            //---------------Test Result -----------------------
            Assert.AreEqual(FilterModes.Search, filterControl.FilterMode);
            
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestLayout_0Columns_UsesFlowLayout()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());
            FilterPropertyDef filterPropertyDef1 =
                new FilterPropertyDef(TestUtil.CreateRandomString(), TestUtil.CreateRandomString(),
                    "Habanero.Test.UI.Base.FilterController.SimpleFilter", "Habanero.Test.UI.Base", null);
            FilterDef filterDef = new FilterDef(new List<FilterPropertyDef> { filterPropertyDef1 });
          
            //---------------Execute Test ----------------------
            filterDef.Columns = 0;
            IFilterControl filterControl = builder.BuildFilterControl(filterDef);

            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(FlowLayoutManager), filterControl.LayoutManager);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestLayout_1OrMoreColumns_UsesGridLayout()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());
            FilterPropertyDef filterPropertyDef1 =
                new FilterPropertyDef(TestUtil.CreateRandomString(), TestUtil.CreateRandomString(),
                    "Habanero.Test.UI.Base.FilterController.SimpleFilter", "Habanero.Test.UI.Base", null);
            FilterDef filterDef = new FilterDef(new List<FilterPropertyDef> { filterPropertyDef1 });
          
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
        public void TestLayout_1OrMoreColumns_UsesGridLayout_MoreThanOneRow()
        {
            //---------------Set up test pack-------------------
            FilterControlBuilder builder = new FilterControlBuilder(GetControlFactory());
            FilterPropertyDef filterPropertyDef1 =
                new FilterPropertyDef(TestUtil.CreateRandomString(), TestUtil.CreateRandomString(),
                    "Habanero.Test.UI.Base.FilterController.SimpleFilter", "Habanero.Test.UI.Base", null); 
            FilterPropertyDef filterPropertyDef2 =
                new FilterPropertyDef(TestUtil.CreateRandomString(), TestUtil.CreateRandomString(),
                    "Habanero.Test.UI.Base.FilterController.SimpleFilter", "Habanero.Test.UI.Base", null);
            FilterPropertyDef filterPropertyDef3 =
                new FilterPropertyDef(TestUtil.CreateRandomString(), TestUtil.CreateRandomString(),
                    "Habanero.Test.UI.Base.FilterController.SimpleFilter", "Habanero.Test.UI.Base", null);
            FilterDef filterDef = new FilterDef(new List<FilterPropertyDef> { filterPropertyDef1, filterPropertyDef2, filterPropertyDef3 });
          
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


    }
    
    internal class SimpleFilter : ICustomFilter
    {
        private IControlHabanero _textBox;
        public SimpleFilter(IControlFactory controlFactory, string propertyName, FilterClauseOperator filterClauseOperator)
        {
            _textBox = controlFactory.CreateTextBox();
        }
        public IControlHabanero Control { get { return _textBox; } }
        public IFilterClause GetFilterClause(IFilterClauseFactory filterClauseFactory) { throw new System.NotImplementedException(); }
        public void Clear() { throw new System.NotImplementedException(); }
        public event EventHandler ValueChanged;
        public string PropertyName { get { throw new System.NotImplementedException(); } }
    }


}
