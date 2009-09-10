// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
    /// This test class tests the base inherited methods of the Splitter class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_Splitter : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateSplitter();
        }

        [Test]
        public override void TestConversion_DockStyle_None()
        {
            //Splitter does not support setting dock styles at design time.
        }

        [Test]
        public override void TestConversion_DockStyle_Fill()
        {
            //Splitter does not support setting dock styles at design time.
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the Splitter class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_Splitter : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateSplitter();
        }

        [Test]
        public override void TestConversion_DockStyle_None()
        {
            //Splitter does not support setting dock styles at design time.
        }

        [Test]
        public override void TestConversion_DockStyle_Fill()
        {
            //Splitter does not support setting dock styles at design time.
        }
    }


    /// <summary>
    /// This test class tests the Splitter class.
    /// </summary>
    public abstract class TestSplitter
    {
        protected abstract IControlFactory GetControlFactory();

        protected ISplitter CreateSplitter()
        {
            return GetControlFactory().CreateSplitter();
        }

        [TestFixture]
        public class TestSplitterWin : TestSplitter
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }
        }

        [TestFixture]
        public class TestSplitterVWG : TestSplitter
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryVWG();
            }
        }

        [Test]
        public void Test_Create()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ISplitter splitter = CreateSplitter();
            //---------------Test Result -----------------------
            Assert.IsNotNull(splitter);
        }
    }
}
