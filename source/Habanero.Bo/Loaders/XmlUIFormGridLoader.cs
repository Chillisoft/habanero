//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Xml;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Base;
using Habanero.Util;
using Habanero.Util.File;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Loads UI form grid information from xml data
    /// </summary>
    public class XmlUIFormGridLoader : XmlLoader
    {
        private string _relationshipName;
        private Type _gridType;
        private string _correspondingRelationshipName;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlUIFormGridLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlUIFormGridLoader()
        {
        }

        /// <summary>
        /// Loads a form grid definition from the xml string provided
        /// </summary>
        /// <param name="xmlUIFormGrid">The xml string</param>
        /// <returns>Returns a UIFormGrid object</returns>
        public UIFormGrid LoadUIFormGrid(string xmlUIFormGrid)
        {
            return this.LoadUIFormGrid(this.CreateXmlElement(xmlUIFormGrid));
        }

        /// <summary>
        /// Loads a form grid definition from the xml element provided
        /// </summary>
        /// <param name="xmlUIFormGrid">The xml element</param>
        /// <returns>Returns a UIFormGrid object</returns>
        public UIFormGrid LoadUIFormGrid(XmlElement xmlUIFormGrid)
        {
            return (UIFormGrid) Load(xmlUIFormGrid);
        }

        /// <summary>
        /// Creates a form grid definition from the data already loaded
        /// </summary>
        /// <returns>Returns a UIFormGrid object</returns>
        protected override object Create()
        {
			return _defClassFactory.CreateUIFormGrid(_relationshipName, _gridType, _correspondingRelationshipName);
			//return new UIFormGrid(_relationshipName, _gridType, _correspondingRelationshipName);
        }

        /// <summary>
        /// Loads form grid data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            _reader.Read();
            LoadRelationshipName();
            LoadGridType();
            LoadCorrespondingRelationshipName();
        }

        /// <summary>
        /// Loads the corresponding relationship name from the reader. This
        /// method is called by LoadFromReader().
        /// </summary>
        private void LoadCorrespondingRelationshipName()
        {
            _correspondingRelationshipName = _reader.GetAttribute("reverseRelationship");
        }

        /// <summary>
        /// Loads the grid type from the reader. This
        /// method is called by LoadFromReader().
        /// </summary>
        private void LoadGridType()
        {
            string className = "Habanero.UI.Grid.EditableGrid"; //"_reader.GetAttribute("gridType");
            string assemblyName = "Habanero.UI.Pro";
            try
            {
                _gridType = TypeLoader.LoadType(assemblyName, className);
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException(String.Format(
                    "While attempting to load a 'formGrid' element, an " +
                    "error occurred while loading the grid type. " +
                    "The type supplied was '{0}' and the assembly was '{1}'. " +
                    "Please ensure that the type exists in the assembly provided.",
                    className, assemblyName), ex);
            }
        }

        /// <summary>
        /// Loads the relationship name from the reader. This
        /// method is called by LoadFromReader().
        /// </summary>
        private void LoadRelationshipName()
        {
            _relationshipName = _reader.GetAttribute("relationship");
        }
    }
}