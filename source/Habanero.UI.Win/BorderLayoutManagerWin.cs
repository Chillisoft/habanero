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
    public class BorderLayoutManagerWin : BorderLayoutManager
    {
        private readonly IControlChilli[] _controls;
        private readonly bool[] _splitters;
        private Control _ctl;
        public BorderLayoutManagerWin(IControlChilli managedControl, IControlFactory controlFactory)
            : base(managedControl, controlFactory)
        {
            _controls = new IControlChilli[5];
            _splitters = new bool[5];
        }

        protected override void SetupDockOfControl(IControlChilli control, Position pos)
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

        public override IControlChilli AddControl(IControlChilli control, Position pos, bool includeSplitter)
        {
            SetupDockOfControl(control, pos);
            _controls[(int)pos] = control;
            _splitters[(int)pos] = includeSplitter;
            this.ManagedControl.Controls.Clear();
            for (int i = 0; i < _controls.Length; i++)
            {
                IControlChilli chilliControl = _controls[i];
                if (chilliControl != null)
                {
                    if (_splitters[i])
                    {
                        ISplitter splt = _controlFactory.CreateSplitter();
                        Color newBackColor =
                            Color.FromArgb(Math.Min(splt.BackColor.R - 30, 255), Math.Min(splt.BackColor.G - 30, 255),
                                           Math.Min(splt.BackColor.B - 30, 255));
                        splt.BackColor = newBackColor;

                        //TODO: port  splt.Dock = ((Control)_controls[i]).Dock;
                        ManagedControl.Controls.Add(splt);
                    }
                    this.ManagedControl.Controls.Add(chilliControl);
                }
            }

            return control;
        }
    }
}
