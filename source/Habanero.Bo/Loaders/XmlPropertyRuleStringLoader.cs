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
    /// Loads a string property rule from xml data
    /// </summary>
    public class XmlPropertyRuleStringLoader : XmlPropertyRuleLoader
    {
        private int _minLength;
        private int _maxLength;
		private string _patternMatch;
		private string _patternMatchErrorMesssage;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
		public XmlPropertyRuleStringLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlPropertyRuleStringLoader()
        {
        }

        /// <summary>
        /// Creates the property rule object from the data already loaded
        /// </summary>
        /// <returns>Returns a PropRuleString object</returns>
        protected override object Create()
        {
			return _defClassFactory.CreatePropRuleString(_ruleName, _isCompulsory, _minLength, _maxLength, _patternMatch, _patternMatchErrorMesssage);
            //return new PropRuleString(_name, _isCompulsory, _minLength, _maxLength);
        }

        /// <summary>
        /// Loads the property rule from the reader
        /// </summary>
        protected override void LoadPropertyRuleFromReader()
        {
			_patternMatch = _reader.GetAttribute("patternMatch");
			_patternMatchErrorMesssage = _reader.GetAttribute("patternMatchErrorMessage");
			try
            {
                _minLength = Convert.ToInt32(_reader.GetAttribute("minLength"));
                _maxLength = Convert.ToInt32(_reader.GetAttribute("maxLength"));
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException("In a 'propertyRuleString' " +
                    "element, the 'minLength' or 'maxLength' was not set to a valid " +
                    "integer format.", ex);
            }
        }
    }
}