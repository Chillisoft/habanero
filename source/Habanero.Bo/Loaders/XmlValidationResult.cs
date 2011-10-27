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
using System.Collections.Generic;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Represents the result of an Xml validation check, containing a boolean (<see cref="IsValid"/>) flag 
    /// indicating validity and a list of <see cref="Messages"/> containing any validation error messages.
    /// </summary>
    
    public class XmlValidationResult
    {
        private readonly List<string> _messages;

        /// <summary>
        /// Constructs a validation result.
        /// </summary>
        /// <param name="isValid">Whether the validation was successful</param>
        /// <param name="messages">Any messages that have arisen in the validation.</param>
        public XmlValidationResult(bool isValid, List<string> messages)
        {
            _messages = messages;
            IsValid = isValid;
        }

        /// <summary>
        /// Whether the validation was successful
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Any messages that have arisen in the validation.
        /// </summary>
        public List<string> Messages { get { return _messages; } }
    }
}