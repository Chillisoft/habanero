// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Habanero.Base.Exceptions;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Controls the behaviour of a wizard, which guides users through a process one
    /// step at a time. Implementsts the <see cref="IWizardController"/>
    /// </summary>
    public class WizardController : IWizardController
    {
        private List<IWizardStep> _wizardSteps;
        private int _currentStep = -1;
        /// <summary>
        /// Event Handler for the Wizard being finished. Allows you to do special handling when this occurs.
        /// </summary>
        public event EventHandler WizardFinished;

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
        public int CurrentStep
        {
            get { return _currentStep; }
            protected set { _currentStep = value; }
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
            throw new WizardStepException("Invalid Wizard Step: " + (_currentStep + 1));
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
            throw new WizardStepException("Invalid Wizard Step: " + (_currentStep - 1));
        }


        /// <summary>
        /// Returns the First Step of the Wizard and sets the current step to that step.
        /// </summary>
        /// <returns>The first step.</returns>
        public virtual IWizardStep GetFirstStep()
        {
            if (_wizardSteps.Count <= 0)
            {
                throw new HabaneroApplicationException(
                    "There was an Error when trying to access the first step of the wizard Controller" 
                    + this.GetType() + ". The wizard controller has not been set up with steps");
            }
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
        /// Method that is to be run when the Wizard is finished. This method should be overridden to do all persistance that is required.
		/// This raises the WizardFinished event which allows you to close forms or do anything else required.
        /// </summary>
        public virtual void Finish()
        {
            if (!IsLastStep()) throw new WizardStepException("Invalid call to Finish(), not at last step");
            FireWizardFinishedEvent();
        }

        private void FireWizardFinishedEvent()
        {
            if (WizardFinished != null)
            {
                WizardFinished(this, new EventArgs());
            }
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
            return _currentStep == -1 ? null : _wizardSteps[_currentStep];
        }

        /// <summary>
        /// This provides a method which is called when the wizard is cancelled. The wizard controller can 
        /// undo any changes that have occured up until that point so as to ensure that the objects are returned
        /// to their original state.
        /// </summary>
        public virtual void CancelWizard()
        {
            foreach (IWizardStep step in _wizardSteps)
            {
                step.CancelStep();
            }
        }
    }
}
