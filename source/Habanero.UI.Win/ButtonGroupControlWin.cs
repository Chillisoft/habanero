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
    }
}