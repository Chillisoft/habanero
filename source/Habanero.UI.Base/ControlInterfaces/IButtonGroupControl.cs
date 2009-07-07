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

namespace Habanero.UI.Base
{
    /// <summary>
    /// Manages a group of buttons that display next to each other
    /// </summary>
    public interface IButtonGroupControl:IControlHabanero
    {
        /// <summary>
        /// Adds a new button to the control with a specified name
        /// </summary>
        /// <param name="buttonName">The name to appear on the button</param>
        /// <returns>Returns the Button object created</returns>
        IButton AddButton(string buttonName);

        /// <summary>
        /// Adds a new button to the control with a specified name and
        /// with an attached event handler to carry out
        /// further actions if the button is pressed
        /// </summary>
        /// <param name="buttonName">The name to appear on the button</param>
        /// <param name="clickHandler">The method that handles the Click event</param>
        /// <returns>Returns the Button object created</returns>
        IButton AddButton(string buttonName, EventHandler clickHandler);
 
        /// <summary>
        /// Adds a new button to the control with a specified name, specified text and
        /// with an attached event handler to carry out
        /// further actions if the button is pressed
        /// </summary>
        /// <param name="buttonName">The name that the button is created with</param>
        /// <param name="buttonText">The text to appear on the button</param>
        /// <param name="clickHandler">The method that handles the Click event</param>
        /// <returns>Returns the Button object created</returns>
        IButton AddButton(string buttonName, string buttonText, EventHandler clickHandler);

        /// <summary>
        /// A facility to index the buttons in the control so that they can
        /// be accessed like an array (eg. button["name"])
        /// </summary>
        /// <param name="buttonName">The name of the button</param>
        /// <returns>Returns the button found by that name, or null if not
        /// found</returns>
        IButton this[string buttonName] { get; }

        /// <summary>
        /// Sets the default button in this control that would be chosen
        /// if the user pressed Enter without changing the focus
        /// </summary>
        /// <param name="buttonName">The name of the button</param>
        void SetDefaultButton(string buttonName);

        /// <summary>
        /// Specifies the object that calculates button sizes for this <see cref="IButtonGroupControl"/>.  By default the buttons are all equally sized
        /// based on the width of the text on the largest button.  To set your own button sizes set this to 
        /// an instance of <see cref="ButtonSizePolicyUserDefined"/> class.
        /// </summary>
        IButtonSizePolicy ButtonSizePolicy { get; set; }
    }
}