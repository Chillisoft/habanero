using System;
using System.Collections;

namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// Holds the property definitions for a column of controls in a user 
    /// interface editing form, as specified in the class definitions
    /// xml file
    /// </summary>
    public class UIFormColumn : ICollection
    {
        private IList itsList;
        private int itsWidth;

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
            itsWidth = width;
            itsList = new ArrayList();
        }

        /// <summary>
        /// Adds a form property to the definition
        /// </summary>
        /// <param name="property">A form property definition</param>
        public void Add(UIFormProperty property)
        {
            itsList.Add(property);
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
        /// Returns the number of property definitions held
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
        public UIFormProperty this[int index]
        {
            get { return (UIFormProperty) itsList[index]; }
        }

        /// <summary>
        /// Gets and sets the column width
        /// </summary>
        public int Width
        {
            get { return itsWidth; }
            set { itsWidth = value; }
        }

        //		public UIDefName Name {
        //			get { return itsName; }
        //			set { itsName = value; }
        //		}
    }
}