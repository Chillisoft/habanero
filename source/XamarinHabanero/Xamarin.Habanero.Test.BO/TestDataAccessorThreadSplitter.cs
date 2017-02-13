using System.Threading;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using NUnit.Framework;

namespace Habanero.Test.BO
{
     [TestFixture]
    public class TestDataAccessorThreadSplitter
    {
        [Test]
        public void Test_Construct_WithDefaults()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            IDataAccessor dataAccessor = new DataAccessorThreadSplitter(new DataAccessorInMemory());
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(IBusinessObjectLoader), dataAccessor.BusinessObjectLoader);
            Assert.IsInstanceOf(typeof(ITransactionCommitter), dataAccessor.CreateTransactionCommitter());
            //---------------Tear down -------------------------
        }

         [Test]
         public void Test_WithThread()
         {
             //---------------Set up test pack-------------------
             const int BarrierTimeout = 10000;
             var dataAccessorMain = new DataAccessorInMemory();
             var dataAccessor = new DataAccessorThreadSplitter(dataAccessorMain);
             var expectedDataAccessorForThread = new DataAccessorInMemory();
             using (var entryBarrier = new Barrier(2))
             using (var exitBarrier = new Barrier(2))
             {
                 var thread = new Thread(() =>
                 {
                     dataAccessor.AddDataAccessorForThread(expectedDataAccessorForThread);
                     entryBarrier.SignalAndWait(BarrierTimeout);
                     exitBarrier.SignalAndWait(BarrierTimeout);
                 });
                 thread.Start();
                 entryBarrier.SignalAndWait(BarrierTimeout);
                 //---------------Execute Test ----------------------
                 var dataAccessorForThread = dataAccessor.GetDataAccessorForThread(thread);
                 exitBarrier.SignalAndWait(BarrierTimeout);
                 //---------------Test Result -----------------------
                 Assert.AreSame(expectedDataAccessorForThread, dataAccessorForThread);
             }
         }

         [Test]
         public void Test_ClearDeadThreads()
         {
             //---------------Set up test pack-------------------
             const int BarrierTimeout = 10000;
             var dataAccessorMain = new DataAccessorInMemory();
             var dataAccessor = new DataAccessorThreadSplitter(dataAccessorMain);
             var expectedDataAccessorForThread = new DataAccessorInMemory();
             using (var entryBarrier = new Barrier(2))
             using (var exitBarrier = new Barrier(2))
             {
                 var thread = new Thread(() =>
                 {
                     dataAccessor.AddDataAccessorForThread(expectedDataAccessorForThread);
                     entryBarrier.SignalAndWait(BarrierTimeout);
                     exitBarrier.SignalAndWait(BarrierTimeout);
                 });
                 thread.Start();
                 entryBarrier.SignalAndWait(BarrierTimeout);
                 //---------------Assert preconditions---------------
                 Assert.AreSame(expectedDataAccessorForThread, dataAccessor.GetDataAccessorForThread(thread));
                 //---------------Execute Test ----------------------
                 exitBarrier.SignalAndWait(BarrierTimeout);
                 thread.Join();
                 dataAccessor.ClearDeadThreads();
                 //---------------Test Result -----------------------
                 var exception = Assert.Throws<HabaneroDeveloperException>(() => dataAccessor.GetDataAccessorForThread(thread));
                 StringAssert.Contains("Data accessor for thread does not exist", exception.Message);
             }
         }
    }
}
