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

using System;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Defines a relationship where the owner relates to only one other object.
    /// </summary>
    public class SingleRelationshipDef : RelationshipDef
    {
        #region Constructors

        /// <summary>
        /// Constructor to create a new single relationship definition
        /// </summary>
        /// <param name="relationshipName">A name for the relationship</param>
        /// <param name="relatedObjectClassType">The class type of the related object</param>
        /// <param name="relKeyDef">The related key definition</param>
        /// <param name="keepReferenceToRelatedObject">Whether to keep a
        /// reference to the related object.  Could be false for memory-
        /// intensive applications.</param>
        /// <param name="deleteParentAction"></param>
        public SingleRelationshipDef
            (string relationshipName, Type relatedObjectClassType, RelKeyDef relKeyDef,
             bool keepReferenceToRelatedObject, DeleteParentAction deleteParentAction)
            : base(relationshipName, relatedObjectClassType, relKeyDef, keepReferenceToRelatedObject, deleteParentAction
                )
        {
            OwningBOHasForeignKey = true;
        }

        /// <summary>
        /// Constructor to create a new single relationship definition
        /// </summary>
        /// <param name="relationshipName">A name for the relationship</param>
        /// <param name="relatedObjectAssemblyName">The assembly name of the related object</param>
        /// <param name="relatedObjectClassName">The class name of the related object</param>
        /// <param name="relKeyDef">The related key definition</param>
        /// <param name="keepReferenceToRelatedObject">Whether to keep a
        /// reference to the related object.  Could be false for memory-
        /// intensive applications.</param>
        /// <param name="deleteParentAction"></param>
        public SingleRelationshipDef
            (string relationshipName, string relatedObjectAssemblyName, string relatedObjectClassName,
             RelKeyDef relKeyDef, bool keepReferenceToRelatedObject, DeleteParentAction deleteParentAction)
            : this(
                relationshipName, relatedObjectAssemblyName, relatedObjectClassName, relKeyDef,
                keepReferenceToRelatedObject, deleteParentAction, RelationshipType.Association
                )
        {
        }

        ///<summary>
        /// Constructs a single Relationship
        ///</summary>
        ///<param name="relationshipName"></param>
        ///<param name="relatedObjectAssemblyName"></param>
        ///<param name="relatedObjectClassName"></param>
        ///<param name="relKeyDef"></param>
        ///<param name="keepReferenceToRelatedObject"></param>
        ///<param name="deleteParentAction"></param>
        ///<param name="relationshipType"></param>
        public SingleRelationshipDef (  string relationshipName, 
                                        string relatedObjectAssemblyName, 
                                        string relatedObjectClassName,
                                        IRelKeyDef relKeyDef, 
                                        bool keepReferenceToRelatedObject, 
                                        DeleteParentAction deleteParentAction, RelationshipType relationshipType)
            : base(
                relationshipName, relatedObjectAssemblyName, relatedObjectClassName, relKeyDef,
                keepReferenceToRelatedObject, deleteParentAction, relationshipType)
        {
            OwningBOHasForeignKey = true;
        }

        #endregion Constructors

        ///<summary>
        /// Returns true where the owning business object has the foreign key for this relationship false otherwise.
        /// This is used to differentiate between the two sides of the relationship.
        ///</summary>
        public override bool OwningBOHasForeignKey { get; set; }


        /// <summary>
        /// Overrides abstract method of RelationshipDef to create a new
        /// relationship
        /// </summary>
        /// <param name="owningBo">The business object that will manage
        /// this relationship</param>
        /// <param name="lBOPropCol">The collection of properties</param>
        /// <returns></returns>
        public override IRelationship CreateRelationship(IBusinessObject owningBo, IBOPropCol lBOPropCol)
        {
            Type relationshipBOType = typeof(SingleRelationship<>).MakeGenericType(this.RelatedObjectClassType);
            return (ISingleRelationship)Activator.CreateInstance(relationshipBOType, owningBo, this, lBOPropCol);
        }
    }
}