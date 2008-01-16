using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Maintains an application-wide store of UI-related
    /// settings
    /// </summary>
    public class GlobalUIRegistry
    {
        private static IUISettings _uiSettings;
        private static DateDisplaySettings _dateDisplaySettings;

        /// <summary>
        /// Gets and sets the store of general user interface settings
        /// </summary>
        public static IUISettings UISettings
        {
            get { return _uiSettings; }
            set { _uiSettings = value; }
        }

        /// <summary>
        /// Gets and sets the store of date display settings
        /// </summary>
        public static DateDisplaySettings DateDisplaySettings
        {
            get { return _dateDisplaySettings; }
            set { _dateDisplaySettings = value; }
        }
    }
}
