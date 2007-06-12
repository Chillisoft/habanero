using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Chillisoft.Generic.v2;

namespace Chillisoft.Util.v2
{
    /// <summary>
    /// Provides a loader of types for the specified assembly and class names
    /// </summary>
    public class TypeLoader
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
            if (assemblyName == null || assemblyName.Length == 0)
            {
                throw new ArgumentNullException("assemblyName","A supplied assembly name was null. " +
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
                    classAssembly = typeof (Control).Assembly;
                }
                else if (assemblyName == "System")
                {
                    classAssembly = typeof (String).Assembly;
                }
                else if (assemblyName == "System.Drawing")
                {
                    classAssembly = typeof (Image).Assembly;
                }
                else
                {
                    classAssembly = Assembly.Load(assemblyName);
                }
            }
            catch (FileNotFoundException)
            {
                try
                {
                    classAssembly = Assembly.LoadWithPartialName(assemblyName);
                    if (classAssembly == null)
                    {
                        classAssembly = Assembly.LoadFrom(assemblyName + ".dll");
                    }
                    if (classAssembly == null)
                    {
                        throw new FileNotFoundException("Thrown manually");
                    }
                }
                catch(FileNotFoundException)
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
            if (classType == null)
            {
                throw new UnknownTypeNameException(
                    String.Format("The type {0} does not exist in assembly {1}", className, assemblyName));
            }
            return classType;
        }
    }
}