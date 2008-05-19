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


using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;

namespace Habanero.UI.WebGUI
{
    /// <summary>
    /// An implementation of WizardStep that does nothing and is drawn as a blank panel.
    /// Can be used as a placeholder step in a Wizard that changes depending on selections made
    /// by users.
    /// </summary>
    public partial class NullWizardStepGiz : UserControl, IWizardStep
    {
        /// <summary>
        /// Constructs the NullWizardStep
        /// </summary>
        public NullWizardStepGiz()
        {
            InitializeComponent();
        }

        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        public void InitialiseStep()
        {
        }

        /// <summary>
        /// Always allows moving on.
        /// </summary>
        /// <param name="message">Will always be the empty string</param>
        /// <returns>True</returns>
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
    }
}
