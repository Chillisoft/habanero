using System;

namespace Habanero.Ui.Generic
{
    /// <summary>
    /// Provides arguments relating to the enabling of a wizard step
    /// </summary>
    public class WizardStepEventArgs : EventArgs
    {
        private bool _enabled;

        /// <summary>
        /// Constructor to initialise a set of arguments
        /// </summary>
        /// <param name="enabled">Whether the wizard step is enabled</param>
        public WizardStepEventArgs(bool enabled) : base()
        {
            _enabled = enabled;
        }

        /// <summary>
        /// Indicates whether the wizard step is enabled
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
        }
    }
}