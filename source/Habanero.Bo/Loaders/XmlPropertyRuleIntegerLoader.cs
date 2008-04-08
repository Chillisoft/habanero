//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using Habanero.Bo;
using Habanero.Bo.ClassDefinition;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.Bo.Loaders
{
    /// <summary>
    /// Loads an integer property rule from xml data
    /// </summary>
    public class XmlPropertyRuleIntegerLoader : XmlPropertyRuleLoader
    {
        private int _minValue;
        private int _maxValue;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlPropertyRuleIntegerLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlPropertyRuleIntegerLoader()
        {
        }

        /// <summary>
        /// Creates the property rule object from the data already loaded
        /// </summary>
        /// <returns>Returns a PropRuleInteger object</returns>
        protected override object Create()
        {
			return _defClassFactory.CreatePropRuleInteger(_ruleName, _isCompulsory, _minValue, _maxValue);
			//return new PropRuleInteger(_name, _isCompulsory, _minValue, _maxValue);
        }

        /// <summary>
        /// Loads the property rule from the reader
        /// </summary>
        protected override void LoadPropertyRuleFromReader()
        {
            try
            {
                if (_reader.GetAttribute("minValue") != "")
                {
                    _minValue = Convert.ToInt32(_reader.GetAttribute("minValue"));
                }
                else
                {
                    _minValue = int.MinValue;
                }
                if (_reader.GetAttribute("maxValue") != "")
                {
                    _maxValue = Convert.ToInt32(_reader.GetAttribute("maxValue"));
                }
                else
                {
                    _maxValue = int.MaxValue;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException("In a " +
                    "'PropertyRuleInteger' element, either the 'minValue' or " +
                    "'maxValue' attribute was set to an invalid integer value.", ex);
            }
        }
    }
}