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

//TODO: delete sometime in the next 10 years
////using Habanero.Base;
////using Habanero.BO;
////using Habanero.BO.ClassDefinition;
////using Habanero.UI.Base;
////using Habanero.UI.WebGUI;
////using NUnit.Framework;

////namespace Habanero.Test.UI.Base
////{
////    public abstract class   TestCollectionTabControlMapper : TestMapperBase
////    {
////        protected abstract IControlFactory GetControlFactory();

////        [SetUp]
////        public void TestSetup()
////        {
////            ClassDef.ClassDefs.Clear();
////        }

////        [TestFixtureSetUp]
////        public void TestFixtureSetup()
////        {
////            //Code that is executed before any test is run in this class. If multiple tests
////            // are executed then it will still only be called once.
////        }

////        [TearDown]
////        public void TestTearDown()
////        {
////            //Code that is executed after each and every test is executed in this fixture/class.
////        }

////        [TestFixture]
////        public class TestControlMapperCollectionVWG : TestCollectionTabControlMapper
////        {
////            protected override IControlFactory GetControlFactory()
////            {
////                return new Habanero.UI.WebGUI.ControlFactoryVWG();
////            }

            

////        }



////        //TODO: Port to windows and add tests
        
////        [Test]
////        public void TestConstructor()
////        {
////            //---------------Set up test pack-------------------
////            ITabControl tabControl = GetControlFactory().CreateTabControl();
////            //---------------Execute Test ----------------------
////            BOColTabControlManager colTabCtlMapper = new BOColTabControlManager(tabControl, GetControlFactory());
////            //---------------Test Result -----------------------
////            Assert.IsNotNull(colTabCtlMapper);
////            Assert.IsNotNull(colTabCtlMapper.PageBoTable);
////            Assert.IsNotNull(colTabCtlMapper.BoPageTable);
////            Assert.AreSame(tabControl,colTabCtlMapper.TabControl);
////            //---------------Tear down -------------------------
////        }

////        [Test]
////        public void TestSetBusinessObjectControl()
////        {
////            //---------------Set up test pack-------------------
////            ITabControl tabControl = GetControlFactory().CreateTabControl();
////            BOColTabControlManager colTabCtlMapper = new BOColTabControlManager(tabControl, GetControlFactory());
////            //---------------Execute Test ----------------------
////            IBusinessObjectControl busControl = new BusinessObjectControlVWG();
////            colTabCtlMapper.BusinessObjectControlWin = busControl;

////            //---------------Test Result -----------------------
////            Assert.AreSame(busControl, colTabCtlMapper.BoControl);
////            //---------------Tear down -------------------------
////        }

////        [Test]
////        public void TestSetCollection()
////        {
////            //---------------Set up test pack-------------------

////            MyBO.LoadDefaultClassDef();
////            ITabControl tabControl = GetControlFactory().CreateTabControl();
////            BOColTabControlManager colTabCtlMapper = new BOColTabControlManager(tabControl, GetControlFactory());
////            IBusinessObjectControl busControl = new BusinessObjectControlVWG();
////            colTabCtlMapper.BusinessObjectControlWin = busControl;
////            //---------------Execute Test ----------------------
////            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
////            myBoCol.Add(new MyBO());
////            myBoCol.Add(new MyBO());
////            myBoCol.Add(new MyBO());
////            colTabCtlMapper.BusinessObjectCollection(myBoCol);
////            //---------------Test Result -----------------------
////            Assert.AreSame(myBoCol, colTabCtlMapper.BusinessObjectCollection);
////            Assert.AreEqual(3, colTabCtlMapper.TabControl.TabPages.Count);
////            //---------------Tear down -------------------------
////        }


////        public void TestGetBo()
////        {
////            //---------------Set up test pack-------------------

////            MyBO.LoadDefaultClassDef();
////            ITabControl tabControl = GetControlFactory().CreateTabControl();
////            BOColTabControlManager colTabCtlMapper = new BOColTabControlManager(tabControl, GetControlFactory());
////            IBusinessObjectControl busControl = new BusinessObjectControlVWG();
////            colTabCtlMapper.BusinessObjectControlWin = busControl;
////            //---------------Execute Test ----------------------
////            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
////            MyBO testBo = new MyBO();
////            myBoCol.Add(new MyBO());
////            myBoCol.Add(testBo);
////            myBoCol.Add(new MyBO());
////            colTabCtlMapper.BusinessObjectCollection(myBoCol);
////            //---------------Test Result -----------------------
////            Assert.AreSame(testBo, colTabCtlMapper.GetBo(colTabCtlMapper.TabControl.TabPages[1]));
////            //---------------Tear down -------------------------
////        }

////        public void TestGetTabPage()
////        {
////            //---------------Set up test pack-------------------

////            MyBO.LoadDefaultClassDef();
////            ITabControl tabControl = GetControlFactory().CreateTabControl();
////            BOColTabControlManager colTabCtlMapper = new BOColTabControlManager(tabControl, GetControlFactory());
////            IBusinessObjectControl busControl = new BusinessObjectControlVWG();
////            colTabCtlMapper.BusinessObjectControlWin = busControl;
////            //---------------Execute Test ----------------------
////            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
////            MyBO testBo = new MyBO();
////            myBoCol.Add(new MyBO());
////            myBoCol.Add(new MyBO());
////            myBoCol.Add(testBo);
////            colTabCtlMapper.BusinessObjectCollection(myBoCol);
////            //---------------Test Result -----------------------
////            Assert.AreSame(colTabCtlMapper.TabControl.TabPages[2], colTabCtlMapper.GetTabPage(testBo));
////            //---------------Tear down -------------------------
////        }
////    }

////    class BusinessObjectControlVWG : ControlVWG, IBusinessObjectControl
////    {
////        public void SetBusinessObject(BusinessObject bo)
////        {
////        }

////        /// <summary>
////        /// Specifies the business object being represented
////        /// </summary>
////        /// <param name="bo">The business object</param>
////        public void SetBusinessObject(IBusinessObject bo)
////        {
            
////        }

////    }
////}
