using Chillisoft.Bo.v2;
using Chillisoft.UI.Generic.v2;

namespace Chillisoft.UI.Application.v2
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
            this.itsDataTable.AcceptChanges();
        }

        /// <summary>
        /// Rolls back all changes to the data table
        /// </summary>
        public void RejectChanges()
        {
            this.itsDataTable.RejectChanges();
        }

        /// <summary>
        /// Creates a new data set provider for the given business object
        /// collection
        /// </summary>
        /// <param name="col">The business object collection</param>
        /// <returns>Returns a new data set provider</returns>
        protected override BusinessObjectCollectionDataSetProvider CreateBusinessObjectCollectionDataSetProvider(
            BusinessObjectBaseCollection col)
        {
            return new BusinessObjectCollectionEditableDataSetProvider(col);
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
        public BusinessObjectBase SelectedBusinessObject
        {
            get { return this.GetSelectedBusinessObject(); }
        }
    }
}