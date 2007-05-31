using System;

namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// Provides arguments relating to the enabling of a wizard step
    /// </summary>
    public class WizardStepEventArgs : EventArgs
    {
        private bool mEnabled;

        /// <summary>
        /// Constructor to initialise a set of arguments
        /// </summary>
        /// <param name="enabled">Whether the wizard step is enabled</param>
        public WizardStepEventArgs(bool enabled) : base()
        {
            mEnabled = enabled;
        }

        /// <summary>
        /// Indicates whether the wizard step is enabled
        /// </summary>
        public bool Enabled
        {
            get { return mEnabled; }
        }
    }
}