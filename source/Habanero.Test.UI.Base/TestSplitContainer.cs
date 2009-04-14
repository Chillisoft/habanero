using System;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the SplitContainer class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_SplitContainer : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateSplitContainer();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the SplitContainer class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_SplitContainer : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateSplitContainer();
        }
    }

    /// <summary>
    /// This test class tests the SplitContainer class.
    /// </summary>
    [TestFixture]
    public class TestSplitContainerVWG
    {
        private IControlFactory _factory;

        protected virtual IControlFactory GetControlFactory()
        {
            if (_factory == null)
            {
                _factory = CreateNewControlFactory();
                GlobalUIRegistry.ControlFactory = _factory;
            }

            return _factory;
        }

        protected virtual IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }


        [Test]
        public void TestCreateSplitContainer()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            ISplitContainer mySplitContainer = GetControlFactory().CreateSplitContainer();
            //---------------Test Result -----------------------
            Assert.IsNotNull(mySplitContainer);
        }

//        [Test]
//        public void Test_PerformClick()
//        {
//            //---------------Set up test pack-------------------
//            ISplitContainer SplitContainer = this.GetControlFactory().CreateSplitContainer();
//            bool clicked = false;
//            SplitContainer.Click += delegate(object sender, EventArgs e)
//            {
//                clicked = true;
//            };
//            //AddControlToForm(SplitContainer);
//            //-------------Assert Preconditions -------------
//            Assert.IsFalse(clicked);
//            //---------------Execute Test ----------------------
//            SplitContainer.PerformClick();
//            //---------------Test Result -----------------------
//            Assert.IsTrue(clicked);
//        }
    }
        /// <summary>
    /// This test class tests the SplitContainer class.
    /// </summary>
    [TestFixture]
    public class TestSplitContainerWin:TestSplitContainerVWG
        {
        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryWin();
        }
        }
}