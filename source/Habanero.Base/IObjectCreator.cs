using System;

namespace Habanero.Base
{
    /// <summary>
    /// An interface to model an object creator
    /// </summary>
    public interface IObjectCreator
    {
        /// <summary>
        /// Creates an object
        /// </summary>
        /// <param name="editor">An object editor</param>
        /// <param name="uiDefName">The name of the set of ui definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a ui definition with no name attribute specified.</param>
        /// <returns>Returns the object created</returns>
        Object CreateObject(IObjectEditor editor, string uiDefName);

        /// <summary>
        /// Creates an object
        /// </summary>
        /// <param name="editor">An object editor</param>
        /// <param name="initialiser">An object initialiser</param>
        /// <param name="uiDefName">The name of the set of ui definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a ui definition with no name attribute specified.</param>
        /// <returns>Returns the object created</returns>
        Object CreateObject(IObjectEditor editor, IObjectInitialiser initialiser, string uiDefName);
    }
}