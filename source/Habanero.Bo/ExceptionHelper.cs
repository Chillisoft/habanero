using System;
using System.Collections.Generic;
using System.Text;
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
