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
using Habanero.Base;

namespace Habanero.BO
{
    /// <summary>
    /// An Error that has occured on a <see cref="IBusinessObject"/>.
    /// </summary>
    public class BOError : IBOError
    {
       /// <summary>
       /// the constructor for a <see cref="BOError"/> 
       /// </summary>
       /// <param name="message">The error message that is being shown to the user</param>
       /// <param name="level">The warning/Error Level (<see cref="ErrorLevel"/> of the Error</param>
        public BOError(string message, ErrorLevel level)
        {
            Message = message;
            Level = level;
        }

        /// <summary>
        /// The Business Object that the error occured on.
        /// </summary>
        public IBusinessObject BusinessObject { get; set; }

        /// <summary>
        /// The <see cref="ErrorLevel"/> of the business object.
        /// </summary>
        public ErrorLevel Level { get; private set; }

        /// <summary>
        /// The Message to be shown to the End user for a particular error message.
        /// </summary>
        public string Message { get; private set; }
    }
}