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
using System.Data;
using System.Runtime.Serialization;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    /// <summary>
    /// This manager groups common logic for IDataGridView objects.
    /// Do not use this object in working code.
    /// </summary>
    public class DataGridViewManager
    {
        private readonly IDataGridView _grid;

        public DataGridViewManager(IDataGridView gridBase)
        {
            _grid = gridBase;
        }

        public void SetSortColumn(string columnName, bool isAscending)
        {
            if (_grid.DataSource is DataView)
            {
                if (isAscending)
                {
                    ((DataView) _grid.DataSource).Sort = columnName + " ASC";
                }
                else
                {
                    ((DataView) _grid.DataSource).Sort = columnName + " DESC";
                }
            }
        }        
    }    
}