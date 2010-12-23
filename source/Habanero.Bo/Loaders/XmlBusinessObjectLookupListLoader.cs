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
using Habanero.Base.Exceptions;
using Habanero.Base.Util;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Loads xml data for a lookup list in a business object
    /// </summary>
    public class XmlBusinessObjectLookupListLoader : XmlLookupListLoader
    {
        //private Type _type;
    	private string _className;
    	private string _assemblyName;
        private string _criteria;
        private string _sort;
        private int _timeout;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlBusinessObjectLookupListLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Loads the lookup list data from the reader
        /// </summary>
        protected override void LoadLookupListFromReader()
        {
            _className = _reader.GetAttribute("class");
            _assemblyName = _reader.GetAttribute("assembly");
            //_type = TypeLoader.LoadType(assemblyName, className);
            _criteria = _reader.GetAttribute("criteria");
            _sort = _reader.GetAttribute("sort");

            try
            {
                _timeout = Int32.Parse(XmlHelpersCF.GetAttributeOrDefault(_reader, "timeout", "10000"));
            }
            catch
            {
                _timeout = -1;
            }

            if (_timeout<0)
            {
                throw new InvalidXmlDefinitionException("In a 'businessObjectLookupList' " +
                    "element, an invalid integer was assigned to the 'timeout' " +
                    "attribute.  The value must be a positive integer or zero.");
            }

            //if (!Int32.TryParse(_reader.GetAttribute("timeout"), out _timeout) ||
            //    _timeout < 0)
            //{
            //    throw new InvalidXmlDefinitionException("In a 'businessObjectLookupList' " +
            //        "element, an invalid integer was assigned to the 'timeout' " +
            //        "attribute.  The value must be a positive integer or zero.");
            //}
        }

        /// <summary>
        /// Creates a business object lookup list data source from the
        /// data already read in
        /// </summary>
        /// <returns>Returns a BusinessObjectLookupList object</returns>
        protected override object Create()
        {
			return _defClassFactory.CreateBusinessObjectLookupList(_assemblyName, _className, _criteria, _sort, _timeout);
		}
    }
}
