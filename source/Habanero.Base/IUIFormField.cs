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
using Habanero.BO.ClassDefinition;

namespace Habanero.Base
{
    ///<summary>
    /// interface for a ui form field
    ///</summary>
    public interface IUIFormField
    {
        /// <summary>
        /// Returns the property name
        /// </summary>
        string PropertyName { get; set; }

        /// <summary>
        /// Returns the mapper type name
        /// </summary>
        string MapperTypeName { get; set; }

        ///<summary>
        /// Returns the mapper assembly
        ///</summary>
        string MapperAssembly { get; }

        /// <summary>
        /// The name of the property type assembly
        /// </summary>
        string ControlAssemblyName { get; set; }

        /// <summary>
        /// The name of the control type
        /// </summary>
        string ControlTypeName { get; set; }

        /// <summary>
        /// Returns the control type
        /// </summary>
        Type ControlType { get; set; }

        /// <summary>
        /// Indicates whether the control is editable
        /// </summary>
        bool Editable { get;set; }

        /// <summary>
        /// Whether shown as compulsory on the form or not
        /// </summary>
        bool? ShowAsCompulsory { get;set; }

        ///<summary>
        /// Returns the text that will be shown in the Tool Tip for the control.
        ///</summary>
        string ToolTipText { get; }

        /// <summary>
        /// Returns the Hashtable containing the property attributes
        /// </summary>
        Hashtable Parameters { get; }

        ///// <summary>
        ///// Returns the collection of triggers managed by this
        ///// field
        ///// </summary>
        //ITriggerCol Triggers { get; }

        ///<summary>
        /// Is the field compulsory (i.e. must it be shown as compulsory on the form or not)
        ///</summary>
        bool IsCompulsory { get; }

        ///<summary>
        /// The <see cref="UIFormColumn"/> that this form field is to be placed in.
        ///</summary>
        IUIFormColumn UIFormColumn { get; set; }

        ///<summary>
        /// The <see cref="LayoutStyle"/> to be used for this form field.
        ///</summary>
        LayoutStyle Layout { get; set; }

        /// <summary>
        /// Returns the label
        /// </summary>
        string Label { get; set; }


        ///<summary>
        /// Gets the label for this form field.
        ///</summary>
        ///<returns> The label for this form field </returns>
        string GetLabel();

        /// <summary>
        /// Returns the parameter value for the name provided
        /// </summary>
        /// <param name="parameterName">The parameter name</param>
        /// <returns>Returns the parameter value or null if not found</returns>
        object GetParameterValue(string parameterName);

        ///<summary>
        /// Returns true if the UIFormField has a paramter value.
        ///</summary>
        ///<param name="parameterName"></param>
        ///<returns></returns>
        bool HasParameterValue(string parameterName);
    }
}