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
    /// Generates a valid value for PropDef of type DateTime.
    /// </summary>
    public class ValidValidGeneratorByteArray : ValidValueGenerator
    {
        public ValidValidGeneratorByteArray(IPropDef propDef) : base(propDef)
        {
        }

        /// <summary>
        /// Generates a valid value taking into account only the <see cref="IPropRule"/>s. I.e. any <see cref="InterPropRule"/>s 
        /// will not be taken into account. The <see cref="IValidValueGeneratorNumeric"/>'s methods are used
        /// by the BOTestFactory to create valid values taking into account InterPropRules
        /// </summary>
        /// <returns></returns>
        public override object GenerateValidValue()
        {
            return new byte[200];
        }
    }
}