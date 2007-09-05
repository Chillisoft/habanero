using System;
using System.Runtime.Serialization;


namespace Habanero.DB
{
    /// <summary>
    /// An exception thrown when an error occurred while attempting to
    /// read from a database
    /// </summary>
    [Serializable()]
    public class DatabaseReadException : Exception
    {
        private string _sqlStatement;
        private string _connectString;
        private string _developerMessage;

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        /// <param name="userMessage">A message to users</param>
        /// <param name="developerMessage">A message to developers</param>
        /// <param name="sqlStatement">The sql statement that was used</param>
        /// <param name="connectString">The connection string that was used</param>
        public DatabaseReadException(string userMessage, string developerMessage,
                                     string sqlStatement, string connectString) : base(userMessage)
        {
            _sqlStatement = sqlStatement;
            _connectString = connectString;
            _developerMessage = developerMessage;
        }

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        /// <param name="userMessage">A message to users</param>
        /// <param name="developerMessage">A message to developers</param>
        /// <param name="inner">The inner exception</param>
        /// <param name="sqlStatement">The sql statement that was used</param>
        /// <param name="connectString">The connection string that was used</param>
        public DatabaseReadException(string userMessage, string developerMessage, Exception inner,
                                     string sqlStatement, string connectString) : base(userMessage, inner)
        {
            _sqlStatement = sqlStatement;
            _connectString = connectString;
            _developerMessage = developerMessage;
        }

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        public DatabaseReadException()
        {
        }

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        /// <param name="message">The message to display</param>
        public DatabaseReadException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="inner">The inner exception</param>
        public DatabaseReadException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        /// <param name="info">Serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected DatabaseReadException(SerializationInfo info, StreamingContext context) : base(info, context)
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

        /// <summary>
        /// Required for ISerializable.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("sqlStatement", _sqlStatement);
            info.AddValue("developerMessage", _developerMessage);
            info.AddValue("connectString", _connectString);
        }
    }
}