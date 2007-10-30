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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Util;

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
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlBusinessObjectLookupListLoader()
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
        }

        /// <summary>
        /// Creates a business object lookup list data source from the
        /// data already read in
        /// </summary>
        /// <returns>Returns a BusinessObjectLookupList object</returns>
        protected override object Create()
        {
			return _defClassFactory.CreateBusinessObjectLookupList(_assemblyName, _className, _criteria);
			//return new BusinessObjectLookupList(_assemblyName, _className);
			//return new BusinessObjectLookupList(_type);
		}
    }
}
