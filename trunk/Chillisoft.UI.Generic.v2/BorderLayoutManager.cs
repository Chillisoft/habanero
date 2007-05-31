using System.Windows.Forms;

namespace Chillisoft.UI.Generic.v2
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
    public class BorderLayoutManager : LayoutManager
    {
        /// <summary>
        /// An enumeration to specify different layout positions that can
        /// be assigned
        /// </summary>
        public enum Position
        {
            Centre = 0,
            East = 1,
            West = 2,
            North = 3,
            South = 4
        }

        private ControlCollection controls;
        private bool[] splitters;

        /// <summary>
        /// Constructor to initalise a new layout manager
        /// </summary>
        /// <param name="managedControl">The control to manage</param>
        public BorderLayoutManager(Control managedControl) : base(managedControl)
        {
            controls = new ControlCollection();
            splitters = new bool[5];
            for (int i = 0; i < 5; i++)
            {
                controls.Add(null);
                splitters[i] = false;
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
            controls[(int) pos] = ctl;
            splitters[(int) pos] = includeSplitter;
            this.ManagedControl.Controls.Clear();
            for (int i = 0; i < 5; i++)
            {
                if (controls[i] != null)
                {
                    if (splitters[i])
                    {
                        Splitter splt = ControlFactory.CreateSplitter();
                        splt.Dock = controls[i].Dock;
                        ManagedControl.Controls.Add(splt);
                    }
                    ManagedControl.Controls.Add(controls[i]);
                }
            }
            return ctl;
        }
    }
}