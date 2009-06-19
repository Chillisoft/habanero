using System;
using System.Collections;

namespace Habanero.BO.ClassDefinition
{
    public interface IUIFormColumn : ICollection
    {
        /// <summary>
        /// Adds a form field to the definition
        /// </summary>
        /// <param name="field">A form field definition</param>
        void Add(IUIFormField field);

        /// <summary>
        /// Removes a form field from the definition
        /// </summary>
        /// <param name="field">A form field definition</param>
        void Remove(IUIFormField field);

        /// <summary>
        /// Checks if a form field is in the definition
        /// </summary>
        /// <param name="field">A form field definition</param>
        bool Contains(IUIFormField field);

        /// <summary>
        /// Provides an indexing facility so that the contents of the definition
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to access</param>
        /// <returns>Returns the property definition at the index position
        /// specified</returns>
        IUIFormField this[int index] { get; }

        /// <summary>
        /// Returns the number of property definitions held
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
        /// Gets and sets the column width
        /// </summary>
        int Width { get; set; }

        ///<summary>
        /// Returns the Form tab that this UIFormColumn is on.
        ///</summary>
        IUIFormTab UIFormTab { get; set; }

        /// <summary>
        /// Returns the definition list's enumerator
        /// </summary>
        /// <returns>Returns an IEnumerator-type object</returns>
        IEnumerator GetEnumerator();

        /// <summary>
        /// Copies the elements of the collection to an Array, 
        /// starting at a particular Array index
        /// </summary>
        /// <param name="array">The array to copy to</param>
        /// <param name="index">The zero-based index position to start
        /// copying from</param>
        void CopyTo(Array array, int index);

        ///<summary>
        /// Clones the collection.
        ///</summary>
        ///<returns>a new collection that is a shallow copy of this collection</returns>
        IUIFormColumn Clone();

        ///<summary>
        /// Returns the Number of rows required to draw this UIFormColumn
        ///</summary>
        ///<returns></returns>
        int GetRowsRequired();

        ///<summary>
        /// Returns the Row span of the column to the right of this UIcolumn
        ///</summary>
        ///<param name="columnsRight"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentException"></exception>
        int GetRowSpanForColumnToTheRight(int columnsRight);

        ///<summary>
        ///Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        ///</summary>
        ///
        ///<returns>
        ///true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        ///</returns>
        ///
        ///<param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>. </param><filterpriority>2</filterpriority>
        bool Equals(IUIFormColumn obj);

        ///<summary>
        /// Inserts a formField. at the specified index.
        /// If Index Less than or equal to zero then the form field wil be inserted at the first postion
        /// If the index is greater than the Count of the list then it will be inserted at the last position.
        ///</summary>
        ///<param name="index">The position at which the formField should be inserted</param>
        ///<param name="formField">The FormField to be iserted.</param>
        void Insert(int index, IUIFormField formField);
    }
}