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
using System.Collections;
using System.Xml;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Util.File;
using log4net;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Loads UI grid property definitions from xml data
    /// </summary>
    public class XmlUIGridColumnLoader : XmlLoader
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.Loaders.XmlUIGridColumnLoader");
        private string _heading;
        private string _propertyName;
        private Type _gridControlType;
        private bool _editable;
        private int _width;
        private UIGridColumn.PropAlignment _alignment;
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
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlUIGridColumnLoader()
        {
        }

        /// <summary>
        /// Loads a grid property definition from the xml string provided
        /// </summary>
        /// <param name="xmlUIProp">The xml string</param>
        /// <returns>Returns a UIGridProperty object</returns>
        public UIGridColumn LoadUIProperty(string xmlUIProp)
        {
            return this.LoadUIProperty(this.CreateXmlElement(xmlUIProp));
        }

        /// <summary>
        /// Loads a grid property definition from the xml element provided
        /// </summary>
        /// <param name="uiPropElement">The xml element</param>
        /// <returns>Returns a UIGridProperty object</returns>
        public UIGridColumn LoadUIProperty(XmlElement uiPropElement)
        {
            return (UIGridColumn) Load(uiPropElement);
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
            LoadIsReadOnly();
            LoadWidth();
            LoadAlignment();
            LoadParameters();
        }

        /// <summary>
        /// Loads the "isReadOnly" attribute from the reader. This method
        /// is called by LoadFromReader().
        /// </summary>
        private void LoadIsReadOnly()
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
            if (_propertyName == null || _propertyName.Length == 0)
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
                _alignment = UIGridColumn.PropAlignment.left;
            }
            else if (alignmentStr == "right")
            {
                _alignment = UIGridColumn.PropAlignment.right;
            }
            else
            {
                _alignment = UIGridColumn.PropAlignment.centre;
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
                if (attName == null || attName.Length == 0 ||
                    attValue == null || attValue.Length == 0)
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