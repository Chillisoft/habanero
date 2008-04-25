using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Grid;
using NUnit.Framework;

namespace Habanero.Test.UI.Grid
{
    [TestFixture]
    public class TestGridSelectionController
    {
        private BusinessObjectCollection<MyBO> _col;
        private GridBase _grid;
        private GridSelectionController _gridSelectionController;
        private Form _form;

        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            _grid = new ReadOnlyGrid();
            _grid.CreateControl();
            MyBO bo = new MyBO();
            _col = new BusinessObjectCollection<MyBO>();
            _col.Add(new MyBO());
            _col.Add(bo);
            _col.Add(new MyBO());
            _grid.SetCollection(_col);
            _form = new Form();
            _grid.Dock = DockStyle.Fill;
            _form.Controls.Add(_grid);
            _form.Show();
            //_grid.ResumeLayout(true);
            _gridSelectionController = new GridSelectionController(_grid);
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
            _form.Close();
            _form.Dispose();
        }

        [Test]//, Ignore("Read only grid is not doing setSelected")]
        public void TestSetSelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObject bo = _col[0];

            //---------------Execute Test ----------------------
            _gridSelectionController.SelectedBusinessObject = bo;

            //---------------Test Result -----------------------
            Assert.AreEqual(bo, _gridSelectionController.SelectedBusinessObject);
        }

        [Test]//, Ignore("Read only grid is not doing setSelected")]
        public void TestSetSelectedBusinessObject_ToNull()
        {
            //---------------Set up test pack-------------------
            BusinessObject bo = _col[0];

            //---------------Execute Test ----------------------
            _gridSelectionController.SelectedBusinessObject = bo;
            _gridSelectionController.SelectedBusinessObject = null;

            //---------------Test Result -----------------------
            Assert.IsNull(_gridSelectionController.SelectedBusinessObject);
            Assert.IsNull(_gridSelectionController.Grid.CurrentRow);
        }

        [Test]//, Ignore("Read only grid is not firing ItemSelected")]
        public void TestReadOnlyGridFiringItemSelected()
        {
            //---------------Set up test pack-------------------
            bool gridItemSelected = false;
            _gridSelectionController.SelectedBusinessObject = null;
            _gridSelectionController.AddItemSelectedDelegate(delegate
            {
                gridItemSelected = true;
            });
            BusinessObject bo = _col[0];

            //---------------Execute Test ----------------------
            _gridSelectionController.SelectedBusinessObject = bo;

            //---------------Test Result -----------------------
            Assert.IsTrue(gridItemSelected);
        }
    }
}
