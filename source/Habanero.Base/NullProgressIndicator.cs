//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

namespace Habanero.Base
{
    /// <summary>
    /// Provides a null progress indicator that does nothing with the inputs
    /// it receives
    /// </summary>
    public class NullProgressIndicator : IProgressIndicator
    {
        /// <summary>
        /// Constructor to initialise a new indicator
        /// </summary>
        public NullProgressIndicator()
        {
        }

        /// <summary>
        /// Does nothing, so the parameters can be set to null
        /// </summary>
        public void UpdateProgress(int amountComplete, int totalToComplete, string description)
        {
        }

        /// <summary>
        /// Completes the progress, in this case doing nothing
        /// </summary>
        public void Complete()
        {
        }
    }
}