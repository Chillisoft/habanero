using System;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Db.v2;
using Chillisoft.Generic.v2;

namespace Chillisoft.Bo.v2
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
    /// TODO ERIC - a method to set the lookup type?
    public class DatabaseLookupListSource : ILookupListSource
    {
        private readonly int _timeout;
        private readonly Type _lookupObjectType;
        private DateTime _lastCallTime;
        private readonly string _statement;
        private StringGuidPairCollection _stringGuidPairCollection;

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
        {
            this._statement = statement;
            _timeout = timeout;
            _lookupObjectType = lookupObjectType;
            _lastCallTime = DateTime.MinValue;
        }

        /// <summary>
        /// Returns a lookup-list loaded using the sql 
        /// statement stored in this instance. If the
        /// time-out period has not expired, then the currently held list
        /// will be returned, otherwise a fresh one will be loaded.
        /// </summary>
        /// <returns>Returns a collection of string-Guid pairs</returns>
        public StringGuidPairCollection GetLookupList()
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
        public StringGuidPairCollection GetLookupList(IDatabaseConnection connection)
        {
            if (DateTime.Now.Subtract(_lastCallTime).TotalMilliseconds < _timeout)
            {
                return _stringGuidPairCollection;
            }
            _stringGuidPairCollection = new StringGuidPairCollection();
            ISqlStatement statement = new SqlStatement(connection.GetConnection());
            statement.Statement.Append(_statement);
            _stringGuidPairCollection.Load(connection, statement);
            _lastCallTime = DateTime.Now;
            return _stringGuidPairCollection;
        }

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
        public StringGuidPairCollection GetLookupList(BusinessObject bo)
        {
            return this.GetLookupList(bo.GetDatabaseConnection());
        }

        /// <summary>
        /// Returns the sql statement which is used to specify which
        /// objects to load for the lookup-list
        /// </summary>
        public string SqlString
        {
            get { return _statement; }
        }

        /// <summary>
        /// Returns the class definition of the lookup type
        /// </summary>
        public ClassDef ClassDef
        {
            get
            {
                if (_lookupObjectType == null)
                {
                    return null;
                }
                else
                {
                    return ClassDef.GetClassDefCol[_lookupObjectType];
                }
            }
        }
    }
}