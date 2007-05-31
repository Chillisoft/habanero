using System;
using System.Windows.Forms;

namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// A control for a set of buttons in a user interface
    /// </summary>
    public class ButtonControl : UserControl
    {
        private FlowLayoutManager layoutManager;
        private v2.ControlCollection itsButtons;

        /// <summary>
        /// Constructor to initialise a new button controller.  Sets up a new
        /// layout manager for the buttons that will be added.
        /// </summary>
        public ButtonControl()
        {
            layoutManager = new FlowLayoutManager(this);
            layoutManager.Alignment = FlowLayoutManager.Alignments.Right;
            itsButtons = new v2.ControlCollection();
            Button sample = ControlFactory.CreateButton("Sample");
            this.Height = sample.Height + 10;
        }

        /// <summary>
        /// Adds a new button to the control by the name specified
        /// </summary>
        /// <param name="name">The name to appear on the button</param>
        /// <returns>Returns the Button object created</returns>
        public Button AddButton(string name)
        {
            Button btn = ControlFactory.CreateButton(name);
            btn.Text = name;

            itsButtons.Add(btn);
            layoutManager.AddControl(btn);
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
            btn.UseMnemonic = true;
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
            foreach (Button btn in itsButtons)
            {
                Label lbl = new Label();
                lbl.Text = btn.Text;
                if (lbl.PreferredWidth + 10 > maxButtonWidth)
                {
                    maxButtonWidth = lbl.PreferredWidth + 10;
                }
            }
            if (maxButtonWidth < Screen.PrimaryScreen.Bounds.Width/16)
            {
                maxButtonWidth = Screen.PrimaryScreen.Bounds.Width/16;
            }
            foreach (Button btn in itsButtons)
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
                foreach (Button button in itsButtons)
                {
                    if (button.Name.Replace("&", "") == buttonName)
                    {
                        return button;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Sets the default button in this control that would be chosen
        /// if the user pressed Enter without changing the focus
        /// </summary>
        /// <param name="buttonName">The name of the button</param>
        public void SetDefaultButton(string buttonName)
        {
            this.FindForm().AcceptButton = this[buttonName];
        }
    }
}