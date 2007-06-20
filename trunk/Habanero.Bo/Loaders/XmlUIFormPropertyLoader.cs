using System;
using System.Collections;
using System.Xml;
using Habanero.Generic;
using Habanero.Util;

namespace Habanero.Bo.Loaders
{
    /// <summary>
    /// Loads UI form property information from xml data
    /// </summary>
    public class XmlUIFormPropertyLoader : XmlLoader
    {
        private string _label;
        private string _propertyName;
        private string _mapperTypeName;
        private Type _controlType;
        private bool _isReadOnly;
        private Hashtable _propertyAttributes;

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlUIFormPropertyLoader()
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlUIFormPropertyLoader(string dtdPath) : base(dtdPath)
        {
        }

        /// <summary>
        /// Loads a form property definition from the xml string provided
        /// </summary>
        /// <param name="xmlUIProp">The xml string</param>
        /// <returns>Returns a UIFormProperty object</returns>
        public UIFormProperty LoadUIProperty(string xmlUIProp)
        {
            return this.LoadUIProperty(this.CreateXmlElement(xmlUIProp));
        }

        /// <summary>
        /// Loads a form property definition from the xml element provided
        /// </summary>
        /// <param name="uiPropElement">The xml element</param>
        /// <returns>Returns a UIFormProperty object</returns>
        public UIFormProperty LoadUIProperty(XmlElement uiPropElement)
        {
            return (UIFormProperty) Load(uiPropElement);
        }

        /// <summary>
        /// Creates a form property definition from the data already loaded
        /// </summary>
        /// <returns>Returns a UIFormProperty object</returns>
        protected override object Create()
        {
            return
                new UIFormProperty(_label, _propertyName, _controlType, _mapperTypeName, _isReadOnly,
                                   _propertyAttributes);
        }

        /// <summary>
        /// Loads form property data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            _reader.Read();
            LoadLabel();
            LoadPropertyName();
            LoadControlType();
            LoadMapperTypeName();
            LoadIsReadOnly();
            LoadAttributes();
        }

        /// <summary>
        /// Loads the mapper type name from the reader.  This method is
        /// called by LoadFromReader().
        /// </summary>
        private void LoadMapperTypeName()
        {
            _mapperTypeName = _reader.GetAttribute("mapperTypeName");
        }

        /// <summary>
        /// Loads the control type name from the reader. This method is
        /// called by LoadFromReader().
        /// </summary>
        private void LoadControlType()
        {
            string controlTypeName = _reader.GetAttribute("controlTypeName");
            string controlAssemblyName = _reader.GetAttribute("controlAssemblyName");
            try
            {
                _controlType = TypeLoader.LoadType(controlAssemblyName, controlTypeName);
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException(String.Format(
                    "While attempting to load a 'uiFormProperty' element, an " +
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
            _propertyName = _reader.GetAttribute("propertyName");
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
        /// Loads the "isReadOnly" attribute from the reader. This method is
        /// called by LoadFromReader().
        /// </summary>
        private void LoadIsReadOnly()
        {
            try
            {
                _isReadOnly = Convert.ToBoolean(_reader.GetAttribute("isReadOnly"));
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException("The 'isReadOnly' attribute " +
                    "in a 'uiFormProperty' element is invalid. The valid options " +
                    "are 'true' and 'false'.", ex);
            }
        }

        /// <summary>
        /// Loads the attributes from the reader. This method is
        /// called by LoadFromReader().
        /// </summary>
        private void LoadAttributes()
        {
            _propertyAttributes = new Hashtable();
            System.Console.WriteLine(_reader.Name);
            _reader.Read();
            System.Console.WriteLine(_reader.Name);

            while (_reader.Name == "uiFormPropertyAtt")
            {
                string attName = _reader.GetAttribute("name");
                string attValue = _reader.GetAttribute("value");
                if (attName == null || attName.Length == 0 ||
                    attValue == null || attValue.Length == 0)
                {
                    throw new InvalidXmlDefinitionException("In a " +
                        "'uiFormPropertyAtt' element, either the 'name' or " +
                        "'value' attribute has been omitted.");
                }

                try
                {
                    _propertyAttributes.Add(attName, attValue);
                }
                catch (Exception ex)
                {
                    throw new InvalidXmlDefinitionException("An error occurred " +
                        "while loading a 'uiFormPropertyAtt' element.  There may " +
                        "be duplicates with the same 'name' attribute.", ex);
                }
                ReadAndIgnoreEndTag();
            }
        }
    }
}