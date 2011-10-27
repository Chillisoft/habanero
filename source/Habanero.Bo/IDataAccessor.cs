//  ---------------------------------------------------------------------------------
//   Copyright (C) 2007-2010 Chillisoft Solutions
//   
//   This file is part of the Habanero framework.
//   
//       Habanero is a free framework: you can redistribute it and/or modify
//       it under the terms of the GNU Lesser General Public License as published by
//       the Free Software Foundation, either version 3 of the License, or
//       (at your option) any later version.
//   
//       The Habanero framework is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU Lesser General Public License for more details.
//   
//       You should have received a copy of the GNU Lesser General Public License
//       along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//  ---------------------------------------------------------------------------------
using Habanero.Base;

namespace Habanero.BO
{

    /// <summary>
    /// Defines a method of loading and persisting. To create a new implementation of IDataAccessor you will need to create 
    /// an implementation of <see cref="IBusinessObjectLoader"/> and of <see cref="ITransactionCommitter"/> first.  These will
    /// define how BusinessObjects are loaded and persisted to and from a data store.  This class simply brings these under one
    /// umbrella, and so together they define your data access method.
    /// 
    /// To use the supplied DataAccessorDB, which loads and persists using a database as its data store, you would set up your
    /// IDataAccessor somewhere in your application startup as follows:
    /// 
    /// <code>BORegistry.DataAccessor = new DataAccessorDB(DatabaseConnection.CurrentConnection)</code>
    /// 
    /// To do this, make sure you have set up your DatabaseConnection.CurrentConnection already.
    /// 
    /// This is done in HabaneroApp and its subtypes already, with the default being the DB DataAccessor - you can of course
    /// set your own if you'd like to instead.
    /// </summary>
    public interface IDataAccessor
    {

        /// <summary>
        /// The <see cref="BusinessObjectLoader"/> to use to load BusinessObjects
        /// </summary>
        IBusinessObjectLoader BusinessObjectLoader { get; }

        /// <summary>
        /// Creates a TransactionCommitter for you to use to persist BusinessObjects. A new TransactionCommitter is required
        /// each time an object or set of objects is persisted.
        /// </summary>
        /// <returns></returns>
        ITransactionCommitter CreateTransactionCommitter();
    }
}