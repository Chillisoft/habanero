using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Habanero.UI.Base
{
    public interface IButton:IControlChilli
    {
        /// <summary>
        /// Executes the click event on a button
        /// </summary>
        void PerformClick();

        /// <summary>
        /// Event handler for the click event
        /// </summary>
        /// <param name="b"></param>
        void NotifyDefault(bool b);
    }
}

