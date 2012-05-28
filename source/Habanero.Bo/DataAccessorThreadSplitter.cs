using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
    public class DataAccessorThreadSplitter : IDataAccessor
    {
        private readonly Dictionary<Thread, IDataAccessor> _dataAccessors;
        
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

        public ITransactionCommitter CreateTransactionCommitter()
        {
            lock (this)
            {
                return GetDataAccessorForThread(Thread.CurrentThread).CreateTransactionCommitter();
            }
        }

        public void AddDataAccessorForThread(IDataAccessor dataAccessor)
        {
            lock (this)
            {
                _dataAccessors.Add(Thread.CurrentThread, dataAccessor);
            }
        }

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