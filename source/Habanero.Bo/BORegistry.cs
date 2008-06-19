using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;

namespace Habanero.BO
{
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
            get { return _dataAccessor; }
            set { _dataAccessor = value; }
        }
    }
}
