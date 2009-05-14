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
using Habanero.Base;

namespace Habanero.Base
{
    /// <summary>
    /// Provides arguments to attach for an event involving business objects
    /// </summary>
    public class BOEventArgs : EventArgs
    {
        private readonly IBusinessObject _bo;

        /// <summary>
        /// Constructor to initialise a new set of arguments
        /// </summary>
        /// <param name="bo">The related business object</param>
        public BOEventArgs(IBusinessObject bo)
        {
            _bo = bo;
        }

        /// <summary>
        /// Returns the business object related to the event
        /// </summary>
        public IBusinessObject BusinessObject
        {
            get { return _bo; }
        }
    }


    /// <summary>
    /// Provides arguments to attach for an event involving business objects
    /// </summary>
    public class BOEventArgs<TBusinessObject> : BOEventArgs where TBusinessObject : IBusinessObject
    {
        private readonly TBusinessObject _bo;

        /// <summary>
        /// Constructor to initialise a new set of arguments
        /// </summary>
        /// <param name="bo">The related business object</param>
        public BOEventArgs(TBusinessObject bo) : base(bo) {
            _bo = bo;
        }

        /// <summary>
        /// Returns the business object related to the event
        /// </summary>
        public new TBusinessObject BusinessObject
        {
            get { return _bo; }
        }
    }

}