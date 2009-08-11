using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    public class ButtonSizePolicyVWG : IButtonSizePolicy
    {
        private readonly IControlFactory _controlFactory;

        public ButtonSizePolicyVWG(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public void RecalcButtonSizes(IControlCollection buttonCollection)
        {
            int maxButtonWidth = 0;
            foreach (IButton btn in buttonCollection)
            {
                ILabel lbl = _controlFactory.CreateLabel(btn.Text);
                if (lbl.PreferredWidth + 10 > maxButtonWidth)
                {
                    maxButtonWidth = lbl.PreferredWidth + 10;
                }
            }
            foreach (IButton btn in buttonCollection)
            {
                btn.Width = maxButtonWidth;
            }
        }
    }
}