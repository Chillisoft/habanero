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
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestEnumerationConverter
    {
        [Test]
        public void Test_ConvertHorizontalAlignment_HabaneroToVWG_Left()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            Gizmox.WebGUI.Forms.HorizontalAlignment horizontalAlignment = 
                EnumerationConverter.HorizontalAlignmentToVWG(HorizontalAlignment.Left);
            //---------------Test Result -----------------------
            Assert.AreEqual(typeof(Gizmox.WebGUI.Forms.HorizontalAlignment), horizontalAlignment.GetType());
            Assert.AreEqual(Gizmox.WebGUI.Forms.HorizontalAlignment.Left.ToString(), horizontalAlignment.ToString());
        }
        
        [Test]
        public void Test_ConvertHorizontalAlignment_HabaneroToVWG_Right()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            Gizmox.WebGUI.Forms.HorizontalAlignment horizontalAlignment = 
                EnumerationConverter.HorizontalAlignmentToVWG(HorizontalAlignment.Right);
            //---------------Test Result -----------------------
            Assert.AreEqual(typeof(Gizmox.WebGUI.Forms.HorizontalAlignment), horizontalAlignment.GetType());
            Assert.AreEqual(Gizmox.WebGUI.Forms.HorizontalAlignment.Right.ToString(), horizontalAlignment.ToString());
        }

        [Test]
        public void Test_ConvertHorizontalAlignment_HabaneroToVWG_Center()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            Gizmox.WebGUI.Forms.HorizontalAlignment horizontalAlignment =
                EnumerationConverter.HorizontalAlignmentToVWG(HorizontalAlignment.Center);
            //---------------Test Result -----------------------
            Assert.AreEqual(typeof(Gizmox.WebGUI.Forms.HorizontalAlignment), horizontalAlignment.GetType());
            Assert.AreEqual(Gizmox.WebGUI.Forms.HorizontalAlignment.Center.ToString(), horizontalAlignment.ToString());
        }

        [Test]
        public void Test_ConvertHorizontalAlignment_VWGToHabanero_Left()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            HorizontalAlignment horizontalAlignment =
                EnumerationConverter.HorizontalAlignmentToHabanero(Gizmox.WebGUI.Forms.HorizontalAlignment.Left);
            //---------------Test Result -----------------------
            Assert.AreEqual(typeof(HorizontalAlignment), horizontalAlignment.GetType());
            Assert.AreEqual(HorizontalAlignment.Left.ToString(), horizontalAlignment.ToString());
        }

        [Test]
        public void Test_ConvertHorizontalAlignment_VWGToHabanero_Right()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            HorizontalAlignment horizontalAlignment =
                EnumerationConverter.HorizontalAlignmentToHabanero(Gizmox.WebGUI.Forms.HorizontalAlignment.Right);
            //---------------Test Result -----------------------
            Assert.AreEqual(typeof(HorizontalAlignment), horizontalAlignment.GetType());
            Assert.AreEqual(HorizontalAlignment.Right.ToString(), horizontalAlignment.ToString());
        }

        [Test]
        public void Test_ConvertHorizontalAlignment_VWGToHabanero_Center()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            HorizontalAlignment horizontalAlignment =
                EnumerationConverter.HorizontalAlignmentToHabanero(Gizmox.WebGUI.Forms.HorizontalAlignment.Center);
            //---------------Test Result -----------------------
            Assert.AreEqual(typeof(HorizontalAlignment), horizontalAlignment.GetType());
            Assert.AreEqual(HorizontalAlignment.Center.ToString(), horizontalAlignment.ToString());
        }


    }

    
}
