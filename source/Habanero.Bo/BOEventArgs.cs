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
    /// Provides arguments to attach for an event involving business objects
    /// </summary>
    public class BOEventArgs : EventArgs
    {
        private BusinessObject _bo;

        /// <summary>
        /// Constructor to initialise a new set of arguments
        /// </summary>
        /// <param name="bo">The related business object</param>
        public BOEventArgs(BusinessObject bo)
        {
            _bo = bo;
        }

        /// <summary>
        /// Returns the business object related to the event
        /// </summary>
        public BusinessObject BusinessObject
        {
            get { return _bo; }
        }
    }
}