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

namespace Habanero.UI.Wizard
{
    /// <summary>
    /// Defines the interface for a WizardStep.
    /// </summary>
    public interface IWizardStep
    {
        /// <summary>
        /// Initialises the step. Run when the step is reached.
        /// </summary>
        void InitialiseStep();

        /// <summary>
        /// Verifies whether this step can be passed.
        /// </summary>
        /// <param name="message">Error message should moving on be disallowed. This message will be displayed to the user by the WizardControl.</param>
        /// <returns></returns>
        bool CanMoveOn(out String message);
    }
}
