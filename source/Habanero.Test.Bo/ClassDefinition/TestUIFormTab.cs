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

using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestUIFormTab
    {
        [Test]
        public void TestRemove()
        {
            UIFormColumn column = new UIFormColumn();
            UIFormTab uiFormTab = new UIFormTab();
            uiFormTab.Add(column);

            Assert.IsTrue(uiFormTab.Contains(column));
            uiFormTab.Remove(column);
            Assert.IsFalse(uiFormTab.Contains(column));
        }

        [Test]
        public void TestCopyTo()
        {
            UIFormColumn column1 = new UIFormColumn();
            UIFormColumn column2 = new UIFormColumn();
            UIFormTab uiFormTab = new UIFormTab();
            uiFormTab.Add(column1);
            uiFormTab.Add(column2);

            UIFormColumn[] target = new UIFormColumn[2];
            uiFormTab.CopyTo(target, 0);
            Assert.AreEqual(column1, target[0]);
            Assert.AreEqual(column2, target[1]);
        }

        // Just gets test coverage up
        [Test]
        public void TestSync()
        {
            UIFormTab uiFormTab = new UIFormTab();
            Assert.AreEqual(typeof(object), uiFormTab.SyncRoot.GetType());
            Assert.IsFalse(uiFormTab.IsSynchronized);
        }

        [Test]
        public void TestUIFormGrid()
        {
            UIFormGrid grid = new UIFormGrid("rel", typeof(MyBO), "correl");
            UIFormTab uiFormTab = new UIFormTab();

            Assert.IsNull(uiFormTab.UIFormGrid);
            uiFormTab.UIFormGrid = grid;
            Assert.AreEqual(grid, uiFormTab.UIFormGrid);
        }
    }
}