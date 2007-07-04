using System;
using System.Collections;

namespace Habanero.Base
{
    /// <summary>
    /// Holds the property definitions for a column of controls in a user 
    /// interface editing form, as specified in the class definitions
    /// xml file
    /// </summary>
    public class UIFormColumn : ICollection
    {
        private IList _list;
        private int _width;

        /// <summary>
        /// Constructor to initialise a new column definition
        /// </summary>
        public UIFormColumn() : this(-1)
        {
        }

        /// <summary>
        /// Constructor to initialise a new column definition with the
        /// specified width
        /// </summary>
        /// <param name="width">The column width</param>
        public UIFormColumn(int width)
        {
            _width = width;
            _list = new ArrayList();
        }

        /// <summary>
        /// Adds a form property to the definition
        /// </summary>
        /// <param name="property">A form property definition</param>
        public void Add(UIFormProperty property)
        {
            _list.Add(property);
        }

		/// <summary>
		/// Removes a form property from the definition
		/// </summary>
		/// <param name="property">A form property definition</param>
		public void Remove(UIFormProperty property)
		{
			_list.Add(property);
		}

		/// <summary>
		/// Checks if a form property is in the definition
		/// </summary>
		/// <param name="property">A form property definition</param>
		public bool Contains(UIFormProperty property)
		{
			return _list.Contains(property);
		}

		/// <summary>
		/// Provides an indexing facility so that the contents of the definition
		/// collection can be accessed with square brackets like an array
		/// </summary>
		/// <param name="index">The index position to access</param>
		/// <returns>Returns the property definition at the index position
		/// specified</returns>
		public UIFormProperty this[int index]
		{
			get { return (UIFormProperty)_list[index]; }
		}


		/// <summary>
		/// Returns the number of property definitions held
		/// </summary>
		public int Count
		{
			get { return _list.Count; }
		}

		/// <summary>
		/// Returns the definition list's enumerator
		/// </summary>
		/// <returns>Returns an IEnumerator-type object</returns>
		public IEnumerator GetEnumerator()
		{
			return _list.GetEnumerator();
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
        /// Gets and sets the column width
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        //		public UIDefName Name {
        //			get { return _name; }
        //			set { _name = value; }
        //		}
    }
}