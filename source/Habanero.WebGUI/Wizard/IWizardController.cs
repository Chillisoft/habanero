//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------


namespace Habanero.WebGUI.Wizard
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
