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
using System.Reflection;
using System.Runtime.Serialization;
using Habanero.Base.Exceptions;
using log4net;

namespace Habanero.Util
{
    ///<summary>
    /// This class contains methods that use reflection to perform 
    /// different operations on objects.
    ///</summary>
    public static class ReflectionUtilities
    {
        private static readonly ILog log = LogManager.GetLogger("Habanero.Util.ReflectionUtilities");

        ///<summary>
        /// This method is used to set the enum value of a property of an object.
        /// This is done entirely using reflection.
        ///</summary>
        ///<param name="obj">The object for which the property is being set</param>
        ///<param name="propertyName">The name of the property being set</param>
        ///<param name="enumItemName">The name of the Enumerated value (eg. this value for "TypeCode.Int64" would be "Int64")</param>
        public static void setEnumPropertyValue(object obj, string propertyName, string enumItemName)
        {
            if (obj == null) throw new HabaneroArgumentException("obj");
            if (String.IsNullOrEmpty(propertyName)) throw new HabaneroArgumentException("propertyName");
            if (String.IsNullOrEmpty(propertyName)) throw new HabaneroArgumentException("enumItemName");
            Type parameterType = obj.GetType();
            PropertyInfo propInfo = parameterType.GetProperty(
                propertyName, BindingFlags.Instance | BindingFlags.Public);
            if (propInfo != null)
            {
                MethodInfo setMethod = propInfo.GetSetMethod();
                if (setMethod != null)
                {
                    ParameterInfo[] setMethodParamInfos = setMethod.GetParameters();
                    if (setMethodParamInfos.Length == 1)
                    {
                        ParameterInfo enumParamInfo = setMethodParamInfos[0];
                        Type enumType = enumParamInfo.ParameterType;
                        if (enumType.IsEnum)
                        {
                            object customEnumValue = Enum.Parse(enumType, enumItemName);
                            if (customEnumValue != null)
                            {
                                propInfo.SetValue(obj, customEnumValue, new object[] {});
                                return;
                            }
                            throw new ReflectionException("Specified enum value ('" + enumItemName +
                                                          "') does not belong to this enum type.");
                        }
                        throw new ReflectionException("Specified property ('" + propertyName +
                                                      "') is not an Enum type.");
                    }
                    throw new ReflectionException("Specified property ('" + propertyName +
                                                  "') does not have any parameters to set.");
                }
                throw new ReflectionException("Cannot find get for property ('" + propertyName + "')");
            }
            throw new ReflectionException("Cannot find public property ('" + propertyName + "')");
        }

        ///<summary>
        /// This method is used to get the enum value of a property of an object as a string.
        /// This is done entirely using reflection.
        ///</summary>
        ///<param name="obj">The object for which the property value is being requested</param>
        ///<param name="propertyName">The name of the property being set</param>
        public static string getEnumPropertyValue(object obj, string propertyName)
        {
            if (obj == null) throw new HabaneroArgumentException("obj");
            if (String.IsNullOrEmpty(propertyName)) throw new HabaneroArgumentException("propertyName");
            Type parameterType = obj.GetType();
            PropertyInfo propInfo = parameterType.GetProperty(
                propertyName, BindingFlags.Instance | BindingFlags.Public);
            if (propInfo != null)
            {
                MethodInfo getMethod = propInfo.GetGetMethod();
                if (getMethod != null)
                {
                    Type enumType = getMethod.ReturnType;
                    if (enumType.IsEnum)
                    {
                        object returnedEnumValue = propInfo.GetValue(obj, new object[] {});
                        return returnedEnumValue != null ? Enum.GetName(enumType, returnedEnumValue) : "";
                    }
                    throw new ReflectionException("Specified property ('" + propertyName + "') is not an Enum type.");
                }
                throw new ReflectionException("Cannot find get for property ('" + propertyName + "')");
            }
            throw new ReflectionException("Cannot find public property ('" + propertyName + "')");
        }

        ///<summary>
        /// Returns the value of a property of an object using reflection
        ///</summary>
        ///<param name="obj">The object to get the value from</param>
        ///<param name="propertyName">The name of the property to get the value from</param>
        ///<returns>The value of the specified property of the supplied object</returns>
        ///<exception cref="HabaneroArgumentException">This error is thrown when an invalid parameter is given</exception>
        ///<exception cref="TargetInvocationException">This error is thrown when there is an error in finding the property on the supplied object</exception>
        ///<exception cref="Exception">This is a general exception that is thrown if there is an error in retrieving the value.</exception>
        public static object GetPropertyValue(object obj, string propertyName)
        {
            if (obj == null) throw new HabaneroArgumentException("obj");
            if (String.IsNullOrEmpty(propertyName)) throw new HabaneroArgumentException("propertyName");
            Type type = obj.GetType();
            string className = type.Name;
            try
            {
                PropertyInfo propInfo = GetPropertyInfo(type, propertyName);
                if (propInfo == null)
                {
                    throw new TargetInvocationException(new Exception(
                                                            String.Format("Virtual property '{0}' does not exist for object of type '{1}'.", propertyName, className)));
                }
                object propValue = propInfo.GetValue(obj, new object[] { });
                return propValue;
            }
            catch (TargetInvocationException ex)
            {
                log.Error(String.Format("Error retrieving virtual property '{0}' from object of type '{1}'" +
                                        Environment.NewLine + "{2}", propertyName, className,
                                        ExceptionUtilities.GetExceptionString(ex.InnerException, 8, true)));
                throw ex.InnerException;
            }
        }

        ///<summary>
        /// Returns the PropertyInfo for the specified property, otherwise it returns nothing if the property does not exist
        ///</summary>
        ///<param name="type">The type to find the specifed property on</param>
        ///<param name="propertyName">The name of the property to search for</param>
        ///<returns>The PropertyInfo object representing the requested method, or null if it does not exist.</returns>
        ///<exception cref="HabaneroArgumentException">This error is thrown when an invalid parameter is given</exception>
        public static PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            if (type == null) throw new HabaneroArgumentException("type");
            if (String.IsNullOrEmpty(propertyName)) throw new HabaneroArgumentException("propertyName");
            try
            {
                return type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
            }
            catch
            {
                return null;
            }
        }


        ///<summary>
        /// Returns the value for a private property
        ///</summary>
        ///<param name="obj">The object to get the value from</param>
        ///<param name="propertyName">The name of the property to get the value from</param>
        ///<returns>The value of the specified property of the supplied object</returns>
        ///<exception cref="HabaneroArgumentException"></exception>
        ///<exception cref="TargetInvocationException"></exception>
        ///<exception cref="Exception"></exception>
        public static object GetPrivatePropertyValue(object obj, string propertyName)
        {
            if (obj == null) throw new HabaneroArgumentException("obj");
            if (String.IsNullOrEmpty(propertyName)) throw new HabaneroArgumentException("propertyName");
            Type type = obj.GetType();
            string className = type.Name;
            try
            {
                PropertyInfo propInfo = GetPrivatePropertyInfo(type, propertyName);
                if (propInfo == null)
                {
                    throw new TargetInvocationException(new Exception(
                                                            String.Format("Virtual property '{0}' does not exist for object of type '{1}'.", propertyName, className)));
                }
                object propValue = propInfo.GetValue(obj, new object[] { });
                return propValue;
            }
            catch (TargetInvocationException ex)
            {
                log.Error(String.Format("Error retrieving virtual property '{0}' from object of type '{1}'" +
                                        Environment.NewLine + "{2}", propertyName, className,
                                    ExceptionUtilities.GetExceptionString(ex.InnerException, 8, true)));
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Returns the PropertyInfo for a Private Property
        /// </summary>
        ///<param name="type">The type to find the specifed property on</param>
        ///<param name="propertyName">The name of the property to search for</param>
        ///<returns>The PropertyInfo object representing the requested method, or null if it does not exist.</returns>
        public static PropertyInfo GetPrivatePropertyInfo(Type type, string propertyName)
        {
            if (type == null) throw new HabaneroArgumentException("type");
            if (String.IsNullOrEmpty(propertyName)) throw new HabaneroArgumentException("propertyName");
            try
            {
                return type.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance);
            }
            catch
            {
                return null;
            }
        }


        ///<summary>
        /// Returns the MethodInfo for the specified property, otherwise it 
        ///    returns nothing if the property does not exist
        ///</summary>
        ///<param name="type">The type to find the specifed property on</param>
        ///<param name="methodName">The name of the property to search for</param>
        ///<returns>The PropertyInfo object representing the requested method, or null if it does not exist.</returns>
        ///<exception cref="HabaneroArgumentException">This error is thrown when an invalid parameter is given</exception>
        public static MethodInfo GetMethodInfo(Type type, string methodName)
        {
            if (type == null) throw new HabaneroArgumentException("type");
            if (String.IsNullOrEmpty(methodName)) throw new HabaneroArgumentException("methodName");
            try
            {
                return type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
            }
            catch
            {
                return null;
            }
        }

        ///<summary>
        /// Returns the MethodInfo for the specified property, otherwise it 
        ///    returns nothing if the property does not exist
        ///</summary>
        ///<param name="type">The type to find the specifed property on</param>
        ///<param name="methodName">The name of the property to search for</param>
        ///<returns>The PropertyInfo object representing the requested method, or null if it does not exist.</returns>
        ///<exception cref="HabaneroArgumentException">This error is thrown when an invalid parameter is given</exception>
        public static MethodInfo GetPrivateMethodInfo(Type type, string methodName)
        {
            if (type == null) throw new HabaneroArgumentException("type");
            if (String.IsNullOrEmpty(methodName)) throw new HabaneroArgumentException("methodName");
            try
            {
                return type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            }
            catch
            {
                return null;
            }
        }

        ///<summary>
        /// Sets the value of a property of an object using reflection
        ///</summary>
        ///<param name="obj">The object for which to set the value</param>
        ///<param name="propertyName">The name of the property to be set</param>
        ///<param name="value">The value that is to be set</param>
        ///<exception cref="HabaneroArgumentException">This error is thrown when an invalid parameter is given</exception>
        ///<exception cref="TargetInvocationException">This error is thrown when there is an error in finding the property on the supplied object</exception>
        ///<exception cref="Exception">This is a general exception that is thrown if there is an error in retrieving the value.</exception>
        public static void SetPropertyValue(object obj, string propertyName, object value)
        {
            if (obj == null) throw new HabaneroArgumentException("obj");
            if (String.IsNullOrEmpty(propertyName)) throw new HabaneroArgumentException("propertyName");
            Type type = obj.GetType();
            string className = type.Name;
            try
            {
                PropertyInfo propInfo = GetPropertyInfo(type, propertyName);
                if (propInfo == null)
                {
                    throw new TargetInvocationException(new Exception(
                                                            String.Format("Virtual property set for '{0}' does not exist for object of type '{1}'.", propertyName, className)));
                }
                object newValue = Convert.ChangeType(value, propInfo.PropertyType);
                propInfo.SetValue(obj, newValue, new object[] { });
            }
            catch (TargetInvocationException ex)
            {
                log.Error(String.Format("Error setting virtual property '{0}' for object of type '{1}'" +
                                        Environment.NewLine + "{2}", propertyName, className,
                                        ExceptionUtilities.GetExceptionString(ex.InnerException, 8, true)));
                throw ex.InnerException;
            }
        }
        ///<summary>
        /// Sets the value of a property of an object using reflection
        ///</summary>
        ///<param name="obj">The object for which to set the value</param>
        ///<param name="propertyName">The name of the property to be set</param>
        ///<param name="value">The value that is to be set</param>
        ///<exception cref="HabaneroArgumentException">This error is thrown when an invalid parameter is given</exception>
        ///<exception cref="TargetInvocationException">This error is thrown when there is an error in finding the property on the supplied object</exception>
        ///<exception cref="Exception">This is a general exception that is thrown if there is an error in retrieving the value.</exception>
        public static void SetPrivatePropertyValue(object obj, string propertyName, object value)
        {
            if (obj == null) throw new HabaneroArgumentException("obj");
            if (String.IsNullOrEmpty(propertyName)) throw new HabaneroArgumentException("propertyName");
            Type type = obj.GetType();
            string className = type.Name;
            try
            {
                PropertyInfo propInfo = GetPrivatePropertyInfo(type, propertyName);
                if (propInfo == null)
                {
                    throw new TargetInvocationException(new Exception(
                                                            String.Format("Virtual property set for '{0}' does not exist for object of type '{1}'.", propertyName, className)));
                }
                object newValue = Convert.ChangeType(value, propInfo.PropertyType);
                propInfo.SetValue(obj, newValue, new object[] { });
            }
            catch (TargetInvocationException ex)
            {
                log.Error(String.Format("Error setting virtual property '{0}' for object of type '{1}'" +
                                        Environment.NewLine + "{2}", propertyName, className,
                                        ExceptionUtilities.GetExceptionString(ex.InnerException, 8, true)));
                throw ex.InnerException;
            }
        }
        /// <summary>
        /// Executes a parameterless method of an object using reflection
        /// </summary>
        /// <param name="obj">The object owning the method</param>
        /// <param name="methodName">The name of the method</param>
        public static void ExecuteMethod(object obj, string methodName)
        {
            if (obj == null) throw new HabaneroArgumentException("obj");
            if (String.IsNullOrEmpty(methodName)) throw new HabaneroArgumentException("methodName");
            Type type = obj.GetType();
            string className = type.Name;
            try
            {
                MethodInfo methodInfo = GetMethodInfo(type, methodName);
                if (methodInfo == null)
                {
                    throw new TargetInvocationException(
                        new Exception(String.Format(
                                          "Virtual method call for '{0}' does not exist for object of type '{1}'.",
                                          methodName, className)));
                }
                methodInfo.Invoke(obj, new object[] {});
            }
            catch (TargetInvocationException ex)
            {
                log.Error(String.Format("Error calling virtual method '{0}' for object of type '{1}'" +
                                        Environment.NewLine + "{2}", methodName, className,
                                        ExceptionUtilities.GetExceptionString(ex.InnerException, 8, true)));
                throw ex.InnerException;
            }
        }
        /// <summary>
        /// Executes a parameterless method of an object using reflection
        /// </summary>
        /// <param name="obj">The object owning the method</param>
        /// <param name="methodName">The name of the method</param>
        public static void ExecutePrivateMethod(object obj, string methodName)
        {
            if (obj == null) throw new HabaneroArgumentException("obj");
            if (String.IsNullOrEmpty(methodName)) throw new HabaneroArgumentException("methodName");
            Type type = obj.GetType();
            string className = type.Name;
            try
            {
                MethodInfo methodInfo = GetPrivateMethodInfo(type, methodName);
                if (methodInfo == null)
                {
                    throw new TargetInvocationException(
                        new Exception(String.Format(
                                          "Virtual method call for '{0}' does not exist for object of type '{1}'.",
                                          methodName, className)));
                }
                methodInfo.Invoke(obj, new object[] { });
            }
            catch (TargetInvocationException ex)
            {
                log.Error(String.Format("Error calling virtual method '{0}' for object of type '{1}'" +
                                        Environment.NewLine + "{2}", methodName, className,
                                        ExceptionUtilities.GetExceptionString(ex.InnerException, 8, true)));
                throw ex.InnerException;
            }
        }
    }

    

    /// <summary>
    /// Provides an exception to throw when the reflection cannot be done
    /// </summary>
    public class ReflectionException : Exception
    {
        /// <summary>
        /// Constructor to initialise the exception
        /// </summary>
        public ReflectionException()
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display
        /// </summary>
        /// <param name="message">The error message</param>
        public ReflectionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with a specific message
        /// to display and the inner exception specified
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="inner">The inner exception</param>
        public ReflectionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Constructor to initialise the exception with the serialisation info
        /// and streaming context provided
        /// </summary>
        /// <param name="info">The serialisation info</param>
        /// <param name="context">The streaming context</param>
        protected ReflectionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}