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
        private PropDefCol itsPropDefCol;
		private Type itsRelatedClassType;
		private RelKeyDef itsRelKeyDef;
        private string itsName;
        private string itsType;
        private bool itsKeepReferenceToRelatedObject;
        private string itsOrderBy;
        private int itsMinNoOfRelatedObjects;
        private int itsMaxNoOfRelatedObjects;
        private DeleteParentAction itsDeleteParentAction;

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
            itsPropDefCol = propDefs;
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
            if (itsType == "single")
            {
                return
                    new SingleRelationshipDef(itsName, itsRelatedClassType, itsRelKeyDef,
                                              itsKeepReferenceToRelatedObject);
            }
            else if (itsType == "multiple")
            {
                return
                    new MultipleRelationshipDef(itsName, itsRelatedClassType, itsRelKeyDef,
                                                itsKeepReferenceToRelatedObject, itsOrderBy, itsMinNoOfRelatedObjects,
                                                itsMaxNoOfRelatedObjects, itsDeleteParentAction);
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
            itsRelatedClassType = TypeLoader.LoadType(relatedAssemblyName, relatedClassName);
            itsName = itsReader.GetAttribute("name");
            itsType = itsReader.GetAttribute("type");
            if (itsReader.GetAttribute("keepReferenceToRelatedObject") == "true")
            {
                itsKeepReferenceToRelatedObject = true;
            }
            else
            {
                itsKeepReferenceToRelatedObject = false;
            }
            itsOrderBy = itsReader.GetAttribute("orderBy");
            itsMinNoOfRelatedObjects = Convert.ToInt32(itsReader.GetAttribute("minNoOfRelatedObjects"));
            itsMaxNoOfRelatedObjects = Convert.ToInt32(itsReader.GetAttribute("maxNoOfRelatedObjects"));
            itsDeleteParentAction =
                (DeleteParentAction)
                Enum.Parse(typeof (DeleteParentAction), itsReader.GetAttribute("deleteParentAction"));
        }

        /// <summary>
        /// Loads the RelKeyDef information from the reader.  This method
        /// is called by LoadFromReader().
        /// </summary>
        private void LoadRelKeyDef()
        {
            itsRelKeyDef = new RelKeyDef();
            itsReader.Read();
            do
            {
                itsRelKeyDef.Add(
                    new RelPropDef(itsPropDefCol[itsReader.GetAttribute("name")],
                                   itsReader.GetAttribute("relatedPropName")));
                itsReader.Read();
            } while (itsReader.Name == "relProp");
        }
    }
}