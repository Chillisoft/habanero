using Habanero.Bo;
using Habanero.Ui.Grid;

namespace Habanero.Ui.Grid
{
    /// <summary>
    /// Manages a simple grid
    /// </summary>
    public class SimpleGrid : GridBase, IEditableGrid
    {
        /// <summary>
        /// Accepts and saves all changes to the data table
        /// </summary>
        public void AcceptChanges()
        {
            this._dataTable.AcceptChanges();
        }

        /// <summary>
        /// Rolls back all changes to the data table
        /// </summary>
        public void RejectChanges()
        {
            this._dataTable.RejectChanges();
        }

        /// <summary>
        /// Creates a new data set provider for the given business object
        /// collection
        /// </summary>
        /// <param name="col">The business object collection</param>
        /// <returns>Returns a new data set provider</returns>
        protected override BOCollectionDataSetProvider CreateBusinessObjectCollectionDataSetProvider(
            BusinessObjectCollection col)
        {
            return new BOCollectionEditableDataSetProvider(col);
        }

        /// <summary>
        /// Saves changes made on the grid (calls AcceptChanges())
        /// </summary>
        public void SaveChanges()
        {
            this.AcceptChanges();
        }

        /// <summary>
        /// Returns the selected business object
        /// </summary>
        public BusinessObject SelectedBusinessObject
        {
            get { return this.GetSelectedBusinessObject(); }
        }
    }
}