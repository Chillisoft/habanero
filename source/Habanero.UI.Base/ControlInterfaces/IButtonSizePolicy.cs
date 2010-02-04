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
namespace Habanero.UI.Base
{
    /// <summary>
    /// Specifies an object that calculates button sizes.  This is used by the <see cref="IButtonGroupControl"/> when a button is added to it.  You can control
    /// how the buttons are laid out on the <see cref="IButtonGroupControl"/> by creating a class that implements <see cref="IButtonSizePolicy"/>
    /// and setting the <see cref="IButtonGroupControl.ButtonSizePolicy"/> property on your <see cref="IButtonGroupControl"/>.
    /// </summary>
    public interface IButtonSizePolicy
    {
        /// <summary>
        /// Recalculates the button sizes of the given collection of buttons.
        /// </summary>
        /// <param name="buttonCollection"></param>
        void RecalcButtonSizes(IControlCollection buttonCollection);
    }
}