#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion

using System.Data;

namespace Habanero.Base
{
    /// <summary>
    /// An interface to model a business object initialiser: A business object initialiser is used by the Habanero framework's standard forms
    ///  and grids to initialise a new object with data. There are default object initialisers that will be used in the case where
    ///  a custom initialiser is not specified. 
    /// </summary>
    public interface IBusinessObjectInitialiser
    {
        /// <summary>
        /// Initialises the given object
        /// </summary>
        /// <param name="objToInitialise">The object to initialise</param>
        void InitialiseObject(IBusinessObject objToInitialise);

        /// <summary>
        /// Initialises a DataRow object
        /// </summary>
        /// <param name="row">The DataRow object to initialise</param>
        void InitialiseDataRow(DataRow row);
    }


    /// <summary>
    /// An interface to model a business object initialiser: A business object initialiser is used by the Habanero framework's standard forms
    ///  and grids to initialise a new object with data. There are default object initialisers that will be used in the case where
    ///  a custom initialiser is not specified. 
    /// </summary>
    public interface IBusinessObjectInitialiser<T> : IBusinessObjectInitialiser where T : IBusinessObject
    {
        /// <summary>
        /// Initialises the given object
        /// </summary>
        /// <param name="objToInitialise">The object to initialise</param>
        void InitialiseObject(T objToInitialise);
    }
}