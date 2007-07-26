using Habanero.BO;
using Habanero.UI.Base;
using Habanero.UI.Grid;

namespace Habanero.UI.Grid
{
    /// <summary>
    /// Manages an editable grid, that displays a business object
    /// collection that has been pre-loaded.<br/>
    /// NOTE: Changes are not persisted until AcceptChanges() is called.
    /// Either use EditableGridWithButtons, which has a Save and Cancel button
    /// attached, or add some feature that causes changes to be saved
    /// once the user has finished editing.
    /// </summary>
    public class EditableGrid : GridBase, IEditableGrid
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public EditableGrid()
        {
            Permission.Check(this);
        }
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
            BusinessObjectCollection<BusinessObject> col)
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