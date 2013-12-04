#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

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
        public static IClassDef LoadClassDef()
        {
            if (ClassDef.ClassDefs.Contains(typeof(TransactionLogBusObj)))
            {
                return ClassDef.ClassDefs[typeof(TransactionLogBusObj)];
            }
            XmlClassLoader xmlClassLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef classDef =
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

        ///<summary>
        /// Create read update or delete action. I.e. the action that was carried out for this business object.
        ///</summary>
        public string CrudAction
        {
            get { return GetPropertyValueString("CrudAction"); }
            set { SetPropertyValue("CrudAction", value); }
        }

        ///<summary>
        /// The dirty XML for the business object that this log record is for.
        ///</summary>
        public string DirtyXMLLog
        {
            get { return GetPropertyValueString("DirtyXMLLog"); }
            set { SetPropertyValue("DirtyXMLLog", value); }
        }

        ///<summary>
        /// The business object type this business object is.
        ///</summary>
        public string BusinessObjectTypeName
        {
            get { return GetPropertyValueString("BusinessObjectTypeName"); }
            set { SetPropertyValue("BusinessObjectTypeName", value); }
        }

        ///<summary>
        /// The logged in windows user that this log is for.
        ///</summary>
        public string WindowsUser
        {
            get { return GetPropertyValueString("WindowsUser"); }
            set { SetPropertyValue("WindowsUser", value); }
        }

        ///<summary>
        /// The logon user in cases where non windows authentication is used.
        ///</summary>
        public string LogonUser
        {
            get { return GetPropertyValueString("LogonUser"); }
            set { SetPropertyValue("LogonUser", value); }
        }

        ///<summary>
        /// The machine (actual PC name) that was used to make the update.
        ///</summary>
        public string MachineUpdatedName
        {
            get { return GetPropertyValueString("MachineUpdatedName"); }
            set { SetPropertyValue("MachineUpdatedName", value); }
        }

        ///<summary>
        /// The datetime that the update to the business object was made.
        ///</summary>
        public DateTime? DateTimeUpdated
        {
            get { return (DateTime?) GetPropertyValue("DateTimeUpdated"); }
            set { SetPropertyValue("DateTimeUpdated", value); }
        }

        ///<summary>
        /// The ToString value for the business object (usually set to be the alternate key).
        ///</summary>
        public string BusinessObjectToString
        {
            get { return GetPropertyValueString("BusinessObjectToString"); }
            set { SetPropertyValue("BusinessObjectToString", value); }
        }
    }

    /// <summary>
    /// A Transaction logger factory that creates <see cref="NullTransactionLogger"/> objects which don't log anything.
    /// </summary>
    public class TransactionLoggerFactoryNull : ITransactionLoggerFactory
    {
        #region Implementation of ITransactionLoggerFactory

        /// <summary>
        /// Creates a <see cref="NullTransactionLogger"/>
        /// </summary>
        /// <param name="bo">ignored</param>
        /// <param name="tableName">ignored</param>
        /// <returns></returns>
        public ITransactionLog GetLogger(BusinessObject bo, string tableName)
        {
            return new NullTransactionLogger();
        }

        /// <summary>
        /// Creates a <see cref="NullTransactionLogger"/>
        /// </summary>
        /// <param name="bo">ignored</param>
        /// <returns></returns>
        public ITransactionLog GetLogger(BusinessObject bo)
        {
            return new NullTransactionLogger();
        }

        #endregion
    }

    /// <summary>
    /// A transaction logger that does nothing (ie it doesn't log anything)
    /// </summary>
    public class NullTransactionLogger : ITransactionLog
    {
        #region Implementation of ITransactional

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
            return "NullTransactionLoggerID" + Guid.NewGuid();
        }

        ///<summary>
        /// Does nothing
        ///</summary>
        public void UpdateStateAsCommitted()
        {
            //Do nothing
        }

        ///<summary>
        /// Does nothing
        ///</summary>
        public void UpdateAsRolledBack()
        {
            //Do nothing.
        }

        ///<summary>
        /// Returns an empty list
        ///</summary>
        public IEnumerable<ISqlStatement> GetPersistSql()
        {
            return new List<ISqlStatement>();
        }

        #endregion
    }


    /// <summary>
    /// Defines methods that create <see cref="ITransactionLog"/> objects, which are log entries for a specific BO.
    /// </summary>
    public interface ITransactionLoggerFactory
    {
        /// <summary>
        /// Creates a <see cref="ITransactionLog"/> with the provided table name. 
        /// Note that for a db transaction log this will refer to the actual table name. For a 
        /// file based transaction log this might refer to the file name.
        /// </summary>
        /// <param name="bo">The bo to log</param>
        /// <param name="tableName">The table to log to</param>
        /// <returns></returns>
        ITransactionLog GetLogger(BusinessObject bo, string tableName);

        /// <summary>
        /// Creates a <see cref="ITransactionLog"/> that will log the provided BO.
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        ITransactionLog GetLogger(BusinessObject bo);
    }
}