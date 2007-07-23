namespace Habanero.UI.Base
{
    /// <summary>
    /// An interface to model a version upgrade tool for an application.
    /// One common use of this interface is to provide a database update by
    /// using DBMigrator in the Upgrade() method.
    /// </summary>
    public interface IApplicationVersionUpgrader
    {
        /// <summary>
        /// Upgrades the application to the latest version
        /// </summary>
        void Upgrade();
    }
}