//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using System.Xml;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Loads a relationship from xml data
    /// </summary>
    public class XmlRelationshipLoader : XmlLoader
    {
        private PropDefCol _propDefCol;
		//private Type _relatedClassType;
    	private string _relatedAssemblyName;
    	private string _relatedClassName;
		private RelKeyDef _relKeyDef;
        private string _name;
        private string _type;
        private bool _keepReferenceToRelatedObject;
        private string _orderBy;
       // private int _minNoOfRelatedObjects;
       // private int _maxNoOfRelatedObjects;
        private DeleteParentAction _deleteParentAction;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdLoader">The dtd loader</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlRelationshipLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory)
			: base(dtdLoader, defClassFactory)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlRelationshipLoader()
        {
        }

        /// <summary>
        /// Loads a relationship definition from the xml string provided
        /// </summary>
        /// <param name="xmlRelationshipDef">The xml string</param>
        /// <param name="propDefs">The property definition collection</param>
        /// <returns>Returns a relationship definition</returns>
        public RelationshipDef LoadRelationship(string xmlRelationshipDef, PropDefCol propDefs)
        {
            return LoadRelationship(this.CreateXmlElement(xmlRelationshipDef), propDefs);
        }

        /// <summary>
        /// Loads a relationship definition from the xml element provided
        /// </summary>
        /// <param name="relationshipElement">The xml element</param>
        /// <param name="propDefs">The property definition collection</param>
        /// <returns>Returns a relationship definition</returns>
        public RelationshipDef LoadRelationship(XmlElement relationshipElement, PropDefCol propDefs)
        {
            _propDefCol = propDefs;
            return (RelationshipDef) this.Load(relationshipElement);
        }

        /// <summary>
        /// Creates a single or multiple relationship from the data already
        /// loaded
        /// </summary>
        /// <returns>Returns either a SingleRelationshipDef or
        /// MultipleRelationshipDef object, depending on the type
        /// specification
        /// </returns>
        /// <exception cref="InvalidXmlDefinitionException">Thrown if a
        /// relationship other than 'single' or 'multiple' is specified
        /// </exception>
        protected override object Create()
        {
            if (_type == "single")
            {
				return _defClassFactory.CreateSingleRelationshipDef(_name, _relatedAssemblyName, _relatedClassName, 
					_relKeyDef, _keepReferenceToRelatedObject, _deleteParentAction);
				//return new SingleRelationshipDef(_name, _relatedAssemblyName, _relatedClassName, 
				//    _relKeyDef, _keepReferenceToRelatedObject);				//return
				//    new SingleRelationshipDef(_name, _relatedClassType, _relKeyDef,
				//                  _keepReferenceToRelatedObject);
			}
            else if (_type == "multiple")
            {
				return _defClassFactory.CreateMultipleRelationshipDef(_name, _relatedAssemblyName, _relatedClassName, 
					_relKeyDef, _keepReferenceToRelatedObject, _orderBy, _deleteParentAction);
				//return new MultipleRelationshipDef(_name, _relatedAssemblyName, _relatedClassName, 
				//    _relKeyDef, _keepReferenceToRelatedObject, _orderBy, _minNoOfRelatedObjects,
				//    _maxNoOfRelatedObjects, _deleteParentAction);
				//return
				//    new MultipleRelationshipDef(_name, _relatedClassType, _relKeyDef,
				//                                _keepReferenceToRelatedObject, _orderBy, _minNoOfRelatedObjects,
				//                                _maxNoOfRelatedObjects, _deleteParentAction);
            }
            else
            {
                throw new InvalidXmlDefinitionException(
                    "There seems to be a problem with the relationship dtd as it should only allow relationships of type 'single' or 'multiple'.");
            }
        }

        /// <summary>
        /// Loads the relationship definition from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            _reader.Read();
            LoadRelationshipDef();

            _reader.Read();
            LoadRelKeyDef();
        }

        /// <summary>
        /// Loads the relationship definition from the reader.  This method
        /// is called by LoadFromReader().
        /// </summary>
        protected void LoadRelationshipDef()
        {
            _relatedClassName = _reader.GetAttribute("relatedClass");
			_relatedAssemblyName = _reader.GetAttribute("relatedAssembly");
            //_relatedClassType = TypeLoader.LoadType(relatedAssemblyName, relatedClassName);
            _name = _reader.GetAttribute("name");
            _type = _reader.GetAttribute("type");
            
            if (_type == null || (_type != "single" && _type != "multiple"))
            {
                throw new InvalidXmlDefinitionException("In a 'relationship' " +
                    "element, the 'type' attribute was not included or was given " +
                    "an invalid value.  The 'type' refers to the type of " +
                    "relationship and can be either 'single' or 'multiple'.");
            }

            if (_reader.GetAttribute("keepReference") == "true")
            {
                _keepReferenceToRelatedObject = true;
            }
            else
            {
                _keepReferenceToRelatedObject = false;
            }
            _orderBy = _reader.GetAttribute("orderBy");

            //try
            //{
            //    _minNoOfRelatedObjects = Convert.ToInt32(_reader.GetAttribute("minNoOfRelatedObjects"));
            //    _maxNoOfRelatedObjects = Convert.ToInt32(_reader.GetAttribute("maxNoOfRelatedObjects"));
            //}
            //catch (Exception ex)
            //{
            //    throw new InvalidXmlDefinitionException("In a 'relationship' " +
            //        "element, either the 'minNoOfRelatedObjects' or " +
            //        "'maxNoOfRelatedObjects' attribute has been given an invalid " +
            //        "integer value.", ex);
            //}

            try
            {
                _deleteParentAction =
                    (DeleteParentAction)
                    Enum.Parse(typeof (DeleteParentAction), _reader.GetAttribute("deleteAction"));
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException("In a 'relationship' " +
                    "element, the 'deleteAction' attribute has been given " +
                    "an invalid value. The available options are " +
                    "DeleteRelated, DereferenceRelated and " +
                    "Prevent.", ex);
            }
        }

        /// <summary>
        /// Loads the RelKeyDef information from the reader.  This method
        /// is called by LoadFromReader().
        /// </summary>
        private void LoadRelKeyDef()
        {
			_relKeyDef = _defClassFactory.CreateRelKeyDef();
			//_relKeyDef = new RelKeyDef();
            //_reader.Read();
            while (_reader.Name == "relatedProperty")
            {
                string defName = _reader.GetAttribute("property");
                string relPropName = _reader.GetAttribute("relatedProperty");
                if (defName == null || defName.Length == 0)
                {
                    throw new InvalidXmlDefinitionException("A 'relatedProperty' element " +
                        "is missing the 'property' attribute, which specifies the " +
                        "property in this class to which the " +
                        "relationship will link.");
                }
                if (relPropName == null || relPropName.Length == 0)
                {
                    throw new InvalidXmlDefinitionException("A 'relatedProperty' element " +
                        "is missing the 'relatedProperty' attribute, which specifies the " +
                        "property in the related class to which the " + 
                        "relationship will link.");
                }

                //This error was moved to the XmlClassDefsLoader.DoPostLoadChecks method so that it handles inherited properties
                //if (!_propDefCol.Contains(defName))
                //{
                //    throw new InvalidXmlDefinitionException(String.Format(
                //        "In a 'relatedProperty' element, the property '{0}' given in the " +
                //        "'property' attribute has not been defined among the class's " +
                //        "property definitions. Either add " +
                //        "the property definition or check the spelling and " +
                //        "capitalisation.", defName));
                //}
				_relKeyDef.Add(_defClassFactory.CreateRelPropDef(_propDefCol[defName], relPropName));
				//_relKeyDef.Add(new RelPropDef(_propDefCol[defName], relPropName));
                ReadAndIgnoreEndTag();
            }
        }
    }
}