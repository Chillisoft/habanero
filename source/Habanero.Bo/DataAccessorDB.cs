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
using Habanero.DB;

namespace Habanero.BO
{
    ///<summary>
    /// A Data Accessor for Retrieving and committing data from a relational database.
    ///</summary>
    public class DataAccessorDB : IDataAccessor
    {
        private readonly IBusinessObjectLoader _businessObjectLoader;


        ///<summary>
        /// The constructor for relational database data accessor. Sets up the appropriate
        /// Business object loader for the databse using the current connection string.
        ///</summary>
        public DataAccessorDB()
        {
            _businessObjectLoader = new BusinessObjectLoaderDB(DatabaseConnection.CurrentConnection);
        }

        /// <summary>
        /// The <see cref="IDataAccessor.BusinessObjectLoader"/> to use to load BusinessObjects
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
        public ITransactionCommitter CreateTransactionCommitter()
        {
            return new TransactionCommitterDB();
        }

  
    }
}