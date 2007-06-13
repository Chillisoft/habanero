using System;
using System.Runtime.Serialization;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Chillisoft.Db.v2
{
    /// <summary>
    /// An exception thrown when an error occurred while attempting to
    /// write to a database
    /// </summary>
    [Serializable()]
    public class DatabaseWriteException : BaseApplicationException
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
        public DatabaseWriteException(string userMessage, string developerMessage,
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
        public DatabaseWriteException(string userMessage, string developerMessage, Exception inner,
                                      string SqlStatement, string ConnectString) : base(userMessage, inner)
        {
            _sqlStatement = SqlStatement;
            _connectString = ConnectString;
            _developerMessage = developerMessage;
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
        public DatabaseWriteException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="inner">The inner exception</param>
        public DatabaseWriteException(string message, Exception inner) : base(message, inner)
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

        /// <summary>
        /// Returns the message for developers
        /// </summary>
        public string DeveloperMessage
        {
            get { return _developerMessage; }
        }
    }
}