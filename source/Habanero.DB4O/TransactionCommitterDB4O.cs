using System.Collections.Generic;
using Db4objects.Db4o;
using Habanero.Base;
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
            _objectContainer.Rollback();
        }

        protected override void ExecuteTransactionToDataSource(ITransactional transaction)
        {
            if (transaction is TransactionalBusinessObject)
            {
                TransactionalBusinessObject tbo = transaction as TransactionalBusinessObject;
                tbo.BusinessObject.ClassDefName = tbo.BusinessObject.ClassDef.ClassName;
                string primaryKey = tbo.BusinessObject.ID.ToString();
                if (tbo.BusinessObject.Status.IsDeleted)
                {
                    DeleteRelatedChildren(tbo.BusinessObject);
                    DereferenceRelatedChildren(tbo.BusinessObject);
                    IList<BusinessObjectDTO> matchingObjects = _objectContainer.Query<BusinessObjectDTO>
                        (obj =>
                         obj.ClassDefName == tbo.BusinessObject.ClassDef.ClassName && obj.ID == primaryKey.ToString());
                    if (matchingObjects.Count > 0)
                    {
                        BusinessObjectDTO dtoToDelete = matchingObjects[0];
                        _objectContainer.Delete(dtoToDelete);
                    }
                }
                else if (tbo.BusinessObject.Status.IsNew)
                {
                    BusinessObjectDTO dto = new BusinessObjectDTO(tbo.BusinessObject);
                    _objectContainer.Store(dto);
                }
                else
                {
                    IList<BusinessObjectDTO> matchingObjects = _objectContainer.Query<BusinessObjectDTO>
                        (obj => obj.ClassDefName == tbo.BusinessObject.ClassDef.ClassName && obj.ID == primaryKey);
                    if (matchingObjects.Count == 0)
                    {
                        throw new BusObjDeleteConcurrencyControlException
                            (string.Format
                                 ("The object of type {0} with ID: {1} has been deleted from the data store.",
                                  tbo.BusinessObject.ClassDef.ClassName, primaryKey));
                    }
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
            _objectContainer.Commit();
            return true;
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