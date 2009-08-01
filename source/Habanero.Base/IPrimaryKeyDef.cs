using System.Collections;

namespace Habanero.Base
{
    public interface IPrimaryKeyDef : IEnumerable
    {
        /// <summary>
        /// Returns true if the primary key is a propery the object's ID, that is,
        /// the primary key is a single discrete property that is an immutable Guid and serves as the ID.
        /// </summary>
        bool IsGuidObjectID { get; set; }

        void Add(IPropDef propDef);
        string KeyName { get; set;  }
        int Count { get;  }

        ///<summary>
        /// Returns true if the primary key is a composite Key (i.e. if it consists of more than one property)
        ///</summary>
        bool IsCompositeKey { get; }

        /// <summary>
        /// Provides an indexing facility for the collection of property
        /// definitions that belong to the key, so that items
        /// in the collection can be accessed like an array 
        /// (e.g. collection["surname"])
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns the property stored under that property name</returns>
        IPropDef this[string propName] { get; }

        /// <summary>
        /// Provides an indexing facility for the collection of property
        /// definitions that belong to the key, so that items
        /// in the collection can be accessed like an array. The order is
        /// always the same, but not determinable
        /// </summary>
        /// <param name="index">The index of the property</param>
        /// <returns>Returns the property stored under that index</returns>
        IPropDef this[int index] { get; }
    }
}