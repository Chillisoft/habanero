//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System.Windows.Forms;

namespace Habanero.UI.Win
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