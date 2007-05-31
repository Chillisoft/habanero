using System;
using System.Collections;
using Chillisoft.Bo.v2;

namespace Chillisoft.UI.BOControls.v2
{
    /// <summary>
    /// Manages a collection of objects that are sub-types of ControlMapper
    /// </summary>
    public class ControlMapperCollection : ICollection
    {
        private IList itsCollection;
        private BusinessObjectBase itsBusinessObject;

        /// <summary>
        /// Contructor to initialise an empty collection
        /// </summary>
        public ControlMapperCollection()
        {
            itsCollection = new ArrayList();
        }

        /// <summary>
        /// This method not yet implemented
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// TODO ERIC - needs to be implemented
        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the number of objects in the collection
        /// </summary>
        public int Count
        {
            get { return itsCollection.Count; }
        }

        /// <summary>
        /// Returns the collection's synchronisation root
        /// </summary>
        public object SyncRoot
        {
            get { return itsCollection.SyncRoot; }
        }

        /// <summary>
        /// Indicates whether the collection is synchronised
        /// </summary>
        public bool IsSynchronized
        {
            get { return itsCollection.IsSynchronized; }
        }

        /// <summary>
        /// Provides an enumerator of the collection
        /// </summary>
        /// <returns>Returns an enumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return itsCollection.GetEnumerator();
        }

        /// <summary>
        /// Provides an indexing facility so that the collection can be
        /// accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to read</param>
        /// <returns>Returns the mapper at the position specified</returns>
        public ControlMapper this[int index]
        {
            get { return (ControlMapper) itsCollection[index]; }
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
                foreach (ControlMapper controlMapper in itsCollection)
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
            this.itsCollection.Add(mapper);
        }

        /// <summary>
        /// Gets and sets the business object being represented by
        /// the mapper collection.  Changes are applied to the business
        /// object represented by this collection and to each BO within the
        /// collection.
        /// </summary>
        public BusinessObjectBase BusinessObject
        {
            get { return itsBusinessObject; }
            set
            {
                foreach (ControlMapper controlMapper in itsCollection)
                {
                    controlMapper.BusinessObject = value;
                }
                itsBusinessObject = value;
            }
        }
    }
}