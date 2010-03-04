using System;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Test.Structure;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestCollapsibleMenuItem
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

        protected virtual IControlFactory GetControlFactory()
        {
            IControlFactory factory = CreateNewControlFactory();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected abstract IMenuItem CreateControl();
        protected abstract IMenuItem CreateControl(string name);
        protected abstract IMenuItem CreateControl(HabaneroMenu.Item habaneroMenuItem);
        protected abstract IMenuItem CreateControl(IControlFactory controlFactory, HabaneroMenu.Item habaneroMenuItem);
        protected abstract IControlFactory CreateNewControlFactory();

        [Test]
        public void Test_ConstructMenuItem_ShouldSetName()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMenuItem collapsibleMenuItem = CreateControl(name);
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
            HabaneroMenu.Item item = new HabaneroMenu.Item(null, name);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                CreateControl(null, item);
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
            HabaneroMenu.Item item = new HabaneroMenu.Item(null, name);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMenuItem collapsibleMenuItem = CreateControl(item);
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
            IMenuItem collapsibleMenuItem = CreateControl(item);

            //---------------Test Result -----------------------
            TestUtil.AssertStringEmpty(collapsibleMenuItem.Text, "collapsibleMenuItem.Text");
            Assert.IsNotNull(collapsibleMenuItem.MenuItems);
        }

        [Test]
        public void Test_MenuItems_ShouldAlwaysReturnTheSameInstance()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item item = new HabaneroMenu.Item(null, name);
            IMenuItem collapsibleMenuItem = CreateControl(item);
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
}