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
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Habanero.Base.Exceptions;

namespace Habanero.Util
{
    /// <summary>
    /// Provides a loader of types for the specified assembly and class names
    /// </summary>
    public static class TypeLoader
    {
        /// <summary>
        /// Returns the type for the specified assembly and class name
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The class name</param>
        /// <returns>Returns the type if found</returns>
        /// <exception cref="UnknownTypeNameException">Thrown if either
        /// the assembly name or class name cannot be found</exception>
        public static Type LoadType(string assemblyName, string className)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName", "A supplied assembly name was null. " +
                                                                "There may be several reasons for this, including " +
                                                                "an incorrectly named or incorrectly capitalised XML " +
                                                                "element name, resulting in the default assembly for " +
                                                                "that element not being loaded.");
            }
            Assembly classAssembly;
            try
            {
                if (assemblyName == "System.Windows.Forms")
                {
                    classAssembly = typeof(Control).Assembly;
                } else if (assemblyName == "System")
                {
                    classAssembly = typeof(String).Assembly;
                } else if (assemblyName == "System.Drawing")
                {
                    classAssembly = typeof(Image).Assembly;
                } else if (assemblyName == "System.Data")
                {
                    classAssembly = typeof (DataSet).Assembly;
                } else 
                {
                    classAssembly = Assembly.Load(assemblyName);
                }
            }
            catch (FileNotFoundException)
            {
                try
                {
                    try
                    {
                        classAssembly = Assembly.Load(assemblyName);
                    }
                    catch (Exception)
                    {
                        classAssembly = Assembly.LoadFrom(assemblyName + ".dll");
                    }
                    if (classAssembly == null)
                    {
                        throw new FileNotFoundException("Thrown manually");
                    }
                }
                catch (FileNotFoundException)
                {
                    throw new UnknownTypeNameException(String.Format("The assembly {0} could not be found", assemblyName));
                }
            }
            Type classType = classAssembly.GetType(assemblyName + "." + className);
            //If the classType is not found then check if it is a fully qualified 
            // classname. If so, then use it to get the classType.
            if (classType == null && className.Contains(assemblyName))
            {
                classType = classAssembly.GetType(className, false, true);
            }
            if (classType == null)
            {
                int posPoint = assemblyName.LastIndexOf(".");
                if (posPoint != -1)
                {
                    string assemblyPrefix = assemblyName.Substring(0, posPoint);
                    classType = classAssembly.GetType(assemblyPrefix + "." + className, false, true);
                }
            }
            if (classType == null) {
                classType = classAssembly.GetType(className, false, true);
            }
            if (classType == null)
            {
                throw new UnknownTypeNameException(
                    String.Format("The type {0} does not exist in assembly {1}", className, assemblyName));
            }
            return classType;
        }

        ///<summary>
        /// This method is used to load a class type from an assembly name and 
        /// class name if it has not been loaded yet. It also gives a descriptive 
        /// error message if it cannot be loaded.
        ///</summary>
        ///<param name="classType">The class type that is being loaded. 
        /// If this parameter is not null, then nothing is changed, otherwise
        /// the class type once loaded is returned through this parameter.</param>
        ///<param name="assemblyName">The assembly name to use if the class type needs loading</param>
        ///<param name="className">The class name to use if the class type needs loading</param>
        ///<param name="loadingTypeDesc">A description of what the type represents (eg. "Class")</param>
        ///<param name="loadingFor">The name of the class that this type is being loaded for. (eg. "Property Definition")</param>
        ///<exception cref="UnknownTypeNameException">This exception is thrown if the assembly name and 
        /// class name cannot be converted to a type object.</exception>
        public static void LoadClassType(ref Type classType,
                                         string assemblyName, string className,
                                         string loadingTypeDesc, string loadingFor)
        {
            //TODO error: What happens if the assemblyName or className is null?
            if (classType == null && assemblyName != null && className != null)
            {
                try
                {
                    classType = LoadType(assemblyName, className);
                }
                catch (Exception ex)
                {
                    if (loadingTypeDesc.Length == 0)
                        loadingTypeDesc = "class";
                    if (loadingFor.Length > 0)
                        loadingFor = " while attempting to load a " + loadingFor;
                    string errorMessage = string.Format(
                        "Unable to load the {0} type{1}, given the 'assembly' as: '{2}', " +
                        "and the 'type' as: '{3}'. Check that the type exists in the " +
                        "given assembly name and that spelling and capitalisation are correct.",
                        loadingTypeDesc, loadingFor, assemblyName, className);
                    throw new UnknownTypeNameException(errorMessage, ex);
                }
            }
        }

        ///<summary>
        /// Retrieves the assembly name and class name from a class type object.
        ///</summary>
        ///<param name="classType">The class type to get information from</param>
        ///<param name="assemblyName">The return parameter for the assembly name</param>
        ///<param name="className">The return parameter for the class name</param>
        public static void ClassTypeInfo(Type classType, 
                                         out string assemblyName, out string className)
        {
            if (classType != null)
            {
                assemblyName = CleanUpAssemblyName(classType.Assembly.ManifestModule.ScopeName);
                className = classType.FullName;
            } else
            {
                assemblyName = null;
                className = null;
            }
        }

        ///<summary>
        /// A Method used to ensure that an assembly name is not of a bad format for loading.
        ///</summary>
        ///<param name="assemblyName">The assembly name to be cleanded up.</param>
        ///<returns>The assembly name with any assembly file extensions removed.</returns>
        public static string CleanUpAssemblyName(string assemblyName)
        {
            if (assemblyName.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase)
                || assemblyName.EndsWith(".exe", StringComparison.CurrentCultureIgnoreCase))
                assemblyName = assemblyName.Remove(assemblyName.Length - 4);
            return assemblyName;
        }
    }
}