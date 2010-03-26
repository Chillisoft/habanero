// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
namespace Habanero.Base
{

    /// <summary>
    /// This class contains the definition of a property that participates in a relationship between two Classes.
    /// This class collaborates with the <see cref="IRelKeyDef"/>, the <see cref="IClassDef"/> 
    ///   to provide a definition of the properties involved in the <see cref="IRelationshipDef"/> between 
    ///   two <see cref="IBusinessObject"/>. This provides
    ///   an implementation of the Foreign Key Mapping pattern (Fowler (236) -
    ///   'Patterns of Enterprise Application Architecture' - 'Maps an association between objects to a 
    ///   foreign Key Reference between tables.')
    /// the RelPropdef should not be used by the Application developer since it is usually constructed 
    ///    based on the mapping in the ClassDef.xml file.
    /// 
    /// The RelPropDef is used by the RelKeyDef. The RelPropDef (Relationship Property Definition) defines
    ///   the property definition <see cref="IPropDef"/> from the owner Business object defintion and the Property name that this
    ///   Property Definition is mapped to. A <see cref="IRelProp"/> is created from this definition for a particular 
    ///   <see cref="IBusinessObject"/>.
    /// </summary>
    public interface IRelPropDef
    {
        /// <summary>
        /// Returns the property name for the relationship owner
        /// </summary>
        string OwnerPropertyName { get; }

        /// <summary>
        /// The property name to be matched to in the related class
        /// </summary>
        string RelatedClassPropName { get; }
    }
}