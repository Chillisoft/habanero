// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
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