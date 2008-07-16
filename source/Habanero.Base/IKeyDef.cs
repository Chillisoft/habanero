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

using System.Collections.Generic;


namespace Habanero.Base
{
    public interface IKeyDef: IEnumerable<IPropDef>
    {
        /// <summary>
        /// A method used by BOKey to determine whether to check for
        /// duplicate keys.  If true, then the uniqueness check will be ignored
        /// if any of the properties making up the key are null.<br/>
        /// NOTE: If the BOKey is a primary key, then this cannot be
        /// set to true.
        /// </summary>
        bool IgnoreIfNull { get; set; }

        /// <summary>
        /// Returns the key name for this key definition - this key name is built
        /// up through a combination of the key name and the property names
        /// </summary>
        string KeyName { get;  set; }

        /// <summary>
        /// Returns just the key name as given by the user
        /// </summary>
        string KeyNameForDisplay { get;  set; }

        /// <summary>
        /// Gets and sets the message to show to the user if a key validation
        /// fails.  A default message will be provided if this is null.
        /// </summary>
        string Message { get; set; }
    }
}