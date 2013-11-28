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
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.BO.ClassDefinition
{

    /// <summary>
    /// Defines a relationship where the owner may relate to more than one object.
    /// </summary>
    public class MultipleRelationshipDef : RelationshipDef
    {
		#region Constructors

        /// <summary>
        /// Constructor to create a new Multiple Relationship Definition
        /// </summary>
        /// <param name="relationshipName">A name for the relationship</param>
        /// <param name="relatedObjectClassType">The class type of the related objects</param>
        /// <param name="relKeyDef">The related key definition</param>
        /// <param name="keepReferenceToRelatedObject">Whether to keep a
        /// reference to the related object.  Could be false for memory-
        /// intensive applications.</param>
        /// <param name="orderBy">The sql order-by clause</param>
        /// <param name="deleteParentAction">Provides specific instructions 
        /// with regards to deleting a parent object.  See the DeleteParentAction 
        /// enumeration for more detail.</param>
        public MultipleRelationshipDef(string relationshipName,
                                       Type relatedObjectClassType,
                                       IRelKeyDef relKeyDef,
                                       bool keepReferenceToRelatedObject,
                                       string orderBy,
                                       DeleteParentAction deleteParentAction)
            : base(relationshipName, relatedObjectClassType, relKeyDef, keepReferenceToRelatedObject, deleteParentAction)
        {
            ArgumentValidationHelper.CheckArgumentNotNull(orderBy, "orderBy");
            OrderCriteriaString = orderBy;

            this.OrderCriteria = Base.OrderCriteria.FromString( orderBy);
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
    	/// <param name="orderBy">The sql order-by clause</param>
    	/// <param name="deleteParentAction">Provides specific instructions 
    	/// with regards to deleting a parent object.  See the DeleteParentAction 
    	/// enumeration for more detail.</param>
    	public MultipleRelationshipDef(string relationshipName, string relatedObjectAssemblyName,
    	                               string relatedObjectClassName, IRelKeyDef relKeyDef,
    	                               bool keepReferenceToRelatedObject, string orderBy,
    	                               DeleteParentAction deleteParentAction)
            : base(relationshipName, relatedObjectAssemblyName, relatedObjectClassName, 
                    relKeyDef, keepReferenceToRelatedObject, deleteParentAction, 
                    InsertParentAction.InsertRelationship, RelationshipType.Association)
		{
            ArgumentValidationHelper.CheckArgumentNotNull(orderBy, "orderBy");
            OrderCriteriaString = orderBy; 
            OrderCriteria = Base.OrderCriteria.FromString(orderBy);
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
        /// <param name="orderBy">The sql order-by clause</param>
        /// <param name="deleteParentAction">Provides specific instructions 
        /// with regards to deleting a parent object.  See the DeleteParentAction 
        /// enumeration for more detail.</param>
        /// <param name="insertParentAction"></param>
        /// <param name="relationshipType">Provides specific instructions for adding/removing a child object.</param>
        /// <param name="timeout">The timout in milliseconds. The collection will not be automatically refreshed from the DB if the timeout has nto expired</param>
        public MultipleRelationshipDef(string relationshipName, string relatedObjectAssemblyName, string relatedObjectClassName, IRelKeyDef relKeyDef, bool keepReferenceToRelatedObject, string orderBy, DeleteParentAction deleteParentAction, InsertParentAction insertParentAction, RelationshipType relationshipType, int timeout)
             : base(relationshipName, relatedObjectAssemblyName, relatedObjectClassName, relKeyDef, 
                        keepReferenceToRelatedObject, deleteParentAction, insertParentAction, relationshipType)
        {
            ArgumentValidationHelper.CheckArgumentNotNull(orderBy, "orderBy");
            TimeOut = timeout;
            OrderCriteriaString = orderBy; 
            OrderCriteria = new OrderCriteria();
            OrderCriteria = Base.OrderCriteria.FromString(orderBy);
        }
		#endregion Constructors

        ///<summary>
        /// Returns true where the owning business object has the foreign key for this relationship false otherwise.
        /// This is used to differentiate between the two sides of the relationship.
        /// For A Multiple Relatiosnhip this will always return false since the FK must be on the related Class.
        ///</summary>
        public override bool OwningBOHasForeignKey { get {
            return false;
        } set {  } }

        /// <summary>
		/// Overrides abstract method of parent to facilitate creation of 
		/// a new Multiple Relationship
		/// </summary>
		/// <param name="owningBo">The business object that will manage
		/// this relationship</param>
		/// <param name="lBOPropCol">The collection of properties</param>
		/// <returns>Returns the new relationship that has been created</returns>
		public override IRelationship CreateRelationship(IBusinessObject owningBo, IBOPropCol lBOPropCol)
		{
            var relationshipBOType = typeof(MultipleRelationship<>).MakeGenericType(this.RelatedObjectClassType);
            return (IMultipleRelationship)Activator.CreateInstance(relationshipBOType, owningBo, this, lBOPropCol, this.TimeOut);
		}

        ///<summary>
        /// Returns true if this is a Multiple Relationship and the Reverse is a single relationship
        ///</summary>
        public override bool IsOneToMany
        {
            get { return true; }
        }

        ///<summary>
        /// Returns true if this is a Single Relationship and the Reverse is a Multiple relationship
        ///</summary>
        public override bool IsManyToOne
        {
            get { return false; }
        }

        ///<summary>
        /// Returns true if this is a Single Relationship and the Reverse is a Single relationship
        ///</summary>
        public override bool IsOneToOne
        {
            get { return false; }
        }

        /// <summary>
        /// Returns true if this RelationshipDef is compulsory.
        /// This relationship def will be considered to be compulsory if this
        /// <see cref="IRelationshipDef.OwningBOHasForeignKey"/> and all the <see cref="IPropDef"/>'s that make up the 
        /// <see cref="IRelKeyDef"/> are compulsory. This is only relevant for ManyToOne and OneToOne Relationships.
        /// I.e. to single Relationships
        /// </summary>
        public override bool IsCompulsory
        {
            get { return false; }
        }
    }

    
}