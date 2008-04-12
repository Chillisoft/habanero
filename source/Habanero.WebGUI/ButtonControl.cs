#region Using

using System;
using System.Collections.Generic;
using Gizmox.WebGUI.Forms;

#endregion

namespace Habanero.WebGUI
{
    public partial class ButtonControl : UserControl
    {

        private readonly List<Button> _buttons;
        public ButtonControl()
        {
            InitializeComponent();

            _buttons = new List<Button>();
            
            Button sample = new Button();
            Height = sample.Height + 10;
        }

        /// <summary>
        /// Adds a new button to the control by the name specified
        /// </summary>
        /// <param name="name">The name to appear on the button</param>
        /// <returns>Returns the Button object created</returns>
        public Button AddButton(string name)
        {
            Button btn = new Button();
            btn.Name = name;
            btn.Text = name;

            _buttons.Add(btn);
            flowLayoutPanel1.Controls.Add(btn);
            RecalcButtonSizes();
            return btn;
        }

        /// <summary>
        /// Adds a button as before, but also specifies a handler method to 
        /// call when the button is pressed
        /// </summary>
        public Button AddButton(string name, EventHandler clickHandler)
        {
            Button btn = AddButton(name);
            //btn.UseMnemonic = true;
            btn.Click += clickHandler;
            return btn;
        }

        /// <summary>
        /// A method called by AddButton() to recalculate the size of the
        /// button
        /// </summary>
        public void RecalcButtonSizes()
        {
            int maxButtonWidth = 0;
            foreach (Button btn in _buttons)
            {
                Label lbl = new Label();
                lbl.Text = btn.Text;
                if (lbl.PreferredSize.Width + 10 > maxButtonWidth)
                {
                    maxButtonWidth = lbl.PreferredSize.Width + 10;
                }
            }
            if (maxButtonWidth < Screen.PrimaryScreen.Bounds.Width / 16)
            {
                maxButtonWidth = Screen.PrimaryScreen.Bounds.Width / 16;
            }
            foreach (Button btn in _buttons)
            {
                btn.Width = maxButtonWidth;
            }
        }

        /// <summary>
        /// Simulates the press of a button so that its click handler can
        /// be called
        /// </summary>
        /// <param name="buttonName">The name of the button</param>
        public void ClickButton(string buttonName)
        {
            Button button = this[buttonName];
            if (button == null) return;
            button.PerformClick();

        }

        /// <summary>
        /// Hides the button with the name specified
        /// </summary>
        /// <param name="buttonName">The name of the button</param>
        public void HideButton(string buttonName)
        {
            Button button = this[buttonName];
            if (button == null) return;
            button.Visible = false;
        }

        /// <summary>
        /// A facility to index the buttons in the control so that they can
        /// be accessed like an array (eg. button["name"])
        /// </summary>
        /// <param name="buttonName">The name of the button</param>
        /// <returns>Returns the button found by that name, or null if not
        /// found</returns>
        public Button this[string buttonName]
        {
            get
            {
                foreach (Button button in _buttons)
                {
                    if (button.Name.Replace("&", "") == buttonName)
                    {
                        return button;
                    }
                }
                return null;
            }
        }

        ///// <summary>
        ///// Sets the default button in this control that would be chosen
        ///// if the user pressed Enter without changing the focus
        ///// </summary>
        ///// <param name="buttonName">The name of the button</param>
        //public void SetDefaultButton(string buttonName)
        //{
        //    this.FindForm().AcceptButton = this[buttonName];
        //}
    }
}