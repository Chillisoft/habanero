using System;
using System.Runtime.Serialization;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Chillisoft.Util.v2
{
    /// <summary>
    /// Provides an exception to throw for an invalid argument
    /// </summary>
    /// TODO ERIC - rename to something contemporary
    [Serializable()]
    public class CoreBizArgumentException : BaseApplicationException
    {
        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        public CoreBizArgumentException()
        {
        }

        /// <summary>
        /// Constructor to initialise a new exception with the parameter name
        /// </summary>
        /// <param name="parameterName">The parameter name</param>
        public CoreBizArgumentException(string parameterName) : base(parameterName + " not valid. ")
        {
        }

        /// <summary>
        /// Constructor to initialise a new exception with the parameter name,
        /// a message to display and an inner exception
        /// </summary>
        /// <param name="parameterName">The parameter name</param>
        /// <param name="message">The error message to display</param>
        /// <param name="inner">The inner exception</param>
        public CoreBizArgumentException(string parameterName,
                                        string message,
                                        Exception inner) : base(parameterName + " not valid. " + message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise a new exception with the parameter name
        /// and the inner exception
        /// </summary>
        /// <param name="parameterName">The parameter name</param>
        /// <param name="inner">The inner exception</param>
        public CoreBizArgumentException(string parameterName
                                        , Exception inner) : base(parameterName + " not valid. ", inner)
        {
        }

        /// <summary>
        /// Constructor to initialise a new exception with the parameter name
        /// and an error message to display
        /// </summary>
        /// <param name="parameterName">The parameter name</param>
        /// <param name="message">The error message to display</param>
        public CoreBizArgumentException(string parameterName,
                                        string message) : base(parameterName + " not valid. " + message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected CoreBizArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}