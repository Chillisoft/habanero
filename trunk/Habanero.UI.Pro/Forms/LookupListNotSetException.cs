using System;


namespace Habanero.Ui.BoControls
{
    /// <summary>
    /// An exception to be thrown when the lookup list has not been set before
    /// using a control that requires it
    /// </summary>
    public class LookupListNotSetException : Exception
    {
        /// <summary>
        /// Constructor to initialise the exception with a standard message
        /// </summary>
        public LookupListNotSetException()
            : base("You must set the lookup list before using a control that requires it.")
        {
        }

        /// <summary>
        /// Constructor as before, except that a personalised message can be
        /// specified
        /// </summary>
        /// <param name="message">The message to display</param>
        public LookupListNotSetException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor as before, except that a personalised message and
        /// inner exception can be specified
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="inner">The inner exception</param>
        public LookupListNotSetException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}