using System;
using System.Collections;
using Habanero.BO;
using Habanero.UI.Base;
using Habanero.UI.Forms;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Manages a collection of objects that are sub-types of ControlMapper
    /// </summary>
    public class ControlMapperCollection : ICollection
    {
        private IList _collection;
        private BusinessObject _businessObject;

        /// <summary>
        /// Contructor to initialise an empty collection
        /// </summary>
        public ControlMapperCollection()
        {
            Permission.Check(this);
            _collection = new ArrayList();
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
            _collection.CopyTo(array, index);
        }

        /// <summary>
        /// Returns the number of objects in the collection
        /// </summary>
        public int Count
        {
            get { return _collection.Count; }
        }

        /// <summary>
        /// Returns the collection's synchronisation root
        /// </summary>
        public object SyncRoot
        {
            get { return _collection.SyncRoot; }
        }

        /// <summary>
        /// Indicates whether the collection is synchronised
        /// </summary>
        public bool IsSynchronized
        {
            get { return _collection.IsSynchronized; }
        }

        /// <summary>
        /// Provides an enumerator of the collection
        /// </summary>
        /// <returns>Returns an enumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        /// <summary>
        /// Provides an indexing facility so that the collection can be
        /// accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to read</param>
        /// <returns>Returns the mapper at the position specified</returns>
        public ControlMapper this[int index]
        {
            get { return (ControlMapper) _collection[index]; }
        }

        /// <summary>
        /// Provides an indexing facility as before, but allows the objects
        /// to be referenced using their property names instead of their
        /// numerical position
        /// </summary>
        /// <param name="propertyName">The property name of the object</param>
        /// <returns>Returns the mapper if found, or null if not</returns>
        public ControlMapper this[string propertyName]
        {
            get
            {
                foreach (ControlMapper controlMapper in _collection)
                {
                    if (controlMapper.PropertyName == propertyName)
                    {
                        return controlMapper;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Adds a mapper object to the collection
        /// </summary>
        /// <param name="mapper">The object to add, which must be a type or
        /// sub-type of ControlMapper</param>
        public void Add(ControlMapper mapper)
        {
            this._collection.Add(mapper);
        }

        /// <summary>
        /// Gets and sets the business object being represented by
        /// the mapper collection.  Changes are applied to the business
        /// object represented by this collection and to each BO within the
        /// collection.
        /// </summary>
        public BusinessObject BusinessObject
        {
            get { return _businessObject; }
            set
            {
                foreach (ControlMapper controlMapper in _collection)
                {
                    controlMapper.BusinessObject = value;
                }
                _businessObject = value;
            }
        }
    }
}