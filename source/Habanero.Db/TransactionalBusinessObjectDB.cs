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
using Habanero.BO;

namespace Habanero.DB
{
    ///<summary>
    /// Utility class that wraps the business object and implements a database persistance strategy for the business object.
    /// This class is used with allong with the Transaction Committer to implement transactional support
    /// for multiple business objects.
    ///</summary>
    [Serializable]
    public class TransactionalBusinessObjectDB
        : TransactionalBusinessObject, ITransactionalDB
    {
        private readonly IDatabaseConnection _databaseConnection;
        ///<summary>
        ///</summary>
        ///<param name="businessObject"></param>
        public TransactionalBusinessObjectDB(IBusinessObject businessObject, IDatabaseConnection databaseConnection) : base(businessObject)
        {
            _databaseConnection = databaseConnection;
        }

        ///<summary>
        /// Returns the appropriate sql statement collection depending on the state of the object.
        /// E.g. Update SQL, InsertSQL or DeleteSQL.
        ///</summary>
        public virtual IEnumerable<ISqlStatement> GetPersistSql()
        {
            if (IsNewAndDeleted()) return null;

            IEnumerable<ISqlStatement> sqlStatementCollection;
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
            var boStatus = BusinessObject.Status;
            var transactionLog = BusinessObject.TransactionLog;
            var sqlStatements = sqlStatementCollection.ToList();
            if (transactionLog != null && (boStatus.IsNew || boStatus.IsDeleted || boStatus.IsDirty))
            {
                transactionLog.GetPersistSql().ForEach(sqlStatements.Add);
            }
            return sqlStatements;
        }

        /// <summary>
        /// Returns an "insert" sql statement list for inserting this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        private IEnumerable<ISqlStatement> GetInsertSql()
        {
            InsertStatementGenerator gen = new InsertStatementGenerator(BusinessObject, _databaseConnection);
            return gen.Generate();
        }

        /// <summary>
        /// Builds a "delete" sql statement list for this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        private IEnumerable<ISqlStatement> GetDeleteSql()
        {
            DeleteStatementGenerator generator = new DeleteStatementGenerator(BusinessObject, _databaseConnection);
            return generator.Generate();
        }

        /// <summary>
        /// Returns an "update" sql statement list for updating this object
        /// </summary>
        /// <returns>Returns a collection of sql statements</returns>
        private IEnumerable<ISqlStatement> GetUpdateSql()
        {
            UpdateStatementGenerator gen = new UpdateStatementGenerator(BusinessObject, _databaseConnection);
            return gen.Generate();
        }

    }
}