using System;
using System.Xml;
using Chillisoft.Generic.v2;
using Chillisoft.Util.v2;
using log4net;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Loads UI grid property definitions from xml data
    /// </summary>
    public class XmlUIGridPropertyLoader : XmlLoader
    {
        private static readonly ILog log = LogManager.GetLogger("Chillisoft.Bo.Loaders.v2.XmlUIGridPropertyLoader");
        private string itsHeading;
        private string itsPropertyName;
        private Type itsGridControlType;
        private bool itsIsReadOnly;
        private int itsWidth;
        private UIGridProperty.PropAlignment itsAlignment;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlUIGridPropertyLoader(string dtdPath) : base(dtdPath)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlUIGridPropertyLoader()
        {
        }

        /// <summary>
        /// Loads a grid property definition from the xml string provided
        /// </summary>
        /// <param name="xmlUIProp">The xml string</param>
        /// <returns>Returns a UIGridProperty object</returns>
        public UIGridProperty LoadUIProperty(string xmlUIProp)
        {
            return this.LoadUIProperty(this.CreateXmlElement(xmlUIProp));
        }

        /// <summary>
        /// Loads a grid property definition from the xml element provided
        /// </summary>
        /// <param name="uiPropElement">The xml element</param>
        /// <returns>Returns a UIGridProperty object</returns>
        public UIGridProperty LoadUIProperty(XmlElement uiPropElement)
        {
            return (UIGridProperty) Load(uiPropElement);
        }

        /// <summary>
        /// Creates a grid property definition from the data already loaded
        /// </summary>
        /// <returns>Returns a UIGridProperty object</returns>
        protected override object Create()
        {
            return
                new UIGridProperty(itsHeading, itsPropertyName, itsGridControlType, itsIsReadOnly, itsWidth,
                                   itsAlignment);
        }

        /// <summary>
        /// Loads grid property definition data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            itsReader.Read();
            LoadHeading();
            LoadPropertyName();
            LoadGridControlType();
            LoadIsReadOnly();
            LoadWidth();
            LoadAlignment();
        }

        /// <summary>
        /// Loads the "isReadOnly" attribute from the reader. This method
        /// is called by LoadFromReader().
        /// </summary>
        private void LoadIsReadOnly()
        {
            itsIsReadOnly = Convert.ToBoolean(itsReader.GetAttribute("isReadOnly"));
        }

        /// <summary>
        /// Loads the grid control type name from the reader. This method
        /// is called by LoadFromReader().
        /// </summary>
        private void LoadGridControlType()
        {
            string assemblyName;
            string className = itsReader.GetAttribute("gridControlTypeName");
            if (className == "DataGridViewTextBoxColumn" || className == "DataGridViewCheckBoxColumn" ||
                className == "DataGridViewComboBoxColumn")
            {
                assemblyName = "System.Windows.Forms";
            }
            else
            {
                assemblyName = "Chillisoft.UI.Generic.v2";
            }
            //log.Debug("assembly: " + assemblyName + ", class: " + className) ;
            itsGridControlType = TypeLoader.LoadType(assemblyName, className);
        }

        /// <summary>
        /// Loads the property name from the reader. This method
        /// is called by LoadFromReader().
        /// </summary>
        private void LoadPropertyName()
        {
            itsPropertyName = itsReader.GetAttribute("propertyName");
        }

        /// <summary>
        /// Loads the heading from the reader. This method
        /// is called by LoadFromReader().
        /// </summary>
        private void LoadHeading()
        {
            itsHeading = itsReader.GetAttribute("heading");
        }

        /// <summary>
        /// Loads the width from the reader. This method
        /// is called by LoadFromReader().
        /// </summary>
        private void LoadWidth()
        {
            itsWidth = Convert.ToInt32(itsReader.GetAttribute("width"));
        }

        /// <summary>
        /// Loads the alignment attribute from the reader. This method
        /// is called by LoadFromReader().
        /// </summary>
        private void LoadAlignment()
        {
            string alignmentStr = Convert.ToString(itsReader.GetAttribute("alignment"));
            if (alignmentStr == "left")
            {
                itsAlignment = UIGridProperty.PropAlignment.left;
            }
            else if (alignmentStr == "right")
            {
                itsAlignment = UIGridProperty.PropAlignment.right;
            }
            else
            {
                itsAlignment = UIGridProperty.PropAlignment.centre;
            }
        }
    }
}