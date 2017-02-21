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
using Habanero.Base;
using Habanero.BO.Rules;

namespace Habanero.Testability
{
    /// <summary>
    /// An inverface for Objects that are of type that is generally numeric.
    /// e.g. DateTime, Single, Double, Decimal, Int, Long.
    /// This interface provides an interface for generating a Random Number that is Greater
    /// than or less than a certain value. This is primarily used
    /// for generating valid values where <see cref="InterPropRule"/>s exist.
    /// The Methods will generate a valid value taking the <see cref="IPropRule"/> and the
    /// minValue/maxValue derived from the <see cref="InterPropRule"/> into account.
    /// </summary>
    public interface IValidValueGeneratorNumeric
    {
        /// <summary>
        /// Generates a Valid Value taking <see cref="IPropRule"/> and minValue into 
        /// account.
        /// </summary>
        /// <param name="minValue"></param>
        /// <returns></returns>
        object GenerateValidValueGreaterThan(object minValue);
        /// <summary>
        /// Generates a Valid Value taking <see cref="IPropRule"/> and maxValue into 
        /// account.
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        object GenerateValidValueLessThan(object maxValue);
    }
}
