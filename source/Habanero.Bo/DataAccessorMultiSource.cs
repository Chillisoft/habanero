// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO
{
    /// <summary>
    /// A Data Accessor that is used to configure loading of business objects from 
    /// multiple data sources.
    /// E.g. By using <see cref="AddDataAccessor"/> you can configure the DataSource 
    /// that should be used for loading and saving of each Type of Business Object.
    /// </summary>
    public class DataAccessorMultiSource : IDataAccessor
    {
        private readonly IDataAccessor _defaultDataAccessor;
        private readonly BusinessObjectLoaderMultiSource _multiSourceBusinessObjectLoader;

        private readonly Dictionary<Type, IDataAccessor> _dataAccessors;
        /// <summary>
        /// Construct the DataSource with a default accessor
        /// </summary>
        /// <param name="defaultDataAccessor"></param>
        public DataAccessorMultiSource(IDataAccessor defaultDataAccessor)
        {
            _dataAccessors= new Dictionary<Type, IDataAccessor>();
            _defaultDataAccessor = defaultDataAccessor;
            _multiSourceBusinessObjectLoader = new BusinessObjectLoaderMultiSource(_defaultDataAccessor.BusinessObjectLoader);
        }
        /// <summary>
        /// Returns the <see cref="IBusinessObjectLoader"/> to be used when using this
        /// <see cref="IDataAccessor"/>.
        /// </summary>
        public IBusinessObjectLoader BusinessObjectLoader
        {
            get { return _multiSourceBusinessObjectLoader; }
        }
        /// <summary>
        /// Returns the <see cref="ITransactionCommitter"/> to be used when using this
        /// <see cref="IDataAccessor"/>.
        /// </summary>
        public ITransactionCommitter CreateTransactionCommitter()
        {
            return new TransactionCommitterMultiSource(_defaultDataAccessor, _dataAccessors);
        }
        /// <summary>
        /// Add the <paramref name="dataAccessor"/> for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The business object type</param>
        /// <param name="dataAccessor">The data accessor to use for loading and persisting business objects.</param>
        public void AddDataAccessor(Type type, IDataAccessor dataAccessor)
        {
            _dataAccessors.Add(type, dataAccessor);
            _multiSourceBusinessObjectLoader.AddBusinessObjectLoader(type, dataAccessor.BusinessObjectLoader);
        }
    }
}