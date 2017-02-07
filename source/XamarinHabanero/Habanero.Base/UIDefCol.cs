#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Manages a collection of user interface definitions
    /// </summary>
    public class UIDefCol :  IEnumerable<IUIDef>
    {
        private readonly Dictionary<string, IUIDef> _defs;

        /// <summary>
        /// Constructor to initialise a new empty collection
        /// </summary>
        public UIDefCol()
        {
            //_defs = new Hashtable();
            _defs = new Dictionary<string, IUIDef>();
        }

        /// <summary>
        /// Adds a UI definition to the collection
        /// </summary>
        /// <param name="def">The UI definition to add</param>
        public void Add(IUIDef def)
        {
            if (Contains(def.Name))
            {
                throw new InvalidXmlDefinitionException(String.Format(
                        "A 'ui' definition with the name '{0}' is being added to " +
                        "the collection of ui definitions for the class '{1}', but a " +
                        "definition with that name already exists.  (Note: " +
                        "'default' is the name given to a 'ui' element without " +
                        "a 'name' attribute.)", def.Name, def.ClassName));
            }
            def.UIDefCol = this;
            def.ClassDef = this.ClassDef;
            _defs.Add(def.Name, def);
        }

        /// <summary>
        /// Indicates whether the given ui definition is contained in the
        /// collection
        /// </summary>
        /// <param name="def">The ui definition</param>
        /// <returns>Returns true if contained</returns>
        public bool Contains(IUIDef def)
        {
            return Contains(def.Name);
        }

        /// <summary>
        /// Indicates whether a ui definition with the given name is contained in the
        /// collection
        /// </summary>
        /// <param name="uiDefName">The ui definition name</param>
        /// <returns>Returns true if contained</returns>
        public bool Contains(string uiDefName)
        {
            return _defs.ContainsKey(uiDefName);
        }

        /// <summary>
        /// Removes the specified ui definition from the collection
        /// </summary>
        /// <param name="def">The ui definition to remove</param>
        public void Remove(IUIDef def)
        {
            _defs.Remove(def.Name);
        }

        /// <summary>
        /// Provides an indexing facility so that the contents of the
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="name">The name of the definition to access</param>
        /// <returns>Returns the definition with the name specified</returns>
        public IUIDef this[string name]
        {
            get
            {
                if (!Contains(name))
                {
                    if (name == "default")
                    {
                        throw new HabaneroApplicationException(
                            "No default 'ui' definition exists (a definition with " +
                            "no name attribute).  Check that you have at least one " +
                            "set of 'ui' definitions for the class, or check that " +
                            "you have a default 'ui' definition, or ensure that " +
                            "you have correctly indicated the name of the ui " +
                            "definition you are intending to use.");
                    }
                    throw new HabaneroApplicationException(String.Format(
                                                               "The ui definition with the name '{0}' does not " +
                                                               "exist in the collection of definitions for the " +
                                                               "class '{1}'.", name, this.ClassName));
                }
                return this._defs[name];
            }
        }



        /// <summary>
        /// Returns a count of the number of ui definitions held
        /// in this collection
        /// </summary>
        public int Count
        {
            get { return _defs.Count; }
        }

        private IClassDef _classDef;

        ///<summary>
        /// Returns the class definition for the UIDefCol.
        ///</summary>
        public IClassDef ClassDef
        {
            get { return _classDef; }
            set { _classDef = value;
                foreach (KeyValuePair<string, IUIDef> def in _defs)
                {
                    def.Value.ClassDef = value;
                }}
        }
        /// <summary>
        /// Returns the ClassName of the Class tha this UIDef is for.
        /// </summary>
        public string ClassName
        {
            get { return this.ClassDef == null ? "" : this.ClassDef.ClassName; }
        }

        ///<summary>
        /// Clones the uidefcol.  The new ui defCol will have a clone of each UIGrid and UIForm.
        ///  i.e. this is a deep copy of the uiDefCol
        ///</summary>
        ///<returns></returns>
        public UIDefCol Clone()
        {
            UIDefCol newUIDefCol = new UIDefCol();
            foreach (IUIDef def in this)
            {
                newUIDefCol.Add(def.Clone());
            }
            return newUIDefCol;
        }


        ///<summary>
        ///Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        ///</summary>
        ///
        ///<returns>
        ///true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        ///</returns>
        ///
        ///<param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            UIDefCol otherUIDefCol = obj as UIDefCol;
            if (otherUIDefCol == null) return false;
            if (this.Count != otherUIDefCol.Count) return false;
            foreach (IUIDef uiDef in this)
            {
                bool found = false;
                foreach (IUIDef otherUiDef in otherUIDefCol)
                {
                    if (otherUiDef.Equals(uiDef))
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Hashcode for the UIDefCol is the sum of the hashcodes of the UIDefs it contains
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = 0;
            foreach (IUIDef def in this)
            {
                hashCode += def.GetHashCode();
            }
            return hashCode;
        }

        ///<summary>
        /// overloads the operator == 
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns></returns>
        public static bool operator ==(UIDefCol a, UIDefCol b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b)) return true;

            // If one is null, but not both, return false.
            if (((object) a == null) || ((object) b == null)) return false;

            // Return true if the fields match:
            return a.Equals(b);
        }

        ///<summary>
        /// overloads the operator != 
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns></returns>
        public static bool operator !=(UIDefCol a, UIDefCol b)
        {
            return !(a == b);
        }

        /////<summary>
        ///// Clones the collection of ui columns this performs a copy of all uicolumns but does not copy the uiFormFields.
        /////</summary>
        /////<returns>a new collection that is a shallow copy of this collection</returns>
        //public UIGridColumn Clone()
        //{
        //    UIGridColumn newUIForm = new UIGridColumn(this.Heading,
        //        this.PropertyName, this.GridControlTypeName, this.GridControlAssemblyName,
        //        this.Editable, this.Width, this.Alignment, this.Parameters);
        //    return newUIForm;
        //}


        #region IEnumerable<UIDef> Members
        ///<summary>
        ///Returns an enumerator that iterates through the collection.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>1</filterpriority>
        IEnumerator<IUIDef> IEnumerable<IUIDef>.GetEnumerator()
        {
            return _defs.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members
        /// <summary>
        /// Returns the collection's enumerator
        /// </summary>
        /// <returns>Returns an object of type IEnumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _defs.Values.GetEnumerator();
        }

        #endregion
    }
}