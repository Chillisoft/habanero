using System;
using System.Collections;

namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// Manages a collection of user interface definitions
    /// </summary>
    public class UIDefCol : IEnumerable
    {
        private Hashtable itsDefs;

        /// <summary>
        /// Constructor to initialise a new empty collection
        /// </summary>
        public UIDefCol()
        {
            itsDefs = new Hashtable();
        }

        /// <summary>
        /// Adds a UI definition to the collection
        /// </summary>
        /// <param name="def">The UI definition to add</param>
        public void Add(UIDef def)
        {
            itsDefs.Add(def.Name, def);
        }

        /// <summary>
        /// Provides an indexing facility so that the contents of the
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="name">The name of the definition to access</param>
        /// <returns>Returns the definition with the name specified</returns>
        public UIDef this[string name]
        {
            get { return (UIDef) this.itsDefs[name]; }
        }

        /// <summary>
        /// Not yet implemented
        /// </summary>
        /// <returns></returns>
        /// TODO ERIC - implement
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}