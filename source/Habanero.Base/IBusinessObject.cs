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
using System;
using System.Xml.Serialization;

namespace Habanero.Base
{
    /// <summary>
    /// Provides an interface for business objects. This interface contains all the publically accessable
    /// methods for Business Objects.
    /// </summary>
    public interface IBusinessObject : IXmlSerializable
    {

        /// <summary>
        /// Returns the primary key ID of this object.  If there is no primary key on this
        /// class, the primary key of the nearest suitable parent is found and populated
        /// with the values held for that key in this object.  This is a possible situation
        /// in some forms of inheritance.
        /// </summary>
        IPrimaryKey ID { get; }

        /// <summary>
        /// Returns or sets the class definition. Setting the classdef is not recommended
        /// </summary>
        IClassDef ClassDef { get; set; }

        /// <summary>
        /// Gets and sets the collection of relationships
        /// </summary>
        IRelationshipCol Relationships { get; set; }

        /// <summary>
        /// The BOState object for this BusinessObject, which records the state information of the object
        /// </summary>
        IBOStatus Status { get; }

        /// <summary>
        /// The BOProps in this business object
        /// </summary>
        IBOPropCol Props { get; }

        ///<summary>
        /// This method can be overridden by a class that inherits from Business object.
        /// The method allows the Business object developer to add customised rules that determine.
        /// The editable state of a business object.
        /// E.g. Once an invoice is paid it is no longer editable. Or when a course is old it is no
        /// longer editable. This allows a UI developer to standise Code for enabling and disabling controls.
        /// These rules are applied to new object as well so if you want a new object 
        /// to be editable then you must include this.Status.IsNew in evaluating IsEditable.
        /// </summary>
        bool IsEditable(out string message);

        ///<summary>
        /// This method can be overridden by a class that inherits from Business object.
        /// The method allows the Business object developer to add customised rules that determine.
        /// The Deletable state of a business object. E.g. Invoices can never be delted once created. 
        /// Objects cannot be deteled once they have reached certain stages e.g. a customer order after it is accepted.
        /// These rules are applied to new object as well so if you want a new object 
        /// to be deletable then you must include this.Status.IsNew in evaluating IsDeletable.
        ///</summary>
        bool IsDeletable(out string message);

        /// <summary>
        /// Returns the value under the property name specified
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns the value if found</returns>
        object GetPropertyValue(string propName);

        /// <summary>
        /// Returns the value under the property name specified, accessing it through the 'source'
        /// </summary>
        /// <param name="source">The source of the property ie - the relationship or C# property this property is on</param>
        /// <param name="propName">The property name</param>
        /// <returns>Returns the value if found</returns>
        object GetPropertyValue(Source source, string propName);

        /// <summary>
        /// Sets a property value to a new value
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <param name="newPropValue">The new value to set to</param>
        void SetPropertyValue(string propName, object newPropValue);

        /// <summary>
        /// Cancel all edits made to the object since it was loaded from the 
        /// database or last saved to the database
        /// </summary>
        [Obsolete("This is replaced by CancelEdits().")]
        void Restore();

        /// <summary>
        /// Cancel all edits made to the object since it was loaded from the 
        /// database or last saved to the database
        /// </summary>
        void CancelEdits();

        /// <summary>
        /// Marks the business object for deleting.  Calling Save() will
        /// then carry out the deletion from the database.
        /// </summary>
        void MarkForDelete();

        /// <summary>
        /// Marks the business object for deleting.  Calling Save() will
        /// then carry out the deletion from the database.
        /// </summary>
        [Obsolete("This method has been replaced with MarkForDelete() since it is far more explicit that this does not instantly delete the business object.")]
        void Delete();

        /// <summary>
        /// Fired every time an object is persisted.
        ///   Whether the object is updated, inserted or deleted.
        /// Also fired when the object is restored.
        /// </summary>
        event EventHandler<BOEventArgs> Updated;
        /// <summary>
        /// Fired when an object that is not deleted is updated to the database
        /// </summary>
        event EventHandler<BOEventArgs> Saved;
        /// <summary>
        /// Event fired when an object marked for deletion is persisted to the databse.
        /// </summary>
        event EventHandler<BOEventArgs> Deleted;
        /// <summary>
        /// Fired when the object is restored.
        /// </summary>
        event EventHandler<BOEventArgs> Restored;
        /// <summary>
        /// The event is fired when the business object is updated
        /// </summary>
        event EventHandler<BOEventArgs> IDUpdated;
        /// <summary>
        /// Indicates whether all of the property values are valid
        /// </summary>
        /// <param name="invalidReason">A string to modify with a reason
        /// for any invalid values</param>
        /// <returns>Returns true if all are valid</returns>
        bool IsValid(out string invalidReason);

        /// <summary>
        /// Indicates whether all of the property values are valid
        /// </summary>
        /// <returns>Returns true if all are valid</returns>
        bool IsValid();

        ///<summary>
        /// This method can be overridden by a class that inherits from Business object.
        /// The method allows the Business object developer to add customised rules that determine.
        /// The Creatable rules of a business object.
        /// E.g. Certain users may not be allowed to create certain Business Objects.
        /// </summary>
        bool IsCreatable(out string message);

        /// <summary>
        /// Event is raised when any <see cref="IBOProp"/> belonging to this <see cref="IBusinessObject"/>
        ///  is updated (i.e. modified.
        /// </summary>
        event EventHandler<BOPropUpdatedEventArgs> PropertyUpdated;

        /// <summary>
        /// This event is raised when this <see cref="IBusinessObject"/> is Marked for deletion.
        /// </summary>
        event EventHandler<BOEventArgs> MarkedForDeletion;

        /// <summary>
        /// Returns the value stored in the DataStore for the property name specified, accessing it through the 'source'
        /// </summary>
        /// <param name="source">The source of the property ie - the relationship or C# property this property is on</param>
        /// <param name="propName">The property name</param>
        /// <returns>Returns the value if found</returns>
        object GetPersistedPropertyValue(Source source, string propName);

        /// <summary>
        /// Returns the named property value that should be displayed
        ///   on a user interface e.g. a textbox.
        /// This is used particularly for dates and guids where there is a
        /// particular format for the GUID/Date as a string.
        /// This is used for Database lookups lookup lists etc
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns the property value as a string</returns>
        string GetPropertyValueString(string propName);
    }
}