#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System.Collections.Generic;
using System.Xml;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Loads UI form tab information from xml data
    /// </summary>
    public class XmlUIFormTabLoader : XmlLoader
    {
        private IUIFormTab _tab;

        private const string MixedContentMessage = "A 'tab' can have either a set of 'columnLayout' or 'field' nodes or a single 'formGrid' node, but not a mixture.";


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
        public IUIFormTab LoadUIFormTab(string formTabElement)
        {
            return this.LoadUIFormTab(this.CreateXmlElement(formTabElement));
        }

        /// <summary>
        /// Loads a form tab definition from the xml element provided
        /// </summary>
        /// <param name="formTabElement">The xml element</param>
        /// <returns>Returns a UIFormTab object</returns>
        public IUIFormTab LoadUIFormTab(XmlElement formTabElement)
        {
            return (IUIFormTab) this.Load(formTabElement);
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
            _reader.Read();
            _tab.Name = _reader.GetAttribute("name");
            _reader.Read();
            var fields = new List<IUIFormField>();
            var contentType = "";
            while (_reader.Name != "tab")
            {
                if (_reader.Name == "columnLayout")
                {
                    if (contentType.Length > 0 && contentType != "columnLayout")
                    {
                        throw new InvalidXmlDefinitionException(MixedContentMessage);
                    }
                    contentType = "columnLayout";
                    var columnLoader = new XmlUIFormColumnLoader(DtdLoader, _defClassFactory);
                    _tab.Add(columnLoader.LoadUIFormColumn(_reader.ReadOuterXml()));
                }
                else if (_reader.Name == "field")
                {
                    if (contentType.Length > 0 && contentType != "field")
                    {
                        throw new InvalidXmlDefinitionException(MixedContentMessage);
                    }
                    contentType = "field";
                    var fieldLoader = new XmlUIFormFieldLoader(DtdLoader, _defClassFactory);
                    fields.Add(fieldLoader.LoadUIProperty(_reader.ReadOuterXml()));

                }
                else
                {
                    throw new InvalidXmlDefinitionException(MixedContentMessage);
                }
            }
            if (contentType == "field")
            {
                var col = _defClassFactory.CreateUIFormColumn();
                fields.ForEach(col.Add);
                _tab.Add(col);
            }
        }
    }
}