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

using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
  
    public abstract class TestLayoutManager
    {
        private MockLayoutManager manager;
        private IControlChilli managedControl;

        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestLayoutManagerWin : TestLayoutManager
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }
        }

        [TestFixture]
        public class TestLayoutManagerGiz : TestLayoutManager
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }
        }
        [SetUp]
        public void SetupLayoutManager()
        {
            managedControl = GetControlFactory().CreateControl();
            managedControl.Width = 100;
            managedControl.Height = 100;
            manager = new MockLayoutManager(managedControl);
        }

        [Test]
        public void TestManagedControl()
        {
            Assert.AreSame(managedControl, manager.ManagedControl);
        }
        //TODO: Peter what must I do with these
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

        [Test]
        public void TestCreate_AlreadyHasControls()
        {
            //---------------Set up test pack-------------------
            IControlChilli controlChilli = GetControlFactory().CreatePanel();
            controlChilli.Controls.Add(GetControlFactory().CreateControl());
            //---------------Execute Test ----------------------
            try
            {
                new MockLayoutManager(controlChilli);
                Assert.Fail("Should raise an error");
            }
            //---------------Test Result -----------------------
            catch (LayoutManagerException ex)
            {
                StringAssert.Contains("You cannot initialise the layout manager with a control that already contains controls", ex.Message);
            }
        }

        [Test]
        public void TestCreate_NullControl()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            try
            {
                new MockLayoutManager(null);
                Assert.Fail("Should raise an error");
            }
            //---------------Test Result -----------------------
            catch (LayoutManagerException ex)
            {
                StringAssert.Contains("You cannot initialise the layout manager with a null control", ex.Message);
            }
        }

        private class MockLayoutManager : Habanero.UI.Base.LayoutManager
        {
            private bool mRefreshed;

            public bool IsRefreshed
            {
                get { return mRefreshed; }
            }

            public MockLayoutManager(IControlChilli managedControl) : base(managedControl, null)
            {
            }

            protected override void RefreshControlPositions()
            {
                mRefreshed = true;
            }

            public override IControlChilli AddControl(IControlChilli control)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
