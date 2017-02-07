#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
namespace Habanero.Base
{
    /// <summary>
    /// An interface to model a progress indicator that informs the user
    /// how much progress has been made in completing a task
    /// </summary>
    public interface IProgressIndicator
    {
        /// <summary>
        /// Updates the indicator with progress information
        /// </summary>
        /// <param name="amountComplete">The amount complete already</param>
        /// <param name="totalToComplete">The total amount to be completed</param>
        /// <param name="description">A description</param>
        void UpdateProgress(int amountComplete, int totalToComplete, string description);
        
        /// <summary>
        /// Sets the indicator to completion status
        /// </summary>
        void Complete();
    }
}