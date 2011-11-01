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
    /// An interface to model a filter clause that filters which data to
    /// display, according to some criteria
    /// </summary>
    public interface IFilterClause
    {
        /// <summary>
        /// Returns the filter clause as a string. The filter clause is a clause used for filtering
        /// a ADO.Net <see cref="DataView"/>
        /// </summary>
        /// <returns>Returns a string</returns>
        string GetFilterClauseString();
       

       /// <summary>
       /// Returns the filter clause as a string. The filter clause will be delimited with a string like delimiter or a dateTime delimiter as
        /// appropriate this is to deal with the fact that the ADO.Net <see cref="DataView"/> uses * and # for the like and date delimiter respectively
        /// but most databases use % and '. NNB when loading a collection these will be interpretted appropriately for the database being loaded.
       /// </summary>
       /// <param name="stringLikeDelimiter">The delimiter to use in the case of a like clause e.g. * or %</param>
       /// <param name="dateTimeDelimiter">The delimiter to use in the case of a date time filter clause e.g. # for dataview</param>
       ///<returns>Returns a string</returns>
        string GetFilterClauseString(string stringLikeDelimiter, string dateTimeDelimiter);
    }
}
