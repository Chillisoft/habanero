using System;
using System.Collections;
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

        public static bool IsEqualTo(this IEnumerable enumerable1, IEnumerable enumerable2)
        {
            var enumerator1 = enumerable1.GetEnumerator();
            var enumerator2 = enumerable2.GetEnumerator();
            while (enumerator1.MoveNext())
            {
                if (!enumerator2.MoveNext()) return false;
                if (!enumerator1.Current.Equals(enumerator2.Current)) return false;
            }
            return !enumerator2.MoveNext();
        }
    }
}