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
    /// An implementation of <see cref="IButtonSizePolicy"/> that does no resizing, thus allowing you to specify your own button sizes for buttons on
    /// an <see cref="IButtonGroupControl"/>.
    /// </summary>
    public class ButtonSizePolicyUserDefined : IButtonSizePolicy
    {
        /// <summary>
        /// Recalculates the button sizes of the given collection of buttons.  This implementation does nothing to the buttons, allowing you to specify
        /// your own button sizes.
        /// </summary>
        /// <param name="buttonCollection"></param>
        public void RecalcButtonSizes(IControlCollection buttonCollection)
        {

        }
    }
}