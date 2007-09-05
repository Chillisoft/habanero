using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Grid
{
    /// <summary>
    /// Provides arguments relating to a filter control event
    /// </summary>
    public class FilterControlEventArgs : EventArgs
    {
        private readonly Control _sendingControl;

        /// <summary>
        /// Constructor to initialise a set of arguments
        /// </summary>
        /// <param name="sendingControl">The sending control</param>
        public FilterControlEventArgs(Control sendingControl)
        {
            Permission.Check(this);
            _sendingControl = sendingControl;
        }

        /// <summary>
        /// Returns the sending control object
        /// </summary>
        public Control SendingControl { get { return _sendingControl; } }
    }
}