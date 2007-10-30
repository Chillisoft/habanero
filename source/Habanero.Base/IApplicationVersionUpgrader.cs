//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

namespace Habanero.Base
{
    /// <summary>
    /// An interface to model a version upgrade tool for an application.
    /// One common use of this interface is to provide a database update by
    /// using DBMigrator in the Upgrade() method.
    /// </summary>
    public interface IApplicationVersionUpgrader
    {
        /// <summary>
        /// Upgrades the application to the latest version
        /// </summary>
        void Upgrade();
    }
}