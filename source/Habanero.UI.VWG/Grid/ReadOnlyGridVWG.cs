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
using Habanero.BO;
using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// Provides a grid on which a business object collection can be
    /// listed but not edited.  If you would like more functionality,
    /// including the ability to add, edit and delete the objects, use
    /// <see cref="IReadOnlyGridControl"/> instead.
    /// </summary>
    public class ReadOnlyGridVWG : GridBaseVWG, IReadOnlyGrid
    {
        /// <summary>
        /// Constructs the <see cref="ReadOnlyGridVWG"/>
        /// </summary>
        public ReadOnlyGridVWG()
        {
            this.ReadOnly = true;
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.SelectionMode = Gizmox.WebGUI.Forms.DataGridViewSelectionMode.FullRowSelect;
        }

        /// <summary>
        /// Creates a dataset provider that is applicable to this grid. For example, a readonly grid would
        /// return a <see cref="ReadOnlyDataSetProvider"/>, while an editable grid would return an editable one.
        /// </summary>
        /// <param name="col">The collection to create the datasetprovider for</param>
        /// <returns>Returns the data set provider</returns>
        public override IDataSetProvider CreateDataSetProvider(IBusinessObjectCollection col)
        {
            ReadOnlyDataSetProvider dataSetProvider = new ReadOnlyDataSetProvider(col);
            dataSetProvider.RegisterForBusinessObjectPropertyUpdatedEvents = false;
            return dataSetProvider;
        }
    }
}