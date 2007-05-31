using System.Windows.Forms;
using Chillisoft.UI.Generic.v2;
using NUnit.Framework;

namespace Chillisoft.Test.UI.Generic.v2
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