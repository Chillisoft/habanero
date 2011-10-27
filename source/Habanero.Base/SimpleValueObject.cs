// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
namespace Habanero.Base
{
    /// <summary>
    /// A super class for a simple value object (i.e. a value object that consists of only one property.
    /// </summary>
    public abstract class SimpleValueObject: CustomProperty
    {
        /// <summary>
        /// Constructor to initialise the property
        /// </summary>
        /// <param name="value">The value to customise</param>
        /// <param name="isLoading">Whether the value is being loaded from
        /// the database, rather than being prepared to send to the database.
        /// This might determine whether the object is in its normal or
        /// customised form.
        /// </param>
        public SimpleValueObject(object value, bool isLoading):base(value, isLoading)
        {
        }

        /// <summary>
        /// Provides a base method for implementing rules to determine whether this object is valid or not.
        /// </summary>
        /// <returns></returns>
        public abstract Result IsValid();

    }
}