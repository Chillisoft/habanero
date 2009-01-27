using System;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestEnumerations
    {
        [Test]
        public void TestHorizontalAlignment_HabaneroSameAsWin()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            Assert.AreEqual(Convert.ToInt32(System.Windows.Forms.HorizontalAlignment.Center),
                Convert.ToInt32(Habanero.UI.Base.HorizontalAlignment.Center));
            Assert.AreEqual(Convert.ToInt32(System.Windows.Forms.HorizontalAlignment.Left),
                Convert.ToInt32(Habanero.UI.Base.HorizontalAlignment.Left));
            Assert.AreEqual(Convert.ToInt32(System.Windows.Forms.HorizontalAlignment.Right),
                Convert.ToInt32(Habanero.UI.Base.HorizontalAlignment.Right));
        }

        // VWG has the HorizontalAlignment done in different order to WinForms
        [Test]
        public void TestHorizontalAlignment_HabaneroDifferentToVWG()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            Assert.AreEqual(Convert.ToInt32(Gizmox.WebGUI.Forms.HorizontalAlignment.Center),
                Convert.ToInt32(Habanero.UI.Base.HorizontalAlignment.Left));
            Assert.AreEqual(Convert.ToInt32(Gizmox.WebGUI.Forms.HorizontalAlignment.Right),
                Convert.ToInt32(Habanero.UI.Base.HorizontalAlignment.Center));
            Assert.AreEqual(Convert.ToInt32(Gizmox.WebGUI.Forms.HorizontalAlignment.Left),
                Convert.ToInt32(Habanero.UI.Base.HorizontalAlignment.Right));
        }
    }
}