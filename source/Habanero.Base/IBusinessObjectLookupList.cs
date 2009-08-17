namespace Habanero.Base
{
    public interface IBusinessObjectLookupList : ILookupListWithClassDef
    {
        /// <summary>
        /// The assembly containing the class from which values are loaded
        /// </summary>
        string AssemblyName { get; set; }

        /// <summary>
        /// The class from which values are loaded
        /// </summary>
        string ClassName { get; set; }

        /// <summary>
        /// Gets and sets the sql criteria used to limit which objects
        /// are loaded in the BO collection
        /// </summary>
        Criteria Criteria { get; }

        /// <summary>
        /// This raw sort string.  Preferrably use <see cref="OrderCriteria"/>
        /// </summary>
        string SortString { get; }

        /// <summary>
        /// This raw criteria string.  Preferrably use <see cref="Criteria"/>
        /// </summary>
        string CriteriaString { get; }



        /// <summary>
        /// Gets and sets the sort string used to sort the lookup
        /// list.  This string must contain the name of a property
        /// belonging to the business object used to construct the list.
        /// The possible formats are: "property", "property asc",
        /// "property desc" and "property des".
        /// </summary>
        OrderCriteria OrderCriteria
        {
            get; //set { _sort = FormatSortAttribute(value); }
        }
    }
}