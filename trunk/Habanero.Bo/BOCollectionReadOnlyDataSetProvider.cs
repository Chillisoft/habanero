using System;
using Habanero.Base;

namespace Habanero.Bo
{
    /// <summary>
    /// Provides a read-only data-set for business objects
    /// </summary>
    public class BOCollectionReadOnlyDataSetProvider : BOCollectionDataSetProvider
    {
        /// <summary>
        /// Constructor to initialise a new provider with the business object
        /// collection provided
        /// </summary>
        /// <param name="collection">The business object collection</param>
        public BOCollectionReadOnlyDataSetProvider(BusinessObjectCollection collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Adds handlers to be called when business object updates occur
        /// </summary>
        public override void AddHandlersForUpdates()
        {
            foreach (BusinessObject businessObject in _collection)
            {
                businessObject.Updated += new BusinessObjectUpdatedHandler(UpdatedHandler);
            }
            _collection.BusinessObjectAdded += new BusinessObjectEventHandler(AddedHandler);
            _collection.BusinessObjectRemoved += new BusinessObjectEventHandler(RemovedHandler);
        }

        /// <summary>
        /// Handles the event of a business object being removed. Removes the
        /// data row that contains the object.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void RemovedHandler(object sender, BOEventArgs e)
        {
            int rowNum = this.FindRow(e.BusinessObject);
            if (rowNum != -1)
            {
                this._table.Rows.RemoveAt(rowNum);
            }
            e.BusinessObject.Updated -= new BusinessObjectUpdatedHandler(UpdatedHandler);
        }

        /// <summary>
        /// Handles the event of a business object being added. Adds a new
        /// data row containing the object.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void AddedHandler(object sender, BOEventArgs e)
        {
            object[] values = new object[_uiGridProperties.Count + 1];
            values[0] = e.BusinessObject.ID.ToString();
            int i = 1;
            BOMapper mapper = new BOMapper(e.BusinessObject);
            foreach (UIGridProperty gridProperty in _uiGridProperties)
            {
                object val = mapper.GetPropertyValueToDisplay(gridProperty.PropertyName);
                if (val != null && val is DateTime)
                {
                    val = ((DateTime) val).ToString("yyyy/MM/dd");
                }
                else if (val == null)
                {
                    val = "";
                }
                values[i++] = val;
            }
            _table.LoadDataRow(values, true);
            e.BusinessObject.Updated += new BusinessObjectUpdatedHandler(UpdatedHandler);
        }

        /// <summary>
        /// Handles the event of a row being updated
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void UpdatedHandler(object sender, BOEventArgs e)
        {
            int rowNum = this.FindRow(e.BusinessObject);
            if (rowNum == -1)
            {
                return;
            }

            object[] values = new object[_uiGridProperties.Count + 1];
            values[0] = e.BusinessObject.ID.ToString();
            int i = 1;
            BOMapper mapper = new BOMapper(e.BusinessObject);
            foreach (UIGridProperty gridProperty in _uiGridProperties)
            {
                object val = mapper.GetPropertyValueToDisplay(gridProperty.PropertyName);
                if (val != null && val is DateTime)
                {
                    val = ((DateTime) val).ToString("yyyy/MM/dd");
                }
                else if (val == null)
                {
                    val = "";
                }
                values[i++] = val;
            }
            _table.Rows[rowNum].ItemArray = values;
        }

        /// <summary>
        /// Initialises the local data
        /// </summary>
        public override void InitialiseLocalData()
        {
        }
    }
}