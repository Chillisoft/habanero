using System;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB;

namespace Habanero.BO
{
    ///<summary>
    /// Provides a BusinessObject to use the TransactionLog table.
    ///</summary>
    public class TransactionLogBusObj : BusinessObject
    {

        /// <summary>
        /// Load ClassDef for accessing the transacionLog table through Business Object.
        /// </summary>
        public static ClassDef LoadClassDef()
        {
            if (ClassDef.ClassDefs.Contains(typeof(TransactionLogBusObj)))
            {
                return ClassDef.ClassDefs[typeof(TransactionLogBusObj)];
            }
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
            ClassDef.ClassDefs.Add(classDef);
            return classDef;
        }

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