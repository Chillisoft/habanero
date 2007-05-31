using System;
using System.Collections;

namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// Manages property definitions for a user interface grid, as specified
    /// in the class definitions xml file
    /// </summary>
    public class UIGridDef : ICollection
    {
        private IList itsList;

        /// <summary>
        /// Constructor to initialise a new collection of definitions
        /// </summary>
        public UIGridDef()
        {
            itsList = new ArrayList();
        }

        /// <summary>
        /// Adds a grid property definition
        /// </summary>
        /// <param name="prop">The grid property definition</param>
        public void Add(UIGridProperty prop)
        {
            itsList.Add(prop);
        }

        /// <summary>
        /// Not yet implemented
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// TODO ERIC - implement
        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the number of definitions held
        /// </summary>
        public int Count
        {
            get { return itsList.Count; }
        }

        /// <summary>
        /// Returns the synchronisation root
        /// </summary>
        public object SyncRoot
        {
            get { return itsList.SyncRoot; }
        }

        /// <summary>
        /// Indicates whether the definitions are synchronised
        /// </summary>
        public bool IsSynchronized
        {
            get { return itsList.IsSynchronized; }
        }

        /// <summary>
        /// Returns the definition list's enumerator
        /// </summary>
        /// <returns>Returns an IEnumerator-type object</returns>
        public IEnumerator GetEnumerator()
        {
            return itsList.GetEnumerator();
        }

        /// <summary>
        /// Provides an indexing facility so that the contents of the definition
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to access</param>
        /// <returns>Returns the property definition at the index position
        /// specified</returns>
        public UIGridProperty this[int index]
        {
            get { return (UIGridProperty) itsList[index]; }
        }
    }
}