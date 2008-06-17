using System;

namespace Habanero.Base
{
    public interface IBusinessObject
    {
        ///<summary>
        /// This method can be overridden by a class that inherits from Business object.
        /// The method allows the Business object developer to add customised rules that determine.
        /// The editable state of a business object.
        /// E.g. Once an invoice is paid it is no longer editable. Or when a course is old it is no
        /// longer editable. This allows a UI developer to standise Code for enabling and disabling controls.
        /// These rules are applied to new object as well so if you want a new object 
        /// to be editable then you must include this.State.IsNew in evaluating IsEditable.
        /// </summary>
        bool IsEditable(out string message);

        ///<summary>
        /// This method can be overridden by a class that inherits from Business object.
        /// The method allows the Business object developer to add customised rules that determine.
        /// The Deletable state of a business object. E.g. Invoices can never be delted once created. 
        /// Objects cannot be deteled once they have reached certain stages e.g. a customer order after it is accepted.
        /// These rules are applied to new object as well so if you want a new object 
        /// to be deletable then you must include this.State.IsNew in evaluating IsDeletable.
        ///</summary>
        bool IsDeletable(out string message);

        /// <summary>
        /// Returns the value under the property name specified
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns the value if found</returns>
        object GetPropertyValue(string propName);

        /// <summary>
        /// Sets a property value to a new value
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <param name="newPropValue">The new value to set to</param>
        void SetPropertyValue(string propName, object newPropValue);

        /// <summary>
        /// Commits to the database any changes made to the object
        /// </summary>
        void Save();

        /// <summary>
        /// Cancel all edits made to the object since it was loaded from the 
        /// database or last saved to the database
        /// </summary>
        void Restore();

        /// <summary>
        /// Marks the business object for deleting.  Calling Save() will
        /// then carry out the deletion from the database.
        /// </summary>
        void Delete();

        /// <summary>
        /// The primary key for this busines object 
        /// </summary>
        IPrimaryKey PrimaryKey { get; set; }

        /// <summary>
        /// Returns the primary key ID of this object.  If there is no primary key on this
        /// class, the primary key of the nearest suitable parent is found and populated
        /// with the values held for that key in this object.  This is a possible situation
        /// in some forms of inheritance.
        /// </summary>
        IPrimaryKey ID
        {
            get;
        }

        /// <summary>
        /// Returns or sets the class definition. Setting the classdef is not recommended
        /// </summary>
        IClassDef ClassDef
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the collection of relationships
        /// </summary>
        IRelationshipCol Relationships
        {
            get;
            set;
        }

        /// <summary>
        /// The BOState object for this BusinessObject, which records the state information of the object
        /// </summary>
        IBOState State
        {
            get;
        }

        event EventHandler<BOEventArgs> Updated;
        event EventHandler<BOEventArgs> Saved;
        event EventHandler<BOEventArgs> Deleted;
        event EventHandler<BOEventArgs> Restored;

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
    }
}