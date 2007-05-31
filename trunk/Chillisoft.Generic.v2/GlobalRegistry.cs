namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// Stores key global settings for an application
    /// </summary>
    public class GlobalRegistry
    {
        private static ISettingsStorer itsSettingsStorer;
        private static IExceptionNotifier itsExceptionNotifier;
        private static SynchronisationController itsSynchronisationController;
        private static string itsApplicationName;
        private static string itsApplicationVersion;
        private static int itsDatabaseVersion;

        /// <summary>
        /// Gets and sets the application's settings storer, which stores
        /// database settings
        /// </summary>
        public static ISettingsStorer SettingsStorer
        {
            get { return itsSettingsStorer; }
            set { itsSettingsStorer = value; }
        }

        /// <summary>
        /// Gets and sets the application's exception notifier, which
        /// provides a means to communicate exceptions to the user
        /// </summary>
        public static IExceptionNotifier UIExceptionNotifier
        {
            get { return itsExceptionNotifier; }
            set { itsExceptionNotifier = value; }
        }

        /// <summary>
        /// Gets and sets the application's synchronisation controller,
        /// which implements a synchronisation strategy for the application
        /// </summary>
        public static SynchronisationController SynchronisationController
        {
            get
            {
                if (itsSynchronisationController == null)
                {
                    itsSynchronisationController = new NullSynchronisationController();
                }
                return itsSynchronisationController;
            }
            set { itsSynchronisationController = value; }
        }
        
        /// <summary>
        /// Gets and sets the application name
        /// </summary>
        public static string ApplicationName {
            get {
                return itsApplicationName;
            }
            set {
                itsApplicationName = value;
            }
        }

        /// <summary>
        /// Gets and sets the application version as a string
        /// </summary>
        public static string ApplicationVersion
        {
            get
            {
                return itsApplicationVersion;
            }
            set
            {
                itsApplicationVersion = value;
            }
        }        
        
        /// <summary>
        /// Gets and sets the database version as an integer
        /// </summary>
        public static int DatabaseVersion {
            get
            {
                return itsDatabaseVersion;
            }
            set
            {
                itsDatabaseVersion = value;
            }
        }
    }
}