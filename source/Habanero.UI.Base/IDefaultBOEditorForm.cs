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

        /// <summary>
        /// Pops the form up in a modal dialog.  If the BO is successfully edited and saved, returns true
        /// else returns false.
        /// </summary>
        /// <returns>True if the edit was a success, false if not</returns>
        bool ShowDialog();
    }
}