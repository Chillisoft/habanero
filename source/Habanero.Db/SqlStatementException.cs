using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Habanero.DB
{
    /// <summary>
    /// An exception thrown when an error occurred while attempting to
    /// construct a SQL statement
    /// </summary>
    [Serializable()]
    public class SqlStatementException : Exception
    {
        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        public SqlStatementException()
        {
        }

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        /// <param name="message">The message to display</param>
        public SqlStatementException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="inner">The inner exception</param>
        public SqlStatementException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        /// <param name="info">Serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected SqlStatementException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        ///// <summary>
        ///// Returns a summary of this exception as a string
        ///// </summary>
        ///// <returns>Returns a string</returns>
        //public override string ToString()
        //{
        //    return base.ToString();
        //}

        ///// <summary>
        ///// Required for ISerializable.
        ///// </summary>
        ///// <param name="info"></param>
        ///// <param name="context"></param>
        //public override void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    base.GetObjectData(info, context);
        //    info.AddValue("sqlStatement", _sqlStatement);
        //    info.AddValue("developerMessage", _developerMessage);
        //    info.AddValue("connectString", _connectString);
        //}
    }
}
