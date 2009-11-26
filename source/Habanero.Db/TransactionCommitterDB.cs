// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using System;
using System.Collections.Generic;
using System.Data;
using Habanero.Base;
using Habanero.BO;
using log4net;

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
        private static readonly ILog _log = LogManager.GetLogger("Habanero.BO.TransactionCommitterDB");
        private readonly IDatabaseConnection _databaseConnection;
        private IDbConnection _dbConnection;
        private IDbTransaction _dbTransaction;
        private Dictionary<string, ITransactional> _transactionsExecutingToDataSource;


        ///<summary>
        /// Constructs the TransactionCommitterDB
        ///</summary>
        public TransactionCommitterDB() : this(null)
        {
        }

        ///<summary>
        /// Constructs the TransactionCommitter for a specific database connection
        ///</summary>
        public TransactionCommitterDB(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        /// <summary>
        /// Begins the transaction on the appropriate databasource.
        /// </summary>
        protected override void BeginDataSource()
        {
            _transactionsExecutingToDataSource = new Dictionary<string, ITransactional>();
            IDatabaseConnection databaseConnection = GetDatabaseConnection();
            _dbConnection = databaseConnection.GetConnection();
            _dbConnection.Open();
            _dbTransaction = _dbConnection.BeginTransaction(databaseConnection.IsolationLevel);
//            IDbCommand command = _dbConnection.CreateCommand();
//            command.Transaction = _dbTransaction;
//            command.CommandText = "sp_MSForEachTable";
//            command.CommandType = CommandType.StoredProcedure;
//            command.Parameters.Add(new SqlParameter("@command1", "ALTER TABLE ? NOCHECK CONSTRAINT ALL"));
//            command.ExecuteNonQuery();
        }

        private IDatabaseConnection GetDatabaseConnection()
        {
            return _databaseConnection ?? DatabaseConnection.CurrentConnection;
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
            return new TransactionalBusinessObjectDB(businessObject);
        }

        /// <summary>
        /// Add the Business Object for a child added to the relationship.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="relationship"></param>
        /// <param name="businessObject"></param>
        protected override void AddAddedChildBusinessObject<T>(IRelationship relationship, T businessObject)
        {
            AddTransaction(new TransactionalSingleRelationship_Added_DB(relationship, businessObject));
        }

        /// <summary>
        /// Remove the Business Object for a child added to the relationship.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="relationship"></param>
        /// <param name="businessObject"></param>
        protected override void AddRemovedChildBusinessObject<T>(IRelationship relationship, T businessObject)
        {
            AddTransaction(new TransactionalSingleRelationship_Removed_DB(relationship, businessObject));
        }

        /// <summary>
        /// Tries to execute an individual transaction against the datasource.
        /// 1'st phase of a 2 phase database commit.
        /// </summary>
        protected override void ExecuteTransactionToDataSource(ITransactional transaction)
        {
            string transactionID = transaction.TransactionID();
            if (_transactionsExecutingToDataSource.ContainsKey(transactionID)) return;
            _transactionsExecutingToDataSource.Add(transactionID, transaction);

            var transactionDB = (ITransactionalDB)transaction;

            if (transaction is TransactionalBusinessObjectDB)
            {
                IBusinessObject businessObject = ((TransactionalBusinessObjectDB) transaction).BusinessObject;
                if (businessObject.Status.IsDeleted)
                {
                    DeleteRelatedChildren(businessObject);
                    DereferenceRelatedChildren(businessObject);
                }
            }

            ISqlStatementCollection sql = transactionDB.GetPersistSql();
            if (sql == null) return;
            IDatabaseConnection databaseConnection = GetDatabaseConnection();
            databaseConnection.ExecuteSql(sql, _dbTransaction);
            base.ExecuteTransactionToDataSource(transaction);
        }

        /// <summary>
        /// Commits all the successfully executed statements to the datasource.
        /// 2'nd phase of a 2 phase database commit.
        /// </summary>
        protected override bool CommitToDatasource()
        {
            try
            {
//                IDbCommand command = _dbConnection.CreateCommand();
//                command.Transaction = _dbTransaction;
//                command.CommandText = "sp_MSForEachTable";
//                command.CommandType = CommandType.StoredProcedure;
//                command.Parameters.Add(new SqlParameter("@command1", "ALTER TABLE ? CHECK CONSTRAINT ALL"));
//                command.ExecuteNonQuery();
                _dbTransaction.Commit();
                return true;
            }
            finally
            {
                if (_dbConnection != null && _dbConnection.State == ConnectionState.Open)
                {
//                    IDbCommand command = _dbConnection.CreateCommand();
//                    command.Transaction = _dbTransaction;
//                    command.CommandText = "sp_MSForEachTable";
//                    command.CommandType = CommandType.StoredProcedure;
//                    command.Parameters.Add(new SqlParameter("@command1", "ALTER TABLE ? CHECK CONSTRAINT ALL"));
//                    command.ExecuteNonQuery();
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
//                    IDbCommand command = _dbConnection.CreateCommand();
//                    command.Transaction = _dbTransaction;
//                    command.CommandText = "sp_MSForEachTable";
//                    command.CommandType = CommandType.StoredProcedure;
//                    command.Parameters.Add(new SqlParameter("@command1", "ALTER TABLE ? CHECK CONSTRAINT ALL"));
//                    command.ExecuteNonQuery();
                    _dbConnection.Close();
                }
            }
        }
    }
}