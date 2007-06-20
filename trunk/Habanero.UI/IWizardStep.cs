using System;
using System.Collections;
using System.Windows.Forms;

namespace Habanero.Ui.Generic
{
    public delegate void WizardStepEnabledUpdatedHandler(Object sender, WizardStepEventArgs e);

    /// <summary>
    /// An interface to model a step in a wizard helper
    /// </summary>
    public interface IWizardStep
    {
        /// <summary>
        /// The event where a wizard step has been enabled
        /// </summary>
        event WizardStepEnabledUpdatedHandler WizardStepEnabledUpdated;

        /// <summary>
        /// Provides a validation
        /// </summary>
        /// <param name="errMsg">The error message to be updated if needed</param>
        /// <returns>Returns true if valid, false if not</returns>
        bool Validate(ref string errMsg);

        /// <summary>
        /// Removes the changes made and restores the original values
        /// </summary>
        void CancelChanges();

        /// <summary>
        /// Returns the panel object
        /// </summary>
        /// <returns>Returns the panel object</returns>
        Panel GetPanel();

        /// <summary>
        /// Returns the heading
        /// </summary>
        /// <returns>Returns a string</returns>
        string GetHeading();

        /// <summary>
        /// Returns an IList of the objects to persist to the database
        /// </summary>
        /// <returns>Returns an IList object</returns>
        IList GetObjectsToPersist();

        /// <summary>
        /// Activates
        /// </summary>
        /// TODO ERIC - activates what?
        void Activate();

        /// <summary>
        /// Indicates whether the step has been enabled
        /// </summary>
        /// <returns>Returns true if enabled, false if not</returns>
        bool Enabled();
    }
}