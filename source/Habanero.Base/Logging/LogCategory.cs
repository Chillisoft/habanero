namespace Habanero.Base.Logging
{
    /// <summary>
    /// The Category of Log file that you want to log for.
    /// This is typically used by your <see cref="IHabaneroLogger"/> implementation
    /// to provide control for what is logged. Since logging is a time consuming procedure it is
    /// critical that the log is created only where required for released applicatione etc
    /// </summary>
    public enum LogCategory
    {
        /// <summary>
        /// Are you logging information specifically to be used when debugging an application
        /// </summary>
        Debug, 
        /// <summary>
        /// Are you logging an exception i.e. an Exception has been thrown and you are logging it.
        /// </summary>
        Exception, 
        /// <summary>
        /// Are you logging general info e.g. Resolved ...
        /// Bootstrapped application ...
        /// Starting ....
        /// </summary>
        Info, 
        /// <summary>
        /// Has an error occured whereby you do not want to specifically throw an exception and prevent the 
        /// user from continuing but you do want it logged so that your support team can look into it.
        /// </summary>
        Warn,
        /// <summary>
        /// There is a Fatal Exception that has occured. 
        ///   You want to log this. Probably want to log and close the app down or something like that.
        /// </summary>
        Fatal,
        /// <summary>
        /// There is an Error Exception that has occured.
        /// </summary>
        Error
    }
}