using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Test.Structure;
using Habanero.UI.Base;

using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestCollapsibleMenu
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
            ClassDef.ClassDefs.Add(new XmlClassDefsLoader(BOBroker.GetClassDefsXml(), new DtdLoader(), new DefClassFactory()).LoadClassDefs());
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GlobalUIRegistry.ControlFactory = CreateNewControlFactory();
        }

        protected abstract IMainMenuHabanero CreateControl();
        protected abstract IMainMenuHabanero CreateControl(HabaneroMenu menu);
        protected abstract IControlFactory CreateNewControlFactory();

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
            IMainMenuHabanero collapsibleMenu = CreateControl();
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof (IControlHabanero), collapsibleMenu);
            Assert.IsInstanceOf(typeof (IMainMenuHabanero), collapsibleMenu);
            Assert.IsInstanceOf(typeof (IPanel), collapsibleMenu);
            Assert.IsNotNull(collapsibleMenu.MenuItems);
            TestUtil.AssertStringEmpty(collapsibleMenu.Name, "collapsibleMenu.Name");
            Assert.IsInstanceOf(typeof (ICollapsiblePanelGroupControl), collapsibleMenu);
        }

        [Test]
        public void Test_ConstructMainMenu_WithHabaneroMenu()
        {
            //---------------Set up test pack-------------------
            HabaneroMenu menu = new HabaneroMenu(TestUtil.GetRandomString());
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMainMenuHabanero collapsibleMenu = CreateControl(menu);
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


    public abstract class TestCollapsibleSubMenuItem
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
            ClassDef.ClassDefs.Add(new XmlClassDefsLoader(BOBroker.GetClassDefsXml(), new DtdLoader(), new DefClassFactory()).LoadClassDefs());
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GlobalUIRegistry.ControlFactory = CreateNewControlFactory();
        }

        protected abstract IMenuItem CreateControl();
        protected abstract IMenuItem CreateControl(string name);
        protected abstract IMenuItem CreateControl(HabaneroMenu.Item item);
        protected abstract IControlFactory CreateNewControlFactory();

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
            Assert.IsInstanceOf(typeof (ICollapsiblePanel), collapsibleMenuItem);
            ICollapsiblePanel cp = (ICollapsiblePanel) collapsibleMenuItem ;
            Assert.AreEqual(name, cp.CollapseButton.Text);
        }

        [Test]
        public void Test_ConstructMenuItem_WithHabaneroMenuItem_ShouldSetName()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item item = new HabaneroMenu.Item(null, name);
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
            HabaneroMenu.Item item = new HabaneroMenu.Item(null, name);
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
            HabaneroMenu.Item item = new HabaneroMenu.Item(null, name);
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
            HabaneroMenu.Item item = new HabaneroMenu.Item(null, name);
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
            HabaneroMenu.Item item = new HabaneroMenu.Item(null,name);
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



}