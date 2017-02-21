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
namespace Habanero.Testability
{
    using Habanero.Base;
    using Habanero.Base.Exceptions;
    using System;
    /// <summary>
    /// A generalised factory for creating any habanero business object.
    /// </summary>
    public class BOFactory : IBOFactory
    {
        /// <summary>
        /// Creates a business object of Type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T CreateBusinessObject<T>() where T : IBusinessObject
        {
            return (T)this.CreateBusinessObject(typeof(T));
        }
        /// <summary>
        /// Creates the specified business object of tppe <paramref name="type"/>
        /// </summary>
        /// <param name="type">The type of business object being created.</param>
        /// <returns>the created BO</returns>
        public IBusinessObject CreateBusinessObject(Type type)
        {
            if (!typeof(IBusinessObject).IsAssignableFrom(type))
            {
                throw new HabaneroDeveloperException("The BOFactory.CreateBusinessObject was called with Type that does not implement IBusinessObject");
            }
            return (Activator.CreateInstance(type) as IBusinessObject);
        }
    }
}
