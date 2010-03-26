// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
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
        /// <summary>
        /// The <see cref="IControlFactory"/> used to create any controls required for this layout manager
        /// </summary>
        protected readonly IControlFactory _controlFactory;
        private int _borderSize = DefaultBorderSize;
        private int _gapSize = DefaultGapSize;
        /// <summary>
        /// The default border size used in all layout managers. The Border size is the 
        /// Gap between the Edge of the parent control and the placement of the control in it.
        /// </summary>
        public const int DefaultBorderSize = 5;
        /// <summary>
        /// The default gap size is the gap between the various controls placed on the parent control
        /// </summary>
        public const int DefaultGapSize = 2;

        /// <summary>
        /// Constructor to initialise a new layout manager
        /// </summary>
        /// <param name="managedControl">The control to manage</param>
        /// <param name="controlFactory">control factory used to create any child controls</param>
        protected LayoutManager(IControlHabanero managedControl, IControlFactory controlFactory)
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
        /// Updates the layout and appearance of the managed controls
        /// </summary>
        public void Refresh()
        {
            RefreshControlPositions();
        }

        /// <summary>
        /// Add a control to the layout
        /// </summary>
        /// <param name="control">The control to add</param>
        /// <returns>Returns the control added</returns>
        public abstract IControlHabanero AddControl(IControlHabanero control);

    }

    /// <summary>
    /// Thrown when an error occurs due to laying out or refreshing controls
    /// </summary>
    public class LayoutManagerException : HabaneroDeveloperException
    {
        ///<summary>
        /// Constructor for <see cref="LayoutManager"/>
        ///</summary>
        ///<param name="message"></param>
        public LayoutManagerException(string message)
            : base(message, "")
        {
        }

        ///<summary>
        /// Constructor for <see cref="LayoutManager"/>
        ///</summary>
        ///<param name="info"></param>
        ///<param name="context"></param>
        public LayoutManagerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        ///<summary>
        /// Constructor for <see cref="LayoutManager"/>
        ///</summary>
        ///<param name="message"></param>
        ///<param name="inner"></param>
        public LayoutManagerException(string message, Exception inner)
            : base(message, "", inner)
        {
        }

        ///<summary>
        /// Constructor for <see cref="LayoutManager"/>
        ///</summary>
        public LayoutManagerException()
        {
        }
    }
}