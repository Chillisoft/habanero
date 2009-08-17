namespace Habanero.Base
{
    public interface IDatabaseLookupList : ILookupListWithClassDef
    {
        /// <summary>
        /// Gets and sets the assembly name for the class being sourced for data
        /// </summary>
        string AssemblyName { get; set; }

        /// <summary>
        /// Gets and sets the class name being sourced for data
        /// </summary>
        string ClassName { get; set; }

        /// <summary>
        /// Gets the sql statement which is used to specify which
        /// objects to load for the lookup-list
        /// </summary>
        string SqlString { get; set; }
    }
}