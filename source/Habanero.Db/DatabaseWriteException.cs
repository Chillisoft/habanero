// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using System;
using System.Runtime.Serialization;
using Habanero.Base.Exceptions;

namespace Habanero.DB
{
    /// <summary>
    /// An exception thrown when an error occurred while attempting to
    /// write to a database
    /// </summary>
    [Serializable]
    public class DatabaseWriteException : HabaneroDeveloperException
    {
        private readonly string _sqlStatement;
        private readonly string _connectString;
//        private readonly string _developerMessage;

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        /// <param name="userMessage">A message to users</param>
        /// <param name="developerMessage">A message to developers</param>
        /// <param name="sqlStatement">The sql statement that was used</param>
        /// <param name="connectString">The connection string that was used</param>
        public DatabaseWriteException(string userMessage, string developerMessage,
                                      string sqlStatement, string connectString)
            : base(userMessage, developerMessage + Environment.NewLine + sqlStatement)
        {
            _sqlStatement = sqlStatement;
            _connectString = connectString;
//            _developerMessage = developerMessage;
        }

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        /// <param name="userMessage">A message to users</param>
        /// <param name="developerMessage">A message to developers</param>
        /// <param name="inner">The inner exception</param>
        /// <param name="sqlStatement">The sql statement that was used</param>
        /// <param name="connectString">The connection string that was used</param>
        public DatabaseWriteException(string userMessage, string developerMessage, Exception inner,
                                      string sqlStatement, string connectString)
            : base(userMessage, developerMessage + Environment.NewLine + sqlStatement, inner)
        {
            _sqlStatement = sqlStatement;
            _connectString = connectString;
//            _developerMessage = developerMessage;
        }

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        public DatabaseWriteException()
        {
        }

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        /// <param name="message">The message to display</param>
        public DatabaseWriteException(string message) : base(message, "")
        {
        }

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="inner">The inner exception</param>
        public DatabaseWriteException(string message, Exception inner) : base(message, "", inner)
        {
        }

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        /// <param name="info">Serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected DatabaseWriteException(SerializationInfo info, StreamingContext context) : base(info, context)
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

        ///// <summary>
        ///// Returns the message for developers
        ///// </summary>
        //public string DeveloperMessage
        //{
        //    get { return _developerMessage; }
        //}

        /// <summary>
        /// Required for ISerializable.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("sqlStatement", _sqlStatement);
            info.AddValue("developerMessage",_developerMessage);
            info.AddValue("connectString",_connectString);
        }
    }
}