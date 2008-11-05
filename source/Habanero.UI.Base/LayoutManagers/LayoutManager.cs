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
using System.Runtime.Serialization;
using Habanero.Base.Exceptions;

namespace Habanero.UI.Base
{
    /// <summary>
    /// A super-class for layout managers that dictate how and where controls
    /// are placed in user interface
    /// </summary>
    public abstract class LayoutManager : MarshalByRefObject
    {
        private IControlHabanero _managedControl;
        protected readonly IControlFactory _controlFactory;
        private int _borderSize = DefaultBorderSize;
        private int _gapSize = DefaultGapSize;
        public const int DefaultBorderSize = 5;
        public const int DefaultGapSize = 2;

        /// <summary>
        /// Constructor to initialise a new layout manager
        /// </summary>
        /// <param name="managedControl">The control to manage</param>
        /// <param name="controlFactory">control factory used to create any child controls</param>
        public LayoutManager(IControlHabanero managedControl, IControlFactory controlFactory)
        {
            if (managedControl == null)
            {
                throw new LayoutManagerException("You cannot initialise the layout manager with a null control");
            }
            if (managedControl.Controls == null)
            {
                throw new LayoutManagerException("You cannot initialise the layout manager with a control that has a null controls collection");
            }
            if (managedControl.Controls.Count > 0)
            {
                throw new LayoutManagerException("You cannot initialise the layout manager with a control that already contains controls");
            }
            _controlFactory = controlFactory;
            SetManagedControl(managedControl);
        }

        private void SetManagedControl(IControlHabanero managedControl)
        {
            _managedControl = managedControl;
            _managedControl.Resize += this.ManagedControlResizeHandler;
        }

        /// <summary>
        /// Gets and sets the managed control
        /// </summary>
        public IControlHabanero ManagedControl
        {
            get { return _managedControl; }
            set { SetManagedControl(value); }
        }

        /// <summary>
        /// A handler to deal with the event where a control has been
        /// resized by the user
        /// </summary>
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

        /// <summary>
        /// Add a control to the layout
        /// </summary>
        /// <param name="label">The control to add</param>
        /// <returns>Returns the control added</returns>
        public abstract IControlHabanero AddControl(IControlHabanero label);
    }

    /// <summary>
    /// Thrown when an error occurs due to laying out or refreshing controls
    /// </summary>
    public class LayoutManagerException : HabaneroDeveloperException
    {
        public LayoutManagerException(string message)
            : base(message, "")
        {
        }

        public LayoutManagerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public LayoutManagerException(string message, Exception inner)
            : base(message, "", inner)
        {
        }

        public LayoutManagerException()
        {
        }
    }
}