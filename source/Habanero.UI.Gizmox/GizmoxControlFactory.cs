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
    }
}
