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
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Loads lookup list data from the reader, using sql information as
    /// specified in the reader
    /// </summary>
    public class XmlDatabaseLookupListLoader : XmlLookupListLoader
    {
        private string _sqlString;
        private int _timeout;
    	private string _assemblyName;
    	private string _className;

        /// <summary>
        /// Constructor to initialise a loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlDatabaseLookupListLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Constructor to initialise a loader
        /// </summary>
        public XmlDatabaseLookupListLoader()
        {
        }

        /// <summary>
        /// Loads the lookup list data from the reader, using the sql string, time-out
        /// class name and assembly name specified in the reader
        /// </summary>
        protected override void LoadLookupListFromReader()
        {
            _sqlString = _reader.GetAttribute("sql");
            _className = _reader.GetAttribute("class");
            _assemblyName = _reader.GetAttribute("assembly");

            if (!Int32.TryParse(_reader.GetAttribute("timeout"), out _timeout) ||
                _timeout < 0)
            {
                throw new InvalidXmlDefinitionException("In a 'databaseLookupList' " +
                    "element, an invalid integer was assigned to the 'timeout' " +
                    "attribute.  The value must be a positive integer or zero.");
            }

        }

        /// <summary>
        /// Creates a database lookup list data source from the
        /// data already read in
        /// </summary>
        /// <returns>Returns a DatabaseLookupList object</returns>
        protected override object Create()
        {
			return _defClassFactory.CreateDatabaseLookupList(_sqlString, _timeout,_assemblyName, _className);
			//return new DatabaseLookupList(_sqlString, _assemblyName, _className);
		}
    }
}