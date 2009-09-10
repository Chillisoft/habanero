//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
using System.Data;
using Habanero.BO.ClassDefinition;

namespace Habanero.Base
{
    /// <summary>
    /// An interface to model data-set providers
    /// </summary>
    public interface IDataSetProvider
    {
        /// <summary>
        /// Returns a data table with the UIGridDef provided
        /// </summary>
        /// <param name="uiGrid">The UIGridDef</param>
        /// <returns>Returns a DataTable object</returns>
        DataTable GetDataTable(IUIGrid uiGrid);

        /// <summary>
        /// Finds an IBusinessObject given the ID.
        /// </summary>
        /// <param name="objectID">The id of the bo to search for</param>
        /// <returns>The business object corresponding to the ID</returns>
        IBusinessObject Find(Guid objectID);

        /// <summary>
        /// Returns the business object at the row number specified
        /// </summary>
        /// <param name="rowNum">The row number</param>
        /// <returns>Returns a business object</returns>
        IBusinessObject Find(int rowNum);

        /// <summary>
        /// Returns the business object at the row specified
        /// </summary>
        /// <param name="row">The row related to the business object</param>
        /// <returns>Returns a business object</returns>
        IBusinessObject Find(DataRow row);

        /// <summary>
        /// Finds the row number in which a specified business object resides
        /// </summary>
        /// <param name="bo">The business object to search for</param>
        /// <returns>Returns the row number if found, or -1 if not found</returns>
        int FindRow(IBusinessObject bo);

        ///<summary>
        /// Updates the row values for the specified <see cref="IBusinessObject"/>.
        ///</summary>
        ///<param name="businessObject">The <see cref="IBusinessObject"/> for which the row values need to updated.</param>
        void UpdateBusinessObjectRowValues(IBusinessObject businessObject);

        ///<summary>
        /// The column name used for the <see cref="DataTable"/> column which stores the unique object identifier of the <see cref="IBusinessObject"/>.
        /// This column's values will always be the current <see cref="IBusinessObject"/>'s <see cref="IBusinessObject.ID"/> value.
        ///</summary>
        string IDColumnName { get; }

        ///<summary>
        /// Gets and sets whether the property update handler shold be set or not.
        /// This is used to 
        ///    change behaviour typically to differentiate behaviour
        ///    between windows and web.
        ///Typically in windows every time a business object property is changed
        ///   the grid is updated with Web the grid is updated only when the object
        ///    is persisted.
        /// </summary>
        bool RegisterForBusinessObjectPropertyUpdatedEvents { get; set; }
    }
}