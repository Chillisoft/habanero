#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO
{
    /// <summary>
    /// A Transaction committer for commiting items to a remote data source 
    /// e.g. via remoting.
    /// </summary>
    public class TransactionCommitterRemote : ITransactionCommitter
    {
        private readonly ITransactionCommitter _remoteTransactionCommitter;

        public TransactionCommitterRemote(ITransactionCommitter remoteTransactionCommitter)
        {
            _remoteTransactionCommitter = remoteTransactionCommitter;
        }

        public void AddBusinessObject(IBusinessObject businessObject)
        {
            _remoteTransactionCommitter.AddBusinessObject(businessObject);
        }

        public void AddTransaction(ITransactional transaction)
        {
            _remoteTransactionCommitter.AddTransaction(transaction);
        }

        List<Guid> ITransactionCommitter.CommitTransaction()
        {
            List<Guid> executedTransactions = _remoteTransactionCommitter.CommitTransaction();
            executedTransactions.ForEach(guid => ((BusinessObject) BORegistry.BusinessObjectManager[guid]).UpdateStateAsPersisted());
            return executedTransactions;
        }
    }
}