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
        protected IObjectContainer _objectContainer;

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
                    IList<BusinessObjectDTO> matchingObjects = _objectContainer.Query<BusinessObjectDTO>(
                                        obj => obj.ClassDefName == tbo.BusinessObject.ClassDef.ClassName && obj.ID == tbo.BusinessObject.ID.ToString());
                    BusinessObjectDTO dtoToDelete = matchingObjects[0];
                    _objectContainer.Delete(dtoToDelete);
                }
                else if (tbo.BusinessObject.Status.IsNew)
                {
                    BusinessObjectDTO dto = new BusinessObjectDTO(tbo.BusinessObject);
                    _objectContainer.Store(dto);
                } else
                {
                    IList<BusinessObjectDTO> matchingObjects = _objectContainer.Query<BusinessObjectDTO>(
                                        obj => obj.ClassDefName == tbo.BusinessObject.ClassDef.ClassName && obj.ID == tbo.BusinessObject.ID.ToString());
                    BusinessObjectDTO dtoToUpdate = matchingObjects[0];
                    foreach (IBOProp boProp in tbo.BusinessObject.Props)
                    {
                        dtoToUpdate.Props[boProp.PropertyName.ToUpper()] = boProp.Value;
                    }
                    _objectContainer.Store(dtoToUpdate);
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

        protected override void AddAddedChildBusinessObject<T>(IRelationship relationship, T businessObject)
        {
            this.AddBusinessObject(businessObject);
        }
        protected override void AddRemovedChildBusinessObject<T>(IRelationship relationship, T businessObject)
        {
            this.AddBusinessObject(businessObject);
        }
    }
}