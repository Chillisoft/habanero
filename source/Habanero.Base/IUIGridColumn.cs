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
using System;
using System.Collections;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Interface describing a column of a grid.  This is implemented by UIGridColumn.
    /// </summary>
    public interface IUIGridColumn
    {
        /// <summary>
        /// Returns the heading text that will be used for this column.
        /// </summary>
        string Heading { get; set; }

        /// <summary>
        /// Returns the property name
        /// </summary>
        string PropertyName { get; set; }

        /// <summary>
        /// Indicates whether the column is editable
        /// </summary>
        bool Editable { get; set; }

        /// <summary>
        /// Returns the width
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Returns the horizontal alignment
        /// </summary>
        PropAlignment Alignment { get; set; }

        /// <summary>
        /// Returns the Hashtable containing the property parameters
        /// </summary>
        Hashtable Parameters { get; }

        /// <summary>
        /// Gets and sets the name of the grid control type
        /// </summary>
        String GridControlTypeName { get; set; }

        /// <summary>
        /// Gets and sets the assembly name of the grid control type
        /// </summary>
        String GridControlAssemblyName { get; set; }

        ///<summary>
        /// The <see cref="IUIGrid">Grid Definition</see> that this IUIGridColumn belongs to.
        ///</summary>
        IUIGrid UIGrid { get; set; }

        ///<summary>
        /// The <see cref="IClassDef">ClassDefinition</see> that this IUIGridColumn belongs to.
        ///</summary>
        IClassDef ClassDef { get; }

        /// <summary>
        /// Returns the LookupList for the PropDef that 
        /// is associated with this PropDef.
        /// If there is no PropDef associated with this column
        /// then returns NullLookupList.
        /// </summary>
        ILookupList LookupList { get; }

        /// <summary>
        /// Returns the PropDef associated with this UIGridColumn. If there is one
        /// If this GridColumn is for a reflective Prop then returns null.
        /// </summary>
        IPropDef PropDef { get; }

        /// <summary>
        /// Return true if this UIGridColumn is associated with a <see cref="IPropDef"/>.
        /// This is used since a GridColumn can be associated with
        /// Reflective Property 
        /// </summary>
        bool HasPropDef { get; }

        ///<summary>
        /// Gets the heading for this grid column.
        ///</summary>
        ///<returns> The heading for this grid column </returns>
        string GetHeading();

        ///<summary>
        /// Gets the heading for this grid column given a classDef.
        ///</summary>
        ///<param name="classDef">The class definition that corresponds to this grid column. </param>
        ///<returns> The heading for this grid column </returns>
        [Obsolete("This is no longer required since the IUIGridColumn can now acquire its ClassDef")]
        string GetHeading(IClassDef classDef);

        /// <summary>
        /// Returns the parameter value for the name provided
        /// </summary>
        /// <param name="parameterName">The parameter name</param>
        /// <returns>Returns the parameter value or null if not found</returns>
        /// TODO this should return a string
        object GetParameterValue(string parameterName);

        ///<summary>
        /// Gets the heading for this grid column.
        ///</summary>
        ///<returns> The heading for this grid column </returns>
        Type GetPropertyType();
    }
}