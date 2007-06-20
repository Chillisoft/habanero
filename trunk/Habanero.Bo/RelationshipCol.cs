using System.Collections;
using Habanero.Util;
using log4net;

namespace Habanero.Bo
{
    /// <summary>
    /// Manages a collection of relationships between business objects
    /// </summary>
    /// TODO ERIC - where is set for multiple rel's?
    public class RelationshipCol : DictionaryBase, IRelationshipCol
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.Bo.RelationshipCol");
        private BusinessObject _bo;

        /// <summary>
        /// Constructor to initialise a new relationship, specifying the
        /// business object that owns the relationships
        /// </summary>
        /// <param name="bo">The business object</param>
        public RelationshipCol(BusinessObject bo)
        {
            _bo = bo;
        }

        /// <summary>
        /// Adds a relationship to the business object
        /// </summary>
        /// <param name="lRelationship">The relationship to add</param>
        public void Add(Relationship lRelationship)
        {
            //TODO_Err: Add sensible error handling if prop already exists etc
            base.Dictionary.Add(lRelationship.RelationshipName, lRelationship);
        }

        /// <summary>
        /// Adds a collection of relationships to the business object
        /// </summary>
        /// <param name="relCol">The collection of relationships to add</param>
        public void Add(RelationshipCol relCol)
        {
            foreach (DictionaryEntry entry in relCol)
            {
                this.Add((Relationship) (entry.Value));
            }
        }

        /// <summary>
        /// Provides an indexing facility so the relationships can be
        /// accessed with square brackets like an array.
        /// Returns the relationship with the given name.
        /// </summary>
        /// <exception cref="RelationshipNotFoundException">Thrown
        /// if a relationship with the given name is not found</exception>
        public Relationship this[string relationshipName]
        {
            get
            {
                Relationship foundRel = ((Relationship) Dictionary[relationshipName]);
                if (foundRel == null)
                {
                    throw new RelationshipNotFoundException("The relationship " + relationshipName +
                                                            " was not found on a BusinessObject of type " +
                                                            this._bo.GetType().ToString());
                }
                return foundRel;
            }
        }

        /// <summary>
        /// Returns the business object that is related to this object
        /// through the specified relationship (eg. would return a father
        /// if the relationship was called "father").  This method is to be
        /// used in the case of single relationships.
        /// </summary>
        /// <param name="relationshipName">The name of the relationship</param>
        /// <returns>Returns a business object</returns>
        /// <exception cref="InvalidRelationshipAccessException">Thrown if
        /// the relationship specified is a multiple relationship, when a
        /// single one was expected</exception>
        public BusinessObject GetRelatedBusinessObject(string relationshipName)
        {
            ArgumentValidationHelper.CheckStringArgumentNotEmpty(relationshipName, "relationshipName");
            Relationship relationship = this[relationshipName];
            if (relationship is MultipleRelationship)
            {
                throw new InvalidRelationshipAccessException("The 'multiple' relationship " + relationshipName +
                                                             " was accessed as a 'single' relationship (using GetRelatedBusinessObject()).");
            }
            return ((SingleRelationship) relationship).GetRelatedObject(_bo.GetDatabaseConnection());
        }

        /// <summary>
        /// Returns a collection of business objects that are connected to
        /// this object through the specified relationship (eg. would return
        /// a father and a mother if the relationship was "parents").  This
        /// method is to be used in the case of multiple relationships.
        /// </summary>
        /// <param name="relationshipName">The name of the relationship</param>
        /// <returns>Returns a business object collection</returns>
        /// <exception cref="InvalidRelationshipAccessException">Thrown if
        /// the relationship specified is a single relationship, when a
        /// multiple one was expected</exception>
        public BusinessObjectCollection GetRelatedBusinessObjectCol(string relationshipName)
        {
            ArgumentValidationHelper.CheckStringArgumentNotEmpty(relationshipName, "relationshipName");
            Relationship relationship = this[relationshipName];
            if (relationship is SingleRelationship)
            {
                throw new InvalidRelationshipAccessException("The 'single' relationship " + relationshipName +
                                                             " was accessed as a 'multiple' relationship (using GetRelatedBusinessObjectCol()).");
            }
            return ((MultipleRelationship) relationship).GetRelatedBusinessObjectCol();
        }

        /// <summary>
        /// Sets the related business object of a <b>single</b> relationship.  
        /// This includes setting the RelKey properties of the relationship owner 
        /// to the value of the related object's properties.
        /// </summary>
        /// <param name="relationshipName">The name of a single relationship</param>
        /// <param name="relatedObject">The new related object</param>
        /// <exception cref="InvalidRelationshipAccessException">Thrown if
        /// the relationship named is a multiple relationship instead of a
        /// single one</exception>
        public void SetRelatedBusinessObject(string relationshipName, BusinessObject relatedObject)
        {
            Relationship relationship = this[relationshipName];
            if (relationship is MultipleRelationship)
            {
                throw new InvalidRelationshipAccessException("SetRelatedBusinessObject() was passed a relationship (" +
                                                             relationshipName +
                                                             ") that is of type 'multiple' when it expects a 'single' relationship");
            }
            ((SingleRelationship) this[relationshipName]).SetRelatedObject(relatedObject);
        }
    }
}