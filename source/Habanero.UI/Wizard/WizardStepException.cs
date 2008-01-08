using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.UI.Wizard
{
    /// <summary>
    /// An exception used in Wizards denoting that some error has occurred in navigating between wizard steps.
    /// </summary>
    public class WizardStepException : Exception
    {

        /// <summary>
        /// Initialises the WizardStepException
        /// </summary>
        /// <param name="message">the message of the exception</param>
        public WizardStepException(string message) :base(message)
        {
            
        }
    }
}
