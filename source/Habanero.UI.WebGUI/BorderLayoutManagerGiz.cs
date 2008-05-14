using System;
using System.Collections.Generic;
using System.Text;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;
using Habanero.UI.Base.LayoutManagers;

namespace Habanero.UI.WebGUI
{
    public class BorderLayoutManagerGiz : BorderLayoutManager
    {
        private readonly IChilliControl[] _controls;
        private readonly bool[] _splitters;

        public BorderLayoutManagerGiz(IChilliControl managedControl) : base(managedControl)
        {
            _controls = new IChilliControl[5];
            _splitters = new bool[5];
        }

        protected override void SetupDockOfControl(IChilliControl control, Position pos)
        {
            Control ctl = (Control)control;
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

        public override IChilliControl AddControl(IChilliControl control, Position pos, bool includeSplitter)
        {
            SetupDockOfControl(control, pos);
            _controls[(int)pos] = control;
            this.ManagedControl.Controls.Clear();
            foreach (IChilliControl chilliControl in _controls)
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
