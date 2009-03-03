//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.VWG;
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
            MyBO.LoadDefaultClassDef();
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
        public class TestBOColTabControlVWG : TestBOColTabControl
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryVWG();
            }

            protected override IBusinessObjectControl GetBusinessObjectControlStub()
            {
                return new BusinessObjectControlVWGStub();
            }

            /// <summary>
            /// This test doesn't work for Win as it doesn't raise an event when setting the SelectedIndex
            /// </summary>
            [Test]
            public void TestBusinessObjectControlHasDifferentBOWhenTabChanges()
            {
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
                Assert.AreNotSame(firstBo, boColTabControl.BusinessObjectControl.BusinessObject);
                Assert.AreEqual(thirdBO, boColTabControl.BusinessObjectControl.BusinessObject);
            }
        }


        [Test]
        public void TestConstructor()
        {
            //---------------Execute Test ----------------------
            IBOColTabControl iboColTabControl = GetControlFactory().CreateBOColTabControl();
            
            //---------------Test Result -----------------------
            Assert.IsNotNull(iboColTabControl.TabControl);
            Assert.IsInstanceOfType(typeof(ITabControl), iboColTabControl.TabControl);
        }

        [Test]
        public void TestSetBusinessObjectControl()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            IBusinessObjectControl busControl = GetBusinessObjectControlStub();

            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectControl = busControl;

            //---------------Test Result -----------------------
            Assert.AreSame(busControl, boColTabControl.BusinessObjectControl);
        }

        [Test]
        public void TestSetCollection()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectCollection<MyBO> myBoCol = SetupColTabControlWith3ItemCollection(boColTabControl);

            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectCollection = myBoCol;

            //---------------Test Result -----------------------
            Assert.AreSame(myBoCol, boColTabControl.BusinessObjectCollection);
            Assert.AreEqual(myBoCol.Count, boColTabControl.TabControl.TabPages.Count);
        }

        [Test]
        public void TestSetCollectionTwice()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectCollection<MyBO> myBoCol = SetupColTabControlWith3ItemCollection(boColTabControl);

            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectCollection = myBoCol;
            boColTabControl.BusinessObjectCollection = myBoCol;

            //---------------Test Result -----------------------
            Assert.AreSame(myBoCol, boColTabControl.BusinessObjectCollection);
            Assert.AreEqual(3, boColTabControl.TabControl.TabPages.Count);
        }

//        [Test]
//        public void TestSetCollectionHasNoObjects()
//        {
//            //---------------Set up test pack-------------------
//            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
//            IBusinessObjectControl busControl = GetBusinessObjectControlStub();
//            boColTabControl.IBOEditor = busControl;
//            BusinessObjectCollection<MyBO> myBOS = new BusinessObjectCollection<MyBO>();
//
//            //---------------Execute Test ----------------------
//            boColTabControl.BusinessObjectCollection = myBOS;
//
//            //---------------Test Result -----------------------
//            Assert.AreSame(myBOS, boColTabControl.BusinessObjectCollection);
//            Assert.IsNull(boColTabControl.CurrentBusinessObject);
//        }

        [Test]
        public void TestGetBo()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectCollection<MyBO> myBoCol = SetupColTabControlWith3ItemCollection(boColTabControl);

            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectCollection= myBoCol;

            //---------------Test Result -----------------------
            Assert.AreSame(myBoCol[1], boColTabControl.GetBo(boColTabControl.TabControl.TabPages[1]));
        }

        [Test]
        public void TestGetTabPage()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectCollection<MyBO> myBoCol = SetupColTabControlWith3ItemCollection(boColTabControl);

            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectCollection = myBoCol;

            //---------------Test Result -----------------------
            Assert.AreSame(boColTabControl.TabControl.TabPages[2], boColTabControl.GetTabPage(myBoCol[2]));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestCurrentBusinessObject_ReturnsNullWhenNoCollectionIsSet()
        {
            //---------------Execute Test ----------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            boColTabControl.BusinessObjectControl = GetBusinessObjectControlStub();
            //---------------Test Result -----------------------
            Assert.IsNull(boColTabControl.CurrentBusinessObject);
        }

        [Test]
        public void TestCurrentBusinessObject_ReturnsNullWhenCollectionIsSetAndThenSetToNull()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectCollection<MyBO> myBoCol = SetupColTabControlWith3ItemCollection(boColTabControl);
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
        public void TestCurrentBusinessObject_IsSetToFirstObjectInCollection()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectCollection<MyBO> myBoCol = SetupColTabControlWith3ItemCollection(boColTabControl);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectCollection = myBoCol;
            //---------------Test Result -----------------------
            Assert.AreEqual(myBoCol[0],boColTabControl.CurrentBusinessObject);
        }

        [Test]
        public void TestCurrentBusinessObject_ChangesWhenTabIsChanged()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectCollection<MyBO> myBoCol = SetupColTabControlWith3ItemCollection(boColTabControl);
            boColTabControl.BusinessObjectCollection = myBoCol;

            //---------------Execute Test ----------------------
            boColTabControl.TabControl.SelectedTab = boColTabControl.TabControl.TabPages[2];

            //---------------Test Result -----------------------
            Assert.AreEqual(myBoCol[2], boColTabControl.CurrentBusinessObject);
        }

        [Test]
        public void TestCurrentBusinessObject_SettingCurrentBusinessObjectChangesSelectedTab()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectCollection<MyBO> myBoCol = SetupColTabControlWith3ItemCollection(boColTabControl);
            boColTabControl.BusinessObjectCollection = myBoCol;

            //---------------Assert Precondition----------------
            Assert.AreEqual(myBoCol[0], boColTabControl.CurrentBusinessObject);

            //---------------Execute Test ----------------------
            boColTabControl.CurrentBusinessObject = myBoCol[2];

            //---------------Test Result -----------------------
            Assert.AreEqual(2, boColTabControl.TabControl.SelectedIndex);
        }

        [Test]
        public void TestCurrentBusinessObject_SettingCurrentBusinessObjectToNullHasNoEffect()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectCollection<MyBO> myBoCol = SetupColTabControlWith3ItemCollection(boColTabControl);
            boColTabControl.BusinessObjectCollection = myBoCol;
            boColTabControl.CurrentBusinessObject = myBoCol[2];

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
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            IBusinessObjectControl busControl = GetBusinessObjectControlStub();

            //---------------Assert Precondition----------------
            Assert.IsNull(boColTabControl.BusinessObjectControl);

            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectControl = busControl;

            //---------------Test Result -----------------------
            Assert.IsNull(boColTabControl.BusinessObjectControl.BusinessObject);
        }

        [Test]
        public void TestBusinessObjectControlIsSetWhenCollectionIsSet()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectCollection<MyBO> myBoCol = SetupColTabControlWith3ItemCollection(boColTabControl);
            
            //---------------Assert Precondition----------------
            Assert.IsNull(boColTabControl.BusinessObjectControl.BusinessObject);

            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectCollection = myBoCol;

            //---------------Test Result -----------------------
            Assert.AreSame(myBoCol[0], boColTabControl.BusinessObjectControl.BusinessObject);
        }

        [Test]
        public void TestBusinessObjectControlIsSetWhenCurrentBusinessObjectIsChanged()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectCollection<MyBO> myBoCol = SetupColTabControlWith3ItemCollection(boColTabControl);
            boColTabControl.BusinessObjectCollection = myBoCol;

            //---------------Execute Test ----------------------
            boColTabControl.CurrentBusinessObject = myBoCol[1];

            //---------------Test Result -----------------------
            Assert.AreSame(myBoCol[1], boColTabControl.BusinessObjectControl.BusinessObject);
        }

        [Test]
        public void TestInitialLayout()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            IBusinessObjectControl busControl = GetBusinessObjectControlStub();
            boColTabControl.BusinessObjectControl = busControl;
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO firstBo = new MyBO();
            myBoCol.Add(firstBo);

            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectCollection = myBoCol;

            //---------------Test Result -----------------------
            Assert.AreEqual(1, boColTabControl.TabControl.TabPages[0].Controls.Count);
            Assert.AreSame(busControl, boColTabControl.TabControl.TabPages[0].Controls[0]);
            Assert.AreEqual(DockStyle.Fill, busControl.Dock);
            Assert.AreEqual(firstBo.ToString(), boColTabControl.TabControl.TabPages[0].Text);
        }

        [Test]
        public void TestLayoutAfterChangingBusinessObject()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            IBusinessObjectControl busControl = GetBusinessObjectControlStub();
            boColTabControl.BusinessObjectControl = busControl;
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO firstBo = new MyBO();
            myBoCol.Add(firstBo);
            MyBO secondBo = new MyBO();
            myBoCol.Add(secondBo);
            myBoCol.Add(new MyBO());
            boColTabControl.BusinessObjectCollection = myBoCol;

            //---------------Execute Test ----------------------
            boColTabControl.CurrentBusinessObject = secondBo;

            //---------------Test Result -----------------------
            Assert.AreEqual(1, boColTabControl.TabControl.TabPages[1].Controls.Count);
            Assert.AreSame(busControl, boColTabControl.TabControl.TabPages[1].Controls[0]);
        }

        private BusinessObjectCollection<MyBO> SetupColTabControlWith3ItemCollection(IBOColTabControl boColTabControl)
        {
            IBusinessObjectControl busControl = GetBusinessObjectControlStub();
            boColTabControl.BusinessObjectControl = busControl;
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), new MyBO(), new MyBO()};
            return myBoCol;
        }

        public class BusinessObjectControlVWGStub : ControlVWG, IBusinessObjectControl
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

        public class BusinessObjectControlWinStub : ControlWin, IBusinessObjectControl
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



 

}