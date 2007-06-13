using System.Collections;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Generic.v2;
using log4net;
using System.Data;

namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// Initialises a relationship object
    /// </summary>
    public class RelationshipObjectInitialiser : IObjectInitialiser
    {
        private readonly string _correspondingRelationshipName;
        private static readonly ILog log = LogManager.GetLogger("Chillisoft.Bo.v2.RelationshipObjectInitialiser");
        private readonly RelationshipDef _relationship;
        private readonly BusinessObject _parentObject;

        /// <summary>
        /// Constructor for a new initialiser
        /// </summary>
        /// <param name="parentObject">The parent for the relationship</param>
        /// <param name="relationship">The relationship object</param>
        /// <param name="correspondingRelationshipName">The corresponding
        /// relationship name</param>
        /// TODO ERIC - corresponding?
        public RelationshipObjectInitialiser(BusinessObject parentObject, RelationshipDef relationship,
                                             string correspondingRelationshipName)
        {
            _parentObject = parentObject;
            _relationship = relationship;
            _correspondingRelationshipName = correspondingRelationshipName;
        }

        /// <summary>
        /// Initialises the given object
        /// </summary>
        /// <param name="objToInitialise">The object to initialise</param>
        public void InitialiseObject(object objToInitialise)
        {
            //log.Debug("Entered initialiseobject.") ;
            //log.Debug(objToInitialise.GetType().Name);
            BusinessObject newBo = (BusinessObject) objToInitialise;
            //log.Debug(_relationship);
            //log.Debug(_relationship.RelKeyDef.Count + " props in relkeydef. ");

            // TODO - this code should go in the SetRelatedBusinessObject method.
            foreach (DictionaryEntry relKeyDef in _relationship.RelKeyDef)
            {
                RelPropDef propDef = (RelPropDef) relKeyDef.Value;
                //log.Debug(propDef.OwnerPropertyName);
                //log.Debug(propDef.RelatedClassPropName);
                newBo.SetPropertyValue(propDef.OwnerPropertyName,
                                       _parentObject.GetPropertyValue(propDef.RelatedClassPropName));
            }
            newBo.Relationships.SetRelatedBusinessObject(_correspondingRelationshipName, _parentObject);
        }

        /// <summary>
        /// Initialises a DataRow object
        /// </summary>
        /// <param name="row">The DataRow object to initialise</param>
        public void InitialiseDataRow(DataRow row) {
        }
    }
}