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
using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    /// <summary>
    /// Represents a label
    /// </summary>
    public class LabelGiz : Label, ILabel
    {
        private const int WIDTH_FACTOR = 6;

        public LabelGiz(string labelText)
        {
            base.Text = labelText;
        }

        public LabelGiz()
        {
        }

        /// <summary>
        /// Gets the preferred width of the control
        /// </summary>
        public int PreferredWidth
        {
            get { return this.Text.Length * WIDTH_FACTOR; }
        }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent
        /// control and determines how a control is resized with its parent
        /// </summary>
        Base.DockStyle IControlChilli.Dock
        {
            get { return (Base.DockStyle)base.Dock; }
            set { base.Dock = (Gizmox.WebGUI.Forms.DockStyle)value; }
        }
    }
}