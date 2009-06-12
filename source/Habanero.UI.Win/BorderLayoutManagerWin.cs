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
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Habanero.UI.Base;
using DockStyle=System.Windows.Forms.DockStyle;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Manages the layout of controls in a user interface by having a
    /// component assigned a compass position.  For instance, having the
    /// "east" position assigned will result in the control being placed
    /// against the right border.
    /// </summary>
    public class BorderLayoutManagerWin : BorderLayoutManager
    {
        private readonly IControlHabanero[] _controls;
        private readonly bool[] _splitters;
        private Control _ctl;
        ///<summary>
        /// Constructor for <see cref="BorderLayoutManagerWin"/>
        ///</summary>
        ///<param name="managedControl"></param>
        ///<param name="controlFactory"></param>
        public BorderLayoutManagerWin(IControlHabanero managedControl, IControlFactory controlFactory)
            : base(managedControl, controlFactory)
        {
            _controls = new IControlHabanero[5];
            _splitters = new bool[5];
        }

        /// <summary>
        /// Sets how the specified control is docked within its parent
        /// </summary>
        protected override void SetupDockOfControl(IControlHabanero control, Position pos)
        {
            _ctl = (Control)control;
            switch (pos)
            {
                case Position.Centre:
                    _ctl.Dock = DockStyle.Fill;
                    break;
                case Position.North:
                    _ctl.Dock = DockStyle.Top;
                    break;
                case Position.South:
                    _ctl.Dock = DockStyle.Bottom;
                    break;
                case Position.East:
                    _ctl.Dock = DockStyle.Right;
                    break;
                case Position.West:
                    _ctl.Dock = DockStyle.Left;
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
        public override IControlHabanero AddControl(IControlHabanero control, Position pos, bool includeSplitter)
        {
            SetupDockOfControl(control, pos);
            _controls[(int)pos] = control;
            _splitters[(int)pos] = includeSplitter;
            this.ManagedControl.Controls.Clear();
            for (int i = 0; i < _controls.Length; i++)
            {
                IControlHabanero habaneroControl = _controls[i];
                if (habaneroControl != null)
                {
                    if (_splitters[i])
                    {
                        ISplitter splt = _controlFactory.CreateSplitter();
                        Color newBackColor =
                            Color.FromArgb(Math.Min(splt.BackColor.R - 30, 255), Math.Min(splt.BackColor.G - 30, 255),
                                           Math.Min(splt.BackColor.B - 30, 255));
                        splt.BackColor = newBackColor;

                        if (_controls[i].Dock != Base.DockStyle.Fill)
                            splt.Dock = _controls[i].Dock;
                        ManagedControl.Controls.Add(splt);
                    }
                    this.ManagedControl.Controls.Add(habaneroControl);
                }
            }

            return control;
        }
    }
}
