#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using Habanero.Base;

namespace Habanero.Test
{
    public class MockExceptionNotifier : IExceptionNotifier
    {
        private Exception _exception;
        private string _furtherMessage;
        private string _title;

        public void Notify(Exception ex, string furtherMessage, string title)
        {
            _exception = ex;
            _furtherMessage = furtherMessage;
            _title = title;
        }

        ///<summary>
        /// The last exception logged by the exception notifier
        ///</summary>
        public string ExceptionMessage
        {
            get { return _exception.Message; }
        }

        public Exception Exception
        {
            get { return _exception; }
            set { _exception = value; }
        }

        public string FurtherMessage
        {
            get { return _furtherMessage; }
            set { _furtherMessage = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
    }
}
