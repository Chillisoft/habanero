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
using Habanero.Base.Exceptions;
using Habanero.Util;

namespace Habanero.Base
{
    /// <summary>
    /// Manages a collection of class definitions.
    /// </summary>
    public class ClassDefCol : IEnumerable<IClassDef>
    {
        private static ClassDefCol _classDefcol;
        private static bool _instanceFlag;
        private readonly Dictionary<string, IClassDef> _classDefs;

        /// <summary>
        /// Initialises an empty collection
        /// </summary>
        public ClassDefCol()
        {
            _classDefs = new Dictionary<string, IClassDef>();
        }

        /// <summary>
        /// Provides an indexing facility for the collection so that items
        /// in the collection can be accessed like an array 
        /// (e.g. collection["surname"])
        /// </summary>
        /// <param name="key">The name of the class definition</param>
        /// <returns>Returns the class definition that matches the key
        /// or null if none is found</returns>
        public IClassDef this[Type key]
        {
            get
            {
                bool found;
                string typeId = GetTypeIdForItem(key, out found);
                if (found) 
                    return _classDefs[typeId];
                
                ThrowClassDefNotFoundForTypeException(key);
                
                return null;
            }
        }


        private static void ThrowClassDefNotFoundForTypeException(Type type)
        {
            throw new HabaneroDeveloperException(
                string.Format("No ClassDef has been loaded for {0}. " +
                              "If you have loaded your ClassDefs please check that the Assembly specified matches the Assembly the class is in. " +
                              "In this case, make sure that the ClassDef of {1} specifies the assembly {2}, or move the class into the assembly " +
                              "specified in the ClassDef.",
                              type.FullName, type.Name, type.Assembly.GetName().Name), "");
        }

        private static void ThrowClassDefNotFoundForTypeException(string assemblyName, string className, string typeid)
        {
            throw new HabaneroDeveloperException(
                string.Format("No ClassDef has been loaded for {0}. " +
                              "If you have loaded your ClassDefs please check that the Assembly specified matches the Assembly the class is in. " +
                              "In this case, make sure that the ClassDef of {1} specifies the assembly {2}, or move the class into the assembly " +
                              "specified in the ClassDef.",
                              typeid, className, assemblyName), "");
        }

        /// <summary>
        /// Provides an indexing facility for the collection so that items
        /// in the collection can be accessed like an array 
        /// (e.g. collection["surname"])
        /// </summary>
        /// <param name="assemblyName">The name of the class assembly</param>
        /// <param name="className">The name of the class</param>
        /// <returns>Returns the class definition that matches the key
        /// or null if none is found</returns>
        public IClassDef this[string assemblyName, string className]
        {
            get
            {
                bool found;
                string typeId = GetTypeIdForItem(assemblyName, className, out found);
                if (found)
                    return _classDefs[typeId];

                ThrowClassDefNotFoundForTypeException(assemblyName, className, typeId);
                return null;
            }
        }

        /// <summary>
        /// Returns a collection of the key names being stored
        /// </summary>
        internal ICollection Keys
        {
            get { return (_classDefs.Keys); }
        }

        /// <summary>
        /// Returns a collection of the values being stored
        /// </summary>
        internal ICollection Values
        {
            get { return (_classDefs.Values); }
        }

        #region IEnumerable<ClassDef> Members

        IEnumerator<IClassDef> IEnumerable<IClassDef>.GetEnumerator()
        {
            return _classDefs.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _classDefs.Values.GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Adds a class definition to the collection
        /// </summary>
        /// <param name="value">The class definition to add</param>
        public void Add(IClassDef value)
        {
            if (_classDefs.ContainsValue(value)) return;
            string typeId = GetTypeId(value.AssemblyName, value.ClassName, true);
            if (_classDefs.ContainsKey(typeId))
            {
                throw new InvalidXmlDefinitionException(String.Format(
                                                            "A duplicate class element has been encountered, where the " +
                                                            "type '{0}.{1}' has already been defined previously.",
                                                            value.AssemblyName, value.ClassName));
            }
            _classDefs.Add(typeId, value);
        }


        /// <summary>
        /// Adds a class definition to the collection
        /// </summary>
        /// <param name="value">The class definition to add</param>
        public void Add(ClassDefCol classDefCol)
        {
            foreach (IClassDef classDef in classDefCol)
            {
                Add(classDef);
            }
        }
        
        /// <summary>
        /// Indicates whether the collection contains a class definition
        /// representing the passed type.
        /// </summary>
        /// <param name="key">The name of the class definition</param>
        /// <returns>Returns true if found, false if not</returns>
        public bool Contains(Type key)
        {
            bool found;
            GetTypeIdForItem(key, out found);
            return found;
        }

        /// <summary>
        /// Indicates whether the collection contains a class definition
        /// representing the passed type.
        /// </summary>
        /// <param name="assemblyName">The name of the class assembly</param>
        /// <param name="className">The name of the class</param>
        /// <returns>Returns true if found, false if not</returns>
        public bool Contains(string assemblyName, string className)
        {
            bool found;
            GetTypeIdForItem(assemblyName, className, out found);
            return found;
        }

        /// <summary>
        /// Indicates whether the collection contains the class definition
        /// that is passed as a parameter.
        /// </summary>
        /// <param name="classDef">The class definition to look for.</param>
        /// <returns>Returns true if found, false if not</returns>
        public bool Contains(IClassDef classDef)
        {
            bool found;
            GetTypeIdForItem(classDef.AssemblyName, classDef.ClassName, out found);
            return found;
        }


        /// <summary>
        /// Removes the class definition for the specified type from the
        /// collection.
        /// </summary>
        /// <param name="key">The name of the class definition</param>
        public void Remove(Type key)
        {
            bool found;
            string typeId = GetTypeIdForItem(key, out found);
            if (found) _classDefs.Remove(typeId);
        }

        /// <summary>
        /// Removes the specified class definition from the collection.
        /// </summary>
        /// <param name="classDef">The class definition to be removed</param>
        public void Remove(IClassDef classDef)
        {
            bool found;
            string typeId = GetTypeIdForItem(classDef.AssemblyName, classDef.ClassName, out found);
            if (found) _classDefs.Remove(typeId);
        }

        /// <summary>
        /// Removes the specified class definition from the collection.
        /// </summary>
        /// <param name="assemblyName">The assembly name of the class definition to remove</param>
        /// <param name="className">The name of the class definition to remove</param>
        public void Remove(string assemblyName, string className)
        {
            bool found;
            string typeId = GetTypeIdForItem(assemblyName, className, out found);
            if (found) _classDefs.Remove(typeId);
        }

        /// <summary>
        /// Removes a flag that indicates that a collection exists.  After
        /// this flag is removed, calling LoadColClassDef will result in a
        /// new empty collection replacing the existing one.
        /// </summary>
        protected static void FinalizeInstanceFlag()
        {
            _instanceFlag = false;
        }

        ///<summary>
        /// Finds the class definition in the collection with the specified name.
        /// Returns null if there is no class definition found with the specified name.
        ///</summary>
        ///<param name="className">The name of the class to find</param>
        ///<returns>The class definition with the specified name, otherwise returns null.</returns>
        public IClassDef FindByClassName(string className)
        {
            foreach (KeyValuePair<string, IClassDef> keyValuePair in _classDefs)
            {
                IClassDef classDef = keyValuePair.Value;
                if (classDef.ClassName == className)
                {
                    return classDef;
                }
            }
            return null;
        }

        #region Singleton ClassDefCol

        ///<summary>
        /// The number of class defs in the collection.
        ///</summary>
        public int Count
        {
            get { return _classDefs.Count; }
        }

        /// <summary>
        /// Returns the existing collection, or creates and returns a 
        /// new empty collection.
        /// </summary>
        /// <returns>A collection of class definitions</returns>
        public static ClassDefCol GetColClassDef()
        {
            if (_instanceFlag)
            {
                return _classDefcol;
            }
            return LoadColClassDef(new ClassDefCol());
        }

        /// <summary>
        /// Returns the existing collection, or creates and returns a 
        /// new empty collection.
        /// </summary>
        /// <param name="classDefCol">A loaded collection of class definitions to initialise the collection with</param>
        /// <returns>A collection of class definitions</returns>
        public static ClassDefCol LoadColClassDef(ClassDefCol classDefCol)
        {
            if (classDefCol == null)
            {
                throw new HabaneroArgumentException("classDefCol", "Cannot load a ClassDefCol if it is null.");
            }
            if (!_instanceFlag || _classDefcol.Count == 0)
            {
                _classDefcol = classDefCol;
                _instanceFlag = true;
            }
            else
            {
                foreach (IClassDef classDef in classDefCol)
                {
                    //ClassDef classDef = (ClassDef)entry.Value;
                    if (!_classDefcol.Contains(classDef))
                    {
                        _classDefcol.Add(classDef);
                    }
                }
            }
            return _classDefcol;
        }

        //public IEnumerator GetEnumerator()
        //{
        //    return _classDefs.Values.GetEnumerator();
        //}

        ///<summary>
        /// Clears the class definitions colleciton.
        ///</summary>
        public void Clear()
        {
            _classDefs.Clear();
        }

        #endregion

        #region TypeId Methods

        private string GetTypeIdForItem(Type key, out bool found)
        {
            string typeId = GetTypeId(key, false);
            found = false;
            if (_classDefs.ContainsKey(typeId))
                found = true;
            else
            {
                typeId = GetTypeId(key, true);
                if (_classDefs.ContainsKey(typeId))
                {
                    found = true;
                }
            }
            return typeId;
        }

        private string GetTypeIdForItem(string assemblyName, string className, out bool found)
        {
            string typeId = GetTypeId(assemblyName, className, false);
            found = false;
            if (_classDefs.ContainsKey(typeId))
                found = true;
            else
            {
                typeId = GetTypeId(assemblyName, className, true);
                if (_classDefs.ContainsKey(typeId))
                {
                    found = true;
                }
            }
            return typeId;
        }

        ///<summary>
        /// This method combines the assembly name and class name to 
        /// create a string that represents the Class Type.
        ///</summary>
        ///<param name="assemblyName">The class's assembly name</param>
        ///<param name="className">The class's name</param>
        ///<returns>A string representing the Class Type.</returns>
        ///<param name="includeNamespace">Should the TypeId include the namespace or not</param>
        internal static string GetTypeId(string assemblyName, string className, bool includeNamespace)
        {
            string namespaceString;
            className = StripOutNameSpace(className, out namespaceString);
            if (includeNamespace && !string.IsNullOrEmpty(namespaceString))
            {
                namespaceString = " Namespace:" + namespaceString;
            }
            else
            {
                namespaceString = "";
            }
            assemblyName = TypeLoader.CleanUpAssemblyName(assemblyName);
            string id = "Assembly:" + assemblyName + namespaceString + " _className:" + className;
            return id.ToUpper();
        }

        ///<summary>
        /// This method returns a string that represents the given Class Type.
        ///</summary>
        ///<param name="classType">The class's Type object.</param>
        ///<returns>A string representing the Class Type.</returns>
        ///<param name="includeNamespace">Should the TypeId include the namespace or not</param>
        private static string GetTypeId(Type classType, bool includeNamespace)
        {
            string assemblyName;
            string className;
            TypeLoader.ClassTypeInfo(classType, out assemblyName, out className );
            return GetTypeId(assemblyName, className, includeNamespace);
        }

        public static string StripOutNameSpace(string className)
        {
            string namespaceString;
            return StripOutNameSpace(className, out namespaceString);
        }

        public static string StripOutNameSpace(string className, out string namespaceString)
        {
            if (className != null)
            {
                int pos = className.LastIndexOf(".");
                if (pos != -1)
                {
                    namespaceString = className.Substring(0, pos);
                    className = className.Substring(pos + 1);
                }
                else
                {
                    namespaceString = "";
                }
            }
            else
            {
                namespaceString = null;
            }
            return className;
        }

        #endregion
    }
}