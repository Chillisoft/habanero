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