using System.Collections;

namespace Habanero.BO
{
    /// <summary>
    /// An interface to model a collection of relationships between
    /// business objects
    /// </summary>
    public interface IRelationshipCol
    {
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
        BusinessObject GetRelatedObject(string relationshipName);

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
		T GetRelatedObject<T>(string relationshipName) 
			where T: BusinessObject;

        /// <summary>
        /// Returns a collection of business objects that are connected to
        /// this object through the specified relationship (eg. would return
        /// a father and a mother if the relationship was "parents").  This
        /// method is to be used in the case of multiple relationships.
        /// </summary>
        /// <param name="relationshipName">The name of the relationship</param>
        /// <returns>Returns a business object collection</returns>
        BusinessObjectCollection<BusinessObject> GetRelatedCollection(string relationshipName);

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
		BusinessObjectCollection<T> GetRelatedCollection<T>(string relationshipName)
			where T : BusinessObject;

        /// <summary>
        /// Relates a business object to this object through the type of
        /// relationship specified (eg. could add a new child through a
        /// relationship called "children")
        /// </summary>
        /// <param name="relationshipName">The name of the relationship</param>
        /// <param name="parentObject">The object to relate to</param>
        void SetRelatedObject(string relationshipName, BusinessObject parentObject);

        /// <summary>
        /// Searches the relationship col for the relationship with the given name
        /// </summary>
        /// <param name="name">The relationship name</param>
        /// <returns>The Relationship object</returns>
        Relationship this[string name] { get;}

    }
}