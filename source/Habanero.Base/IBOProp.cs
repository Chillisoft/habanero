using System;

namespace Habanero.Base
{
    public interface IBOProp
    {
        /// <summary>
        /// Indicates that the value held by the property has been
        /// changed
        /// </summary>
        event EventHandler<BOPropEventArgs> Updated;

        ///<summary>
        /// The property definition of the property that this BOProp represents.
        ///</summary>
        IPropDef PropDef { get; }

        /// <summary>
        /// Gets and sets the value for this property
        /// </summary>
        object Value { get; set; }

        /// <summary>
        /// Gets the value held before the value was last updated.
        /// If the object has just been created, this v
        /// </summary>
        object ValueBeforeLastEdit { get; }

        /// <summary>
        /// Returns the persisted property value in its object form
        /// </summary>
        object PersistedPropertyValue { get; }

        /// <summary>
        /// Indicates whether the property value is valid
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Returns a string which indicates why the property value may
        /// be invalid
        /// </summary>
        string InvalidReason { get; }

        /// <summary>
        /// Indicates whether the property's value has been changed since
        /// it was last backed up or committed to the database
        /// </summary>
        bool IsDirty { get; }

        /// <summary>
        /// Returns the property type
        /// </summary>
        Type PropertyType { get; }

        /// <summary>
        /// The field name as given to the user in the user interface
        /// (eg. "Computer Part" rather than "ComputerPartID").  This
        /// property is used to improve error messaging, so that the
        /// user recognises the property name as displayed to them,
        /// rather than as it is represented in the code.
        /// </summary>
        string DisplayName { get; set; }

        /// <summary>
        /// Returns the property value as a string
        /// </summary>
        string PropertyValueString
        {
            get;
        }

        /// <summary>
        /// Returns the database field name
        /// </summary>
        string DatabaseFieldName
        {
            get;
        }

        /// <summary>
        /// Returns the property name
        /// </summary>
        string PropertyName
        {
            get;
        }

        /// <summary>
        /// Returns an XML string to describe changes between the property
        /// value and the persisted value.  It consists of an element with the 
        /// property name, containing "PreviousValue" and "NewValue" elements
        /// </summary>
        string DirtyXml
        {
            get;
        }

        /// <summary>
        /// Indicates whether the object is new
        /// </summary>
        bool IsObjectNew
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the persisted property value as a string (the value 
        /// assigned at the last backup or database committal)
        /// </summary>
        string PersistedPropertyValueString
        {
            get;
        }

        ///<summary>
        /// Does the business object property have a specified display name or not.
        ///</summary>
        ///<returns>True if a display name has been set for this property, otherwise false.</returns>
        bool HasDisplayName();

        /// <summary>
        /// Restores the property's original value as defined in PersistedValue
        /// </summary>
        void RestorePropValue();

        /// <summary>
        /// Copies the current property value to PersistedValue.
        /// This is usually called when the object is persisted
        /// to the database.
        /// </summary>
        void BackupPropValue();

        /// <summary>
        /// Initialises the property with the specified value
        /// </summary>
        /// <param name="propValue">The value to assign</param>
        void InitialiseProp(object propValue);

        /// <summary>
        /// Initialises the property with the specified value, and indicates
        /// whether the object is new or not
        /// </summary>
        /// <param name="propValue">The value to assign</param>
        /// <param name="isObjectNew">Whether the object is new or not</param>
        void InitialiseProp(object propValue, bool isObjectNew);
    }
}