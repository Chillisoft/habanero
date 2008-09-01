using Habanero.UI.Base;

namespace Habanero.UI.Base
{
    public interface IWizardForm : IFormChilli
    {
        string WizardText { get; set; }

        /// <summary>
        /// Gets the WizardControl
        /// </summary>
        IWizardControl WizardControl { get; }
    }
}