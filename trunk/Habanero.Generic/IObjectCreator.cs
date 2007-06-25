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
        /// <returns>Returns the object created</returns>
        Object CreateObject(IObjectEditor editor);

        /// <summary>
        /// Creates an object
        /// </summary>
        /// <param name="editor">An object editor</param>
        /// <param name="initialiser">An object initialiser</param>
        /// <returns>Returns the object created</returns>
        Object CreateObject(IObjectEditor editor, IObjectInitialiser initialiser);
    }
}