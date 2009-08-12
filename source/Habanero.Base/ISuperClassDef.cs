namespace Habanero.Base
{
    public interface ISuperClassDef
    {
        /// <summary>
        /// Returns the type of ORMapping used.  See the ORMapping
        /// enumeration for more detail.
        /// </summary>
        ORMapping ORMapping { get; set; }

        ///<summary>
        /// The assembly name of the SuperClass
        ///</summary>
        string AssemblyName { get; set; }

        ///<summary>
        /// The class name of the SuperClass
        ///</summary>
        string ClassName { get; set; }

        /// <summary>
        /// Returns the name of the discriminator column used to determine which class is being
        /// referred to in a row of the database table.
        /// This property applies only to SingleTableInheritance.
        /// </summary>
        string Discriminator { get; set; }

        /// <summary>
        /// Returns the class definition for this super-class
        /// </summary>
        IClassDef SuperClassClassDef { get; set; }

        /// <summary>
        /// Returns the name of the property that identifies which field
        /// in the child class (containing the super class definition)
        /// contains a copy of the parent's ID.  An empty string implies
        /// that the parent's ID is simply inherited and is used as the
        /// child's ID.  This property applies only to ClassTableInheritance.
        /// </summary>
        string ID { get; set; }
    }
}