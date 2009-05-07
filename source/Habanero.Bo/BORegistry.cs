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

using System;
using System.Collections.Generic;
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
    ///<summary>
    /// This is a global registry used by the Business object layer.
    /// This class allows you to set and get Globally accessable objects. E.g. DataAccessor
    ///</summary>
    public class BORegistry
    {
        private static IDataAccessor _defaultDataAccessor;
        private static readonly Dictionary<Type, IDataAccessor> _dataAccessors = new Dictionary<Type, IDataAccessor>();

        /// <summary>
        /// Gets and sets the DataAccessor to be used. This determines the location your
        /// BusinessObjects will persist to and load from (eg a DataAccessorDB would make the BusinessObjects
        /// persist to and load from the database).
        /// </summary>
        public static IDataAccessor DataAccessor
        {
            get
            {
                if (_defaultDataAccessor == null) 
                    throw new HabaneroApplicationException("The DataAccessor has not been set up on BORegistry. Please initialise it before attempting to load or save Business Objects.");
                return _defaultDataAccessor;
            }
            set { _defaultDataAccessor = value; }
        }

        /// <summary>
        /// Adds a custom <see cref="IDataAccessor"/> to use for a specific type.
        /// Use this facility to support different databases in the same application.
        /// </summary>
        /// <param name="type">The type for this accessor</param>
        /// <param name="dataAccessor">The data accessor to use for this type</param>
        public static void AddDataAccessor(Type type, IDataAccessor dataAccessor)
        {
            _dataAccessors.Add(type, dataAccessor);
        }

        /// <summary>
        /// Gets the data accessor for the specific type, if it has been assigned using
        /// <see cref="AddDataAccessor"/>.  If none is found,
        /// the default IDataAccessor will be found, as provided by the 
        /// <see cref="DataAccessor"/> property.
        /// </summary>
        /// <param name="type">The type to search on</param>
        /// <returns>Returns a custom data accessor</returns>
        public static IDataAccessor GetDataAccessor(Type type)
        {
            if (_dataAccessors.ContainsKey(type))
            {
                return _dataAccessors[type];
            }
            return DataAccessor;
        }

        /// <summary>
        /// Clears the registry of any custom accessors that have been set
        /// for specific types.  The default <see cref="IDataAccessor"/> as defined in the
        /// <see cref="DataAccessor"/> property remains intact.
        /// </summary>
        public static void ClearCustomDataAccessors()
        {
            _dataAccessors.Clear();
        }
    }
}
