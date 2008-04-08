//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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

using System.Data;

namespace Habanero.Base
{
    /// <summary>
    /// An interface to model an object initialiser
    /// </summary>
    public interface IObjectInitialiser
    {
        /// <summary>
        /// Initialises the given object
        /// </summary>
        /// <param name="objToInitialise">The object to initialise</param>
        void InitialiseObject(object objToInitialise);

        /// <summary>
        /// Initialises a DataRow object
        /// </summary>
        /// <param name="row">The DataRow object to initialise</param>
        void InitialiseDataRow(DataRow row);
    }
}