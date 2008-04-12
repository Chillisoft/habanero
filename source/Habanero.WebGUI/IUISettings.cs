//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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

namespace Habanero.WebGUI
{
    /// <summary>
    /// Assign a method to this delegate that returns a boolean
    /// to indicate whether the user has permission to right-click
    /// on the ComboBox that represents the given
    /// BusinessObject type.  This applies to all ComboBoxes in the
    /// application that are mapped using a Habanero ComboBoxMapper,
    /// but the individual XML class definition parameter settings for
    /// a field take precedence
    /// </summary>
    /// <param name="boClassType">The class type of the BusinessObject
    /// being mapped in the ComboBox</param>
    /// <param name="controlMapper">The control mapper that maps the
    /// BusinessObject to the ComboBox.  This mapper will provide
    /// information like the BusinessObject of the form.</param>
    public delegate bool PermitComboBoxRightClickDelegate(Type boClassType, IControlMapper controlMapper);

    /// <summary>
    /// An interface to model a class that stores application-wide
    /// settings for the user interface
    /// </summary>
    public interface IUISettings
    {
        /// <summary>
        /// Assign a method to this delegate that returns a boolean
        /// to indicate whether the user has permission to right-click
        /// on the ComboBox that represents the given
        /// BusinessObject type.  This applies to all ComboBoxes in the
        /// application that are mapped using a Habanero ComboBoxMapper,
        /// but the individual XML class definition parameter settings for
        /// a field take precedence.
        /// </summary>
        PermitComboBoxRightClickDelegate PermitComboBoxRightClick
        {
            get;
            set;
        }
    }
}