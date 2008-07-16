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
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class ButtonGroupControlWin : ControlWin, IButtonGroupControl
    {
        private readonly IControlFactory _controlFactory;

        public ButtonGroupControlWin(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public IButton AddButton(string buttonName)
        {
            return _controlFactory.CreateButton();
        }

        public IButton this[string buttonName]
        {
            get { throw new System.NotImplementedException(); }
        }

        public void SetDefaultButton(string buttonName)
        {
            throw new System.NotImplementedException();
        }

        public IButton AddButton(string buttonName, EventHandler clickHandler)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new button to the control by the name specified
        /// </summary>
        /// <param name="buttonName">The name that the button is created with</param>
        /// <returns>Returns the Button object created</returns>
        /// <param name="buttonText">The text to appear on the button</param>
        /// <param name="clickHandler">The event handler to be triggered on the button click</param>
        public IButton AddButton(string buttonName, string buttonText, EventHandler clickHandler)
        {
            throw new NotImplementedException();
        }
    }
}