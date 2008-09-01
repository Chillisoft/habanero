//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO
{

    public class TransactionCommitterInMemory : TransactionCommitter
    {
        private readonly DataStoreInMemory _dataStoreInMemory;

        public TransactionCommitterInMemory(DataStoreInMemory dataStoreInMemory)
        {
            _dataStoreInMemory = dataStoreInMemory;
        }

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
            UpdateTransactionsAsCommited();
        }

        /// <summary>
        /// In the event of any errors occuring during executing statements to the datasource 
        /// <see cref="TransactionCommitter.ExecuteTransactionToDataSource"/> or during committing to the datasource
        /// <see cref="TransactionCommitter.CommitToDatasource"/>
        /// </summary>
        protected override void TryRollback()
        {
            
        }

        /// <summary>
        /// Used to decorate a businessObject in a TransactionalBusinessObject. To be overridden in the concrete 
        /// implementation of a TransactionCommitter depending on the type of transaction you need.
        /// </summary>
        /// <param name="businessObject">The business object to decorate</param>
        /// <returns>A decorated Business object (TransactionalBusinessObject)</returns>
        protected override TransactionalBusinessObject CreateTransactionalBusinessObject(IBusinessObject businessObject)
        {
           return new TransactionalBusinessObject(businessObject);
        }

        protected override void ExecuteTransactionToDataSource(ITransactional transaction)
        {
            if (transaction is TransactionalBusinessObject)
            {
                IBusinessObject businessObject = ((TransactionalBusinessObject) transaction).BusinessObject;
                if (!_dataStoreInMemory.AllObjects.ContainsKey(businessObject.ID))
                {
                    _dataStoreInMemory.Add(businessObject);
                }
                else if (businessObject.Status.IsDeleted) _dataStoreInMemory.Remove(businessObject);
            }
            base.ExecuteTransactionToDataSource(transaction);
        }
    }
}