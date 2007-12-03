using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Habanero.UI.Util
{
    ///<summary>
    /// Provides useful utilities for windows controls.
    ///</summary>
    public static class ControlsHelper
    {

        ///<summary>
        /// Executes the provided delegate in the specified control's thread.
        /// Use this method to avoid problems with corss thread calls.
        ///</summary>
        ///<param name="control">The control running on the thread to be used.</param>
        ///<param name="invoker">The delegate to execute on the control's thread.</param>
        public static void SafeGui(Control control, MethodInvoker invoker)
        {
            if (invoker == null) return;
            if (control != null)
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(invoker, null);
                }
                else
                {
                    invoker();
                }
            }
            else
            {
                invoker();
            }
        }

    }
}
