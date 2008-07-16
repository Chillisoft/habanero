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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Grid;
using NUnit.Framework;

namespace Habanero.Test.UI.Grid
{
    [TestFixture]
    public class TestReadonlyGridControl
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

        #region Utility Methods

        private static ReadOnlyGridWithButtons SetupGridWithCollection(out BusinessObjectCollection<MyBO> col)
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            ReadOnlyGridWithButtons grid = new ReadOnlyGridWithButtons();
            grid.CreateControl();
            MyBO bo = new MyBO();
            col = new BusinessObjectCollection<MyBO>();
            col.Add(new MyBO());
            col.Add(bo);
            col.Add(new MyBO());
            grid.SetCollection(col);
            grid.ResumeLayout(true);
            return grid;
        }

        #endregion //Utility Methods

        [Test]//, Ignore("Read only grid is not doing setSelected")]
        public void TestSetSelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            ReadOnlyGridWithButtons grid = SetupGridWithCollection(out col);
            BusinessObject bo = col[0];

            //---------------Execute Test ----------------------
            grid.SelectedBusinessObject = bo;

            //---------------Test Result -----------------------
            Assert.AreEqual(bo, grid.SelectedBusinessObject);
        }

        [Test]//, Ignore("Read only grid is not doing setSelected")]
        public void TestSetSelectedBusinessObject_ToNull()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            ReadOnlyGridWithButtons grid = SetupGridWithCollection(out col);
            BusinessObject bo = col[0];

            //---------------Execute Test ----------------------
            grid.SelectedBusinessObject = bo;
            grid.SelectedBusinessObject = null;

            //---------------Test Result -----------------------
            Assert.IsNull(grid.SelectedBusinessObject);
            Assert.IsNull(grid.Grid.CurrentRow);
        }

        [Test]//, Ignore("Read only grid is not firing ItemSelected")]
        public void TestReadOnlyGridFiringItemSelected()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            ReadOnlyGridWithButtons grid = SetupGridWithCollection(out col);
            bool gridItemSelected = false;
            grid.SelectedBusinessObject = null;
            grid.AddItemSelectedDelegate(delegate
            {
                gridItemSelected = true;
            });
            BusinessObject bo = col[0];

            //---------------Execute Test ----------------------
            grid.SelectedBusinessObject = bo;

            //---------------Test Result -----------------------
            Assert.IsTrue(gridItemSelected);
        }
        
    }
}
