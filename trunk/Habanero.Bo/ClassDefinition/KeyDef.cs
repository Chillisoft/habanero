using System;
using System.Collections;
using Habanero.Base.Exceptions;
using Habanero.Bo;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Bo.ClassDefinition
{
    /// <summary>
    /// The KeyDef is a definition of a Business Objects key.
    /// It is essentially a key name and a collection of property 
    /// definitions that place certain limitations on the data
    /// that the key can hold.  The property definitions can also relate
    /// together in some way (e.g. for a composite alternate 
    /// key, the combination of properties is required to be unique).
    /// </summary>
    public class KeyDef : DictionaryBase
    {
        protected bool _ignoreIfNull = false;
        protected string _keyName = "";
        protected bool _buildKeyName = true; //this is a flag used to 
                //indicate whether the the keyname should be built up 
				//from the property names or not

		#region Constructors

		/// <summary>
		/// A deparameterised constructor that causes the keyName to be
		/// set as a concatenation of the PropDef.PropertyNames separated
		/// by an underscore.
		/// </summary>
		public KeyDef()
			: this("")
		{
		}

		/// <summary>
        /// Constructor that initialises the object with the name
        /// of the key.
        /// </summary>
        /// <param name="keyName">The name of the key.  If the name is
        /// null or a zero-length string, then the keyName will be 
        /// a concatenation of the PropDef.PropertyNames separated by
        /// an underscore.</param>
        public KeyDef(string keyName)
        {
            //TODO_Err check that keyName is valid. Eric: what is a valid keyname?
            KeyName = keyName;
        }

		#endregion Constructors

		#region Properties

		/// <summary>
		/// A method used by BOKey to determine whether to check for
		/// duplicate keys.  It will always check if either
		/// IgnoreIfNull is set to false or if it there are no null
		/// properties.<br/>
		/// NOTE: If the BOKey is a primary key, then this cannot be
		/// set to true.
		/// </summary>
		public virtual bool IgnoreIfNull
		{
			get { return _ignoreIfNull; }
			set { _ignoreIfNull = value; }
		}

		/// <summary>
		/// Returns the key name for this key definition
		/// </summary>
		public string KeyName
		{
			get { return _keyName; }
			protected internal set
			{
				_keyName = value;
				if (_keyName == null || _keyName.Length == 0)
				{
					_keyName = "";
					_buildKeyName = true;
				}
			}
		}

		#endregion Properties

		#region Dictionary Methods

        /// <summary>
        /// Provides an indexing facility for the collection of property
        /// definitions that belong to the key, so that items
        /// in the collection can be accessed like an array 
        /// (e.g. collection["surname"])
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns the property stored under that property name</returns>
        internal PropDef this[string propName]
        {
            get
            {
                if (!Dictionary.Contains(propName))
                {
                    throw new InvalidPropertyNameException(String.Format(
                        "In a key definition, no property with the name '{0}' " +
                        "exists in the collection of properties.",propName));
                }
                return ((PropDef) Dictionary[propName]);
            }
        }

        /// <summary>
        /// Adds a property definition to the collection of definitions.
        /// The object to be added cannot be null.  If a key name was not
        /// originally provided at instantiation of the key definition, then
        /// the new property definition's name will be appended to the key name.
        /// </summary>
        /// <param name="propDef">The PropDef object to add</param>
        /// <exeption cref="HabaneroArgumentException">Will throw an exception
        /// if the argument is null</exeption>
        public virtual void Add(PropDef propDef)
        {
            if (propDef == null)
            {
                throw new HabaneroArgumentException("lPropDef",
                                                   "ClassDef-Add. You cannot add a null prop def to a classdef");
            }
            if (!Contains(propDef))
            {
                Dictionary.Add(propDef.PropertyName, propDef);
                if (_buildKeyName)
                {
                    if (_keyName.Length > 0)
                    {
                        _keyName += "_";
                    }
                    _keyName += propDef.PropertyName;
                }
            }
        }

		/// <summary>
		/// Removes a Property definition from the key
		/// </summary>
		/// <param name="propDef">The Property Definition to remove</param>
		protected void Remove(PropDef propDef)
		{
			if (Contains(propDef))
			{
				base.Dictionary.Remove(propDef.PropertyName);
			}
		}

		/// <summary>
		/// Indicates if the specified property definition exists
		/// in the key.
		/// </summary>
		/// <param name="propDef">The Property definition to search for</param>
		/// <returns>Returns true if found, false if not</returns>
		protected bool Contains(PropDef propDef)
		{
			return (Dictionary.Contains(propDef.PropertyName));
		}

        /// <summary>
        /// Returns true if the key definition holds a property definition
        /// with the name provided.
        /// </summary>
        /// <param name="propName">The property name in question</param>
        /// <returns>Returns true if found or false if not</returns>
        internal bool Contains(string propName)
        {
            return (Dictionary.Contains(propName));
		}

		#endregion Dictionary Methods
		
		/// <summary>
        /// Indicates whether the key definition is valid based on
        /// whether it contains one or more properties.
        /// </summary>
        /// <returns>Returns true if so, or false if no properties are
        /// stored</returns>
        internal virtual bool IsValid()
        {
            return (Count > 0);
        }

		/// <summary>
        /// Creates a new business object key (BOKey) using this key
        /// definition and its property definitions
        /// </summary>
        /// <param name="lBOPropCol">The master property collection</param>
        /// <returns>Returns a new BOKey object that mirrors this
        /// key definition</returns>
        public virtual BOKey CreateBOKey(BOPropCol lBOPropCol)
        {
            BOKey lBOKey = new BOKey(this);
            foreach (DictionaryEntry item in this)
            {
                PropDef lPropDef = (PropDef) item.Value;
                lBOKey.Add(lBOPropCol[lPropDef.PropertyName]);
            }
            return lBOKey;
        }

    }



}