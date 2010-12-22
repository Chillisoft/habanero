// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using System.Xml;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using OpenNETCF;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Loads a relationship from xml data
    /// </summary>
    public class XmlRelationshipLoader : XmlLoader
    {
        private readonly string _className;
//        private IPropDefCol _propDefCol;
        private string _relatedAssemblyName;
        private string _relatedClassName;
        private IRelKeyDef _relKeyDef;
        private string _name;
        private string _type;
        private bool _keepReferenceToRelatedObject;
        private string _orderBy;
        private DeleteParentAction _deleteParentAction;
        private RelationshipType _relationshipType;
        private bool _owningBOHasForeignKey;
        private string _reverseRelationshipName;
        private string _typeParameter;
        private int _timeout;
        private InsertParentAction _insertParentAction;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdLoader">The dtd loader</param>
        /// <param name="defClassFactory">The factory for the definition classes</param>
        /// <param name="className">The name of the class that has this relationship</param>
        public XmlRelationshipLoader(DtdLoader dtdLoader, IDefClassFactory defClassFactory, string className)
            : base(dtdLoader, defClassFactory)
        {
            _className = className;
        }

        /// <summary>
        /// Loads a relationship definition from the xml string provided
        /// </summary>
        /// <param name="xmlRelationshipDef">The xml string</param>
        /// <param name="propDefs">The property definition collection</param>
        /// <returns>Returns a relationship definition</returns>
        public IRelationshipDef LoadRelationship(string xmlRelationshipDef, IPropDefCol propDefs)
        {
            return LoadRelationship(this.CreateXmlElement(xmlRelationshipDef), propDefs);
        }

        /// <summary>
        /// Loads a relationship definition from the xml element provided
        /// </summary>
        /// <param name="relationshipElement">The xml element</param>
        /// <param name="propDefs">The property definition collection</param>
        /// <returns>Returns a relationship definition</returns>
        public IRelationshipDef LoadRelationship(XmlElement relationshipElement, IPropDefCol propDefs)
        {
//            _propDefCol = propDefs;
            return (IRelationshipDef) this.Load(relationshipElement);
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
                IRelationshipDef relationshipDef = _defClassFactory.CreateSingleRelationshipDef
                    (_name, _relatedAssemblyName, _relatedClassName, _relKeyDef, _keepReferenceToRelatedObject,
                     _deleteParentAction, _insertParentAction, _relationshipType);
                relationshipDef.OwningBOHasForeignKey = _owningBOHasForeignKey;
                relationshipDef.ReverseRelationshipName = _reverseRelationshipName;
                relationshipDef.RelatedObjectTypeParameter = _typeParameter;
                return relationshipDef;
            }
            if (_type == "multiple")
            {
                IRelationshipDef relationshipDef = _defClassFactory.CreateMultipleRelationshipDef
                    (_name, _relatedAssemblyName, _relatedClassName, _relKeyDef, _keepReferenceToRelatedObject, _orderBy,
                     _deleteParentAction, _insertParentAction, _relationshipType, _timeout);
                relationshipDef.ReverseRelationshipName = _reverseRelationshipName;
                relationshipDef.RelatedObjectTypeParameter = _typeParameter;
                return relationshipDef;
            }
            throw new InvalidXmlDefinitionException
                ("There seems to be a problem with the relationship dtd as it should only allow relationships of type 'single' or 'multiple'.");
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
            _name = _reader.GetAttribute("name");
            _type = _reader.GetAttribute("type");

            string relationshipTypeString = _reader.GetAttribute("relationshipType");

            try
            {
                _relationshipType = (RelationshipType) Enum2.Parse(typeof (RelationshipType), relationshipTypeString);
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException
                    (String.Format
                         ("In the definition for the relationship '{0}' on class '{1}' " + "the 'relationshipType' "
                          + "was set to an invalid value ('{2}'). The valid options are "
                          + "Association, Aggregation and Composition.", _name, _className, relationshipTypeString), ex);
            }

            if (_type == null || (_type != "single" && _type != "multiple"))
            {
                throw new InvalidXmlDefinitionException
                    ("In a 'relationship' " + "element, the 'type' attribute was not included or was given "
                     + "an invalid value.  The 'type' refers to the type of "
                     + "relationship and can be either 'single' or 'multiple'.");
            }

            _keepReferenceToRelatedObject = _reader.GetAttribute("keepReference") == "true";
            _owningBOHasForeignKey = _reader.GetAttribute("owningBOHasForeignKey") == "true";
            _reverseRelationshipName = _reader.GetAttribute("reverseRelationship");
            _typeParameter = _reader.GetAttribute("typeParameter");
            try
            {
                _timeout = Convert.ToInt32(_reader.GetAttribute("timeout"));
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException
                    ("In a 'relationship' element, " + "the 'timeout' attribute has been given "
                     + "an invalid integer value.", ex);
            }


            _orderBy = _reader.GetAttribute("orderBy");

            try
            {
                _deleteParentAction =
                    (DeleteParentAction) Enum2.Parse(typeof (DeleteParentAction), _reader.GetAttribute("deleteAction"));
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException
                    ("In a 'relationship' " + "element, the 'deleteAction' attribute has been given "
                     + "an invalid value. The available options are " + "DeleteRelated, DereferenceRelated and "
                     + "Prevent.", ex);
            }
            if (_relationshipType == RelationshipType.Association)
            {
                try
                {
                    string attribute = _reader.GetAttribute("insertAction");
                    if (string.IsNullOrEmpty(attribute))
                    {
                        attribute = "InsertRelationship";
                    }
                    _insertParentAction =
                        (InsertParentAction)
                        Enum2.Parse(typeof (InsertParentAction), attribute);
                }
                catch (Exception ex)
                {
                    throw new InvalidXmlDefinitionException
                        ("In a 'relationship' " + "element, the 'deleteAction' attribute has been given "
                         + "an invalid value. The available options are " + "DeleteRelated, DereferenceRelated and "
                         + "Prevent.", ex);
                }
            }
            else
            {
                _insertParentAction = InsertParentAction.InsertRelationship;
            }
        }

        /// <summary>
        /// Loads the RelKeyDef information from the reader.  This method
        /// is called by LoadFromReader().
        /// </summary>
        private void LoadRelKeyDef()
        {
            _relKeyDef = _defClassFactory.CreateRelKeyDef();
            while (_reader.Name == "relatedProperty")
            {
                string propName = _reader.GetAttribute("property");
                string relPropName = _reader.GetAttribute("relatedProperty");
                if (string.IsNullOrEmpty(propName))
                {
                    throw new InvalidXmlDefinitionException
                        ("A 'relatedProperty' element " + "is missing the 'property' attribute, which specifies the "
                         + "property in this class to which the " + "relationship will link.");
                }
                if (string.IsNullOrEmpty(relPropName))
                {
                    throw new InvalidXmlDefinitionException
                        ("A 'relatedProperty' element "
                         + "is missing the 'relatedProperty' attribute, which specifies the "
                         + "property in the related class to which the " + "relationship will link.");
                }
                //if (!_propDefCol.Contains(propName))
                //{
                //    throw new InvalidXmlDefinitionException
                //        (string.Format
                //             ("The property '{0}' defined in the " + "'relatedProperty' element "
                //              + "in its 'Property' attribute, which specifies the "
                //              + "property in the class '{1}' from which the "
                //              + "relationship '{2}' will link is not defined in the class '{1}'.", propName, _className,
                //              _name));
                //}
                _relKeyDef.Add(_defClassFactory.CreateRelPropDef(propName, relPropName));
                ReadAndIgnoreEndTag();
            }
        }
    }
}