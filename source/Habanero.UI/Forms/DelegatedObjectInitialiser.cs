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


using System.Data;
using Habanero.Base;
using Habanero.BO;

namespace Habanero.UI.Forms
{
    ///<summary>
    /// An implememtation of the IObjectInitialiser interface where both of the Initialise methods
    /// execute the corresponding supplied delegate.
    ///</summary>
    public class DelegatedObjectInitialiser<T> : IObjectInitialiser
        where T: BusinessObject, new()
    {
        /// <summary>
        /// A delegate that contains the same parameters as the InitialiseObject method.
        /// </summary>
        /// <param name="objToInitialise">The object to initialise</param>
        public delegate void InitialiseObjectDelegate(T objToInitialise);

        /// <summary>
        /// A delegate that contains the same parameters as the InitialiseDataRow method.
        /// </summary>
        /// <param name="row">The DataRow object to initialise</param>
        public delegate void InitialiseDataRowDelegate(DataRow row);

        private InitialiseObjectDelegate _initialiseObjectDelegate;
        private InitialiseDataRowDelegate _initialiseDataRowDelegate;


        /// <summary>
        /// The constructor for this class allows for the implementation of the InitialiseObject
        /// method to be supplied by a delegate.
        /// </summary>
        /// <param name="initialiseObjectDelegate">This delegate will be executed as the implementation of the InitialiseObject method.</param>
        public DelegatedObjectInitialiser(InitialiseObjectDelegate initialiseObjectDelegate)
            : this(initialiseObjectDelegate, null) { }
        
        /// <summary>
        /// The constructor for this class allows for the implementation of the InitialiseDataRow
        /// method to be supplied by a delegate.
        /// </summary>
        /// <param name="initialiseDataRowDelegate">This delegate will be executed as the implementation of the InitialiseDataRow method.</param>
        public DelegatedObjectInitialiser(InitialiseDataRowDelegate initialiseDataRowDelegate) 
            : this(null, initialiseDataRowDelegate) {}

        /// <summary>
        /// The constructor for this class allows for the implementation of the InitialiseDataRow and InitialiseObject
        /// methods to be supplied by corresponding delegates.
        /// </summary>
        /// <param name="initialiseObjectDelegate">This delegate will be executed as the implementation of the InitialiseObject method.</param>
        /// <param name="initialiseDataRowDelegate">This delegate will be executed as the implementation of the InitialiseDataRow method.</param>
        public DelegatedObjectInitialiser(InitialiseObjectDelegate initialiseObjectDelegate, InitialiseDataRowDelegate initialiseDataRowDelegate)
        {
            _initialiseObjectDelegate = initialiseObjectDelegate;
            _initialiseDataRowDelegate = initialiseDataRowDelegate;
        }

        /// <summary>
        /// Initialises the given object
        /// </summary>
        /// <param name="objToInitialise">The object to initialise</param>
        public void InitialiseObject(object objToInitialise)
        {
            T businessObject = objToInitialise as T;
            if (businessObject != null && _initialiseObjectDelegate != null)
            {
                _initialiseObjectDelegate(businessObject);
            }
        }

        /// <summary>
        /// Initialises a DataRow object
        /// </summary>
        /// <param name="row">The DataRow object to initialise</param>
        public void InitialiseDataRow(DataRow row)
        {
            if (_initialiseDataRowDelegate != null)
            {
                _initialiseDataRowDelegate(row);
            }
        }
    }
}
