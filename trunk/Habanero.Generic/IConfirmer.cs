namespace Habanero.Base
{
    /// <summary>
    /// An interface to model a tool to get confirmation from the user before
    /// proceeding with some action
    /// </summary>
    public interface IConfirmer
    {
        /// <summary>
        /// Gets confirmation from the user after providing them with an option
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <returns>Returns true if the user confirms the choice and false
        /// if they decline the offer</returns>
        bool Confirm(string message);
    }
}