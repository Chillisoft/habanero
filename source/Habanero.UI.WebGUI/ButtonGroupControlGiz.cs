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
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    /// <summary>
    /// Manages a group of buttons that display next to each other
    /// </summary>
    [MetadataTag("P")]
    public class ButtonGroupControlGiz : PanelGiz, IButtonGroupControl
    {
        private ButtonGroupControlManager _buttonGroupControlManager;
        private IControlFactory _controlFactory;

        public ButtonGroupControlGiz(IControlFactory controlFactory)
        {
            _buttonGroupControlManager = new ButtonGroupControlManager(this, controlFactory);

            //_layoutManager = new FlowLayoutManager(this, controlFactory);
            //_layoutManager.Alignment = FlowLayoutManager.Alignments.Right;
            _controlFactory = controlFactory;
            //IButton sampleBtn = _controlFactory.CreateButton();
            //this.Height = sampleBtn.Height + 10;
        }

        /// <summary>
        /// Adds a new button to the control with a specified name
        /// </summary>
        /// <param name="buttonName">The name to appear on the button</param>
        /// <returns>Returns the Button object created</returns>
        public IButton AddButton(string buttonName)
        {
            //IButton button = _controlFactory.CreateButton();
            //button.Name = buttonName;
            //button.Text = buttonName;
            //_layoutManager.AddControl(button);
            //RecalcButtonSizes();
            //Controls.Add((Control) button);
            //return button;

            IButton button = _buttonGroupControlManager.AddButton(buttonName);
            RecalcButtonSizes();
            return button;
        }

        /// <summary>
        /// A facility to index the buttons in the control so that they can
        /// be accessed like an array (eg. button["name"])
        /// </summary>
        /// <param name="buttonName">The name of the button</param>
        /// <returns>Returns the button found by that name, or null if not
        /// found</returns>
        public IButton this[string buttonName]
        {
            get { return (IButton) this.Controls[buttonName]; }
        }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }

        /// <summary>
        /// Sets the default button in this control that would be chosen
        /// if the user pressed Enter without changing the focus
        /// </summary>
        /// <param name="buttonName">The name of the button</param>
        public void SetDefaultButton(string buttonName)
        {
            ///not implemented in GIz
        }

        /// <summary>
        /// Adds a new button to the control with a specified name and
        /// with an attached event handler to carry out
        /// further actions if the button is pressed
        /// </summary>
        /// <param name="buttonName">The name to appear on the button</param>
        /// <param name="clickHandler">The method that handles the Click event</param>
        /// <returns>Returns the Button object created</returns>
        public IButton AddButton(string buttonName, EventHandler clickHandler)
        {
            //IButton button = this.AddButton(buttonName);
            ////TODO: Not supported by Gizmox button.UseMnemonic = true;
            //button.Click += clickHandler;
            //return button;
            return AddButton(buttonName, buttonName, clickHandler);
        }

        /// <summary>
        /// Adds a new button to the control with a specified name, specified text and
        /// with an attached event handler to carry out
        /// further actions if the button is pressed
        /// </summary>
        /// <param name="buttonName">The name that the button is created with</param>
        /// <param name="buttonText">The text to appear on the button</param>
        /// <param name="clickHandler">The method that handles the Click event</param>
        /// <returns>Returns the Button object created</returns>
        public IButton AddButton(string buttonName, string buttonText, EventHandler clickHandler)
        {
            //IButton button = this.AddButton(buttonName);
            //button.Name = buttonName;
            //button.Text = buttonText;
            //RecalcButtonSizes();
            //button.Click += clickHandler;
            //return button;
            IButton button = _buttonGroupControlManager.AddButton(buttonName, buttonText, clickHandler);
            RecalcButtonSizes();
            return button;
        }

        /// <summary>
        /// A method called by AddButton() to recalculate the size of the
        /// button
        /// </summary>
        public void RecalcButtonSizes()
        {
            int maxButtonWidth = 0;
            foreach (IButton btn in _buttonGroupControlManager.LayoutManager.ManagedControl.Controls)
            {
                ILabel lbl = _controlFactory.CreateLabel(btn.Text);
                if (lbl.PreferredWidth + 10 > maxButtonWidth)
                {
                    maxButtonWidth = lbl.PreferredWidth + 10;
                }
            }
            //if (maxButtonWidth < Screen.PrimaryScreen.Bounds.Width / 16)
            //{
            //    maxButtonWidth = Screen.PrimaryScreen.Bounds.Width / 16;
            //}
            foreach (IButton btn in _buttonGroupControlManager.LayoutManager.ManagedControl.Controls)
            {
                btn.Width = maxButtonWidth;
            }
        }
    }


}