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

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// An enumeration specifying the means used to preserve a class
    /// inheritance structure when writing to a database, since relational
    /// databases don't support inheritance.
    /// </summary>
    public enum ORMapping
    {
        /// <summary>
        /// Uses one database table per class in the inheritance structure
        /// </summary>
        ClassTableInheritance,
        /// <summary>
        /// Maps all fields of all classes of an inheritance structure into a single table
        /// </summary>
        SingleTableInheritance,
        /// <summary>
        /// Uses a table for each concrete class in the inheritance hierarchy
        /// </summary>
        ConcreteTableInheritance
    }
}