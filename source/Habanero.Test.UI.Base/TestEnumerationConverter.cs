using System;
using System.Collections.Generic;
using System.Text;
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
