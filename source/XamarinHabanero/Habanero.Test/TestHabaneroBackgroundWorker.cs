using System;
using Habanero.Base;
using NUnit.Framework;
// ReSharper disable InconsistentNaming
namespace Habanero.Test
{
    [TestFixture]
    public class TestHabaneroBackgroundWorker
    {
        private class MyMethodDispatcher : IActionDispatcher
        {
            public void Dispatch(Action method)
            {
                method();
            }
        }

        [Test]
        public void Run_CallsSuccessDelegateOnBackgroundSuccess()
        {
            //---------------Set up test pack-------------------
            var successCalled = false;
            var cancelCalled = false;
            var exceptionCalled = false;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var worker = HabaneroBackgroundWorker.Run(
                new MyMethodDispatcher(),
                null, data => true, data => { successCalled = true; }, data => { cancelCalled = true; }, ex => { exceptionCalled = true; });
            worker.WaitForBackgroundWorkerToComplete();
            //---------------Test Result -----------------------
            Assert.IsTrue(successCalled, "Success delegate not called (and should have been)");
            Assert.IsFalse(cancelCalled, "Cancel delegate was called (and shoudn't have been)");
            Assert.IsFalse(exceptionCalled, "Exception delegate called (and shouldn't have been)");
        }

        [Test]
        public void Run_CallsCancelDelegateOnBackgroundCancel()
        {
            //---------------Set up test pack-------------------
            var successCalled = false;
            var cancelCalled = false;
            var exceptionCalled = false;
            //---------------Assert Precondition----------------


            //---------------Execute Test ----------------------
            var worker = HabaneroBackgroundWorker.Run(
                new MyMethodDispatcher(),
                null, data => false, data => { successCalled = true; }, data => { cancelCalled = true; }, ex => { exceptionCalled = true; });
            worker.WaitForBackgroundWorkerToComplete();
            //---------------Test Result -----------------------
            Assert.IsFalse(successCalled, "Success delegate called (and shouldn't have been)");
            Assert.IsTrue(cancelCalled, "Cancel delegate wasn't called (and shoud have been)");
            Assert.IsFalse(exceptionCalled, "Exception delegate called (and shouldn't have been)");
        }

        [Test]
        public void Run_CallsExceptionDelegateOnBackgroundThreadException()
        {
            //---------------Set up test pack-------------------
            var successCalled = false;
            var cancelCalled = false;
            var exceptionCalled = false;
            //---------------Assert Precondition----------------


            //---------------Execute Test ----------------------
            var worker = HabaneroBackgroundWorker.Run(
                new MyMethodDispatcher(),
                null, data => { throw new Exception("Die!"); }, data => { successCalled = true; }, data => { cancelCalled = true; }, ex => { exceptionCalled = true; });
            worker.WaitForBackgroundWorkerToComplete();
            //---------------Test Result -----------------------
            Assert.IsFalse(successCalled, "Success delegate called (and shouldn't have been)");
            Assert.IsTrue(cancelCalled, "Cancel delegate wasn't called (and should have been)");
            Assert.IsTrue(exceptionCalled, "Exception not delegate called (and should have been)");
        }
    }
}
