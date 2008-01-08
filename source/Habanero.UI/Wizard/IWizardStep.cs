using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.UI.Wizard
{
    /// <summary>
    /// Defines the interface for a WizardStep.
    /// </summary>
    public interface IWizardStep
    {
        /// <summary>
        /// Initialises the step. Run when the step is reached.
        /// </summary>
        void InitialiseStep();

        /// <summary>
        /// Verifies whether this step can be passed.
        /// </summary>
        /// <param name="message">Error message should moving on be disallowed. This message will be displayed to the user by the WizardControl.</param>
        /// <returns></returns>
        bool CanMoveOn(out String message);
    }
}
