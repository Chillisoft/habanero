using System;
using System.Collections;

namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// Manages property definitions for a tab in a user interface editing 
    /// form, as specified in the class definitions xml file
    /// </summary>
    public class UIFormTab : ICollection
    {
        private IList _list;
        private string _name;
        private UIFormGrid _uiFormGrid;
        //private UIDefName _name;

        /// <summary>
        /// Constructor to initialise a new tab definition
        /// </summary>
        public UIFormTab() : this("")
        {
        }

        /// <summary>
        /// Constructor to initialise a new tab definition with a tab name
        /// </summary>
        /// <param name="name">The tab name</param>
        public UIFormTab(string name)
        {
            _name = name;
            _list = new ArrayList();
        }

        /// <summary>
        /// Adds a column definition to the collection of definitions
        /// </summary>
        /// <param name="column">The UIFormColumn object</param>
        public void Add(UIFormColumn column)
        {
            _list.Add(column);
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

        /// <summary>
        /// Provides an indexing facility so that the contents of the definition
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to access</param>
        /// <returns>Returns the property definition at the index position
        /// specified</returns>
        public UIFormColumn this[int index]
        {
            get { return (UIFormColumn) _list[index]; }
        }

        /// <summary>
        /// Gets and sets the tab name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets and sets the UIFormGrid definition
        /// </summary>
        public UIFormGrid UIFormGrid
        {
            set { _uiFormGrid = value; }
            get { return _uiFormGrid; }
        }

        //		public UIDefName Name {
        //			get { return _name; }
        //			set { _name = value; }
        //		}
    }
}