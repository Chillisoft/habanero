//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.Util;
using Habanero.Util.File;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Manages a collection of class definitions.
    /// </summary>
    public class ClassDefCol : IEnumerable<ClassDef>
    {
        private static ClassDefCol _classDefcol;
        private static bool _instanceFlag = false;
        private Dictionary<string, ClassDef> _classDefs;

		/// <summary>
		/// Initialises an empty collection
		/// </summary>
		internal protected ClassDefCol()
			: base()
		{
		    _classDefs = new Dictionary<string, ClassDef>();
		}
		
		/// <summary>
		/// Provides an indexing facility for the collection so that items
		/// in the collection can be accessed like an array 
		/// (e.g. collection["surname"])
		/// </summary>
		/// <param name="key">The name of the class definition</param>
		/// <returns>Returns the class definition that matches the key
		/// or null if none is found</returns>
		public ClassDef this[Type key]
		{
			get
			{
				bool found;
				string typeId = GetTypeIdForItem(key, out found);
				if (found) return _classDefs[typeId];
				else return null;
				//TODO error: When converted to use generic collection then 
				// an error (KeyNotFoundException) should be thrown if the item is not in the collection?
			}
		}

        //public ClassDef Get<T>()
        //    where T : BusinessObject
        //{
        //    return this[typeof(T)];
        //}

		/// <summary>
		/// Provides an indexing facility for the collection so that items
		/// in the collection can be accessed like an array 
		/// (e.g. collection["surname"])
		/// </summary>
		/// <param name="assemblyName">The name of the class assembly</param>
		/// <param name="className">The name of the class</param>
		/// <returns>Returns the class definition that matches the key
		/// or null if none is found</returns>
		public ClassDef this[string assemblyName, string className]
		{
			get
			{
				bool found;
				string typeId = GetTypeIdForItem(assemblyName, className, out found);
				if (found) return _classDefs[typeId];
				else return null;
				//TODO error: When converted to use generic collection then 
				// an error (KeyNotFoundException) should be thrown if the item is not in the collection?
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

		/// <summary>
		/// Adds a class definition to the collection
		/// </summary>
		/// <param name="value">The class definition to add</param>
		public void Add(ClassDef value)
		{
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
        /// Indicates whether the collection contains a class definition
        /// representing the passed type.
        /// </summary>
        /// <param name="key">The name of the class definition</param>
        /// <returns>Returns true if found, false if not</returns>
		public bool Contains(Type key)
        {
			bool found;
			string typeId = GetTypeIdForItem(key, out found);
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
			string typeId = GetTypeIdForItem(assemblyName,className, out found);
			return found;
		}

		/// <summary>
		/// Indicates whether the collection contains the class definition
		/// that is passed as a parameter.
		/// </summary>
		/// <param name="classDef">The class definition to look for.</param>
		/// <returns>Returns true if found, false if not</returns>
		public bool Contains(ClassDef classDef)
		{
			bool found;
			string typeId = GetTypeIdForItem(classDef.AssemblyName, classDef.ClassName, out found);
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
			//TODO error: When converted to use generic collection then 
			// an the value of found should be returned (method type should be bool).
		}

		/// <summary>
		/// Removes the specified class definition from the collection.
		/// </summary>
		/// <param name="classDef">The class definition to be removed</param>
		public void Remove(ClassDef classDef)
		{
			bool found;
			string typeId = GetTypeIdForItem(classDef.AssemblyName, classDef.ClassName, out found);
			if (found) _classDefs.Remove(typeId);
			//TODO error: When converted to use generic collection then 
			// an the value of found should be returned (method type should be bool).
		}

        /// <summary>
        /// Removes a flag that indicates that a collection exists.  After
        /// this flag is removed, calling LoadColClassDef will result in a
        /// new empty collection replacing the existing one.
        /// </summary>
        protected void Finalize()
        {
            _instanceFlag = false;
        }

		#region Singleton ClassDefCol

		/// <summary>
		/// Returns the existing collection, or creates and returns a 
		/// new empty collection.
		/// </summary>
		/// <returns>A collection of class definitions</returns>
		internal static ClassDefCol GetColClassDef()
		{
			if (_instanceFlag)
			{
				return _classDefcol;
			} else
			{
				return LoadColClassDef(new ClassDefCol());
				//TODO: Is throwing an error correct? Maybe return null?
				//throw new Generic.HabaneroApplicationException(
				//    "The Class Definitions cannot be accessed before they have been loaded.");
			}
		}

		/// <summary>
		/// Returns the existing collection, or creates and returns a 
		/// new empty collection.
		/// </summary>
		/// <param name="classDefCol">A loaded collection of class definitions to initialise the collection with</param>
		/// <returns>A collection of class definitions</returns>
		internal static ClassDefCol LoadColClassDef(ClassDefCol classDefCol)
		{
			if (classDefCol == null)
			{
				throw new HabaneroArgumentException("classDefCol", "Cannot load a ClassDefCol if it is null.");
			}
			if (!_instanceFlag || _classDefcol.Count == 0)
			{
				_classDefcol = classDefCol;
				_instanceFlag = true;
			} else
			{
                foreach (ClassDef classDef in classDefCol)
				{
                    //ClassDef classDef = (ClassDef)entry.Value;
					if (!_classDefcol.Contains(classDef))
					{
						_classDefcol.Add(classDef);
					} else
					{
						//TODO: Shouldn't this be removed?
						Console.Out.WriteLine("Attempted to load a class def when it was already defined.");
					}
				}
			}
			return _classDefcol;
		}

        public int Count
        {
            get { return _classDefs.Count; }
        }

		//public IEnumerator GetEnumerator()
		//{
		//    return _classDefs.Values.GetEnumerator();
		//}

        public void Clear()
        {
            _classDefs.Clear();
            
        }

		#endregion

		#region TypeId Methods

		private string GetTypeIdForItem(Type key, out bool found)
		{
			string typeId = ClassDefCol.GetTypeId(key, false);
			found = false;
			if (_classDefs.ContainsKey(typeId))
				found = true;
			else
			{
				typeId = ClassDefCol.GetTypeId(key, true);
                if (_classDefs.ContainsKey(typeId))
				{
					found = true;
				}
			}
			return typeId;
		}

		private string GetTypeIdForItem(string assemblyName, string className, out bool found)
		{
			string typeId = ClassDefCol.GetTypeId(assemblyName, className, false);
			found = false;
            if (_classDefs.ContainsKey(typeId))
				found = true;
			else
			{
				typeId = ClassDefCol.GetTypeId(assemblyName, className, true);
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
			if (includeNamespace && namespaceString != null && namespaceString.Length > 0)
			{
				namespaceString = " Namespace:" + namespaceString;
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
    	internal static string GetTypeId(Type classType, bool includeNamespace)
    	{
    		if (includeNamespace)
				return GetTypeId(classType.Assembly.ManifestModule.ScopeName, classType.FullName, includeNamespace);
    		else
				return GetTypeId(classType.Assembly.ManifestModule.ScopeName, classType.Name, includeNamespace);
    	}

		internal static string StripOutNameSpace(string className)
		{
			string namespaceString;
			return StripOutNameSpace(className, out namespaceString);
		}

		internal static string StripOutNameSpace(string className, out string namespaceString)
		{
			if (className != null)
			{
				int pos = className.LastIndexOf(".");
				if (pos != -1)
				{
					namespaceString = className.Substring(0, pos);
					className = className.Substring(pos + 1);
				}else
				{
					namespaceString = "";
				}
			} else
			{
				namespaceString = null;
			}
			return className;
		}

		#endregion

    	#region IEnumerable<ClassDef> Members

    	IEnumerator<ClassDef> IEnumerable<ClassDef>.GetEnumerator()
    	{
			return _classDefs.Values.GetEnumerator(); 
    	}

    	#endregion

    	#region IEnumerable Members

    	IEnumerator IEnumerable.GetEnumerator()
    	{
    		return _classDefs.Values.GetEnumerator(); 
    	}

    	#endregion
    }

    #region "self Tests"

    #endregion
} 