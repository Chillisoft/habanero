using System;
using System.Xml;
using Habanero.Bo.ClassDefinition;
using Habanero.Base;

namespace Habanero.Bo.Loaders
{
    /// <summary>
    /// Loads UI form definitions from xml data
    /// </summary>
    public class XmlUIFormDefLoader : XmlLoader
    {
        private UIFormDef _uiFormDef;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdPath">The dtd path</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
		public XmlUIFormDefLoader(string dtdPath, IDefClassFactory defClassFactory)
			: base(dtdPath, defClassFactory)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlUIFormDefLoader()
        {
        }

        /// <summary>
        /// Loads a form definition from the xml string provided
        /// </summary>
        /// <param name="formDefElement">The xml string</param>
        /// <returns>Returns a UIFormDef object</returns>
        public UIFormDef LoadUIFormDef(string formDefElement)
        {
            return this.LoadUIFormDef(this.CreateXmlElement(formDefElement));
        }

        /// <summary>
        /// Loads a form definition from the xml element provided
        /// </summary>
        /// <param name="formDefElement">The xml element</param>
        /// <returns>Returns a UIFormDef object</returns>
        public UIFormDef LoadUIFormDef(XmlElement formDefElement)
        {
            return (UIFormDef) this.Load(formDefElement);
        }

        /// <summary>
        /// Creates a form definition from the data already loaded
        /// </summary>
        /// <returns>Returns a UIFormDef object</returns>
        protected override object Create()
        {
            return _uiFormDef;
        }

        /// <summary>
        /// Loads form definition data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
			_uiFormDef = _defClassFactory.CreateUIFormDef();
			//_uiFormDef = new UIFormDef();

            //_reader.Read();
            //string className = _reader.GetAttribute("class");
            //string assemblyName = _reader.GetAttribute("assembly");
            //_collection.Class = TypeLoader.LoadType(assemblyName, className);
            //_collection.Name = new UIPropertyCollectionName(_collection.Class, _reader.GetAttribute("name"));

            _reader.Read();
            _uiFormDef.Heading = _reader.GetAttribute("heading");
            try
            {
                _uiFormDef.Width = Convert.ToInt32(_reader.GetAttribute("width"));
                _uiFormDef.Height = Convert.ToInt32(_reader.GetAttribute("height"));
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException("In a 'uiFormDef' element, " +
                    "either the 'width' or 'height' attribute has been given " +
                    "an invalid integer pixel value.", ex);
            }


            _reader.Read();
			XmlUIFormTabLoader loader = new XmlUIFormTabLoader(_dtdPath, _defClassFactory);
            while (_reader.Name == "uiFormTab")
            {
                _uiFormDef.Add(loader.LoadUIFormTab(_reader.ReadOuterXml()));
            }

            if (_uiFormDef.Count == 0)
            {
                throw new InvalidXmlDefinitionException("No 'uiFormTab' " +
                    "elements were specified in a 'uiFormDef' element.  Ensure " +
                    "that the element contains one or more 'uiFormTab' elements, " +
                    "which each define a tab to appear in the editing form for " +
                    "the business object.");
            }
        }
    }
}