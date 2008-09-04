using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    /// <summary>
    /// Provides a form that displays information about the application
    /// </summary>
    public class HelpAboutBoxGiz : FormGiz
    {
        /// <summary>
        /// Constructor to initialise a new About form with the given
        /// information
        /// </summary>
        /// <param name="programName">The program name</param>
        /// <param name="producedForName">Who the program is produced for</param>
        /// <param name="producedByName">Who produced the program</param>
        /// <param name="versionNumber">The version number</param>
        public HelpAboutBoxGiz(string programName, string producedForName, string producedByName, string versionNumber)
        {
            new HelpAboutBoxManager(new ControlFactoryGizmox(), this, programName, producedForName, producedByName, versionNumber);

        }
    }
}
