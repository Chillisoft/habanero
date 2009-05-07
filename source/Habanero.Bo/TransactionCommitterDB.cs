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
using System.Data;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using log4net;

namespace Habanero.BO
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
    public class TransactionCommitterDB : TransactionCommitter
    {
        private readonly IDatabaseConnection _databaseConnection;
        private IDbTransaction _dbTransaction;
        private IDbConnection _dbConnection;
        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.TransactionCommitterDB");


        ///<summary>
        /// Constructs the TransactionCommitter for a specific database connection
        ///</summary>
        public TransactionCommitterDB(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        /// <summary>
        /// Constructs the TransactionCommitter using <see cref="DB.DatabaseConnection.CurrentConnection"/>
        /// </summary>
        public TransactionCommitterDB() : this(DB.DatabaseConnection.CurrentConnection)
        {
        }

        ///<summary>
        /// Gets the database connection used to perform database persistence
        ///</summary>
        internal IDatabaseConnection DatabaseConnection
        {
            get { return _databaseConnection; }
        }

        /// <summary>
        /// Begins the transaction on the appropriate databasource.
        /// </summary>
        protected override void BeginDataSource()
        {
            IDatabaseConnection databaseConnection = GetDatabaseConnection();
            _dbConnection = databaseConnection.GetConnection();
            _dbConnection.Open();
            _dbTransaction = _dbConnection.BeginTransaction(databaseConnection.IsolationLevel);
        }

        private IDatabaseConnection GetDatabaseConnection()
        {
            return _databaseConnection ?? DB.DatabaseConnection.CurrentConnection;
        }

        /// <summary>
        /// Decorates the business object with a TransactionalBusinessObjectDB.
        /// See <see cref="TransactionCommitter.CreateTransactionalBusinessObject" />
        /// </summary>
        /// <param name="businessObject">The business object to decorate in a TransactionalBusinessObjectDB</param>
        /// <returns>The decorated TransactionalBusinessObjectDB</returns>
        protected internal override TransactionalBusinessObject CreateTransactionalBusinessObject(
            IBusinessObject businessObject)
        {
            return new TransactionalBusinessObjectDB(businessObject);
        }

        /// <summary>
        /// Tries to execute an individual transaction against the datasource.
        /// 1'st phase of a 2 phase database commit.
        /// </summary>
        protected internal override void ExecuteTransactionToDataSource(ITransactional transaction)
        {
            ITransactionalDB transactionDB = (ITransactionalDB)transaction;

            if (transaction is TransactionalBusinessObjectDB)
            {
                IBusinessObject businessObject = ((TransactionalBusinessObjectDB) transaction).BusinessObject;
                if (businessObject.Status.IsDeleted)
                {
                    DeleteRelatedChildren(businessObject);
                    DereferenceRelatedChildren(businessObject);
                }
            }

            IDatabaseConnection databaseConnection = GetDatabaseConnection();
            ISqlStatementCollection sql = transactionDB.GetPersistSql(databaseConnection);
            if (sql == null) return;
            databaseConnection.ExecuteSql(sql, _dbTransaction);
            base.ExecuteTransactionToDataSource(transaction);
        }

        /// <summary>
        /// Commits all the successfully executed statements to the datasource.
        /// 2'nd phase of a 2 phase database commit.
        /// </summary>
        protected override void CommitToDatasource()
        {
            try
            {
                _dbTransaction.Commit();
                _commitSuccess = true;
            }
            catch (Exception ex)
            {
                log.Error("Error commiting transaction: " + Environment.NewLine +
                    ExceptionUtilities.GetExceptionString(ex, 4, true));
                TryRollback(ex);
                throw;
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
        protected override void TryRollback(Exception origException)
        {
            try
            {
                if (_dbTransaction != null) _dbTransaction.Rollback();
            }
            catch (Exception ex)
            {
                log.Error("Error rolling back transaction: " + Environment.NewLine +
                    ExceptionUtilities.GetExceptionString(ex, 4, true));
                throw origException;
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