using System;
using System.Collections;

namespace Chillisoft.Bo.ClassDefinition.v2
{
    /// <summary>
    /// Manages a collection of class definitions.
    /// </summary>
    public class ClassDefCol : DictionaryBase
    {
        private static ClassDefCol mClassDefcol;
        private static bool instanceFlag = false;

        /// <summary>
        /// Initialises an empty collection
        /// </summary>
        private ClassDefCol() : base()
        {
        }

        /// <summary>
        /// Returns the existing collection, or creates and returns a 
        /// new empty collection.
        /// </summary>
        /// <returns>A collection of class definitions</returns>
        internal static ClassDefCol GetColClassDef()
        {
            if (! instanceFlag)
            {
                mClassDefcol = new ClassDefCol();
                instanceFlag = true;
                return mClassDefcol;
            }
            else
            {
                return mClassDefcol;
            }
        }

        /// <summary>
        /// Provides an indexing facility for the collection so that items
        /// in the collection can be accessed like an array 
        /// (e.g. collection["surname"])
        /// </summary>
        /// <param name="key">The name of the class definition</param>
        /// <returns>Returns the class definition that matches the key
        /// or null if none is found</returns>
        public ClassDef this[Type key]
        {
            get
            {
                //TODO: Error if (this.Contains(key))
                return ((ClassDef) Dictionary[key]);
            }
        }

        /// <summary>
        /// Returns a collection of the key names being stored
        /// </summary>
        internal ICollection Keys
        {
            get { return (Dictionary.Keys); }
        }

        /// <summary>
        /// Returns a collection of the values being stored
        /// </summary>
        internal ICollection Values
        {
            get { return (Dictionary.Values); }
        }

        /// <summary>
        /// Adds a class definition to the collection
        /// </summary>
        /// <param name="key">The name of the class definition</param>
        /// <param name="value">The class definition to add</param>
        internal void Add(Type key, ClassDef value)
        {
            Dictionary.Add(key, value);
        }

        /// <summary>
        /// Indicates whether the collection contains a class definition
        /// by the name indicated
        /// </summary>
        /// <param name="key">The name of the class definition</param>
        /// <returns>Returns true if found, false if not</returns>
        internal bool Contains(Type key)
        {
            return (Dictionary.Contains(key));
        }

        /// <summary>
        /// Removes the class definition with the specified name from the
        /// collection.
        /// </summary>
        /// <param name="key">The name of the class definition</param>
        internal void Remove(Type key)
        {
            Dictionary.Remove(key);
        }

        /// <summary>
        /// Removes a flag that indicates that a collection exists.  After
        /// this flag is removed, calling GetColClassDef will result in a
        /// new empty collection replacing the existing one.
        /// </summary>
        protected void Finalize()
        {
            instanceFlag = false;
        }
    }

    #region "self Tests"

    #endregion
}