using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.BO;

namespace Habanero.UI.Base
{
    public interface IGridBase: IChilliControl
    {
        /// <summary>
        /// Sets the business object collection displayed in the grid.  This
        /// collection must be pre-loaded using the collection's Load() command.
        /// The default ui definition will be used, that is a 'ui' element 
        /// without a 'name' attribute.
        /// </summary>
        /// <param name="col">The collection of business objects to display.  This
        /// collection must be pre-loaded.</param>
        void SetCollection(IBusinessObjectCollection col);

        IDataGridViewRowCollection Rows { get; }

        IDataGridViewColumnCollection Columns { get; }

        bool AllowUserToAddRows { get; set; }

        object DataSource { get; set; }

        BusinessObject SelectedBusinessObject { get; set; }

        IList<BusinessObject> SelectedBusinessObjects { get; }

        IDataGridViewSelectedRowCollection SelectedRows { get; }

        bool AutoGenerateColumns { get; set; }

        IDataGridViewRow CurrentRow { get; }

        event EventHandler<BOEventArgs> BusinessObjectSelected;
        event EventHandler SelectionChanged;
        event EventHandler CollectionChanged;

        /// <summary>
        /// Clears the business object collection and the rows in the data table
        /// </summary>
        void Clear();

        /// <summary>
        /// Returns the business object collection being displayed in the grid
        /// </summary>
        /// <returns>Returns a business collection</returns>
        IBusinessObjectCollection GetCollection();

        /// <summary>
        /// Returns the business object at the row specified
        /// </summary>
        /// <param name="row">The row number in question</param>
        /// <returns>Returns the busines object at that row, or null
        /// if none is found</returns>
        BusinessObject GetBusinessObjectAtRow(int row);

        /// <summary>
        /// Sets the sort column and indicates whether
        /// it should be sorted in ascending or descending order
        /// </summary>
        /// <param name="columnName">The column number to set</param>
        /// <param name="isBoProperty">Whether the property is a business
        /// object property</param>
        /// <param name="ascending">Whether sorting should be done in ascending
        /// order ("false" sets it to descending order)</param>
        void SetSortColumn(string columnName, bool isBoProperty, bool ascending);
    }
}