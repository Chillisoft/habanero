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

    /// <summary>Represents the method that will handle the <see cref="IControlHabanero"></see>.KeyPress event of a <see cref="IControlHabanero"></see>.</summary>
    /// <filterpriority>2</filterpriority>
    public delegate void KeyPressEventHandler(object sender, KeyPressEventArgs e);


    /// <summary>Provides data for the <see cref="IControlHabanero"></see>.KeyPress event.</summary>
    /// <filterpriority>2</filterpriority>
    //[Serializable()]
	public class KeyPressEventArgs : EventArgs
    {
        /// <summary>Initializes a new instance of the <see cref="KeyPressEventArgs"></see> class.</summary>
        /// <param name="keyChar">The ASCII character corresponding to the key the user pressed. </param>
        public KeyPressEventArgs(char keyChar)
        {
            this.KeyChar = keyChar;
        }


        /// <summary>Gets or sets a value indicating whether the <see cref="IControlHabanero"></see>.KeyPress event was handled.</summary>
        /// <returns>true if the event is handled; otherwise, false.</returns>
        /// <filterpriority>1</filterpriority>
        public bool Handled { get; set; }

        /// <summary>Gets or sets the character corresponding to the key pressed.</summary>
        /// <returns>The ASCII character that is composed. For example, if the user presses SHIFT + K, this property returns an uppercase K.</returns>
        /// <filterpriority>1</filterpriority>
        public char KeyChar { get; set; }
    } 
}
