//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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

namespace Habanero.Base.Exceptions
{
    /// <summary>
    /// Provides an exception to throw for an invalid argument
    /// </summary>
    [Serializable()]
    public class HabaneroArgumentException : Exception
    {
        /// <summary>
        /// Constructor to initialise a new exception
        /// </summary>
        public HabaneroArgumentException()
        {
        }

        /// <summary>
        /// Constructor to initialise a new exception with the parameter name
        /// </summary>
        /// <param name="parameterName">The parameter name</param>
        public HabaneroArgumentException(string parameterName) : base("The argument '" + parameterName + "' is not valid. ")
        {
        }

        /// <summary>
        /// Constructor to initialise a new exception with the parameter name,
        /// a message to display and an inner exception
        /// </summary>
        /// <param name="parameterName">The parameter name</param>
        /// <param name="message">The error message to display</param>
        /// <param name="inner">The inner exception</param>
        public HabaneroArgumentException(string parameterName,
                                         string message,
                                         Exception inner)
            : base("The argument '" + parameterName + "' is not valid. " + message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise a new exception with the parameter name
        /// and the inner exception
        /// </summary>
        /// <param name="parameterName">The parameter name</param>
        /// <param name="inner">The inner exception</param>
        public HabaneroArgumentException(string parameterName
                                         , Exception inner)
            : base("The argument '" + parameterName + "' is not valid. ", inner)
        {
        }

        /// <summary>
        /// Constructor to initialise a new exception with the parameter name
        /// and an error message to display
        /// </summary>
        /// <param name="parameterName">The parameter name</param>
        /// <param name="message">The error message to display</param>
        public HabaneroArgumentException(string parameterName,
                                         string message)
            : base("The argument '" + parameterName + "' is not valid. " + message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected HabaneroArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}