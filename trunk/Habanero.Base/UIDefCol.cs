using System;
using System.Collections;

namespace Habanero.Base
{
    /// <summary>
    /// Manages a collection of user interface definitions
    /// </summary>
    public class UIDefCol : IEnumerable
    {
        private Hashtable _defs;

        /// <summary>
        /// Constructor to initialise a new empty collection
        /// </summary>
        public UIDefCol()
        {
            _defs = new Hashtable();
        }

        /// <summary>
        /// Adds a UI definition to the collection
        /// </summary>
        /// <param name="def">The UI definition to add</param>
        public void Add(UIDef def)
        {
            _defs.Add(def.Name, def);
        }

		public bool Contains(UIDef def)
		{
			return _defs.ContainsKey(def.Name);
		}

		public void Remove(UIDef def)
		{
			_defs.Remove(def.Name);
		}

        /// <summary>
        /// Provides an indexing facility so that the contents of the
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="name">The name of the definition to access</param>
        /// <returns>Returns the definition with the name specified</returns>
        public UIDef this[string name]
        {
            get { return (UIDef) this._defs[name]; }
        }

        /// <summary>
        /// Not yet implemented
        /// </summary>
        /// <returns></returns>
        /// TODO ERIC - implement
        public IEnumerator GetEnumerator()
        {
            //throw new NotImplementedException();
        	return _defs.GetEnumerator();
			//TODO: Is this right?
        }
    }
}