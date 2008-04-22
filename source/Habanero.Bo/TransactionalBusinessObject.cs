using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Principal;
using Habanero.Base;
using Habanero.DB;
using Habanero.Util;

namespace Habanero.BO
{
    ///<summary>
    /// This is a base class which is used as a wrapper (decorator around a business object)
    /// This class along with the TransactionCommiter implement transactional and persistence 
    /// strategies for the business object
    ///</summary>
    public class TransactionalBusinessObject : ITransactionalBusinessObject
    {
        private readonly BusinessObject _businessObject;

        ///<summary>
        ///</summary>
        ///<param name="businessObject"></param>
        protected internal TransactionalBusinessObject(BusinessObject businessObject)
        {
            if (businessObject == null) throw new ArgumentNullException("businessObject");

            _businessObject = businessObject;
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
        public virtual bool IsDeleted
        {
            get { return _businessObject.State.IsDeleted; }
        }

        /// <summary>
        /// Whether the business object's state is new
        /// </summary>
        /// <returns></returns>
        public bool IsNew()
        {
            return _businessObject.State.IsNew;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected internal bool IsNewAndDeleted()
        {
            return _businessObject.State.IsNew && (_businessObject.State.IsDeleted);
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
        public virtual bool IsValid(out string invalidReason)
        {
            return _businessObject.IsValid(out invalidReason);
        }

        /// <summary>
        /// Indicates whether there is a duplicate of this object in the data store
        /// eg. for a database this will select from the table to find an object
        /// that matches this object's primary key. In this case this object would be
        /// a duplicate.
        /// </summary>
        /// <param name="boKey">The key that the duplication is being checked on</param>
        /// <param name="checkDuplicateSql">The SQL statement being executed in checking for duplicates</param>
        /// <param name="errMsg">The description of the duplicate</param>
        /// <returns>Whether a duplicate of this object exists in the data store (based on the ID/primary key)</returns>
        private bool HasDuplicateObjectInDatabase(BOKey boKey, ISqlStatement checkDuplicateSql, out string errMsg)
        {
            errMsg = "";
            using (IDataReader dr = DatabaseConnection.CurrentConnection.LoadDataReader(checkDuplicateSql))
            {
                if (dr.Read()) //Database object with these criteria already exists
                {
                    /*TODO:                               log.Error(String.Format("Duplicate record error occurred on primary key for: " +
                                                                       "Class: {0}, Username: {1}, Machinename: {2}, " +
                                                                       "Time: {3}, Sql: {4}, Object: {5}",
                                                                       this.BusinessObject.ClassName, WindowsIdentity.GetCurrent().Name, Environment.MachineName,
                                                                       DateTime.Now, whereClause, this));
                           */

                    string classDisplayName = this.BusinessObject.ClassDef.DisplayName;

                    errMsg = GetDuplicateObjectErrMsg(boKey, classDisplayName);
                    return true;
                }
            }
            return false;
        }

        private static string GetDuplicateObjectErrMsg(BOKey boKey, string classDisplayName)
        {
            string errMsg;
            string propNames = "";
            foreach (BOProp prop in boKey.GetBOPropCol())
            {
                if (propNames.Length > 0) propNames += ", ";
                propNames += string.Format("{0} = {1}", prop.PropertyName, prop.Value);
            }
            errMsg =
                string.Format("A '{0}' already exists with the same identifier: {1}.",
                              classDisplayName, propNames);
            return errMsg;
        }

        /// <summary>
        /// Indicates whether there is a duplicate of this object in the data store based on
        /// an alternate identifier.
        /// eg. for a database this will select from the table to find an object
        /// that matches this object's alternate key. In this case this object would be
        /// a duplicate.
        /// </summary>
        /// <param name="errMsg">The description of the duplicate</param>
        /// <returns>Whether a duplicate of this object exists in the data store (based on the alternate key)</returns>
        public bool HasDuplicateIdentifier(out string errMsg)
        {
            errMsg = "";
            //if (this.BusinessObject == null) return false;
            if (this.BusinessObject.GetBOKeyCol() == null) return false;
            if (this.BusinessObject.State.IsDeleted) return false;

            List<BOKey> allKeys = new List<BOKey>();
            if (this.BusinessObject.PrimaryKey != null) allKeys.Add(this.BusinessObject.PrimaryKey);
            foreach (BOKey key in this.BusinessObject.GetBOKeyCol())
            {
                allKeys.Add(key);
            }
            foreach (BOKey boKey in allKeys)
            {
                if (!boKey.IsDirtyOrNew()) continue;
                if (boKey is BOPrimaryKey && (this.BusinessObject.ClassDef.HasObjectID)) continue;

                SqlStatement checkDuplicateSql =
                    new SqlStatement(DatabaseConnection.CurrentConnection);
                checkDuplicateSql.Statement.Append(this.BusinessObject.GetSelectSql());

                // Special case where super class and subclass have same ID name causes ambiguous field name
                string idWhereClause = this.BusinessObject.WhereClause(checkDuplicateSql);
                string id = StringUtilities.GetLeftSection(idWhereClause, " ");
                if (StringUtilities.CountOccurrences(checkDuplicateSql.ToString(), id) >= 3)
                {
                    idWhereClause = idWhereClause.Insert(idWhereClause.IndexOf(id),
                                                         this.BusinessObject.ClassDef.TableName + ".");
                }

                string whereClause = "";
                if (!(boKey is BOPrimaryKey))
                {
                    whereClause += " ( NOT (" + idWhereClause + ")) AND ";
                }
                whereClause += GetCheckForDuplicateWhereClause(boKey, checkDuplicateSql);
                checkDuplicateSql.AppendCriteria(whereClause);

                if (HasDuplicateObjectInDatabase(boKey, checkDuplicateSql, out errMsg)) return true;
            }
            return false;
        }

        /// <summary>
        /// Creates a "where" clause used to check for duplicate keys
        /// </summary>
        /// <param name="lBOKey">The business object key</param>
        /// <param name="sql">The sql statement used to generate and track
        /// parameters</param>
        /// <returns>Returns a string</returns>
        private static string GetCheckForDuplicateWhereClause(BOKey lBOKey, SqlStatement sql)
        {
            if (lBOKey == null)
            {
                throw new InvalidKeyException("An error occurred because a " +
                    "BOKey argument was null.");
            }
            return lBOKey.DatabaseWhereClause(sql);
        }

        ///<summary>
        /// Executes any custom code required by the business object before it is persisted to the database.
        /// This has the additionl capability of creating or updating other business objects and adding these
        /// to the transaction committer.
        ///</summary>
        ///<param name="transactionCommitter">the transaction committer that is executing the transaction</param>
        public void UpdateObjectBeforePersisting(TransactionCommitter transactionCommitter)
        {
           // if (this.BusinessObject == null) return;

            this.BusinessObject.UpdateObjectBeforePersisting(transactionCommitter);
        }
    }
}