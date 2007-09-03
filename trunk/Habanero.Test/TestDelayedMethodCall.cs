using System;
using NUnit.Framework;
using Habanero.Util;

namespace Habanero.Test
{
    [TestFixture]
    public class TestDelayedMethodCall
    {
        private bool calledYet = false;
        
        /// <summary>
        /// Note that the test values seem to leave a lot of leeway - this is
        /// because of our slow build server, may need further review because the tests
        /// are still failing sometimes on the server
        /// </summary>
        [Test, Ignore("The delay is not working on the slow integration server")]
        public void TestCall()
        {
            DateTime time = DateTime.Now;

            DelayedMethodCall call = new DelayedMethodCall(200, this);
            call.Call(new VoidMethodWithSender(MethodToCall));
             
            while (!calledYet && (DateTime.Now - time).TotalMilliseconds < 600)
            {
                //
            }
            Assert.IsTrue(calledYet);
            double difference = (DateTime.Now - time).TotalMilliseconds;
            Assert.GreaterOrEqual(difference, 200d);
            Assert.LessOrEqual(difference, 550d);
        }

        private void MethodToCall(object sender)
        {
            calledYet = true;
        }
    }
}