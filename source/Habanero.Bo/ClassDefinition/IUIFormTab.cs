using System;
using System.Collections;

namespace Habanero.BO.ClassDefinition
{
    public interface IUIFormTab : ICollection
    {
        /// <summary>
        /// Adds a column definition to the collection of definitions
        /// </summary>
        /// <param name="column">The UIFormColumn object</param>
        void Add(IUIFormColumn column);

        /// <summary>
        /// Removes a column definition from the collection of definitions
        /// </summary>
        /// <param name="column">The UIFormColumn object</param>
        void Remove(IUIFormColumn column);

        /// <summary>
        /// Checks if a column definition is in the collection of definitions
        /// </summary>
        /// <param name="column">The UIFormColumn object</param>
        bool Contains(IUIFormColumn column);

        /// <summary>
        /// Copies the elements of the collection to an Array, 
        /// starting at a particular Array index
        /// </summary>
        /// <param name="array">The array to copy to</param>
        /// <param name="index">The zero-based index position to start
        /// copying from</param>
        void CopyTo(Array array, int index);

        /// <summary>
        /// Provides an indexing facility so that the contents of the definition
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to access</param>
        /// <returns>Returns the property definition at the index position
        /// specified</returns>
        IUIFormColumn this[int index] { get; }

        /// <summary>
        /// Returns the number of definitions held
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Returns the synchronisation root
        /// </summary>
        object SyncRoot { get; }

        /// <summary>
        /// Indicates whether the definitions are synchronised
        /// </summary>
        bool IsSynchronized { get; }

        /// <summary>
        /// Gets and sets the tab name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets and sets the UIFormGrid definition
        /// </summary>
        IUIFormGrid UIFormGrid { set; get; }

        /// <summary>
        /// Returns the <see cref="UIForm"/> that this <see cref="UIFormTab"/> is defined for.
        /// </summary>
        IUIForm UIForm { get; set; }

        /// <summary>
        /// Returns the definition list's enumerator
        /// </summary>
        /// <returns>Returns an IEnumerator-type object</returns>
        IEnumerator GetEnumerator();

        ///<summary>
        /// Clones the collection of ui columns this performs a copy of all uicolumns but does not copy the uiFormFields.
        ///</summary>
        ///<returns>a new collection that is a shallow copy of this collection</returns>
        IUIFormTab Clone();

        ///<summary>
        ///Indicates whether the current object is equal to another object of the same type.
        ///</summary>
        ///
        ///<returns>
        ///true if the current object is equal to the other parameter; otherwise, false.
        ///</returns>
        ///
        ///<param name="uiFormTab">An object to compare with this object.</param>
        bool Equals(IUIFormTab uiFormTab);

        ///<summary>
        /// Get the count of the Maximum number of fields
        ///</summary>
        ///<returns></returns>
        int GetMaxFieldCount();

        ///<summary>
        /// Get the Max rows In the Columns.
        ///</summary>
        ///<returns></returns>
        int GetMaxRowsInColumns();
    }
}