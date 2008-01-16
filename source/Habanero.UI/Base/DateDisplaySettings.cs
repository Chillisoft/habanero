using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Stores date display settings that define how dates should
    /// be displayed to users in various user interfaces
    /// </summary>
    public class DateDisplaySettings
    {
        private string _gridDateFormat;

        /// <summary>
        /// Gets and sets the .Net style date format string that
        /// determines how a date is displayed in a grid.
        /// Set this value to null to use the short
        /// date format of the underlying user's environment.
        /// The format for this string is the same as that of
        /// DateTime.ToString(), including shortcuts such as
        /// "d" which use the short date format of the culture
        /// on the user's machine.
        /// </summary>
        public string GridDateFormat
        {
            get { return _gridDateFormat; }
            set { _gridDateFormat = value; }
        }
    }
}
