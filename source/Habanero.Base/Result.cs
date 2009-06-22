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
using System.Collections.Generic;
using System.Text;

namespace Habanero.Base
{
    /// <summary>
    /// Indicates whether an operation was successful and provides
    /// a message to display to the user.  This class provides a simpler alternative to
    /// using "out string errors" as a way of recognising errors in an operation.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Instantiates a new result
        /// </summary>
        /// <param name="successful">Whether the operation was a success or not</param>
        public Result(bool successful)
        {
            Successful = successful;
        }

        /// <summary>
        /// Instantiates a new result
        /// </summary>
        /// <param name="successful">Whether the operation was a success or not</param>
        /// <param name="message">The message to display to the user.  A message can
        /// be returned for either a successful or failed operation.</param>
        public Result(bool successful, string message)
        {
            Successful = successful;
            Message = message;
        }

        /// <summary>
        /// Gets whether the operation was successful
        /// </summary>
        public bool Successful { get; private set; }

        /// <summary>
        /// Gets the message provided for the user.  A message can be provided
        /// for either successful or failed operations.
        /// </summary>
        public string Message { get; private set; }
    }
}
