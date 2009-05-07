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
        public virtual ISqlStatementCollection GetPersistSql(IDatabaseConnection databaseConnection)
        {
            if (IsNewAndDeleted()) return null;

            SqlStatementCollection sqlStatementCollection;
            if (IsNew())
            {
                sqlStatementCollection = GetInsertSql(databaseConnection);
            } else if (IsDeleted)
            {
                sqlStatementCollection = GetDeleteSql(databaseConnection);
            }
            else
            {
                sqlStatementCollection = GetUpdateSql(databaseConnection);
            }
            IBOStatus boStatus = BusinessObject.Status;
            ITransactionalDB transactionLog = BusinessObject.TransactionLog as ITransactionalDB;
            if (transactionLog != null && (boStatus.IsNew || boStatus.IsDeleted || boStatus.IsDirty))
            {
                sqlStatementCollection.Add(transactionLog.GetPersistSql(databaseConnection));
            }
            return sqlStatementCollection;
        }

        /// <summary>
        /// Returns an "insert" sql statement list for inserting this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        private SqlStatementCollection GetInsertSql(IDatabaseConnection databaseConnection)
        {
            InsertStatementGenerator gen = new InsertStatementGenerator(BusinessObject, databaseConnection);
            return gen.Generate();
        }

        /// <summary>
        /// Builds a "delete" sql statement list for this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        private SqlStatementCollection GetDeleteSql(IDatabaseConnection databaseConnection)
        {
            DeleteStatementGenerator generator = new DeleteStatementGenerator(BusinessObject, databaseConnection);
            return generator.Generate();
        }

        /// <summary>
        /// Returns an "update" sql statement list for updating this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        private SqlStatementCollection GetUpdateSql(IDatabaseConnection databaseConnection)
        {
            UpdateStatementGenerator gen = new UpdateStatementGenerator(BusinessObject, databaseConnection);
            return gen.Generate();
        }

    }
}
