using System;

namespace Habanero.Base
{
    /// <summary>
    /// An interface to model a tool that lists objects
    /// </summary>
    public interface IObjectLister
    {
        event EventHandler ItemActioned;
        event EventHandler ItemSelected;

        /// <summary>
        /// Sets the parent object
        /// </summary>
        /// <param name="parentObject">The parent object</param>
        void SetParentObject(object parentObject);

        /// <summary>
        /// Sets the text of the action button
        /// </summary>
        /// <param name="text">The text to display on the button</param>
        void SetActionButtonText(string text);

        /// <summary>
        /// Returns the currently selected object
        /// </summary>
        /// <returns>Returns the currently selected object</returns>
        object GetSelectedObject();

        /// <summary>
        /// Removes the specified object from the list
        /// </summary>
        /// <param name="objectToRemove">The object to remove</param>
        void RemoveObject(object objectToRemove);

        /// <summary>
        /// Adds the specified object to the list
        /// </summary>
        /// <param name="objectToAdd">The object to add</param>
        void AddObject(object objectToAdd);
    }
}