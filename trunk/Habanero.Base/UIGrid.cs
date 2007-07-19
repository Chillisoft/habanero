using System;
using System.Collections;

namespace Habanero.Base
{
    /// <summary>
    /// Manages property definitions for a user interface grid, as specified
    /// in the class definitions xml file
    /// </summary>
    public class UIGrid : ICollection
    {
        private IList _list;

        /// <summary>
        /// Constructor to initialise a new collection of definitions
        /// </summary>
        public UIGrid()
        {
            _list = new ArrayList();
        }

		/// <summary>
		/// Adds a grid property definition
		/// </summary>
		/// <param name="prop">The grid property definition</param>
		public void Add(UIGridColumn prop)
		{
			_list.Add(prop);
		}

		/// <summary>
		/// Removes a grid property definition
		/// </summary>
		/// <param name="prop">The grid property definition</param>
		public void Remove(UIGridColumn prop)
		{
			_list.Remove(prop);
		}

		/// <summary>
		/// Checks if a grid property definition is in the Grid definition
		/// </summary>
		/// <param name="prop">The grid property definition</param>
		public bool Contains(UIGridColumn prop)
		{
			return _list.Contains(prop);
		}
		
		/// <summary>
		/// Provides an indexing facility so that the contents of the definition
		/// collection can be accessed with square brackets like an array
		/// </summary>
		/// <param name="index">The index position to access</param>
		/// <returns>Returns the property definition at the index position
		/// specified</returns>
		public UIGridColumn this[int index]
		{
			get { return (UIGridColumn)_list[index]; }
		}

        /// <summary>
        /// Copies the elements of the collection to an Array, 
        /// starting at a particular Array index
        /// </summary>
        /// <param name="array">The array to copy to</param>
        /// <param name="index">The zero-based index position to start
        /// copying from</param>
        public void CopyTo(Array array, int index)
        {
            _list.CopyTo(array, index);
        }

        /// <summary>
        /// Returns the number of definitions held
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// Returns the synchronisation root
        /// </summary>
        public object SyncRoot
        {
            get { return _list.SyncRoot; }
        }

        /// <summary>
        /// Indicates whether the definitions are synchronised
        /// </summary>
        public bool IsSynchronized
        {
            get { return _list.IsSynchronized; }
        }

        /// <summary>
        /// Returns the definition list's enumerator
        /// </summary>
        /// <returns>Returns an IEnumerator-type object</returns>
        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

    }
}