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
using System.Collections.Generic;
using System.Text;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;
using DockStyle=Gizmox.WebGUI.Forms.DockStyle;

namespace Habanero.UI.WebGUI
{
    /// <summary>
    /// Manages the layout of controls in a user interface by having a
    /// component assigned a compass position.  For instance, having the
    /// "east" position assigned will result in the control being placed
    /// against the right border.
    /// </summary>
    public class BorderLayoutManagerGiz : BorderLayoutManager
    {
        private readonly IControlChilli[] _controls;
        private readonly bool[] _splitters;

        public BorderLayoutManagerGiz(IControlChilli managedControl, IControlFactory controlFactory)
            : base(managedControl, controlFactory)
        {
            _controls = new IControlChilli[5];
            _splitters = new bool[5];
        }

        /// <summary>
        /// Sets how the specified control is docked within its parent
        /// </summary>
        protected override void SetupDockOfControl(IControlChilli control, Position pos)
        {
            Control ctl = (Control) control;
            switch (pos)
            {
                case Position.Centre:
                    ctl.Dock = DockStyle.Fill;
                    break;
                case Position.North:
                    ctl.Dock = DockStyle.Top;
                    break;
                case Position.South:
                    ctl.Dock = DockStyle.Bottom;
                    break;
                case Position.East:
                    ctl.Dock = DockStyle.Right;
                    break;
                case Position.West:
                    ctl.Dock = DockStyle.Left;
                    break;
            }
        }

        /// <summary>
        /// Add a control to the layout
        /// </summary>
        /// <param name="control">The control to add</param>
        /// /// <param name="pos">The position at which to add the control</param>
        /// <param name="includeSplitter">True to include a splitter between the controls</param>
        /// <returns>Returns the control added</returns>
        public override IControlChilli AddControl(IControlChilli control, Position pos, bool includeSplitter)
        {
            SetupDockOfControl(control, pos);
            _controls[(int) pos] = control;
            this.ManagedControl.Controls.Clear();
            foreach (IControlChilli chilliControl in _controls)
            {
                if (chilliControl != null)
                {
                    this.ManagedControl.Controls.Add(chilliControl);
                }
            }
            return control;
        }
    }
}