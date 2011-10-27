//  ---------------------------------------------------------------------------------
//   Copyright (C) 2007-2010 Chillisoft Solutions
//   
//   This file is part of the Habanero framework.
//   
//       Habanero is a free framework: you can redistribute it and/or modify
//       it under the terms of the GNU Lesser General Public License as published by
//       the Free Software Foundation, either version 3 of the License, or
//       (at your option) any later version.
//   
//       The Habanero framework is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU Lesser General Public License for more details.
//   
//       You should have received a copy of the GNU Lesser General Public License
//       along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//  ---------------------------------------------------------------------------------
using System.Xml;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Loads UI grid definitions from xml data
    /// </summary>
    public class XmlUIGridLoader : XmlLoader
    {
        private IUIGrid _uiGrid;


        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlUIGridLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Loads a grid definition from the xml string provided
        /// </summary>
        /// <param name="formDefElement">The xml string</param>
        /// <returns>Returns a UIGridDef object</returns>
        public IUIGrid LoadUIGridDef(string formDefElement)
        {
            return this.LoadUIGridDef(this.CreateXmlElement(formDefElement));
        }

        /// <summary>
        /// Loads a grid definition from the xml element provided
        /// </summary>
        /// <param name="formDefElement">The xml element</param>
        /// <returns>Returns a UIGridDef object</returns>
        public IUIGrid LoadUIGridDef(XmlElement formDefElement)
        {
            return (IUIGrid) this.Load(formDefElement);
        }

        /// <summary>
        /// Creates a grid definition from the data already loaded
        /// </summary>
        /// <returns>Returns a UIGridDef object</returns>
        protected override object Create()
        {
            return _uiGrid;
        }

        /// <summary>
        /// Loads grid definition data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
			_uiGrid = _defClassFactory.CreateUIGridDef();

            _reader.Read();
            _uiGrid.SortColumn = _reader.GetAttribute("sortColumn");

            _reader.Read();

            if (_reader.Name == "filter")
            {
                XmlFilterLoader filterLoader = new XmlFilterLoader(DtdLoader, _defClassFactory);
                _uiGrid.FilterDef = filterLoader.LoadFilterDef(_reader.ReadOuterXml());
            }

            while (_reader.Name == "column")
            {
                XmlUIGridColumnLoader propLoader = new XmlUIGridColumnLoader(DtdLoader, _defClassFactory);
                _uiGrid.Add(propLoader.LoadUIProperty(_reader.ReadOuterXml()));
            }

            if (_uiGrid.Count == 0)
            {
                throw new InvalidXmlDefinitionException("No 'column' " +
                    "elements were specified in a 'grid' element.  Ensure " +
                    "that the element " +
                    "contains one or more 'column' elements, which " +
                    "specify the columns to appear in the grid.");
            }
        }
    }
}