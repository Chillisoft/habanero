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
    /// An interface to model a generator of unique numbers. It keeps
    /// record of the last number retrieved and can provide a new
    /// unique number on request.  An example usage might be a till
    /// receipt number for a retail business.  Incrementing the numbers
    /// as they are dispensed is one means of achieving uniqueness.
    /// </summary>
    /// <remarks>
    /// This number generator does not implement concurrency control of any type.
    /// It is susceptible to both lost number and to multiple users generating the 
    /// same number. If either of these scenarious exist in your application then
    /// please use one of the sub types of <see cref="INumberGenerator"/>. 
    /// </remarks>
    public interface IDBNumberGenerator
    {
        /// <summary>
        /// Returns the next available unique number. One possible means
        /// of providing unique numbers is simply to increment the last one
        /// dispensed.
        /// </summary>
        /// <returns>Returns an integer</returns>
        int GetNextNumberInt();

        /// <summary>
        /// Creates a database transaction that updates the database to the
        /// last number dispensed, so the next number dispensed will be a
        /// fresh increment
        /// </summary>
        /// <returns>Returns an ITransactional object</returns>
        ITransactional GetUpdateTransaction();
    }
}