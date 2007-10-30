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

using System;

namespace Habanero.Base
{
    /// <summary>
    /// Indicates to the user how much progress has been made in 
    /// completing a task, by adding text output to the console
    /// </summary>
    public class ConsoleProgressIndicator : IProgressIndicator
    {
        /// <summary>
        /// Constructor to initialise a new indicator
        /// </summary>
        public ConsoleProgressIndicator()
        {
        }

        /// <summary>
        /// Updates the indicator with progress information by adding a line
        /// of text output to the console
        /// </summary>
        /// <param name="amountComplete">The amount complete already</param>
        /// <param name="totalToComplete">The total amount to be completed</param>
        /// <param name="description">A description</param>
        public void UpdateProgress(int amountComplete, int totalToComplete, string description)
        {
            Console.WriteLine(amountComplete + " of " + totalToComplete + " steps complete. " + description);
        }

        /// <summary>
        /// Adds a line of text to the console with the message "Complete."
        /// </summary>
        public void Complete()
        {
            Console.WriteLine("Complete.");
        }
    }
}