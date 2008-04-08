//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestUIGrid
    {
        [Test]
        public void TestRemove()
        {
            UIGridColumn column = new UIGridColumn("heading", null, null, false,
                100, UIGridColumn.PropAlignment.left, null);
            UIGrid uiGrid = new UIGrid();
            uiGrid.Add(column);

            Assert.IsTrue(uiGrid.Contains(column));
            uiGrid.Remove(column);
            Assert.IsFalse(uiGrid.Contains(column));
        }

        [Test]
        public void TestCopyTo()
        {
            UIGridColumn column1 = new UIGridColumn("heading", null, null, false,
                100, UIGridColumn.PropAlignment.left, null);
            UIGridColumn column2 = new UIGridColumn("heading", null, null, false,
                100, UIGridColumn.PropAlignment.left, null);
            UIGrid uiGrid = new UIGrid();
            uiGrid.Add(column1);
            uiGrid.Add(column2);

            UIGridColumn[] target = new UIGridColumn[2];
            uiGrid.CopyTo(target, 0);
            Assert.AreEqual(column1, target[0]);
            Assert.AreEqual(column2, target[1]);
        }

        // Just gets test coverage up
        [Test]
        public void TestSync()
        {
            UIGrid uiGrid = new UIGrid();
            Assert.AreEqual(typeof(object), uiGrid.SyncRoot.GetType());
            Assert.IsFalse(uiGrid.IsSynchronized);
        }
    }
}