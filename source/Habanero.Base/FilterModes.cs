using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.Base
{
    /// <summary>
    /// Provides filter modes that can be set up for a grid.
    /// The default options is FilterModes.Filter.
    /// </summary>
    public enum FilterModes
    {
        /// <summary>
        /// Hides rows in a loaded collection that do not meet the filter criteria.  This is a
        /// preferred option if the size of the unfiltered collection is not expected to cause a
        /// deterioration in the performance of the system.
        /// </summary>
        Filter,
        /// <summary>
        /// Reloads the collection shown in the grid, using the criteria as set by the filter.
        /// This is a useful option if the collection in the grid is potentially large.
        /// </summary>
        Search
    }
}
