// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using System.Xml;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Loads UI form column information from xml data
    /// </summary>
    public class XmlUIFormColumnLoader : XmlLoader
    {
        private IUIFormColumn _column;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlUIFormColumnLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Loads a form column definition from the xml string provided
        /// </summary>
        /// <param name="formColumnElement">The xml string</param>
        /// <returns>Returns a UIFormColumn object</returns>
        public IUIFormColumn LoadUIFormColumn(string formColumnElement)
        {
            return this.LoadUIFormColumn(this.CreateXmlElement(formColumnElement));
        }

        /// <summary>
        /// Loads a form column definition from the xml element provided
        /// </summary>
        /// <param name="formColumnElement">The xml element</param>
        /// <returns>Returns a UIFormColumn object</returns>
        public IUIFormColumn LoadUIFormColumn(XmlElement formColumnElement)
        {
            return (IUIFormColumn) this.Load(formColumnElement);
        }

        /// <summary>
        /// Creates a form column definition from the data already loaded
        /// </summary>
        /// <returns>Returns a UIFormColumn object</returns>
        protected override object Create()
        {
            return _column;
        }

        /// <summary>
        /// Loads form column data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
			_column = _defClassFactory.CreateUIFormColumn();
			//_column = new UIFormColumn();

            //_reader.Read();
            //string className = _reader.GetAttribute("class");
            //string assemblyName = _reader.GetAttribute("assembly");
            //_collection.Class = TypeLoader.LoadType(assemblyName, className);
            //_collection.Name = new UIPropertyCollectionName(_collection.Class, _reader.GetAttribute("name"));

            _reader.Read();
            try
            {
                _column.Width = Convert.ToInt32(_reader.GetAttribute("width"));
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException("In a 'columnLayout' " + 
                    "element, the 'width' attribute has been given " +
                    "an invalid integer pixel value.", ex);
            }

            _reader.Read();
            while (_reader.Name == "field")
            {
                XmlUIFormFieldLoader propLoader = new XmlUIFormFieldLoader(DtdLoader, _defClassFactory);
                _column.Add(propLoader.LoadUIProperty(_reader.ReadOuterXml()));
            }

            if (_column.Count == 0)
            {
                throw new InvalidXmlDefinitionException("No 'field' " +
                    "elements were specified in a 'columnLayout' element.  Ensure " +
                    "that the element " +
                    "contains one or more 'field' elements, which " +
                    "specify the property editing controls to appear in the " +
                    "editing form column.");
            }
        }
    }
}