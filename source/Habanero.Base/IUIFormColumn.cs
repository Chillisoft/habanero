using System.Collections;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// An interface describing a column of fields on a form.  This is implemented by <see cref="UIFormColumn"/>.
    /// </summary>
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
        /// Gets and sets the column width
        /// </summary>
        int Width { get; set; }

        ///<summary>
        /// Returns the Form tab that this UIFormColumn is on.
        ///</summary>
        IUIFormTab UIFormTab { get; set; }

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