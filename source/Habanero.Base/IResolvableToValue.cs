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
    /// Defines a generalised type that can be resolved to a value
    ///</summary>
    public interface IResolvableToValue : IResolvableToValue<object> { }

    ///<summary>
    /// Defines a generalised type that can be resolved to a value of type <see cref="T"/>
    ///</summary>
    ///<typeparam name="T">The value type that this object can be resolved to.</typeparam>
    public interface IResolvableToValue<T>
    {
        ///<summary>
        /// Resolved the instance class to a value of type <see cref="T"/>.
        ///</summary>
        ///<returns>The value that the instance class is resolved to.</returns>
        T ResolveToValue();
    }
}