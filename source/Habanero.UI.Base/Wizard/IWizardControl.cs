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

namespace Habanero.UI.Base
{
    /// <summary>
    /// Stores common constants used by wizard controls
    /// </summary>
    public abstract class WizardControl
    {
        public const int PADDING = 3;
    }

    /// <summary>
    /// Provides the controls for a wizard, which guides users through a process one
    /// step at a time.
    /// </summary>
    public interface IWizardControl : IControlChilli
    {
        /// <summary>
        /// Raised when the wizard is complete to notify the containing control or controlling object.
        /// </summary>
        event EventHandler Finished;

        /// <summary>
        /// Raised when a message is communicated so the controlling object can display or log the message.
        /// </summary>
        event Action<string> MessagePosted;//TODO: Peter what the hell are these things 

        /// <summary>
        /// Gets the control that is currently displayed in the WizardControl (the current wizard step's control)
        /// </summary>
        IControlChilli CurrentControl { get; }

        /// <summary>
        /// Gets the Next Button so that it can be programmatically interacted with.
        /// </summary>
        IButton NextButton { get; }

        /// <summary>
        /// Gets the Previous Button so that it can be programmatically interacted with.
        /// </summary>
        IButton PreviousButton { get; }

        /// <summary>
        /// Gets or sets the WizardController.  Upon setting the controller, the Start() method is called to begin the wizard.
        /// </summary>
        IWizardController WizardController { get; set; }

        /// <summary>
        /// The panel that the controls are physically being placed on.
        /// </summary>
        IPanel WizardStepPanel { get; }

        /// <summary>
        /// Gets the Cancel Button so that it can be programmatically interacted with.
        /// </summary>
        IButton CancelButton { get; }

        ///// <summary>
        ///// The label that is displayed at the top of the wizard control for each step.
        ///// </summary>
        //ILabel HeadingLabel { get; }

        /// <summary>
        /// Attempts to go to the next step in the wizard.  If this is disallowed by the wizard controller a MessagePosted event will be fired.
        /// </summary>
        void Next();

        /// <summary>
        /// Attempts to go to the previous step in the wizard.
        ///  </summary>
        /// <exception cref="WizardStepException">If the wizard is on the first step this exception will be thrown.</exception>
        void Previous();

        /// <summary>
        /// Starts the wizard by moving to the first step.
        /// </summary>
        void Start();

        /// <summary>
        /// Calls the finish method on the controller to being the completion process.  If this is successful the Finished event is fired.
        /// </summary>
        void Finish();
    }
}