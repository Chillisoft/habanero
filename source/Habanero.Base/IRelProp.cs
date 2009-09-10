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

namespace Habanero.Base
{
    /// <summary>
    /// Represents the property on which two objects match up in a relationship
    /// </summary>
    public interface IRelProp
    {
        /// <summary>
        /// Returns the property name of the relationship owner
        /// </summary>
        string OwnerPropertyName
        {
            get;
        }

        /// <summary>
        /// Returns the property name of the related object
        /// </summary>
        string RelatedClassPropName
        {
            get;
        }

        /// <summary>
        /// The BoProp this RelProp requires to generate its search expression
        /// </summary>
        IBOProp BOProp
        {
            get;
        }

        /// <summary>
        /// Indicates if the property is null
        /// </summary>
        bool IsNull { get; }

        /// <summary>
        /// The event that is raised when the <see cref="IRelProp.BOProp"/>'s value is changed
        /// </summary>
        event EventHandler PropValueUpdated;
    }
}