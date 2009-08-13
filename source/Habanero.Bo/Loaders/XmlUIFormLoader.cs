//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Xml;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Loads UI form definitions from xml data
    /// </summary>
    public class XmlUIFormLoader : XmlLoader
    {
        private IUIForm _uiForm;

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
        /// Loads a form definition from the xml string provided
        /// </summary>
        /// <param name="formDefElement">The xml string</param>
        /// <returns>Returns a UIFormDef object</returns>
        public IUIForm LoadUIFormDef(string formDefElement)
        {
            return this.LoadUIFormDef(this.CreateXmlElement(formDefElement));
        }

        /// <summary>
        /// Loads a form definition from the xml element provided
        /// </summary>
        /// <param name="formDefElement">The xml element</param>
        /// <returns>Returns a UIFormDef object</returns>
        public IUIForm LoadUIFormDef(XmlElement formDefElement)
        {
            return (IUIForm) this.Load(formDefElement);
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
            List<IUIFormColumn> columns = new List<IUIFormColumn>();
            List<IUIFormField> fields = new List<IUIFormField>();
            string contentType = "";
            while (_reader.Name != "form") {
                if (_reader.Name == "tab") {
                    if (contentType.Length > 0 && contentType != "tab") {
                        throw new InvalidXmlDefinitionException(
                            "A form can have either a set of 'tab', 'columnLayout' or 'field' nodes, but not a mixture.");
                    }
                    contentType = "tab";
                    XmlUIFormTabLoader tabLoader = new XmlUIFormTabLoader(DtdLoader, _defClassFactory);
                    _uiForm.Add(tabLoader.LoadUIFormTab(_reader.ReadOuterXml()));
                }
                else if (_reader.Name == "columnLayout") {
                    if (contentType.Length > 0 && contentType != "columnLayout") {
                        throw new InvalidXmlDefinitionException(
                            "A form can have either a set of 'tab', 'columnLayout' or 'field' nodes, but not a mixture.");
                    }
                    contentType = "columnLayout";
                    XmlUIFormColumnLoader columnLoader = new XmlUIFormColumnLoader(DtdLoader, _defClassFactory);
                    columns.Add(columnLoader.LoadUIFormColumn(_reader.ReadOuterXml()));
                }
                else if (_reader.Name == "field") {
                    if (contentType.Length > 0 && contentType != "field") {
                        throw new InvalidXmlDefinitionException(
                            "A form can have either a set of 'tab', 'columnLayout' or 'field' nodes, but not a mixture.");
                    }
                    contentType = "field";
                    XmlUIFormFieldLoader fieldLoader = new XmlUIFormFieldLoader(DtdLoader, _defClassFactory);
                    fields.Add(fieldLoader.LoadUIProperty(_reader.ReadOuterXml()));

                } else {
                    throw new InvalidXmlDefinitionException(
                        "A form can have either a set of 'tab', 'columnLayout' or 'field' nodes.");
                }
            }
            if (contentType == "columnLayout") {
                IUIFormTab tab = _defClassFactory.CreateUIFormTab();
                columns.ForEach(tab.Add);
                _uiForm.Add(tab);
            }
            else if (contentType == "field") {
                IUIFormTab tab = _defClassFactory.CreateUIFormTab();
                IUIFormColumn col = _defClassFactory.CreateUIFormColumn();
                fields.ForEach(col.Add);
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