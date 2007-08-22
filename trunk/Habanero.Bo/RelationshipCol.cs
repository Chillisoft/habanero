using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.Base.Exceptions;
using Habanero.Util;
using log4net;

namespace Habanero.BO
{
    /// <summary>
    /// Manages a collection of relationships between business objects
    /// </summary>
    public class RelationshipCol : IRelationshipCol
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.BO.RelationshipCol");
        private BusinessObject _bo;
        private Dictionary<string, Relationship> _relationships;

        /// <summary>
        /// Constructor to initialise a new relationship, specifying the
        /// business object that owns the relationships
        /// </summary>
        /// <param name="bo">The business object</param>
        public RelationshipCol(BusinessObject bo)
        {
            _bo = bo;
            _relationships = new Dictionary<string, Relationship>();
        }

        /// <summary>
        /// Adds a relationship to the business object
        /// </summary>
        /// <param name="lRelationship">The relationship to add</param>
        public void Add(Relationship lRelationship)
        {
            if (_relationships.ContainsKey(lRelationship.RelationshipName))
            {
                throw new InvalidPropertyException(String.Format(
                    "A relationship with the name '{0}' is being added to a " +
                    "collection, but already exists in the collection.",
                    lRelationship.RelationshipName));
            }
            _relationships.Add(lRelationship.RelationshipName, lRelationship);
        }

        /// <summary>
        /// Adds a collection of relationships to the business object
        /// </summary>
        /// <param name="relCol">The collection of relationships to add</param>
        public void Add(RelationshipCol relCol)
        {
            foreach (Relationship rel in relCol)
            {
                this.Add(rel);
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
                if (!_relationships.ContainsKey(relationshipName))
                {
                    throw new RelationshipNotFoundException("The relationship " + relationshipName +
                                                            " was not found on a BusinessObject of type " +
                                                            this._bo.GetType().ToString());
                }
                return _relationships[relationshipName];
            }
		}

		#region IRelationshipCol Members

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
        public BusinessObject GetRelatedObject(string relationshipName)
        {
			return GetRelatedObject<BusinessObject>(relationshipName);
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
    	public T GetRelatedObject<T>(string relationshipName) where T : BusinessObject
    	{
			ArgumentValidationHelper.CheckStringArgumentNotEmpty(relationshipName, "relationshipName");
			Relationship relationship = this[relationshipName];
			if (relationship is MultipleRelationship)
			{
				throw new InvalidRelationshipAccessException("The 'multiple' relationship " + relationshipName +
															 " was accessed as a 'single' relationship (using GetRelatedObject()).");
			}
			return ((SingleRelationship)relationship).GetRelatedObject<T>(_bo.GetDatabaseConnection());
    	}

    	#endregion

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
		public BusinessObjectCollection<BusinessObject> GetRelatedCollection(string relationshipName)
		{
			return GetRelatedCollection<BusinessObject>(relationshipName);
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
        public BusinessObjectCollection<T> GetRelatedCollection<T>(string relationshipName)
			where T : BusinessObject
		{
            ArgumentValidationHelper.CheckStringArgumentNotEmpty(relationshipName, "relationshipName");
            Relationship relationship = this[relationshipName];
            if (relationship is SingleRelationship)
            {
                throw new InvalidRelationshipAccessException("The 'single' relationship " + relationshipName +
                                                             " was accessed as a 'multiple' relationship (using GetRelatedCollection()).");
            }
            return ((MultipleRelationship) relationship).GetRelatedBusinessObjectCol<T>();
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
        public void SetRelatedObject(string relationshipName, BusinessObject relatedObject)
        {
            Relationship relationship = this[relationshipName];
            if (relationship is MultipleRelationship)
            {
                throw new InvalidRelationshipAccessException("SetRelatedObject() was passed a relationship (" +
                                                             relationshipName +
                                                             ") that is of type 'multiple' when it expects a 'single' relationship");
            }
            ((SingleRelationship) this[relationshipName]).SetRelatedObject(relatedObject);
        }

        public IEnumerator<Relationship> GetEnumerator()
        {
            return _relationships.Values.GetEnumerator();
        }
    }
}