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


        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            IBOColTabControl iboColTabControl = GetControlFactory().CreateBOColTabControl();
            
            //---------------Test Result -----------------------
            Assert.IsNotNull(iboColTabControl.TabControl);
            Assert.IsInstanceOfType(typeof(ITabControl), iboColTabControl.TabControl);
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
        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
        public void TestCurrentBusinessObject_ReturnsNullWhenNoCollectionIsSet()
        {
            //---------------Execute Test ----------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            //---------------Test Result -----------------------
            Assert.IsNull(boColTabControl.CurrentBusinessObject);
        }

        [Test]
        public void TestCurrentBusinessObject_ReturnsNullWhenCollectionIsSetAndThenSetToNull()
        {

            MyBO.LoadDefaultClassDef();
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();

            IBusinessObjectControl busControl = GetBusinessObjectControlStub();
            boColTabControl.BusinessObjectControl = busControl;

            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            boColTabControl.BusinessObjectCollection = myBoCol;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boColTabControl.BusinessObjectCollection);
            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectCollection = null;
            //---------------Test Result -----------------------
            Assert.IsNull(boColTabControl.CurrentBusinessObject);
            Assert.IsNull(boColTabControl.BusinessObjectCollection);
        }

        [Test]
        public void TestCurrentBusinessObject_CurrentBusinessObjectIsSetToFirstObjectInCollection()
        {
            MyBO.LoadDefaultClassDef();
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();

            IBusinessObjectControl busControl = GetBusinessObjectControlStub();
            boColTabControl.BusinessObjectControl = busControl;

            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO firstBo = new MyBO();
            myBoCol.Add(firstBo);
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectCollection = myBoCol;
            //---------------Test Result -----------------------
            Assert.AreEqual(firstBo,boColTabControl.CurrentBusinessObject);
        }

        [Test]
        public void TestCurrentBusinessObject_CurrentBusinessObjectChangesWhenTabIsChanged()
        {
            MyBO.LoadDefaultClassDef();
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();

            IBusinessObjectControl busControl = GetBusinessObjectControlStub();
            boColTabControl.BusinessObjectControl = busControl;

            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO thirdBO = new MyBO();
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            myBoCol.Add(thirdBO);
            boColTabControl.BusinessObjectCollection = myBoCol;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            boColTabControl.TabControl.SelectedTab = boColTabControl.TabControl.TabPages[2];
            //---------------Test Result -----------------------
            Assert.AreEqual(thirdBO, boColTabControl.CurrentBusinessObject);
        }

        [Test]
        public void TestCurrentBusinessObject_SettingCurrentBusinessObjectChangesSelectedTab()
        {
            MyBO.LoadDefaultClassDef();
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();

            IBusinessObjectControl busControl = GetBusinessObjectControlStub();
            boColTabControl.BusinessObjectControl = busControl;

            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO firstBO = new MyBO();
            myBoCol.Add(firstBO);
            myBoCol.Add(new MyBO());
            MyBO thirdBO = new MyBO();
            myBoCol.Add(thirdBO);
            boColTabControl.BusinessObjectCollection = myBoCol;
            //---------------Assert Precondition----------------
            Assert.AreEqual(firstBO, boColTabControl.CurrentBusinessObject);
            //---------------Execute Test ----------------------
            boColTabControl.CurrentBusinessObject = thirdBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(2, boColTabControl.TabControl.SelectedIndex);
        }

        [Test]
        public void TestCurrentBusinessObject_SettingCurrentBusinessObjectToNullHasNoEffect()
        {
            MyBO.LoadDefaultClassDef();
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();

            IBusinessObjectControl busControl = GetBusinessObjectControlStub();
            boColTabControl.BusinessObjectControl = busControl;

            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO firstBO = new MyBO();
            myBoCol.Add(firstBO);
            myBoCol.Add(new MyBO());
            MyBO thirdBO = new MyBO();
            myBoCol.Add(thirdBO);
            boColTabControl.BusinessObjectCollection = myBoCol;
            boColTabControl.CurrentBusinessObject = thirdBO;
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, boColTabControl.TabControl.SelectedIndex);
            //---------------Execute Test ----------------------
            boColTabControl.CurrentBusinessObject = null;
            //---------------Test Result -----------------------
            Assert.AreEqual(2, boColTabControl.TabControl.SelectedIndex);
        }

        [Test]
        public void TestBusinessObjectControlHasNullBusinessObjectByDefault()
        {
            MyBO.LoadDefaultClassDef();
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();

            IBusinessObjectControl busControl = GetBusinessObjectControlStub();

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------

            boColTabControl.BusinessObjectControl = busControl;
            //---------------Test Result -----------------------
            Assert.IsNull(boColTabControl.BusinessObjectControl.BusinessObject);
        }

        [Test]
        public void TestBusinessObjectControlIsSetWhenCollectionIsSet()
        {
            MyBO.LoadDefaultClassDef();
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();

            IBusinessObjectControl busControl = GetBusinessObjectControlStub();
            boColTabControl.BusinessObjectControl = busControl;

            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO firstBo = new MyBO();
            myBoCol.Add(firstBo);
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            //---------------Assert Precondition----------------
            Assert.IsNull(boColTabControl.BusinessObjectControl.BusinessObject);
            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectCollection = myBoCol;
            //---------------Test Result -----------------------
            Assert.AreEqual(firstBo, boColTabControl.BusinessObjectControl.BusinessObject);
        }

        //TODO - found out why the win version is not firing and make sure this
        //  won't affect working apps
        [Test, Ignore("The SelectedIndexChanged is not firing on the Win side.")]
        public void TestBusinessObjectControlHasDifferentBOWhenTabChanges()
        {
            MyBO.LoadDefaultClassDef();
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();

            IBusinessObjectControl busControl = GetBusinessObjectControlStub();
            boColTabControl.BusinessObjectControl = busControl;

            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO firstBo = new MyBO();
            myBoCol.Add(firstBo);
            myBoCol.Add(new MyBO());
            MyBO thirdBO = new MyBO();
            myBoCol.Add(thirdBO);
            boColTabControl.BusinessObjectCollection = myBoCol;
            //---------------Assert Precondition----------------
            Assert.AreEqual(firstBo, boColTabControl.BusinessObjectControl.BusinessObject);
            //---------------Execute Test ----------------------
            boColTabControl.TabControl.SelectedIndex = 2;
            //---------------Test Result -----------------------
            Assert.AreEqual(thirdBO, boColTabControl.BusinessObjectControl.BusinessObject);
        }

        //TODO: still need to do further testing that the bocontrol is correctly placed, has
        //  the right objects, and also that the tabs get cleared when collections changed
        //  Also review the old tests below.

        //[
        //    Test,
        //    ExpectedException(typeof(ArgumentException),
        //        "boControl must be of type Control or one of its subtypes.")]
        //public void TestCheckForControlSubClass()
        //{
        //    Mock mock = new DynamicMock(typeof(IBusinessObjectControl));
        //    IBusinessObjectControl mockBoControl = (IBusinessObjectControl)mock.MockInstance;
        //    BoTabColControl testControl = new BoTabColControl();
        //    testControl.SetBusinessObjectControl(mockBoControl);
        //}

        //[Test]
        //public void TestNumberOfTabs()
        //{
        //    Assert.AreEqual(2, itsTabColControl.TabControl.TabPages.Count);
        //}

        //[Test]
        //public void TestCorrespondingBO()
        //{
        //    Assert.AreSame(itsBo1, itsTabColControl.GetBo(itsTabColControl.TabControl.TabPages[0]));
        //    Assert.AreSame(itsBo2, itsTabColControl.GetBo(itsTabColControl.TabControl.TabPages[1]));
        //}

        //[Test]
        //public void TestCorrespondingBONull()
        //{
        //    Assert.IsNull(itsTabColControl.GetBo(new TabPage()));
        //}


        //[Test]
        //public void TestGetBoWithNullTab()
        //{
        //    Assert.IsNull(itsTabColControl.GetBo(null));
        //}

        //[Test]
        //public void TestCorrespondingTabPage()
        //{
        //    Assert.AreSame(itsTabColControl.TabControl.TabPages[0], itsTabColControl.GetTabPage(itsBo1));
        //    Assert.AreSame(itsTabColControl.TabControl.TabPages[1], itsTabColControl.GetTabPage(itsBo2));
        //}

        //[Test]
        //public void TestSettingCollectionTwice()
        //{
        //    BoTabColControl tabColControl = new BoTabColControl();
        //    tabColControl.SetBusinessObjectControl(new NullBusinessObjectControl());
        //    tabColControl.SetCollection(itsCol);
        //    tabColControl.SetCollection(itsCol);
        //    Assert.AreEqual(2, itsTabColControl.TabControl.TabPages.Count);
        //}

        //[Test]
        //public void TestCurrentBusinessObject()
        //{
        //    itsTabColControl.TabControl.SelectedTab = itsTabColControl.TabControl.TabPages[1];
        //    Assert.AreSame(itsBo2, itsTabColControl.CurrentBusinessObject);
        //    itsTabColControl.CurrentBusinessObject = itsBo1;
        //    Assert.AreSame(itsTabColControl.TabControl.TabPages[0], itsTabColControl.TabControl.SelectedTab);
        //}


        //private class NullBusinessObjectControl : Control, IBusinessObjectControl
        //{
        //    public void SetBusinessObject(BusinessObject bo)
        //    {
        //    }
        //}
    }

    internal class BusinessObjectControlGizStub : ControlGiz, IBusinessObjectControl
    {
        private IBusinessObject _bo;

        /// <summary>
        /// Specifies the business object being represented
        /// </summary>
        /// <param name="value">The business object</param>
        public IBusinessObject BusinessObject
        {
            get { return _bo; }
            set { _bo = value; }
        }
    }


    internal class BusinessObjectControlWinStub : ControlWin, IBusinessObjectControl
    {
        private IBusinessObject _bo;

        /// <summary>
        /// Specifies the business object being represented
        /// </summary>
        /// <param name="value">The business object</param>
        public IBusinessObject BusinessObject
        {
            get { return _bo; }
            set { _bo = value; }
        }
    }

}