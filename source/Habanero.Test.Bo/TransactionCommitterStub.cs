using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO;

namespace Habanero.Test.BO
{
    public class TransactionCommitterStub: TransactionCommitter
    {
        /// <summary>
        /// Begins the transaction on the appropriate databasource.
        /// </summary>
        protected override void BeginDataSource()
        {
            
        }

        /// <summary>
        /// Commits all the successfully executed statements to the datasource.
        /// 2'nd phase of a 2 phase database commit.
        /// </summary>
        protected override void CommitToDatasource()
        {
            _CommittSuccess = true;
        }

        /// <summary>
        /// In the event of any errors occuring during executing statements to the datasource 
        /// <see cref="TransactionCommitter.ExecuteTransactionToDataSource"/> or during committing to the datasource
        /// <see cref="TransactionCommitter.CommitToDatasource"/>
        /// </summary>
        protected override void TryRollback()
        {
            
        }
    }
}
