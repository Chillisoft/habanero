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
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
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
    public class TransactionLogTable : ITransactionLog, ITransactionalDB
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
        private readonly ISecurityController _securityController;
        private readonly string _businessObjectToStringFieldName;

        ///<summary>
        /// Constructs the new transactionlogTable with default table name and logging fields.
        ///</summary>
        ///<param name="busObjToLog"></param>
        public TransactionLogTable(BusinessObject busObjToLog)
            : this(busObjToLog, "transactionLog", "DateTimeUpdated",
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
        /// <param name="businessObjectToStringFieldName"></param>
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
            LoadClassDef();
        }

        /// <summary>
        /// Load ClassDef for accessing the transacionLog table through Business Object.
        /// </summary>
        public static void LoadClassDef()
        {
            XmlClassLoader xmlClassLoader = new XmlClassLoader();
            ClassDef classDef =
                xmlClassLoader.LoadClass(
                    @"
               <class name=""TransactionLogBusObj"" assembly=""Habanero.BO"" table=""transactionlog"">
					<property  name=""TransactionSequenceNo"" type=""Int32"" autoIncrementing=""true"" />
					<property  name=""DateTimeUpdated"" type=""DateTime"" />
					<property  name=""WindowsUser""/>
					<property  name=""LogonUser"" />
					<property  name=""MachineUpdatedName"" databaseField=""MachineName""/>
					<property  name=""BusinessObjectTypeName"" />
                    <property  name=""BusinessObjectToString""/>
					<property  name=""CRUDAction"" />
					<property  name=""DirtyXMLLog"" databaseField=""DirtyXML""/>
					<primaryKey isObjectID=""false"">
						<prop name=""TransactionSequenceNo"" />
					</primaryKey>
			    </class>
			");
            if(!ClassDef.ClassDefs.Contains(classDef))
                ClassDef.ClassDefs.Add(classDef);
            return;
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

        ///<summary>
        /// Returns the appropriate sql statement collection depending on the state of the object.
        /// E.g. Update SQL, InsertSQL or DeleteSQL.
        ///</summary>
        public ISqlStatementCollection GetPersistSql()
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
            tranSql.AddParameterToStatement(WindowsIdentity.GetCurrent().Name);
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

            SqlStatementCollection sqlStatements = new SqlStatementCollection(tranSql);
            return sqlStatements;
        }

        private string GetLogonUserName()
        {
            if (_securityController != null) return _securityController.CurrentUserName;
            if (GlobalRegistry.SecurityController != null) return GlobalRegistry.SecurityController.CurrentUserName;
            return "";
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
}