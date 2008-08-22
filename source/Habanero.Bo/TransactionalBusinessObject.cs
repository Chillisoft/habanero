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
using Habanero.Base;

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
        protected internal BusinessObject BusinessObject
        {
            get { return _businessObject; }
        }

        /// <summary>
        /// Whether the business object's state is deleted
        /// </summary>
        protected internal virtual bool IsDeleted
        {
            get { return _businessObject.State.IsDeleted; }
        }

        /// <summary>
        /// Whether the business object's state is new
        /// </summary>
        /// <returns></returns>
        protected internal bool IsNew()
        {
            return _businessObject.State.IsNew;
        }

        /// <summary>
        /// returns true if the underlying business object is new and deleted. I.e. it is in 
        /// an invalid state which means that it has already been deleted from the database.
        /// </summary>
        /// <returns></returns>
        protected internal  bool IsNewAndDeleted()
        {
            return _businessObject.State.IsNew && (_businessObject.State.IsDeleted);
        }

        ///<summary>
        ///</summary>
        ///<returns>The ID that uniquelty identifies this item of the transaction. In the case of business objects the object Id.
        /// for non business objects that no natural id exists for the particular transactional item a guid that uniquely identifies 
        /// transactional item should be generated. This is used by the transaction committer to ensure that the transactional item
        /// is not added twice in error.</returns>
        public string TransactionID()
        {
            return this.BusinessObject.ID.GetObjectId();
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
            return _businessObject.IsValid(out invalidReason);
        }

        ///<summary>
        ///</summary>
        ///<param name="boKey"></param>
        ///<param name="classDisplayName"></param>
        ///<returns></returns>
        protected internal static string GetDuplicateObjectErrMsg(BOKey boKey, string classDisplayName)
        {
            string propNames = "";
            foreach (BOProp prop in boKey.GetBOPropCol())
            {
                if (propNames.Length > 0) propNames += ", ";
                propNames += string.Format("{0} = {1}", prop.PropertyName, prop.Value);
            }
            string errMsg = string.Format("A '{0}' already exists with the same identifier: {1}.",
                                          classDisplayName, propNames);
            return errMsg;
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
        /// returns true if there is already an object in the database with the same primary identifier (primary key)
        ///  or with the same alternate identifier (alternate key)
        ///</summary>
        ///<param name="errMsg"></param>
        ///<returns></returns>
        protected internal virtual bool HasDuplicateIdentifier(out string errMsg)
        {
            errMsg = "";
            return false;
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
    }
}