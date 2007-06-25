using System;
using System.Xml;
using Habanero.Bo.ClassDefinition;
using Habanero.Base;
using Habanero.Util;
using log4net;

namespace Habanero.Bo.Loaders
{
    /// <summary>
    /// Loads UI grid property definitions from xml data
    /// </summary>
    public class XmlUIGridPropertyLoader : XmlLoader
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.Bo.Loaders.XmlUIGridPropertyLoader");
        private string _heading;
        private string _propertyName;
        private Type _gridControlType;
        private bool _isReadOnly;
        private int _width;
        private UIGridProperty.PropAlignment _alignment;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdPath">The dtd path</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
		public XmlUIGridPropertyLoader(string dtdPath, IDefClassFactory defClassFactory)
			: base(dtdPath, defClassFactory)
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
			return _defClassFactory.CreateUIGridProperty(_heading, _propertyName, 
				_gridControlType, _isReadOnly, _width, _alignment);
			//return
			//    new UIGridProperty(_heading, _propertyName, _gridControlType, _isReadOnly, _width,
			//                       _alignment);
        }

        /// <summary>
        /// Loads grid property definition data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            _reader.Read();
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
            try
            {
                _isReadOnly = Convert.ToBoolean(_reader.GetAttribute("isReadOnly"));
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException("The 'isReadOnly' attribute " +
                    "in a 'uiGridProperty' element is invalid. The valid options " +
                    "are 'true' and 'false'.", ex);
            }
        }

        /// <summary>
        /// Loads the grid control type name from the reader. This method
        /// is called by LoadFromReader().
        /// </summary>
        private void LoadGridControlType()
        {
            string assemblyName;
            string className = _reader.GetAttribute("gridControlTypeName");
            if (className == "DataGridViewTextBoxColumn" || className == "DataGridViewCheckBoxColumn" ||
                className == "DataGridViewComboBoxColumn")
            {
                assemblyName = "System.Windows.Forms";
            }
            else
            {
                assemblyName = "Habanero.Ui.Generic";
            }
            //log.Debug("assembly: " + assemblyName + ", class: " + className) ;
            try
            {
                _gridControlType = TypeLoader.LoadType(assemblyName, className);
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException("In a 'uiGridProperty' " +
                    "element, the 'gridControlTypeName' attribute has an invalid " +
                    "type. The available options are: DataGridViewTextBoxColumn, " +
                    "DataGridViewCheckBoxColumn, DataGridViewComboBoxColumn and " +
                    "DataGridViewDateTimeColumn.", ex);
            }
        }

        /// <summary>
        /// Loads the property name from the reader. This method
        /// is called by LoadFromReader().
        /// </summary>
        private void LoadPropertyName()
        {
            _propertyName = _reader.GetAttribute("propertyName");
            if (_propertyName == null || _propertyName.Length == 0)
            {
                throw new InvalidXmlDefinitionException("In a 'uiGridProperty' " +
                    "element, the 'propertyName' attribute was not specified. This " +
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
            if (_heading == null || _heading.Length == 0)
            {
                throw new InvalidXmlDefinitionException("In a 'uiGridProperty' " +
                    "element, the 'heading' attribute was not specified. This " +
                    "attribute sets the heading of the column as seen by the user.");
            }
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
                    "of a 'uiGridProperty' element, the value provided was " +
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
                _alignment = UIGridProperty.PropAlignment.left;
            }
            else if (alignmentStr == "right")
            {
                _alignment = UIGridProperty.PropAlignment.right;
            }
            else
            {
                _alignment = UIGridProperty.PropAlignment.centre;
            }
        }
    }
}