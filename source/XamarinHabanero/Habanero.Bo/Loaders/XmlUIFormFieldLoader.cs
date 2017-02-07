#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using System.Collections;
using System.Xml;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.Loaders
{
#pragma warning disable 612,618
    /// <summary>
    /// Loads UI form property information from xml data
    /// </summary>
    public class XmlUIFormFieldLoader : XmlLoader
    {
        private string _label;
        private string _propertyName;
		private string _mapperTypeName;
		private string _mapperTypeAssembly;
		private string _controlTypeName;
		private string _controlAssembly;
        private bool _editable;
        private Hashtable _parameters;
        private string _toolTipText;
        private LayoutStyle _layout;
        private bool _showAsCompulsory;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlUIFormFieldLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Loads a form property definition from the xml string provided
        /// </summary>
        /// <param name="xmlUIProp">The xml string</param>
        /// <returns>Returns a UIFormProperty object</returns>
        public IUIFormField LoadUIProperty(string xmlUIProp)
        {
            try
            {
                return this.LoadUIProperty(this.CreateXmlElement(xmlUIProp));
            }
            catch (InvalidXmlDefinitionException ex)
            {
                throw new InvalidXmlDefinitionException("The XML: " + xmlUIProp 
                        + " could not be parsed into a UIProp because :- " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Loads a form property definition from the xml element provided
        /// </summary>
        /// <param name="uiPropElement">The xml element</param>
        /// <returns>Returns a UIFormProperty object</returns>
        public IUIFormField LoadUIProperty(XmlElement uiPropElement)
        {
            return (IUIFormField) Load(uiPropElement);
        }

        /// <summary>
        /// Creates a form property definition from the data already loaded
        /// </summary>
        /// <returns>Returns a UIFormProperty object</returns>
        protected override object Create()
        {
			return _defClassFactory.CreateUIFormProperty(_label, _propertyName,
				_controlTypeName, _controlAssembly, _mapperTypeName, _mapperTypeAssembly,
                _editable, _showAsCompulsory, _toolTipText, _parameters, _layout);
        }

        /// <summary>
        /// Loads form property data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            _reader.Read();
            LoadPropertyName();
            LoadLabel();
            LoadControlType();
            LoadMapperTypeName();
            LoadMapperTypeAssembly();
            LoadEditable();
            LoadShowAsCompulsory();
            LoadToolTipText();
            LoadLayout();
            LoadParameters();
        }

        private void LoadLayout()
        {
            string layoutAttribute = "";
            try
            {
                layoutAttribute = _reader.GetAttribute("layout");
                if(layoutAttribute != null) _layout = (LayoutStyle) Enum.Parse(typeof(LayoutStyle), layoutAttribute);
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException(String.Format(
                    "In the definition for the field '{0}' the 'layout' " +
                    "was set to an invalid value ('{1}'). The valid options are " +
                    "Label and GroupBox.", _propertyName, layoutAttribute), ex);
            }
        }

        /// <summary>
        /// Loads the mapper type name from the reader.  This method is
        /// called by LoadFromReader().
        /// </summary>
        private void LoadMapperTypeName()
        {
            _mapperTypeName = _reader.GetAttribute("mapperType");
        }

        /// <summary>
        /// Loads the mapper type assembly from the reader.  Called from LoadFromReader();
        /// </summary>
        private void LoadMapperTypeAssembly()
        {
            _mapperTypeAssembly = _reader.GetAttribute("mapperAssembly");
        }

        /// <summary>
        /// Loads the control type name from the reader. This method is
        /// called by LoadFromReader().
        /// </summary>
        private void LoadControlType()
        {
			_controlTypeName = _reader.GetAttribute("type");
			_controlAssembly = _reader.GetAttribute("assembly");
        }

        /// <summary>
        /// Loads the property name from the reader. This method is
        /// called by LoadFromReader().
        /// </summary>
        private void LoadPropertyName()
        {
            _propertyName = _reader.GetAttribute("property");
        }

        /// <summary>
        /// Loads the label from the reader. This method is
        /// called by LoadFromReader().
        /// </summary>
        private void LoadLabel()
        {
            _label = _reader.GetAttribute("label");
        }

        /// <summary>
        /// Loads the "edtiable" attribute from the reader. This method is
        /// called by LoadFromReader().
        /// </summary>
        private void LoadEditable()
        {
            try
            {
                _editable = Convert.ToBoolean(_reader.GetAttribute("editable"));
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException("The 'editable' attribute " +
                    "in a 'field' element is invalid. The valid options " +
                    "are 'true' and 'false'.", ex);
            }
        }
        /// <summary>
        /// Loads the "edtiable" attribute from the reader. This method is
        /// called by LoadFromReader().
        /// </summary>
        private void LoadShowAsCompulsory()
        {
            try
            {
                _showAsCompulsory = Convert.ToBoolean(_reader.GetAttribute("showAsCompulsory"));
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException("The 'showAsCompulsory' attribute " +
                    "in a 'field' element is invalid. The valid options " +
                    "are 'true' and 'false'.", ex);
            }
        }

        /// <summary>
        /// Loads the tool tip text value from the reader. This method is
        /// called by LoadFromReader().
        /// </summary>
        private void LoadToolTipText()
        {
            _toolTipText = _reader.GetAttribute("toolTipText");
        }

        /// <summary>
        /// Loads the attributes from the reader. This method is
        /// called by LoadFromReader().
        /// </summary>
        private void LoadParameters()
        {
            _parameters = new Hashtable();
            _reader.Read();

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
                    _parameters.Add(attName, attValue);
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

#pragma warning restore 612,618
}