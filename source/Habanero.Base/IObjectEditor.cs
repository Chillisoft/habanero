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
        bool EditObject(object obj, string uiDefName);
    }

    /// <summary>
    /// An interface to model an object editor
    /// </summary>
    public interface IObjectEditor<T> : IObjectEditor
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
        bool EditObject(T obj, string uiDefName);
    }
}