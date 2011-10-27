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