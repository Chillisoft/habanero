using System;
using System.Collections.Generic;
using System.Text;
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
