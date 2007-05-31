using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Chillisoft.Util.v2
{
    ///<summary>
    /// This class contains methods that use reflection to perform 
    /// different operations on objects.
    ///</summary>
    public static class ReflectionUtilities
    {
        ///<summary>
        /// This method is used to set the enum value of a property of an object.
        /// This is done entirely using reflection.
        ///</summary>
        ///<param name="obj">The object for which the property is being set</param>
        ///<param name="propertyName">The name of the property being set</param>
        ///<param name="enumItemName">The name of the Enumerated value (eg. this value for "TypeCode.Int64" would be "Int64")</param>
        public static void setEnumPropertyValue(object obj, string propertyName, string enumItemName)
        {
            if (obj == null) throw new CoreBizArgumentException("obj");
            if (propertyName == "") throw new CoreBizArgumentException("propertyName");
            if (enumItemName == "") throw new CoreBizArgumentException("enumItemName");
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
                            else throw new ReflectionException("Specified enum value ('" + enumItemName +
                                                               "') does not belong to this enum type.");
                        }
                        else throw new ReflectionException("Specified property ('" + propertyName +
                                                           "') is not an Enum type.");
                    }
                    else throw new ReflectionException("Specified property ('" + propertyName +
                                                       "') does not have any parameters to set.");
                }
                else throw new ReflectionException("Cannot find get for property ('" + propertyName + "')");
            }
            else throw new ReflectionException("Cannot find public property ('" + propertyName + "')");
        }

        ///<summary>
        /// This method is used to get the enum value of a property of an object as a string.
        /// This is done entirely using reflection.
        ///</summary>
        ///<param name="obj">The object for which the property value is being requested</param>
        ///<param name="propertyName">The name of the property being set</param>
        public static string getEnumPropertyValue(object obj, string propertyName)
        {
            if (obj == null) throw new CoreBizArgumentException("obj");
            if (propertyName == "") throw new CoreBizArgumentException("propertyName");
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
                        if (returnedEnumValue != null)
                        {
                            return Enum.GetName(enumType, returnedEnumValue);
                        }
                        else return "";
                    }
                    else throw new ReflectionException("Specified property ('" + propertyName + "') is not an Enum type.");
                }
                else throw new ReflectionException("Cannot find get for property ('" + propertyName + "')");
            }
            else throw new ReflectionException("Cannot find public property ('" + propertyName + "')");
        }
    }

    /// <summary>
    /// Provides an exception to throw when the reflection cannot be done
    /// </summary>
    public class ReflectionException : BaseApplicationException
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