//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util
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
        [Test]
        public void TestCall()
        {
            DateTime time = DateTime.Now;

            DelayedMethodCall call = new DelayedMethodCall(200, this);
            call.Call(new VoidMethodWithSender(MethodToCall));
             
            while (!calledYet && (DateTime.Now - time).TotalMilliseconds < 1000)
            {
                //
            }
            Assert.IsTrue(calledYet);
            double difference = (DateTime.Now - time).TotalMilliseconds;
            Assert.GreaterOrEqual(difference, 200d);
            Assert.LessOrEqual(difference, 950d);
        }

        private void MethodToCall(object sender)
        {
            calledYet = true;
        }
    }
}