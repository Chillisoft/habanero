//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

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
        object CreateObject(IObjectEditor editor, string uiDefName);

        /// <summary>
        /// Creates an object
        /// </summary>
        /// <param name="editor">An object editor</param>
        /// <param name="initialiser">An object initialiser</param>
        /// <param name="uiDefName">The name of the set of ui definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a ui definition with no name attribute specified.</param>
        /// <returns>Returns the object created</returns>
        object CreateObject(IObjectEditor editor, IObjectInitialiser initialiser, string uiDefName);

        /// <summary>
        /// Just creates the object, without editing or saving it.
        /// </summary>
        /// <returns></returns>
        object CreateObject();


    }

    ///<summary>
    /// An abstract base class for your own ObjectCreators, created for convenience as it is strongly typed.
    ///</summary>
    ///<typeparam name="T">The type of BO this creator creates.</typeparam>
    public abstract class ObjectCreator<T> : IObjectCreator
    {
        #region IObjectCreator Members

        object IObjectCreator.CreateObject(IObjectEditor editor, string uiDefName)
        {
            return CreateObject(editor, uiDefName);
        }

        object IObjectCreator.CreateObject(IObjectEditor editor, IObjectInitialiser initialiser, string uiDefName)
        {
            return CreateObject(editor, initialiser, uiDefName);
        }

        object IObjectCreator.CreateObject()
        {
            return CreateObject();
        }

        #endregion
        /// <summary>
        /// Creates an object
        /// </summary>
        /// <param name="editor">An object editor</param>
        /// <param name="uiDefName">The name of the set of ui definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a ui definition with no name attribute specified.</param>
        /// <returns>Returns the object created</returns>
        public abstract T CreateObject(IObjectEditor editor, string uiDefName);

        /// <summary>
        /// Creates an object
        /// </summary>
        /// <param name="editor">An object editor</param>
        /// <param name="initialiser">An object initialiser</param>
        /// <param name="uiDefName">The name of the set of ui definitions
        /// used to design the edit form. Setting this to an empty string
        /// will use a ui definition with no name attribute specified.</param>
        /// <returns>Returns the object created</returns>
        public abstract T CreateObject(IObjectEditor editor, IObjectInitialiser initialiser, string uiDefName);

        /// <summary>
        /// Just creates the object, without editing or saving it.
        /// </summary>
        /// <returns></returns>
        public abstract T CreateObject();
    }
}