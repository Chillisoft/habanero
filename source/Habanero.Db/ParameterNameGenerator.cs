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
using System;
using Habanero.Base;

namespace Habanero.DB
{
    /// <summary>
    /// Generates parameter names for parameterised sql statements
    /// </summary>
    [Serializable]
    public class ParameterNameGenerator : IParameterNameGenerator
    {
        private int _number;
        private const string _parameterNameBase = "Param";


        ///<summary>Constructor. You can specify the prefix character/string the name generator should use (this varies from DBMS to DBMS)
        ///</summary>
        ///<param name="prefixCharacter">The string to start each parameter name with</param>
        public ParameterNameGenerator(string prefixCharacter)
        {
            PrefixCharacter = prefixCharacter;
        }

        /// <summary>
        /// The prefix character used. For example, setting this to "?" will make parameters called "?Param0", "?Param1" etc.
        /// </summary>
        public string PrefixCharacter { get; protected set; }

        /// <summary>
        /// Generates a parameter name with the current seed value and
        /// increments the seed
        /// </summary>
        /// <returns>Returns a string</returns>
        public virtual string GetNextParameterName()
        {
            return PrefixCharacter + _parameterNameBase + _number++;
        }

        /// <summary>
        /// Sets the parameter count back to zero
        /// </summary>
        public void Reset()
        {
            _number = 0;
        }
    }
}