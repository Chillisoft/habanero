using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;

namespace Habanero.BO
{
    public class DataAccessorRemote : IDataAccessor
    {
        private readonly IDataAccessor _remoteDataAccessor;

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
