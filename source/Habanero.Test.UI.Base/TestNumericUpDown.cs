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

using System;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the NumericUpDown class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_NumericUpDown : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateNumericUpDown();
        }



        [Test]
        public void Test_setTextAlignment_Left()
        {
            //---------------Set up test pack-------------------
            INumericUpDown textBox = GetControlFactory().CreateNumericUpDown();
            //---------------Execute Test ----------------------
            textBox.TextAlign = HorizontalAlignment.Left;
            //---------------Test Result -----------------------
            Assert.AreEqual(HorizontalAlignment.Left, textBox.TextAlign);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void Test_setTextAlignment_Center()
        {
            //---------------Set up test pack-------------------
            INumericUpDown textBox = GetControlFactory().CreateNumericUpDown();
            //---------------Execute Test ----------------------
            textBox.TextAlign = HorizontalAlignment.Center;
            //---------------Test Result -----------------------
            Assert.AreEqual(HorizontalAlignment.Center, textBox.TextAlign);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void Test_setTextAlignment_Right()
        {
            //---------------Set up test pack-------------------
            INumericUpDown textBox = GetControlFactory().CreateNumericUpDown();
            //---------------Execute Test ----------------------
            textBox.TextAlign = HorizontalAlignment.Right;
            //---------------Test Result -----------------------
            Assert.AreEqual(HorizontalAlignment.Right, textBox.TextAlign);
            //---------------Tear Down -------------------------
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the NumericUpDown class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_NumericUpDown : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateNumericUpDown();
        }

        [Test]
        public void Test_defaultTextAlignment()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            INumericUpDown numericUpDown = GetControlFactory().CreateNumericUpDown();
            //---------------Test Result -----------------------
            Assert.AreEqual(HorizontalAlignment.Left, numericUpDown.TextAlign);
            //---------------Tear Down -------------------------
        }


        

        [Test ]
        public void Test_setTextAlignment_Center()
        {
            //, Ignore("VWG does not support setting the TextAlign Property. Default value is Left")
        }

        [Test]
        public void Test_setTextAlignment_Right()
        {
             //Ignore("VWG does not support setting the TextAlign Property. Default value is Left")
//            //---------------Set up test pack-------------------
//            INumericUpDown numericUpDown = GetControlFactory().CreateNumericUpDown();
//            //---------------Execute Test ----------------------
//            numericUpDown.TextAlign = HorizontalAlignment.Right;
//            //---------------Test Result -----------------------
//            Assert.AreEqual(HorizontalAlignment.Right, numericUpDown.TextAlign);
//            //---------------Tear Down -------------------------
        }
    }

    /// <summary>
    /// This test class tests the NumericUpDown class.
    /// </summary>
    [TestFixture]
    public class TestNumericUpDown
    {

    }
}
