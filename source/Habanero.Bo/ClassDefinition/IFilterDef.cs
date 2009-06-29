using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Provides an inteface for a filter definition. See <see cref="FilterDef"/>.  Consists of a number of <see cref="IFilterPropertyDef"/> objects and
    /// a mode (see <see cref="FilterModes"/>).
    /// </summary>
    public interface IFilterDef
    {
        /// <summary>
        /// The list of <see cref="FilterPropertyDef"/>s which define each filter.
        /// </summary>
        IList<IFilterPropertyDef> FilterPropertyDefs { get; set; }

        /// <summary>
        /// The mode of filtering, where <see cref="FilterModes.Filter"/> means to filter already loaded data, and 
        /// <see cref="FilterModes.Search"/> means to search via a query to the data store in the process of loading.
        /// </summary>
        FilterModes FilterMode { get; set; }

        /// <summary>
        /// The number of columns to layout the filters in.  By default this is 0, and the layout used is that of a flow layout
        /// which simply adds controls until the right side of the container is reached and then moves to the next line.
        /// If this is set to a number greater than zero it will lay out the filter controls in that number of columns.
        /// </summary>
        int Columns { get; set; }
    }
}