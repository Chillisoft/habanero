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

namespace Habanero.Base
{
    /// <summary>
    /// Provides an interface for BusinessObject rules that test the validity of
    /// a BusinessObject.  If you would like to implement your own BusinessObject
    /// rule checker, implement this interface.
    /// The BusinessObject rules for the <see cref="IBusinessObject"/> are implemented
    ///  using the GOF Strategy Pattern.
    /// </summary>
    public interface IBusinessObjectRule
    {
        /// <summary>
        /// Returns the rule name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns the error message for if the rule fails.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// The <see cref="ErrorLevel"/> for this BusinessObjectRule e.g. Warning, Error. 
        /// </summary>
        ErrorLevel ErrorLevel { get; }

        /// <summary>
        /// Indicates whether the property value is valid against the rules.
        /// Using <see cref="IsValid(IBusinessObject)"/> is the recommended 
        /// practice since this allows Habanero to optimise the Loading of BusinessObjectRules.
        /// I.e. only one rule object needs to be loaded per Business Object Type (ClassDef).
        /// If the <see cref="IsValid(IBusinessObject)"/> method is used otherwise this
        /// rule object will have to be created and loaded in memory for every single 
        /// Business Object of that Type.
        /// </summary>
        /// <returns>Returns true if valid</returns>
        [Obsolete("Using IsValid(IBusinessObject) is the recommended")]
        bool IsValid();


        /// <summary>
        /// Indicates whether the property value is valid against the rules
        /// </summary>
        /// <returns>Returns true if valid</returns>
// ReSharper disable UnusedParameter.Global
        bool IsValid(IBusinessObject bo);
// ReSharper restore UnusedParameter.Global

//        /// <summary>
//        /// Indicates whether the property value is valid against the rules
//        /// </summary>
//        /// <param name="errorMessage">A string to amend with an error
//        /// message indicating why the value might have been invalid</param>
//        /// <returns>Returns true if valid</returns>
//        bool IsValid(ref string errorMessage);
    }
}