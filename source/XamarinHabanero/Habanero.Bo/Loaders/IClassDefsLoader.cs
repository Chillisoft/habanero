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
using System;
using Habanero.Base;

namespace Habanero.BO.Loaders
{
	/// <summary>
	/// An interface to model classes that load class definitions from
	/// xml data
	/// </summary>
	public interface IClassDefsLoader
	{
        
		/// <summary>
		/// Loads class definitions from the xmlPassed into the Loader.
		/// </summary>
        /// <returns>Returns an ClassDefCol containing the definitions</returns>
        [Obsolete("Please use parameterless LoadClassDefs() and pass the xml in via the constructor or another paramter")]
		ClassDefCol LoadClassDefs(string classDefsXml);

        /// <summary>
		/// Loads class definitions from loader source data
		/// </summary>
        /// <returns>Returns an ClassDefCol containing the definitions</returns>
		ClassDefCol LoadClassDefs();
	}
}