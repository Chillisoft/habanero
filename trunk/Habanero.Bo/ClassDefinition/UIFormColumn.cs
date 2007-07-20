using System;
using System.Collections;

namespace Habanero.Bo.ClassDefinition
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
        /// Adds a form field to the definition
        /// </summary>
        /// <param name="field">A form field definition</param>
        public void Add(UIFormField field)
        {
            _list.Add(field);
        }

        /// <summary>
        /// Removes a form field from the definition
        /// </summary>
        /// <param name="field">A form field definition</param>
        public void Remove(UIFormField field)
        {
            _list.Add(field);
        }

        /// <summary>
        /// Checks if a form field is in the definition
        /// </summary>
        /// <param name="field">A form field definition</param>
        public bool Contains(UIFormField field)
        {
            return _list.Contains(field);
        }

        /// <summary>
        /// Provides an indexing facility so that the contents of the definition
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to access</param>
        /// <returns>Returns the property definition at the index position
        /// specified</returns>
        public UIFormField this[int index]
        {
            get { return (UIFormField)_list[index]; }
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