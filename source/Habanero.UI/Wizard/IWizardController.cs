using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Habanero.UI.Wizard
{
    /// <summary>
    /// Defines the interface for a Wizard Controller that is used by the WizardControl.  This controls the behaviour of the wizard.
    /// </summary>
    public interface IWizardController
    {
        /// <summary>
        /// Returns the next step in the Wizard and sets the current step to that step.
        /// </summary>
        /// <exception cref="WizardStepException">Thrown if the current step is the last step.</exception>
        /// <returns>The next step.</returns>
        IWizardStep GetNextStep();

        /// <summary>
        /// Returns the Previous Step and sets the step pointer to that step.
        /// </summary>
        /// <exception cref="WizardStepException">Thrown if the current step is the first step.</exception>
        /// <returns>The previous step.</returns>
        IWizardStep GetPreviousStep();

        /// <summary>
        /// Returns the First Step of the Wizard and sets the current step to that step.
        /// </summary>
        /// <returns>The first step.</returns>
        IWizardStep GetFirstStep();

        /// <summary>
        /// Checks if the current step is the last step.
        /// </summary>
        /// <returns>True if the current step is the last step.</returns>
        bool IsLastStep();

        /// <summary>
        /// Checks if the current Step is the first step.
        /// </summary>
        /// <returns>True if the current step is the first step.</returns>
        bool IsFirstStep();

        /// <summary>
        /// Method that is to be run when the Wizard is finished. This method should do all persistance that is required.
        /// </summary>
        void Finish();

        /// <summary>
        /// Checks if the Wizard can proceed to the next step. Calls through to the CanMoveOn method of the current IWizardStep.
        /// </summary>
        /// <param name="message">Describes why the Wizard cannot move on. Only applicable if CanMoveOn returns false.</param>
        /// <returns>True if moving to the next step is allowed.</returns>
        bool CanMoveOn(out string message);

        /// <summary>
        /// Returns the number of Steps in the Wizard.
        /// </summary>
        int StepCount { get; }

        /// <summary>
        /// Returns the step that the Wizard is currently on.
        /// </summary>
        /// <returns></returns>
        IWizardStep GetCurrentStep();
    }
}
