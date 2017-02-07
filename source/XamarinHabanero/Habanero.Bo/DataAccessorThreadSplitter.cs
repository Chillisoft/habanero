using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
    /// <summary>
    /// This Data accessor manages separate sub-data accessors per thread. This is needed because in some cases
    /// the ADO.NET provider is not thread-safe (eg SQL CE) so maintaining a separate database connection per thread
    /// is required.
    /// 
    /// To use it make sure that each thread in your system has added its own dataaccessor using <see cref="AddDataAccessorForThread"/>
    /// </summary>
    public class DataAccessorThreadSplitter : IDataAccessor
    {
        private readonly Dictionary<Thread, IDataAccessor> _dataAccessors;
        
        /// <summary>
        /// Construct using an initial <see cref="IDataAccessor"/>, which will be the one used for this thread.
        /// </summary>
        /// <param name="dataAccessor"></param>
        public DataAccessorThreadSplitter(IDataAccessor dataAccessor)
        {
            _dataAccessors = new Dictionary<Thread, IDataAccessor>();
            AddDataAccessorForThread(dataAccessor);
            var thread = new Thread(() =>
                                           {
                                               while (true)
                                               {
                                                   ClearDeadThreads();
                                                   Thread.Sleep(5000);
                                               }
                                           });
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// The <see cref="IDataAccessor.BusinessObjectLoader"/> to use to load BusinessObjects
        /// </summary>
        public IBusinessObjectLoader BusinessObjectLoader
        {
            get
            {
                lock (this)
                {
                    return GetDataAccessorForThread(Thread.CurrentThread).BusinessObjectLoader;
                }
            }
        }

        /// <summary>
        /// Creates a TransactionCommitter for you to use to persist BusinessObjects. A new TransactionCommitter is required
        /// each time an object or set of objects is persisted.
        /// </summary>
        /// <returns></returns>
        public ITransactionCommitter CreateTransactionCommitter()
        {
            lock (this)
            {
                return GetDataAccessorForThread(Thread.CurrentThread).CreateTransactionCommitter();
            }
        }

        /// <summary>
        /// Adds a data accessor to the collection of data accessors. This data accessor will be used for this thread.
        /// </summary>
        /// <param name="dataAccessor"></param>
        public void AddDataAccessorForThread(IDataAccessor dataAccessor)
        {
            lock (this)
            {
                _dataAccessors.Add(Thread.CurrentThread, dataAccessor);
            }
        }

        /// <summary>
        /// Returns the data accessor for the given thread. You can use this to check if your current thread has a data accessor before
        /// trying to use it.
        /// </summary>
        /// <param name="thread"></param>
        /// <returns>The <see cref="IDataAccessor"/></returns>
        /// <exception cref="HabaneroDeveloperException">If no data accessor for this thread is found this exception will be thrown.</exception>
        public IDataAccessor GetDataAccessorForThread(Thread thread)
        {
            try
            {
                lock (this)
                {
                    return _dataAccessors[thread];
                }
            } catch (KeyNotFoundException)
            {
                throw new HabaneroDeveloperException("A Data accessor for thread does not exist");
            }
        }

        /// <summary>
        /// Clears data accessors for threads that have died.
        /// This method is called every 5 seconds, or can be called explicitly.
        /// </summary>
        public void ClearDeadThreads()
        {
            lock (this)
            {
                var threads = from thread in _dataAccessors.Select(pair => pair.Key)
                              where !thread.IsAlive
                              select thread;

                foreach (var thread in threads.ToArray())
                {
                    _dataAccessors.Remove(thread);
                }
            }
        }
    }
}