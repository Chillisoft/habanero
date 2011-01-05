
using Habanero.Base.Util;
using NUnit.Framework;

namespace Habanero.Test.Util
{
    [TestFixture]
    public class TestHabaneroEnvironmentCF
    {
        [Test]
        public void Test_MachineName_WhenOnDesktop_ShouldReturn_testMachine()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var environ = HabaneroEnvironmentCF.MachineName();
            //---------------Test Result -----------------------
            Assert.AreEqual("testMachine", environ);
        }

        [Test]
        public void Test_GetCurrentUser_WhenCalled_Returns_NA()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string currentUser = HabaneroEnvironmentCF.GetCurrentUser();
            //---------------Test Result -----------------------
            Assert.AreEqual("NA", currentUser);
        }
    }
}
