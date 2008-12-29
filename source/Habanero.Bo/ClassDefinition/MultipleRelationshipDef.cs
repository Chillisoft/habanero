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
using Habanero.Base.Exceptions;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// An enumeration that gives some instructions or limitations in the
    /// case where a parent is to be deleted.
    /// </summary>
    public enum DeleteParentAction
    {
        /// <summary>Delete all related objects when the parent is deleted</summary>
        DeleteRelated = 1,
        /// <summary>Dereference all related objects when the parent is deleted</summary>
        DereferenceRelated = 2,
        /// <summary>Prevent deletion of parent if it has related objects</summary>
        Prevent = 3,
        /// <summary>Don't perform any delete related activities on the businessobjects in this relationship</summary>
        DoNothing = 4
    }
    /// <summary>
    /// An enumeration that gives some instructions or limitations in the
    /// on a child business object being removed from the relationship.
    /// This typically differentiats between a composition, aggregation and Association relationship.
    /// </summary>
    public enum RemoveChildAction
    {
        /// <summary>Dereference the child being removed. 
        /// This is allowed for an aggregation and association relationship.
        /// E.g. if a wheel is removed from a car.
        /// </summary>
        Dereference = 1,
        /// <summary>Prevents the child from being removed (Composition relationship). Raises an error if the 
        /// RelatedBusinessObjectCollection.Remove method is called. E.g. an invoice line
        /// cannot exist independently of the invoice and cannot be moved from one invoice
        /// to another (it therefore cannot be removed.) </summary>
        Prevent = 2,
    }

    /// <summary>
    /// An enumeration that gives some instructions or limitations in the
    /// on a child business object being added to the relationship.
    /// This typically differentiats between a composition, aggregation and Association relationship.
    /// </summary>
    public enum AddChildAction
    {
        /// <summary>Adds a persisted or non persisted child to the relationship. This is allowed for an
        /// aggregation and association relationship.
        /// E.g. if a wheel is removed from a car it can be added to another car.</summary>
        AddChild = 1,
        /// <summary>Prevents a persisted child from being added (Composition relationship). Raises an error if the 
        /// RelatedBusinessObjectCollection.Add method is called with a perssited business object.
        /// E.g. an invoice line
        /// cannot exist independently of the invoice and cannot be moved from one invoice to 
        /// another (it therefore cannot be added). A new (non persisted) business object can always be added to 
        /// a relationship </summary>
        Prevent = 2,
    }
    /// <summary>
    /// Defines a relationship where the owner may relate to more than one object.
    /// </summary>
    public class MultipleRelationshipDef : RelationshipDef
    {
        // protected int _minNoOfRelatedObjects;
       // protected int _maxNoOfRelatedObjects;

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
                                       RelKeyDef relKeyDef,
                                       bool keepReferenceToRelatedObject,
            //				IExpression searchCriteria, 
                                       string orderBy,
                                       DeleteParentAction deleteParentAction)
            : base(relationshipName, relatedObjectClassType, relKeyDef, keepReferenceToRelatedObject, deleteParentAction)
        {
            ArgumentValidationHelper.CheckArgumentNotNull(orderBy, "orderBy");
            _orderCriteria = OrderCriteria.FromString( orderBy);
            //_minNoOfRelatedObjects = minNoOfRelatedObjects;
            // _maxNoOfRelatedObjects = maxNoOfRelatedObjects;

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
    	                               string relatedObjectClassName, RelKeyDef relKeyDef,
    	                               bool keepReferenceToRelatedObject, string orderBy,
    	                              DeleteParentAction deleteParentAction)
    		: base(relationshipName, relatedObjectAssemblyName, relatedObjectClassName, relKeyDef, keepReferenceToRelatedObject, deleteParentAction, RemoveChildAction.Dereference, AddChildAction.AddChild)
		{
            ArgumentValidationHelper.CheckArgumentNotNull(orderBy, "orderBy");
    	    _orderCriteria = OrderCriteria.FromString(orderBy);
			//_minNoOfRelatedObjects = minNoOfRelatedObjects;
			//_maxNoOfRelatedObjects = maxNoOfRelatedObjects;

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
        /// <param name="removeChildAction">Provides specific instructions for removing a child object.</param>
        /// <param name="addChildAction">Provides specific instructions for adding a child object.</param>
        public MultipleRelationshipDef(string relationshipName, string relatedObjectAssemblyName,
                                       string relatedObjectClassName, RelKeyDef relKeyDef,
                                       bool keepReferenceToRelatedObject, string orderBy,
                                      DeleteParentAction deleteParentAction, RemoveChildAction removeChildAction, 
                                        AddChildAction addChildAction)
            : base(relationshipName, relatedObjectAssemblyName, relatedObjectClassName, relKeyDef, keepReferenceToRelatedObject, deleteParentAction, removeChildAction, addChildAction)
        {
            ArgumentValidationHelper.CheckArgumentNotNull(orderBy, "orderBy");
            _orderCriteria = OrderCriteria.FromString(orderBy);
            //_minNoOfRelatedObjects = minNoOfRelatedObjects;
            //_maxNoOfRelatedObjects = maxNoOfRelatedObjects;

        }
		#endregion Constructors

		#region Properties

        ///// <summary>
        ///// Returns the sql order-by clause
        ///// </summary>
        //public string OrderBy
        //{
        //    get { return _orderCriteria; }
        //    protected set { _orderCriteria = value; }
        //}

        ///// <summary>
        ///// Indicates the minimum number of related objects that the owner
        ///// object must have to be valid (e.g. A car must have at least 4 wheels.)
        ///// </summary>
        //public int MinNoOfRelatedObjects
        //{
        //    get { return _minNoOfRelatedObjects; }
        //    protected set { _minNoOfRelatedObjects = value; }
        //}

        ///// <summary>
        ///// Indicates the maximum number of related objects that the owner 
        ///// object can have and still be valid (e.g. A person cannot have 
        ///// more than two biological parents.)
        ///// </summary>
        //public int MaxNoOfRelatedObjects
        //{
        //    get { return _maxNoOfRelatedObjects; }
        //    protected set { _maxNoOfRelatedObjects = value; }
        //}

        #endregion Properties

        /// <summary>
		/// Overrides abstract method of parent to facilitate creation of 
		/// a new Multiple Relationship
		/// </summary>
		/// <param name="owningBo">The business object that will manage
		/// this relationship</param>
		/// <param name="lBOPropCol">The collection of properties</param>
		/// <returns>Returns the new relationship that has been created</returns>
		public override Relationship CreateRelationship(IBusinessObject owningBo, BOPropCol lBOPropCol)
		{
			return new MultipleRelationship(owningBo, this, lBOPropCol);
		}
    }

    
}