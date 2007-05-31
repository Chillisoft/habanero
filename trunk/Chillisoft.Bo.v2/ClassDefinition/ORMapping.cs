namespace Chillisoft.Bo.ClassDefinition.v2
{
    /// <summary>
    /// An enumeration specifying the means used to preserve a class
    /// inheritance structure when writing to a database, since relational
    /// databases don't support inheritance.
    /// </summary>
    public enum ORMapping
    {
        /// <summary>
        /// Uses one database table per class in the inheritance structure
        /// </summary>
        ClassTableInheritance,
        /// <summary>
        /// Maps all fields of all classes of an inheritance structure into a single table
        /// </summary>
        SingleTableInheritance,
        /// <summary>
        /// Uses a table for each concrete class in the inheritance hierarchy
        /// </summary>
        ConcreteTableInheritance
    }
}