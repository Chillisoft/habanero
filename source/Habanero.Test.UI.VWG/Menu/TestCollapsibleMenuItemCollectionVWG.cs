using System;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Menu
{
    [TestFixture]
    public class TestCollapsibleMenuItemCollectionVWG : TestCollapsibleMenuItemCollection
    {
        protected override IMenuItemCollection CreateControl()
        {
            CollapsibleMenuVWG menu = new CollapsibleMenuVWG();
            return new CollapsibleMenuItemCollectionVWG(menu);
        }

        protected override IMenuItemCollection CreateControl(IMenuItem menuItem)
        {
            return new CollapsibleMenuItemCollectionVWG(menuItem);
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }

        protected override IMenuItem CreateMenuItem()
        {
            return new CollapsibleMenuItemVWG(TestUtil.GetRandomString());
        }

        protected override IMenuItem CreateMenuItem(HabaneroMenu.Item habaneroMenuItem)
        {
            return new CollapsibleMenuItemVWG(habaneroMenuItem);
        }

        [Test]
        public virtual void Test_AddSubMenuItem_ShouldAddCollapsiblePanel()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            CollapsibleMenuVWG menu = new CollapsibleMenuVWG();
            CollapsibleMenuItemCollectionVWG collapsibleMenuItemCollection = new CollapsibleMenuItemCollectionVWG(menu);
            HabaneroMenu.Item item = new HabaneroMenu.Item(name);
            IMenuItem menuLeafItem = new CollapsibleSubMenuItemVWG(GetControlFactory(), item);
            //---------------Assert Precondition----------------
            Assert.AreSame(menu, collapsibleMenuItemCollection.OwnerMenuItem);
            Assert.IsInstanceOfType(typeof(CollapsibleSubMenuItemVWG), menuLeafItem);
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
        public virtual void Test_AddLeafMenuItem_ShouldAddButtonToCollapsiblePanel()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item ownerItem = new HabaneroMenu.Item(name);
            CollapsibleSubMenuItemVWG subMenuItem = new CollapsibleSubMenuItemVWG(GetControlFactory(), ownerItem);
            CollapsibleMenuItemCollectionVWG collapsibleMenuItemCollection = new CollapsibleMenuItemCollectionVWG
                (subMenuItem);
            HabaneroMenu.Item item = new HabaneroMenu.Item(name);
            CollapsibleMenuItemVWG menuLeafItem = new CollapsibleMenuItemVWG(GetControlFactory(), item);
            //---------------Assert Precondition----------------
            Assert.AreSame(subMenuItem, collapsibleMenuItemCollection.OwnerMenuItem);
            Assert.IsInstanceOfType(typeof(CollapsibleMenuItemVWG), menuLeafItem);
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
            Assert.AreEqual(menuLeafItem.Dock, DockStyleVWG.GetDockStyle(DockStyle.Top));
        }
        [Test]
        public virtual void Test_AddLeafMenuItem_ShouldIncreaseMinSizeOfCollapsiblePanel()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item ownerItem = new HabaneroMenu.Item(name);
            CollapsibleSubMenuItemVWG subMenuItem = new CollapsibleSubMenuItemVWG(GetControlFactory(), ownerItem);
            CollapsibleMenuItemCollectionVWG collapsibleMenuItemCollection = new CollapsibleMenuItemCollectionVWG
                (subMenuItem);
            HabaneroMenu.Item item = new HabaneroMenu.Item(name);
            CollapsibleMenuItemVWG menuLeafItem = new CollapsibleMenuItemVWG(GetControlFactory(), item);
            //---------------Assert Precondition----------------
            Assert.AreSame(subMenuItem, collapsibleMenuItemCollection.OwnerMenuItem);
            Assert.IsInstanceOfType(typeof(CollapsibleMenuItemVWG), menuLeafItem);
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