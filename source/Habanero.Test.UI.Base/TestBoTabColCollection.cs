using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestBoTabColControl : TestMapperBase
    {
        protected abstract IControlFactory GetControlFactory();

        [SetUp]
        public void TestSetup()
        {
            ClassDef.ClassDefs.Clear();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TestTearDown()
        {
            //Code that is executed after each and every test is executed in this fixture/class.
        }

        [TestFixture]
        public class TestBoTabColControlGiz : TestCollectionTabControlMapper
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.WebGUI.ControlFactoryGizmox();
            }



        }



        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            //---------------Execute Test ----------------------
            BoTabColControl boTabColControl = new BoTabColControl(GetControlFactory());
            //IBusinessObjectControl busControl = new IBusinessObjectControlGiz();


            //---------------Test Result -----------------------
            Assert.IsNotNull(boTabColControl.TabControl);
            Assert.IsNotNull(boTabColControl.CollectionTabCtlMapper);
            Assert.IsNotNull(boTabColControl.ControlFactory);
            //---------------Tear down -------------------------
        }


    }

    class BusinessObjectControlGiz : ControlGiz, IBusinessObjectControl
    {


        /// <summary>
        /// Specifies the business object being represented
        /// </summary>
        /// <param name="bo">The business object</param>
        public void SetBusinessObject(IBusinessObject bo)
        {

        }

    }
}
