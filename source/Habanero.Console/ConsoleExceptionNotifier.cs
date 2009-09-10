//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.Console
{
    /// <summary>
    /// Displays exception message output to the console
    /// </summary>
    public class ConsoleExceptionNotifier : IExceptionNotifier
    {
        /// <summary>
        /// Notifies the user of an exception that has occurred, by adding
        /// the error text to the console
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="furtherMessage">Any further error messages</param>
        /// <param name="title">The title</param>
        public void Notify(Exception ex, string furtherMessage, string title)
        {
            _exceptionMessage = "Error: " + furtherMessage + "Further details: "
                                + ExceptionUtilities.GetExceptionString(ex, 0, true);
            System.Console.Out.WriteLine("Error: " + furtherMessage);
            System.Console.Out.WriteLine("Further details: " + ExceptionUtilities.GetExceptionString(ex, 0, true));
        }
        private string _exceptionMessage;

        ///<summary>
        /// The last exception logged by the exception notifier
        ///</summary>
        public string ExceptionMessage
        {
            get { return _exceptionMessage; }
        }
    }
}