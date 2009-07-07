using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// An implementation of <see cref="IButtonSizePolicy"/> that will size all the buttons equally based on the widest one. 
    /// </summary>
    public class ButtonSizePolicyVWG : IButtonSizePolicy
    {
        private readonly IControlFactory _controlFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controlFactory">The <see cref="IControlFactory"/> to use.</param>
        public ButtonSizePolicyVWG(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        /// <summary>
        /// Recalculates the button sizes of the given collection of buttons.
        /// </summary>
        /// <param name="buttonCollection"></param>
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