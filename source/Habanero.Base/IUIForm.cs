//  ---------------------------------------------------------------------------------
//   Copyright (C) 2007-2010 Chillisoft Solutions
//   
//   This file is part of the Habanero framework.
//   
//       Habanero is a free framework: you can redistribute it and/or modify
//       it under the terms of the GNU Lesser General Public License as published by
//       the Free Software Foundation, either version 3 of the License, or
//       (at your option) any later version.
//   
//       The Habanero framework is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU Lesser General Public License for more details.
//   
//       You should have received a copy of the GNU Lesser General Public License
//       along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//  ---------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// An interface describing a form consisting of one or more <see cref="IUIFormTab"/> objects.  
    /// Implemented by UIForm.
    /// </summary>
    public interface IUIForm : ICollection, IEnumerable<IUIFormTab>
    {
        /// <summary>
        /// Adds a tab to the form
        /// </summary>
        /// <param name="tab">A UIFormTab object</param>
        void Add(IUIFormTab tab);

        /// <summary>
        /// Removes a tab from the form
        /// </summary>
        /// <param name="tab">A UIFormTab object</param>
        void Remove(IUIFormTab tab);

        /// <summary>
        /// Checks if the form contains the specified tab
        /// </summary>
        /// <param name="tab">A UIFormTab object</param>
        bool Contains(IUIFormTab tab);

        /// <summary>
        /// Provides an indexing facility so that the contents of the definition
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to access</param>
        /// <returns>Returns the property definition at the index position
        /// specified</returns>
        IUIFormTab this[int index] { get; }

        /// <summary>
        /// Gets and sets the width
        /// </summary>
        int Width { set; get; }

        /// <summary>
        /// Gets and sets the height
        /// </summary>
        int Height { set; get; }

        /// <summary>
        /// Gets and sets the heading
        /// </summary>
        string Title { set; get; }

        ///<summary>
        /// The UI Def that this UIForm is related to.
        ///</summary>
        IUIDef UIDef { get; set; }

        ///<summary>
        /// The ClassDef that this IUIForm is related to.
        ///</summary>
        IClassDef ClassDef { get; set; }

    }
}