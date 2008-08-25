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

using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.Base
{
    /// <summary>
    /// A model of a Select Query that can be used to load data from a data store.  This includes the Fields to load, the source to load from
    /// (such as the database table name), the OrderCriteria to use (what fields must be sorted on), the Criteria to use (only objects that
    /// match the given criteria will be loaded), and the number of objects to load (defined by the Limit).
    /// The SelectQuery provides an implementation of the QueryObject Pattern (Fowler (316) - 'Patterns of Enterprise Application Architecture'
    ///   - 'An object that represents a database query'). The use of this object allows the Framework to generate the appropriate
    ///   sql for any given database ase well as to load object from other data sources such as .xml or .csv files.
    /// </summary>
    public interface ISelectQuery
    {
        /// <summary>
        /// The Criteria to use when loading. Only objects that match these criteria will be loaded.
        /// </summary>
        Criteria Criteria { get; set; }

        /// <summary>
        /// The fields to load from the data store.
        /// </summary>
        Dictionary<string, QueryField> Fields { get; }

        /// <summary>
        /// The source of the data. In a database query this would be the first table listed in the FROM clause.
        /// </summary>
        Source Source { get; set; }

        /// <summary>
        /// The fields to use to order a collection of objects when loading them.
        /// </summary>
        OrderCriteria OrderCriteria { get; set; }

        /// <summary>
        /// The number of objects to load
        /// </summary>
        int Limit
        {
            get;
            set;
        }

        /// <summary>
        /// The classdef this select query corresponds to. This can be null if the select query is being used
        /// without classdefs, but if it is built using the QueryBuilder 
        /// </summary>
        IClassDef ClassDef
        {
            get;
            set;
        }

        ///<summary>
        ///</summary>
        Criteria DiscriminatorCriteria { get; set; }
    }
}