namespace Habanero.UI.Base
{
    /// <summary>
    /// Creates OK/Cancel dialogs which contain OK and Cancel buttons, as well
    /// as control placed above the buttons, which the developer must provide.
    /// </summary>
    public interface IOKCancelDialogFactory
    {
        /// <summary>
        /// Creates a panel containing OK and Cancel buttons
        /// </summary>
        /// <param name="nestedControl">The control to place above the buttons</param>
        /// <returns>Returns the created panel</returns>
        IOKCancelPanel CreateOKCancelPanel(IControlHabanero nestedControl);

        /// <summary>
        /// Creates a form containing OK and Cancel buttons
        /// </summary>
        /// <param name="nestedControl">The control to place above the buttons</param>
        /// <param name="formTitle">The title shown on the form</param>
        /// <returns>Returns the created form</returns>
        IFormHabanero CreateOKCancelForm(IControlHabanero nestedControl, string formTitle);
    }

    /// <summary>
    /// Represents a panel that contains an OK and Cancel button
    /// </summary>
    public interface IOKCancelPanel: IPanel
    {
        /// <summary>
        /// Gets the OK button
        /// </summary>
        IButton OKButton { get; }

        /// <summary>
        /// Gets the Cancel button
        /// </summary>
        IButton CancelButton { get; }
    }
}