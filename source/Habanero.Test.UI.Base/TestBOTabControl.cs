using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestBOTabControl : TestMapperBase
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
        public class TestBOTabControlGiz : TestBOTabControl
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

            //---------------Execute Test ----------------------
            IBOTabControl iboTabControl = GetControlFactory().CreateBOTabControl();
            
            //---------------Test Result -----------------------
            Assert.IsNotNull(iboTabControl.TabControl);
            //Assert.IsNotNull(iboTabControl.BOTabControlManager);
            //---------------Tear down -------------------------
        }


        //[Test]
        //public void TestConstructor()
        //{
        //    //---------------Set up test pack-------------------
        //    ITabControl tabControl = GetControlFactory().CreateTabControl();
        //    //---------------Execute Test ----------------------
        //    BOTabControlManager colTabCtlMapper = new BOTabControlManager(tabControl, GetControlFactory());
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(colTabCtlMapper);
        //    Assert.IsNotNull(colTabCtlMapper.PageBoTable);
        //    Assert.IsNotNull(colTabCtlMapper.BoPageTable);
        //    Assert.AreSame(tabControl, colTabCtlMapper.TabControl);
        //    //---------------Tear down -------------------------
        //}


        //TODO: check this is used
        [Test]
        public void TestSetBusinessObjectControl()
        {
            //---------------Set up test pack-------------------
            IBOTabControl boTabControl = GetControlFactory().CreateBOTabControl();

            //---------------Execute Test ----------------------
            IBusinessObjectControl busControl = new BusinessObjectControlGiz();
            boTabControl.BusinessObjectControl = busControl;

            //---------------Test Result -----------------------
            Assert.AreSame(busControl, boTabControl.BusinessObjectControl);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestSetCollection()
        {
            //---------------Set up test pack-------------------

            MyBO.LoadDefaultClassDef();
            IBOTabControl boTabControl = GetControlFactory().CreateBOTabControl();
            IBusinessObjectControl busControl = new BusinessObjectControlGiz();
            boTabControl.BusinessObjectControl = busControl;
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            //---------------Execute Test ----------------------

            boTabControl.BusinessObjectCollection = myBoCol;
            //---------------Test Result -----------------------

            Assert.AreSame(myBoCol, boTabControl.BusinessObjectCollection);
            Assert.AreEqual(3, boTabControl.TabControl.TabPages.Count);
            //---------------Tear down -------------------------
        }


        public void TestGetBo()
        {
            //---------------Set up test pack-------------------

            MyBO.LoadDefaultClassDef();
            IBOTabControl boTabControl = GetControlFactory().CreateBOTabControl();
            IBusinessObjectControl busControl = new BusinessObjectControlGiz();
            boTabControl.BusinessObjectControl = busControl;
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO testBo = new MyBO();
            myBoCol.Add(new MyBO());
            myBoCol.Add(testBo);
            myBoCol.Add(new MyBO());
            //---------------Execute Test ----------------------

            boTabControl.BusinessObjectCollection= myBoCol;
            //---------------Test Result -----------------------
            Assert.AreSame(testBo, boTabControl.GetBo(boTabControl.TabControl.TabPages[1]));
            //---------------Tear down -------------------------
        }

        public void TestGetTabPage()
        {
            //---------------Set up test pack-------------------


            MyBO.LoadDefaultClassDef();
            IBOTabControl boTabControl = GetControlFactory().CreateBOTabControl();
            IBusinessObjectControl busControl = new BusinessObjectControlGiz();
            boTabControl.BusinessObjectControl = busControl;
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO testBo = new MyBO();
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            myBoCol.Add(testBo);
            //---------------Execute Test ----------------------

            boTabControl.BusinessObjectCollection = myBoCol;
            //---------------Test Result -----------------------
            Assert.AreSame(boTabControl.TabControl.TabPages[2], boTabControl.GetTabPage(testBo));
            //---------------Tear down -------------------------
        }
    }

    internal class BusinessObjectControlGiz : ControlGiz, IBusinessObjectControl
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