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
    public class DataAccessorMultiSource : IDataAccessor
    {
        private IDataAccessor _defaultDataAccessor;
        private BusinessObjectLoaderMultiSource _multiSourceBusinessObjectLoader;

        private Dictionary<Type, IDataAccessor> _dataAccessors;

        public DataAccessorMultiSource(IDataAccessor defaultDataAccessor)
        {
            _dataAccessors= new Dictionary<Type, IDataAccessor>();
            _defaultDataAccessor = defaultDataAccessor;
            _multiSourceBusinessObjectLoader = new BusinessObjectLoaderMultiSource(_defaultDataAccessor.BusinessObjectLoader);
        }

        public IBusinessObjectLoader BusinessObjectLoader
        {
            get { return _multiSourceBusinessObjectLoader; }
        }

        public ITransactionCommitter CreateTransactionCommitter()
        {
            return new TransactionCommitterMultiSource(_defaultDataAccessor, _dataAccessors);
        }

        public void AddDataAccessor(Type type, IDataAccessor dataAccessor)
        {
            _dataAccessors.Add(type, dataAccessor);
            _multiSourceBusinessObjectLoader.AddBusinessObjectLoader(type, dataAccessor.BusinessObjectLoader);
        }
    }
}