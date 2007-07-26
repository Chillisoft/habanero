using System;
using System.Collections;
using System.Xml;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Base;
using Habanero.Util;
using Habanero.Util.File;

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
        private Type _controlType;
        private bool _editable;
        private Hashtable _propertyAttributes;
        private string _mapperTypeAssembly;

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
        public UIFormField LoadUIProperty(string xmlUIProp)
        {
            return this.LoadUIProperty(this.CreateXmlElement(xmlUIProp));
        }

        /// <summary>
        /// Loads a form property definition from the xml element provided
        /// </summary>
        /// <param name="uiPropElement">The xml element</param>
        /// <returns>Returns a UIFormProperty object</returns>
        public UIFormField LoadUIProperty(XmlElement uiPropElement)
        {
            return (UIFormField) Load(uiPropElement);
        }

        /// <summary>
        /// Creates a form property definition from the data already loaded
        /// </summary>
        /// <returns>Returns a UIFormProperty object</returns>
        protected override object Create()
        {
			return _defClassFactory.CreateUIFormProperty(_label, _propertyName, 
				_controlType, _mapperTypeName, _mapperTypeAssembly, _editable, _propertyAttributes);
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
            LoadParameters();
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
            string controlTypeName = _reader.GetAttribute("type");
            string controlAssemblyName = _reader.GetAttribute("assembly");
            try
            {
                _controlType = TypeLoader.LoadType(controlAssemblyName, controlTypeName);
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException(String.Format(
                    "While attempting to load a 'field' element, an " +
                    "error occurred while loading the control type. " +
                    "The type supplied was '{0}' and the assembly was '{1}'. " +
                    "Please ensure that the type exists in the assembly provided.",
                    controlTypeName, controlAssemblyName), ex);
            }
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
            if (_label == null)
            {
                _label = StringUtilities.DelimitPascalCase(_propertyName, " ") + ":";
            }
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