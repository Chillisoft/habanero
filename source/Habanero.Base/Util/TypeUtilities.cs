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
using System.ComponentModel;
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.Util
{
    /// <summary>
    /// Provides a set of Utilities to work with types.
    /// </summary>
    public static class TypeUtilities
    {
        /// <summary>
        /// Indicates if type is an integer type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>true if type is an integer type.</returns>
        public static bool IsInteger(this Type type)
        {
            return type == typeof(int) || type ==typeof(uint) || type == typeof(ushort) || type ==typeof(ulong) || 
                   type==typeof(short) || type ==typeof(long) || type ==typeof(byte) || type ==typeof(sbyte);
        }

        /// <summary>
        /// Indicates if type is an decimal type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>true if type is an decimal type.</returns>
        public static bool IsDecimal(this Type type)
        {
            return type == typeof(decimal) || type==typeof(float) || type==typeof(double);
        }

        ///<summary>
        /// Converts a value to the specified type using the <see cref="TypeConverter"/> associated with the source value.
        ///</summary>
        ///<param name="obj">The value to convert</param>
        ///<typeparam name="TDestinationType">The type to convert the value to.</typeparam>
        ///<returns>The converted value</returns>
        public static TDestinationType ConvertTo<TDestinationType>(this object obj)
        {
            return (TDestinationType)ConvertTo(typeof(TDestinationType), obj);
        }

        ///<summary>
        /// Converts a value to the specified type using the <see cref="TypeConverter"/> associated with the source value.
        ///</summary>
        ///<param name="type">The type to convert the value to.</param>
        ///<param name="obj">The value to convert</param>
        ///<returns>The converted value</returns>
        public static object ConvertTo(Type type, object obj)
        {
            
            if (Utilities.IsNull(obj)) return null;
            Type sourceType = obj.GetType();
            bool isNullableType = false;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                isNullableType = true;
                type = Nullable.GetUnderlyingType(type);
            }
            object returnValue;
            if (type == sourceType || sourceType.IsSubclassOf(type)) returnValue = obj;
            else
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(sourceType);
                returnValue = typeConverter.ConvertTo(obj, type);
            }
            if (isNullableType)
            {
                returnValue = Activator.CreateInstance(typeof (Nullable<>).MakeGenericType(type), returnValue);
            }
            return returnValue;
        }

        ///<summary>
        ///</summary>
        ///<param name="type"></param>
        ///<exception cref="UnknownTypeNameException"></exception>
        public static void CheckTypeCanBeCreated(this Type type)
        {
            //Check that the type can be created and raise appropriate error 
            try
            {
                Activator.CreateInstance(type, true);
            }
            catch (Exception ex)
            {
                throw new UnknownTypeNameException
                    (String.Format
                         ("An error occurred while attempting to load a related "
                          + "business object collection, with the type given as '{0}'. "
                          + "Check that the given type exists and has been correctly "
                          + "defined in the relationship and class definitions for the classes " + "involved.", type),
                     ex);
            }
        }
    }
}