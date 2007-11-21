//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using NUnit.Framework;
using Habanero.Util;

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