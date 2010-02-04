//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
//
//TODO: delete sometime in the next 10 years
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class   TestCollectionTabControlMapperVWG : TestMapperBase
    {
        protected virtual IControlFactory GetControlFactory()
        {
            return new Habanero.UI.VWG.ControlFactoryVWG();
        }

        protected virtual IBusinessObjectControl CreateBusinessObjectControl()
        {
            return new BusinessObjectControlVWG();
        }
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
        public class TestCollectionTabControlMapperWin : TestCollectionTabControlMapperVWG
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.Win.ControlFactoryWin();
            }

            protected override IBusinessObjectControl CreateBusinessObjectControl()
            {
                return new BusinessObjectControlWin();
            }

            class BusinessObjectControlWin: Habanero.UI.Win.ControlWin, IBusinessObjectControl
            {

                #region Implementation of IBusinessObjectControl
                // ReSharper disable ValueParameterNotUsed
                /// <summary>
                /// Gets or sets the business object being represented
                /// </summary>
                public IBusinessObject BusinessObject
                {
                    get { return null; }
                    set { }
                }
                // ReSharper restore ValueParameterNotUsed

                #endregion
            }
        }
        
        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            //---------------Execute Test ----------------------
            BOColTabControlManager colTabCtlMapper = new BOColTabControlManager(tabControl, GetControlFactory());
            //---------------Test Result -----------------------
            Assert.IsNotNull(colTabCtlMapper);
            Assert.IsNotNull(colTabCtlMapper.PageBoTable);
            Assert.IsNotNull(colTabCtlMapper.BoPageTable);
            Assert.AreSame(tabControl,colTabCtlMapper.TabControl);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestSetBusinessObjectControl()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            BOColTabControlManager colTabCtlMapper = new BOColTabControlManager(tabControl, GetControlFactory());
            //---------------Execute Test ----------------------
            IBusinessObjectControl busControl = CreateBusinessObjectControl();
            colTabCtlMapper.BusinessObjectControl = busControl;

            //---------------Test Result -----------------------
            Assert.AreSame(busControl, colTabCtlMapper.BusinessObjectControl);
            //---------------Tear down -------------------------
        }


        [Test]
        public void TestSetCollection()
        {
            //---------------Set up test pack-------------------

            MyBO.LoadDefaultClassDef();
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            BOColTabControlManager colTabCtlMapper = new BOColTabControlManager(tabControl, GetControlFactory());
            IBusinessObjectControl busControl = this.CreateBusinessObjectControl();
            colTabCtlMapper.BusinessObjectControl = busControl;
            //---------------Execute Test ----------------------
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            colTabCtlMapper.BusinessObjectCollection = myBoCol;
            //---------------Test Result -----------------------
            Assert.AreSame(myBoCol, colTabCtlMapper.BusinessObjectCollection);
            Assert.AreEqual(3, colTabCtlMapper.TabControl.TabPages.Count);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestGetBo()
        {
            //---------------Set up test pack-------------------

            MyBO.LoadDefaultClassDef();
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            BOColTabControlManager colTabCtlMapper = new BOColTabControlManager(tabControl, GetControlFactory());
            IBusinessObjectControl busControl = this.CreateBusinessObjectControl();
            colTabCtlMapper.BusinessObjectControl = busControl;
            //---------------Execute Test ----------------------
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO testBo = new MyBO();
            myBoCol.Add(new MyBO());
            myBoCol.Add(testBo);
            myBoCol.Add(new MyBO());
            colTabCtlMapper.BusinessObjectCollection = myBoCol;
            //---------------Test Result -----------------------
            Assert.AreSame(testBo, colTabCtlMapper.GetBo(colTabCtlMapper.TabControl.TabPages[1]));
            //---------------Tear down -------------------------
        }
        [Test]
        public void TestGetTabPage()
        {
            //---------------Set up test pack-------------------

            MyBO.LoadDefaultClassDef();
            ITabControl tabControl = GetControlFactory().CreateTabControl();
            BOColTabControlManager colTabCtlMapper = new BOColTabControlManager(tabControl, GetControlFactory());
            IBusinessObjectControl busControl = this.CreateBusinessObjectControl();
            colTabCtlMapper.BusinessObjectControl = busControl;
            //---------------Execute Test ----------------------
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO testBo = new MyBO();
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            myBoCol.Add(testBo);
            colTabCtlMapper.BusinessObjectCollection = myBoCol;
            //---------------Test Result -----------------------
            Assert.AreSame(colTabCtlMapper.TabControl.TabPages[2], colTabCtlMapper.GetTabPage(testBo));
            //---------------Tear down -------------------------
        }
    }

    class BusinessObjectControlVWG : Habanero.UI.VWG.ControlVWG, IBusinessObjectControl
    {

//        /// <summary>
//        /// Specifies the business object being represented
//        /// </summary>
//        /// <param name="bo">The business object</param>
//        public void SetBusinessObject(IBusinessObject bo)
//        {
//            
//        }

        #region Implementation of IBusinessObjectControl
        // ReSharper disable ValueParameterNotUsed
        /// <summary>
        /// Gets or sets the business object being represented
        /// </summary>
        public IBusinessObject BusinessObject
        {
            get { return null; }

            set { }
        }
        // ReSharper restore ValueParameterNotUsed
        #endregion
    }
}
