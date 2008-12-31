namespace Habanero.Base
{

    /// <summary>
    /// This class contains the definition of a property that participates in a relationship between two Classes.
    /// This class collaborates with the <see cref="IRelKeyDef"/>, the <see cref="IClassDef"/> 
    ///   to provide a definition of the properties involved in the <see cref="IRelationshipDef"/> between 
    ///   two <see cref="IBusinessObject"/>. This provides
    ///   an implementation of the Foreign Key Mapping pattern (Fowler (236) -
    ///   'Patterns of Enterprise Application Architecture' - 'Maps an association between objects to a 
    ///   foreign Key Reference between tables.')
    /// the RelPropdef should not be used by the Application developer since it is usually constructed 
    ///    based on the mapping in the ClassDef.xml file.
    /// 
    /// The RelPropDef is used by the RelKeyDef. The RelPropDef (Relationship Property Definition) defines
    ///   the property definition <see cref="IPropDef"/> from the owner Business object defintion and the Property name that this
    ///   Property Definition is mapped to. A <see cref="IRelProp"/> is created from this definition for a particular 
    ///   <see cref="IBusinessObject"/>.
    /// </summary>
    public interface IRelPropDef
    {
        /// <summary>
        /// Returns the property name for the relationship owner
        /// </summary>
        string OwnerPropertyName { get; }

        /// <summary>
        /// The property name to be matched to in the related class
        /// </summary>
        string RelatedClassPropName { get; }
    }
}