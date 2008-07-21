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
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class ButtonGroupControlWin : ControlWin, IButtonGroupControl
    {
        private readonly IControlFactory _controlFactory;
        

        private ButtonGroupControlManager _buttonGroupControlManager;

        public ButtonGroupControlWin(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            _buttonGroupControlManager = new ButtonGroupControlManager(this, controlFactory);

            //_layoutManager = new FlowLayoutManager(this, controlFactory);
            //_layoutManager.Alignment = FlowLayoutManager.Alignments.Right;
            //_controlFactory = controlFactory;
            //IButton sampleBtn = _controlFactory.CreateButton();
            //this.Height = sampleBtn.Height + 10;
        }

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

        public IButton this[string buttonName]
        {
            get { return (IButton) this.Controls[buttonName]; }
        }

        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

        public void SetDefaultButton(string buttonName)
        {
            this.FindForm().AcceptButton = (Button)this[buttonName];
        }

        public IButton AddButton(string buttonName, EventHandler clickHandler)
        {
            //IButton button = this.AddButton(buttonName);
           
            //button.Click += clickHandler;
            //return button;
            IButton button = AddButton(buttonName, buttonName, clickHandler);
            
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
            //IButton button = this.AddButton(buttonName);
            //button.Name = buttonName;
            //button.Text = buttonText;
            //RecalcButtonSizes();
            //button.Click += clickHandler;
            //return button;
            IButton button = _buttonGroupControlManager.AddButton(buttonName, buttonText, clickHandler);
            RecalcButtonSizes();
            ((Button)button).UseMnemonic = true;
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
            if (maxButtonWidth < Screen.PrimaryScreen.Bounds.Width / 16)
            {
                maxButtonWidth = Screen.PrimaryScreen.Bounds.Width / 16;
            }
            foreach (IButton btn in _buttonGroupControlManager.LayoutManager.ManagedControl.Controls)
            {
                btn.Width = maxButtonWidth;
            }
        }

    }
}
