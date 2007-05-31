using System;
using System.Security.Principal;
using Chillisoft.Db.v2;

namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// Logs transactions in the same database that is used to store the
    /// business objects
    /// </summary>
    public class TransactionLogTable : ITransactionLog
    {
        private string mTransactionLogTable;
        private string mDateTimeUpdatedFieldName;
        private string mWindowsUserFieldName;
        private string mLogonUserFieldName;
        private string mMachineUpdateName;
        private string mBusinessObjectTypeNameFieldName;
        private string mCrudActionFieldName;
        private string mDirtyXmlFieldName;

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
            this.mTransactionLogTable = transactionLogTable;
            this.mDateTimeUpdatedFieldName = dateTimeUpdatedFieldName;
            this.mWindowsUserFieldName = windowsUserFieldName;
            this.mLogonUserFieldName = logonUserFieldName;
            this.mMachineUpdateName = machineUpdateName;
            this.mBusinessObjectTypeNameFieldName = businessObjectTypeNameFieldName;
            this.mCrudActionFieldName = crudActionFieldName;
            this.mDirtyXmlFieldName = dirtyXMLFieldName;
        }

        /// <summary>
        /// Record a transaction log for the specified business object
        /// </summary>
        /// <param name="busObj">The business object</param>
        /// <param name="logonUserName">The name of the user who carried 
        /// out the changes</param>
        public void RecordTransactionLog(BusinessObjectBase busObj, string logonUserName)
        {
            //TODO: Peter - make this proper parametrized SQL
            SqlStatement tranSQL = new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection());
            string sql = "INSERT INTO " + this.mTransactionLogTable + " (" +
                         this.mDateTimeUpdatedFieldName + ", " +
                         this.mLogonUserFieldName + ", " +
                         this.mWindowsUserFieldName + ", " +
                         this.mMachineUpdateName + ", " +
                         this.mBusinessObjectTypeNameFieldName + ", " +
                         this.mCrudActionFieldName + ", " +
                         this.mDirtyXmlFieldName + ") VALUES ( '" +
                         DatabaseUtil.FormatDatabaseDateTime(DateTime.Now) + "', '" +
                         logonUserName + "', '" +
                         WindowsIdentity.GetCurrent().Name + "', '" +
                         Environment.MachineName + "', '" +
                         busObj.ClassName + "', '" +
                         GetCrudAction(busObj) + "', '" +
                         busObj.DirtyXML + "' )";
            tranSQL.Statement.Append(sql);
            DatabaseConnection.CurrentConnection.ExecuteSql(new SqlStatementCollection(tranSQL));
        }

        /// <summary>
        /// Returns the status of the business object specified, such as
        /// "Created", "Deleted" or "Updated" (if dirty)
        /// </summary>
        /// <param name="busObj">The business object in question</param>
        /// <returns>Returns a string</returns>
        private string GetCrudAction(BusinessObjectBase busObj)
        {
            if (busObj.IsNew)
            {
                return "Created";
            }
            else if (busObj.IsDeleted)
            {
                return "Deleted";
            }
            else if (busObj.IsDirty)
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