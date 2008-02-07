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

namespace Habanero.BO
{
    /// <summary>
    /// Provides BOKey related arguments to an event
    /// </summary>
    public class BOKeyEventArgs: EventArgs
    {
        private readonly BOKey _boKey;

        /// <summary>
        /// Constructor to initialise a new event argument
        /// with the affected BOKey
        /// </summary>
        /// <param name="boKey">The affected BOKey</param>
        public BOKeyEventArgs(BOKey boKey)
        {
            _boKey = boKey;
        }

        /// <summary>
        /// Gets the BOKey affected in the event
        /// </summary>
        public BOKey BOKey
        {
            get { return _boKey; }
        }

    }
}
