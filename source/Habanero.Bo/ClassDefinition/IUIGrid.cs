using System;
using System.Collections;

namespace Habanero.BO.ClassDefinition
{
    public interface IUIGrid : ICollection
    {
        /// <summary>
        /// Adds a grid property definition
        /// </summary>
        /// <param name="prop">The grid property definition</param>
        void Add(IUIGridColumn prop);

        /// <summary>
        /// Removes a grid property definition
        /// </summary>
        /// <param name="prop">The grid property definition</param>
        void Remove(IUIGridColumn prop);

        /// <summary>
        /// Checks if a grid property definition is in the Grid definition
        /// </summary>
        /// <param name="prop">The grid property definition</param>
        bool Contains(IUIGridColumn prop);

        /// <summary>
        /// Provides an indexing facility so that the contents of the definition
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to access</param>
        /// <returns>Returns the property definition at the index position
        /// specified</returns>
        IUIGridColumn this[int index] { get; }

        /// <summary>
        /// Provides an indexing facility so that the contents of the definition
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="propName">The index position to access</param>
        /// <returns>Returns the property definition at the index position
        /// specified</returns>
        IUIGridColumn this[string propName] { get; }

        /// <summary>
        /// Copies the elements of the collection to an Array, 
        /// starting at a particular Array index
        /// </summary>
        /// <param name="array">The array to copy to</param>
        /// <param name="index">The zero-based index position to start
        /// copying from</param>
        void CopyTo(Array array, int index);

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
        /// The column on which rows are ordered initially.
        /// Indicate the direction by adding " asc" or " desc"
        /// after the column name (" asc" is assumed if left out).  If this
        /// property is not specified, rows will be listed in the order
        /// they were added to the database.
        /// </summary>
        string SortColumn { get; set; }

        ///<summary>
        /// The definition of the filter that will be used for this grid.
        ///</summary>
        IFilterDef FilterDef { get; set; }

        /// <summary>
        /// Returns the definition list's enumerator
        /// </summary>
        /// <returns>Returns an IEnumerator-type object</returns>
        IEnumerator GetEnumerator();

        ///<summary>
        /// Clones the collection of ui columns this performs a copy of all uicolumns but does not copy the uiFormFields.
        ///</summary>
        ///<returns>a new collection that is a shallow copy of this collection</returns>
        IUIGrid Clone();

        /// <summary>
        /// Determines whether this object is equal to obj.
        /// </summary>
        /// <param name="obj">The object being compared to</param>
        /// <returns></returns>
        bool Equals(IUIGrid obj);

    }
}