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

namespace Habanero.Base
{
    public interface IRelationship
    {
        /// <summary>
        /// Returns the set of business objects that relate to this one
        /// through the specific relationship
        /// </summary>
        /// <returns>Returns a collection of business objects</returns>
        IBusinessObjectCollection GetRelatedBusinessObjectCol();

        IRelKey RelKey
        {
            get;
        }

        ///<summary>
        /// 
        ///</summary>
        OrderCriteria OrderCriteria
        {
            get;
        }
    }
}