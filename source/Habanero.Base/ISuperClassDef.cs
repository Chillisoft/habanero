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
    }
}