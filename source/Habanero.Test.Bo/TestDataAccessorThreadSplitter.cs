using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Habanero.BO;
using Habanero.Base;
using Habanero.Base.Exceptions;
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
             var dataAccessorMain = new DataAccessorInMemory();
             var dataAccessor = new DataAccessorThreadSplitter(dataAccessorMain);
             var expectedDataAccessorForThread = new DataAccessorInMemory();
             var thread = new Thread(() =>
                                         {
                                             dataAccessor.AddDataAccessorForThread(expectedDataAccessorForThread);
                                             Thread.Sleep(1000);
                                         });
             thread.Start();
             Thread.Sleep(500);
             //---------------Execute Test ----------------------
             var dataAccessorForThread = dataAccessor.GetDataAccessorForThread(thread);
             //---------------Test Result -----------------------
             Assert.AreSame(expectedDataAccessorForThread, dataAccessorForThread);
         }

         [Test]
         public void Test_ClearDeadThreads()
         {
             //---------------Set up test pack-------------------
             var dataAccessorMain = new DataAccessorInMemory();
             var dataAccessor = new DataAccessorThreadSplitter(dataAccessorMain);
             var expectedDataAccessorForThread = new DataAccessorInMemory();
             var thread = new Thread(() => dataAccessor.AddDataAccessorForThread(expectedDataAccessorForThread));
             thread.Start();
             thread.Join();
             //---------------Assert preconditions---------------
             Assert.AreSame(expectedDataAccessorForThread, dataAccessor.GetDataAccessorForThread(thread));
             //---------------Execute Test ----------------------
             dataAccessor.ClearDeadThreads();
             //---------------Test Result -----------------------
             try
             {
                 Assert.IsNull(dataAccessor.GetDataAccessorForThread(thread));
                 Assert.Fail("An exception should be thrown");
             } catch (HabaneroDeveloperException ex)
             {
                 StringAssert.Contains("Data accessor for thread does not exist", ex.Message);
             }
         }
    }
}
