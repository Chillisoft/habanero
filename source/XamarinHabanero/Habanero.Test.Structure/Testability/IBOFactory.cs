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
using Habanero.Base;
using System;
namespace Habanero.Testability
{
    /// <summary>
    /// The IBOFactory is an interface for creating a Standard Business Object.
    /// This is used so as to prevent the random construction of business objects in code
    /// all over the project as well as to allow the easy mocking/Stubbing or faking of business
    /// object creation when requried for testing.
    /// </summary>
    public interface IBOFactory
    {
        /// <summary>
        /// Creates a business object of Type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T CreateBusinessObject<T>() where T : IBusinessObject;
        /// <summary>
        /// Creates of business object of type <paramref name="type"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IBusinessObject CreateBusinessObject(Type type);
    }
}
