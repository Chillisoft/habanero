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
        public bool Available { get; private set; }

        public ManagedConnection(IDbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            _connection = connection;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public IDbTransaction BeginTransaction()
        {
            return _connection.BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return _connection.BeginTransaction(il);
        }

        public void Close()
        {
            lock (this)
            {
                _connection.Close();
                Available = true;
            }
        }

        public void ChangeDatabase(string databaseName)
        {
            _connection.ChangeDatabase(databaseName);
        }

        public IDbCommand CreateCommand()
        {
            return _connection.CreateCommand();
        }

        public void Open()
        {
            lock (this)
            {
                _connection.Open();
                Available = false;
            }
        }

        public string ConnectionString 
        {
            get { return _connection.ConnectionString; }
            set { _connection.ConnectionString = value; }
        }
        public int ConnectionTimeout { get { return _connection.ConnectionTimeout; } }
        public string Database { get { return _connection.Database; } }
        public ConnectionState State { get { return _connection.State; } }
    }
}