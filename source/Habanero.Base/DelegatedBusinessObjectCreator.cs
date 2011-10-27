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
namespace Habanero.Base
{
    ///<summary>
    /// A <see cref="IBusinessObjectCreator"/> that passes off its creation logic to a delegate passed in the constructor.
    ///</summary>
    public class DelegatedBusinessObjectCreator : DelegatedBusinessObjectCreator<IBusinessObject>
    {
        ///<summary>
        /// Initialises the <see cref="DelegatedBusinessObjectCreator"/> with the specified delegate.
        ///</summary>
        ///<param name="createBusinessObjectDelegate">The delegate to be executed when CreateBusinessObject is called.</param>
        public DelegatedBusinessObjectCreator(CreateBusinessObjectDelegate createBusinessObjectDelegate)
            : base(createBusinessObjectDelegate)
        {
        }
    }

    ///<summary>
    /// A <see cref="IBusinessObjectCreator"/> that passes off its creation logic to a delegate passed in the constructor.
    ///</summary>
    public class DelegatedBusinessObjectCreator<TBusinessObject> : IBusinessObjectCreator
        where TBusinessObject : class, IBusinessObject
    {
        ///<summary>
        /// The delegate for the CreateBusinessObject method.
        ///</summary>
        /// <returns>The newly created <see cref="IBusinessObject"/>.</returns>
        public delegate TBusinessObject CreateBusinessObjectDelegate();
        private readonly CreateBusinessObjectDelegate _createBusinessObjectDelegate;

        ///<summary>
        /// Initialises the <see cref="DelegatedBusinessObjectCreator{TBusinessObject}"/> with the specified delegate.
        ///</summary>
        ///<param name="createBusinessObjectDelegate">The delegate to be executed when CreateBusinessObject is called.</param>
        public DelegatedBusinessObjectCreator(CreateBusinessObjectDelegate createBusinessObjectDelegate)
        {
            _createBusinessObjectDelegate = createBusinessObjectDelegate;
        }

        #region Implementation of IBusinessObjectCreator

        /// <summary>
        /// Creates the object, without editing or saving it.
        /// </summary>
        /// <returns></returns>
        public IBusinessObject CreateBusinessObject()
        {
            if (_createBusinessObjectDelegate != null) return _createBusinessObjectDelegate();
            return null;
        }

        #endregion
    }
}