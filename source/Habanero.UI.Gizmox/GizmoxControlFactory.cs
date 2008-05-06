using System.Collections;
using System.Collections.Generic;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Gizmox
{
    public class GizmoxControlFactory : IControlFactory
    {
        public IFilterControl CreateFilterControl()
        {
            return new FilterControlGiz(this);
        }

        public ITextBox CreateTextBox()
        {
            return new TextBoxGiz(this);
        }

        public IListBox CreateListBox()
        {
            return new ListBoxGiz(this);
        }

        public IMultiSelector<T> CreateMultiSelector<T>()
        {
            return new MultiSelectorGiz<T>(this);
        }

        public IButton CreateButton()
        {
            return new ButtonGiz();
        }
    }
}
