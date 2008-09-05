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
    /// <summary>
    /// Holds a collection of properties on which two classes in a relationship
    /// are matching
    /// </summary>
    public interface IRelKey : IEnumerable<IRelProp>
    {
        ///<summary>
        /// Gets the number of properties in this relationship key.
        ///</summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// Indexes the array of relprops this relkey contains.
        /// </summary>
        /// <param name="index">The position of the relprop to get</param>
        /// <returns>Returns the RelProp object found with that name</returns>
        IRelProp this[int index]
        {
            get;
        }
    }
}