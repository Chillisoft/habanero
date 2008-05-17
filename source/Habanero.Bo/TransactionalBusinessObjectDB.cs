using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Principal;
using Habanero.Base;
using Habanero.BO.SqlGeneration;
using Habanero.DB;
using Habanero.Util;
using log4net;

namespace Habanero.BO
{
    ///<summary>
    /// Utility class that wraps the business object and implements a database persistance strategy for the business object.
    /// This class is used with allong with the Transaction Committer to implement transactional support
    /// for multiple business objects.
    ///</summary>
    public class TransactionalBusinessObjectDB
        : TransactionalBusinessObject, ITransactionalDB
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.TransactionalBusinessObjectDB");
        ///<summary>
        ///</summary>
        ///<param name="businessObject"></param>
        protected internal TransactionalBusinessObjectDB(BusinessObject businessObject) : base(businessObject)
        {
        }
        ///<summary>
        /// Returns the appropriate sql statement collection depending on the state of the object.
        /// E.g. Update SQL, InsertSQL or DeleteSQL.
        ///</summary>
        public virtual ISqlStatementCollection GetPersistSql()
        {
            if (IsNewAndDeleted()) return null;

            if (IsNew())
            {
                return GetInsertSql();
            }
            else if(IsDeleted)
            {
                return GetDeleteSql();
            }
            else
            {
                return GetUpdateSql();
            }
        }

        /// <summary>
        /// Returns an "insert" sql statement list for inserting this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        private SqlStatementCollection GetInsertSql()
        {
            InsertStatementGenerator gen = new InsertStatementGenerator(BusinessObject, BusinessObject.GetDatabaseConnection());
            return gen.Generate();
        }
        /// <summary>
        /// Builds a "delete" sql statement list for this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        private SqlStatementCollection GetDeleteSql()
        {
            DeleteStatementGenerator generator = new DeleteStatementGenerator(BusinessObject, BusinessObject.GetDatabaseConnection());
            return generator.Generate();
        }
        /// <summary>
        /// Returns an "update" sql statement list for updating this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        private SqlStatementCollection GetUpdateSql()
        {
            UpdateStatementGenerator gen = new UpdateStatementGenerator(BusinessObject, BusinessObject.GetDatabaseConnection());
            return gen.Generate();
        }

        private bool HasDuplicateObjectInDatabase(BOKey boKey, ISqlStatement checkDuplicateSql, out string errMsg)
        {
            errMsg = "";
            using (IDataReader dr = DatabaseConnection.CurrentConnection.LoadDataReader(checkDuplicateSql))
            {
                if (dr.Read()) //Database object with these criteria already exists
                {
                      log.Error(String.Format("Duplicate record error occurred on primary key for: " +
                                                                       "Class: {0}, Username: {1}, Machinename: {2}, " +
                                                                       "Time: {3}, Sql: {4}, Object: {5}",
                                                                       this.BusinessObject.ClassName, WindowsIdentity.GetCurrent().Name, Environment.MachineName,
                                                                       DateTime.Now, boKey, this));
                           

                    string classDisplayName = this.BusinessObject.ClassDef.DisplayName;

                    errMsg = GetDuplicateObjectErrMsg(boKey, classDisplayName);
                    return true;
                }
            }
            return false;
        }

        ///<summary>
        /// returns true if there is already an object in the database with the same primary identifier (primary key)
        ///  or with the same alternate identifier (alternate key)
        ///</summary>
        ///<param name="errMsg"></param>
        ///<returns></returns>
        protected internal override bool HasDuplicateIdentifier(out string errMsg)
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
                SelectStatementGenerator generator =
                    new SelectStatementGenerator(this.BusinessObject, this.BusinessObject.GetDatabaseConnection());
                checkDuplicateSql.Statement.Append(generator.GenerateDuplicateSelect());

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

        private static string GetCheckForDuplicateWhereClause(BOKey lBOKey, SqlStatement sql)
        {
            if (lBOKey == null)
            {
                throw new InvalidKeyException("An error occurred because a " +
                                              "BOKey argument was null.");
            }
            return lBOKey.DatabaseWhereClause(sql);
        }
    }
}
