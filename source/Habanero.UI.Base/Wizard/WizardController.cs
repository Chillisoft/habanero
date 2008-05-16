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


using System.Collections.Generic;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Defines a simple implmentation of IWizardController. To customise the behaviour of a Wizard inherit from this class and override the applicable methods such as the constructor, GetNextStep(), GetPreviousStep().
    /// </summary>
    public class WizardController : IWizardController
    {
        private List<IWizardStep> _wizardSteps;
        private int _currentStep = -1;

        /// <summary>
        /// Initiliases the Wizard. When the Wizard is created there is no current step, the first call to GetNextStep() will move to the first step.
        /// </summary>
        public WizardController()
        {
            _wizardSteps = new List<IWizardStep>();
        }

        /// <summary>
        /// Adds a step to the Wizard.  These are added in order.  To add items out of order use the WizardSteps property.
        /// </summary>
        /// <param name="step">The IWizardStep to add.</param>
        public void AddStep(IWizardStep step)
        {
            _wizardSteps.Add(step);
        }

        /// <summary>
        /// Gets or Sets the list of Wizard Steps in the Wizard.
        /// </summary>
        protected List<IWizardStep> WizardSteps
        {
            get { return _wizardSteps; }
            set { _wizardSteps = value; }
        }


        /// <summary>
        /// Gets or Sets the Current Step of the Wizard.
        /// </summary>
        protected int CurrentStep
        {
            get { return _currentStep; }
            set { _currentStep = value; }
        }


        /// <summary>
        /// Returns the next step in the Wizard and sets the current step to that step.
        /// </summary>
        /// <exception cref="WizardStepException">Thrown if the current step is the last step.</exception>
        /// <returns>The next step.</returns>
        public virtual IWizardStep GetNextStep()
        {

            if (_currentStep < _wizardSteps.Count - 1)
            {
                return _wizardSteps[++_currentStep];
            }
            else
            {
                throw new WizardStepException("Invalid Wizard Step: " + (_currentStep + 1));
            }
        }


        /// <summary>
        /// Returns the Previous Step and sets the step pointer to that step.
        /// </summary>
        /// <exception cref="WizardStepException">Thrown if the current step is the first step.</exception>
        /// <returns>The previous step.</returns>
        public virtual IWizardStep GetPreviousStep()
        {
            if (_currentStep > 0)
            {
                return _wizardSteps[--_currentStep];
            }
            else
            {
                throw new WizardStepException("Invalid Wizard Step: " + (_currentStep - 1));
            }
        }


        /// <summary>
        /// Returns the First Step of the Wizard and sets the current step to that step.
        /// </summary>
        /// <returns>The first step.</returns>
        public virtual IWizardStep GetFirstStep()
        {
            return _wizardSteps[_currentStep = 0];
        }


        /// <summary>
        /// Checks if the current step is the last step.
        /// </summary>
        /// <returns>True if the current step is the last step.</returns>
        public virtual bool IsLastStep()
        {
            return (_currentStep == _wizardSteps.Count - 1);
        }


        /// <summary>
        /// Checks if the current Step is the first step.
        /// </summary>
        /// <returns>True if the current step is the first step.</returns>
        public virtual bool IsFirstStep()
        {
            return (_currentStep == 0);
        }


        /// <summary>
        /// Method that is to be run when the Wizard is finished. This method should do all persistance that is required.
        /// </summary>
        public virtual void Finish()
        {
            if (!IsLastStep()) throw new WizardStepException("Invalid call to Finish(), not at last step");
        }


        /// <summary>
        /// Checks if the Wizard can proceed to the next step. Calls through to the CanMoveOn method of the current IWizardStep.
        /// </summary>
        /// <param name="message">Describes why the Wizard cannot move on. Only applicable if CanMoveOn returns false.</param>
        /// <returns>True if moving to the next step is allowed.</returns>
        public virtual bool CanMoveOn(out string message)
        {
            return _wizardSteps[_currentStep].CanMoveOn(out message);
        }


        /// <summary>
        /// Returns the number of Steps in the Wizard.
        /// </summary>
        public virtual int StepCount
        {
            get { return _wizardSteps.Count; }
        }

        /// <summary>
        /// Returns the step that the Wizard is currently on.
        /// </summary>
        /// <returns></returns>
        public virtual IWizardStep GetCurrentStep()
        {
            if (_currentStep == -1)
            {
                return null;
            } else
            {
                return _wizardSteps[_currentStep];
            }
        }
    }
}
