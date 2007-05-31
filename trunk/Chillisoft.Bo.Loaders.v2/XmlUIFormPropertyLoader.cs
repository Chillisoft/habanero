using System;
using System.Collections;
using System.Xml;
using Chillisoft.Generic.v2;
using Chillisoft.Util.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Loads UI form property information from xml data
    /// </summary>
    public class XmlUIFormPropertyLoader : XmlLoader
    {
        private string itsLabel;
        private string itsPropertyName;
        private string itsMapperTypeName;
        private Type itsControlType;
        private bool itsIsReadOnly;
        private Hashtable itsPropertyAttributes;

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
                new UIFormProperty(itsLabel, itsPropertyName, itsControlType, itsMapperTypeName, itsIsReadOnly,
                                   itsPropertyAttributes);
        }

        /// <summary>
        /// Loads form property data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            itsReader.Read();
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
            itsMapperTypeName = itsReader.GetAttribute("mapperTypeName");
        }

        /// <summary>
        /// Loads the control type name from the reader. This method is
        /// called by LoadFromReader().
        /// </summary>
        private void LoadControlType()
        {
            string controlTypeName = itsReader.GetAttribute("controlTypeName");
            string controlAssemblyName = itsReader.GetAttribute("controlAssemblyName");
            itsControlType = TypeLoader.LoadType(controlAssemblyName, controlTypeName);
        }

        /// <summary>
        /// Loads the property name from the reader. This method is
        /// called by LoadFromReader().
        /// </summary>
        private void LoadPropertyName()
        {
            itsPropertyName = itsReader.GetAttribute("propertyName");
        }

        /// <summary>
        /// Loads the label from the reader. This method is
        /// called by LoadFromReader().
        /// </summary>
        private void LoadLabel()
        {
            itsLabel = itsReader.GetAttribute("label");
        }

        /// <summary>
        /// Loads the "isReadOnly" attribute from the reader. This method is
        /// called by LoadFromReader().
        /// </summary>
        private void LoadIsReadOnly()
        {
            itsIsReadOnly = Convert.ToBoolean(itsReader.GetAttribute("isReadOnly"));
        }

        /// <summary>
        /// Loads the attributes from the reader. This method is
        /// called by LoadFromReader().
        /// </summary>
        private void LoadAttributes()
        {
            itsPropertyAttributes = new Hashtable();
            System.Console.WriteLine(itsReader.Name);
            itsReader.Read();
            System.Console.WriteLine(itsReader.Name);

            while (itsReader.Name == "uiFormPropertyAtt")
            {
                itsPropertyAttributes.Add(itsReader.GetAttribute("name"), itsReader.GetAttribute("value"));
                itsReader.Read();
            }
        }
    }
}