using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;

namespace Habanero.BO
{
    /// <summary>
    /// A data Accessor used for accessing a remote source.
    /// </summary>
    public class DataAccessorRemote : IDataAccessor
    {
        private readonly IDataAccessor _remoteDataAccessor;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="remoteDataAccessor"></param>
        public DataAccessorRemote(IDataAccessor remoteDataAccessor) {
            _remoteDataAccessor = remoteDataAccessor;
        }

        public ITransactionCommitter CreateTransactionCommitter() {
            return new TransactionCommitterRemote( _remoteDataAccessor.CreateTransactionCommitter());
        }
        public IBusinessObjectLoader BusinessObjectLoader
        {
            get { return _remoteDataAccessor.BusinessObjectLoader; }
        }

    }
   
}
