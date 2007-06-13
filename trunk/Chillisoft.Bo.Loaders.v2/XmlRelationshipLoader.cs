using System;
using System.Xml;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Generic.v2;
using Chillisoft.Util.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Loads a relationship from xml data
    /// </summary>
    public class XmlRelationshipLoader : XmlLoader
    {
        private PropDefCol _propDefCol;
		private Type _relatedClassType;
		private RelKeyDef _relKeyDef;
        private string _name;
        private string _type;
        private bool _keepReferenceToRelatedObject;
        private string _orderBy;
        private int _minNoOfRelatedObjects;
        private int _maxNoOfRelatedObjects;
        private DeleteParentAction _deleteParentAction;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlRelationshipLoader(string dtdPath) : base(dtdPath)
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
                return
                    new SingleRelationshipDef(_name, _relatedClassType, _relKeyDef,
                                              _keepReferenceToRelatedObject);
            }
            else if (_type == "multiple")
            {
                return
                    new MultipleRelationshipDef(_name, _relatedClassType, _relKeyDef,
                                                _keepReferenceToRelatedObject, _orderBy, _minNoOfRelatedObjects,
                                                _maxNoOfRelatedObjects, _deleteParentAction);
            }
            else
            {
                throw new InvalidXmlDefinitionException(
                    "There seems to be a problem with the relationshipDef dtd as it should only allow relationships of type 'single' or 'multiple'.");
            }
        }

        /// <summary>
        /// Loads the relationship definition from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            itsReader.Read();
            LoadRelationshipDef();

            itsReader.Read();
            LoadRelKeyDef();
        }

        /// <summary>
        /// Loads the relationship definition from the reader.  This method
        /// is called by LoadFromReader().
        /// </summary>
        protected void LoadRelationshipDef()
        {
            string relatedClassName = itsReader.GetAttribute("relatedType");
            string relatedAssemblyName = itsReader.GetAttribute("relatedAssembly");
            _relatedClassType = TypeLoader.LoadType(relatedAssemblyName, relatedClassName);
            _name = itsReader.GetAttribute("name");
            _type = itsReader.GetAttribute("type");
            if (itsReader.GetAttribute("keepReferenceToRelatedObject") == "true")
            {
                _keepReferenceToRelatedObject = true;
            }
            else
            {
                _keepReferenceToRelatedObject = false;
            }
            _orderBy = itsReader.GetAttribute("orderBy");
            _minNoOfRelatedObjects = Convert.ToInt32(itsReader.GetAttribute("minNoOfRelatedObjects"));
            _maxNoOfRelatedObjects = Convert.ToInt32(itsReader.GetAttribute("maxNoOfRelatedObjects"));
            _deleteParentAction =
                (DeleteParentAction)
                Enum.Parse(typeof (DeleteParentAction), itsReader.GetAttribute("deleteParentAction"));
        }

        /// <summary>
        /// Loads the RelKeyDef information from the reader.  This method
        /// is called by LoadFromReader().
        /// </summary>
        private void LoadRelKeyDef()
        {
            _relKeyDef = new RelKeyDef();
            itsReader.Read();
            do
            {
                _relKeyDef.Add(
                    new RelPropDef(_propDefCol[itsReader.GetAttribute("name")],
                                   itsReader.GetAttribute("relatedPropName")));
                itsReader.Read();
            } while (itsReader.Name == "relProp");
        }
    }
}