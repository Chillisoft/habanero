using System;
using System.Windows.Forms;

namespace Habanero.Ui.Generic
{
    /// <summary>
    /// A super-class for layout managers that dictate how and where controls
    /// are placed in a designated user interface
    /// </summary>
    public abstract class LayoutManager : MarshalByRefObject
    {
        private Control _managedControl;
        private int _borderSize = 5;
        private int _gapSize = 2;

        /// <summary>
        /// Constructor to initialise a new layout manager
        /// </summary>
        /// <param name="managedControl">The control to manage</param>
        public LayoutManager(Control managedControl)
        {
            _managedControl = managedControl;
            _managedControl.Resize += new EventHandler(this.ManagedControlResizeHandler);
        }

        /// <summary>
        /// Returns the managed control
        /// </summary>
        public Control ManagedControl
        {
            get { return _managedControl; }
        }

        /// <summary>
        /// A handler to deal with the event where a control has been
        /// resized by the user
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void ManagedControlResizeHandler(Object sender, EventArgs e)
        {
            RefreshControlPositions();
        }

        /// <summary>
        /// Gets and sets the attribute controlling the border size
        /// </summary>
        public int BorderSize
        {
            get { return _borderSize; }
            set
            {
                _borderSize = value;
                RefreshControlPositions();
            }
        }

        /// <summary>
        /// Gets and sets the attribute controlling the gap size
        /// </summary>
        public int GapSize
        {
            get { return _gapSize; }
            set
            {
                _gapSize = value;
                RefreshControlPositions();
            }
        }

        /// <summary>
        /// Updates the layout and appearance of the managed controls
        /// </summary>
        protected abstract void RefreshControlPositions();
    }
}