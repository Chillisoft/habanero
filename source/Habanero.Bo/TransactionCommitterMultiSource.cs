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
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
    /// <summary>
    /// An implementation of <see cref="ITransactionCommitter"/> used as an aggregate transaction committer. It stores a 
    /// dictionary of <see cref="IDataAccessor"/> - one for each type, as well as a default. If the business object added
    /// is linked to a particular DataAccessor it will be added to that one, otherwise it will be added to the default.
    /// Note that only one underlying DataAccessor will be allowed to be used. If you add business objects whose types are
    /// linked to different DataAccessors you will get an error.
    /// 
    /// This can be used when you have some objects that are persisted to one database and others to another. See <see cref="DataAccessorMultiSource"/>
    /// </summary>
    public class TransactionCommitterMultiSource : ITransactionCommitter
    {
        private readonly IDataAccessor _defaultDataAccessor;
        private readonly Dictionary<Type, IDataAccessor> _dataAccessors;

        private ITransactionCommitter _transactionCommitter;
        private IDataAccessor _myDataAccessor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="defaultDataAccessor">The default <see cref="IDataAccessor"/>, used for all objects that don't have an alternate DataAccessor specified</param>
        /// <param name="dataAccessors">The alternate DataAccessors - each type can be linked to one data accessor (and more than one type to the same one.</param>
        public TransactionCommitterMultiSource(IDataAccessor defaultDataAccessor, Dictionary<Type, IDataAccessor> dataAccessors)
        {
            _defaultDataAccessor = defaultDataAccessor;
            _dataAccessors = dataAccessors;
        }


        ///<summary>
        /// Add an object of type business object to the transaction.
        /// The DBTransactionCommiter wraps this Business Object in the
        /// appropriate Transactional Business Object
        ///</summary>
        ///<param name="businessObject"></param>
        public void AddBusinessObject(IBusinessObject businessObject)
        {
            if (_myDataAccessor == null)
            {
                _myDataAccessor = GetDataAccessorForType(businessObject.GetType());
                _transactionCommitter = _myDataAccessor.CreateTransactionCommitter();
            } else
            {
                IDataAccessor dataAccessorToUseForType = GetDataAccessorForType(businessObject.GetType());
                if (dataAccessorToUseForType != _myDataAccessor)
                {
                    throw new HabaneroDeveloperException("A problem occurred while trying to save, please see log for details", string.Format("A BusinessObject of type {0} was added to a TransactionCommitterMultiSource which has been set up with a different source to this type.", businessObject.GetType().FullName));
                }
            }
            _transactionCommitter.AddBusinessObject(businessObject);
        }

        private IDataAccessor GetDataAccessorForType(Type type)
        {
            return _dataAccessors.ContainsKey(type) ? _dataAccessors[type] : _defaultDataAccessor;
        }

        ///<summary>
        /// This method adds an <see cref="ITransactional"/> to the list of transactions.
        ///</summary>
        ///<param name="transaction">The transaction to add to the <see cref="ITransactionCommitter"/>.</param>
        public void AddTransaction(ITransactional transaction)
        {
            if (_myDataAccessor == null)
            {
                _myDataAccessor = _defaultDataAccessor;
                _transactionCommitter = _myDataAccessor.CreateTransactionCommitter();
            }
            _transactionCommitter.AddTransaction(transaction);
        }

        ///<summary>
        /// Commit the transactions to the datasource e.g. the database, file, memory DB
        ///</summary>
        ///<returns></returns>
        public List<Guid> CommitTransaction()
        {
            return _transactionCommitter != null ? _transactionCommitter.CommitTransaction() : new List<Guid>();
        }
    }
}