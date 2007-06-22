namespace Habanero.Ui.Application
{
    /// <summary>
    /// An interface to model a version upgrade tool for an application
    /// </summary>
    public interface ApplicationVersionUpgrader
    {
        /// <summary>
        /// Upgrades the application to the latest version
        /// </summary>
        void Upgrade();
    }
}