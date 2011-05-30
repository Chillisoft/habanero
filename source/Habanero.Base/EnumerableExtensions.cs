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
        ///<summary>
        ///</summary>
        ///<param name="enumeration"></param>
        ///<param name="action"></param>
        ///<typeparam name="T"></typeparam>
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }
    }
}