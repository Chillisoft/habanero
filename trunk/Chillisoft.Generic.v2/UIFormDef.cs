using System;
using System.Collections;

namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// Manages a collection of property definitions for a user interface
    /// editing form, as specified in the class definitions xml file
    /// </summary>
    public class UIFormDef : ICollection
    {
        private IList itsList;
        private int itsWidth;
        private int itsHeight;
        private string itsHeading;

        /// <summary>
        /// Constructor to initialise a new definition
        /// </summary>
        public UIFormDef()
        {
            itsList = new ArrayList();
        }

        /// <summary>
        /// Adds a tab to the form
        /// </summary>
        /// <param name="tab">A UIFormTab object</param>
        public void Add(UIFormTab tab)
        {
            itsList.Add(tab);
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
        public UIFormTab this[int index]
        {
            get { return (UIFormTab) itsList[index]; }
        }

        /// <summary>
        /// Gets and sets the width
        /// </summary>
        public int Width
        {
            set { itsWidth = value; }
            get { return itsWidth; }
        }

        /// <summary>
        /// Gets and sets the height
        /// </summary>
        public int Height
        {
            set { itsHeight = value; }
            get { return itsHeight; }
        }

        /// <summary>
        /// Gets and sets the heading
        /// </summary>
        public string Heading
        {
            set { itsHeading = value; }
            get { return itsHeading; }
        }
    }
}