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

using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;
using Habanero.UI.VWG;

namespace Habanero.UI.VWG.Wizard
{
    /// <summary>
    /// A basic implementation of WizardStep that can be used for simply displaying a message.  
    /// Should a step be required that is a simple message for the user (such as at the end of a wizard), this step can be used
    /// </summary>
    public partial class MessageWizardStepVWG : UserControlVWG, IWizardStep
    {
        /// <summary>
        /// Constructs the MessageWizardStep
        /// </summary>
        public MessageWizardStepVWG()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlHabanero.Controls
        {
            get { return new ControlCollectionVWG(base.Controls); }
        }


        /// <summary>
        /// Initialises the step. Run when the step is reached.
        /// </summary>
        public void InitialiseStep() { }

        /// <summary>
        /// Always returns true as this wizard step is simply for displaying a message to a user.
        /// </summary>
        /// <param name="message">Out parameter that will always be the empty string</param>
        /// <returns>true</returns>
        public bool CanMoveOn(out string message)
        {
            message = "";
            return true;
        }

        /// <summary>
        /// Verifies whether the user can move back from this step.
        /// </summary>
        /// <returns></returns>
        public bool CanMoveBack()
        {
            return true;
        }

        /// <summary>
        /// The text that you want displayed at the top of the wizard control when this step is active.
        /// </summary>
        public string HeaderText
        {
            get { return ""; }
        }

        /// <summary>
        /// Provides an interface for the developer to implement functionality to cancel any edits made as part of this
        /// wizard step. The default wizard controller functionality is to call all wizard steps cancelStep methods when
        /// its Cancel method is called.
        /// </summary>
        public void CancelStep()
        {
            
        }

        /// <summary>
        /// Sets the message to be displayed to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        public void SetMessage(string message)
        {
            _uxMessageLabel.Text = message;
        }
    }
}