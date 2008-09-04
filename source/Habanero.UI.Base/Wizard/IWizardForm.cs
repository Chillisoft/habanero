using Habanero.UI.Base;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Represents a form containing a wizard control that guides users
    /// through a process step by step.
    /// This form simply wraps the WizardControl in a form and handles communication with the user.
    /// </summary>
    public interface IWizardForm : IFormChilli
    {
        /// <summary>
        /// Gets and sets the text to dispaly
        /// </summary>
        string WizardText { get; set; }

        /// <summary>
        /// Gets the WizardControl
        /// </summary>
        IWizardControl WizardControl { get; }
    }
}