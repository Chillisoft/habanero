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
using Habanero.Base;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Provides a message box giving the user the choice of proceeding with
    /// an option (such as deleting an object) or cancelling
    /// </summary>
    public class OKCancelConfirmer : IConfirmer
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public OKCancelConfirmer()
        {
        }
        /// <summary>
        /// Gets confirmation from the user as to whether they would like to
        /// proceed and accept the choice given to them.  Displays a message box
        /// with "OK" and "Cancel" buttons.
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <returns>Returns true if the user accepts the offer by pressing
        /// "OK" and false if they decline by pressing "Cancel"</returns>
        public bool Confirm(string message)
        {
            return
                (MessageBox.Show(message, "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
                                 MessageBoxDefaultButton.Button1) == DialogResult.OK);
        }
    }
}