//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
    /// Specifies which scroll bars will be visible on a control
    /// </summary>
    public enum ScrollBars
    {
        /// <summary>
        /// No scroll bars are shown
        /// </summary>
        None,
        /// <summary>
        /// Only horizontal scroll bars are shown
        /// </summary>
        Horizontal,
        /// <summary>
        /// Only vertical scroll bars are shown
        /// </summary>
        Vertical,
        /// <summary>
        /// Both horizontal and vertical scroll bars are shown
        /// </summary>
        Both
    }

    /// <summary>
    /// Represents a TextBox control
    /// </summary>
    public interface ITextBox : IControlHabanero
    {
        /// <summary>
        /// Gets or sets a value indicating whether this is a multiline TextBox control
        /// </summary>
        bool Multiline { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether pressing ENTER in a multiline TextBox
        /// control creates a new line of text in the control or activates the default button for the form
        /// </summary>
        bool AcceptsReturn { get; set; }

        /// <summary>
        /// Gets or sets the character used to mask characters of a password in a single-line TextBox control.
        /// Set the value of this property to 0 (character value) if you do not want
        /// the control to mask characters as they are typed. Equals 0 (character value) by default.
        /// </summary>
        char PasswordChar { get; set; }

        /// <summary>
        /// Gets or sets which scroll bars should appear in a multiline TextBox control
        /// </summary>
        ScrollBars ScrollBars { get; set; }

        /// <summary>
        /// Gets or sets the alignment of text in the TextBox control
        /// </summary>
        HorizontalAlignment TextAlign { get; set; }

        /// <summary>
        /// Selects all text in the text box
        /// </summary>
        void SelectAll();
    }
}