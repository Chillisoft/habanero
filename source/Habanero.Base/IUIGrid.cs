using System;
using System.Collections;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// An interface describing a grid.  An IUIGrid contains a collection of <see cref="IUIGridColumn"/> objects, a <see cref="IFilterDef"/>
    /// and a sort column.  Implemented by <see cref="UIGrid"/>
    /// </summary>
    public interface IUIGrid : ICollection
    {
        /// <summary>
        /// Adds a grid property definition
        /// </summary>
        /// <param name="prop">The grid property definition</param>
        void Add(IUIGridColumn prop);

        /// <summary>
        /// Removes a grid property definition
        /// </summary>
        /// <param name="prop">The grid property definition</param>
        void Remove(IUIGridColumn prop);

        /// <summary>
        /// Checks if a grid property definition is in the Grid definition
        /// </summary>
        /// <param name="prop">The grid property definition</param>
        bool Contains(IUIGridColumn prop);

        /// <summary>
        /// Provides an indexing facility so that the contents of the definition
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to access</param>
        /// <returns>Returns the property definition at the index position
        /// specified</returns>
        IUIGridColumn this[int index] { get; }

        /// <summary>
        /// Provides an indexing facility so that the contents of the definition
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="propName">The index position to access</param>
        /// <returns>Returns the property definition at the index position
        /// specified</returns>
        IUIGridColumn this[string propName] { get; }

        /// <summary>
        /// The column on which rows are ordered initially.
        /// Indicate the direction by adding " asc" or " desc"
        /// after the column name (" asc" is assumed if left out).  If this
        /// property is not specified, rows will be listed in the order
        /// they were added to the database.
        /// </summary>
        string SortColumn { get; set; }

        ///<summary>
        /// The definition of the filter that will be used for this grid.
        ///</summary>
        IFilterDef FilterDef { get; set; }

    }
}