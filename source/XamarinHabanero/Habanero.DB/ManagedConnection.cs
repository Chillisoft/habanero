using System;
using System.Data;

namespace Habanero.DB
{
    /// <summary>
    /// Facade to provide a cheap leasing effect on database connections for helping with concurrency
    /// </summary>
    public class ManagedConnection: IDbConnection
    {
        private IDbConnection _connection;
        /// <summary>
        /// Is this connection available for use?
        /// </summary>
        public bool Available { get; private set; }

        /// <summary>
        /// Wrap an <see cref="IDbConnection"/> object with the managed availablility wrapper.
        /// </summary>
        /// <param name="connection">The <see cref="IDbConnection"/> to wrap</param>
        /// <exception cref="ArgumentNullException">This exception is thrown if the object to wrap is null.</exception>
        public ManagedConnection(IDbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            _connection = connection;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            _connection.Dispose();
        }

        /// <summary>
        /// Begins a database transaction.
        /// </summary>
        /// <returns>
        /// An object representing the new transaction.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public IDbTransaction BeginTransaction()
        {
            return _connection.BeginTransaction();
        }

        /// <summary>
        /// Begins a database transaction with the specified <see cref="T:System.Data.IsolationLevel"/> value.
        /// </summary>
        /// <returns>
        /// An object representing the new transaction.
        /// </returns>
        /// <param name="il">One of the <see cref="T:System.Data.IsolationLevel"/> values. </param><filterpriority>2</filterpriority>
        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return _connection.BeginTransaction(il);
        }

        /// <summary>
        /// Closes the connection to the database.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Close()
        {
            lock (this)
            {
                _connection.Close();
                Available = true;
            }
        }

        /// <summary>
        /// Changes the current database for an open Connection object.
        /// </summary>
        /// <param name="databaseName">The name of the database to use in place of the current database. </param><filterpriority>2</filterpriority>
        public void ChangeDatabase(string databaseName)
        {
            _connection.ChangeDatabase(databaseName);
        }

        /// <summary>
        /// Creates and returns a Command object associated with the connection.
        /// </summary>
        /// <returns>
        /// A Command object associated with the connection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public IDbCommand CreateCommand()
        {
            return _connection.CreateCommand();
        }

        /// <summary>
        /// Opens a database connection with the settings specified by the ConnectionString property of the provider-specific Connection object.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Open()
        {
            lock (this)
            {
                _connection.Open();
                Available = false;
            }
        }

        /// <summary>
        /// Gets or sets the string used to open a database.
        /// </summary>
        /// <returns>
        /// A string containing connection settings.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public string ConnectionString 
        {
            get { return _connection.ConnectionString; }
            set { _connection.ConnectionString = value; }
        }

        /// <summary>
        /// Gets the time to wait while trying to establish a connection before terminating the attempt and generating an error.
        /// </summary>
        /// <returns>
        /// The time (in seconds) to wait for a connection to open. The default value is 15 seconds.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public int ConnectionTimeout { get { return _connection.ConnectionTimeout; } }

        /// <summary>
        /// Gets the name of the current database or the database to be used after a connection is opened.
        /// </summary>
        /// <returns>
        /// The name of the current database or the name of the database to be used once a connection is open. The default value is an empty string.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public string Database { get { return _connection.Database; } }

        /// <summary>
        /// Gets the current state of the connection.
        /// </summary>
        /// <returns>
        /// One of the <see cref="T:System.Data.ConnectionState"/> values.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public ConnectionState State { get { return _connection.State; } }
    }
}