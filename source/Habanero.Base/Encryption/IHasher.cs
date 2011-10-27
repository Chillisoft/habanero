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
namespace Habanero.Base
{
    /// <summary>
    /// Interface for classes that provide a hashing facility.
    /// </summary>
    public interface IHasher
    {
        /// <summary>
        /// Returns a hash created out the string given. Each time the string is Hashed it will return the same hash. This is a one way encryption mechanism.
        /// </summary>
        /// <param name="value">The string to hash</param>
        /// <returns>The hash</returns>
        string HashString(string value);
    }
}