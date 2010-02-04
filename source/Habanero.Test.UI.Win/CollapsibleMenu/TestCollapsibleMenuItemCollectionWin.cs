using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.CollapsibleMenu
{
    [TestFixture]
    public class TestCollapsibleMenuItemCollectionWin : TestCollapsibleMenuItemCollection
    {

        protected override IMenuItemCollection CreateControl()
        {
            CollapsibleMenuWin menu = new CollapsibleMenuWin();
            return new CollapsibleMenuItemCollectionWin(menu);
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryWin();
        }

        [Test]
        public override void Test_AddSubMenuItem_ShouldAddCollapsiblePanel()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            CollapsibleMenuWin menu = new CollapsibleMenuWin();
            CollapsibleMenuItemCollectionWin collapsibleMenuItemCollection = new CollapsibleMenuItemCollectionWin(menu);
            HabaneroMenu.Item item = new HabaneroMenu.Item(name);
            IMenuItem menuLeafItem = new CollapsibleSubMenuItemWin(GetControlFactory(), item);
            //---------------Assert Precondition----------------
            Assert.AreSame(menu, collapsibleMenuItemCollection.OwnerMenuItem);
            Assert.IsInstanceOfType(typeof(CollapsibleSubMenuItemWin), menuLeafItem);
            Assert.AreEqual(0, collapsibleMenuItemCollection.Count);
            //---------------Execute Test ----------------------
            collapsibleMenuItemCollection.Add(menuLeafItem);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, collapsibleMenuItemCollection.Count);
            Assert.AreEqual(1, menu.PanelsList.Count);
            ICollapsiblePanel collapsiblePanel = menu.PanelsList[0];
            Assert.AreEqual(name, collapsiblePanel.Text);
        }

        [Test]
        public override void Test_AddLeafMenuItem_ShouldAddButtonToCollapsiblePanel()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item ownerItem = new HabaneroMenu.Item(name);
            CollapsibleSubMenuItemWin subMenuItem = new CollapsibleSubMenuItemWin(GetControlFactory(), ownerItem);
            CollapsibleMenuItemCollectionWin collapsibleMenuItemCollection = new CollapsibleMenuItemCollectionWin
                (subMenuItem);
            HabaneroMenu.Item item = new HabaneroMenu.Item(name);
            CollapsibleMenuItemWin menuLeafItem = new CollapsibleMenuItemWin(GetControlFactory(), item);
            //---------------Assert Precondition----------------
            Assert.AreSame(subMenuItem, collapsibleMenuItemCollection.OwnerMenuItem);
            Assert.IsInstanceOfType(typeof(CollapsibleMenuItemWin), menuLeafItem);
            Assert.AreEqual(1, subMenuItem.Controls.Count, "CollapsiblePanel always has header button");
            Assert.AreEqual(0, collapsibleMenuItemCollection.Count);
            //---------------Execute Test ----------------------
            collapsibleMenuItemCollection.Add(menuLeafItem);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, collapsibleMenuItemCollection.Count);
            Assert.AreEqual(2, subMenuItem.Controls.Count, "Should have additional button control");
            IControlHabanero contentControl = subMenuItem.ContentControl;
            Assert.IsInstanceOfType(typeof(IPanel), contentControl);
            Assert.AreEqual(1, contentControl.Controls.Count);
            Assert.IsInstanceOfType(typeof(IButton), contentControl.Controls[0]);
        }

        [Test]
        public override void Test_AddLeafMenuItem_ShouldIncreaseMinSizeOfCollapsiblePanel()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item ownerItem = new HabaneroMenu.Item(name);
            CollapsibleSubMenuItemWin subMenuItem = new CollapsibleSubMenuItemWin(GetControlFactory(), ownerItem);
            CollapsibleMenuItemCollectionWin collapsibleMenuItemCollection = new CollapsibleMenuItemCollectionWin
                (subMenuItem);
            HabaneroMenu.Item item = new HabaneroMenu.Item(name);
            CollapsibleMenuItemWin menuLeafItem = new CollapsibleMenuItemWin(GetControlFactory(), item);
            //---------------Assert Precondition----------------
            Assert.AreSame(subMenuItem, collapsibleMenuItemCollection.OwnerMenuItem);
            Assert.IsInstanceOfType(typeof(CollapsibleMenuItemWin), menuLeafItem);
            Assert.AreEqual(subMenuItem.CollapseButton.Height, subMenuItem.MinimumSize.Height);
            //---------------Execute Test ----------------------
            collapsibleMenuItemCollection.Add(menuLeafItem);
            //---------------Test Result -----------------------
            int expectedHeight = subMenuItem.CollapseButton.Height + menuLeafItem.Height;
            Assert.AreEqual(expectedHeight, subMenuItem.Height);
            Assert.AreEqual
                (expectedHeight, subMenuItem.ExpandedHeight, "this will be zero untill the first time this is collapsed");
        }
    }
}