namespace Habanero.Base
{
    /// <summary>
    /// Stores key global settings for an application
    /// </summary>
    public class GlobalRegistry
    {
        private static ISettings _settings;
        private static IExceptionNotifier _exceptionNotifier;
        //private static ISynchronisationController _synchronisationController;
        private static string _applicationName;
        private static string _applicationVersion;
        private static int _databaseVersion;

        /// <summary>
        /// Gets and sets the application's settings storer, which stores
        /// database settings
        /// </summary>
        public static ISettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        /// <summary>
        /// Gets and sets the application's exception notifier, which
        /// provides a means to communicate exceptions to the user
        /// </summary>
        public static IExceptionNotifier UIExceptionNotifier
        {
            get { return _exceptionNotifier; }
            set { _exceptionNotifier = value; }
        }

        ///// <summary>
        ///// Gets and sets the application's synchronisation controller,
        ///// which implements a synchronisation strategy for the application
        ///// </summary>
        //public static ISynchronisationController SynchronisationController
        //{
        //    get
        //    {
        //        if (_synchronisationController == null)
        //        {
        //            _synchronisationController = new NullSynchronisationController();
        //        }
        //        return _synchronisationController;
        //    }
        //    set { _synchronisationController = value; }
        //}
        
        /// <summary>
        /// Gets and sets the application name
        /// </summary>
        public static string ApplicationName {
            get {
                return _applicationName;
            }
            set {
                _applicationName = value;
            }
        }

        /// <summary>
        /// Gets and sets the application version as a string
        /// </summary>
        public static string ApplicationVersion
        {
            get
            {
                return _applicationVersion;
            }
            set
            {
                _applicationVersion = value;
            }
        }        
        
        /// <summary>
        /// Gets and sets the database version as an integer
        /// </summary>
        public static int DatabaseVersion {
            get
            {
                return _databaseVersion;
            }
            set
            {
                _databaseVersion = value;
            }
        }
    }
}