using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Test.Structure;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestCollapsibleMenuItemCollection
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

        protected abstract IMenuItemCollection CreateControl();
        protected abstract IMenuItemCollection CreateControl(IMenuItem menuItem);
        protected abstract IControlFactory CreateNewControlFactory();
        protected abstract IMenuItem CreateMenuItem();
        protected abstract IMenuItem CreateMenuItem(HabaneroMenu.Item habaneroMenuItem);

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
            IMenuItem menuItem = CreateMenuItem();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMenuItemCollection collapsibleMenuItemCollection = CreateControl(menuItem);
            //---------------Test Result -----------------------
            Assert.AreSame(menuItem, collapsibleMenuItemCollection.OwnerMenuItem);
        }

        [Test]
        public void Test_ConstructMenuItem_WithHabaneroMenuItem_ShouldSetName()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item item = new HabaneroMenu.Item(name);
            IMenuItem menuItem = CreateMenuItem(item);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMenuItemCollection collapsibleMenuItemCollection = CreateControl(menuItem);
            //---------------Test Result -----------------------
            Assert.AreSame(menuItem, collapsibleMenuItemCollection.OwnerMenuItem);
        }


      
    }
}