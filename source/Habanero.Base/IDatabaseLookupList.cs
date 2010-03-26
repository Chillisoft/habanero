// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
namespace Habanero.Base
{
    public interface IDatabaseLookupList : ILookupListWithClassDef
    {
        /// <summary>
        /// Gets and sets the assembly name for the class being sourced for data
        /// </summary>
        string AssemblyName { get; set; }

        /// <summary>
        /// Gets and sets the class name being sourced for data
        /// </summary>
        string ClassName { get; set; }

        /// <summary>
        /// Gets the sql statement which is used to specify which
        /// objects to load for the lookup-list
        /// </summary>
        string SqlString { get; set; }
    }
}