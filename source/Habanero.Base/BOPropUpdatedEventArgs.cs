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

namespace Habanero.Base
{
    /// <summary>
    /// Provides <see cref="IBusinessObject"/> and <see cref="IBOProp"/> related arguments 
    /// to an event which is fired when the <see cref="IBOProp"/> is updated.
    /// </summary>
    public class BOPropUpdatedEventArgs : EventArgs
    {
        private readonly IBOProp _prop;
        private readonly IBusinessObject _businessObject;

        /// <summary>
        /// Constructor to initialise a new event argument
        /// with the updated <see cref="IBOProp"/> and the <see cref="IBusinessObject"/> to which the prop belongs.
        /// </summary>
        /// <param name="businessObject">The <see cref="IBusinessObject"/> to which the updated <see cref="IBOProp"/> belongs.</param>
        /// <param name="prop">The updated <see cref="IBOProp"/>.</param>
        public BOPropUpdatedEventArgs(IBusinessObject businessObject, IBOProp prop)
        {
            _businessObject = businessObject;
            _prop = prop;
        }

        /// <summary>
        /// Gets the BOProp updated in the event
        /// </summary>
        public IBOProp Prop
        {
            get { return _prop; }
        }

        ///<summary>
        /// The <see cref="IBusinessObject"/> to which the updated <see cref="IBOProp"/> belongs.
        ///</summary>
        public IBusinessObject BusinessObject
        {
            get { return _businessObject; }
        }
    }

    /// <summary>
    /// Provides arguments to attach for an event involving business objects
    /// </summary>
    public class BOPropUpdatedEventArgs<TBusinessObject> : BOPropUpdatedEventArgs where TBusinessObject : IBusinessObject
    {
        private readonly TBusinessObject _bo;

        /// <summary>
        /// Constructor to initialise a new set of arguments
        /// </summary>
        /// <param name="bo">The related business object</param>
        public BOPropUpdatedEventArgs(TBusinessObject bo, IBOProp prop)
            : base(bo, prop)
        {
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