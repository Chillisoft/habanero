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
using Habanero.UI.Base;
using Habanero.UI.Base.Wizard;

namespace Habanero.UI.Win
{
    /// <summary>
    /// A basic implementation of WizardStep that can be used for simply displaying a message.  
    /// Should a step be required that is a simple message for the user (such as at the end of a wizard), this step can be used
    /// </summary>
    public partial class MessageWizardStepWin :WizardStepWin, IMessageWizardStep
    {
        /// <summary>
        /// Constructs the MessageWizardStep
        /// </summary>
        public MessageWizardStepWin()
        {
            InitializeComponent();
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