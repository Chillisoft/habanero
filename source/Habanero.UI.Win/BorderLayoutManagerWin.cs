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
