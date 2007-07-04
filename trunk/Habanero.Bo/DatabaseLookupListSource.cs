using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Habanero.Bo.ClassDefinition;
using Habanero.DB;
using Habanero.Base;
using Habanero.Util;
using Habanero.Util.File;

namespace Habanero.Bo
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
    public class DatabaseLookupListSource : ILookupListSource
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
        public DatabaseLookupListSource(string statement) : this(statement, 10000)
        {
        }

        /// <summary>
        /// Constructor that specifies the sql statement and the type of 
        /// object represented in the lookup-list
        /// </summary>
        /// <param name="statement">The sql statement used to specify which
        /// objects to load for the lookup-list</param>
        /// <param name="lookupObjectType">The object type</param>
        public DatabaseLookupListSource(string statement, Type lookupObjectType)
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
		public DatabaseLookupListSource(string statement, string assemblyName, string className)
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
		public DatabaseLookupListSource(string statement, int timeout, string assemblyName, string className)
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
        public DatabaseLookupListSource(string statement, int timeout) : this(statement, timeout, null)
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
    	public DatabaseLookupListSource(string statement, int timeout, Type lookupObjectType)
    		: this(statement, timeout, lookupObjectType, null, null)
        {
		}

		private DatabaseLookupListSource(string statement, int timeout, 
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

		protected string AssemblyName
		{
			get { return _assemblyName; }
			set
			{
				if (_assemblyName != value)
				{
					_className = null;
					_lookupObjectType = null;
				}
				_assemblyName = value;
			}
		}

		protected string ClassName
		{
			get { return _className; }
			set
			{
				if (_className != value)
				{
					_lookupObjectType = null;
				}
				_className = value;
			}
		}

		/// <summary>
		/// Returns the sql statement which is used to specify which
		/// objects to load for the lookup-list
		/// </summary>
		public string SqlString
		{
			get { return _statement; }
			protected set { _statement = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		protected int TimeOut
    	{
			get { return _timeout; }
    		set { _timeout = value; }
    	}

		/// <summary>
		/// Returns the class definition of the lookup type
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

		#region ILookupListSource Implementation

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
            ISqlStatement statement = new SqlStatement(connection.GetConnection());
            statement.Statement.Append(_statement);

            DataTable dt = connection.LoadDataTable(statement, "", "");
            ArrayList list = new ArrayList(dt.Rows.Count);
            foreach (DataRow row in dt.Rows)
            {
                string stringValue = DBNull.Value.Equals(row[0]) ? "" : (string)row[0];
                _lookupList.Add(stringValue, new Guid((string)row[1]));
            }
            _lastCallTime = DateTime.Now;
            return _lookupList;
		}

		#endregion ILookupListSource Implementation

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
        /// <param name="connection">The database connection</param>
        /// <param name="bo">A business object with attached database
        /// connection</param>
        /// <returns>Returns a collection of string-Guid pairs</returns>
        public Dictionary<string, object> GetLookupList(BusinessObject bo)
        {
            return this.GetLookupList(bo.GetDatabaseConnection());
        }
		 
    }
}