//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using System.Security.Principal;
using Habanero.DB;

namespace Habanero.BO
{
    /// <summary>
    /// Logs transactions in the same database that is used to store the
    /// business objects. Used to log every change to the business object.
    /// Stores datetime action carried out actionstype (CRUD). windows user, logged user. 
    /// Database Field name and the dirty XML field (shows the previously persisted state
    ///    and newly persisted state of the field.
    /// </summary>
    public class TransactionLogTable : ITransactionLog
    {
        private readonly string _transactionLogTable;
        private readonly string _dateTimeUpdatedFieldName;
        private readonly string _windowsUserFieldName;
        private readonly string _logonUserFieldName;
        private readonly string _machineUpdateName;
        private readonly string _businessObjectTypeNameFieldName;
        private readonly string _crudActionFieldName;
        private readonly string _dirtyXmlFieldName;

        /// <summary>
        /// Constructor to initialise a new log table
        /// </summary>
        /// <param name="transactionLogTable">The log table name</param>
        /// <param name="dateTimeUpdatedFieldName">Time updated field name</param>
        /// <param name="windowsUserFieldName">Windows user field name</param>
        /// <param name="logonUserFieldName">Logon user field name</param>
        /// <param name="machineUpdateName">Machine update name</param>
        /// <param name="businessObjectTypeNameFieldName">BO type field name</param>
        /// <param name="crudActionFieldName">Crud action field name</param>
        /// <param name="dirtyXMLFieldName">Dirty xml field name</param>
        public TransactionLogTable(string transactionLogTable, string dateTimeUpdatedFieldName,
                                   string windowsUserFieldName, string logonUserFieldName, string machineUpdateName,
                                   string businessObjectTypeNameFieldName, string crudActionFieldName,
                                   string dirtyXMLFieldName)
        {
            this._transactionLogTable = transactionLogTable;
            this._dateTimeUpdatedFieldName = dateTimeUpdatedFieldName;
            this._windowsUserFieldName = windowsUserFieldName;
            this._logonUserFieldName = logonUserFieldName;
            this._machineUpdateName = machineUpdateName;
            this._businessObjectTypeNameFieldName = businessObjectTypeNameFieldName;
            this._crudActionFieldName = crudActionFieldName;
            this._dirtyXmlFieldName = dirtyXMLFieldName;
        }

        /// <summary>
        /// Record a transaction log for the specified business object
        /// </summary>
        /// <param name="busObj">The business object</param>
        /// <param name="logonUserName">The name of the user who carried 
        /// out the changes</param>
        public void RecordTransactionLog(BusinessObject busObj, string logonUserName)
        {
            //TODO: Peter - make this proper parametrized Sql
            SqlStatement tranSql = new SqlStatement(DatabaseConnection.CurrentConnection);
            string sql = "INSERT INTO " + this._transactionLogTable + " (" +
                         this._dateTimeUpdatedFieldName + ", " +
                         this._logonUserFieldName + ", " +
                         this._windowsUserFieldName + ", " +
                         this._machineUpdateName + ", " +
                         this._businessObjectTypeNameFieldName + ", " +
                         this._crudActionFieldName + ", " +
                         this._dirtyXmlFieldName + ") VALUES ( '" +
                         DatabaseUtil.FormatDatabaseDateTime(DateTime.Now) + "', '" +
                         logonUserName + "', '" +
                         WindowsIdentity.GetCurrent().Name + "', '" +
                         Environment.MachineName + "', '" +
                         busObj.ClassName + "', '" +
                         GetCrudAction(busObj) + "', '" +
                         busObj.DirtyXML + "' )";
            tranSql.Statement.Append(sql);
            DatabaseConnection.CurrentConnection.ExecuteSql(new SqlStatementCollection(tranSql));
        }

        /// <summary>
        /// Returns the status of the business object specified, such as
        /// "Created", "Deleted" or "Updated" (if dirty)
        /// </summary>
        /// <param name="busObj">The business object in question</param>
        /// <returns>Returns a string</returns>
        private static string GetCrudAction(BusinessObject busObj)
        {
            if (busObj.State.IsNew)
            {
                return "Created";
            }
            else if (busObj.State.IsDeleted)
            {
                return "Deleted";
            }
            else if (busObj.State.IsDirty)
            {
                return "Updated";
            }
            else
            {
                return "Unknown";
            }
        }
    }
}