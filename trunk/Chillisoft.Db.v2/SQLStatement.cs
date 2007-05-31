using System;
using System.Collections;
using System.Data;
using System.Text;
using Chillisoft.Generic.v2;
using Chillisoft.Util.v2;

namespace Chillisoft.Db.v2
{
    /// <summary>
    /// Manages a sql statement
    /// </summary>
    public class SqlStatement : ISqlStatement
    {
        private StringBuilder mStatement;
        private IDbConnection mConnection;
        private IList mParameters;
        private IDbCommand mSampleCommand;
        private ParameterNameGenerator gen;

        /// <summary>
        /// Constructor to initialise a new sql statement
        /// </summary>
        /// <param name="connection">A database connection</param>
        public SqlStatement(IDbConnection connection)
        {
            mParameters = new ArrayList();
            mConnection = connection;
            if (connection != null)
            {
                mSampleCommand = mConnection.CreateCommand();
                gen = new ParameterNameGenerator(connection);
            }
            else
            {
                gen = new ParameterNameGenerator(null);
            }
            mStatement = new StringBuilder(100);
        }

        /// <summary>
        /// Constructor to initialise a new sql statement using the existing
        /// statement provided
        /// </summary>
        /// <param name="connection">A database connection</param>
        /// <param name="statement">An existing sql statement</param>
        public SqlStatement(IDbConnection connection, string statement) : this(connection)
        {
            mStatement = new StringBuilder(statement);
        }

        /// <summary>
        /// Gets and sets the sql statement
        /// </summary>
        public StringBuilder Statement
        {
            get { return mStatement; }
            set { mStatement = value; }
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
            IDbDataParameter newParameter = mSampleCommand.CreateParameter();
            //			if ((paramValue is string) && (mConnection is MySqlConnection)) {
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

            mParameters.Add(newParameter);
            return newParameter;
        }

        private void databaseSpecificParameterSettings(IDbDataParameter newParameter, object paramValue)
        {
            string connectionNamespace = mConnection.GetType().Namespace;
            if (connectionNamespace == "System.Data.OracleClient")
            {
                if (paramValue.GetType().Name == "LongText")
                {
                    ReflectionUtilities.setEnumPropertyValue(newParameter,"OracleType","Clob");
                }
            }
        }
        

        /// <summary>
        /// Returns a list of parameters
        /// </summary>
        public IList Parameters
        {
            get { return mParameters; }
        }

        /// <summary>
        /// Sets up the IDbCommand object
        /// </summary>
        /// <param name="command">The command</param>
        public void SetupCommand(IDbCommand command)
        {
            command.CommandType = CommandType.Text;
            command.CommandText = this.mStatement.ToString();
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
        /// TODO ERIC - this might be more useful if it was in usable format
        public override string ToString()
        {
            string str = "Raw statement: " + this.Statement + "   , Parameter values: ";
            foreach (IDbDataParameter param in Parameters)
            {
                str += param.Value.ToString() + ", ";
            }
            return str;
        }

        /// <summary>
        /// Returns the parameter name generator
        /// </summary>
        /// <returns>Returns a ParameterNameGenerator object</returns>
        public ParameterNameGenerator GetParameterNameGenerator()
        {
            return gen;
        }

        /// <summary>
        /// Adds a parameter to the sql statement
        /// </summary>
        /// <param name="obj">The parameter to add</param>
        public void AddParameterToStatement(object obj)
        {
            string paramName = gen.GetNextParameterName();
            this.AddParameter(paramName, obj);
            this.Statement.Append(paramName);
        }

        /// <summary>
        /// Indicates whether this sql statement instance is equal in
        /// content to the one specified
        /// </summary>
        /// <param name="obj">The sql statement object to compare with</param>
        /// <returns>Returns true if equal</returns>
        public override bool Equals(object obj)
        {
            if (obj is SqlStatement)
            {
                SqlStatement statement = (SqlStatement) obj;
                if (!mStatement.ToString().Equals(statement.Statement.ToString()))
                {
                    return false;
                }
                if (mParameters.Count != statement.Parameters.Count)
                {
                    Console.WriteLine("Param count different");
                    return false;
                }
                for (int i = 0; i < mParameters.Count; i++)
                {
                    IDbDataParameter myParam = (IDbDataParameter) mParameters[i];
                    IDbDataParameter theirParam = (IDbDataParameter) statement.Parameters[i];
                    if (!myParam.GetType().Equals(theirParam.GetType()) ||
                        !myParam.ParameterName.Equals(theirParam.ParameterName) ||
                        !myParam.Value.Equals(theirParam.Value))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return base.Equals(obj);
            }
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
        /// Appends a criteria clause to the sql statement.  " WHERE " (or
        /// " AND " where appropriate) is automatically prefixed by this
        /// method.
        /// </summary>
        /// <param name="criteria">The criteria clause</param>
        public void AppendCriteria(string criteria)
        {
            if (criteria != null && criteria.Length > 0)
            {
                if (this.Statement.ToString().IndexOf(" WHERE ") != -1)
                {
                    this.Statement.Append(" AND " + criteria);
                }
                else
                {
                    this.Statement.Append(" WHERE " + criteria);
                }
            }
        }

        /// <summary>
        /// Appends " WHERE " to the sql statement, or " AND " if a
        /// where-clause already exists
        /// </summary>
        public void AppendWhere()
        {
            if (this.Statement.ToString().IndexOf(" WHERE ") != -1)
            {
                this.Statement.Append(" AND ");
            }
            else
            {
                this.Statement.Append(" WHERE ");
            }
        }

        /// <summary>
        /// Appends an order-by clause to the sql statement. " ORDER BY " is
        /// automatically prefixed by this method.
        /// </summary>
        /// <param name="orderByCriteria">The order-by clause</param>
        public void AppendOrderBy(string orderByCriteria)
        {
            if (orderByCriteria != null && orderByCriteria.Length > 0)
            {
                this.Statement.Append(" ORDER BY " + orderByCriteria);
            }
        }
    }
}