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
using System.Data;
using System.Data.OleDb;
using System.Text;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.DB
{
    /// <summary>
    /// Manages a sql statement
    /// </summary>
    [Serializable]
    public class SqlStatement : ISqlStatement
    {
        private StringBuilder _statement;
        private readonly List<IDbDataParameter> _parameters;
        private readonly IDbCommand _sampleCommand;
        private readonly IParameterNameGenerator _gen;
        private readonly IDbConnection _idbConnection;

        /// <summary>
        /// Constructor to initialise a new sql statement
        /// </summary>
        /// <param name="connection">A database connection</param>
        public SqlStatement(IDatabaseConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            _parameters = new List<IDbDataParameter>();
            DatabaseConnection = connection;
            _idbConnection = DatabaseConnection.GetConnection();
            if (_idbConnection != null)
            {
                _sampleCommand = _idbConnection.CreateCommand();
                _gen = connection.CreateParameterNameGenerator();
            }
            else
            {
                _gen = new ParameterNameGenerator(null);
            }
            _statement = new StringBuilder(100);
        }

        ///<summary>
        /// Returns the <see cref="IDatabaseConnection"/> this statement is using. This
        /// connection is required for generating commands, parameters etc for the correct
        /// db type
        ///</summary>
        public IDatabaseConnection DatabaseConnection { get; private set; }

        /// <summary>
        /// Constructor to initialise a new sql statement using the existing
        /// statement provided
        /// </summary>
        /// <param name="connection">A database connection</param>
        /// <param name="statement">An existing sql statement</param>
        public SqlStatement(IDatabaseConnection connection, string statement)
            : this(connection)
        {
            _statement = new StringBuilder(statement);
        }

        /// <summary>
        /// Gets and sets the sql statement
        /// </summary>
        public StringBuilder Statement
        {
            get { return _statement; }
            set { _statement = value; }
        }

        /// <summary>
        /// Adds a parameter value
        /// </summary>
        /// <param name="paramName">The parameter name</param>
        /// <param name="paramValue">The value to assign</param>
        /// <param name="paramType">The type of the parameter (only necessary if the value is null, or you wish to specify more directly the type to use when creating the parameter)</param>
        /// <returns>Returns an IDbDataParameter object</returns>
        public IDbDataParameter AddParameter(string paramName, object paramValue, Type paramType)
        {
            var newParameter = CreateParameter(paramName, paramValue, paramType);
            _parameters.Add(newParameter);
            return newParameter;
        }

        private IDbDataParameter CreateParameter(string paramName, object paramValue, Type paramType)
        {
            var paramDbType = DbType.String;
            if (paramType != null) paramDbType = GetParamTypeForType(paramType);
            var newParameter = CreateParameter(paramName);
            if (paramValue == null)
            {
                UpdateParam(newParameter, DBNull.Value, paramDbType);
                return newParameter;
            }
            var preparedValue = DatabaseConnection.SqlFormatter.PrepareValue(paramValue);
            if (paramType == null) paramDbType = GetParamTypeForType(preparedValue.GetType());
            UpdateParam(newParameter, preparedValue, paramDbType);
            databaseSpecificParameterSettings(newParameter, paramValue);
            return newParameter;
        }

        /// <summary>
        /// Adds a parameter value
        /// </summary>
        /// <param name="paramName">The parameter name</param>
        /// <param name="paramValue">The value to assign</param>
        /// <returns>Returns an IDbDataParameter object</returns>
        public IDbDataParameter AddParameter(string paramName, object paramValue)
        {
            return AddParameter(paramName, paramValue, null);
        }

        private DbType GetParamTypeForType(Type paramType)
        {
            if (paramType == typeof(DateTime)) return DbType.DateTime;
            if (paramType == typeof(TimeSpan)) return DbType.DateTime;
            if (paramType == typeof(Decimal)) return DbType.Decimal;
            if (paramType == typeof(Double)) return DbType.Double;
            if (paramType == typeof(int)) return DbType.Int32;
            if (paramType == typeof(bool)) return DbType.Boolean;
            if (paramType == typeof(byte[])) return DbType.Binary;
            return DbType.String;
        }

        private void UpdateParam(IDbDataParameter parameter, object value, DbType dbType)
        {
            parameter.Value = value;
            parameter.DbType = dbType;
        }

        private IDbDataParameter CreateParameter(string paramName)
        {
            var newParameter = _sampleCommand.CreateParameter();
            newParameter.ParameterName = paramName;
            return newParameter;
        }

        private void databaseSpecificParameterSettings(IDbDataParameter newParameter, object paramValue)
        {
            string connectionNamespace = _idbConnection.GetType().Namespace;
            UpdateParameterTypeForOracleLongText(connectionNamespace, paramValue, newParameter);
            UpdateParameterTypeForSqlServerCEImage(connectionNamespace, paramValue, newParameter);
            UpdateParameterTypeForOleDBDateTime(paramValue, newParameter);
        }

        private void UpdateParameterTypeForOleDBDateTime(object paramValue, IDbDataParameter newParameter)
        {
            if (_idbConnection is OleDbConnection)
            {
                OleDbParameter oleDbParameter = newParameter as OleDbParameter;
                if (oleDbParameter != null && paramValue is DateTime && oleDbParameter.OleDbType == OleDbType.DBTimeStamp)
                {
                    oleDbParameter.OleDbType = OleDbType.Date;
                }
            }
        }


        private void UpdateParameterTypeForSqlServerCEImage(string connectionNamespace, object paramValue, IDbDataParameter newParameter)
        {
            if (connectionNamespace == "System.Data.SqlServerCe")
            {
                if (paramValue is byte[])
                {
                    ReflectionUtilities.SetEnumPropertyValue(newParameter, "SqlDbType", "Image");
                }

                if (paramValue is string && Encoding.Default.GetByteCount((string)paramValue) > 4000)
                {
                    ReflectionUtilities.SetEnumPropertyValue(newParameter, "SqlDbType", "NText");
                }
            }
        }

        private void UpdateParameterTypeForOracleLongText(string connectionNamespace, object paramValue, IDbDataParameter newParameter)
        {
            if (connectionNamespace == "System.Data.OracleClient")
            {
                if (paramValue.GetType().Name == "LongText")
                {
                    ReflectionUtilities.SetEnumPropertyValue(newParameter,"OracleType","Clob");
                }
            }
        }

        /// <summary>
        /// Returns a list of parameters
        /// </summary>
        public List<IDbDataParameter> Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// Sets up the IDbCommand object
        /// </summary>
        /// <param name="command">The command</param>
        public void SetupCommand(IDbCommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            command.CommandType = CommandType.Text;
            command.CommandText = this._statement.ToString();
            command.Parameters.Clear();
            foreach (IDbDataParameter param in this.Parameters)
            {
                command.Parameters.Add(param);
            }
        }

        /// <summary>
        /// Returns a string describing the sql statement and its parameters.
        /// This string cannot be used directly as a sql statement on its own.
        /// </summary>
        /// <returns>Returns a string</returns>
        public override string ToString()
        {
            var s = new StringBuilder(string.Format("Raw statement: {0}   , Parameter values: ", this.Statement));
            foreach (var param in Parameters)
            {
                s.AppendFormat("{0}, ", param.Value);
            }
            return s.ToString();
        }

        /// <summary>
        /// Returns the parameter name generator
        /// </summary>
        /// <returns>Returns a ParameterNameGenerator object</returns>
        public IParameterNameGenerator ParameterNameGenerator
        {
            get { return _gen; }
        }

        /// <summary>
        /// Adds a parameter to the sql statement
        /// </summary>
        /// <param name="obj">The parameter to add</param>
        public void AddParameterToStatement(object obj)
        {
            string paramName = _gen.GetNextParameterName();
            this.AddParameter(paramName, obj);
            this.Statement.Append(paramName);
        }


        /// <summary>
        /// Returns a hashcode calculation from the sql statement string
        /// </summary>
        /// <returns>Returns a hashcode</returns>
        public override int GetHashCode()
        {
            //TODO: need a hash code dependent on the data here because this is a value type.
            return base.GetHashCode();
        }

        /// <summary>
        /// Carries out instructions after execution of the sql statement
        /// </summary>
        internal virtual void DoAfterExecute(DatabaseConnection conn, IDbTransaction tran, IDbCommand command)
        {
        }

        /// <summary>
        /// Gets the database connection provided upon
        /// instantiation of this sql statement
        /// </summary>
        public IDatabaseConnection Connection
        {
            get { return DatabaseConnection; }
        }

        /// <summary>
        /// Indicates whether this sql statement instance is equal in
        /// content to the one specified
        /// </summary>
        /// <param name="obj">The sql statement object to compare with</param>
        /// <returns>Returns true if equal</returns>
        public override bool Equals(object obj)
        {
            var statement = obj as SqlStatement;
            if (statement != null)
            {
                if (!_statement.ToString().Equals(statement.Statement.ToString()))
                {
                    return false;
                }
                if (_parameters.Count != statement.Parameters.Count)
                {
                    Console.WriteLine(@"Param count different");
                    return false;
                }
                for (int i = 0; i < _parameters.Count; i++)
                {
                    var myParam = _parameters[i];
                    var theirParam = statement.Parameters[i];
                    if (!myParam.GetType().Equals(theirParam.GetType()) ||
                        !myParam.ParameterName.Equals(theirParam.ParameterName) ||
                        !myParam.Value.Equals(theirParam.Value))
                    {
                        return false;
                    }
                }
                return true;
            }
            return base.Equals(obj);
        }
    }
}