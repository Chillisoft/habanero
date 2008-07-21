using System;

namespace Habanero.UI.Base
{
    public class ButtonGroupControlManager
    {
        private IControlFactory _controlFactory;
        private FlowLayoutManager _layoutManager;
        private IButtonGroupControl _buttonGroupControl;

        public ButtonGroupControlManager(IButtonGroupControl buttonGroupControl, IControlFactory controlFactory)
        {
            _buttonGroupControl = buttonGroupControl;
            _layoutManager = new FlowLayoutManager(_buttonGroupControl, controlFactory);
            _layoutManager.Alignment = FlowLayoutManager.Alignments.Right;
            _controlFactory = controlFactory;
            IButton sampleBtn = _controlFactory.CreateButton();
            _buttonGroupControl.Height = sampleBtn.Height + 10;
        }

        public FlowLayoutManager LayoutManager
        {
            get { return _layoutManager; }
        }

        public IButton AddButton(string buttonName)
        {
            IButton button = _controlFactory.CreateButton();
            button.Name = buttonName;
            button.Text = buttonName;
            _layoutManager.AddControl(button);
            //RecalcButtonSizes();
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
            //RecalcButtonSizes();
            button.Click += clickHandler;
            return button;
        }

       
    }
}