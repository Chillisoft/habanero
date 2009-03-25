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

using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    /// <summary>
    /// An interface to model a mapper that wraps a control in
    /// order to display and capture information related to a business object 
    /// </summary>
    public interface IControlMapper
    {
        /// <summary>
        /// Returns the control being mapped
        /// </summary>
        IControlHabanero Control { get; }

        /// <summary>
        /// Returns the name of the property being edited in the control
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Controls access to the business object being represented
        /// by the control.  Where the business object has been amended or
        /// altered, the <see cref="UpdateControlValueFromBusinessObject"/> method can be called here to 
        /// implement the changes in the control itself.
        /// </summary>
        IBusinessObject BusinessObject { get; set; }

        /// <summary>
        /// Gets the error provider for this control <see cref="IErrorProvider"/>
        /// </summary>
        IErrorProvider ErrorProvider { get; }

        ///<summary>
        /// Gets and Sets the Class Def of the Business object whose property
        /// this control maps.
        ///</summary>
        IClassDef ClassDef { get; set; }

        /// <summary>
        /// Updates the properties on the represented business object
        /// </summary>
        void ApplyChangesToBusinessObject();

        /// <summary>
        /// Updates the value on the control from the corresponding property
        /// on the represented <see cref="BusinessObject"/>
        /// </summary>
        void UpdateControlValueFromBusinessObject();

        /// <summary>
        /// Sets the Error Provider Error with the appropriate value for the property e.g. if it is invalid then
        ///  sets the error provider with the invalid reason else sets the error provider with a zero length string.
        /// </summary>
        void UpdateErrorProviderErrorMessage();

        /// <summary>
        /// Returns the Error Provider's Error message.
        /// </summary>
        /// <returns></returns>
        string GetErrorMessage();
    }
}