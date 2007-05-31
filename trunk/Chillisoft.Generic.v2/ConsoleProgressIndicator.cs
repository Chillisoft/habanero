using System;

namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// Indicates to the user how much progress has been made in 
    /// completing a task, by adding text output to the console
    /// </summary>
    public class ConsoleProgressIndicator : ProgressIndicator
    {
        /// <summary>
        /// Constructor to initialise a new indicator
        /// </summary>
        public ConsoleProgressIndicator()
        {
        }

        /// <summary>
        /// Updates the indicator with progress information by adding a line
        /// of text output to the console
        /// </summary>
        /// <param name="amountComplete">The amount complete already</param>
        /// <param name="totalToComplete">The total amount to be completed</param>
        /// <param name="description">A description</param>
        public void UpdateProgress(int amountComplete, int totalToComplete, string description)
        {
            Console.WriteLine(amountComplete + " of " + totalToComplete + " steps complete. " + description);
        }

        /// <summary>
        /// Adds a line of text to the console with the message "Complete."
        /// </summary>
        public void Complete()
        {
            Console.WriteLine("Complete.");
        }
    }
}