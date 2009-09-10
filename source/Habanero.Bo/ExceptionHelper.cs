// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
    internal static class ExceptionHelper
    {
        internal static readonly string _loaderGenericTypeMethodExceptionMessage = "The GetBusinessObjectCollection<> methods on the BusinessObjectLoader should not have the base 'BusinessObject' type " +
                                                                                  "specified as it's type parameter. Rather use the 'GetBusinessObjectCollection(ClassDef classdef, ...' methods for this. ";

        internal static readonly string _habaneroDeveloperExceptionUserMessage = "There is an application exception please contact your system administrator";

        internal static HabaneroDeveloperException CreateLoaderGenericTypeMethodException()
        {
            return new HabaneroDeveloperException( _habaneroDeveloperExceptionUserMessage, 
                _loaderGenericTypeMethodExceptionMessage);
        }
    }
}
