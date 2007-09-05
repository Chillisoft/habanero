using System;

namespace Habanero.Base
{
    /// <summary>
    /// An interface to model an object editor
    /// </summary>
    public interface IObjectEditor
    {
        /// <summary>
        /// Edits the given object
        /// </summary>
        /// <param name="obj">The object to edit</param>
        /// <param name="uiDefName">The name of the set of ui definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a ui definition with no name attribute specified.</param>
        /// <returns>Returs true if edited successfully of false if the edits
        /// were cancelled</returns>
        bool EditObject(Object obj, string uiDefName);
    }
}