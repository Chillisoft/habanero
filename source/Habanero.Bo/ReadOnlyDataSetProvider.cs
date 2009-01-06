//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using Habanero.Base;

namespace Habanero.BO
{
    /// <summary>
    /// Provides a read-only data-set for business objects
    /// </summary>
    public class ReadOnlyDataSetProvider : DataSetProvider
    {
        private bool _addPropertyUpdatedHandler = true;

        /// <summary>
        /// Constructor to initialise a new provider with the business object
        /// collection provided
        /// </summary>
        /// <param name="collection">The business object collection</param>
		public ReadOnlyDataSetProvider(IBusinessObjectCollection collection)
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
                //businessObject.Updated += UpdatedHandler;
                if (_addPropertyUpdatedHandler)
                {
                    businessObject.PropertyUpdated += UpdatedHandler;
                }
                else
                {
                    businessObject.Updated += UpdatedHandler;
                }
            }
            _collection.BusinessObjectAdded += AddedHandler;
            _collection.BusinessObjectRemoved += RemovedHandler;
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
            //e.BusinessObject.Updated -= UpdatedHandler;

            if (_addPropertyUpdatedHandler)
            {
                e.BusinessObject.PropertyUpdated -= UpdatedHandler;
            }
            else
            {
                e.BusinessObject.Updated -= UpdatedHandler;  
            }
            
        }

        /// <summary>
        /// Handles the event of a business object being added. Adds a new
        /// data row containing the object.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void AddedHandler(object sender, BOEventArgs e)
        {
            BusinessObject businessObject = (BusinessObject) e.BusinessObject;
            object[] values = GetValues(businessObject);
            _table.LoadDataRow(values, true);
            //businessObject.Updated += UpdatedHandler;

            if (_addPropertyUpdatedHandler)
            {
                businessObject.PropertyUpdated += UpdatedHandler;
            }
            else
            {
                businessObject.Updated += UpdatedHandler;
            }
            
        }

        /// <summary>
        /// Handles the event of a row being updated
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void UpdatedHandler(object sender, BOEventArgs e)
        {
            BusinessObject businessObject = (BusinessObject) e.BusinessObject;
            int rowNum = this.FindRow(businessObject);
            if (rowNum == -1)
            {
                return;
            }
            object[] values = GetValues(businessObject);
            //values[0] = _table.Rows[rowNum].ItemArray[0];
            _table.Rows[rowNum].ItemArray = values;
        }

        /// <summary>
        /// Initialises the local data
        /// </summary>
        public override void InitialiseLocalData()
        {
        }

        ///<summary>
        /// Gets and sets whether the property update handler shold be set or not.
        ///</summary>
        public bool AddPropertyUpdatedHandler
        {
            get { return _addPropertyUpdatedHandler; }
            set{ _addPropertyUpdatedHandler = value;}
        }

    }
}