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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Util.File;

namespace Habanero.BO
{
    /// <summary>
    /// Provides a lookup-list sourced from a database using the
    /// sql statement provided.
    /// A lookup-list is typically used to populate features like a ComboBox,
    /// where the string would be displayed, but the Guid would be the
    /// value stored (for reasons of data integrity).
    /// The sql statement will need to load two fields in correct order:
    /// a Guid (such as the object ID or primary key) and a string.
    /// </summary>
    public class DatabaseLookupList : ILookupList
    {
        private string _statement;
        private Type _lookupObjectType;
		private string _assemblyName;
		private string _className;
        private int _timeout;
		private DateTime _lastCallTime;
        private Dictionary<string, object> _lookupList;

		#region Constructors

		/// <summary>
        /// Constructor that specifies the sql statement
        /// </summary>
        /// <param name="statement">The sql statement used to specify which
        /// objects to load for the lookup-list</param>
        public DatabaseLookupList(string statement) : this(statement, 10000)
        {
        }

        /// <summary>
        /// Constructor that specifies the sql statement and the type of 
        /// object represented in the lookup-list
        /// </summary>
        /// <param name="statement">The sql statement used to specify which
        /// objects to load for the lookup-list</param>
        /// <param name="lookupObjectType">The object type</param>
        public DatabaseLookupList(string statement, Type lookupObjectType)
            : this(statement, 10000, lookupObjectType)
        {
        }

		/// <summary>
		/// Constructor that specifies the sql statement and the type of 
		/// object represented in the lookup-list
		/// </summary>
		/// <param name="statement">The sql statement used to specify which
		/// objects to load for the lookup-list</param>
		/// <param name="assemblyName">The class type assembly name.</param>
		/// <param name="className">The class type name</param>
		public DatabaseLookupList(string statement, string assemblyName, string className)
			: this(statement, 10000, null, assemblyName, className)
		{
		}

		/// <summary>
		/// Constructor that specifies the sql statement and the type of 
		/// object represented in the lookup-list
		/// </summary>
		/// <param name="statement">The sql statement used to specify which
		/// objects to load for the lookup-list</param>
		/// <param name="timeout">The time-out period in milliseconds after
		/// which a fresh copy will be loaded</param>
		/// <param name="assemblyName">The class type assembly name.</param>
		/// <param name="className">The class type name</param>
		public DatabaseLookupList(string statement, int timeout, string assemblyName, string className)
			: this(statement, timeout, null, assemblyName, className)
		{
		}

        /// <summary>
        /// Constructor that specifies the sql statement and time-out period
        /// </summary>
        /// <param name="statement">The sql statement used to specify which
        /// objects to load for the lookup-list</param>
        /// <param name="timeout">The time-out period in milliseconds after
        /// which a fresh copy will be loaded</param>
        public DatabaseLookupList(string statement, int timeout) : this(statement, timeout, null)
        {
        }

    	/// <summary>
    	/// Constructor that specifies the sql statement, time-out period and
    	/// the type of object represented in the lookup-list
    	/// </summary>
    	/// <param name="statement">The sql statement used to specify which
    	/// objects to load for the lookup-list</param>
    	/// <param name="timeout">The time-out period in milliseconds after
    	/// which a fresh copy will be loaded</param>
    	/// <param name="lookupObjectType">The object type</param>
    	public DatabaseLookupList(string statement, int timeout, Type lookupObjectType)
    		: this(statement, timeout, lookupObjectType, null, null)
        {
		}

        /// <summary>
        /// Private constructor with all available parameters
        /// </summary>
		private DatabaseLookupList(string statement, int timeout, 
			Type lookupObjectType, string assemblyName, string className)
		{
			_statement = statement;
			_timeout = timeout;
			if (lookupObjectType != null)
			{
				MyLookupObjectType = lookupObjectType;
			}else
			{
				_assemblyName = assemblyName;
				_className = className;
				_lookupObjectType = null;
			}
			_lastCallTime = DateTime.MinValue;
		}

		#endregion Constructors

		#region Properties

        /// <summary>
        /// Gets and sets the assembly name for the class being sourced for data
        /// </summary>
		public string AssemblyName
		{
			get { return _assemblyName; }
			protected set
			{
				if (_assemblyName != value)
				{
					_className = null;
					_lookupObjectType = null;
				}
				_assemblyName = value;
			}
		}

        /// <summary>
        /// Gets and sets the class name being sourced for data
        /// </summary>
		public string ClassName
		{
			get { return _className; }
			protected set
			{
				if (_className != value)
				{
					_lookupObjectType = null;
				}
				_className = value;
			}
		}

		/// <summary>
		/// Gets the sql statement which is used to specify which
		/// objects to load for the lookup-list
		/// </summary>
		public string SqlString
		{
			get { return _statement; }
			set { _statement = value; }
		}

		/// <summary>
		/// Gets and sets the time-out period in seconds after which a fresh
		/// copy will be loaded
		/// </summary>
		public int TimeOut
    	{
			get { return _timeout; }
    		set { _timeout = value; }
    	}

		/// <summary>
		/// Gets the class definition of the lookup type
		/// </summary>
		public ClassDef ClassDef
		{
			get
			{
				if (MyLookupObjectType == null)
				{
					return null;
				} else
				{
					return ClassDef.ClassDefs[MyLookupObjectType];
				}
			}
		}

		#endregion Properties

		#region ILookupList Implementation

		/// <summary>
        /// Returns a lookup-list loaded using the sql 
        /// statement stored in this instance. If the
        /// time-out period has not expired, then the currently held list
        /// will be returned, otherwise a fresh one will be loaded.
        /// </summary>
        /// <returns>Returns a collection of string-Guid pairs</returns>
        public Dictionary<string, object> GetLookupList()
        {
            return this.GetLookupList(DatabaseConnection.CurrentConnection);
        }

        /// <summary>
        /// Returns a lookup-list loaded using the database connection
        /// provided and the sql statement stored in this instance. If the
        /// time-out period has not expired, then the currently held list
        /// will be returned, otherwise a fresh one will be loaded.
        /// </summary>
        /// <param name="connection">The database connection</param>
        /// <returns>Returns a collection of string-Guid pairs</returns>
        public Dictionary<string, object> GetLookupList(IDatabaseConnection connection)
        {
            if (DateTime.Now.Subtract(_lastCallTime).TotalMilliseconds < _timeout)
            {
                return _lookupList;
            }
            _lookupList = new Dictionary<string, object>();
            ISqlStatement statement = new SqlStatement(connection);
            statement.Statement.Append(_statement);

            DataTable dt = connection.LoadDataTable(statement, "", "");
            ArrayList list = new ArrayList(dt.Rows.Count);
            foreach (DataRow row in dt.Rows)
            {
                string originalValue = null;
                int count = 1;
                string stringValue = DBNull.Value.Equals(row[1]) ? "" : (string)row[1];
                while (_lookupList.ContainsKey(stringValue)) {
                    if (originalValue == null) originalValue = stringValue;
                    stringValue = originalValue + "(" + ++count + ")";
                }
                _lookupList.Add(stringValue, new Guid((string)row[0]));
            }
            _lastCallTime = DateTime.Now;
            return _lookupList;
		}

		#endregion ILookupList Implementation

		#region Type Initialisation

		private Type MyLookupObjectType
		{
			get
			{
				TypeLoader.LoadClassType(ref _lookupObjectType,_assemblyName,_className,
					"source class", "Database Lookup List Source Definition");
				return _lookupObjectType;
			}
			set
			{
				_lookupObjectType = value;
				TypeLoader.ClassTypeInfo(_lookupObjectType, out _assemblyName, out _className);
			}
		}

		#endregion Type Initialisation


		/// <summary>
        /// Returns a lookup-list loaded using the sql statement stored in 
        /// this instance. The database connection used is the
        /// one associated with the business object provided. If the
        /// time-out period has not expired, then the currently held list
        /// will be returned, otherwise a fresh one will be loaded.
        /// </summary>
        /// <param name="bo">A business object with attached database
        /// connection</param>
        /// <returns>Returns a collection of string-Guid pairs</returns>
        public Dictionary<string, object> GetLookupList(BusinessObject bo)
        {
            return this.GetLookupList(bo.GetDatabaseConnection());
        }
		 
    }
}