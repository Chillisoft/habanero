using System;
using System.Collections.Generic;
using System.Xml;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Base;
using Habanero.Util;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Loads UI form definitions from xml data
    /// </summary>
    public class XmlUIFormLoader : XmlLoader
    {
        private UIForm _uiForm;

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
        public UIForm LoadUIFormDef(string formDefElement)
        {
            return this.LoadUIFormDef(this.CreateXmlElement(formDefElement));
        }

        /// <summary>
        /// Loads a form definition from the xml element provided
        /// </summary>
        /// <param name="formDefElement">The xml element</param>
        /// <returns>Returns a UIFormDef object</returns>
        public UIForm LoadUIFormDef(XmlElement formDefElement)
        {
            return (UIForm) this.Load(formDefElement);
        }

        /// <summary>
        /// Creates a form definition from the data already loaded
        /// </summary>
        /// <returns>Returns a UIFormDef object</returns>
        protected override object Create()
        {
            return _uiForm;
        }

        /// <summary>
        /// Loads form definition data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
			_uiForm = _defClassFactory.CreateUIFormDef();
			//_uiFormDef = new UIFormDef();

            //_reader.Read();
            //string className = _reader.GetAttribute("class");
            //string assemblyName = _reader.GetAttribute("assembly");
            //_collection.Class = TypeLoader.LoadType(assemblyName, className);
            //_collection.Name = new UIPropertyCollectionName(_collection.Class, _reader.GetAttribute("name"));

            _reader.Read();
            _uiForm.Title = _reader.GetAttribute("title");
            try
            {
                _uiForm.Width = Convert.ToInt32(_reader.GetAttribute("width"));
                _uiForm.Height = Convert.ToInt32(_reader.GetAttribute("height"));
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
            List<UIFormField> fields = new List<UIFormField>();
            string contentType = "";
            while (_reader.Name != "form") {
                if (_reader.Name == "tab") {
                    if (contentType.Length > 0 && contentType != "tab") {
                        throw new InvalidXmlDefinitionException(
                            "A form can have either a set of 'tab', 'columnLayout' or 'field' nodes, but not a mixture.");
                    }
                    contentType = "tab";
                    _uiForm.Add(tabLoader.LoadUIFormTab(_reader.ReadOuterXml()));
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
                _uiForm.Add(tab);
            }
            else if (contentType == "field") {
                UIFormTab tab = _defClassFactory.CreateUIFormTab();
                UIFormColumn col = _defClassFactory.CreateUIFormColumn();
                fields.ForEach(delegate(UIFormField obj) { col.Add(obj); });
                tab.Add(col);
                _uiForm.Add(tab);
            }

            if (_uiForm.Count == 0)
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