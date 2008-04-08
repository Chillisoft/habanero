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

namespace Habanero.UI.Forms
{
    /// <summary>
    /// An exception to be thrown when the lookup list has not been set before
    /// using a control that requires it
    /// </summary>
    public class LookupListNotSetException : Exception
    {
        /// <summary>
        /// Constructor to initialise the exception with a standard message
        /// </summary>
        public LookupListNotSetException()
            : base("You must set the lookup list before using a control that requires it.")
        {
        }

        /// <summary>
        /// Constructor as before, except that a personalised message can be
        /// specified
        /// </summary>
        /// <param name="message">The message to display</param>
        public LookupListNotSetException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor as before, except that a personalised message and
        /// inner exception can be specified
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="inner">The inner exception</param>
        public LookupListNotSetException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}