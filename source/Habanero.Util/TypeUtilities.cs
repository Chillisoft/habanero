//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.Util
{
    /// <summary>
    /// Provides a set of Utilities to work with types.
    /// </summary>
    public class TypeUtilities
    {
        /// <summary>
        /// Indicates if type is an integer type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>true if type is an integer type.</returns>
        public static bool IsInteger(Type type)
        {
            if(type == typeof(int) || type ==typeof(uint) || type == typeof(ushort) || type ==typeof(ulong) || 
                type==typeof(short) || type ==typeof(long) || type ==typeof(byte) || type ==typeof(sbyte))
            {
                return true;
            }else
            {
                return false;
            }
        }

        /// <summary>
        /// Indicates if type is an decimal type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>true if type is an decimal type.</returns>
        public static bool IsDecimal(Type type)
        {
            if(type == typeof(decimal) || type==typeof(float) || type==typeof(double))
            {
                return true;
            }else
            {
                return false;
            }
        }
    }
}
