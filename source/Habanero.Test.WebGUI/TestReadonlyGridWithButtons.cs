using System;
using System.Threading;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.WebGUI;
using NUnit.Framework;

namespace Habanero.Test.WebGUI
{
    [TestFixture]
    public class TestReadonlyGridWithButtons
    {
        private bool _gridItemSelected;
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
        public  void TearDownTest()
        {
            //runs every time any testmethod is complete
            
        }
        [Test, Ignore("Read only grid is not firing ItemSelected")]
        public void TestReadOnlyGridFiringItemSelected()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadWebGuiClassDef();
            ReadOnlyGridWithButtons grid = new ReadOnlyGridWithButtons();
            MyBO bo = new MyBO();
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            col.Add(new MyBO()); 
            col.Add(bo);
            col.Add(new MyBO());
            _gridItemSelected = false;

            grid.SetCollection(col);
            grid.ItemSelected += delegate { _gridItemSelected = true; };
            
            //---------------Execute Test ----------------------
            grid.SelectedBusinessObject = bo;
            //---------------Test Result -----------------------

            Assert.IsTrue(_gridItemSelected);
        }

        [Test, Ignore("Read only grid is not doing setSelected")]
        public void TestSetSelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadWebGuiClassDef();
            ReadOnlyGrid grid = new ReadOnlyGrid();
            grid.CreateControl();
            MyBO bo = new MyBO();
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            col.Add(new MyBO());
            col.Add(bo);
            col.Add(new MyBO());
            _gridItemSelected = false;

            grid.SetCollection(col);
            grid.ResumeLayout(true);

            //---------------Execute Test ----------------------
            grid.SelectedBusinessObject = bo;

            //---------------Test Result -----------------------

            Assert.AreEqual(bo, grid.SelectedBusinessObject);
        }

    }
}
