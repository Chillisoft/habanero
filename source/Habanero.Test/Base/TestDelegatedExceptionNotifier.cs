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
using System;
using Habanero.Base;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.Base
{
    [TestFixture]
    public class TestDelegatedExceptionNotifier
    {
        [Test]
        public void Test_Construct()
        {
            //---------------Set up test pack-------------------
            DelegatedExceptionNotifier.NotifyDelegate notifyDelegate =
                MockRepository.GenerateStub<DelegatedExceptionNotifier.NotifyDelegate>();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DelegatedExceptionNotifier delegatedExceptionNotifier = new DelegatedExceptionNotifier(notifyDelegate);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(IExceptionNotifier), delegatedExceptionNotifier);
        }

        [Test]
        public void Test_Construct_WhenNullDelegate_ShouldThrowError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                DelegatedExceptionNotifier delegatedExceptionNotifier = new DelegatedExceptionNotifier(null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("notifyDelegate", ex.ParamName);
            }
        }

        [Test]
        public void Test_Notify_WhenCalled_ShouldCallDelegate()
        {
            //---------------Set up test pack-------------------
            Exception exception = new Exception();
            string furtherMessage = TestUtil.GetRandomString();
            string title = TestUtil.GetRandomString();
            DelegatedExceptionNotifier.NotifyDelegate notifyDelegate =
                MockRepository.GenerateStub<DelegatedExceptionNotifier.NotifyDelegate>();
            DelegatedExceptionNotifier delegatedExceptionNotifier = new DelegatedExceptionNotifier(notifyDelegate);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            delegatedExceptionNotifier.Notify(exception, furtherMessage, title );
            //---------------Test Result -----------------------
            notifyDelegate.AssertWasCalled(d => d(exception, furtherMessage, title));
        }

    }
}
