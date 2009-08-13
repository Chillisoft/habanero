using System;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Test.Structure;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{

    [TestFixture]
    public class TestCollapsibleMenuVWG
    {
        [SetUp]
        public void SetupTest()
        {
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef.LoadClassDefs(new XmlClassDefsLoader(BOBroker.GetClassDefsXml(), new DtdLoader(), new DefClassFactory()));
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GlobalUIRegistry.ControlFactory = CreateNewControlFactory();
        }

        protected virtual IMainMenuHabanero CreateControl()
        {
            return new CollapsibleMenuVWG();
        }

        protected virtual IMainMenuHabanero CreateControl(HabaneroMenu menu)
        {
            return new CollapsibleMenuVWG(menu);
        }

        protected virtual IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }

        protected virtual IControlFactory GetControlFactory()
        {
            IControlFactory factory = CreateNewControlFactory();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }


        [Test]
        public void Test_Construction()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            CollapsibleMenuVWG collapsibleMenu = new CollapsibleMenuVWG();
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof (IControlHabanero), collapsibleMenu);
            Assert.IsInstanceOfType(typeof (IMainMenuHabanero), collapsibleMenu);
            Assert.IsInstanceOfType(typeof (IPanel), collapsibleMenu);
            Assert.AreEqual(0, collapsibleMenu.Controls.Count);
            Assert.IsNotNull(collapsibleMenu.MenuItems);
            TestUtil.AssertStringEmpty(collapsibleMenu.Name, "collapsibleMenu.Name");
            Assert.IsInstanceOfType(typeof (ICollapsiblePanelGroupControl), collapsibleMenu);
        }

        [Test]
        public void Test_ConstructMainMenu_WithHabaneroMenu()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu menu = new HabaneroMenu(TestUtil.GetRandomString());
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            CollapsibleMenuVWG collapsibleMenu = new CollapsibleMenuVWG(menu);
            //---------------Test Result -----------------------
            Assert.IsNotNull(collapsibleMenu);
            Assert.IsNotNull(collapsibleMenu.MenuItems);
            Assert.AreEqual(menu.Name, collapsibleMenu.Name);
        }

        [Test]
        public void Test_MenuItems_ShouldAlwaysReturnTheSameInstance()
        {
            //---------------Set up test pack-------------------
            IMainMenuHabanero mainMenu = CreateControl();
            IMenuItemCollection expectedMenuItems = mainMenu.MenuItems;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(mainMenu);
            Assert.IsNotNull(expectedMenuItems);
            //---------------Execute Test ----------------------
            IMenuItemCollection secondCallToMenuItems = mainMenu.MenuItems;
            //---------------Test Result -----------------------
            Assert.AreSame(expectedMenuItems, secondCallToMenuItems);
        }

        [Test]
        public void Test_ConstructMainMenu_WithHabaneroMenuNull_ShouldNotSetName()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMainMenuHabanero mainMenu = CreateControl(null);
            //---------------Test Result -----------------------
            Assert.IsNotNull(mainMenu);
            TestUtil.AssertStringEmpty(mainMenu.Name, "mainMenu.Name");
            Assert.IsNotNull(mainMenu.MenuItems);
        }

        [Test]
        public void Test_DockInForm()
        {
            //---------------Set up test pack-------------------
            IFormHabanero formHabanero = GetControlFactory().CreateForm();
            IMainMenuHabanero mainMenu = CreateControl();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, formHabanero.Controls.Count);
            //---------------Execute Test ----------------------
            mainMenu.DockInForm(formHabanero);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, formHabanero.Controls.Count);
        }
    }

    [TestFixture]
    public class TestCollapsibleMenuWin : TestCollapsibleMenuVWG
    {

        protected override IMainMenuHabanero CreateControl()
        {
            return new CollapsibleMenuWin();
        }

        protected override IMainMenuHabanero CreateControl(HabaneroMenu menu)
        {
            return new CollapsibleMenuWin(menu);
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryWin();
        }
    }


    [TestFixture]
    public class TestCollapsibleMenuItemCollectionVWG
    {
        [SetUp]
        public void SetupTest()
        {
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef.LoadClassDefs(new XmlClassDefsLoader(BOBroker.GetClassDefsXml(), new DtdLoader(), new DefClassFactory()));
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GlobalUIRegistry.ControlFactory = CreateNewControlFactory();
        }

        protected virtual IMenuItemCollection CreateControl()
        {
            CollapsibleMenuVWG menu = new CollapsibleMenuVWG();
            return new CollapsibleMenuItemCollectionVWG(menu);
        }

        protected virtual IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }

        protected virtual IControlFactory GetControlFactory()
        {
            IControlFactory factory = CreateNewControlFactory();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        [Test]
        public void Test_Construct_ShouldSetMenuItem()
        {
            //---------------Set up test pack-------------------
            CollapsibleMenuVWG menu = new CollapsibleMenuVWG();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            CollapsibleMenuItemCollectionVWG collapsibleMenuItemCollection = new CollapsibleMenuItemCollectionVWG(menu);
            //---------------Test Result -----------------------
            Assert.AreSame(menu, collapsibleMenuItemCollection.OwnerMenuItem);
        }

        [Test]
        public void Test_ConstructMenuItem_WithHabaneroMenuItem_ShouldSetName()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item item = new HabaneroMenu.Item(name);
            CollapsibleMenuItemVWG menuItem = new CollapsibleMenuItemVWG(item);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            CollapsibleMenuItemCollectionVWG collapsibleMenuItemCollection = new CollapsibleMenuItemCollectionVWG
                (menuItem);
            //---------------Test Result -----------------------
            Assert.AreSame(menuItem, collapsibleMenuItemCollection.OwnerMenuItem);
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
            Assert.IsInstanceOfType(typeof (CollapsibleSubMenuItemVWG), menuLeafItem);
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
            Assert.IsInstanceOfType(typeof (CollapsibleMenuItemVWG), menuLeafItem);
            Assert.AreEqual(1, subMenuItem.Controls.Count, "CollapsiblePanel always has header button");
            Assert.AreEqual(0, collapsibleMenuItemCollection.Count);
            //---------------Execute Test ----------------------
            collapsibleMenuItemCollection.Add(menuLeafItem);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, collapsibleMenuItemCollection.Count);
            Assert.AreEqual(2, subMenuItem.Controls.Count, "Should have additional button control");
            IControlHabanero contentControl = subMenuItem.ContentControl;
            Assert.IsInstanceOfType(typeof (IPanel), contentControl);
            Assert.AreEqual(1, contentControl.Controls.Count);
            Assert.IsInstanceOfType(typeof (IButton), contentControl.Controls[0]);
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
            Assert.IsInstanceOfType(typeof (CollapsibleMenuItemVWG), menuLeafItem);
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

    [TestFixture]
    public class TestCollapsibleMenuItemCollectionWin : TestCollapsibleMenuItemCollectionVWG
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

    [TestFixture]
    public class TestCollapsibleMenuItemVWG
    {
        [SetUp]
        public void SetupTest()
        {
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef.LoadClassDefs(new XmlClassDefsLoader(BOBroker.GetClassDefsXml(), new DtdLoader(), new DefClassFactory()));
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GlobalUIRegistry.ControlFactory = CreateNewControlFactory();
        }

        protected virtual IControlFactory GetControlFactory()
        {
            IControlFactory factory = CreateNewControlFactory();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected virtual IMenuItem CreateControl()
        {
            return new CollapsibleMenuItemVWG(TestUtil.GetRandomString());
        }

        protected virtual IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }

        [Test]
        public void Test_ConstructMenuItem_ShouldSetName()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            CollapsibleMenuItemVWG collapsibleMenuItem = new CollapsibleMenuItemVWG(name);
            //---------------Test Result -----------------------
            Assert.AreEqual(name, collapsibleMenuItem.Text);
            Assert.IsNotNull(collapsibleMenuItem.MenuItems);
            Assert.IsInstanceOfType(typeof (IButton), collapsibleMenuItem);
        }

        [Test]
        public void Test_ConstructMenuItem_ControlFactoryNull_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item item = new HabaneroMenu.Item(name);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                new CollapsibleMenuItemVWG(null, item);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("controlFactory", ex.ParamName);
            }
        }

        [Test]
        public void Test_ConstructMenuItem_WithHabaneroMenuItem_ShouldSetName()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item item = new HabaneroMenu.Item(name);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            CollapsibleMenuItemVWG collapsibleMenuItem = new CollapsibleMenuItemVWG(item);
            //---------------Test Result -----------------------
            Assert.AreEqual(name, collapsibleMenuItem.Text);
            Assert.AreEqual(item.Name, collapsibleMenuItem.Text);
            Assert.IsNotNull(collapsibleMenuItem.MenuItems);
        }

        [Test]
        public void Test_ConstructMenuItem_WithHabaneroMenuItemNull_ShouldNotSetName()
        {
            //---------------Set up test pack-------------------
            const HabaneroMenu.Item item = null;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            CollapsibleMenuItemVWG collapsibleMenuItem = new CollapsibleMenuItemVWG(item);
            //---------------Test Result -----------------------
            TestUtil.AssertStringEmpty(collapsibleMenuItem.Text, "collapsibleMenuItem.Text");
            Assert.IsNotNull(collapsibleMenuItem.MenuItems);
        }

        [Test]
        public void Test_MenuItems_ShouldAlwaysReturnTheSameInstance()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item item = new HabaneroMenu.Item(name);
            CollapsibleMenuItemVWG collapsibleMenuItem = new CollapsibleMenuItemVWG(item);
            IMenuItemCollection expectedMenuItems = collapsibleMenuItem.MenuItems;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(collapsibleMenuItem);
            Assert.IsNotNull(expectedMenuItems);
            //---------------Execute Test ----------------------
            IMenuItemCollection secondCallToMenuItems = collapsibleMenuItem.MenuItems;
            //---------------Test Result -----------------------
            Assert.AreSame(expectedMenuItems, secondCallToMenuItems);
        }
    }

    [TestFixture]
    public class TestCollapsibleMenuItemWin : TestCollapsibleMenuItemVWG
    {

        protected override IMenuItem CreateControl()
        {
            return new CollapsibleMenuItemWin(TestUtil.GetRandomString());
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryWin();
        }
    }

    [TestFixture]
    public class TestCollapsibleSubMenuItemVWG
    {
        [SetUp]
        public void SetupTest()
        {
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef.LoadClassDefs(new XmlClassDefsLoader(BOBroker.GetClassDefsXml(), new DtdLoader(), new DefClassFactory()));
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GlobalUIRegistry.ControlFactory = CreateNewControlFactory();
        }

        protected virtual IMenuItem CreateControl()
        {
            return new CollapsibleSubMenuItemVWG(GetControlFactory(), TestUtil.GetRandomString());
        }
        protected virtual IMenuItem CreateControl(string name)
        {
            return new CollapsibleSubMenuItemVWG(GetControlFactory(), name);
        }
        protected virtual IMenuItem CreateControl(HabaneroMenu.Item item)
        {
            return new CollapsibleSubMenuItemVWG(GetControlFactory(), item);
        }

        protected virtual IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }

        protected virtual IControlFactory GetControlFactory()
        {
            IControlFactory factory = CreateNewControlFactory();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        [Test]
        public void Test_ConstructMenuItem_ShouldSetName()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMenuItem collapsibleMenuItem = CreateControl( name);
            //---------------Test Result -----------------------
            Assert.AreEqual(name, collapsibleMenuItem.Text);
            Assert.IsNotNull(collapsibleMenuItem.MenuItems);
            Assert.IsInstanceOfType(typeof (ICollapsiblePanel), collapsibleMenuItem);
            ICollapsiblePanel cp = (ICollapsiblePanel) collapsibleMenuItem ;
            Assert.AreEqual(name, cp.CollapseButton.Text);
        }

        [Test]
        public void Test_ConstructMenuItem_WithHabaneroMenuItem_ShouldSetName()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item item = new HabaneroMenu.Item(name);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMenuItem collapsibleMenuItem = CreateControl( item);
            //---------------Test Result -----------------------
            Assert.AreEqual(name, collapsibleMenuItem.Text);
            Assert.AreEqual(item.Name, collapsibleMenuItem.Text);
            Assert.IsNotNull(collapsibleMenuItem.MenuItems);
        }

        [Test]
        public void Test_ConstructMenuItem_WithHabaneroMenuItemNull_ShouldNotSetName()
        {
            //---------------Set up test pack-------------------
            const HabaneroMenu.Item item = null;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMenuItem collapsibleMenuItem = CreateControl( item);
            //---------------Test Result -----------------------
            TestUtil.AssertStringEmpty(collapsibleMenuItem.Text, "collapsibleMenuItem.Text");
            Assert.IsNotNull(collapsibleMenuItem.MenuItems);
        }

        [Test]
        public void Test_ConstructSubMenu_ShouldBeCollapsed()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item item = new HabaneroMenu.Item(name);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMenuItem collapsibleMenuItem = CreateControl( item);
            ICollapsiblePanel subMenuAsCollapsiblePanel = (ICollapsiblePanel) collapsibleMenuItem;
            //---------------Test Result -----------------------
            Assert.IsTrue(subMenuAsCollapsiblePanel.Collapsed);
            Assert.AreEqual
                (subMenuAsCollapsiblePanel.CollapseButton.Height, subMenuAsCollapsiblePanel.MinimumSize.Height);
        }

        [Test]
        public void Test_MenuItems_ShouldAlwaysReturnTheSameInstance()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item item = new HabaneroMenu.Item(name);
            IMenuItem collapsibleMenuItem = CreateControl( item);
            IMenuItemCollection expectedMenuItems = collapsibleMenuItem.MenuItems;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(collapsibleMenuItem);
            Assert.IsNotNull(expectedMenuItems);
            //---------------Execute Test ----------------------
            IMenuItemCollection secondCallToMenuItems = collapsibleMenuItem.MenuItems;
            //---------------Test Result -----------------------
            Assert.AreSame(expectedMenuItems, secondCallToMenuItems);
        }

        [Test]
        public void Test_PerformClick_ShouldExpandTheCollapsiblePanel()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item item = new HabaneroMenu.Item(name);
            IMenuItem collapsibleMenuItem = CreateControl( item);
            ICollapsiblePanel subMenuAsCollapsiblePanel = (ICollapsiblePanel) collapsibleMenuItem;
            //---------------Assert Precondition----------------
            Assert.IsTrue(subMenuAsCollapsiblePanel.Collapsed);
            //---------------Execute Test ----------------------
            collapsibleMenuItem.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsFalse(subMenuAsCollapsiblePanel.Collapsed);
        }

        [Test]
        public void Test_DoClick_ShouldExpandTheCollapsiblePanel()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item item = new HabaneroMenu.Item(name);
            IMenuItem collapsibleMenuItem = CreateControl( item);
            ICollapsiblePanel subMenuAsCollapsiblePanel = (ICollapsiblePanel) collapsibleMenuItem;
            //---------------Assert Precondition----------------
            Assert.IsTrue(subMenuAsCollapsiblePanel.Collapsed);
            //---------------Execute Test ----------------------
            collapsibleMenuItem.DoClick();
            //---------------Test Result -----------------------
            Assert.IsFalse(subMenuAsCollapsiblePanel.Collapsed);
        }
    }

    [TestFixture]
    public class TestCollapsibleSubMenuItemWin : TestCollapsibleSubMenuItemVWG
    {

        protected override IMenuItem CreateControl()
        {
            return new CollapsibleMenuItemWin(TestUtil.GetRandomString());
        }
        protected override IMenuItem CreateControl(string name)
        {
            return new CollapsibleSubMenuItemWin(GetControlFactory(),  name);
        }
        protected override IMenuItem CreateControl(HabaneroMenu.Item item)
        {
            return new CollapsibleSubMenuItemWin(GetControlFactory(), item);
        }
        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryWin();
        }
    }
}