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
using System;
using System.Collections.Generic;

namespace Habanero.Base
{
    /// <summary>
    /// Holds a collection of properties on which two classes in a relationship
    /// are matching
    /// </summary>
    public interface IRelKey : IEnumerable<IRelProp>
    {
        ///<summary>
        /// Gets the number of properties in this relationship key.
        ///</summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// Provides an indexing facility so that the properties can be
        /// accessed with square brackets like an array
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns the RelProp object found with that name</returns>
        IRelProp this[string propName] { get; }

        /// <summary>
        /// Indexes the array of relprops this relkey contains.
        /// </summary>
        /// <param name="index">The position of the relprop to get</param>
        /// <returns>Returns the RelProp object found with that name</returns>
        IRelProp this[int index]
        {
            get;
        }

        /// <summary>
        /// Indicates if there is a related object.
        /// If all relationship properties are null then it is assumed that 
        /// there is no related object.
        /// </summary>
        /// <returns>Returns true if there is a valid relationship</returns>
        bool HasRelatedObject();

        ///// <summary>
        ///// Returns the relationship expression. This is a copy of the expression as stored in the <see cref="Habanero.BO.RelKey"/>
        ///// </summary>
        ///// <returns>Returns an IExpression object</returns>
        //IExpression RelationshipExpression();

        /// <summary>
        /// Indicates whether a property with the given name is part of the key
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns true if a property with this name is held</returns>
        bool Contains(string propName);

        /// <summary>
        /// Returns a copy of the key's Criteria (ie the search string matching this key). 
        /// </summary>
        /// <returns>Returns a Criteria object</returns>
        Criteria Criteria { get; }
        /// <summary>
        /// Event raised when the value for one of the Properties (<see cref="IRelProp"/>) for this <see cref="IRelKey"/> is changed
        /// </summary>
        event EventHandler RelatedPropValueChanged;
    }
}