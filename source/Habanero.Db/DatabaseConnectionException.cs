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
using System.Runtime.Serialization;

namespace Habanero.DB
{
    /// <summary>
    /// An exception thrown when an error occurred while attempting to
    /// connect to a database
    /// </summary>
    [Serializable()]
    class DatabaseConnectionException : Exception
    {
        private string _sqlStatement;
        private string _connectString;
        private string _developerMessage;

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        /// <param name="userMessage">A message to users</param>
        /// <param name="developerMessage">A message to developers</param>
        /// <param name="SqlStatement">The sql statement that was used</param>
        /// <param name="ConnectString">The connection string that was used</param>
        public DatabaseConnectionException(string userMessage, string developerMessage,
                                     string SqlStatement, string ConnectString) : base(userMessage)
        {
            _sqlStatement = SqlStatement;
            _connectString = ConnectString;
            _developerMessage = developerMessage;
        }

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        /// <param name="userMessage">A message to users</param>
        /// <param name="developerMessage">A message to developers</param>
        /// <param name="inner">The inner exception</param>
        /// <param name="SqlStatement">The sql statement that was used</param>
        /// <param name="ConnectString">The connection string that was used</param>
        public DatabaseConnectionException(string userMessage, string developerMessage, Exception inner,
                                     string SqlStatement, string ConnectString) : base(userMessage, inner)
        {
            _sqlStatement = SqlStatement;
            _connectString = ConnectString;
            _developerMessage = developerMessage;
        }

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        /// <param name="userMessage">A message to users</param>
        /// <param name="developerMessage">A message to developers</param>
        /// <param name="inner">The inner exception</param>
        /// <param name="ConnectString">The connection string that was used</param>
        public DatabaseConnectionException(string userMessage, string developerMessage, Exception inner,
                                     string ConnectString) : base(userMessage, inner)
        {
            _sqlStatement = "";
            _connectString = ConnectString;
            _developerMessage = developerMessage;
        }

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        public DatabaseConnectionException()
        {
        }

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        /// <param name="message">The message to display</param>
        public DatabaseConnectionException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="inner">The inner exception</param>
        public DatabaseConnectionException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        /// <param name="info">Serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected DatabaseConnectionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Returns the sql statement used
        /// </summary>
        public string SqlStatement
        {
            get { return _sqlStatement; }
        }

        /// <summary>
        /// Returns the connection string used
        /// </summary>
        public string ConnectString
        {
            get { return _connectString; }
        }

        /// <summary>
        /// Returns the message for developers
        /// </summary>
        public string DeveloperMessage
        {
            get { return _developerMessage; }
        }

        /// <summary>
        /// Returns a summary of this exception as a string
        /// </summary>
        /// <returns>Returns a string</returns>
        public override string ToString()
        {
            return this.Message + Environment.NewLine + this.DeveloperMessage + Environment.NewLine + this.SqlStatement;
        }
    }
}