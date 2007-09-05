using System;
using System.Collections;
using Habanero.UI.Base;
using Habanero.UI.Util;

namespace Habanero.UI.Util
{
    /// <summary>
    /// Provides a super-class for wizard helpers that lead the user 
    /// through a number of steps
    /// </summary>
    public abstract class Wizard
    {
        private int _currentStep = 1;
        private int _maxNumSteps = 100;
        private IList _wizardSteps;

        /// <summary>
        /// Constructor to initialise a new wizard
        /// </summary>
        public Wizard()
        {
            Permission.Check(this);
            _wizardSteps = new ArrayList(_maxNumSteps);
            for (int i = 1; i <= _maxNumSteps; i++)
            {
                _wizardSteps.Add(null);
            }
        }

        /// <summary>
        /// Returns the current step number. Note that the first step 
        /// is 1 and not 0.
        /// </summary>
        public int CurrentStepNumber
        {
            get { return _currentStep; }
        }

        /// <summary>
        /// Gets and sets the maximum number of steps permitted
        /// </summary>
        public int MaxNumberOfSteps
        {
            get { return _maxNumSteps; }
            set { _maxNumSteps = value; }
        }

        /// <summary>
        /// Moves the user along to the next step, as long as they have
        /// successfully completed the current step
        /// </summary>
        /// <param name="errMsg">The error message string to modify if
        /// an error prevents further progress in the wizard</param>
        /// <returns>Returns true if the advance is successful</returns>
        public bool NextStep(ref string errMsg)
        {
            if (this.ValidateCurrentStep(ref errMsg))
            {
                if (_currentStep < MaxNumberOfSteps)
                {
                    _currentStep++;
                    this.AfterNextStep();
                    if (this.GetCurrentWizardStep() != null)
                    {
                        this.GetCurrentWizardStep().Activate();
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Carries out additional preparation after the user has moved
        /// on to the next step
        /// </summary>
        protected abstract void AfterNextStep();

        /// <summary>
        /// Moves back to the previous step, cancelling any changes made
        /// </summary>
        public void previousStep()
        {
            if (_currentStep > 1)
            {
                this.CancelCurrentStepChanges();
                _currentStep--;
            }
        }

        /// <summary>
        /// Finishes the wizard and persists any changes made, as long
        /// as the user has successfully completed the current step
        /// </summary>
        /// <param name="errMsg">An error string to modify should any
        /// errors occur</param>
        /// <returns>Returns true if the finish is successful, or false if
        /// there is some reason why the wizard cannot finish at the moment</returns>
        /// <exception cref="FinishNotAvailableException">Exception
        /// thrown if the current step number is not equal to the 
        /// maximum step number</exception>
        public bool Finish(ref string errMsg)
        {
            if (!(CurrentStepNumber == MaxNumberOfSteps))
            {
                throw new FinishNotAvailableException("Finish is not available.");
            }
            if (this.ValidateCurrentStep(ref errMsg))
            {
                return this.Persist(ref errMsg);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Persists the changes to the database
        /// </summary>
        /// <param name="errMsg">An error message string to modify if any
        /// errors occur</param>
        /// <returns>Returns true if done successfully</returns>
        protected abstract bool Persist(ref string errMsg);

        /// <summary>
        /// Cancels changes made in the current step and restores original
        /// values
        /// </summary>
        private void CancelCurrentStepChanges()
        {
            if (_wizardSteps[this.CurrentStepNumber] != null)
            {
                GetCurrentWizardStep().CancelChanges();
            }
        }

        /// <summary>
        /// Checks if the user has completed the tasks required for this step
        /// </summary>
        /// <param name="errMsg">An error message string to modify if any
        /// errors occur</param>
        /// <returns>Returns true if valid, false if not</returns>
        private bool ValidateCurrentStep(ref string errMsg)
        {
            if (_wizardSteps[this.CurrentStepNumber] == null || GetCurrentWizardStep().Validate(ref errMsg))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a list of options currently available to the user, including
        /// "Previous", "Next" and "Finish" where applicable
        /// </summary>
        /// <returns>Returns an IList containing the options</returns>
        public IList AvailableOperations()
        {
            IList ops = new ArrayList();
            ops.Add("Next");
            if (this.CurrentStepNumber > 1)
            {
                ops.Add("Previous");
            }
            if (this.CurrentStepNumber == _maxNumSteps)
            {
                ops.Remove("Next");
                ops.Add("Finish");
            }
            return ops;
        }

        /// <summary>
        /// Sets the step object provided at the specified step number
        /// </summary>
        /// <param name="stepNum">The step number to set</param>
        /// <param name="iWiz">The object to set the step to</param>
        public void setWizardStep(int stepNum, IWizardStep iWiz)
        {
            _wizardSteps[stepNum] = iWiz;
        }

        /// <summary>
        /// Returns the step object at the step number specified
        /// </summary>
        /// <param name="stepNum">The step number in question</param>
        /// <returns>Returns the step object</returns>
        public IWizardStep getWizardStep(int stepNum)
        {
            return (IWizardStep) _wizardSteps[stepNum];
        }

        /// <summary>
        /// Returns the step object at the user's current step position
        /// </summary>
        /// <returns>Returns the step object</returns>
        public IWizardStep GetCurrentWizardStep()
        {
            return (IWizardStep) _wizardSteps[this.CurrentStepNumber];
        }

        /// <summary>
        /// Returns the step object at the previous step
        /// </summary>
        /// <returns>Returns the step object, or null if this is the 
        /// first step</returns>
        public IWizardStep GetPreviousWizardStep()
        {
            if (this.CurrentStepNumber > 1)
            {
                return (IWizardStep) _wizardSteps[this.CurrentStepNumber - 1];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a string in the format of:<br/>
        /// Step [current] of [maximum] - [step heading]<br/>
        /// eg. "Step 1 of 4 - Terms and Conditions"
        /// </summary>
        /// <returns>Returns a string</returns>
        public string GetHeading()
        {
            return
                "        Step " + this.CurrentStepNumber + " of " + this.MaxNumberOfSteps + " - " +
                this.GetCurrentWizardStep().GetHeading();
        }

        /// <summary>
        /// An exception thrown when the Finish() method is called but the
        /// current step is not equal to the maximum number of steps
        /// </summary>
        public class FinishNotAvailableException : Exception
        {
            public FinishNotAvailableException(string msg) : base(msg)
            {
            }
        }
    }
}