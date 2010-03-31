using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.Base;
using Habanero.BO;

namespace Habanero.Testing.Base
{
    public class TransactionCommitterInMemoryStubErrorRaising : TransactionCommitterInMemory
    {
        public TransactionCommitterInMemoryStubErrorRaising(DataStoreInMemory dataStoreInMemory) : base(dataStoreInMemory)
        {

        }

        protected override bool CommitToDatasource()
        {
            throw new Exception("");
        }
    }
}
