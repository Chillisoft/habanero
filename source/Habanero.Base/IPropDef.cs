using System;
using System.Collections.Generic;

namespace Habanero.Base
{
    public interface IPropDef
    {
        ///<summary>
        /// The display name for the property.
        ///</summary>
        string DisplayName { get; set; }

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

        /// <summary>
        /// Gets and sets the property rule relevant to this definition
        /// </summary>
        IPropRule PropRule { get; set; }

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
        /// Cdfdasfkl;
        ///</summary>
        bool Persistable { get; set; }

        /// <summary>
        /// The name of the property, e.g. surname
        /// </summary>
        string PropertyName
        {
            get;
            set;
        }

        IClassDef ClassDef { get;  }
    }
}