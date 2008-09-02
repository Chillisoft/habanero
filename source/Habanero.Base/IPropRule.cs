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
    /// <summary>
    /// Provides an interface for property rules that test the validity of
    /// a property value.  If you would like to implement your own property
    /// rule checker, implement this interface or inherit from PropRuleBase.
    /// In the class definitions, in the 'rule'
    /// element under the relevant 'property', specify the class and assembly
    /// of your newly implemented class.
    /// The Property rules for the <see cref="IBusinessObject"/> are implemented
    ///  using the GOF Strategy Pattern.
    /// </summary>
    public interface IPropRule
    {
        /// <summary>
        /// Returns the rule name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Returns the error message for if the rule fails.
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Indicates whether the property value is valid against the rules
        /// </summary>
        /// <param name="displayName">The property name being checked</param>
        /// <param name="propValue">The value to check</param>
        /// <param name="errorMessage">A string to amend with an error
        /// message indicating why the value might have been invalid</param>
        /// <returns>Returns true if valid</returns>
        bool IsPropValueValid(string displayName, Object propValue,
            ref string errorMessage);
    }
}