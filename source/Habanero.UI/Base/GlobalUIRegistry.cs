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

        /// <summary>
        /// Gets and sets the database version as an integer
        /// </summary>
        public static IUISettings UISettings
        {
            get { return _uiSettings; }
            set { _uiSettings = value; }
        }
    }
}
