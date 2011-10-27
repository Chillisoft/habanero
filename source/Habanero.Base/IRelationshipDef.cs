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
namespace Habanero.Base
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
    /// case where a parent is saved and the related objects are new.
    /// For Composition and Aggregation this is always true (I.e. InsertRelationship).
    /// This control is only required for an association relationship (<see cref="RelationshipType"/>.
    /// In this case there are two options 
    /// <li>1) If the owing BO is saved and there are new 
    /// objects related to it via this relationship then these new related objects must be inserted.</li>
    /// <li>2) If the owning BO is saved and there are new objects related to it via this relationship then
    ///   these new objects must not be inserted. This is required where the Related Business Object has other Foreign keys
    ///   to the same Business Object
    ///   and the inserted business object may result in Referential integrity violations in a relational database.
    ///   E.g. ProposalRequest has a collection of Quotes but has a single active quote and the single Active quote
    ///   is tracked by a a ForeignKey on the ProposalRequest Object</li>
    /// </summary>
    public enum InsertParentAction
    {
        /// <summary>Inserts the RelatedObject related objects if required when the parent is saved</summary>
        InsertRelationship = 1,
        /// <summary>Don't perform any insert related activities on the businessobjects in this relationship</summary>
        DoNothing = 4
    }
    /// <summary>
    /// An enumeration that provides instructions or limitations on the child business object being added/removed 
    /// from the relationship as well as differentiating when the owning business object is viewed as dirty.
    /// This typically differs for a Composition, Aggregation, or Association relationship.
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
        ///•	A Manager is considered to be dirty only if it has added, created, MarkedForDeletion or removed dirty Departments. 
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
    /// This class collaborates with the <see cref="RelKeyDef"/>, the <see cref="IClassDef"/> 
    ///   to provide a definition Relationship. This class along with the <see cref="RelKeyDef"/> and 
    ///   <see cref="IRelPropDef"/> provides
    ///   an implementation of the Foreign Key Mapping pattern (Fowler (236) -
    ///   'Patterns of Enterprise Application Architecture' - 'Maps an association between objects to a 
    ///   foreign Key Reference between tables.')
    /// The RelationshipDef should not be used by the Application developer since it is usually constructed 
    ///    based on the mapping in the ClassDef.xml file.
    /// 
    /// The RelationshipDef (Relationship Definition) is used bay a <see cref="IClassDef"/> to define a particular
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
    public interface IRelationshipDef
    {
        /// <summary>
        /// A name for the relationship e.g. Employee, Manager.
        /// </summary>
        string RelationshipName { get; }

        /// <summary>
        /// The assembly name of the related object type. In cases where the related object is in a different assebly
        /// the object will be constructed via reflection.
        /// </summary>
        string RelatedObjectAssemblyName { get; }

        /// <summary>
        /// The class name of the related object type.
        /// </summary>
        string RelatedObjectClassName { get; }

        /// <summary>
        /// The related key definition. <see cref="RelKeyDef"/>
        /// </summary>
        IRelKeyDef RelKeyDef { get; }

        /// <summary>
        /// Whether to keep a reference to the related object or to reload every time the relationship is called.
        /// Could be false for memory-intensive applications.
        /// </summary>
        bool KeepReferenceToRelatedObject { get; }

        /// <summary>
        /// Provides specific instructions with regards to deleting a parent
        /// object.  See the <see cref="DeleteParentAction"/> enumeration for more detail.
        /// </summary>
        DeleteParentAction DeleteParentAction { get; }

        /// <summary>
        /// Provides specific instructions with regards to inserting a new related
        /// Business object.  See the <see cref="InsertParentAction"/> enumeration for more detail.
        /// </summary>
        InsertParentAction InsertParentAction { get; }

        ///<summary>
        /// The order by clause that the related object will be sorted by.
        /// In the case of a single relationship this will return a null string
        ///</summary>
        IOrderCriteria OrderCriteria { get; }

        ///<summary>
        /// The order by clause that the related object will be sorted by.  This is the raw order criteria as loaded or set. Use <see cref="OrderCriteria"/>
        /// to get the parsed criteria object.   In the case of a single relationship this will return a null string
        ///</summary>
        string OrderCriteriaString { get; }

        ///<summary>
        /// Returns the specific action that the relationship must carry out in the case of a child being added to it.
        /// <see cref="RelationshipType"/>
        ///</summary>
        RelationshipType RelationshipType { get; set; }

        ///<summary>
        /// The name of the reverse relationship.
        ///</summary>
        string ReverseRelationshipName { get; set; }

        ///<summary>
        /// Returns true where the owning business object has the foreign key for this relationship false otherwise.
        /// This is used to differentiate between the two sides of the relationship.
        ///</summary>
        bool OwningBOHasForeignKey { get; set; }

        /// <summary>
        /// The <see cref="IClassDef"/> for the related object.
        /// </summary>
        IClassDef RelatedObjectClassDef { get; }

        /// <summary>
        /// The type parameter of the related object type.  This allows you to relate a class with another one that is
        /// type parametrised (ie has multiple classdefs for one .net type)
        /// </summary>
        string RelatedObjectTypeParameter { get; set; }

        /// <summary>
        /// The timout in milliseconds. 
        /// The collection of Business Objects will not be automatically refreshed 
        /// from the DB if the timeout has not expired
        /// </summary>
        int TimeOut { get; set;  }

        /// <summary>
        /// The related class name including its type parameter (if any)
        /// </summary>
        string RelatedObjectClassNameWithTypeParameter { get; }

        /// <summary>
        /// Create and return a new Relationship based on the relationship definition.
        /// </summary>
        /// <param name="owningBo">The business object that owns
        /// this relationship e.g. The department</param>
        /// <param name="lBOPropCol">The collection of properties of the Business object</param>
        /// <returns>The new relationship object created</returns>
        IRelationship CreateRelationship(IBusinessObject owningBo, IBOPropCol lBOPropCol);

        ///<summary>
        /// Checks to see if the child can be added to the relationship
        ///</summary>
        ///<param name="bo"></param>
        void CheckCanAddChild(IBusinessObject bo);

        ///<summary>
        /// Checks to see if the child be removed to the relationship
        ///</summary>
        ///<param name="bo"></param>
        void CheckCanRemoveChild(IBusinessObject bo);

        ///<summary>
        /// Returns true if this is a Multiple Relationship and the Reverse is a single relationship
        ///</summary>
        bool IsOneToMany { get; }

        ///<summary>
        /// Returns true if this is a Single Relationship and the Reverse is a Multiple relationship
        ///</summary>
        bool IsManyToOne { get; }

        ///<summary>
        /// Returns true if this is a Single Relationship and the Reverse is a Single relationship
        ///</summary>
        bool IsOneToOne { get; }

        /// <summary>
        /// Returns true if this RelationshipDef is compulsory.
        /// This relationship def will be considered to be compulsory if this
        /// <see cref="OwningBOHasForeignKey"/> and all the <see cref="IPropDef"/>'s that make up the 
        /// <see cref="IRelKeyDef"/> are compulsory. This is only relevant for ManyToOne and OneToOne Relationships.
        /// I.e. to single Relationships
        /// </summary>
        bool IsCompulsory { get; }

        ///<summary>
        /// Gets and Sets the Class Def to the ClassDefinition (<see cref="IClassDef"/>) that owns this Relationship Def.
        ///</summary>
        IClassDef OwningClassDef { get; set; }
        /// <summary>
        /// Returns the ClassName of the <see cref="IClassDef"/> that owns this <see cref="IRelationshipDef"/>
        /// </summary>
        string OwningClassName { get; }
    }

}