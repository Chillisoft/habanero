// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System.Linq;
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
    ///<summary>
    /// This is a base class which is used as a wrapper (
    ///   GOF Decorator/Wrapper - 'Design Patterns, Elements Of Reusable Object Oriented Software' 
    ///   around a business object). This class is used so that the Business Object can be 
    ///   isolated from any logic regarding updating changes to a datasource.
    /// This class along with the TransactionCommiter implement transactional and persistence 
    /// strategies for the business object.
    /// The TransactionBusinessObject also implements the DataMapper (165) and the MetaData mapping (306) patterns.
    ///   Fowler - Patterns of Enterprise Application Architecture.
    ///  
    /// This class is used by the <see cref="TransactionCommitter"/>. The <see cref="TransactionCommitter"/> 
    ///   class contans all the required explanations of how these classes work together.
    ///</summary>
    [Serializable]
    public class TransactionalBusinessObject : ITransactional
    {
        private readonly BusinessObject _businessObject;

        ///<summary>
        /// Creates a TransactionalBusinessObject that wraps the <see cref="IBusinessObject"/>
        ///</summary>
        ///<param name="businessObject"></param>
        protected internal TransactionalBusinessObject(IBusinessObject businessObject)
        {
            if (businessObject == null) throw new ArgumentNullException("businessObject");

            _businessObject = (BusinessObject) businessObject;
        }

        ///<summary>
        /// Returns the business object that this objects decorates.
        ///</summary>
        public BusinessObject BusinessObject
        {
            get { return _businessObject; }
        }

        /// <summary>
        /// Whether the business object's state is deleted
        /// </summary>
        protected internal virtual bool IsDeleted
        {
            get { return _businessObject.Status.IsDeleted; }
        }

        /// <summary>
        /// Whether the business object's state is new
        /// </summary>
        /// <returns></returns>
        protected internal bool IsNew()
        {
            return _businessObject.Status.IsNew;
        }

        /// <summary>
        /// returns true if the underlying business object is new and deleted. I.e. it is in 
        /// an invalid state which means that it has already been deleted from the database.
        /// </summary>
        /// <returns></returns>
        protected internal  bool IsNewAndDeleted()
        {
            return _businessObject.Status.IsNew && (_businessObject.Status.IsDeleted);
        }

        ///<summary>
        /// This is the ID that uniquely identifies this item of the transaction.
        /// This must be the same for the lifetime of the transaction object. 
        /// This assumption is relied upon for certain optimisations in the Transaction Comitter.
        ///</summary>
        ///<returns>The ID that uniquely identifies this item of the transaction. In the case of business objects the object Id.
        /// for non business objects that no natural id exists for the particular transactional item a guid that uniquely identifies 
        /// transactional item should be generated. This is used by the transaction committer to ensure that the transactional item
        /// is not added twice in error.</returns>
        public string TransactionID()
        {
            //return this.BusinessObject.ID.AsString_CurrentValue();
            return this.BusinessObject.ID.ObjectID.ToString();
        }

        ///<summary>
        /// Updates the business object as committed
        ///</summary>
        public virtual void UpdateStateAsCommitted()
        {
            _businessObject.UpdateStateAsPersisted();
        }

        /// <summary>
        /// Indicates whether all of the property values are valid
        /// </summary>
        /// <param name="invalidReason">A string to modify with a reason
        /// for any invalid values</param>
        /// <returns>Returns true if all are valid</returns>
        protected internal virtual bool IsValid(out string invalidReason)
        {
            return _businessObject.Status.IsValid(out invalidReason);
        }


        ///<summary>
        /// Executes any custom code required by the business object before it is persisted to the database.
        /// This has the additionl capability of creating or updating other business objects and adding these
        /// to the transaction committer.
        ///</summary>
        ///<param name="transactionCommitter">the transaction committer that is executing the transaction</param>
        protected internal virtual void UpdateObjectBeforePersisting(ITransactionCommitter transactionCommitter)
        {
            _businessObject.UpdateObjectBeforePersisting(transactionCommitter);
        }

        ///<summary>
        /// Checks the underlying business object for any concurrency control errors before trying to commit to 
        /// the datasource
        ///</summary>
        protected internal virtual void CheckForConcurrencyErrors()
        {
            _businessObject.CheckConcurrencyBeforePersisting();
        }


        ///<summary>
        /// updates the object as rolled back
        ///</summary>
        public virtual void UpdateAsRolledBack()
        {
            _businessObject.UpdateAsTransactionRolledBack();
        }

        /// <summary>
        /// Checks whether the Wrapped business object can be deleted.
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        protected internal bool CheckCanDelete(out string errMsg)
        {
            return DeleteHelper.CheckCanDelete(this.BusinessObject, out errMsg);
        }
        /// <summary>
        /// Checks whether the Wrapped business object can be Persisted.
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        protected internal bool CanBePersisted(out string errMsg)
        {
            return _businessObject.CanBePersisted(out errMsg);
        }

        ///<summary>
        /// returns true if there is already an object in the database with the same primary identifier (primary key)
        ///  or with the same alternate identifier (alternate key)
        ///</summary>
        ///<param name="transactionalBusinessObject"></param>
        ///<param name="pendingTransactions"></param>
        ///<param name="errorMessages"></param>
        ///<param name="bo"></param>
        ///<returns></returns>
        protected internal virtual void CheckDuplicateIdentifier(List<ITransactional> pendingTransactions, List<string> errorMessages)
        {
            BusinessObject bo = BusinessObject;
            var boKeyCol = bo.GetBOKeyCol();
            if (boKeyCol == null) return;
            if (bo.Status.IsDeleted) return;

            var primaryKey = bo.ID;
            CheckDuplicatePrimaryKey(bo, pendingTransactions, primaryKey, errorMessages);

            var primaryKeyCriteria = Criteria.FromPrimaryKey(primaryKey);
            foreach (BOKey boKey in boKeyCol)
            {
                if (!boKey.IsDirtyOrNew()) continue;

                var keyCriteria = boKey.GetKeyCriteria();
                if (primaryKeyCriteria != null) keyCriteria = GetDuplicateKeyCriteria(keyCriteria, primaryKeyCriteria);

                var duplicateObjects = GetDuplicateObjects(bo, pendingTransactions, keyCriteria);
                if (duplicateObjects.Count > 0)
                {
                    foreach (IBusinessObject businessObject in duplicateObjects)
                    {
                        var thisBusinessObject = businessObject;
                        var transactionForBO = pendingTransactions
                            .FirstOrDefault(transactional => thisBusinessObject.ID.ObjectID.ToString() == transactional.TransactionID())
                            as TransactionalBusinessObject;
                        if (transactionForBO != null)
                        {
                            if (transactionForBO.IsDeleted) continue;
                        }
                        var duplicateObjectErrMsg = GetDuplicateObjectErrMsg(boKey, GetClassDisplayName(bo));
                        if (errorMessages.Contains(duplicateObjectErrMsg)) continue;
                        errorMessages.Add(duplicateObjectErrMsg);
                    }
                }
            }
        }

        private static string GetClassDisplayName(BusinessObject bo)
        {
            return bo.ClassDef.DisplayName;
        }

        private static IBusinessObjectCollection GetDuplicateObjects(BusinessObject bo, List<ITransactional> pendingTransactions, Criteria keyCriteria)
        {
            var classDef = bo.ClassDef;
            var duplicateObjects = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(classDef, keyCriteria);

            var duplicatesFromTransactions = (from transaction in pendingTransactions
                                              where transaction is TransactionalBusinessObject
                                              let businessObject = ((TransactionalBusinessObject) transaction).BusinessObject
                                              where businessObject.ClassDef == classDef
                                              where businessObject!=bo //This is necessary for the PK check
                                              where keyCriteria.IsMatch(businessObject, false)
                                              select businessObject
                                             ).ToList();

            duplicatesFromTransactions.ForEach(duplicateObjects.Add);

            return duplicateObjects;
        }

        private static Criteria GetDuplicateKeyCriteria(Criteria keyCriteria, Criteria primaryKeyCriteria)
        {
            return new Criteria(keyCriteria, Criteria.LogicalOp.And, new Criteria(Criteria.LogicalOp.Not, primaryKeyCriteria));
        }

        private static void CheckDuplicatePrimaryKey(BusinessObject bo, List<ITransactional> pendingTransactions, IPrimaryKey primaryKey, List<string> errorMessages)
        {
            var primaryKeyCriteria = Criteria.FromPrimaryKey(primaryKey);

            if (bo.ClassDef.HasObjectID) return;
            if (bo.Status.IsNew && primaryKey.HasAutoIncrementingProperty) return;
            var boKey = primaryKey as BOKey;
            if (boKey == null) throw new HabaneroApplicationException(String.Format("The Primary key '{0}' is not a BOKEY", primaryKey));
            if (!boKey.IsDirtyOrNew()) return;
            if (GetDuplicateObjects(bo, pendingTransactions, primaryKeyCriteria).Count > 0)
            {
                var duplicateObjectErrMsg = GetDuplicateObjectErrMsg(primaryKey, GetClassDisplayName(bo));
                if (!errorMessages.Contains(duplicateObjectErrMsg))
                {
                    errorMessages.Add(duplicateObjectErrMsg);
                }
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="boKey"></param>
        ///<param name="classDisplayName"></param>
        ///<returns></returns>
        private static string GetDuplicateObjectErrMsg(IBOKey boKey, string classDisplayName)
        {
            string propNames = "";
            foreach (BOProp prop in boKey.GetBOPropCol())
            {
                if (propNames.Length > 0) propNames += ", ";
                propNames += String.Format("{0} = {1}", prop.PropertyName, prop.Value);
            }
            var errMsg = String.Format("A '{0}' already exists with the same identifier: {1}.",
                                       classDisplayName, propNames);
            return errMsg;
        }
    }
}
