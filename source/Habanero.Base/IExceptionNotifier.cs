//  ---------------------------------------------------------------------------------
//   Copyright (C) 2007-2010 Chillisoft Solutions
//   
//   This file is part of the Habanero framework.
//   
//       Habanero is a free framework: you can redistribute it and/or modify
//       it under the terms of the GNU Lesser General Public License as published by
//       the Free Software Foundation, either version 3 of the License, or
//       (at your option) any later version.
//   
//       The Habanero framework is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU Lesser General Public License for more details.
//   
//       You should have received a copy of the GNU Lesser General Public License
//       along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//  ---------------------------------------------------------------------------------
using System;

namespace Habanero.Base
{
    /// <summary>
    /// An interface to model a tool that notifies the user of an exception
    /// </summary>
    public interface IExceptionNotifier
    {
        /// <summary>
        /// Notifies the user of an exception that has occurred
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="furtherMessage">Any further error messages</param>
        /// <param name="title">The title</param>
        void Notify(Exception ex, string furtherMessage, string title);

        ///<summary>
        /// The last exception logged by the exception notifier
        ///</summary>
        string ExceptionMessage { get; }
    }
}