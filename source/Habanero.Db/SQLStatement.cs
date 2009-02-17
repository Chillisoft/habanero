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
    public class SqlStatement : ISqlStatement
    {

        private StringBuilder _statement;
        private readonly IDatabaseConnection _connection;
        private readonly List<IDbDataParameter> _parameters;
        private readonly IDbCommand _sampleCommand;
        private readonly ParameterNameGenerator _gen;
        private readonly IDbConnection _idbConnection;

        /// <summary>
        /// Constructor to initialise a new sql statement
        /// </summary>
        /// <param name="connection">A database connection</param>
        public SqlStatement(IDatabaseConnection connection)
        {
            _parameters = new List<IDbDataParameter>();
            _connection = connection;
            if (_connection != null)
            {
                _idbConnection = _connection.GetConnection();
                if (_idbConnection != null)
                {
                    _sampleCommand = _idbConnection.CreateCommand();
                    _gen = new ParameterNameGenerator(_idbConnection);
                }
                else
                {
                    _gen = new ParameterNameGenerator(null);
                }
            }
            else
            {
                _idbConnection = null;
                _gen = new ParameterNameGenerator(null);
            }
            _statement = new StringBuilder(100);
        }

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
        /// <returns>Returns an IDbDataParameter object</returns>
        public IDbDataParameter AddParameter(string paramName, object paramValue)
        {
            if (paramValue == null)
            {
                paramValue = DBNull.Value;
            }
            IDbDataParameter newParameter = _sampleCommand.CreateParameter();
            //			if ((paramValue is string) && (_idbConnection is MySqlConnection)) {
            //				((MySqlParameter) newParameter).MySqlDbType = MySqlDbType.String ;
            //			}
            newParameter.ParameterName = paramName;
            object preparedValue = DatabaseUtil.PrepareValue(paramValue);
            newParameter.Value = preparedValue;
            if (preparedValue is DateTime)
            {
                newParameter.DbType = DbType.DateTime;
            }
            else if (preparedValue is Decimal)
            {
                newParameter.DbType = DbType.Double; //TODO: workaround for bug in Mysql/Connector with Mysql > 4.1.10
            }
            else if (preparedValue is int)
            {
                newParameter.DbType = DbType.Int32;
            }
            else if (preparedValue is bool)
            {
                newParameter.DbType = DbType.Boolean;
            }
            else if (preparedValue is byte[])
            {
                newParameter.DbType = DbType.Binary;
            }
            else
            {
                newParameter.DbType = DbType.String;
            }

            databaseSpecificParameterSettings(newParameter, paramValue);

            _parameters.Add(newParameter);
            return newParameter;
        }

        private void databaseSpecificParameterSettings(IDbDataParameter newParameter, object paramValue)
        {
            string connectionNamespace = _idbConnection.GetType().Namespace;
            if (connectionNamespace == "System.Data.OracleClient")
            {
                if (paramValue.GetType().Name == "LongText")
                {
                    ReflectionUtilities.SetEnumPropertyValue(newParameter,"OracleType","Clob");
                }
            }
            if (_idbConnection is OleDbConnection)
            {
                OleDbParameter oleDbParameter = newParameter as OleDbParameter;
                if (oleDbParameter != null && paramValue is DateTime && oleDbParameter.OleDbType == OleDbType.DBTimeStamp)
                {
                    oleDbParameter.OleDbType = OleDbType.Date;
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
            StringBuilder s = new StringBuilder(string.Format("Raw statement: {0}   , Parameter values: ", this.Statement));
            foreach (IDbDataParameter param in Parameters)
            {
                s.AppendFormat("{0}, ", param.Value);
            }
            return s.ToString();
        }

        /// <summary>
        /// Returns the parameter name generator
        /// </summary>
        /// <returns>Returns a ParameterNameGenerator object</returns>
        public ParameterNameGenerator ParameterNameGenerator
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
            get { return _connection; }
        }

        /// <summary>
        /// Indicates whether this sql statement instance is equal in
        /// content to the one specified
        /// </summary>
        /// <param name="obj">The sql statement object to compare with</param>
        /// <returns>Returns true if equal</returns>
        public override bool Equals(object obj)
        {
            SqlStatement statement = obj as SqlStatement;
            if (statement != null)
            {

                if (!_statement.ToString().Equals(statement.Statement.ToString()))
                {
                    return false;
                }
                if (_parameters.Count != statement.Parameters.Count)
                {
                    Console.WriteLine("Param count different");
                    return false;
                }
                for (int i = 0; i < _parameters.Count; i++)
                {
                    IDbDataParameter myParam = _parameters[i];
                    IDbDataParameter theirParam = statement.Parameters[i];
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