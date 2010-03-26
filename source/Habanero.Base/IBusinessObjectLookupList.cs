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
    public interface IBusinessObjectLookupList : ILookupListWithClassDef
    {
        /// <summary>
        /// The assembly containing the class from which values are loaded
        /// </summary>
        string AssemblyName { get; set; }

        /// <summary>
        /// The class from which values are loaded
        /// </summary>
        string ClassName { get; set; }

        /// <summary>
        /// Gets and sets the sql criteria used to limit which objects
        /// are loaded in the BO collection
        /// </summary>
        Criteria Criteria { get; }

        /// <summary>
        /// This raw sort string.  Preferrably use <see cref="OrderCriteria"/>
        /// </summary>
        string SortString { get; }

        /// <summary>
        /// This raw criteria string.  Preferrably use <see cref="Criteria"/>
        /// </summary>
        string CriteriaString { get; }



        /// <summary>
        /// Gets and sets the sort string used to sort the lookup
        /// list.  This string must contain the name of a property
        /// belonging to the business object used to construct the list.
        /// The possible formats are: "property", "property asc",
        /// "property desc" and "property des".
        /// </summary>
        IOrderCriteria OrderCriteria
        {
            get; //set { _sort = FormatSortAttribute(value); }
        }
    }
}