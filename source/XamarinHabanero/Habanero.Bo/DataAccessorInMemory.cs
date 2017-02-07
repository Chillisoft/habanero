#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using Habanero.Base;

namespace Habanero.BO
{
    ///<summary>
    /// The data accessor used when using the in-memory data store.
    ///</summary>
    public class DataAccessorInMemory : IDataAccessor
    {
        /// <summary>
        /// The <see cref="DataStoreInMemory"/> that is used by this data accessor.
        /// </summary>
        protected readonly DataStoreInMemory _dataStore;
        private readonly IBusinessObjectLoader _businessObjectLoader;

        ///<summary>
        /// Creates a new in-memory data store.
        ///</summary>
        public DataAccessorInMemory() : this(new DataStoreInMemory())
        {
        }

        ///<summary>
        /// Alternate constructor for the in-memory data store where the DataStore to be used is passed in.
        ///</summary>
        ///<param name="dataStore">The Data Store to be used.</param>
        public DataAccessorInMemory(DataStoreInMemory dataStore)
        {
            _dataStore = dataStore;
            _businessObjectLoader = new BusinessObjectLoaderInMemory(_dataStore);
        }

        /// <summary>
        /// The <see cref="IDataAccessor.BusinessObjectLoader"/> to use to load BusinessObjects.
        /// </summary>
        public IBusinessObjectLoader BusinessObjectLoader
        {
            get { return _businessObjectLoader; }
        }

        /// <summary>
        /// Creates a TransactionCommitter for you to use to persist BusinessObjects. A new TransactionCommitter is required
        /// each time an object or set of objects is persisted.
        /// </summary>
        /// <returns></returns>
        public virtual ITransactionCommitter CreateTransactionCommitter()
        {
            return new TransactionCommitterInMemory(_dataStore);
        }

        ///<summary>
        /// Returns the DataAccessorInMemory's internal data store.
        ///</summary>
        public DataStoreInMemory DataStoreInMemory
        {
            get { return _dataStore; }
        }
    }
}