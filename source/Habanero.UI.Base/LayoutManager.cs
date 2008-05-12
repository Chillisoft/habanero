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

namespace Habanero.UI.Base
{
    /// <summary>
    /// A super-class for layout managers that dictate how and where controls
    /// are placed in a designated user interface
    /// </summary>
    public abstract class LayoutManager : MarshalByRefObject
    {
        private readonly IChilliControl _managedControl;
        private int _borderSize = 5;
        private int _gapSize = 2;

        /// <summary>
        /// Constructor to initialise a new layout manager
        /// </summary>
        /// <param name="managedControl">The control to manage</param>
        public LayoutManager(IChilliControl managedControl)
        {
            _managedControl = managedControl;
            _managedControl.Resize += this.ManagedControlResizeHandler;
        }

        /// <summary>
        /// Returns the managed control
        /// </summary>
        public IChilliControl ManagedControl
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

        public abstract IChilliControl AddControl(IChilliControl label);

        public abstract void AddGlue();
    }
}