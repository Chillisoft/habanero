namespace Habanero.UI.Base
{
    /// <summary>
    /// Specifies an object that calculates button sizes.  This is used by the <see cref="IButtonGroupControl"/> when a button is added to it.  You can control
    /// how the buttons are laid out on the <see cref="IButtonGroupControl"/> by creating a class that implements <see cref="IButtonSizePolicy"/>
    /// and setting the <see cref="IButtonGroupControl.ButtonSizePolicy"/> property on your <see cref="IButtonGroupControl"/>.
    /// </summary>
    public interface IButtonSizePolicy
    {
        /// <summary>
        /// Recalculates the button sizes of the given collection of buttons.
        /// </summary>
        /// <param name="buttonCollection"></param>
        void RecalcButtonSizes(IControlCollection buttonCollection);
    }
}