// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using System.Collections.Generic;

namespace Habanero.Base
{
    ///<summary>
    /// interface for a property definition.
    ///</summary>
    public interface IPropDef
    {
        ///<summary>
        /// The display name for the property.
        ///</summary>
        string DisplayName { get; } // set; }

        ///<summary>
        /// The description of the property.
        ///</summary>
        string Description { get; set; }

        /// <summary>
        /// The name of the property type assembly
        /// </summary>
        string PropertyTypeAssemblyName { get; set; }

        /// <summary>
        /// The name of the property type
        /// </summary>
        string PropertyTypeName { get; set; }

        /// <summary>
        /// The type of the property, e.g. string
        /// </summary>
        Type PropertyType { get; set; }

        ///// <summary>
        ///// Gets and sets the property rule relevant to this definition
        ///// </summary>
        //IPropRule PropRule { get; set; }
        /// <summary>
        /// Returns a List of PropRules <see cref="IPropRule"/> for the Property Definition.
        /// </summary>
        List<IPropRule> PropRules { get; }

        ///<summary>
        /// Adds an <see cref="IPropRule"/> to the <see cref="PropRules"/> for the 
        /// Property Definiton.
        ///</summary>
        ///<param name="rule">The new rules to be added for the Property Definition.</param>
        void AddPropRule(IPropRule rule);

        /// <summary>
        /// The database field name - this allows you to have a 
        /// database field name that is different to the
        /// property name, which is useful for migrating systems where
        /// the database has already been set up
        /// </summary>
        string DatabaseFieldName { get; set; }

        /// <summary>
        /// The default value that a property of a new object will be set to
        /// </summary>
        object DefaultValue { get; set; }

        /// <summary>
        /// The default value that a property of a new object will be set to
        /// </summary>
        string DefaultValueString { get; set; }

        ///<summary>
        /// Is this property compulsary or not
        ///</summary>
        bool Compulsory { get; set; }

        /// <summary>
        /// Provides access to read and write the ILookupList object
        /// in this definition
        /// </summary>
        ILookupList LookupList { get; set; }

        /// <summary>
        /// Returns the rule for how the property can be accessed. 
        /// See the PropReadWriteRule enumeration for more detail.
        /// </summary>
        PropReadWriteRule ReadWriteRule { get; set; }

        /// <summary>
        /// Indicates whether this object has a LookupList object set
        /// </summary>
        /// <returns>Returns true if so, or false if the local
        /// LookupList equates to NullLookupList</returns>
        bool HasLookupList();

        /// <summary>
        /// Indicates whether this property is auto-incrementing (from the database)
        /// In this case when the BusinessObject is inserted the field will be filled
        /// from the database field.
        /// </summary>
        bool AutoIncrementing { get; }

        /// <summary>
        /// Returns the maximum length for a string property
        /// </summary>
        int Length { get; }

        ///<summary>
        /// Returns whether this property should keep its value private where possible.
        /// This will usually be set to 'true' for password fields. This will then prevent
        /// the value being revealed in error messages and by default controls the user interface.
        ///</summary>
        bool KeepValuePrivate { get; }

        /// <summary>
        /// Returns an appropriate IComparer object depending on the
        /// property type.  Can be used, for example, to provide to the
        /// ArrayList.Sort() function in order to determine how to compare
        /// items.  Caters for the following types: String, Int, Guid,
        /// DateTime, Single, Double, TimeSpan 
        /// and anything else that supports IComparable.
        /// </summary>
        /// <returns>Returns an IComparer object, or null if the property
        /// type is not one of those mentioned above</returns>
        IPropertyComparer<T> GetPropertyComparer<T>() where T : IBusinessObject;

        ///<summary>
        /// Is this property persistable or not. This is used for special properties e.g. Dynamically inserted properties
        /// as for Asset Management System (See Intermap Asset Management) or for any reflective/calculated field that 
        /// you would like to store propdef information for e.g. rules, Units of measure etc.
        /// This will prevent the property from being persisted in the usual manner.
        ///</summary>
        bool Persistable { get; set; }

        /// <summary>
        /// The name of the property, e.g. surname
        /// </summary>
        string PropertyName { get; set; }

        ///<summary>
        /// Returns the class definition that this property definition is owned by.
        ///</summary>
        IClassDef ClassDef { get; }

        ///<summary>
        /// Returns the full display name for a property definition.
        /// If there is a unit of measure then it is appended to the display name in brackets e.g. DisplayName (UOM).
        /// If there is no display name then it will return the PascalCase Delimited property Name i.e. Display Name.
        ///</summary>
        string DisplayNameFull { get; }

        /// <summary>
        /// Creates a new Business Object property (BOProp)
        /// </summary>
        /// <param name="assignDefaultValue">Whether to initialise the property 
        /// with the default value.
        /// </param>
        /// <returns>The newly created BO property</returns>
        IBOProp CreateBOProp(bool assignDefaultValue);


        ///<summary>
        /// returns true if obj is equal to this object
        ///</summary>
        ///<param name="obj"></param>
        ///<returns></returns>
        bool Equals(object obj);

        /// <summary>
        /// This method provides a the functionality to convert any object to the appropriate
        ///   type for the particular BOProp Type. e.g it will convert a valid guid string to 
        ///   a valid Guid Object.
        /// </summary>
        /// <param name="valueToParse">The value to be converted</param>
        /// <param name="returnValue"></param>
        /// <returns>An object of the correct type.</returns>
        bool TryParsePropValue(object valueToParse, out object returnValue);

        /// <summary>
        /// Converts the value of a valid type for this property definition to a string relevant.
        /// A null value will be oonverted to a zero length string.
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <returns>The converted string.</returns>
        string ConvertValueToString(object value);

        ///<summary>
        /// Makes a shallow clone of this property definition (i.e. the clone includes a list of all the
        ///  property rules but the property rules have not been cloned
        ///</summary>
        ///<returns></returns>
        IPropDef Clone();

        /// <summary>
        /// Tests whether a specified property value is valid against the current
        /// property rule.  A boolean is returned and an error message,
        /// where appropriate, is stored in a referenced parameter.
        /// </summary>
        /// <param name="propValue">The property value to be tested in the user interface, clarifies error messaging</param>
        /// <param name="errorMessage">A string which may be amended to reflect
        /// an error message if the value is not valid</param>
        /// <returns>Returns true if valid, false if not</returns>
        bool IsValueValid(object propValue, ref string errorMessage);
    }
}