namespace Habanero.Base
{
    /// <summary>
    /// Provides a null progress indicator that does nothing with the inputs
    /// it receives
    /// </summary>
    public class NullProgressIndicator : IProgressIndicator
    {
        /// <summary>
        /// Constructor to initialise a new indicator
        /// </summary>
        public NullProgressIndicator()
        {
        }

        /// <summary>
        /// Does nothing, so the parameters can be set to null
        /// </summary>
        public void UpdateProgress(int amountComplete, int totalToComplete, string description)
        {
        }

        /// <summary>
        /// Completes the progress, in this case doing nothing
        /// </summary>
        public void Complete()
        {
        }
    }
}