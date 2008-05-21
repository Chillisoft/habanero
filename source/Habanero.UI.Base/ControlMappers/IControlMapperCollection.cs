using System;
using System.Collections;
using Habanero.BO;

namespace Habanero.UI.Base
{
    public interface IControlMapperCollection
    {
        /// <summary>
        /// Copies the elements of the collection to an Array, 
        /// starting at a particular Array index
        /// </summary>
        /// <param name="array">The array to copy to</param>
        /// <param name="index">The zero-based index position to start
        /// copying from</param>
        void CopyTo(Array array, int index);

        /// <summary>
        /// Returns the number of objects in the collection
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Returns the collection's synchronisation root
        /// </summary>
        object SyncRoot { get; }

        /// <summary>
        /// Indicates whether the collection is synchronised
        /// </summary>
        bool IsSynchronized { get; }

        /// <summary>
        /// Provides an enumerator of the collection
        /// </summary>
        /// <returns>Returns an enumerator</returns>
        IEnumerator GetEnumerator();

        /// <summary>
        /// Provides an indexing facility so that the collection can be
        /// accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to read</param>
        /// <returns>Returns the mapper at the position specified</returns>
        IControlMapper this[int index] { get; }

        /// <summary>
        /// Provides an indexing facility as before, but allows the objects
        /// to be referenced using their property names instead of their
        /// numerical position
        /// </summary>
        /// <param name="propertyName">The property name of the object</param>
        /// <returns>Returns the mapper if found, or null if not</returns>
        IControlMapper this[string propertyName] { get; }

        /// <summary>
        /// Adds a mapper object to the collection
        /// </summary>
        /// <param name="mapper">The object to add, which must be a type or
        /// sub-type of ControlMapper</param>
        void Add(IControlMapper mapper);

        /// <summary>
        /// Gets and sets the business object being represented by
        /// the mapper collection.  Changes are applied to the business
        /// object represented by this collection and to each BO within the
        /// collection.
        /// </summary>
        BusinessObject BusinessObject { get; set; }

        /// <summary>
        /// Enables or Disables all the controls managed in this control mapper collection.
        /// </summary>
        bool ControlsEnabled { set; }

        /// <summary>
        /// Applies the values of the controls to the business object this collection
        /// is mapped to.  Similar to calling ApplyChangesToBusinessObject on each mapper
        /// in this collecction.
        /// </summary>
        void ApplyChangesToBusinessObject();
    }
}