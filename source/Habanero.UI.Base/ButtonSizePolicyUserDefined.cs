namespace Habanero.UI.Base
{
    /// <summary>
    /// An implementation of <see cref="IButtonSizePolicy"/> that does no resizing, thus allowing you to specify your own button sizes for buttons on
    /// an <see cref="IButtonGroupControl"/>.
    /// </summary>
    public class ButtonSizePolicyUserDefined : IButtonSizePolicy
    {
        /// <summary>
        /// Recalculates the button sizes of the given collection of buttons.  This implementation does nothing to the buttons, allowing you to specify
        /// your own button sizes.
        /// </summary>
        /// <param name="buttonCollection"></param>
        public void RecalcButtonSizes(IControlCollection buttonCollection)
        {

        }
    }
}