using System;
using System.Collections.Generic;

namespace Habanero.Base
{
    /// <summary>
    /// This is a public interface that can be used by any Definition that implements a single Value
    /// the two obvious Definitions that implement a Single Value are
    /// 1) IPropDef
    /// 2) SingleRelationshipDef
    /// </summary>
    public interface ISingleValueDef
    {
        ///<summary>
        /// The display name for the property.
        ///</summary>
        string DisplayName { get; }

        ///<summary>
        /// The description of the property.
        ///</summary>
        string Description { get; set; }

        /// <summary>
        /// The name of the property type
        /// </summary>
        string PropertyTypeName { get; set; }

        /// <summary>
        /// The type of the property, e.g. string
        /// </summary>
        Type PropertyType { get; set; }

        ///<summary>
        /// Is this property compulsary or not
        ///</summary>
        bool Compulsory { get; set; }

        /// <summary>
        /// The name of the property, e.g. surname
        /// </summary>
        string PropertyName { get; set; }

        ///<summary>
        /// Returns the class definition that this property definition is owned by.
        ///</summary>
        IClassDef ClassDef { get; set; }

        ///<summary>
        /// Returns the full display name for a property definition.
        /// If there is a unit of measure then it is appended to the display name in brackets e.g. DisplayName (UOM).
        /// If there is no display name then it will return the PascalCase Delimited property Name i.e. Display Name.
        ///</summary>
        string DisplayNameFull { get; }

        ///<summary>
        /// The name of the Class if this PropDef is associated with a ClassDef.
        ///</summary>
        string ClassName { get; }

        /// <summary>
        /// The name of the property type assembly
        /// </summary>
        string PropertyTypeAssemblyName { get; set; }

        /// <summary>
        /// Returns a List of PropRules <see cref="IPropRule"/> for the Property Definition <see cref="IPropDef>"/> or Single RelationshipDefinition <see cref="ISingleRelationshipDef"/>.
        /// </summary>
        List<IPropRule> PropRules { get; }

        /// <summary>
        /// Provides access to read and write the ILookupList object
        /// in this definition
        /// </summary>
        ILookupList LookupList { get; set; }
    }
}