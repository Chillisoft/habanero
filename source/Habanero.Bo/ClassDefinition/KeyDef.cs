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
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// The KeyDef is a definition of a Business Objects key.
    /// It is essentially a key name and a collection of property 
    /// definitions that place certain limitations on the data
    /// that the key can hold.  The property definitions can also relate
    /// together in some way (e.g. for a composite/compound alternate 
    /// key, the combination of properties is required to be unique).
    /// </summary>
    public class KeyDef : IKeyDef
    {
        private readonly Dictionary<string, IPropDef> _propDefs;
        /// <summary>
        /// Ignore the Alternate Key constraint if one or more of the properties has a null value.
        /// </summary>
        protected bool _ignoreIfNull;
        /// <summary>
        /// The Name of the Alternate Key.
        /// </summary>
        protected string _keyName = "";
        protected string _keyNameBuilt = "";
        /// <summary>
        /// The Key Name to dispay to the user
        /// </summary>
        protected string _keyNameForDisplay = "";
        /// <summary>
        /// The message to dispaly to the user in the case of a failure.
        /// </summary>
        protected string _message;
        protected bool _buildKeyName = true; //this is a flag used to 
                //indicate whether the the keyname should be built up 
				//from the property names or not

		#region Constructors

		/// <summary>
		/// A deparameterised constructor that causes the keyName to be
		/// set as a concatenation of the PropDef.PropertyNames separated
		/// by an underscore.
		/// </summary>
		public KeyDef(): this("")
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
            //if (string.IsNullOrEmpty(keyName)) throw new ArgumentNullException("keyName");
		    _propDefs = new Dictionary<string, IPropDef>();
            KeyName = keyName;
        }

		#endregion Constructors

		#region Properties

		/// <summary>
		/// A method used by BOKey to determine whether to check for
		/// duplicate keys.  If true, then the uniqueness check will be ignored
		/// if any of the properties making up the key are null.<br/>
		/// NNB: If the BOKey is a primary key, then this cannot be
		/// set to true.
		/// </summary>
		public virtual bool IgnoreIfNull
		{
			get { return _ignoreIfNull; }
			set { _ignoreIfNull = value; }
		}

		/// <summary>
		/// Returns the key name for this key definition - this key name is built
		/// up through a combination of the key name and the property names
		/// </summary>
		public string KeyName
		{
			get
			{
			    return _buildKeyName ? _keyNameBuilt : _keyName;
			}
		    set
			{
				_keyName = value;
			    KeyNameForDisplay = value;
                if (String.IsNullOrEmpty(_keyName))
				{
					_keyName = "";
				    KeyNameForDisplay = "";
					_buildKeyName = true;
				}
                UpdateKeyNameBuildt();
			}
		}

        private void UpdateKeyNameBuildt()
        {
            if (_buildKeyName)
            {
                List<string> propNames = new List<string>();
                if (!String.IsNullOrEmpty(_keyName))
                {
                    propNames.Add(_keyName);
                }
                foreach (KeyValuePair<string, IPropDef> propDef in _propDefs)
                {
                    propNames.Add(propDef.Value.PropertyName);
                }
                string newKeyName = String.Join("_", propNames.ToArray());
                _keyNameBuilt = newKeyName;
            } else
            {
                _keyNameBuilt = _keyName;
            }
        }

        /// <summary>
        /// Returns just the key name as given by the user
        /// </summary>
        public string KeyNameForDisplay
        {
            get { return _keyNameForDisplay; }
            set { _keyNameForDisplay = value; }
        }

        /// <summary>
        /// Gets and sets the message to show to the user if a key validation
        /// fails.  A default message will be provided if this is null.
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
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
        public IPropDef this[string propName]
        {
            get
            {
                if (!Contains(propName))
                {
                    throw new InvalidPropertyNameException(String.Format(
                        "In a key definition, no property with the name '{0}' " +
                        "exists in the collection of properties.",propName));
                }
                return _propDefs[propName];
            }
        }


        /// <summary>
        /// Provides an indexing facility for the collection of property
        /// definitions that belong to the key, so that items
        /// in the collection can be accessed like an array. The order is
        /// always the same, but not determinable
        /// </summary>
        /// <param name="index">The index of the property</param>
        /// <returns>Returns the property stored under that index</returns>
        public IPropDef this[int index]
        {
            get
            {
                int i = 0;
                foreach (KeyValuePair<string, IPropDef> propDef in _propDefs)
                {
                    if (i == index) return propDef.Value;
                    i++;
                }
                throw new ArgumentOutOfRangeException("index", index, "index out of range");
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
        public virtual void Add(IPropDef propDef)
        {
            if (propDef == null)
            {
                throw new HabaneroArgumentException("lPropDef",
                                                   "ClassDef-Add. You cannot add a null prop def to a classdef");
            }
            if (!Contains(propDef))
            {
                _propDefs.Add(propDef.PropertyName, propDef);
                UpdateKeyNameBuildt();
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
				_propDefs.Remove(propDef.PropertyName);
                UpdateKeyNameBuildt();
			}
		}

		/// <summary>
		/// Indicates if the specified property definition exists
		/// in the key.
		/// </summary>
		/// <param name="propDef">The Property definition to search for</param>
		/// <returns>Returns true if found, false if not</returns>
		protected bool Contains(IPropDef propDef)
		{
			return (_propDefs.ContainsKey(propDef.PropertyName));
		}

        /// <summary>
        /// Returns true if the key definition holds a property definition
        /// with the name provided.
        /// </summary>
        /// <param name="propName">The property name in question</param>
        /// <returns>Returns true if found or false if not</returns>
        internal bool Contains(string propName)
        {
            return (_propDefs.ContainsKey(propName));
		}

        /// <summary>
        /// Returns a count of the number of property definitions held
        /// in this key definition
        /// </summary>
        public int Count
        {
            get { return _propDefs.Count; }
        }

        protected internal void Clear()
        {
            _propDefs.Clear();
            UpdateKeyNameBuildt();
        }

		#endregion Dictionary Methods
		
		/// <summary>
        /// Indicates whether the key definition is valid based on
        /// whether it contains one or more properties. 
        /// I.e. The key is not valid if it does not contain any properties.
        /// </summary>
        /// <returns>Returns true if so, or false if no properties are
        /// stored</returns>
        internal virtual bool IsValid()
        {
            return (_propDefs.Count > 0);
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
            foreach (IPropDef lPropDef in _propDefs.Values)
            {
                lBOKey.Add(lBOPropCol[lPropDef.PropertyName]);
            }
            return lBOKey;
        }

		#region IEnumerable<PropDef> Members

        ///<summary>
        ///Returns an enumerator that iterates through the collection.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>1</filterpriority>
        IEnumerator<IPropDef> IEnumerable<IPropDef>.GetEnumerator()
		{
			return _propDefs.Values.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

        ///<summary>
        ///Returns an enumerator that iterates through a collection.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
		{
			return _propDefs.Values.GetEnumerator();
		}

		#endregion

        
    }



}