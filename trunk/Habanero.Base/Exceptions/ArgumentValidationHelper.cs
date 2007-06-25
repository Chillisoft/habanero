using System;

namespace Habanero.Base.Exceptions
{
    /// <summary>
    /// Provides methods to check the validity of arguments/parameters
    /// </summary>
    public class ArgumentValidationHelper
    {
        /// <summary>
        /// Constructor to initialise a new helper (set as private to prevent
        /// instantiation, seeing as all methods are static)
        /// </summary>
        private ArgumentValidationHelper()
        {
        }

        /// <summary>
        /// Indicates whether the given object is null
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <returns>Returns true if null</returns>
        public static bool IsNull(object obj)
        {
            return (obj == null);
        }
        
        //[DebuggerStepThrough]
        /// <summary>
        /// Throws an exception if the given object is null
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <param name="parameterName">The parameter name, which will be
        /// displayed in the error message</param>
        /// <exception cref="HabaneroArgumentException">Thrown if the
        /// object is null</exception>
        public static void CheckArgumentNotNull(object obj,
                                                String parameterName)
        {
            if (IsNull(obj))
            {
                throw new HabaneroArgumentException(parameterName, parameterName + " cannot be null");
            }
        }

        //[DebuggerStepThrough]
        /// <summary>
        /// Throws an exception with a given message if the given object is null
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <param name="parameterName">The parameter name, which will be
        /// displayed in the error message</param>
        /// <param name="message">The error message to display</param>
        /// /// <exception cref="HabaneroArgumentException">Thrown if the
        /// object is null</exception>
        public static void CheckArgumentNotNull(object obj,
                                                string parameterName,
                                                string message)
        {
            if (IsNull(obj))
            {
                throw new HabaneroArgumentException(parameterName, message);
            }
        }

        //[DebuggerStepThrough]
        /// <summary>
        /// Checks that the given string is not empty and throws an
        /// exception if so
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <param name="parameterName">The parameter name, which will be
        /// displayed in the error message</param>
        /// <exception cref="HabaneroArgumentException">Thrown if the
        /// string is empty</exception>
        public static void CheckStringArgumentNotEmpty(string str,
                                                       string parameterName)
        {
            //unlikely but best to check, if the string can't be empty, it definitely
            //shouldn't be nothing!
            CheckArgumentNotNull(str, parameterName, parameterName + " cannot be an empty string");

            //QUESTION this could happen with  parameters of other types, very unlikely
            //I think it would almost have to be on purpose, but should we check anyway?

            if (str.Length == 0)
            {
                throw new HabaneroArgumentException(parameterName,
                                                    "Argument cannot be a zero length string.");
            }
        }

        //[DebuggerStepThrough]
        /// <summary>
        /// Checks that the given string is not empty and throws an
        /// exception with a given message if so
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <param name="parameterName">The parameter name, which will be
        /// displayed in the error message</param>
        /// <param name="message">The error message to display</param>
        /// <exception cref="HabaneroArgumentException">Thrown if the
        /// string is empty</exception>
        public static void CheckStringArgumentNotEmpty(string str,
                                                       string parameterName,
                                                       string message)
        {
            CheckArgumentNotNull(str, parameterName, parameterName + " cannot be null.\n" + message);

            if (str.Length == 0)
            {
                throw new HabaneroArgumentException(parameterName,
                                                    "Argument cannot be a zero length string.\n" + message);
            }
        }

        /// <summary>
        /// Checks that the argument type is a sub-type of that specified and
        /// throws an exception if not
        /// </summary>
        /// <param name="parameterType">The parameter/argument type</param>
        /// <param name="parameterName">The parameter name, which will be
        /// displayed in the error message</param>
        /// <param name="expectedType">The type of which the argument should
        /// be a sub-type</param>
        /// <exception cref="HabaneroArgumentException">Thrown if the
        /// argument is not a sub-type of that given</exception>
        public static void CheckArgumentIsSubType(Type parameterType,
                                                  string parameterName,
                                                  Type expectedType)
        {
            CheckArgumentIsSubType(parameterType, parameterName, expectedType, "");
        }

        /// <summary>
        /// Checks that the argument type is a sub-type of that specified and
        /// provides an error message to display if not
        /// </summary>
        /// <param name="parameterType">The parameter/argument type</param>
        /// <param name="parameterName">The parameter name, which will be
        /// displayed in the error message</param>
        /// <param name="expectedType">The type of which the argument should
        /// be a sub-type</param>
        /// <param name="message">The error message to display</param>
        public static void CheckArgumentIsSubType(Type parameterType,
                                                  string parameterName,
                                                  Type expectedType,
                                                  string message)
        {
            CheckArgumentNotNull(parameterType, parameterName);

            if (! (parameterType.IsSubclassOf(expectedType)))
            {
                throw new HabaneroArgumentException(parameterName,
                                                    parameterName + " Argument is expected to be of type " +
                                                    expectedType.Name + ".\n" + message);
            }
        }

        //        //[DebuggerStepThrough] public static void CheckIns tanceIsOfType(objValue As Object, objType As Type)
        //            General.CheckArgumentNotNull(objType, "objType")
        //
        //            if objValue Is Nothing { Return
        //
        //            CheckInstanceIsOfType(objValue, objType, objValue.ToString + 
        //                            " cannot be cast to " + objType.Name)
        //
        //        }
        //
        //
        //        //[DebuggerStepThrough] public static void CheckInstanceIsOfType(objValue As Object, 
        //                objType As Type, strMessage As String)
        //            General.CheckArgumentNotNull(objType, "objType")
        //
        //            if Not (objValue Is Nothing OrElse objType.IsInstanceOfType(objValue)) {
        //                throw new InvalidCastException(strMessage)
        //
        //            }
        //
        //        }
        //
        //        //[DebuggerStepThrough] public static void CheckTypeIsSubType(BaseType As Type, 
        //            SubType As Type)
        //
        //            General.CheckArgumentNotNull(BaseType, "BaseType")
        //            General.CheckArgumentNotNull(SubType, "SubType")
        //
        //            CheckTypeIsSubType(BaseType, SubType, SubType.Name + 
        //                            " is not a subtype of " + BaseType.Name)
        //
        //        }
        //
        //
        //        //[DebuggerStepThrough] public static void CheckTypeIsSubType(BaseType As Type, 
        //                SubType As Type, strMessage As String)
        //
        //            General.CheckArgumentNotNull(BaseType, "BaseType")
        //            General.CheckArgumentNotNull(SubType, "SubType")
        //
        //            if Not SubType.IsSubclassOf(BaseType) {
        //                throw new InvalidCastException(strMessage)
        //
        //            }
        //
        //        }
        //
        //        //[DebuggerStepThrough] public static void CheckTypeSupportsInterface(objType As Type, 
        //                strInterface As String)
        //
        //            General.CheckArgumentNotNull(objType, "objType")
        //
        //            CheckTypeSupportsInterface(objType, strInterface, objType.Name + 
        //                        " does not support the " + strInterface + " interface.")
        //
        //        }
        //
        //        //[DebuggerStepThrough] public static void CheckTypeSupportsInterface(objType As Type, 
        //                strInterface As String, 
        //                strMessage As String)
        //
        //            General.CheckArgumentNotNull(objType, "objType")
        //            CheckStringArgumentNotEmpty(strInterface, "strInterface")
        //
        //            if objType.GetInterface(strInterface) Is Nothing {
        //                throw new NotSupportedException(objType.Name + 
        //                        " does not support the " + strInterface + " interface.")
        //            }
        //
        //        }
    }
}