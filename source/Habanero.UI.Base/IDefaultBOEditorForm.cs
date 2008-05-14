namespace Habanero.UI.Base
{
    public interface IDefaultBOEditorForm: IFormChilli
    {
        /// <summary>
        /// Returns the button control for the buttons in the form
        /// </summary>
        IButtonGroupControl Buttons
        {
            get;
        }
    }
}