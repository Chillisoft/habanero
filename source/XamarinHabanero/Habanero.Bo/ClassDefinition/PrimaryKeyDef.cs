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

using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.Loaders;
using Habanero.Util;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Manages the definition of the primary key in a for a particular Business Object (e.g. Customer).
    /// The Primary Key Definition defins the properties of the object that are used to map the business object
    ///  to the database. The Primary key def is a mapping that is used to implement the 
    ///  Identity Field (216) Pattern (Fowler - 'Patterns of Enterprise Application Architecture')
    /// 
    /// In most cases the PrimaryKeyDefinition will only have one property definition and this property definition 
    ///  will be for an immutable property. In the ideal case this property definition will
    ///  represent a property that is globally unique. In these cases the primaryKeyDef will have the flag mIsGUIDObjectID set to true. 
    ///  However we have in many cases had to extend or replace existing systems
    ///  that use mutable composite keys to identify objects in the database. The primary key definition allows you to define
    ///  all of these scenarious.
    /// The Application developer should not usually deal with this class since it is usually created based on the class definition modelled
    ///   and stored in the ClassDef.Xml <see cref="XmlPrimaryKeyLoader"/>.
    /// </summary>
    public class PrimaryKeyDef : KeyDef, IPrimaryKeyDef
    {
        private bool _isGuidObjectID = true;

        /// <summary>
        /// Constructor to create a new primary key definition
        /// </summary>
        public PrimaryKeyDef()
        {
            _ignoreIfNull = false; //you cannot ingnore nulls for a primary key.
        }

        /// <summary>
        /// Adds a property definition to this key
        /// </summary>
        /// <param name="propDef">The property definition to add</param>
        /// <exception cref="InvalidPropertyException">Thrown if the primary key definition is marked as and object id but more than one 
        /// property definition is being added</exception>
        public override void Add(IPropDef propDef)
        {
            if (Count > 0 && _isGuidObjectID)
            {
                throw new InvalidPropertyException("You cannot have more than one " +
                    "property for a primary key that represents an object's Guid ID");
            }
            base.Add(propDef);
        }

		#region Properties

        /// <summary>
        /// Returns true if the primary key is a propery the object's ID, that is,
        /// the primary key is a single discrete property that is an immutable Guid and serves as the ID.
        /// </summary>
        public bool IsGuidObjectID
        {
            get { return _isGuidObjectID; }
            set { _isGuidObjectID = value; }
        }

        /// <summary>
        /// This overridden method will always return false and will prevent the
        /// value being set to true, since primary keys cannot have null
        /// properties.
        /// </summary>
        public override bool IgnoreIfNull
        {
            get { return false; }
            set
            {
                if (value)
                {
                    throw new InvalidKeyException("Error occured since you " +
                        "cannot set a primary key's IgnoreIfNull setting to true.");
                }
            }
		}

        #endregion Properties


		/// <summary>
        /// Creates a new business object key (BOKey) using this key
        /// definition and its property definitions. Creates either a new
        /// BOObjectID object (if the primary key is the object's ID) 
        /// or a BOPrimaryKey object.
        /// </summary>
        /// <param name="lBOPropCol">The master property collection</param>
        /// <returns>Returns a new BOKey object that mirrors this
        /// key definition</returns>
        public override IBOKey CreateBOKey(IBOPropCol lBOPropCol)
        {
            BOPrimaryKey lBOKey = _isGuidObjectID ? new BOObjectID(this) : new BOPrimaryKey(this);
            foreach (PropDef lPropDef in this)
            {
                lBOKey.Add(lBOPropCol[lPropDef.PropertyName]);
            }
            return lBOKey;
        }

        ///<summary>
        /// Returns true if the primary key is a composite Key (i.e. if it consists of more than one property)
        ///</summary>
        public bool IsCompositeKey
        {
            get { return this.Count > 1; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            string toStringValue = "";
            foreach (PropDef lPropDef in this)
            {
                toStringValue =StringUtilities.AppendMessage(toStringValue, lPropDef.PropertyName, "_");
            }
            return toStringValue;
        }
    }
}