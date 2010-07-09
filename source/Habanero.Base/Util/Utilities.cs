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
//using System.Linq;
using Habanero.Base.Exceptions;
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Method
                    | AttributeTargets.Class
                    | AttributeTargets.Assembly)]
    public sealed class ExtensionAttribute : Attribute
    {
    }
}
namespace Habanero.Util
{



    ///<summary>
    /// General Utilities
    ///</summary>
    public static class Utilities
    {
        ///<summary>
        /// This method tests the reference passed in to see if it is null or not.
        /// It inspects the actual memory location of the object's pointer to see if it is null or not.
        /// This is useful in the case where you need to test for null without using the == operator.
        ///</summary>
        ///<param name="obj">The object to be tested it it is null or not.</param>
        ///<returns>True if the object is null, or false if not.</returns>
        public static bool IsNull(this object obj)
        {
            WeakReference testNull = new WeakReference(obj);
            return !testNull.IsAlive;
        }



        ///<summary>
        /// Copies the elements of the <see cref="System.Collections.IList"/> to a new array of the specified type.
        ///</summary>
        ///<param name="list">The <see cref="System.Collections.IList"/> to be copied.</param>
        ///<typeparam name="T">The type of the elemtnes of the array to be returned.</typeparam>
        ///<returns>An array of type <typeparamref name="T"/> containing copies of the elements of the <see cref="System.Collections.IList"/>.</returns>
        public static T[] ToArray<T>(IList list)
        {
            //return list.Cast<T>().ToArray();//TODO_ brett 08 Jun 2010: Removed for compatibility to For DotNet 2_0
            T[] array = new T[list.Count];
            int i = 0;
            foreach (T item in list)
            {
                array[i] = item;
                i++;
            }
            return array;
        }
    }
}
