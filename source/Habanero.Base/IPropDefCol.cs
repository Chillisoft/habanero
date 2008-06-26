using System;
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO.Base
{
    public interface IPropDefCol: IEnumerable<IPropDef>
    {
        /// <summary>
        /// Provides an indexing facility for the collection so that items
        /// in the collection can be accessed like an array 
        /// (e.g. collection["surname"])
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the propertyName is not
        /// found. If you are checking for the existence of a propertyName, use the
        /// Contains() method.</exception>
        IPropDef this[string propertyName] { get; }

        int Count
        {
            get;
        }

        /// <summary>
        /// Creates a business object property collection that mirrors
        /// this one.  The new collection will contain a BOProp object for
        /// each PropDef object in this collection, with that BOProp object
        /// storing an instance of the PropDef object.
        /// </summary>
        /// <param name="newObject">Whether the new BOProps in the
        /// collection will be new objects. See PropDef.CreateBOProp
        /// for more info.</param>
        /// <returns>Returns the new BOPropCol object</returns>
        BOPropCol CreateBOPropertyCol(bool newObject);

        /// <summary>
        /// Add an existing property definition to the collection
        /// </summary>
        /// <param name="propDef">The existing property definition</param>
        void Add(IPropDef propDef);

        ///<summary>
        /// Clones the propdefcol.  The new propdefcol has the same propdefs in it.
        ///</summary>
        ///<returns></returns>
        IPropDefCol Clone();

        /// <summary>
        /// Indicates if the specified property definition exists
        /// in the collection.
        /// </summary>
        /// <param name="propDef">The Property definition to search for</param>
        /// <returns>Returns true if found, false if not</returns>
        bool Contains(IPropDef propDef);

        /// <summary>
        /// Indicates if a property definition with the given key exists
        /// in the collection.
        /// </summary>
        /// <param name="propertyName">The propertyName to match</param>
        /// <returns>Returns true if found, false if not</returns>
        bool Contains(string propertyName);
    }
}