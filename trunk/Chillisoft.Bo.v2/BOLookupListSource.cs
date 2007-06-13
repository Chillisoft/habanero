using System;
using System.Collections;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Generic.v2;
using Chillisoft.Util.v2;

namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// Provides a lookup-list sourced from business object collections.
    /// A lookup-list is typically used to populate features like a ComboBox,
    /// where the string would be displayed, but the Guid would be the
    /// value stored (for reasons of data integrity).
    /// The string-Guid pair collection will be created using each object's
    /// ToString() function and the object's Guid ID.<br/>
    /// Note: this class does not provide criteria, so the entire collection
    /// will be loaded.
    /// </summary>
    public class BOLookupListSource : ILookupListSource
    {
        private readonly Type _boType;

        /// <summary>
        /// Constructor to initialise a new lookup-list
        /// </summary>
        /// <param name="boType">The business object type</param>
        public BOLookupListSource(Type boType)
        {
            _boType = boType;
        }

        /// <summary>
        /// Returns a lookup-list for all the business objects stored under
        /// the class definition held in this instance
        /// </summary>
        /// <returns>Returns a collection of string-Guid pairs</returns>
        public StringGuidPairCollection GetLookupList()
        {
            return this.GetLookupList(null);
        }

        /// <summary>
        /// Returns a lookup-list for all the business objects stored under
        /// the class definition held in this instance, using the database
        /// connection provided
        /// </summary>
        /// <param name="connection">The database connection</param>
        /// <returns>Returns a collection of string-Guid pairs</returns>
        public StringGuidPairCollection GetLookupList(IDatabaseConnection connection)
        {
            BusinessObjectCollection col = new BusinessObjectCollection(ClassDef.GetClassDefCol[_boType]);
            col.Load("", "");
            return CreateStringGuidPairCollection(col);
        }

        /// <summary>
        /// Returns a collection of all the business objects stored under
        /// the class definition held in this instance.  The collection contains
        /// a string version of each of the business objects.
        /// </summary>
        /// <returns>Returns an ICollection object</returns>
        public ICollection GetValueCollection()
        {
            BusinessObjectCollection col = new BusinessObjectCollection(ClassDef.GetClassDefCol[_boType]);
            col.Load("", "");
            return CreateValueList(col);
        }

        /// <summary>
        /// Populates a collection with a string version of each business
        /// object in the collection provided
        /// </summary>
        /// <param name="col">The business object collection</param>
        /// <returns>Returns an ICollection object</returns>
        private static ICollection CreateValueList(BusinessObjectCollection col)
        {
            SortedStringCollection valueList = new SortedStringCollection();
            foreach (BusinessObject bo in col)
            {
                valueList.Add(bo.ToString());
            }
            return valueList;
        }

        /// <summary>
        /// Returns a collection of string Guid pairs from the business object
        /// collection provided. Each pair consists of a string version of a
        /// business object and the object's ID.
        /// </summary>
        /// <param name="col">The business object collection</param>
        /// <returns>Returns a StringGuidPairCollection object</returns>
        public static StringGuidPairCollection CreateStringGuidPairCollection(BusinessObjectCollection col)
        {
            StringGuidPairCollection lookupList = new StringGuidPairCollection();
            foreach (BusinessObject bo in col)
            {
                lookupList.Add(new StringGuidPair(bo.ToString(), bo.ID.GetGuid()));
            }
            return lookupList;
        }
    }
}