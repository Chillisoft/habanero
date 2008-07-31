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

using System;

namespace Habanero.Base
{
    public interface IPrimaryKey : IBOKey
    {
        /// <summary>
        /// Returns the object's ID
        /// </summary>
        /// <returns>Returns a string</returns>
        string GetObjectId();

        /// <summary>
        /// Returns the object ID as if the object had been persisted 
        /// regardless of whether the object is new or not
        /// </summary>
        /// <returns>Returns a string</returns>
        string GetObjectNewID();

        /// <summary>
        /// Get the original ObjectID
        /// </summary>
        /// <returns>Returns a string</returns>
        string GetOrigObjectID();

        /// <summary>
        /// Returns the ID as a Guid
        /// </summary>
        /// <returns>Returns a Guid</returns>
        Guid GetAsGuid();

        /// <summary>
        /// Sets the object's ID
        /// </summary>
        /// <param name="id">The ID to set to</param>
        void SetObjectID(Guid id);
    }
}