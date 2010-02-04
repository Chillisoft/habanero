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

namespace Habanero.Base.Exceptions
{
    /// <summary>
    /// Provides an Exception class which is raised in the Habanero Architecture when a developer uses 
    /// the architecture or a method in the architecture incorrectly e.g. if a method is called with invalid 
    /// paramaters.
    /// </summary>    
[Serializable]
    public class HabaneroDeveloperException:Exception
    {
    /// <summary>
    /// The message shown to the developer, logged and emailed when this error is raised.
    /// </summary>
        protected readonly string _developerMessage;

        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public HabaneroDeveloperException()
        {
        }
       /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="userMessage">The error message</param>
        /// <param name="developerMessage">An Extended error message for the developer</param>
        public HabaneroDeveloperException(string userMessage, string developerMessage) : base(userMessage)
        {
           _developerMessage = developerMessage;
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="userMessage">The user error message</param>
        /// <param name="developerMessage">An extended error message for the developer</param>
        /// <param name="inner">The inner exception</param>
        public HabaneroDeveloperException(string userMessage, string developerMessage, Exception inner) : base(userMessage, inner)
        {
            _developerMessage = developerMessage;
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected HabaneroDeveloperException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        ///<summary>
        /// The developer message set in teh constructor
        ///</summary>
        public virtual string DeveloperMessage
        {
            get { return _developerMessage; }
        }
        /// <summary>
        /// Required for ISerializable.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("developerMessage", _developerMessage);
        }
    }
    /// <summary>
    /// Provides a generalised application exception to throw
    /// </summary>
    [Serializable]
    public class HabaneroApplicationException : HabaneroDeveloperException
    {
        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public HabaneroApplicationException()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public HabaneroApplicationException(string message) : base(message, "")
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public HabaneroApplicationException(string message, Exception inner) : base(message, "", inner)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected HabaneroApplicationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    /// <summary>
    /// Provides an exception to throw in the case where a user action
    /// has resulted in some kind of malfunction
    /// </summary>
    [Serializable]
    public class UserException : Exception
    {
        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public UserException()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public UserException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public UserException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected UserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    /// <summary>
    /// Provides an Exception class which is raised in the Habanero Architecture when a developer uses 
    /// the architecture or a method in the architecture incorrectly e.g. if a method is called with invalid 
    /// paramaters.
    /// </summary>    
    [Serializable]
    public class HabaneroIncorrectTypeException : HabaneroDeveloperException
    {

        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public HabaneroIncorrectTypeException()
        {
        }
        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="userMessage">The error message</param>
        /// <param name="developerMessage">An Extended error message for the developer</param>
        public HabaneroIncorrectTypeException(string userMessage, string developerMessage)
            : base(userMessage, developerMessage)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display, and the inner exception specified
        /// </summary>
        /// <param name="userMessage">The user error message</param>
        /// <param name="developerMessage">An extended error message for the developer</param>
        /// <param name="inner">The inner exception</param>
        public HabaneroIncorrectTypeException(string userMessage, string developerMessage, Exception inner)
            : base(userMessage, developerMessage, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected HabaneroIncorrectTypeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        ///<summary>
        /// The developer message set in the constructor
        ///</summary>
        public override string DeveloperMessage
        {
            get { return _developerMessage; }
        }
        /// <summary>
        /// Required for ISerializable.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("developerMessage", _developerMessage);
        }
    }
}