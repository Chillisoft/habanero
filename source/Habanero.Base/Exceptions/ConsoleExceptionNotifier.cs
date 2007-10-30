//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;

namespace Habanero.Base.Exceptions
{
    /// <summary>
    /// Displays exception message output to the console
    /// </summary>
    public class ConsoleExceptionNotifier : IExceptionNotifier
    {
        /// <summary>
        /// Constructor to initialise a new notifier
        /// </summary>
        public ConsoleExceptionNotifier()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Notifies the user of an exception that has occurred, by adding
        /// the error text to the console
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="furtherMessage">Any further error messages</param>
        /// <param name="title">The title</param>
        public void Notify(Exception ex, string furtherMessage, string title)
        {
            Console.Out.WriteLine("Error: " + furtherMessage);
            Console.Out.WriteLine("Further details: " + ExceptionUtilities.GetExceptionString(ex, 0, true));
        }
    }
}