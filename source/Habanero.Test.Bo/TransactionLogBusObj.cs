using System;
using Habanero.BO;
using Habanero.DB;

namespace Habanero.Test.BO
{
    internal class TransactionLogBusObj : BusinessObject
    {

        public string CrudAction
        {
            get { return GetPropertyValueString("CrudAction"); }
            set { SetPropertyValue("CrudAction", value); }
        }

        public string DirtyXMLLog
        {
            get { return GetPropertyValueString("DirtyXMLLog"); }
            set { SetPropertyValue("DirtyXMLLog", value); }
        }

        public string BusinessObjectTypeName
        {
            get { return GetPropertyValueString("BusinessObjectTypeName"); }
            set { SetPropertyValue("BusinessObjectTypeName", value); }
        }

        public string WindowsUser
        {
            get { return GetPropertyValueString("WindowsUser"); }
            set { SetPropertyValue("WindowsUser", value); }
        }

        public string MachineUpdatedName
        {
            get { return GetPropertyValueString("MachineUpdatedName"); }
            set { SetPropertyValue("MachineUpdatedName", value); }
        }

        public DateTime? DateTimeUpdated
        {
            get { return (DateTime?) GetPropertyValue("DateTimeUpdated"); }
            set { SetPropertyValue("DateTimeUpdated", value); }
        }

        public string BusinessObjectToString
        {
            get { return GetPropertyValueString("BusinessObjectToString"); }
            set { SetPropertyValue("BusinessObjectToString", value); }
        }

        public static void DeleteAllTransactionLogsFromDatabase()
        {
            string sql = "DELETE FROM transactionlog";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
        }
    }
}