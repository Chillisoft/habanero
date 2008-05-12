//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using System.Drawing;
using System.Windows.Forms;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Manages the layout of controls in a user interface by having
    /// component assigned a compass position.  For instance, having the
    /// "east" position assigned will result in the control being placed
    /// against the right border.<br/>
    /// Unlike similar tools available in the dotNet framework, this
    /// layout manager allows the controls to be added in any order and
    /// still be positioned correctly.
    /// </summary>
    public class BorderLayoutManager : UI.LayoutManager
    {
        /// <summary>
        /// An enumeration to specify different layout positions that can
        /// be assigned
        /// </summary>
        public enum Position
        {
            ///<summary>
            ///</summary>
            Centre = 0,
            ///<summary>
            ///</summary>
            East = 1,
            ///<summary>
            ///</summary>
            West = 2,
            ///<summary>
            ///</summary>
            North = 3,
            ///<summary>
            ///</summary>
            South = 4
        }

        private readonly UI.ControlCollection _controls;
        private readonly bool[] _splitters;

        /// <summary>
        /// Constructor to initalise a new layout manager
        /// </summary>
        /// <param name="managedControl">The control to manage (eg. use "this"
        /// if you create the manager inside a form class that you will be
        /// managing)</param>
        public BorderLayoutManager(Control managedControl) : base(managedControl)
        {
            _controls = new UI.ControlCollection();
            _splitters = new bool[5];
            for (int i = 0; i < 5; i++)
            {
                _controls.Add(null);
                _splitters[i] = false;
            }
        }

        /// <summary>
        /// This method has not yet been implemented
        /// </summary>
        /// TODO ERIC  - implement
        protected override void RefreshControlPositions()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a control to the layout at the position specified
        /// </summary>
        /// <param name="ctl">The control to add</param>
        /// <param name="pos">The position at which to add it in the layout.
        /// See the Position enumeration for more detail.</param>
        /// <returns>Returns the control once it has been added</returns>
        public Control AddControl(Control ctl, Position pos)
        {
            return AddControl(ctl, pos, false);
        }

        /// <summary>
        /// Adds a control as before, but includes the option of whether to
        /// include a splitter which enables the user to resize docked controls
        /// </summary>
        public Control  AddControl(Control ctl, Position pos, bool includeSplitter)
        {
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
            _controls[(int) pos] = ctl;
            _splitters[(int) pos] = includeSplitter;
            this.ManagedControl.Controls.Clear();
            for (int i = 0; i < 5; i++)
            {
                if (_controls[i] != null)
                {
                    if (_splitters[i])
                    {
                        Splitter splt = new Splitter();
                        Color newBackColor =
                            Color.FromArgb(Math.Min(splt.BackColor.R - 30, 255), Math.Min(splt.BackColor.G - 30, 255),
                                           Math.Min(splt.BackColor.B - 30, 255));
                        splt.BackColor = newBackColor;

                        splt.Dock = _controls[i].Dock;
                        ManagedControl.Controls.Add(splt);
                    }
                    ManagedControl.Controls.Add(_controls[i]);
                }
            }
            return ctl;
        }
    }
}