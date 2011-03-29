using System;
using System.Collections.Generic;

namespace Habanero.Base
{
    // ReSharper disable UnusedMember.Global
    
    ///<summary>
    /// An extension to add a ForEach method to IEnumerable{T}
    ///</summary>
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }
    }
}