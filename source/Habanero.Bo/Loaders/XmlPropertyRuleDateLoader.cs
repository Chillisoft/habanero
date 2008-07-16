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
using Habanero.Bo;
using Habanero.Bo.ClassDefinition;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.Bo.Loaders
{
    /// <summary>
    /// Loads a date property rule from xml data
    /// </summary>
    public class XmlPropertyRuleDateLoader : XmlPropertyRuleLoader
    {
        private DateTime? _minValue;
        private DateTime? _maxValue;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlPropertyRuleDateLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlPropertyRuleDateLoader()
        {
        }

        /// <summary>
        /// Creates the property rule object from the data already loaded
        /// </summary>
        /// <returns>Returns a PropRuleDate object</returns>
        protected override object Create()
        {
			return _defClassFactory.CreatePropRuleDate(_ruleName, _isCompulsory, _minValue, _maxValue);
			//if (!_minValue.Equals(DateTime.MinValue))
			//{
			//    return new PropRuleDate(_name, _isCompulsory, _minValue, _maxValue);
			//}
			//else
			//{
			//    return new PropRuleDate(_name, _isCompulsory);
			//}
        }

        /// <summary>
        /// Loads the property rule from the reader
        /// </summary>
        protected override void LoadPropertyRuleFromReader()
        {
			_minValue = readDateTimeAttribute("minValue");
			_maxValue = readDateTimeAttribute("maxValue");
			//try
			//{
			//    if (_reader.GetAttribute("minValue") != null)
			//    {
			//        _minValue = Convert.ToDateTime(_reader.GetAttribute("minValue"));
			//        _maxValue = Convert.ToDateTime(_reader.GetAttribute("maxValue"));
			//    }
			//}
			//catch (Exception ex)
			//{
			//    throw new InvalidXmlDefinitionException("In a 'propertyRuleDate' " +
			//        "element, the 'minValue' or 'maxValue' was not set to a valid " +
			//        "date format. A typical date string would be in the form of " +
			//        "yyyy/mm/dd.", ex);
			//}
        }

		private DateTime? readDateTimeAttribute(string attributeName)
		{
			try
			{
				string attributeValue = _reader.GetAttribute(attributeName);
				if (attributeValue != null)
				{
					return Convert.ToDateTime(attributeValue);
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				throw new InvalidXmlDefinitionException(String.Format(
					"In a 'propertyRuleDate' element, the '{0}' " +
					"was not set to a valid date format. A typical date " +
					"string would be in the form of yyyy/mm/dd.", attributeName), ex);
			}
		}
    }
}