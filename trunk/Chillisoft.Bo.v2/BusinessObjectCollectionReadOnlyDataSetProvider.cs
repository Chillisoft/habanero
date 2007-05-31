using System;
using Chillisoft.Generic.v2;

namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// Provides a read-only data-set for business objects
    /// </summary>
    public class BusinessObjectCollectionReadOnlyDataSetProvider : BusinessObjectCollectionDataSetProvider
    {
        /// <summary>
        /// Constructor to initialise a new provider with the business object
        /// collection provided
        /// </summary>
        /// <param name="collection">The business object collection</param>
        public BusinessObjectCollectionReadOnlyDataSetProvider(BusinessObjectBaseCollection collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Adds handlers to be called when business object updates occur
        /// </summary>
        public override void AddHandlersForUpdates()
        {
            foreach (BusinessObjectBase businessObject in itsCollection)
            {
                businessObject.Updated += new BusinessObjectUpdatedHandler(UpdatedHandler);
            }
            itsCollection.BusinessObjectAdded += new BusinessObjectEventHandler(AddedHandler);
            itsCollection.BusinessObjectRemoved += new BusinessObjectEventHandler(RemovedHandler);
        }

        /// <summary>
        /// Handles the event of a business object being removed. Removes the
        /// data row that contains the object.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void RemovedHandler(object sender, BusinessObjectEventArgs e)
        {
            int rowNum = this.FindRow(e.BusinessObject);
            if (rowNum != -1)
            {
                this.itsTable.Rows.RemoveAt(rowNum);
            }
            e.BusinessObject.Updated -= new BusinessObjectUpdatedHandler(UpdatedHandler);
        }

        /// <summary>
        /// Handles the event of a business object being added. Adds a new
        /// data row containing the object.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void AddedHandler(object sender, BusinessObjectEventArgs e)
        {
            object[] values = new object[itsUIGridProperties.Count + 1];
            values[0] = e.BusinessObject.ID.ToString();
            int i = 1;
            BOMapper mapper = new BOMapper(e.BusinessObject);
            foreach (UIGridProperty gridProperty in itsUIGridProperties)
            {
                object val = mapper.GetPropertyValueForUser(gridProperty.PropertyName);
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
            itsTable.LoadDataRow(values, true);
            e.BusinessObject.Updated += new BusinessObjectUpdatedHandler(UpdatedHandler);
        }

        /// <summary>
        /// Handles the event of a row being updated
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void UpdatedHandler(object sender, BusinessObjectEventArgs e)
        {
            int rowNum = this.FindRow(e.BusinessObject);
            if (rowNum == -1)
            {
                return;
            }

            object[] values = new object[itsUIGridProperties.Count + 1];
            values[0] = e.BusinessObject.ID.ToString();
            int i = 1;
            BOMapper mapper = new BOMapper(e.BusinessObject);
            foreach (UIGridProperty gridProperty in itsUIGridProperties)
            {
                object val = mapper.GetPropertyValueForUser(gridProperty.PropertyName);
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
            itsTable.Rows[rowNum].ItemArray = values;
        }

        /// <summary>
        /// Initialises the local data
        /// </summary>
        public override void InitialiseLocalData()
        {
        }
    }
}