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
using System.Security.Principal;
using Habanero.Base;
using Habanero.BO;

namespace Habanero.DB
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
        private readonly BusinessObject _buObjToLog;
        private readonly string _transactionLogTable;
        private readonly string _dateTimeUpdatedFieldName;
        private readonly string _windowsUserFieldName;
        private readonly string _logonUserFieldName;
        private readonly string _machineUpdateName;
        private readonly string _businessObjectTypeNameFieldName;
        private readonly string _crudActionFieldName;
        private readonly string _dirtyXmlFieldName;
        private Guid _id;
        private readonly ISecurityController _securityController;
        private readonly string _businessObjectToStringFieldName;

        /////<summary>
        ///// Constructs the new transactionlogTable with default table name and logging fields.
        /////</summary>
        /////<param name="busObjToLog"></param>
        //public TransactionLogTable(BusinessObject busObjToLog)
        //    : this(busObjToLog, "transactionLog", "DateTimeUpdated",
        //        "WindowsUser", "LogonUser", "MachineName", "BusinessObjectTypeName", "CRUDAction", "DirtyXML")
        //{

        //}

        ///<summary>
        /// Constructs the new transactionlogTable with default table name and logging fields.
        ///</summary>
        ///<param name="busObjToLog"></param>
        public TransactionLogTable(BusinessObject busObjToLog)
            : this(busObjToLog, "transactionLog")
        {

        }

        ///<summary>
        /// Constructs the new transactionlogTable with specified table name and the defaultlogging fields.
        ///</summary>
        ///<param name="busObjToLog">the business object for which the transaction log is being created</param>
        ///<param name="transactionLogTable">The log table name</param>
        public TransactionLogTable(BusinessObject busObjToLog, string transactionLogTable)
            : this(busObjToLog, transactionLogTable, "DateTimeUpdated",
                   "WindowsUser", "LogonUser","BusinessObjectToString", "MachineName", "BusinessObjectTypeName", "CRUDAction", "DirtyXML")
        {

        }

        /// <summary>
        /// Constructs the new transactionlogTable with default table name and logging fields and a specific security controller for getting the currently logged on user.
        /// </summary>
        /// <param name="busObjToLog"></param>
        /// <param name="securityController"></param>
        public TransactionLogTable(BusinessObject busObjToLog,ISecurityController securityController)
            : this(busObjToLog, "transactionLog", "DateTimeUpdated",
                   "WindowsUser", "LogonUser", "BusinessObjectToString", "MachineName", "BusinessObjectTypeName", "CRUDAction", "DirtyXML")
        {
            _securityController = securityController;
        }

        /// <summary>
        /// Constructor to initialise a new log table
        /// </summary>
        /// <param name="buObjToLog">the business object for which the transaction log is being created</param>
        /// <param name="transactionLogTable">The log table name</param>
        /// <param name="dateTimeUpdatedFieldName">Time updated field name</param>
        /// <param name="windowsUserFieldName">Windows user field name</param>
        /// <param name="logonUserFieldName">Logon user field name</param>
        /// <param name="businessObjectToStringFieldName">BusinessObject ToString field name</param>
        /// <param name="machineUpdateName">Machine update name</param>
        /// <param name="businessObjectTypeNameFieldName">BO type field name</param>
        /// <param name="crudActionFieldName">Crud action field name</param>
        /// <param name="dirtyXMLFieldName">Dirty xml field name</param>
        public TransactionLogTable(BusinessObject buObjToLog, string transactionLogTable, string dateTimeUpdatedFieldName,
                                   string windowsUserFieldName, string logonUserFieldName,string businessObjectToStringFieldName, string machineUpdateName,
                                   string businessObjectTypeNameFieldName, string crudActionFieldName,
                                   string dirtyXMLFieldName)
        {
            _buObjToLog = buObjToLog;
            this._transactionLogTable = transactionLogTable;
            this._dateTimeUpdatedFieldName = dateTimeUpdatedFieldName;
            this._windowsUserFieldName = windowsUserFieldName;
            this._logonUserFieldName = logonUserFieldName;
            this._businessObjectToStringFieldName = businessObjectToStringFieldName;
            this._machineUpdateName = machineUpdateName;
            this._businessObjectTypeNameFieldName = businessObjectTypeNameFieldName;
            this._crudActionFieldName = crudActionFieldName;
            this._dirtyXmlFieldName = dirtyXMLFieldName;
            this._id = Guid.NewGuid();
        }

        /// <summary>
        /// Constructor to initialise a new log table
        /// </summary>
        /// <param name="buObjToLog">the business object for which the transaction log is being created</param>
        /// <param name="transactionLogTable">The log table name</param>
        /// <param name="dateTimeUpdatedFieldName">Time updated field name</param>
        /// <param name="windowsUserFieldName">Windows user field name</param>
        /// <param name="logonUserFieldName">Logon user field name</param>
        /// <param name="businessObjectToStringFieldName"></param>
        /// <param name="machineUpdateName">Machine update name</param>
        /// <param name="businessObjectTypeNameFieldName">BO type field name</param>
        /// <param name="crudActionFieldName">Crud action field name</param>
        /// <param name="dirtyXMLFieldName">Dirty xml field name</param>
        /// <param name="securityController"></param>
        public TransactionLogTable(BusinessObject buObjToLog, string transactionLogTable, string dateTimeUpdatedFieldName,
                                   string windowsUserFieldName, string logonUserFieldName,string businessObjectToStringFieldName, string machineUpdateName,
                                   string businessObjectTypeNameFieldName, string crudActionFieldName,
                                   string dirtyXMLFieldName,ISecurityController securityController):this(buObjToLog, transactionLogTable, dateTimeUpdatedFieldName,
                                                                                                         windowsUserFieldName, logonUserFieldName,businessObjectToStringFieldName, machineUpdateName,
                                                                                                         businessObjectTypeNameFieldName, crudActionFieldName,
                                                                                                         dirtyXMLFieldName)
        {
            _securityController = securityController;
        }

        /// <summary>
        /// Returns the status of the business object specified, such as
        /// "Created", "Deleted" or "Updated" (if dirty)
        /// </summary>
        /// <param name="busObj">The business object in question</param>
        /// <returns>Returns a string</returns>
        private static string GetCrudAction(IBusinessObject busObj)
        {
            if (busObj.Status.IsNew)
            {
                return "Created";
            }
            if (busObj.Status.IsDeleted)
            {
                return "Deleted";
            }
            return busObj.Status.IsDirty ? "Updated" : "Unknown";
        }

        ///<summary>
        /// Returns the appropriate sql statement collection depending on the state of the object.
        /// E.g. Update SQL, InsertSQL or DeleteSQL.
        ///</summary>
        public IEnumerable<ISqlStatement> GetPersistSql()
        {
            SqlStatement tranSql = new SqlStatement(DatabaseConnection.CurrentConnection);
            string sql = "INSERT INTO " + this._transactionLogTable + " (" +
                         this._dateTimeUpdatedFieldName + ", " +
                         this._logonUserFieldName + ", " +
                         this._windowsUserFieldName + ", " +
                         this._machineUpdateName + ", " +
                         this._businessObjectTypeNameFieldName + ", " +
                         this._crudActionFieldName + ", " +
                         this._businessObjectToStringFieldName + ", " +
                         this._dirtyXmlFieldName + ") VALUES ( ";

            tranSql.Statement.Append(sql);
            tranSql.AddParameterToStatement(DateTime.Now);
            tranSql.Statement.Append(", ");
            tranSql.AddParameterToStatement(GetLogonUserName());
            tranSql.Statement.Append(", ");
            WindowsIdentity currentWindowsUser = WindowsIdentity.GetCurrent();
            if (currentWindowsUser != null) tranSql.AddParameterToStatement(currentWindowsUser.Name);
            tranSql.Statement.Append(", ");
            tranSql.AddParameterToStatement(Environment.MachineName);
            tranSql.Statement.Append(", ");
            tranSql.AddParameterToStatement(_buObjToLog.ClassDef.ClassName);
            tranSql.Statement.Append(", ");
            tranSql.AddParameterToStatement(GetCrudAction(_buObjToLog));
            tranSql.Statement.Append(", ");
            tranSql.AddParameterToStatement(_buObjToLog.ToString());
            tranSql.Statement.Append(", ");
            tranSql.AddParameterToStatement(_buObjToLog.DirtyXML);
            tranSql.Statement.Append(")");

            return new [] { tranSql };
        }

        private string GetLogonUserName()
        {
            if (_securityController != null) return _securityController.CurrentUserName;
            if (GlobalRegistry.SecurityController != null) return GlobalRegistry.SecurityController.CurrentUserName;
            return "";
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
            return this._id.ToString("B");
        }

        ///<summary>
        /// Updates the business object as committed
        ///</summary>
        public void UpdateStateAsCommitted()
        {
        }


        ///<summary>
        /// updates the object as rolled back
        ///</summary>
        public void UpdateAsRolledBack()
        {
        }
    }




    public class TransactionLoggerFactory : ITransactionLoggerFactory
    {
        public ITransactionLog GetLogger(BusinessObject bo, string tableName)
        {
            return new TransactionLogTable(bo, tableName);
        }

        public ITransactionLog GetLogger(BusinessObject bo)
        {
            return new TransactionLogTable(bo);
        }
    }
}