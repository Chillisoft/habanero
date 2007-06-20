using System;

namespace Habanero.Generic
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
        /// <returns>Returs true if edited successfully of false if the edits
        /// were cancelled</returns>
        bool EditObject(Object obj);
    }
}