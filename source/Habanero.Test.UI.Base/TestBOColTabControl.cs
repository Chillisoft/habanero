using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{

    public abstract class TestBOColTabControl : TestMapperBase
    {
        protected abstract IControlFactory GetControlFactory();
        protected abstract IBusinessObjectControl GetBusinessObjectControlStub();

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
        public class TestBOColTabControlWin : TestBOColTabControl
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }

            protected override IBusinessObjectControl GetBusinessObjectControlStub()
            {
                return new BusinessObjectControlWinStub();
            }
        }

        [TestFixture]
        public class TestBOColTabControlGiz : TestBOColTabControl
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.WebGUI.ControlFactoryGizmox();
            }

            protected override IBusinessObjectControl GetBusinessObjectControlStub()
            {
                return new BusinessObjectControlGizStub();
            }
        }


        [Test, Ignore("Need to implement Dock on IChilliControl first")]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            IBOColTabControl iboColTabControl = GetControlFactory().CreateBOColTabControl();
            
            //---------------Test Result -----------------------
            Assert.IsNotNull(iboColTabControl.TabControl);
            //Assert.IsNotNull(iboColTabControl.BOColTabControlManager);
            //---------------Tear down -------------------------
        }


        //[Test]
        //public void TestConstructor()
        //{
        //    //---------------Set up test pack-------------------
        //    ITabControl tabControl = GetControlFactory().CreateTabControl();
        //    //---------------Execute Test ----------------------
        //    BOColTabControlManager colTabCtlMapper = new BOColTabControlManager(tabControl, GetControlFactory());
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(colTabCtlMapper);
        //    Assert.IsNotNull(colTabCtlMapper.PageBoTable);
        //    Assert.IsNotNull(colTabCtlMapper.BoPageTable);
        //    Assert.AreSame(tabControl, colTabCtlMapper.TabControl);
        //    //---------------Tear down -------------------------
        //}


        //TODO: check this is used
        [Test, Ignore("Need to implement Dock on IChilliControl first")]
        public void TestSetBusinessObjectControl()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();

            //---------------Execute Test ----------------------
            IBusinessObjectControl busControl = GetBusinessObjectControlStub();
            boColTabControl.BusinessObjectControl = busControl;

            //---------------Test Result -----------------------
            Assert.AreSame(busControl, boColTabControl.BusinessObjectControl);
            //---------------Tear down -------------------------
        }

        [Test, Ignore("Need to implement Dock on IChilliControl first")]
        public void TestSetCollection()
        {
            //---------------Set up test pack-------------------

            MyBO.LoadDefaultClassDef();
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            IBusinessObjectControl busControl = GetBusinessObjectControlStub();
            boColTabControl.BusinessObjectControl = busControl;
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            //---------------Execute Test ----------------------

            boColTabControl.BusinessObjectCollection = myBoCol;
            //---------------Test Result -----------------------

            Assert.AreSame(myBoCol, boColTabControl.BusinessObjectCollection);
            Assert.AreEqual(3, boColTabControl.TabControl.TabPages.Count);
            //---------------Tear down -------------------------
        }


        [Test, Ignore("Need to implement Dock on IChilliControl first")]
        public void TestGetBo()
        {
            //---------------Set up test pack-------------------

            MyBO.LoadDefaultClassDef();
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            IBusinessObjectControl busControl = GetBusinessObjectControlStub();
            boColTabControl.BusinessObjectControl = busControl;
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO testBo = new MyBO();
            myBoCol.Add(new MyBO());
            myBoCol.Add(testBo);
            myBoCol.Add(new MyBO());
            //---------------Execute Test ----------------------

            boColTabControl.BusinessObjectCollection= myBoCol;
            //---------------Test Result -----------------------
            Assert.AreSame(testBo, boColTabControl.GetBo(boColTabControl.TabControl.TabPages[1]));
            //---------------Tear down -------------------------
        }

        [Test, Ignore("Need to implement Dock on IChilliControl first")]
        public void TestGetTabPage()
        {
            //---------------Set up test pack-------------------


            MyBO.LoadDefaultClassDef();
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();

            IBusinessObjectControl busControl = GetBusinessObjectControlStub();
            boColTabControl.BusinessObjectControl = busControl;
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO testBo = new MyBO();
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            myBoCol.Add(testBo);
            //---------------Execute Test ----------------------

            boColTabControl.BusinessObjectCollection = myBoCol;
            //---------------Test Result -----------------------
            Assert.AreSame(boColTabControl.TabControl.TabPages[2], boColTabControl.GetTabPage(testBo));
            //---------------Tear down -------------------------
        }
    }

    internal class BusinessObjectControlGizStub : ControlGiz, IBusinessObjectControl
    {
        /// <summary>
        /// Specifies the business object being represented
        /// </summary>
        /// <param name="bo">The business object</param>
        public void SetBusinessObject(IBusinessObject bo)
        {
        }
    }


    internal class BusinessObjectControlWinStub : ControlWin, IBusinessObjectControl
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