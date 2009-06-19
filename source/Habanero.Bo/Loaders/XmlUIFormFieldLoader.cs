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

namespace Habanero.BO.Loaders
{
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
		//private Type _controlType;
        private bool _editable;
        private Hashtable _parameters;
        private TriggerCol _triggers = new TriggerCol();
        private string _toolTipText;
        private LayoutStyle _layout;

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlUIFormFieldLoader()
        {
        }

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
            return this.LoadUIProperty(this.CreateXmlElement(xmlUIProp));
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
                _editable, _toolTipText, _parameters, _triggers, _layout);
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
            LoadToolTipText();
            LoadLayout();
            LoadParameters();
            LoadTriggers();
        }

        private void LoadLayout()
        {
            string layoutAttribute = "";
            try
            {
                layoutAttribute = _reader.GetAttribute("layout");
                _layout = (LayoutStyle) Enum.Parse(typeof(LayoutStyle), layoutAttribute);
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

			//string controlTypeName = _reader.GetAttribute("type");
			//string controlAssemblyName = _reader.GetAttribute("assembly");
			//try
			//{
			//    _controlType = TypeLoader.LoadType(controlAssemblyName, controlTypeName);
			//}
			//catch (Exception ex)
			//{
			//    throw new InvalidXmlDefinitionException(String.Format(
			//        "While attempting to load a 'field' element, an " +
			//        "error occurred while loading the control type. " +
			//        "The type supplied was '{0}' and the assembly was '{1}'. " +
			//        "Please ensure that the type exists in the assembly provided.",
			//        controlTypeName, controlAssemblyName), ex);
			//}
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

        /// <summary>
        /// Loads the triggers for the property
        /// </summary>
        private void LoadTriggers()
        {
            _triggers = new TriggerCol();
            while (_reader.Name == "trigger")
            {
                while (_reader.Name == "trigger")
                {
                    XmlTriggerLoader propLoader = new XmlTriggerLoader(DtdLoader, _defClassFactory);
                    _triggers.Add(propLoader.LoadTrigger(_reader.ReadOuterXml()));
                }
            }
        }
    }
}