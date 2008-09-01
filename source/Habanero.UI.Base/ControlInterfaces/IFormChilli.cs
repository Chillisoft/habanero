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

using System;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Specifies identifiers to indicate the return value of a dialog box
    /// </summary>
    public enum DialogResult
    {
        /// <summary>
        /// Nothing is returned from the dialog box. This means that the modal dialog continues running.
        /// </summary>
        None = 0,
        /// <summary>
        /// The dialog box return value is OK (usually sent from a button labeled OK)
        /// </summary>
        OK = 1,
        /// <summary>
        /// The dialog box return value is Cancel (usually sent from a button labeled Cancel)
        /// </summary>
        Cancel = 2,
        /// <summary>
        /// The dialog box return value is Abort (usually sent from a button labeled Abort)
        /// </summary>
        Abort = 3,
        /// <summary>
        /// The dialog box return value is Retry (usually sent from a button labeled Retry)
        /// </summary>
        Retry = 4,
        /// <summary>
        /// The dialog box return value is Ignore (usually sent from a button labeled Ignore)
        /// </summary>
        Ignore = 5,
        /// <summary>
        /// The dialog box return value is Yes (usually sent from a button labeled Yes)
        /// </summary>
        Yes = 6,
        /// <summary>
        /// The dialog box return value is No (usually sent from a button labeled No)
        /// </summary>
        No = 7
    }

    public enum FormStartPosition
    {
        /// <summary>
        /// The form is centered within the bounds of its parent form.
        /// </summary>
        CenterParent = 4,
        /// <summary>
        /// The form is centered on the current display, and has the dimensions specified in the form's size.
        /// </summary>
        CenterScreen = 1,
        /// <summary>
        /// The position of the form is determined by the System.Windows.Forms.Control.Location property.
        /// </summary>
        Manual = 0,
        /// <summary>
        /// The form is positioned at the Windows default location and has the bounds determined by Windows default.
        /// </summary>
        WindowsDefaultBounds = 3,
        /// <summary>
        /// The form is positioned at the Windows default location and has the dimensions specified in the form's size.
        /// </summary>
        WindowsDefaultLocation = 2
    }

    /// <summary>
    /// Represents a window or dialog box that makes up an application's user interface
    /// </summary>
    public interface IFormChilli : IControlChilli
    {
        /// <summary>
        /// Shows the form to the user
        /// </summary>
        void Show();

        /// <summary>
        /// Shows the form with the specified owner to the user.
        /// </summary>
        /// <param name="owner">Any object that implements System.Windows.Forms.IWin32Window and represents the top-level window that will own this form.</param>
        /// <exception cref="System.ArgumentException">The form specified in the owner parameter is the same as the form being shown.</exception>
        void Show(IControlChilli owner);

        /// <summary>
        /// Forces the form to invalidate its client area and
        /// immediately redraw itself and any child controls
        /// </summary>
        void Refresh();

        /// <summary>
        /// Sets input focus to the Form
        /// </summary>
        /// <returns>true if the input focus request was successful; otherwise, false</returns>
        bool Focus();

        /// <summary>
        /// Forces the form to apply layout logic to all its child controls
        /// </summary>
        void PerformLayout();

        /// <summary>
        /// Closes the form
        /// </summary>
        void Close();

        /// <summary>
        /// Gets or sets the current multiple document interface (MDI) parent form of this form
        /// </summary>
        IFormChilli MdiParent { get; set; }

        /// <summary>
        /// Occurs after the form is closed
        /// </summary>
        event EventHandler Closed;

        /// <summary>
        /// Shows the form as a modal dialog box with the currently active window set as its owner
        /// </summary>
        /// <returns>One of the DialogResult values</returns>
        DialogResult ShowDialog();

        /// <summary>
        /// Gets or sets the form start position.
        /// </summary>
        /// <value></value>
        FormStartPosition StartPosition { get; set; }
    }

}