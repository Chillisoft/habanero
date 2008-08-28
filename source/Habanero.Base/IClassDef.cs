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

using System;
using Habanero.Base.Exceptions;
using Habanero.BO.Base;

namespace Habanero.Base
{
    public interface IClassDef
    {
        /// <summary>
        /// Searches the property definition collection and returns the 
        /// property definition for the property with the name provided.
        /// </summary>
        /// <param name="propertyName">The property name in question</param>
        /// <returns>Returns the property definition if found, or
        /// throws an error if not</returns>
        /// <exception cref="InvalidPropertyNameException">
        /// This exception is thrown if the property is not found</exception>
        IPropDef GetPropDef(string propertyName);

        /// <summary>
        /// Searches the property definition collection and returns 
        /// the lookup-list found under the property with the
        /// name specified.  Also checks the super-class.
        /// </summary>
        /// <param name="propertyName">The property name in question</param>
        /// <returns>Returns the lookup-list if the property is
        /// found, or a NullLookupList object if not</returns>
        ILookupList GetLookupList(string propertyName);


        /// <summary>
        /// Creates a new business object using this class definition
        /// </summary>
        /// <returns>Returns the new object</returns>
        IBusinessObject CreateNewBusinessObject();

        /// <summary>
        /// The table this classdef maps to, if applicable.
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// The type of the class definition
        /// </summary>
        Type ClassType
        {
            get;
            set;
        }

        /// <summary>
        /// The collection of property definitions
        /// </summary>
        IPropDefCol PropDefcol
        {
            get;
            set;
        }

        /// <summary>
        /// The collection of property definitions for this
        /// class and any properties inherited from parent classes
        /// </summary>
        IPropDefCol PropDefColIncludingInheritance
        {
            get;
        }

        /// <summary>
        /// The name of the assembly for the class definition
        /// </summary>
        string AssemblyName
        {
            get;
            set;
        }

        /// <summary>
        /// The full name of the class type for the class definition (including the namespace)
        /// </summary>
        string ClassNameFull
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the class type for the class definition (excluding the namespace, but including the type parameter if applicable)
        /// </summary>
        string ClassName { get; set; }

        /// <summary>
        /// Returns the name of the table that applies to the propdef given, taking into allowance
        /// any inheritance structure.
        /// </summary>
        /// <param name="propDef">The propdef to map to a table name. This propdef must be part of this classdef Hierarchy</param>
        /// <returns></returns>
        string GetTableName(IPropDef propDef);

        ///<summary>
        /// Gets the type of the specified property for this classDef.
        /// The specified property can also have a format like the custom properties for a UiGridColumn or UiFormField def.
        /// eg: MyRelatedBo.MyFurtherRelatedBo|MyAlternateRelatedBo.Name
        ///</summary>
        ///<param name="propertyName">The property to get the type for.</param>
        ///<returns>The type of the specified property</returns>
        Type GetPropertyType(string propertyName);

        ///<summary>
        /// Creates a property comparer for the given property
        /// The specified property can also have a format like the custom properties for a UiGridColumn or UiFormField def.
        /// eg: MyRelatedBo.MyFurtherRelatedBo|MyAlternateRelatedBo.Name
        ///</summary>
        ///<param name="propertyName">The property to get the type for.</param>
        ///<returns>The type of the specified property</returns>
        IPropertyComparer<T> CreatePropertyComparer<T>(string propertyName) where T:IBusinessObject;

        /// <summary>
        /// Returns the table name for this class
        /// </summary>
        /// <returns>Returns the table name of first real table for this class.</returns>
        string GetTableName();

        /// <summary>
        /// Indicates whether ClassTableInheritance is being used. See
        /// the ORMapping enumeration for more detail.
        /// </summary>
        /// <returns>Returns true if so, or false if there is no
        /// super class or another type of inheritance is being used</returns>
        bool IsUsingClassTableInheritance();

        ///<summary>
        /// Returns a particular property definition for a class definition.
        ///</summary>
        ///<param name="source"></param>
        ///<param name="propertyName"></param>
        ///<param name="throwError"></param>
        ///<returns></returns>
        ///<exception cref="ArgumentException"></exception>
        IPropDef GetPropDef(Source source, string propertyName, bool throwError);
    }
}