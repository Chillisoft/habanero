using System.Collections;

namespace Habanero.Base
{
    /// <summary>
    /// Manages the definition of the primary key in a for a particular Business Object (e.g. Customer).
    /// The Primary Key Definition defins the properties of the object that are used to map the business object
    ///  to the database. The Primary key def is a mapping that is used to implement the 
    ///  Identity Field (216) Pattern (Fowler - 'Patterns of Enterprise Application Architecture')
    /// 
    /// In most cases the PrimaryKeyDefinition will only have one property definition and this property definition 
    ///  will be for an immutable property. In the ideal case this property definition will
    ///  represent a property that is globally unique. In these cases the primaryKeyDef will have the flag mIsGUIDObjectID set to true. 
    ///  However we have in many cases had to extend or replace existing systems
    ///  that use mutable composite keys to identify objects in the database. The primary key definition allows you to define
    ///  all of these scenarious.
    /// The Application developer should not usually deal with this class since it is usually created based on the class definition modelled
    ///   and stored in the ClassDef.Xml.
    /// </summary>
    public interface IPrimaryKeyDef : IKeyDef
    {
        /// <summary>
        /// Returns true if the primary key is a propery the object's ID, that is,
        /// the primary key is a single discrete property that is an immutable Guid and serves as the ID.
        /// </summary>
        bool IsGuidObjectID { get; set; }

        ///<summary>
        /// Returns true if the primary key is a composite Key (i.e. if it consists of more than one property)
        ///</summary>
        bool IsCompositeKey { get; }

       
    }
}