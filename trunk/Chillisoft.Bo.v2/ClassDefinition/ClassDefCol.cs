using System;
using System.Collections;

namespace Chillisoft.Bo.ClassDefinition.v2
{
    /// <summary>
    /// Manages a collection of class definitions.
    /// </summary>
    public class ClassDefCol : DictionaryBase
    {
        private static ClassDefCol mClassDefcol;
        private static bool instanceFlag = false;

        /// <summary>
        /// Initialises an empty collection
        /// </summary>
        private ClassDefCol() : base()
        {
        }

        /// <summary>
        /// Returns the existing collection, or creates and returns a 
        /// new empty collection.
        /// </summary>
        /// <returns>A collection of class definitions</returns>
        internal static ClassDefCol GetColClassDef()
        {
            if (! instanceFlag)
            {
                mClassDefcol = new ClassDefCol();
                instanceFlag = true;
                return mClassDefcol;
            }
            else
            {
                return mClassDefcol;
            }
        }

		private string GetTypeIdForItem(Type key, out bool found)
		{
			string typeId = ClassDef.GetTypeId(key, false);
			found = false;
			if (Dictionary.Contains(typeId))
				found = true;
			else
			{
				typeId = ClassDef.GetTypeId(key, true);
				if (Dictionary.Contains(typeId))
				{
					found = true;
				}
			}
			return typeId;
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
				if (found) return (ClassDef)Dictionary[typeId];
				else return null;
			}
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
		public ClassDef this[string assemblyName, string className]
		{
			get
			{
				//TODO: Error if (this.Contains(key))
				return ((ClassDef)Dictionary[ClassDef.GetTypeId(assemblyName, className)]);
			}
		}
		
        /// <summary>
        /// Returns a collection of the key names being stored
        /// </summary>
        internal ICollection Keys
        {
            get { return (Dictionary.Keys); }
        }

        /// <summary>
        /// Returns a collection of the values being stored
        /// </summary>
        internal ICollection Values
        {
            get { return (Dictionary.Values); }
        }

		/// <summary>
		/// Adds a class definition to the collection
		/// </summary>
		/// <param name="value">The class definition to add</param>
		public void Add(ClassDef value)
		{
			Dictionary.Add(value.ClassFullName, value);
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
		/// Indicates whether the collection contains the class definition
		/// that is passed as a parameter.
		/// </summary>
		/// <param name="classDef">The class definition to look for.</param>
		/// <returns>Returns true if found, false if not</returns>
		public bool Contains(ClassDef classDef)
		{
			return (Dictionary.Contains(classDef.ClassFullName));
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
			Dictionary.Remove(typeId);
			//TODO: should this throw an error if it is not found?
		}

		/// <summary>
		/// Removes the specified class definition from the collection.
		/// </summary>
		/// <param name="classDef">The class definition to be removed</param>
		public void Remove(ClassDef classDef)
		{
			Dictionary.Remove(classDef.ClassFullName);
		}

        /// <summary>
        /// Removes a flag that indicates that a collection exists.  After
        /// this flag is removed, calling GetColClassDef will result in a
        /// new empty collection replacing the existing one.
        /// </summary>
        protected void Finalize()
        {
            instanceFlag = false;
        }
    }

    #region "self Tests"

    #endregion
} 