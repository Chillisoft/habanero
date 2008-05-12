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

using System.Windows.Forms;
using Habanero.UI;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestLayoutManager
    {
        private MockLayoutManager manager;
        private Control managedControl;

        [SetUp]
        public void SetupLayoutManager()
        {
            managedControl = new Control();
            managedControl.Width = 100;
            managedControl.Height = 100;
            manager = new MockLayoutManager(managedControl);
        }

        [Test]
        public void TestControl()
        {
            Assert.AreSame(managedControl, manager.ManagedControl);
        }

        //		[Test]
        //		public void TestBorderSize() {
        //			manager.BorderSize = 10;
        //			Assert.AreEqual(10, manager.BorderSize, "BorderSize is not being set.");
        //			Assert.IsTrue(manager.IsRefreshed, "Refresh should happen when border size changes");
        //		}
        //
        //		[Test]
        //		public void TestGapSize() {
        //			manager.GapSize = 20;
        //			Assert.AreEqual(20, manager.GapSize, "GapSize is not being set.");
        //			Assert.IsTrue(manager.IsRefreshed, "Refresh should happend when gap size changes.");
        //		}

        [Test]
        public void TestRefreshOnResize()
        {
            managedControl.Width = 150;
            Assert.IsTrue(manager.IsRefreshed, "Refresh should happen when control size changes.");
        }

        private class MockLayoutManager : LayoutManager
        {
            private bool mRefreshed;

            public bool IsRefreshed
            {
                get { return mRefreshed; }
            }

            public MockLayoutManager(Control managedControl) : base(managedControl)
            {
            }

            protected override void RefreshControlPositions()
            {
                mRefreshed = true;
            }

        }
    }
}