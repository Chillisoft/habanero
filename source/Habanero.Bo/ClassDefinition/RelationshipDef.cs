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
using Habanero.Util;

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
    /// on a child business object being added/removed from the relationship as well
    ///  as differnetiating when the owning business object is viewed as dirty.
    /// This typically differentiats between a composition, aggregation and Association relationship.
    /// </summary>
    public enum RelationshipType
    {
        /// <summary>
        /// Association a related object can be removed, added, deleted or created via the relationship.
        ///   A related object being removed will be dereferenceed.
        ///   A related object being added will be referenced.
        ///   The owning business object is not considered to be dirty because its 
        ///     related business objects are dirty.
        ///•	A typical example of an associative relationship is a Manager and her Departments (assuming a Manager can manage many departments but a department may only have one manager). A Manager can exist independently of any Department and a Department can exist independently of a Manager. The Manager may however be associated with one Department and later associated with a different Department.
        ///•	Unlike a Car and its wheels the Department is not part of a Manager or visa versa.
        ///•	The rules for whether a manager that is associated with one or more departments can be deleted or not is dependent upon the rules configured for the Departments relationship (i.e. a Manager’s Departments relationship could be marked prevent delete, dereference or do nothing). 
        ///•	An already persisted Department can be added to a the Manager’s Departments relationship (In Habanero a new Department can be added to a Manager’s Departments relationship).
        ///•	A driver can be removed from its related car. 
        ///•	A Manager can create a new Department via its Departments Relationship (this is not a strict implementation of domain design but is allowed due to the convenience of this).
        ///•	A Manager is considered to be dirty only if it has added, created, MarkedForDelete or removed dirty Departments. 
        ///•	If a Manager is persisted then it will only persist its Department’s relationship and will not persist a related Department that is dirty (I.e. if a department has been added to the Relationship then it’s foreign key (ManagerID) will be updated. The department name could also have been edited. If the manager is saved then the foreign key (ManagerID) will be updated but the department name will not be updated).
        /// </summary>
        Association = 1,

        /// <summary>
        /// •	A typical example of an aggregation relationship is a Car and its Tyres. A Tyre is part of a Car. A Tyre can exist independently of its Car and a Tyre can only belong to a single Car at any point in time. The Tyre may however be transferred from one car to another. 
        ///•	The Car that has tyres cannot be deleted without it deleting or removing its tyres. The car’s Tyres relationship would be marked as either prevent delete, dereference tyres, delete tyres or do nothing. 
        ///•	An already persisted tyre can be added to a car (In Habanero a new tyre can be added to a car). 
        ///•	A tyre can be removed from its car. 
        ///•	A car can create a new tyre via its Tyres Relationship (This is not a strict implementation of Domain modelling rules but is allowed due to the convenience of this method).
        ///•	A car is considered to be dirty if it has any dirty tyres. A dirty tyre would be any tyre that has had any edits and would include a newly created tyre, an added tyre, a removed tyre or a tyre that has been marked for deletion.
        ///•	If a car is persisted then it must persist all its tyres.
        /// </summary>
        Aggregation = 2,

        /// <summary>
        /// •	A typical example of a composition relationship is an Invoice and its Invoice lines. An invoice is made up of its invoice lines. An Invoice Line is part of an Invoice. An invoice Line cannot exist independently of its invoice and an invoice line can only belong to a single invoice.
        ///•	An invoice that has invoice lines cannot be deleted without it deleting its invoice lines. The invoice’s InvoiceLines relationship would be marked as either prevent delete, delete invoice lines or do nothing.
        ///•	An already persisted invoice line cannot be added to an Invoice (In Habanero a new invoice line can be added to an invoice). 
        ///•	An Invoice line cannot be removed from its invoice.
        ///•	An invoice can create a new invoice line via its InvoiceLines Relationship.
        ///•	An invoice is considered to be dirty if it has any dirty invoice line. A dirty invoice line would be any invoice line that is dirty and would include a newly created invoice line and an invoice line that has been marked for deletion.
        ///•	If an invoice is persisted then it must persist all its invoice lines.
        ///</summary>
        Composition = 3,
    }

    /// <summary>
    /// Defines the relationship between the ownming Business Object (<see cref="IBusinessObject"/> and the 
    /// related Business Object.
    /// This class collaborates with the <see cref="RelKeyDef"/>, the <see cref="ClassDef"/> 
    ///   to provide a definition Relationship. This class along with the <see cref="RelKeyDef"/> and 
    ///   <see cref="RelPropDef"/> provides
    ///   an implementation of the Foreign Key Mapping pattern (Fowler (236) -
    ///   'Patterns of Enterprise Application Architecture' - 'Maps an association between objects to a 
    ///   foreign Key Reference between tables.')
    /// The RelationshipDef should not be used by the Application developer since it is usually constructed 
    ///    based on the mapping in the ClassDef.xml file.
    /// 
    /// The RelationshipDef (Relationship Definition) is used bay a <see cref="ClassDef"/> to define a particular
    ///   relationship. Each relationship has a relationship name e.g. A relationship betwee a person and a department may
    ///   manager, employee etc. The related Class e.g. A Department definition would contain a relationship definition to
    ///   a Person Class. (to allow for the person class to be in a different assemply the assembly name is also stored.
    ///   The list of properties the define the Foreign key mapping of relationship between these two classes is stored using
    ///   the <see cref="RelKeyDef"/>. The Relationship also stores additional information such as <see cref="DeleteParentAction"/>
    ///   and order critieria. The <see cref="DeleteParentAction"/> defines any constraints that the relationship should provide 
    ///   in the case of the parent (relationship owner e.g. Depertment in our example) being deleted. E.g. If the department (parent)
    ///   is being deleted you may want to delete all related object or prevent delete if there are any related objects.
    ///   In cases where there are many related objects e.g. A Department can have many Employees the relationship may be required to 
    ///   load in a specifically order e.g. by employee number. The order criteria is used for this.
    /// </summary>
    public abstract class RelationshipDef
    {
		private Type _relatedObjectClassType;
		private string _relatedObjectAssemblyName;
		private string _relatedObjectClassName;
		private RelKeyDef _relKeyDef;
		private string _relationshipName;
		private bool _keepReferenceToRelatedObject;
        private DeleteParentAction _deleteParentAction;
        private RelationshipType _relationshipType;

        protected OrderCriteria _orderCriteria;

		#region Constructors

        /// <summary>
        /// Constructor to create a new relationship definition
        /// </summary>
        /// <param name="relationshipName">A name for the relationship</param>
        /// <param name="relatedObjectClassType">The class type of the related object</param>
        /// <param name="relKeyDef">The related key definition</param>
        /// <param name="keepReferenceToRelatedObject">Whether to keep a
        /// reference to the related object.  Could be false for memory-
        /// intensive applications.</param>
		/// <param name="deleteParentAction">The required action when the parent is deleted e.g. Dereference related, delete related, prevent delete</param>
        protected RelationshipDef(string relationshipName,
							   Type relatedObjectClassType,
							   RelKeyDef relKeyDef,
                               bool keepReferenceToRelatedObject,
                               DeleteParentAction deleteParentAction)
            : this(relationshipName, relatedObjectClassType, null, null, relKeyDef, keepReferenceToRelatedObject, deleteParentAction, RelationshipType.Association)
		{

		}

        /// <summary>
        /// Constructor to create a new relationship definition
        /// </summary>
        /// <param name="relationshipName">A name for the relationship</param>
        /// <param name="relatedObjectAssemblyName">The assembly that the related object is in</param>
        /// <param name="relatedObjectClassName">The class type of the related object</param>
        /// <param name="relKeyDef">The related key definition</param>
        ///<param name="keepReferenceToRelatedObject">Whether to keep a
        /// reference to the related object.  Could be false for memory-
        /// intensive applications.</param>
        ///<param name="deleteParentAction">The required action when the parent is deleted e.g. Dereference related, delete related, prevent delete</param>
        protected RelationshipDef(string relationshipName,
								string relatedObjectAssemblyName,
								string relatedObjectClassName,
								RelKeyDef relKeyDef,
								bool keepReferenceToRelatedObject,
                                DeleteParentAction deleteParentAction,
                                RelationshipType relationshipType 
            )
            : this(relationshipName, null, relatedObjectAssemblyName, relatedObjectClassName, relKeyDef, keepReferenceToRelatedObject, deleteParentAction, relationshipType)
		{

 
		}

    	private RelationshipDef(string relationshipName,
								Type relatedObjectClassType,
								string relatedObjectAssemblyName,
								string relatedObjectClassName,
								RelKeyDef relKeyDef,
								bool keepReferenceToRelatedObject,
                                DeleteParentAction deleteParentAction,
                                RelationshipType relationshipType)
		{
            ArgumentValidationHelper.CheckArgumentNotNull(relKeyDef, "relKeyDef");
            ArgumentValidationHelper.CheckStringArgumentNotEmpty(relationshipName, "relationshipName");

			if (relatedObjectClassType != null) 
				MyRelatedObjectClassType = relatedObjectClassType;
			else
			{
				_relatedObjectAssemblyName = relatedObjectAssemblyName;
				_relatedObjectClassName = relatedObjectClassName;
				_relatedObjectClassType = null;
			}
			_relKeyDef = relKeyDef;
            _relationshipName = relationshipName;
            _keepReferenceToRelatedObject = keepReferenceToRelatedObject;
            _deleteParentAction = deleteParentAction;
    	    _relationshipType = relationshipType;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// A name for the relationship e.g. Employee, Manager.
		/// </summary>
		public string RelationshipName
		{
			get { return _relationshipName; }
			protected set { _relationshipName = value; }
		}

		/// <summary>
		/// The assembly name of the related object type. In cases where the related object is in a different assebly
		/// the object will be constructed via reflection.
		/// </summary>
		public string RelatedObjectAssemblyName
		{
			get { return _relatedObjectAssemblyName; }
			protected set { _relatedObjectAssemblyName = value; }
		}

		/// <summary>
		/// The class name of the related object type.
		/// </summary>
		public string RelatedObjectClassName
		{
			get { return _relatedObjectClassName; }
			protected set { _relatedObjectClassName = value; }
		}

        /// <summary>
        /// The class type of the related object
        /// </summary>
        public Type RelatedObjectClassType
        {
            get { return MyRelatedObjectClassType; }
			protected set { MyRelatedObjectClassType = value;}
        }

        /// <summary>
        /// The related key definition. <see cref="RelKeyDef"/>
        /// </summary>
        public RelKeyDef RelKeyDef
        {
			get { return _relKeyDef; }
			protected set { _relKeyDef = value; }
        }

        /// <summary>
        /// Whether to keep a reference to the related object or to reload every time the relationship is called.
        /// Could be false for memory-intensive applications.
        /// </summary>
        public bool KeepReferenceToRelatedObject
        {
			get { return _keepReferenceToRelatedObject; }
			protected set { _keepReferenceToRelatedObject = value; }
        }
        /// <summary>
        /// The <see cref="ClassDef"/> for the related object.
        /// </summary>
    	internal ClassDef RelatedObjectClassDef
    	{
    		get
    		{
				ClassDef classDef = null;
				if (ClassDef.ClassDefs.Contains(RelatedObjectAssemblyName, RelatedObjectClassName))
				{
					classDef = ClassDef.ClassDefs[RelatedObjectAssemblyName, RelatedObjectClassName];
				}
    			return classDef;
    		}
    	}

		#endregion Properties

		#region Type Initialisation

		private Type MyRelatedObjectClassType
    	{
			get
			{
				TypeLoader.LoadClassType(ref _relatedObjectClassType, _relatedObjectAssemblyName, _relatedObjectClassName,
					"related object", "relationship definition");
				return _relatedObjectClassType;
			}
			set
			{
				_relatedObjectClassType = value;
				if (_relatedObjectClassType != null)
					ArgumentValidationHelper.CheckArgumentIsSubType(_relatedObjectClassType, "relatedObjectClassType", typeof(BusinessObject));
				TypeLoader.ClassTypeInfo(_relatedObjectClassType, out _relatedObjectAssemblyName, out _relatedObjectClassName);
			}
    	}

        /// <summary>
        /// Provides specific instructions with regards to deleting a parent
        /// object.  See the DeleteParentAction enumeration for more detail.
        /// </summary>
        public DeleteParentAction DeleteParentAction
        {
            get { return _deleteParentAction; }
            protected set { _deleteParentAction = value; }
        }

        ///<summary>
        /// The order by clause that the related object will be sorted by.
        /// In the case of a single relationship this will return a null string
        ///</summary>
        public OrderCriteria OrderCriteria
        {
            get { return _orderCriteria; }
            protected set { _orderCriteria = value; }
        }

        ///<summary>
        /// Returns the specific action that the relationship must carry out in the case of a child being added to it.
        /// <see cref="RelationshipType"/>
        ///</summary>
        public RelationshipType RelationshipType
        {
            get { return _relationshipType; }
            internal set { _relationshipType = value; }
        }


        #endregion Type Initialisation

        /// <summary>
        /// Create and return a new Relationship based on the relationship definition.
        /// </summary>
        /// <param name="owningBo">The business object that owns
        /// this relationship e.g. The department</param>
        /// <param name="lBOPropCol">The collection of properties of the Business object</param>
        /// <returns>The new relationship object created</returns>
        public abstract Relationship CreateRelationship(IBusinessObject owningBo, BOPropCol lBOPropCol);

        internal void CheckCanAddChild(IBusinessObject bo)
        {
            if (!bo.Status.IsNew && (this.RelationshipType == RelationshipType.Composition))
            {
                string message = "The " + this.RelatedObjectClassName + " could not be added since the "
                                 + this.RelationshipName +
                                 " relationship is set up as a composition relationship (AddChildAction.Prevent)";
                throw new HabaneroDeveloperException(message, message);
            }
        }
        internal void CheckCanRemoveChild(IBusinessObject bo)
        {
            if (this.RelationshipType == RelationshipType.Composition)
            {
                string message = "The " + this.RelatedObjectClassName + " could not be removed since the "
                                 + this.RelationshipName
                                 + " relationship is set up as a composition relationship (RemoveChildAction.Prevent)";
                throw new HabaneroDeveloperException(message, message);
            }
        }
    }
}