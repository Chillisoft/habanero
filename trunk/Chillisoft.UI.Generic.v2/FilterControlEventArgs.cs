using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// Provides arguments relating to a filter control event
    /// </summary>
    public class FilterControlEventArgs : EventArgs
    {
        private readonly Control itsSendingControl;

        /// <summary>
        /// Constructor to initialise a set of arguments
        /// </summary>
        /// <param name="sendingControl">The sending control</param>
        public FilterControlEventArgs(Control sendingControl)
        {
            itsSendingControl = sendingControl;
        }

        /// <summary>
        /// Returns the sending control object
        /// </summary>
        public Control SendingControl { get { return itsSendingControl; } }
    }
}
