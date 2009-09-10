//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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

using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Habanero.Base
{
    /// <summary>
    /// An interface that models a sql statement
    /// </summary>
    public interface ISqlStatement
    {
        /// <summary>
        /// Gets and sets the sql statement
        /// </summary>
        StringBuilder Statement { get; set; }

        /// <summary>
        /// Adds a parameter value without appending the name of the parameter to the statement.
        /// </summary>
        /// <param name="paramName">The parameter name</param>
        /// <param name="paramValue">The value to assign</param>
        /// <returns>Returns a IDbDataParameter object</returns>
        IDbDataParameter AddParameter(string paramName, object paramValue);

        /// <summary>
        /// Adds a parameter to the sql statement, creating the parameter name and appending it to the statement
        /// </summary>
        /// <param name="obj">The parameter to add</param>
        void AddParameterToStatement(object obj);

        /// <summary>
        /// Returns the list of parameters
        /// </summary>
        List<IDbDataParameter> Parameters { get; }

        /// <summary>
        /// Sets up the IDbCommand object
        /// </summary>
        /// <param name="command">The command</param>
        void SetupCommand(IDbCommand command);
    }
}