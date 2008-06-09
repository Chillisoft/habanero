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
