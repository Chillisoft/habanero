using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.HabaneroControls
{
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
        protected override Type ExpectedTypeOfBOControl()
        {
            return typeof(BusinessObjectControlVWGStub);
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

    }
}