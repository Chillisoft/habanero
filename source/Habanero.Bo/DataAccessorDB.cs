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
    /// A Data Accessor for retrieving and committing data from a relational database
    ///</summary>
    public class DataAccessorDB : IDataAccessor
    {
        private readonly IBusinessObjectLoader _businessObjectLoader;
        private IDatabaseConnection _databaseConnection;

        ///<summary>
        /// The constructor for relational database data accessor. Sets up the appropriate
        /// Business Object loader for the database using the database connection as
        /// provided by <see cref="DatabaseConnection.CurrentConnection"/>.
        ///</summary>
        public DataAccessorDB() : this(DatabaseConnection.CurrentConnection)
        {
        }

        ///<summary>
        /// The constructor for relational database data accessor. Sets up the appropriate
        /// Business Object loader for the database using the database connection provided.
        ///</summary>
        /// <param name="databaseConnection">The database connection to use for persistence</param>
        public DataAccessorDB(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
            _businessObjectLoader = new BusinessObjectLoaderDB(_databaseConnection);
        }

        /// <summary>
        /// Gets the <see cref="IDataAccessor.BusinessObjectLoader"/> to use to load BusinessObjects
        /// </summary>
        public IBusinessObjectLoader BusinessObjectLoader
        {
            get { return _businessObjectLoader; }
        }

        /// <summary>
        /// Creates a TransactionCommitter for you to use to persist BusinessObjects. A new TransactionCommitter is required
        /// each time an object or set of objects is persisted.
        /// </summary>
        /// <returns>Returns a new <see cref="TransactionCommitterDB"/> object</returns>
        public ITransactionCommitter CreateTransactionCommitter()
        {
            return new TransactionCommitterDB(_databaseConnection);
        }
    }
}