using System;
using NUnit.Framework;
using Habanero.Util;

namespace Habanero.Test
{
    [TestFixture]
    public class TestDelayedMethodCall
    {
        private bool calledYet = false;
        
        [Test]
        public void TestCall()
        {
            DateTime time = DateTime.Now;

            DelayedMethodCall call = new DelayedMethodCall(200, this);
            call.Call(new VoidMethodWithSender(MethodToCall));

            while (!calledYet && (DateTime.Now - time).TotalMilliseconds < 400)
            {
                //
            }
            Assert.IsTrue(calledYet);
            double difference = (DateTime.Now - time).TotalMilliseconds;
            Assert.GreaterOrEqual(difference, 200d);
            Assert.LessOrEqual(difference, 300d);
        }

        private void MethodToCall(object sender)
        {
            calledYet = true;
        }
    }
}