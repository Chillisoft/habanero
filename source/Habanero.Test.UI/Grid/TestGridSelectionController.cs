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
