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
//using System.Linq;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.Util;

namespace Habanero.BO.ClassDefinition
{
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
    public abstract class RelationshipDef : IRelationshipDef
    {
        private Type _relatedObjectClassType;
        private string _relatedObjectAssemblyName;
        private string _relatedObjectClassName;

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
							   IRelKeyDef relKeyDef,
                               bool keepReferenceToRelatedObject,
                               DeleteParentAction deleteParentAction)
            : this(relationshipName, relatedObjectClassType, null, null, relKeyDef, keepReferenceToRelatedObject, deleteParentAction, InsertParentAction.InsertRelationship, RelationshipType.Association)
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
        /// <param name="insertParentAction"><see cref="InsertParentAction"/></param>
        /// <param name="relationshipType"></param>
        protected RelationshipDef(string relationshipName, string relatedObjectAssemblyName, string relatedObjectClassName, IRelKeyDef relKeyDef, bool keepReferenceToRelatedObject, DeleteParentAction deleteParentAction, InsertParentAction insertParentAction, RelationshipType relationshipType)
            : this(relationshipName, null, relatedObjectAssemblyName, relatedObjectClassName, relKeyDef, keepReferenceToRelatedObject, deleteParentAction, insertParentAction, relationshipType)
		{
		}
        // ReSharper disable DoNotCallOverridableMethodsInConstructor
        private RelationshipDef(string relationshipName, Type relatedObjectClassType, string relatedObjectAssemblyName, string relatedObjectClassName, IRelKeyDef relKeyDef, bool keepReferenceToRelatedObject, DeleteParentAction deleteParentAction, InsertParentAction insertParentAction, RelationshipType relationshipType)
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
			RelKeyDef = relKeyDef;
            RelationshipName = relationshipName;
            KeepReferenceToRelatedObject = keepReferenceToRelatedObject;
            this.DeleteParentAction = deleteParentAction;
    	    this.InsertParentAction = insertParentAction;
    	    RelationshipType = relationshipType;
		}
        // ReSharper restore DoNotCallOverridableMethodsInConstructor

        /// <summary>
        /// Returns true if this RelationshipDef is compulsory.
        /// This relationship def will be considered to be compulsory if this
        /// <see cref="IRelationshipDef.OwningBOHasForeignKey"/> and all the <see cref="IPropDef"/>'s that make up the 
        /// <see cref="IRelKeyDef"/> are compulsory. This is only relevant for ManyToOne and OneToOne Relationships.
        /// I.e. to single Relationships
        /// </summary>
        public abstract bool IsCompulsory{ get;}

        ///<summary>
        /// Gets and Sets the Class Def to the ClassDefinition that owns this Relationship Def.
        ///</summary>
        public IClassDef OwningClassDef { get; set; }

        /// <summary>
        /// The name of the owning class of this relationship def.
        /// </summary>
        public string OwningClassName
        {
            get { return this.OwningClassDef == null? "": this.OwningClassDef.ClassName; }
        }

        #endregion Constructors

		#region Properties

        /// <summary>
        /// A name for the relationship e.g. Employee, Manager.
        /// </summary>
        public string RelationshipName { get; protected set; }

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
        public IRelKeyDef RelKeyDef { get; protected set; }

        /// <summary>
        /// Whether to keep a reference to the related object or to reload every time the relationship is called.
        /// Could be false for memory-intensive applications.
        /// </summary>
        public bool KeepReferenceToRelatedObject { get; protected set; }

        /// <summary>
        /// The <see cref="ClassDef"/> for the related object.
        /// </summary>
        public IClassDef RelatedObjectClassDef
    	{
    		get
    		{
    		    
    		    return ClassDef.ClassDefs[RelatedObjectAssemblyName, RelatedObjectClassNameWithTypeParameter];
    		}
    	}

        /// <summary>
        /// Returns true if the related class def has been loaded already.
        /// </summary>
        /// <returns></returns>
        protected bool RelatedClassDefLoaded()
        {
            return ClassDef.ClassDefs.Contains(RelatedObjectAssemblyName, RelatedObjectClassNameWithTypeParameter);
        }

        public string RelatedObjectClassNameWithTypeParameter
        {
            get
            {
                if (!String.IsNullOrEmpty(RelatedObjectTypeParameter))
                    return RelatedObjectClassName + "_" + RelatedObjectTypeParameter;
                return RelatedObjectClassName;
            }
        }

        /// <summary>
        /// The type parameter of the related object type.  This allows you to relate a class with another one that is
        /// type parametrised (ie has multiple classdefs for one .net type)
        /// </summary>
        public string RelatedObjectTypeParameter { get; set; }

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
        /// object.  See the <see cref="DeleteParentAction"/> enumeration for more detail.
        /// </summary>
        public DeleteParentAction DeleteParentAction { get; set; }

        public string PreventDeleteMessage { get; set; }

        ///<summary>
        /// Provides the specific instruction when a new object is added to an association relationship.
        /// See <see cref="InsertParentAction"/> enumeration for more details
        ///</summary>
        public virtual InsertParentAction InsertParentAction { get; set; }

        ///<summary>
        /// The order by clause that the related object will be sorted by.
        /// In the case of a single relationship this will return a null string
        ///</summary>
        public IOrderCriteria OrderCriteria { get; protected set; }

        ///<summary>
        /// The order by clause that the related object will be sorted by.
        /// In the case of a single relationship this will return a null string
        ///</summary>
        public string OrderCriteriaString { get; protected set; }

        ///<summary>
        /// Returns the specific action that the relationship must carry out in the case of a child being added to it.
        /// <see cref="RelationshipType"/>
        ///</summary>
        public RelationshipType RelationshipType { get; set; }

        ///<summary>
        /// Returns true where the owning business object has the foreign key for this relationship false otherwise.
        /// This is used to differentiate between the two sides of the relationship.
        ///</summary>
        public abstract  bool OwningBOHasForeignKey { get; set;}

        ///<summary>
        /// Returns the relationship name of the reverse relationship.
        ///</summary>
        public string ReverseRelationshipName { get; set; }

        ///<summary>
        /// Does this relationship link between the Primary Key of its owningBO and a foreign key of its related BO.
        /// In most cases this will be the reverse of <see cref="OwningBOHasForeignKey"/>
        ///</summary>
        public bool? OwningBOHasPrimaryKey { get; set; }

        /// <summary>
        /// The timout in milliseconds. 
        /// The collection of Business Objects will not be automatically refreshed 
        /// from the DB if the timeout has not expired
        /// </summary>
        public int TimeOut { get; set;  }

        #endregion Type Initialisation

        /// <summary>
        /// Create and return a new Relationship based on the relationship definition.
        /// </summary>
        /// <param name="owningBo">The business object that owns
        /// this relationship e.g. The department</param>
        /// <param name="lBOPropCol">The collection of properties of the Business object</param>
        /// <returns>The new relationship object created</returns>
        public abstract IRelationship CreateRelationship(IBusinessObject owningBo, IBOPropCol lBOPropCol);

        ///<summary>
        /// Can the child business object be added to this relationship. Typically a persisted child business object 
        /// cannot be added to a compositional relationship. A new (i.e. unpersisted) child object can however be.
        ///</summary>
        ///<param name="bo"></param>
        ///<exception cref="HabaneroDeveloperException"></exception>
        public void CheckCanAddChild(IBusinessObject bo)
        {
            string message;
            if (bo == null)
            {
                message = "The " + this.RelatedObjectClassName + " could not be added since the "
                          + " business object is null";
                throw new HabaneroDeveloperException(message, message);  
            }
            if (bo.Status == null) return;
            if (bo.Status.IsNew || (this.RelationshipType != RelationshipType.Composition)) return;

            message = "The " + this.RelatedObjectClassName + " could not be added since the "
                      + this.RelationshipName
                      +
                      " relationship is set up as a composition relationship (AddChildAction.Prevent) and an "
                      + "already persisted child business object cannot be added";
            throw new HabaneroDeveloperException(message, message);
        }

        ///<summary>
        /// Checks to see whether a business object can be removed from the relationship. Typically a
        ///   business object cannot be removed from the relationship if the relationship is composition.
        ///</summary>
        ///<param name="bo">The business object being removed</param>
        ///<exception cref="HabaneroDeveloperException"></exception>
        public void CheckCanRemoveChild(IBusinessObject bo)
        {
            if (bo.Status.IsNew || this.RelationshipType != RelationshipType.Composition) return;

            string message = "The " + this.RelatedObjectClassName + " could not be removed since the "
                             + this.RelationshipName
                             + " relationship is set up as a composition relationship (RemoveChildAction.Prevent)" ;
            throw new HabaneroDeveloperException(message, message);
        }

        ///<summary>
        /// Returns true if this is a Multiple Relationship and the Reverse is a single relationship
        ///</summary>
        public abstract bool IsOneToMany { get; }

        ///<summary>
        /// Returns true if this is a Single Relationship and the Reverse is a Multiple relationship
        ///</summary>
        public abstract bool IsManyToOne { get; }

        ///<summary>
        /// Returns true if this is a Single Relationship and the Reverse is a Single relationship
        ///</summary>
        public abstract bool IsOneToOne { get; }


    }
}
