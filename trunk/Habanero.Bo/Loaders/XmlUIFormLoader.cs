using System;
using System.Collections.Generic;
using System.Xml;
using Habanero.Bo.ClassDefinition;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.Bo.Loaders
{
    /// <summary>
    /// Loads UI form definitions from xml data
    /// </summary>
    public class XmlUIFormLoader : XmlLoader
    {
        private UIFormDef _uiFormDef;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlUIFormLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlUIFormLoader()
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
            _uiFormDef.Title = _reader.GetAttribute("title");
            try
            {
                _uiFormDef.Width = Convert.ToInt32(_reader.GetAttribute("width"));
                _uiFormDef.Height = Convert.ToInt32(_reader.GetAttribute("height"));
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException("In a 'form' element, " +
                    "either the 'width' or 'height' attribute has been given " +
                    "an invalid integer pixel value.", ex);
            }


            _reader.Read();
            XmlUIFormTabLoader tabLoader = new XmlUIFormTabLoader(DtdLoader, _defClassFactory);
            XmlUIFormColumnLoader columnLoader = new XmlUIFormColumnLoader(DtdLoader, _defClassFactory);
            XmlUIFormFieldLoader fieldLoader = new XmlUIFormFieldLoader(DtdLoader, _defClassFactory);
            List<UIFormColumn> columns = new List<UIFormColumn>();
            List<UIFormProperty> fields = new List<UIFormProperty>();
            string contentType = "";
            while (_reader.Name != "form") {
                if (_reader.Name == "tab") {
                    if (contentType.Length > 0 && contentType != "tab") {
                        throw new InvalidXmlDefinitionException(
                            "A form can have either a set of 'tab', 'columnLayout' or 'field' nodes, but not a mixture.");
                    }
                    contentType = "tab";
                    _uiFormDef.Add(tabLoader.LoadUIFormTab(_reader.ReadOuterXml()));
                }
                else if (_reader.Name == "columnLayout") {
                    if (contentType.Length > 0 && contentType != "columnLayout") {
                        throw new InvalidXmlDefinitionException(
                            "A form can have either a set of 'tab', 'columnLayout' or 'field' nodes, but not a mixture.");
                    }
                    contentType = "columnLayout";
                    columns.Add(columnLoader.LoadUIFormColumn(_reader.ReadOuterXml()));
                }
                else if (_reader.Name == "field") {
                    if (contentType.Length > 0 && contentType != "field") {
                        throw new InvalidXmlDefinitionException(
                            "A form can have either a set of 'tab', 'columnLayout' or 'field' nodes, but not a mixture.");
                    }
                    contentType = "field";
                    fields.Add(fieldLoader.LoadUIProperty(_reader.ReadOuterXml()));

                } else {
                    throw new InvalidXmlDefinitionException(
                        "A form can have either a set of 'tab', 'columnLayout' or 'field' nodes.");
                }
            }
            if (contentType == "columnLayout") {
                UIFormTab tab = _defClassFactory.CreateUIFormTab();
                columns.ForEach(delegate(UIFormColumn obj) { tab.Add(obj); });
                _uiFormDef.Add(tab);
            }
            else if (contentType == "field") {
                UIFormTab tab = _defClassFactory.CreateUIFormTab();
                UIFormColumn col = _defClassFactory.CreateUIFormColumn();
                fields.ForEach(delegate(UIFormProperty obj) { col.Add(obj); });
                tab.Add(col);
                _uiFormDef.Add(tab);
            }

            if (_uiFormDef.Count == 0)
            {
                throw new InvalidXmlDefinitionException("No 'tab', 'columnLayout' or 'field' " +
                    "elements were specified in a 'form' element.  Ensure " +
                    "that the element contains one or more of either 'tab', 'columnLayout' or 'field' elements, " +
                    "which each define what must appear in the editing form for " +
                    "the business object.");
            }
        }
    }
}