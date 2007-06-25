using System;
using System.Xml;
using Habanero.Bo.ClassDefinition;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.Bo.Loaders
{
    /// <summary>
    /// Loads UI form column information from xml data
    /// </summary>
    public class XmlUIFormColumnLoader : XmlLoader
    {
        private UIFormColumn _column;

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
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlUIFormColumnLoader()
        {
        }

        /// <summary>
        /// Loads a form column definition from the xml string provided
        /// </summary>
        /// <param name="formColumnElement">The xml string</param>
        /// <returns>Returns a UIFormColumn object</returns>
        public UIFormColumn LoadUIFormColumn(string formColumnElement)
        {
            return this.LoadUIFormColumn(this.CreateXmlElement(formColumnElement));
        }

        /// <summary>
        /// Loads a form column definition from the xml element provided
        /// </summary>
        /// <param name="formColumnElement">The xml element</param>
        /// <returns>Returns a UIFormColumn object</returns>
        public UIFormColumn LoadUIFormColumn(XmlElement formColumnElement)
        {
            return (UIFormColumn) this.Load(formColumnElement);
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
                throw new InvalidXmlDefinitionException("In a 'uiFormColumn' " + 
                    "element, the 'width' attribute has been given " +
                    "an invalid integer pixel value.", ex);
            }

            _reader.Read();
            XmlUIFormPropertyLoader propLoader = new XmlUIFormPropertyLoader(DtdLoader, _defClassFactory);
            while (_reader.Name == "uiFormProperty")
            {
                _column.Add(propLoader.LoadUIProperty(_reader.ReadOuterXml()));
            }

            if (_column.Count == 0)
            {
                throw new InvalidXmlDefinitionException("No 'uiFormProperty' " +
                    "elements were specified in a 'uiFormColumn' element.  Ensure " +
                    "that the element " +
                    "contains one or more 'uiFormProperty' elements, which " +
                    "specify the property editing controls to appear in the " +
                    "editing form column.");
            }
        }
    }
}