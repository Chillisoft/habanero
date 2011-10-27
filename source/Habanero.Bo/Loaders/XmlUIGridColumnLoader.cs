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
using System;
using System.Collections;
using System.Xml;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.Base.Logging;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Loads UI grid property definitions from xml data
    /// </summary>
    public class XmlUIGridColumnLoader : XmlLoader
    {
        protected static readonly IHabaneroLogger _logger = GlobalRegistry.LoggerFactory.GetLogger(typeof(XmlUIGridColumnLoader));
        private string _heading;
        private string _propertyName;
//        private Type _gridControlType;
        private bool _editable;
        private int _width;
        private PropAlignment _alignment;
        private Hashtable _propertyAttributes;
        private string  _assemblyName;
        private string _className;


        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlUIGridColumnLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }


        /// <summary>
        /// Loads a grid property definition from the xml string provided
        /// </summary>
        /// <param name="xmlUIProp">The xml string</param>
        /// <returns>Returns a UIGridProperty object</returns>
        public IUIGridColumn LoadUIProperty(string xmlUIProp)
        {
            return this.LoadUIProperty(this.CreateXmlElement(xmlUIProp));
        }

        /// <summary>
        /// Loads a grid property definition from the xml element provided
        /// </summary>
        /// <param name="uiPropElement">The xml element</param>
        /// <returns>Returns a UIGridProperty object</returns>
        public IUIGridColumn LoadUIProperty(XmlElement uiPropElement)
        {
            return (IUIGridColumn) Load(uiPropElement);
        }

        /// <summary>
        /// Creates a grid property definition from the data already loaded
        /// </summary>
        /// <returns>Returns a UIGridProperty object</returns>
        protected override object Create()
        {
			return _defClassFactory.CreateUIGridProperty(_heading, _propertyName,
                _className ,_assemblyName, _editable, _width, _alignment, _propertyAttributes);
			//return
			//    new UIGridProperty(_heading, _propertyName, _gridControlType, _editable, _width,
			//                       _alignment);
        }

        /// <summary>
        /// Loads grid property definition data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            _reader.Read();
            LoadPropertyName();
            LoadHeading();
            LoadGridControlType();
            LoadIsEditable();
            LoadWidth();
            LoadAlignment();
            LoadParameters();
        }

        /// <summary>
        /// Loads the "isReadOnly" attribute from the reader. This method
        /// is called by LoadFromReader().
        /// </summary>
        private void LoadIsEditable()
        {
            try
            {
                _editable = Convert.ToBoolean(_reader.GetAttribute("editable"));
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException("The 'editable' attribute " +
                    "in a 'column' element is invalid. The valid options " +
                    "are 'true' and 'false'.", ex);
            }
        }

        /// <summary>
        /// Loads the grid control type name from the reader. This method
        /// is called by LoadFromReader().
        /// </summary>
        private void LoadGridControlType()
        {
              _assemblyName = _reader.GetAttribute("assembly");
              _className = _reader.GetAttribute("type");



            //if (_assemblyName == null || _assemblyName.Length == 0)
            //{
            //    if (_className == "DataGridViewTextBoxColumn" || _className == "DataGridViewCheckBoxColumn" ||
            //        _className == "DataGridViewComboBoxColumn")
            //    {
            //        _assemblyName = "System.Windows.Forms";
            //    }
            //    else
            //    {
            //        _assemblyName = "Habanero.UI";
            //        _className = "Habanero.UI.Grid." + _className;
            //    }
            //}
            ////log.Debug("assembly: " + _assemblyName + ", class: " + _className) ;
            //try
            //{
            //    _gridControlType = TypeLoader.LoadType(_assemblyName, _className);
            //}
            //catch (Exception ex)
            //{
            //    throw new InvalidXmlDefinitionException(String.Format(
            //        "In a 'column' element, the grid column type could not be loaded, " +
            //        "with the 'type' given as '{0}' and the assembly as '{1}'. " +
            //        "See the documentation for available types.",
            //        _className, _assemblyName), ex);
            //}
        }

        /// <summary>
        /// Loads the property name from the reader. This method
        /// is called by LoadFromReader().
        /// </summary>
        private void LoadPropertyName()
        {
            _propertyName = _reader.GetAttribute("property");
            if (string.IsNullOrEmpty(_propertyName))
            {
                throw new InvalidXmlDefinitionException("In a 'column' " +
                    "element, the 'property' attribute was not specified. This " +
                    "attribute specifies which property of the class to display " +
                    "in the column.");
            }
        }

        /// <summary>
        /// Loads the heading from the reader. This method
        /// is called by LoadFromReader().
        /// </summary>
        private void LoadHeading()
        {
            _heading = _reader.GetAttribute("heading");
        }

        /// <summary>
        /// Loads the width from the reader. This method
        /// is called by LoadFromReader().
        /// </summary>
        private void LoadWidth()
        {
            try
            {
                _width = Convert.ToInt32(_reader.GetAttribute("width"));
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException("In the 'width' attribute " +
                    "of a 'column' element, the value provided was " +
                    "invalid.  This should be an integer value in pixels.", ex);
            }
        }

        /// <summary>
        /// Loads the alignment attribute from the reader. This method
        /// is called by LoadFromReader().
        /// </summary>
        private void LoadAlignment()
        {
            string alignmentStr = Convert.ToString(_reader.GetAttribute("alignment"));
            if (alignmentStr == "left")
            {
                _alignment = PropAlignment.left;
            }
            else if (alignmentStr == "right")
            {
                _alignment = PropAlignment.right;
            }
            else
            {
                _alignment = PropAlignment.centre;
            }
        }

        /// <summary>
        /// Loads the attributes from the reader. This method is
        /// called by LoadFromReader().
        /// </summary>
        private void LoadParameters()
        {
            _propertyAttributes = new Hashtable();
            //System.Console.WriteLine(_reader.Name);
            _reader.Read();
            //System.Console.WriteLine(_reader.Name);

            while (_reader.Name == "parameter")
            {
                string attName = _reader.GetAttribute("name");
                string attValue = _reader.GetAttribute("value");
                if (string.IsNullOrEmpty(attName) ||
                    string.IsNullOrEmpty(attValue))
                {
                    throw new InvalidXmlDefinitionException("In a " +
                        "'parameter' element, either the 'name' or " +
                        "'value' attribute has been omitted.");
                }

                try
                {
                    _propertyAttributes.Add(attName, attValue);
                }
                catch (Exception ex)
                {
                    throw new InvalidXmlDefinitionException("An error occurred " +
                        "while loading a 'parameter' element.  There may " +
                        "be duplicates with the same 'name' attribute.", ex);
                }
                ReadAndIgnoreEndTag();
            }
        }
    }
}