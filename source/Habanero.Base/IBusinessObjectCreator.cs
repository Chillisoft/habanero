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

namespace Habanero.Base
{

    /// <summary>
    /// An interface to model an object creator
    /// </summary>
    public interface IBusinessObjectCreator
    {

        /// <summary>
        /// Just creates the object, without editing or saving it.
        /// </summary>
        /// <returns></returns>
        IBusinessObject CreateBusinessObject();


    }

    ///<summary>
    /// An abstract base class for your own ObjectCreators, created for convenience as it is strongly typed.
    ///</summary>
    ///<typeparam name="T">The type of BO this creator creates.</typeparam>
    public abstract class BusinessObjectCreator<T> : IBusinessObjectCreator where T : IBusinessObject
    {
        #region IBusinessObjectCreator Members

        IBusinessObject IBusinessObjectCreator.CreateBusinessObject()
        {
            return CreateBusinessObject();
        }

        #endregion

        /// <summary>
        /// Just creates the object, without editing or saving it.
        /// </summary>
        /// <returns></returns>
        public abstract T CreateBusinessObject();
    }
}