using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Habanero.UI.Base;
using Habanero.UI.Base.LayoutManagers;

namespace Habanero.UI.Win
{
    public class BorderLayoutManagerWin : BorderLayoutManager
    {
        private readonly IChilliControl[] _controls;
        private readonly bool[] _splitters;
        private Control _ctl;
        public BorderLayoutManagerWin(IChilliControl managedControl)
            : base(managedControl)
        {
            _controls = new IChilliControl[5];
            _splitters = new bool[5];
        }

        protected override void SetupDockOfControl(IChilliControl control, Position pos)
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

        public override IChilliControl AddControl(IChilliControl control, Position pos, bool includeSplitter)
        {
            SetupDockOfControl(control, pos);
            _controls[(int)pos] = control;
            _splitters[(int)pos] = includeSplitter;
            this.ManagedControl.Controls.Clear();
            for (int i = 0; i < _controls.Length; i++)
            {
                IChilliControl chilliControl = _controls[i];
                if (chilliControl != null)
                {
                    if (_splitters[i])
                    {
                        Splitter splt = new Splitter();
                        Color newBackColor =
                            Color.FromArgb(Math.Min(splt.BackColor.R - 30, 255), Math.Min(splt.BackColor.G - 30, 255),
                                           Math.Min(splt.BackColor.B - 30, 255));
                        splt.BackColor = newBackColor;

                        splt.Dock = ((Control)_controls[i]).Dock;
                        ManagedControl.Controls.Add(splt);
                    }
                    this.ManagedControl.Controls.Add(chilliControl);
                }
            }

            return control;
        }
    }
}
