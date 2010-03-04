using System;
using NUnit.Framework;

namespace Habanero.Test.UI.Win
{
    [TestFixture]
    public class TestEnumerationsWin
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
    }
}