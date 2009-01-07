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
        protected internal TransactionalBusinessObjectDB(IBusinessObject businessObject) : base(businessObject)
        {
        }
        ///<summary>
        /// Returns the appropriate sql statement collection depending on the state of the object.
        /// E.g. Update SQL, InsertSQL or DeleteSQL.
        ///</summary>
        public virtual ISqlStatementCollection GetPersistSql()
        {
            if (IsNewAndDeleted()) return null;

            SqlStatementCollection sqlStatementCollection;
            if (IsNew())
            {
                sqlStatementCollection = GetInsertSql();
            } else if (IsDeleted)
            {
                sqlStatementCollection = GetDeleteSql();
            }
            else
            {
                sqlStatementCollection = GetUpdateSql();
            }
            IBOStatus boStatus = BusinessObject.Status;
            ITransactionalDB transactionLog = BusinessObject.TransactionLog as ITransactionalDB;
            if (transactionLog != null && (boStatus.IsNew || boStatus.IsDeleted || boStatus.IsDirty))
            {
                sqlStatementCollection.Add(transactionLog.GetPersistSql());
            }
            return sqlStatementCollection;
        }

        /// <summary>
        /// Returns an "insert" sql statement list for inserting this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        private SqlStatementCollection GetInsertSql()
        {
            InsertStatementGenerator gen = new InsertStatementGenerator(BusinessObject, DatabaseConnection.CurrentConnection);
            return gen.Generate();
        }
        /// <summary>
        /// Builds a "delete" sql statement list for this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        private SqlStatementCollection GetDeleteSql()
        {
            DeleteStatementGenerator generator = new DeleteStatementGenerator(BusinessObject, DatabaseConnection.CurrentConnection);
            return generator.Generate();
        }
        /// <summary>
        /// Returns an "update" sql statement list for updating this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        private SqlStatementCollection GetUpdateSql()
        {
            UpdateStatementGenerator gen = new UpdateStatementGenerator(BusinessObject, DatabaseConnection.CurrentConnection);
            return gen.Generate();
        }

        //private bool HasDuplicateObjectInDatabase(BOKey boKey, ISqlStatement checkDuplicateSql, out string errMsg)
        //{
        //    errMsg = "";
        //    IDatabaseConnection databaseConnection = DatabaseConnection.CurrentConnection;
        //    using (IDataReader dr = databaseConnection.LoadDataReader(checkDuplicateSql))
        //    {
        //        if (dr.Read()) //Database object with these criteria already exists
        //        {
        //              log.Error(String.Format("Duplicate record error occurred on primary key for: " +
        //                                                               "Class: {0}, Username: {1}, Machinename: {2}, " +
        //                                                               "Time: {3}, Sql: {4}, Object: {5}",
        //                                                               this.BusinessObject.ClassName, WindowsIdentity.GetCurrent().Name, Environment.MachineName,
        //                                                               DateTime.Now, boKey, this));
                           

        //            string classDisplayName = this.BusinessObject.ClassDef.DisplayName;

       //            errMsg = GetDuplicateObjectErrMsg(boKey, classDisplayName);
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        ///<summary>
        /// returns true if there is already an object in the database with the same primary identifier (primary key)
        ///  or with the same alternate identifier (alternate key)
        ///</summary>
        protected internal override bool HasDuplicateIdentifier(out string errMsg)
        {
            errMsg = "";
            //if (this.BusinessObject == null) return false;
            if (this.BusinessObject.GetBOKeyCol() == null) return false;
            if (this.BusinessObject.Status.IsDeleted) return false;

            List<IBOKey> allKeys = new List<IBOKey>();
            if (this.BusinessObject.ID != null) allKeys.Add(this.BusinessObject.ID);
            foreach (BOKey key in this.BusinessObject.GetBOKeyCol())
            {
                allKeys.Add(key);
            }
            //Criteria keyCriteria = null;
            Criteria primaryKeyCriteria = null;
            foreach (BOKey boKey in allKeys)
            {
                if (boKey is BOPrimaryKey)
                {
                    primaryKeyCriteria = Criteria.FromPrimaryKey(boKey as IPrimaryKey);

                }
                if (!boKey.IsDirtyOrNew()) continue;

                if (boKey is BOPrimaryKey)
                {
                    
                    if (this.BusinessObject.ClassDef.HasObjectID) continue;
                    if (this.BusinessObject.Status.IsNew && boKey.HasAutoIncrementingProperty) continue;
                    if (BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(this.BusinessObject.ClassDef, primaryKeyCriteria).
                       Count > 0)
                    {
                        errMsg += GetDuplicateObjectErrMsg(boKey, this.BusinessObject.ClassDef.DisplayName) + Environment.NewLine;
                    }
                    continue;
                }

                //if (allKeyCriteria == null)
                //{
                //    allKeyCriteria = GetKeyCriteria(boKey);
                //    if (primaryKeyCriteria != null) 
                //        allKeyCriteria = new Criteria(allKeyCriteria, Criteria.LogicalOp.And, new Criteria(Criteria.LogicalOp.Not, primaryKeyCriteria));
                //    continue;
                //}

                Criteria keyCriteria = GetKeyCriteria(boKey);
                    if (primaryKeyCriteria != null) 
                        keyCriteria = new Criteria(keyCriteria, Criteria.LogicalOp.And, new Criteria(Criteria.LogicalOp.Not, primaryKeyCriteria));

                    if (BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection(this.BusinessObject.ClassDef, keyCriteria).
                       Count > 0)
                        errMsg += GetDuplicateObjectErrMsg(boKey, this.BusinessObject.ClassDef.DisplayName) + Environment.NewLine;


              

                //Criteria keyCriteria = GetKeyCriteria(boKey);

                //Criteria.FromPrimaryKey(boKey)

                //IDatabaseConnection databaseConnection = DatabaseConnection.CurrentConnection;
                //SqlStatement checkDuplicateSql = new SqlStatement(databaseConnection);
                //SelectStatementGenerator generator =
                //    new SelectStatementGenerator(this.BusinessObject, databaseConnection);
                //checkDuplicateSql.Statement.Append(generator.GenerateDuplicateSelect());

                //// Special case where super class and subclass have same ID name causes ambiguous field name
                //string idWhereClause = this.BusinessObject.WhereClause(checkDuplicateSql);
                //string id = StringUtilities.GetLeftSection(idWhereClause, " ");
                //if (StringUtilities.CountOccurrences(checkDuplicateSql.ToString(), id) >= 3)
                //{
                //    idWhereClause = idWhereClause.Insert(idWhereClause.IndexOf(id),
                //                                         this.BusinessObject.ClassDef.TableName + ".");
                //}

                //string whereClause = "";
                //if (!(boKey is BOPrimaryKey))
                //{
                //    whereClause += " ( NOT (" + idWhereClause + ")) AND ";
                //}
                //whereClause += GetCheckForDuplicateWhereClause(boKey, checkDuplicateSql);
                //checkDuplicateSql.AppendCriteria(whereClause);

                //if (HasDuplicateObjectInDatabase(boKey, checkDuplicateSql, out errMsg)) return true;
            }

            return !String.IsNullOrEmpty(errMsg);
        }

        private Criteria GetKeyCriteria(BOKey boKey)
        {
            if (boKey.Count == 1)
            {
                return new Criteria(boKey[0].PropertyName, Criteria.ComparisonOp.Equals, boKey[0].Value);
            }
            Criteria lastCriteria = null;
            foreach (IBOProp prop in boKey)
            {
                Criteria propCriteria = new Criteria(prop.PropertyName, Criteria.ComparisonOp.Equals, prop.Value);
                lastCriteria = lastCriteria == null
                                   ? propCriteria
                                   : new Criteria(lastCriteria, Criteria.LogicalOp.And, propCriteria);
            }
            return lastCriteria;
        }

        //private static string GetCheckForDuplicateWhereClause(BOKey lBOKey, SqlStatement sql)
        //{
        //    if (lBOKey == null)
        //    {
        //        throw new InvalidKeyException("An error occurred because a " +
        //                                      "BOKey argument was null.");
        //    }
        //    return lBOKey.DatabaseWhereClause(sql);
        //}
    }
}
