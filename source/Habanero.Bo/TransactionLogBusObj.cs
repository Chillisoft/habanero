using System;
using Habanero.DB;

namespace Habanero.BO
{
    ///<summary>
    /// Provides a BusinessObject to use the TransactionLog table.
    ///</summary>
    public class TransactionLogBusObj : BusinessObject
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

        public string LogonUser
        {
            get { return GetPropertyValueString("LogonUser"); }
            set { SetPropertyValue("LogonUser", value); }
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
    }
}