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

namespace Habanero.Base
{

    /// <summary>
    /// Provides BOProp related arguments to an event
    /// </summary>
    public class BOPropEventArgs: EventArgs
    {
        
        private readonly IBOProp _prop;

        /// <summary>
        /// Constructor to initialise a new event argument
        /// with the affected BOProp
        /// </summary>
        /// <param name="prop">The affected BOProp</param>
        public BOPropEventArgs(IBOProp prop)
        {
            _prop = prop;
        }

        /// <summary>
        /// Gets the BOProp affected in the event
        /// </summary>
        public IBOProp Prop
        {
            get { return _prop; }
        }
    }
}