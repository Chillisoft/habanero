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
    /// This class manages and commits a collection of ITransactions to a database using SQL.
    /// </summary>
    public class TransactionCommitterDB : TransactionCommitter
    {
        private readonly IDatabaseConnection _databaseConnection;
        private IDbTransaction _dbTransaction;
        private IDbConnection _dbConnection;
        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.TransactionCommitterDB");


        ///<summary>
        /// Constructs the TransactionCommitterDB
        ///</summary>
        public TransactionCommitterDB(): this(null)
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
            _dbConnection = GetDatabaseConnection().GetConnection();
            _dbConnection.Open();
            _dbTransaction = _dbConnection.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        private IDatabaseConnection GetDatabaseConnection()
        {
            if (_databaseConnection != null)
            {
                return _databaseConnection;
            } else
            {
                return DatabaseConnection.CurrentConnection;
            }
        }

        /// <summary>
        /// Tries to execute an individual transaction against the datasource.
        /// 1'st phase of a 2 phase database commit.
        /// </summary>
        protected override void ExecuteTransactionToDataSource(ITransactional transaction)
        {
            ITransactionalDB transactionDB = (ITransactionalDB)transaction;

            if (transaction is TransactionalBusinessObjectDB)
            {
                TransactionalBusinessObjectDB transactionBusObjDB = (TransactionalBusinessObjectDB) transaction;
                if (transactionBusObjDB.IsDeleted)
                {
                    DeleteRelatedChildren(transactionBusObjDB);
                    DereferenceRelatedChildren(transactionBusObjDB);
                }
            }

            ISqlStatementCollection sql = transactionDB.GetPersistSql();
            if (sql == null) return;
            IDatabaseConnection databaseConnection = GetDatabaseConnection();
            databaseConnection.ExecuteSql(sql, _dbTransaction);
            base.ExecuteTransactionToDataSource(transaction);
        }

        private void DereferenceRelatedChildren(TransactionalBusinessObject transaction)
        {
            foreach (Relationship relationship in transaction.BusinessObject.Relationships)
            {
                if (MustDereferenceRelatedObjects(relationship))
                {
                    IBusinessObjectCollection col = relationship.GetRelatedBusinessObjectCol();
                    for (int i = col.Count - 1; i >= 0; i--)
                    {
                        BusinessObject bo = col[i];
                        foreach (RelPropDef relPropDef in relationship.RelationshipDef.RelKeyDef)
                        {
                            bo.SetPropertyValue(relPropDef.RelatedClassPropName, null);
                        }
                        ExecuteTransactionToDataSource(new TransactionalBusinessObjectDB(bo));
                    }
                }
            }
        }

        private static bool MustDereferenceRelatedObjects(Relationship relationship)
        {
            return relationship.DeleteParentAction == DeleteParentAction.DereferenceRelated;
        }

        private void DeleteRelatedChildren(TransactionalBusinessObject transaction)
        {
            foreach (Relationship relationship in transaction.BusinessObject.Relationships)
            {
                if (MustDeleteRelatedObjects(relationship))
                {
                    IBusinessObjectCollection col = relationship.GetRelatedBusinessObjectCol();
                    for (int i = col.Count - 1; i >= 0; i--)
                    {
                        BusinessObject bo = col[i];
                        bo.Delete();
                        ExecuteTransactionToDataSource(new TransactionalBusinessObjectDB(bo));
                    }
                }
            }
        }

        private static bool MustDeleteRelatedObjects(Relationship relationship)
        {
            return relationship.DeleteParentAction == DeleteParentAction.DeleteRelated;
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
                _CommittSuccess = true;
            }
            catch (Exception ex)
            {
                log.Error("Error commiting transaction: " + Environment.NewLine +
                    ExceptionUtilities.GetExceptionString(ex, 4, true));
                TryRollback();
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
        protected override void TryRollback()
        {
            try
            {
                if (_dbTransaction != null) _dbTransaction.Rollback();
            }
            catch (Exception ex)
            {
                log.Error("Error rolling back transaction: " + Environment.NewLine +
                    ExceptionUtilities.GetExceptionString(ex, 4, true));
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

        ///<summary>
        /// Add an object of type business object to the transaction.
        /// The DBTransactionCommiter wraps this Business Object in the
        /// appropriate Transactional Business Object
        ///</summary>
        ///<param name="bo"></param>
        public override void AddBusinessObject(BusinessObject bo)
        {
            this.AddTransaction(new TransactionalBusinessObjectDB(bo));
        }
    }
}