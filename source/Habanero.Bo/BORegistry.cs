// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
    ///<summary>
    /// This is a global registry used by the Business object layer.
    /// This class allows you to set and get Globally accessable objects. E.g. DataAccessor
    ///</summary>
    public class BORegistry
    {
        private static IDataAccessor _dataAccessor;

        /// <summary>
        /// Gets and sets the DataAccessor to be used. This determines the location your
        /// BusinessObjects will persist to and load from (eg a DataAccessorDB would make the BusinessObjects
        /// persist to and load from the database).
        /// </summary>
        public static IDataAccessor DataAccessor
        {
            get
            {
                if (_dataAccessor == null) 
                    throw new HabaneroApplicationException("The DataAccessor has not been set up on BORegistry. Please initialise it before attempting to load or save Business Objects.");
                return _dataAccessor;
            }
            set { _dataAccessor = value; }
        }

    }
}
