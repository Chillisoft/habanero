// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.BO.ClassDefinition
{


    /// <summary>
    /// This class contains the definition of a property that participates in a relationship between two Classes.
    /// This class collaborates with the <see cref="RelKeyDef"/>, the <see cref="ClassDef"/> 
    ///   to provide a definition of the properties involved in the <see cref="RelationshipDef"/> between 
    ///   two <see cref="IBusinessObject"/>. This provides
    ///   an implementation of the Foreign Key Mapping pattern (Fowler (236) -
    ///   'Patterns of Enterprise Application Architecture' - 'Maps an association between objects to a 
    ///   foreign Key Reference between tables.')
    /// the RelPropdef should not be used by the Application developer since it is usually constructed 
    ///    based on the mapping in the ClassDef.xml file.
    /// 
    /// The RelPropDef is used by the RelKeyDef. The RelPropDef (Relationship Property Definition) defines
    ///   the property definition <see cref="IPropDef"/> from the owner Business object defintion and the Property name that this
    ///   Property Definition is mapped to. A <see cref="RelProp"/> is created from this definition for a particular 
    ///   <see cref="IBusinessObject"/>.
    /// </summary>
    public class RelPropDef : IRelPropDef
    {
        private readonly string _ownerPropDefName;

        /// <summary>
        /// The property name to be matched to in the related class
        /// </summary>
        public string RelatedClassPropName { get; protected set; }

        /// <summary>
        /// Returns the PropDef of the OwnerClass that this RelPropDef defines.
        /// </summary>
        public IPropDef OwnerPropDef { get; protected set; }

        /// <summary>
        /// Constructor to create new RelPropDef object
        /// </summary>
        /// <param name="ownerClassPropDef">The property definition of the 
        /// owner object</param>
        /// <param name="relatedObjectPropName">The property name of the 
        /// related object</param>
        public RelPropDef(IPropDef ownerClassPropDef,
                          string relatedObjectPropName)
        {
            ArgumentValidationHelper.CheckArgumentNotNull(ownerClassPropDef, "ownerClassPropDef");
            OwnerPropDef = ownerClassPropDef;
            RelatedClassPropName = relatedObjectPropName;
		}

        /// <summary>
        /// Constructor to create new RelPropDef object
        /// </summary>
        /// <param name="ownerClassPropDefName">The name of the prop on the owner object</param>
        /// <param name="relatedObjectPropName">The property name of the 
        /// related object</param>
        public RelPropDef(string ownerClassPropDefName, string relatedObjectPropName)
        {
            ArgumentValidationHelper.CheckArgumentNotNull(ownerClassPropDefName, "ownerClassPropDefName");
            _ownerPropDefName = ownerClassPropDefName;
            RelatedClassPropName = relatedObjectPropName;
        }

		/// <summary>
        /// Returns the property name for the relationship owner
        /// </summary>
        public string OwnerPropertyName
        {
            get { return OwnerPropDef != null ? OwnerPropDef.PropertyName : _ownerPropDefName; }
        }


        /// <summary>
        /// Creates a new RelProp object based on this property definition
        /// </summary>
        /// <param name="boPropCol">The collection of properties</param>
        /// <returns>The newly created RelProp object</returns>
        protected internal IRelProp CreateRelProp(IBOPropCol boPropCol)
		{
		    IBOProp boProp = boPropCol[OwnerPropertyName];
		    return new RelProp(this, boProp);
		}
    }
}