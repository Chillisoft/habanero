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

namespace Habanero.Base
{
    ///<summary>
    /// Provides an interface for storing a single property of a <see cref="IBusinessObject"/>.
    /// The property of a business object may represent a property such as FirstName, Surname.
    /// Typically a <see cref="IBusinessObject"/> will have a collection of Properties.
    ///</summary>
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
        /// Gets and sets the value for this property.
        /// </summary>
        object Value { get; set; }

        /// <summary>
        /// Gets the value held before the value was last edited. If the property has been edited multiple times
        ///   since being loaded or persisted to the database then this will not be equal to <see cref="PersistedPropertyValue"/>.
        /// If the object has just been created, this value will be equal to the current <see cref="Value"/> of the property
        /// </summary>
        object ValueBeforeLastEdit { get; }

        /// <summary>
        /// Returns the persisted property value in its object form
        /// </summary>
        object PersistedPropertyValue { get; }

        /// <summary>
        /// Indicates whether the property value is valid. The property will be valid if the current value, See <see cref="Value"/>.
        /// conforms to all the Property Rules for the property See <see cref="IPropRule"/>.
        /// NNB For performance reasons it was decided to not run the validation code 
        /// every time Isvalid is called on the boProp instead the boProp has a <see cref="Validate"/>
        /// which runs the validation code and sets the valid message and status on the BOProp.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Returns a string which indicates why the property value is
        /// be invalid See <see cref="IsValid"/>. If the Property is Valid then returns a null string.
        /// </summary>
        string InvalidReason { get; }

        /// <summary>
        /// Indicates whether the property's value has been changed since
        /// it was last backed up or committed to the database
        /// </summary>
        bool IsDirty { get; }

        /// <summary>
        /// Returns the property type. The property can be of any type but is typically a string, decimal etc.
        /// </summary>
        Type PropertyType { get; }

        /// <summary>
        /// The field name as given to the user in the user interface
        /// (eg. "Computer Part" rather than "ComputerPartID").  This
        /// property is used to improve error messaging, so that the
        /// user recognises the property name as displayed to them,
        /// rather than as it is represented in the code.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Returns the property value as a string
        /// </summary>
        string PropertyValueString
        {
            get;
        }

        /// <summary>
        /// Returns the database field name. This is the field name that the property is mapped to in the datasource.
        /// </summary>
        string DatabaseFieldName
        {
            get;
        }

        /// <summary>
        /// Returns the property name. This is the name of the property used in the business object layer e.g. Surname.
        /// </summary>
        string PropertyName
        {
            get;
        }

        /// <summary>
        /// Returns an XML string to describe changes between the property
        /// value and the persisted value.  It consists of an element with the 
        /// property name, containing "PreviousValue" and "NewValue" elements.
        /// This can be used for a number of purposes but is typically used for
        /// writing transaction logs or for Syunchronising distributed systems.
        /// </summary>
        string DirtyXml
        {
            get;
        }

        /// <summary>
        /// Indicates whether the <see cref="IBusinessObject"/> that this property is 
        /// associated with is new (i.e. has never been persisted to the database.
        /// </summary>
        bool IsObjectNew
        {
            get;
        }

        /// <summary>
        /// Returns the persisted property value as a string (the value 
        /// assigned at the last backup or database committal)
        /// </summary>
        string PersistedPropertyValueString
        {
            get;
        }

        /// <summary>
        /// Returns the named property value that should be displayed
        ///   on a user interface e.g. a textbox or on a report.
        /// This is used primarily for Lookup lists where
        ///    the value stored for the object may be a guid but the value
        ///    to display may be a string.
        /// </summary>
        /// <returns>Returns the property value</returns>
        object PropertyValueToDisplay { get; }

        /// <summary>
        /// Restores the property's original value as defined in PersistedValue.
        /// This is typically called when the edits to a <see cref="IBusinessObject"/> are cancelled.
        /// </summary>
        void RestorePropValue();

        /// <summary>
        /// Copies the current property value to PersistedValue.
        /// This is usually called when the object is persisted
        /// to the database or loaded from the database.
        /// </summary>
        void BackupPropValue();

        /// <summary>
        /// Initialises the property with the specified value,
        /// </summary>
        /// <param name="propValue">The value to assign</param>
        void InitialiseProp(object propValue);

        /// <summary>
        /// Validates whether the property values set for the BOProp are valid
        /// as compared to the BOProp rules.
        /// NNB For performance reasons it was decided to not run the validation code 
        /// every time Isvalid is called on the boProp instead the boProp has a <see cref="Validate"/>
        /// which runs the validation code and sets the valid message and status on the BOProp.
        /// This is used by the <see cref="BOPropCol"/> when  IsValid is called on the <see cref="BOPropCol"/>.
        /// </summary>
        void Validate();
    }
}