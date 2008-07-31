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

namespace Habanero.BO.CriteriaManager
{
    /// <summary>
    /// This interface is created so that any component that is using the 
    /// expression and needs to build up valid sql syntax, will be able to 
    /// replace the parameter name (BO property name) with 
    /// a table name and field name.
    /// </summary>
    public interface IParameterSqlInfo
    {
        /// <summary>
        /// The name in the expression tree to be updated
        /// </summary>
        string ParameterName { get; }

        /// <summary>
        /// The table name to be added to the parameter
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// The field name to be added to the parameter
        /// </summary>
        string FieldName { get; }

        /// <summary>
        /// The parameter type to be added to the parameter
        /// </summary>
        ParameterType ParameterType { get; }
    }
}