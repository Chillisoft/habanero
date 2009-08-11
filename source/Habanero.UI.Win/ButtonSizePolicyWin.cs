using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class ButtonSizePolicyWin : IButtonSizePolicy
    {
        private readonly IControlFactory _controlFactory;

        public ButtonSizePolicyWin(IControlFactory controlFactory)
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
            if (maxButtonWidth < Screen.PrimaryScreen.Bounds.Width / 20)
            {
                maxButtonWidth = Screen.PrimaryScreen.Bounds.Width / 20;
            }
            foreach (IButton btn in buttonCollection)
            {
                btn.Width = maxButtonWidth;
            }
        }
    }
}