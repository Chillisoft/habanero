namespace Habanero.Generic
{
    /// <summary>
    /// An interface to model a progress indicator that informs the user
    /// how much progress has been made in completing a task
    /// </summary>
    /// TODO ERIC - rename to IProgressIndicator
    public interface ProgressIndicator
    {
        /// <summary>
        /// Updates the indicator with progress information
        /// </summary>
        /// <param name="amountComplete">The amount complete already</param>
        /// <param name="totalToComplete">The total amount to be completed</param>
        /// <param name="description">A description</param>
        void UpdateProgress(int amountComplete, int totalToComplete, string description);
        
        /// <summary>
        /// Sets the indicator to completion status
        /// </summary>
        void Complete();
    }
}