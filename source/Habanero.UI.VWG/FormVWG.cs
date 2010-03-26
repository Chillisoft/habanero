// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.ComponentModel;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// Represents a window or dialog box that makes up an application's user interface
    /// </summary>
    public class FormVWG : Form, IFormHabanero
    {
        //private IControlCollection _controls;

        /// <summary>
        /// Gets or sets the anchoring style.
        /// </summary>
        /// <value></value>
        Base.AnchorStyles IControlHabanero.Anchor
        {
            get { return (Base.AnchorStyles)base.Anchor; }
            set { base.Anchor = (Gizmox.WebGUI.Forms.AnchorStyles)value; }
        }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlHabanero.Controls
        {
            get { return new ControlCollectionVWG(base.Controls); }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent
        /// control and determines how a control is resized with its parent
        /// </summary>
        Base.DockStyle IControlHabanero.Dock
        {
            get { return DockStyleVWG.GetDockStyle(base.Dock); }
            set { base.Dock = DockStyleVWG.GetDockStyle(value); }
        }

        /// <summary>
        /// Shows the form with the specified owner to the user.
        /// </summary>
        /// <param name="owner">Any object that implements System.Windows.Forms.IWin32Window and represents the top-level window that will own this form.</param>
        /// <exception cref="System.ArgumentException">The form specified in the owner parameter is the same as the form being shown.</exception>
        public void Show(IControlHabanero owner)
        {
            //TODO: Look into how the owner can be used with VWG because the form class does not have an equivalent show method
            base.Show();
        }

        /// <summary>
        /// Forces the form to invalidate its client area and
        /// immediately redraw itself and any child controls.
        /// Does nothing in the VWG environment.
        /// </summary>
        public void Refresh()
        {
            // do nothing
        }

        /// <summary>
        /// Gets or sets the current multiple document interface (MDI) parent form of this form
        /// </summary>
        IFormHabanero IFormHabanero.MdiParent
        {
            get { throw new NotImplementedException(); }
            set { this.MdiParent = (Form)value; }
        }

        /// <summary>
        /// Gets or sets the form's window state.  The default is Normal.
        /// </summary>
        Base.FormWindowState IFormHabanero.WindowState
        {
            get { return (Base.FormWindowState)base.WindowState; }
            set { base.WindowState = (Gizmox.WebGUI.Forms.FormWindowState)value; }
        }

        /// <summary>
        /// Shows the form as a modal dialog box with the currently active window set as its owner
        /// </summary>
        /// <returns>One of the DialogResult values</returns>
        Base.DialogResult IFormHabanero.ShowDialog()
        {
            return (Base.DialogResult)base.ShowDialog();
        }

        /// <summary>
        /// Gets or sets the form start position.
        /// </summary>
        /// <value></value>
        Base.FormStartPosition IFormHabanero.StartPosition
        {
            get { return (Base.FormStartPosition)base.StartPosition; }
            set { base.StartPosition = (Gizmox.WebGUI.Forms.FormStartPosition)value; }
        }

        /// <summary>
        /// Gets or sets the dialog result that indicates what action was
        /// taken to close the form
        /// </summary>
        public new Base.DialogResult DialogResult
        {
            get { return (Base.DialogResult)base.DialogResult; }
            set { base.DialogResult = (Gizmox.WebGUI.Forms.DialogResult)value; }
        }
//
//        /// <summary>
//        /// Gets or sets the MainMenu that is displayed in the form.
//        /// </summary>
//        public new IMainMenuHabanero Menu
//        {
//            get { return  base.Menu; }
//            set { base.Menu = (MainMenu) value; }
        //        }

        ///<summary>
        /// Gets or sets the border style of the form.
        ///</summary>
        ///<returns>A <see cref="Base.FormBorderStyle" /> that represents the style of border to display for the form. 
        /// The default is <see cref="Base.FormBorderStyle.Sizable" />.
        ///</returns>
        /// <exceptions>
        /// <see cref="InvalidEnumArgumentException"/>: The value specified is outside the range of valid values.
        /// </exceptions>
        Base.FormBorderStyle IFormHabanero.FormBorderStyle
        {
            get { return (Base.FormBorderStyle)base.FormBorderStyle; }
            set { base.FormBorderStyle = (Gizmox.WebGUI.Forms.FormBorderStyle)value; }
        }
    }
}
