// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
    /// <summary>
    /// An enumeration specifying the means used to preserve a class
    /// inheritance structure when writing to a database, since relational
    /// databases don't support inheritance.
    /// </summary>
    public enum ORMapping
    {
        /// <summary>
        /// Uses one database table per class in the inheritance structure.
        /// As per (Fowler (285) - 'Patterns of Enterprise Application Architecture')
        /// </summary>
        ClassTableInheritance,
        /// <summary>
        /// Maps all fields of all classes of an inheritance structure into a single table.
        /// As per (Fowler (278) - 'Patterns of Enterprise Application Architecture')
        /// </summary>
        SingleTableInheritance,
        /// <summary>
        /// Uses a table for each concrete class in the inheritance hierarchy.
        /// As per (Fowler (293) - 'Patterns of Enterprise Application Architecture')
        /// </summary>
        ConcreteTableInheritance
    }
}