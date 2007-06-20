using System.Collections;

namespace Habanero.Bo
{
    /// <summary>
    /// An interface to model a collection of relationships between
    /// business objects
    /// </summary>
    public interface IRelationshipCol : IDictionary
    {
        /// <summary>
        /// Returns the business object that is related to this object
        /// through the specified relationship (eg. would return a father
        /// if the relationship was called "father").  This method is to be
        /// used in the case of single relationships.
        /// </summary>
        /// <param name="relationshipName">The name of the relationship</param>
        /// <returns>Returns a business object</returns>
        BusinessObject GetRelatedBusinessObject(string relationshipName);

        /// <summary>
        /// Returns a collection of business objects that are connected to
        /// this object through the specified relationship (eg. would return
        /// a father and a mother if the relationship was "parents").  This
        /// method is to be used in the case of multiple relationships.
        /// </summary>
        /// <param name="relationshipName">The name of the relationship</param>
        /// <returns>Returns a business object collection</returns>
        BusinessObjectCollection GetRelatedBusinessObjectCol(string relationshipName);

        /// <summary>
        /// Relates a business object to this object through the type of
        /// relationship specified (eg. could add a new child through a
        /// relationship called "children")
        /// </summary>
        /// <param name="relationshipName">The name of the relationship</param>
        /// <param name="parentObject">The object to relate to</param>
        void SetRelatedBusinessObject(string relationshipName, BusinessObject parentObject);
    }
}