using System.Collections.Generic;
using Habanero.Base.Exceptions;

namespace Habanero.Base
{
    /// <summary>
    /// This class contains the definition of a Foreign Key that defines the properties <see cref="IRelPropDef"/> that
    ///   that forms a relationship between two Classes. 
    /// This class collaborates with the <see cref="IRelPropDef"/>, the <see cref="IClassDef"/> 
    ///   to provide a definition of the properties involved in the <see cref="IRelationshipDef"/> between 
    ///   two <see cref="IBusinessObject"/>. This provides
    ///   an implementation of the Foreign Key Mapping pattern (Fowler (236) -
    ///   'Patterns of Enterprise Application Architecture' - 'Maps an association between objects to a 
    ///   foreign Key Reference between tables.')
    /// the RelKeyDef should not be used by the Application developer since it is usually constructed 
    ///    based on the mapping in the ClassDef.xml file.
    /// 
    /// The RelKeyDef (Relationship Key Definition) is a list of relationship Property Defs <see cref="IRelPropDef"/> that 
    ///   define the properties that form the persistant relationship definition (<see cref="IRelationshipDef"/> between 
    ///   two Business object defitions (<see cref="IClassDef"/>.
    ///   <see cref="IBusinessObject"/>.
    /// </summary>
    public interface IRelKeyDef : IEnumerable<IRelPropDef>
    {
        /// <summary>
        /// Provides an indexing facility for the property definitions
        /// in this key definition so that they can be 
        /// accessed like an array (e.g. collection["surname"])
        /// </summary>
        /// <param name="propName">The name of the property</param>
        /// <returns>Returns the corresponding RelPropDef object</returns>
        IRelPropDef this[string propName] { get; }

        ///<summary>
        /// The number of property definitiosn defined in the relKeyDef
        ///</summary>
        int Count { get; }

        /// <summary>
        /// Adds the related property definition to this key, as long as
        /// a property by that name has not already been added.
        /// </summary>
        /// <param name="relPropDef">The RelPropDef object to be added.</param>
        /// <exception cref="HabaneroArgumentException">Thrown if the
        /// argument passed is null</exception>
        void Add(IRelPropDef relPropDef);

        /// <summary>
        /// Create a relationship key based on this key definition and
        /// its associated property definitions
        /// </summary>
        /// <param name="lBoPropCol">The collection of properties</param>
        /// <returns>Returns the new RelKey object</returns>
        IRelKey CreateRelKey(BOPropCol lBoPropCol);
    }
}