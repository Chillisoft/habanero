//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
    ///<summary>
    /// The interface used to implement relationships between two classes.
    ///</summary>
    public interface IRelationship
    {
        /// <summary>
        /// Returns the set of business objects that relate to this one
        /// through the specific relationship
        /// </summary>
        /// <returns>Returns a collection of business objects. If this is a single relationship then
        /// returns a single object in the collection.</returns>
        IBusinessObjectCollection GetRelatedBusinessObjectCol();

        ///<summary>
        /// The key that identifies this relationship i.e. the properties in the 
        /// source object and how they are related to properties in the related object.
        ///</summary>
        IRelKey RelKey
        {
            get;
        }

        ///<summary>
        /// The criteria by which this relationship is ordered. I.e. by default all the
        /// related objects are loaded in this order.
        ///</summary>
        OrderCriteria OrderCriteria
        {
            get;
        }

        /// <summary>
        /// The class Definition for the related object.
        /// </summary>
        IClassDef RelatedObjectClassDef { get; }
    }
}