//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Loads xml data for a lookup list
    /// </summary>
    public class XmlSimpleLookupListLoader : XmlLookupListLoader
    {
        private Dictionary<string, object> _displayValueDictionary;

        ///// <summary>
        ///// Constructor to initialise a loader
        ///// </summary>
        //public XmlSimpleLookupListLoader() : this("", null)
        //{
        //}

        /// <summary>
        /// Constructor to initialise a loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlSimpleLookupListLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
            _displayValueDictionary = new Dictionary<string, object>();
        }

        /// <summary>
        /// Loads the lookup list data from the reader
        /// </summary>
        protected override void LoadLookupListFromReader()
        {
            string options = _reader.GetAttribute("options");
            if (options != null && options.Length > 0) {
                string[] optionsArr = options.Split(new char[] {'|'});
                foreach (string s in optionsArr) {
                    _displayValueDictionary.Add(s, s);
                }
            }
            _reader.Read();
            while (_reader.Name == "item")
            {
                string stringPart = _reader.GetAttribute("display");
                string valuePart = _reader.GetAttribute("value");
                if (stringPart == null || stringPart.Length == 0)
                {
                    throw new InvalidXmlDefinitionException("An 'item' " +
                        "is missing a 'display' attribute that specifies the " +
                        "string to show to the user in a display.");
                }
                if (valuePart == null || valuePart.Length == 0)
                {
                    throw new InvalidXmlDefinitionException("An 'item' " +
                        "is missing a 'value' attribute that specifies the " +
                        "value to store for the given property.");
                }
            	
				Guid newGuid;
            	if (StringUtilities.GuidTryParse(valuePart, out newGuid))
            	{
					_displayValueDictionary.Add(stringPart, newGuid);
            	} else
            	{
					_displayValueDictionary.Add(stringPart, valuePart);
            	}
                
                ReadAndIgnoreEndTag();
            }

            if (_displayValueDictionary.Count == 0)
            {
                throw new InvalidXmlDefinitionException("A 'simpleLookupList' " +
                    "element does not contain any 'item' elements or any items in the 'options' attribute.  It " +
                    "should contain one or more 'item' elements or one or more | separated options in the 'options' attribute that " +
                    "specify each of the available options in the lookup list.");
            }
        }

        /// <summary>
        /// Creates a lookup list data source from the data already read in
        /// </summary>
        /// <returns>Returns a SimpleLookupList object</returns>
        protected override object Create()
        {
			return _defClassFactory.CreateSimpleLookupList(_displayValueDictionary);
			//return new SimpleLookupList(_displayValueDictionary);
        }
    }
}