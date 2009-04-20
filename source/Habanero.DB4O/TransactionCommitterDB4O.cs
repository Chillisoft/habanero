using System;
using System.Collections.Generic;
using Db4objects.Db4o;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using log4net;

namespace Habanero.DB4O
{
    public class TransactionCommitterDB4O : TransactionCommitter
    {
        private readonly IObjectContainer _objectContainer;

        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.TransactionCommitterDB4O");
        public TransactionCommitterDB4O(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        protected override void BeginDataSource()
        {

        }

        protected override void TryRollback()
        {
            if (_objectContainer == null) return;
            //try
            //{
                _objectContainer.Rollback();
            //}
            //finally
            //{
            //    _objectContainer.Close();
            //}
        }

        protected override void ExecuteTransactionToDataSource(ITransactional transaction)
        {
            if (transaction is TransactionalBusinessObject)
            {
                TransactionalBusinessObject tbo = transaction as TransactionalBusinessObject;
                tbo.BusinessObject.ClassDefName = tbo.BusinessObject.ClassDef.ClassName;

                if (tbo.BusinessObject.Status.IsDeleted)
                {
                    //IList<BusinessObject> matchingObjects =
                    //                db.Query<BusinessObject>(
                    //                    obj => obj.ClassDefName == classDef.ClassName && criteria.IsMatch(obj, false));

                    IBusinessObject boToDelete = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(tbo.BusinessObject.ClassDef,
                                                                                                                tbo.BusinessObject.ID);
                    _objectContainer.Delete(boToDelete);
                }
                else
                {
                    _objectContainer.Store(tbo.BusinessObject);
                }
                _executedTransactions.Add(transaction);
            }
        }

        protected override bool CommitToDatasource()
        {
            //try
            //{
                _objectContainer.Commit();
                return true;
            //}
            //finally
            //{
            //    if (_db4o != null) _db4o.Close();
            //}
        }

        protected override TransactionalBusinessObject CreateTransactionalBusinessObject(IBusinessObject businessObject)
        {
            return new TransactionalBusinessObjectDB4O(businessObject);
        }
    }
}