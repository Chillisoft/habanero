using System.Xml;
using Habanero.Bo.ClassDefinition;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.Bo.Loaders
{
    /// <summary>
    /// Loads UI form tab information from xml data
    /// </summary>
    public class XmlUIFormTabLoader : XmlLoader
    {
        private UIFormTab _tab;

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlUIFormTabLoader()
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlUIFormTabLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Loads a form tab definition from the xml string provided
        /// </summary>
        /// <param name="formTabElement">The xml string</param>
        /// <returns>Returns a UIFormTab object</returns>
        public UIFormTab LoadUIFormTab(string formTabElement)
        {
            return this.LoadUIFormTab(this.CreateXmlElement(formTabElement));
        }

        /// <summary>
        /// Loads a form tab definition from the xml element provided
        /// </summary>
        /// <param name="formTabElement">The xml element</param>
        /// <returns>Returns a UIFormTab object</returns>
        public UIFormTab LoadUIFormTab(XmlElement formTabElement)
        {
            return (UIFormTab) this.Load(formTabElement);
        }

        /// <summary>
        /// Creates a form tab definition from the data already loaded
        /// </summary>
        /// <returns>Returns a UIFormTab object</returns>
        protected override object Create()
        {
            return _tab;
        }

        /// <summary>
        /// Loads form tab data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
			_tab = _defClassFactory.CreateUIFormTab();
			//_tab = new UIFormTab();

            //_reader.Read();
            //string className = _reader.GetAttribute("class");
            //string assemblyName = _reader.GetAttribute("assembly");
            //_collection.Class = TypeLoader.LoadType(assemblyName, className);
            //_collection.Name = new UIPropertyCollectionName(_collection.Class, _reader.GetAttribute("name"));

            _reader.Read();
            _tab.Name = _reader.GetAttribute("name");
            _reader.Read();
            //if (_reader.Name == "uiFormGrid")
            //{
            //    XmlUIFormGridLoader gridLoader = new XmlUIFormGridLoader(DtdLoader, _defClassFactory);
            //    _tab.UIFormGrid = gridLoader.LoadUIFormGrid(_reader.ReadOuterXml());
            //}
            //else
            //{
                XmlUIFormColumnLoader loader = new XmlUIFormColumnLoader(DtdLoader, _defClassFactory);
                while (_reader.Name == "columnLayout")
                {
                    _tab.Add(loader.LoadUIFormColumn(_reader.ReadOuterXml()));
                }

                if (_tab.Count == 0)
                {
                    throw new InvalidXmlDefinitionException("In a 'tab' " +
                        "element, there were no 'columnLayout' " +
                        "elements specified.  Ensure that the element " +
                        "contains one or more " +
                        "'columnLayout' elements, which specify the columns of " +
                        "controls to appear in the editing form.");
                }
           //}
        }
    }
}