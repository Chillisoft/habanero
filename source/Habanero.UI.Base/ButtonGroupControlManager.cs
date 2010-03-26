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
using Habanero.Base;

namespace Habanero.UI.Base
{
    /// <summary>
    /// This manager groups common logic for IButtonGroupControl objects.
    /// Do not use this object in working code - rather call CreateButtonGroupControl
    /// in the appropriate control factory.
    /// </summary>
    public class ButtonGroupControlManager
    {
        private readonly IControlFactory _controlFactory;
        private readonly FlowLayoutManager _layoutManager;
        private readonly IButtonGroupControl _buttonGroupControl;
        
        ///<summary>
        /// Constructor for the <see cref="ButtonGroupControlManager"/>
        ///</summary>
        ///<param name="buttonGroupControl"></param>
        ///<param name="controlFactory"></param>
        public ButtonGroupControlManager(IButtonGroupControl buttonGroupControl, IControlFactory controlFactory)
        {
            _buttonGroupControl = buttonGroupControl;
            _layoutManager = new FlowLayoutManager(_buttonGroupControl, controlFactory);
            _layoutManager.Alignment = FlowLayoutManager.Alignments.Right;
            _controlFactory = controlFactory;
            IButton sampleBtn = _controlFactory.CreateButton();
            _buttonGroupControl.Height = sampleBtn.Height + 10;
        }
        
        ///<summary>
        /// Returns the Layout manager being used by this control.
        ///</summary>
        public FlowLayoutManager LayoutManager
        {
            get { return _layoutManager; }
        }

        ///<summary>
        /// Adds a button with te button name and text equal to buttonName.
        ///</summary>
        ///<param name="buttonName"></param>
        ///<returns></returns>
        public IButton AddButton(string buttonName)
        {
            IButton button = _controlFactory.CreateButton();
            button.Name = buttonName;
            button.Text = buttonName;
            _layoutManager.AddControl(button);
            _buttonGroupControl.Controls.Add(button);
            return button;
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
            IButton button = this.AddButton(buttonName);
            button.Name = buttonName;
            button.Text = buttonText;
            if (clickHandler != null) button.Click += (sender,e) =>
            {
                try
                {
                    clickHandler(sender, e);
                }
                catch (Exception ex)
                {
                    GlobalRegistry.UIExceptionNotifier.Notify(ex, "Error performing action", "Error");
                }
            };

            return button;
        }
    }
}