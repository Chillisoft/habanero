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
    public class DataAccessorInMemory : IDataAccessor
    {
        private readonly DataStoreInMemory _dataStore;
        private IBusinessObjectLoader _businessObjectLoader;

        public DataAccessorInMemory() : this(new DataStoreInMemory())
        {
            
        }
        internal DataAccessorInMemory(DataStoreInMemory dataStore)
        {
            _dataStore = dataStore;

            _businessObjectLoader = new BusinessObjectLoaderInMemory(_dataStore);
        }


        public IBusinessObjectLoader BusinessObjectLoader
        {
            get { return _businessObjectLoader; }
        }

        public ITransactionCommitter CreateTransactionCommitter()
        {
            return new TransactionCommitterInMemory(_dataStore);
        }


    }
}