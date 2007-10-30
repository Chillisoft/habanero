//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

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