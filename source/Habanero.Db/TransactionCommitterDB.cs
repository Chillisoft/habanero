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
using System.Data;
using Habanero.Base;
using Habanero.BO;

namespace Habanero.DB
{
    /// <summary>
    /// Provides a transaction committer that persists data to a
    /// database using SQL.
    /// <br/>
    /// A released application will often use a database, whereas a set of
    /// tests for the application used during development and maintenance may use
    /// a combination of database stores and in-memory stores.  Database storage
    /// is useful as a test of structure, but runs comparatively slowly to
    /// in-memory testing, which should be used for testing of the logic.
    /// </summary>
    [Serializable]
    public class TransactionCommitterDB : TransactionCommitter
    {
        private readonly IDatabaseConnection _databaseConnection;
        private IDbConnection _dbConnection;
        private IDbTransaction _dbTransaction;
        private Dictionary<string, ITransactional> _transactionsExecutingToDataSource;

        ///<summary>
        /// Constructs the TransactionCommitter for a specific database connection
        ///</summary>
        public TransactionCommitterDB(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        /// <summary>
        /// The <see cref="IDatabaseConnection"/> this transaction committer uses
        /// </summary>
        public IDatabaseConnection DatabaseConnection
        {
            get { return _databaseConnection; }
        }

        /// <summary>
        /// Begins the transaction on the appropriate databasource.
        /// </summary>
        protected override void BeginDataSource()
        {
            _transactionsExecutingToDataSource = new Dictionary<string, ITransactional>();
            _dbConnection = _databaseConnection.GetConnection();
            _dbConnection.Open();
            _dbTransaction = _databaseConnection.BeginTransaction(_dbConnection);
        }

        /// <summary>
        /// Decorates the business object with a TransactionalBusinessObjectDB.
        /// See <see cref="TransactionCommitter.CreateTransactionalBusinessObject" />
        /// </summary>
        /// <param name="businessObject">The business object to decorate in a TransactionalBusinessObjectDB</param>
        /// <returns>The decorated TransactionalBusinessObjectDB</returns>
        protected override TransactionalBusinessObject CreateTransactionalBusinessObject(
            IBusinessObject businessObject)
        {
            return new TransactionalBusinessObjectDB(businessObject, _databaseConnection);
        }

        /// <summary>
        /// Add the Business Object for a child added to the relationship.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="relationship"></param>
        /// <param name="businessObject"></param>
        protected override void AddAddedChildBusinessObject<T>(IRelationship relationship, T businessObject)
        {
            AddTransaction(new TransactionalSingleRelationship_Added_DB(relationship, businessObject, _databaseConnection));
        }

        /// <summary>
        /// Remove the Business Object for a child added to the relationship.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="relationship"></param>
        /// <param name="businessObject"></param>
        protected override void AddRemovedChildBusinessObject<T>(IRelationship relationship, T businessObject)
        {
            AddTransaction(new TransactionalSingleRelationship_Removed_DB(relationship, businessObject, _databaseConnection));
        }

        /// <summary>
        /// Tries to execute an individual transaction against the datasource.
        /// 1'st phase of a 2 phase database commit.
        /// </summary>
        protected override void ExecuteTransactionToDataSource(ITransactional transaction)
        {
            var transactionID = transaction.TransactionID();
            if (_transactionsExecutingToDataSource.ContainsKey(transactionID)) return;
            _transactionsExecutingToDataSource.Add(transactionID, transaction);

            if (!ExecuteTransactionToDB(transaction)) return;

            base.ExecuteTransactionToDataSource(transaction);
        }

        /// <summary>
        /// This does the actual execution of the <see cref="ITransactional"/> object to the DB.
        /// This is an injection point to handle alternate persistance options for the <see cref="ITransactional"/> object.
        /// For example, an alternate <see cref="ITransactional"/> object type for bulk operations could be handled here using some 
        /// other mechanism than the <see cref="ITransactionalDB"/>.<see cref="ITransactionalDB.GetPersistSql"/> being executed on the DB.
        /// </summary>
        /// <param name="transactional">The <see cref="ITransactional"/> object to execute to the DB</param>
        /// <returns>True if the transaction was executed, or False if it could not be executed.</returns>
        protected virtual bool ExecuteTransactionToDB(ITransactional transactional)
        {
            var transactionDB = (ITransactionalDB) transactional;

            var transactionalBusinessObjectDB = transactional as TransactionalBusinessObjectDB;
            if (transactionalBusinessObjectDB != null)
            {
                var businessObject = transactionalBusinessObjectDB.BusinessObject;
                if (businessObject.Status.IsDeleted)
                {
                    DeleteRelatedChildren(businessObject);
                    DereferenceRelatedChildren(businessObject);
                }
            }

            var sql = transactionDB.GetPersistSql();
            if (sql == null) return false;
            var databaseConnection = _databaseConnection;
            databaseConnection.ExecuteSql(sql, _dbTransaction);
            return true;
        }

        /// <summary>
        /// Commits all the successfully executed statements to the datasource.
        /// 2'nd phase of a 2 phase database commit.
        /// </summary>
        protected override bool CommitToDatasource()
        {
            try
            {
                _dbTransaction.Commit();
                return true;
            }
            finally
            {
                if (_dbConnection != null && _dbConnection.State == ConnectionState.Open)
                {
                    _dbConnection.Close();
                }
            }
        }

        /// <summary>
        /// In the event of any errors occuring during executing statements to the datasource 
        /// <see cref="TransactionCommitter.ExecuteTransactionToDataSource"/> or during committing to the datasource
        /// <see cref="TransactionCommitter.CommitToDatasource"/>
        /// </summary>
        protected override void TryRollback()
        {
            try
            {
                if (_dbTransaction != null) _dbTransaction.Rollback();
            }
            finally
            {
                if (_dbConnection != null && _dbConnection.State == ConnectionState.Open)
                {
                    _dbConnection.Close();
                }
            }
        }
    }
}