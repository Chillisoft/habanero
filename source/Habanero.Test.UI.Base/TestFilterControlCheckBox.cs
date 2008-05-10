using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestFilterControlCheckBox
    {
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
        }
        #region Tests
        //        [Test]
        //        public void TestAddBooleanFilterCheckBox()
        //        {
        //            CheckBox cb = filterControl.AddStringFilterCheckBox("Test?", "TestColumn", true);
        //            IFilterClause clause =
        //                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals, "true");
        //            Assert.AreEqual(clause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
        //            cb.Checked = false;
        //            clause =
        //                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals, "false");
        //            Assert.AreEqual(clause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
        //        }
        [Test]
        public void TestAddCheckBoxGizmox()
        {
            TestAddCheckBox(new GizmoxControlFactory());
        }

        [Test]
        public void TestAddCheckBoxWinForms()
        {
            TestAddCheckBox(new WinControlFactory());
        }

        public void TestAddCheckBox(IControlFactory factory)
        {
            //---------------Set up test pack-------------------
            //IFilterClause nullClause = new DataViewNullFilterClause();
            IFilterControl filterControl = factory.CreateFilterControl();
            //---------------Execute Test ----------------------
            ICheckBox cb = filterControl.AddStringFilterCheckBox("Test?", "TestColumn", true);

            //---------------Test Result -----------------------
            Assert.IsNotNull(cb);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAddStringFilterCheckBoxGiz()
        {
            TestAddStringFilterCheckBox(new GizmoxControlFactory());
        }
        [Test]
        public void TestAddStringFilterCheckBoxWinForms()
        {
            TestAddStringFilterCheckBox(new WinControlFactory());
        }
        public void TestAddStringFilterCheckBox(IControlFactory factory)
        {
            //---------------Set up test pack-------------------
            IFilterClause nullClause = new DataViewNullFilterClause();
            IFilterControl filterControl = factory.CreateFilterControl();
            //---------------Execute Test ----------------------
            filterControl.AddStringFilterCheckBox("Test?", "TestColumn", true);
            //---------------Test Result -----------------------
            Assert.AreEqual(nullClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());

            //---------------Tear Down -------------------------          
        }
        [Test,Ignore("to be implemented")]
        public void TestGetCheckBoxFilterClauseWinForms()
        {
            TestGetCheckBoxFilterClause(new WinControlFactory());
        }

        [Test, Ignore("to be implemented")]
        public void TestGetCheckBoxFilterClauseGiz()
        {
            TestGetCheckBoxFilterClause(new GizmoxControlFactory());
        }

        public void TestGetCheckBoxFilterClause(IControlFactory factory)
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = factory.CreateFilterControl();
            ICheckBox checkBox = filterControl.AddStringFilterCheckBox("Test?", "TestColumn", true);

            //---------------Execute Test ----------------------
            checkBox.Checked = false;
            string filterClauseString = filterControl.GetFilterClause().GetFilterClauseString();

            //---------------Test Result -----------------------
            IFilterClause clause =
                filterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals, "true");
            Assert.AreEqual(clause.GetFilterClauseString(), filterClauseString);

            //---------------Tear Down -------------------------          
        }
        #endregion


    }
}
