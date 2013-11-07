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

namespace Habanero.BO.Exceptions
{
    /// <summary>
    /// Provides an exception to throw when the object cannot be deleted due to either the 
    /// custom rules being broken for a deletion or the IsDeletable flag being set to false.
    /// </summary>
    [Serializable]
    public class BusObjEditableException : BusinessObjectException
    {
        /// <summary>
        /// Constructor to initialise the exception with details regarding the
        /// object whose record was deleted
        /// </summary>
        /// <param name="bo">The business object in question</param>
        /// <param name="message">Additional message</param>
        public BusObjEditableException(BusinessObject bo, string message)
            :
                base(
                    string.Format(
                        "You cannot Edit the '{0}', as the IsEditable is set to false for the object. " +
                        "ObjectID: {1}, also identified as {2} \n " + 
                        "Message: {3}",
                        bo.ClassDef.ClassName, bo.ID, bo, message))
        {
        }
    }
}