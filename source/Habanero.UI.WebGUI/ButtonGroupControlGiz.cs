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
using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class ButtonGroupControlGiz : ControlGiz, IButtonGroupControl
    {
        private readonly IControlFactory _controlFactory;
        private readonly FlowLayoutManager _layoutManager;
        public ButtonGroupControlGiz(IControlFactory controlFactory)
        {
            _layoutManager = new FlowLayoutManager(this, controlFactory);
            _layoutManager.Alignment = FlowLayoutManager.Alignments.Right;
            _controlFactory = controlFactory;
            IButton sampleBtn = _controlFactory.CreateButton();
            this.Height = sampleBtn.Height + 10;
        }

        public IButton AddButton(string buttonName)
        {
            IButton button = _controlFactory.CreateButton();
            button.Name = buttonName;
            button.Text = buttonName;
            _layoutManager.AddControl(button);
            RecalcButtonSizes();
            Controls.Add((Control) button);
            return button;
        }

        public IButton this[string buttonName]
        {
            get { return (IButton) this.Controls[buttonName]; }
        }
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }

        public void SetDefaultButton(string buttonName)
        {
            ///not implemented in GIz
        }

        public IButton AddButton(string buttonName, EventHandler clickHandler)
        {
            //IButton button = this.AddButton(buttonName);
            ////TODO: Not supported by Gizmox button.UseMnemonic = true;
            //button.Click += clickHandler;
            //return button;
            return AddButton(buttonName, buttonName, clickHandler);
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
            RecalcButtonSizes();
            button.Click += clickHandler;
            return button;
        }

        /// <summary>
        /// A method called by AddButton() to recalculate the size of the
        /// button
        /// </summary>
        public void RecalcButtonSizes()
        {
            int maxButtonWidth = 0;
            foreach (IButton btn in _layoutManager.ManagedControl.Controls)
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
            foreach (Button btn in _layoutManager.ManagedControl.Controls)
            {
                btn.Width = maxButtonWidth;
            }
        }
    }
}