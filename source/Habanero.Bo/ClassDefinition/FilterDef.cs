using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Defines a filter for a grid.  Consists of a set of <see cref="FilterPropertyDef"/>s, and a few settings, such as
    /// FilterMode (can be Filter or Search) and a number of columns (where 0 means a flow layout is used).
    /// </summary>
    public class FilterDef
    {

        /// <summary>
        /// The standard constructor, which takes the list of <see cref="FilterPropertyDef"/>s defining the fields of the filter.
        /// </summary>
        /// <param name="filterPropertyDefs">The fields defining what to filter on.</param>
        public FilterDef(IList<FilterPropertyDef> filterPropertyDefs)
        {
            FilterPropertyDefs = filterPropertyDefs;
        }

        /// <summary>
        /// The list of <see cref="FilterPropertyDef"/>s which define each filter.
        /// </summary>
        public IList<FilterPropertyDef> FilterPropertyDefs { get; set; }

        /// <summary>
        /// The mode of filtering, where <see cref="FilterModes.Filter"/> means to filter already loaded data, and 
        /// <see cref="FilterModes.Search"/> means to search via a query to the data store in the process of loading.
        /// </summary>
        public FilterModes FilterMode { get; set; }

        /// <summary>
        /// The number of columns to layout the filters in.  By default this is 0, and the layout used is that of a flow layout
        /// which simply adds controls until the right side of the container is reached and then moves to the next line.
        /// If this is set to a number greater than zero it will lay out the filter controls in that number of columns.
        /// </summary>
        public int Columns { get; set; }


    }
}