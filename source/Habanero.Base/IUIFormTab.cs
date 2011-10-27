#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System.Collections;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Provides an interface for a Form tab - that is, a tab that contains one or more columns of fields on it.
    /// </summary>
    public interface IUIFormTab : ICollection
    {
        /// <summary>
        /// Adds a column definition to the collection of definitions
        /// </summary>
        /// <param name="column">The UIFormColumn object</param>
        void Add(IUIFormColumn column);

        /// <summary>
        /// Removes a column definition from the collection of definitions
        /// </summary>
        /// <param name="column">The UIFormColumn object</param>
        void Remove(IUIFormColumn column);

        /// <summary>
        /// Checks if a column definition is in the collection of definitions
        /// </summary>
        /// <param name="column">The UIFormColumn object</param>
        bool Contains(IUIFormColumn column);

        /// <summary>
        /// Provides an indexing facility so that the contents of the definition
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to access</param>
        /// <returns>Returns the property definition at the index position
        /// specified</returns>
        IUIFormColumn this[int index] { get; }

        /// <summary>
        /// Gets and sets the tab name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets and sets the UIFormGrid definition
        /// </summary>
        IUIFormGrid UIFormGrid { set; get; }

        /// <summary>
        /// Returns the <see cref="UIForm"/> that this <see cref="IUIForm"/> is defined for.
        /// </summary>
        IUIForm UIForm { get; set; }

        /// <summary>
        /// Returns the definition list's enumerator
        /// </summary>
        /// <returns>Returns an IEnumerator-type object</returns>
        IEnumerator GetEnumerator();
    }

}