// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using System.Reflection;
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
        public static void SetEnumPropertyValue(object obj, string propertyName, string enumItemName)
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
        public static string GetEnumPropertyValue(object obj, string propertyName)
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
            if (obj == null) throw new HabaneroArgumentException("obj", "The argument should not be null");
            if (String.IsNullOrEmpty(propertyName)) throw new HabaneroArgumentException("propertyName", "The argument should not be null");
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
                string message = String.Format("Error retrieving public property '{0}' from object of type '{1}'", 
                    propertyName, className);
                log.Error(String.Format("{0}" + Environment.NewLine + "{1}", message,
                                        ExceptionUtilities.GetExceptionString(ex.InnerException, 8, true)));
                //throw ex.InnerException;
                throw new HabaneroApplicationException(message, ex.InnerException);
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
                log.Error(String.Format("Error retrieving private property '{0}' from object of type '{1}'" +
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
                return type.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);
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
                SetPropValue(obj, propInfo, value);

            }
            catch (TargetInvocationException ex)
            {
                string message = String.Format("Error setting public property '{0}' for object of type '{1}'" , propertyName, className);
                log.Error(String.Format(message + Environment.NewLine + "{2}",   ExceptionUtilities.GetExceptionString(ex.InnerException, 8, true)));
                throw new HabaneroApplicationException(message, ex.InnerException);
            }
            catch (ArgumentException ex)
            {
                string message = String.Format("Error setting public property '{0}' for object of type '{1}'", propertyName, className);
                log.Error(String.Format(message + Environment.NewLine + "{2}", ExceptionUtilities.GetExceptionString(ex, 8, true)));
                throw new HabaneroApplicationException(message, ex);
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
                                                            String.Format("Private property set for '{0}' does not exist for object of type '{1}'.", propertyName, className)));
                }
                SetPropValue(obj, propInfo, value);
            }
            catch (TargetInvocationException ex)
            {
                log.Error(String.Format("Error setting private property '{0}' for object of type '{1}'" +
                                        Environment.NewLine + "{2}", propertyName, className,
                                        ExceptionUtilities.GetExceptionString(ex.InnerException, 8, true)));
                throw ex.InnerException;
            }
        }

        private static void SetPropValue(object obj, PropertyInfo propInfo, object value)
        {
            Type propertyType = GetUndelyingPropertType(propInfo);
            object newValue = value == null ? value : ConvertType(value, propertyType);
            propInfo.SetValue(obj, newValue, new object[] { });
        }

        private static object ConvertType(object value, Type propertyType)
        {
            return propertyType.IsAssignableFrom(value.GetType()) ? value : Convert.ChangeType(value, propertyType);
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
        public static void SetInternalPropertyValue(object obj, string propertyName, object value)
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
                SetPropValue(obj, propInfo, value);
            }
            catch (TargetInvocationException ex)
            {
                log.Error(String.Format("Error setting internal property '{0}' for object of type '{1}'" +
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
        public static object ExecuteMethod(object obj, string methodName)
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
                return methodInfo.Invoke(obj, new object[] {});
            }
            catch (TargetInvocationException ex)
            {
                log.Error(String.Format("Error calling public method '{0}' for object of type '{1}'" +
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
        public static object ExecutePrivateMethod(object obj, string methodName)
        {
            return ExecutePrivateMethod(obj, methodName, new object[]{});
        }

        /// <summary>
        /// Executes a parameterless method of an object using reflection
        /// </summary>
        /// <param name="obj">The object owning the method</param>
        /// <param name="methodName">The name of the method</param>
        /// <param name="arguments">The arguments for the private method</param>
        public static object ExecutePrivateMethod(object obj, string methodName, params object[] arguments)
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
                return methodInfo.Invoke(obj, arguments);
            }
            catch (TargetInvocationException ex)
            {
                log.Error(String.Format("Error calling private method '{0}' for object of type '{1}'" +
                                        Environment.NewLine + "{2}", methodName, className,
                                        ExceptionUtilities.GetExceptionString(ex.InnerException, 8, true)));
                throw ex.InnerException;
            }
        }
        /// <summary>
        /// Returnes the Prop Type for the Prop propName.
        /// If the Prop is Nullable then it returns the underlying type.
        /// (i.e bool? will return bool)
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="propName"></param>
        /// <returns>The Property Type</returns>
        public static Type GetUndelyingPropertType(Type classType, string propName)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(classType, propName) 
                        ?? GetPrivatePropertyInfo(classType, propName);
            if (propertyInfo == null || propertyInfo.PropertyType == null)
            {
                return typeof(object);
            }
            return GetUndelyingPropertType(propertyInfo);
        }
        /// <summary>
        /// Returnes the Prop Type for the PropertyInfo.
        /// If the PropertyInfo is Nullable then it returns the underlying type. 
        /// (i.e bool? will return bool)
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static Type GetUndelyingPropertType(PropertyInfo propertyInfo)
        {
            Type propertyType = propertyInfo.PropertyType;
            return IsNullablePropType(propertyType) ? Nullable.GetUnderlyingType(propertyType) : propertyType;
        }

        private static bool IsNullablePropType(Type propertyType)
        {
            return propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
    }

    

   
}