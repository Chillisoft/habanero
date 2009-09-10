// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
    /// <summary>
    /// Manages a primary key, which is a collection of its properties, each
    /// holding runtime values.  The description of the primary key structure is held
    /// in PrimaryKeyDef.
    /// </summary>
    public class BOPrimaryKey : BOKey, IPrimaryKey
    {
        /// <summary>
        /// The object identifier for this BOPrimaryKey
        /// </summary>
        protected Guid _objectID = Guid.Empty;
        private string _currentValue;
        private string _previousValue;

        /// <summary>
        /// Constructor to initialise a new primary key
        /// </summary>
        /// <param name="lKeyDef">The primary key definition</param>
        internal BOPrimaryKey(PrimaryKeyDef lKeyDef) : base(lKeyDef)
        {
        }

        /// <summary>
        /// Sets the object's Guid ID
        /// </summary>
        /// <param name="id">The ID to set to</param>
        public virtual void SetObjectGuidID(Guid id)
        {
            //If the Business object is not new then you cannot set the objectID
            if (!IsObjectNew)
            {
                throw new InvalidObjectIdException("The ObjectGuidID cannot be set for an object that is not new.");
            }
            //If the object id is not already set then set it.
            if (_objectID == Guid.Empty)
            {
                _objectID = id;
            }
            else if (_objectID != id)
            {
                throw new InvalidObjectIdException("The ObjectGuidID has already been set for this object.");
            }
            _objectID = id;
        }

        /// <summary>
        /// Returns the object's ID as a string.
        /// </summary>
        /// <returns>Returns a string representation of the object id</returns>
        public virtual string GetObjectId()
        {
            if (IsObjectNew && (_objectID != Guid.Empty))
            {
                return _objectID.ToString();
            }
            return IsObjectNew ? "" : AsString_LastPersistedValue();
        }

        /// <summary>
        /// Returns a hashcode of the ID
        /// </summary>
        /// <returns>Returns a hashcode integer</returns>
        public override int GetHashCode()
        {
            return ObjectID.GetHashCode();
        }

        /// <summary>
        /// Event handler for handling the event of a property belonging to this key being updated.
        /// </summary>
        protected override void BOPropUpdated_Handler(object sender, BOPropEventArgs e)
        {
            _previousValue = _currentValue;
            _currentValue = AsString_CurrentValue();
            FireValueUpdated();
        }
        //internal string NewObjectID()
        //{
        //    return  _objectID.ToString();
        //}

        /// <summary>
        /// Indicates whether to check for duplicates.  This is true when the
        /// object is no longer new or when the primary key has not been
        /// changed.
        /// </summary>
        /// <returns>Returns true if duplicates must be checked</returns>
        internal override bool IsDirtyOrNew()
        {
            // if the properties have not been edited then ignore them since
            // they could not now cause a duplicate.

            return IsDirty || IsObjectNew;
        }

        /// <summary>
        /// Returns the primary key of the super-class.  If not found, it
        /// searches higher up the Hierarchy and returns the higher primary
        /// key or null if none is found.
        /// </summary>
        /// <param name="subClassDef">The class definition to search on</param>
        /// <param name="subClassObj">The business object</param>
        /// <returns>Returns a BOKey object or null</returns>
        public static IBOKey GetSuperClassKey(ClassDef subClassDef, BusinessObject subClassObj)
        {
            PrimaryKeyDef primaryKeyDef = (PrimaryKeyDef) subClassDef.SuperClassClassDef.PrimaryKeyDef;
            while (primaryKeyDef == null)
            {
                if (subClassDef.SuperClassClassDef == null) return null;

                subClassDef = subClassDef.SuperClassClassDef;
            }
            return primaryKeyDef.CreateBOKey(subClassObj.Props);
        }

        /// <summary>
        /// Returns the ID as a Guid if it can
        /// </summary>
        /// <returns>Returns a Guid</returns>
        public virtual Guid GetAsGuid()
        {
            string objectId = this.GetObjectId();
            objectId = objectId.TrimEnd('\'','}');
            string guidString = objectId.Substring(objectId.Length - 36);
            Guid guid = new Guid(guidString);
            return guid;
        }

        /// <summary>
        /// Indicates whether the key provided is equal to this key
        /// </summary>
        /// <param name="obj">The key to compare with</param>
        /// <returns>Returns true if equal</returns>
        public override bool  Equals(Object obj)
        {
            if (obj is BOKey)
            {
                return (this == (BOPrimaryKey)obj);
            }

            return false;
        }
        
        /// <summary>
        /// Returns true if the primary key is a propery the object's ID, that is,
        /// the primary key is a single discrete property that is an immutable Guid and serves as the ID.
        /// </summary>
        public bool IsGuidObjectID
        {
            get { return ((PrimaryKeyDef) this.KeyDef).IsGuidObjectID; }
        }

        /// <summary>
        /// Returns the ID as a Value. 
        /// <li>"In cases where the <see cref="IBusinessObject"/> has an ID with a single 
        ///    property this will return the value of the property.
        /// </li>
        /// <li>"In cases where the <see cref="IBusinessObject"/>  has an ccomposite ID 
        ///    (i.e. with more than one property) this will return a list with the values of the properties.
        /// </li>
        /// </summary>
        /// <returns>Returns an object</returns>
        public object GetAsValue()
        {
            IBOPropCol boPropCol = this.GetBOPropCol();
            foreach (BOProp  boProp in boPropCol)
            {
                //HACK: This is really wierd code the boPropCol does not have an int accessor.
                // there is therefore no way to get hte first item in the col other than this.
                if (boPropCol.Count == 1)
                {
                    return boProp.Value;
                }
//                list.Add(boProp.PropDef.ClassDef.ClassName + "." + boProp.PropertyName + "=" + boProp.Value);
            }
            return this.AsString_CurrentValue();
        }

        ///<summary>
        /// Returns true if the primary key is a composite Key (i.e. if it consists of more than one property)
        ///</summary>
        public bool IsCompositeKey
        {
            get { return this.Count > 1; }
        }

        ///<summary>
        /// The globally unique object identifier for the object that this Primary Key represents. 
        /// This is the implementation of a fundamental Object Oriented concept 
        /// that every object should be globally uniquely identifiable.
        /// The value returned from this property will be the actual value of the primary key property 
        /// for objects with a <see cref="Guid"/> id, or it will be a newly created <see cref="Guid"/> 
        /// for objects with composite or non-guid primary keys.
        ///</summary>
        public virtual Guid ObjectID
        {
            get { return _objectID; }
        }

        ///<summary>
        /// Returns the Previous Object ID this is only for new objects that are assigned
        ///   an object id and then loaded from the database and the object is is updated to the 
        ///   value from the database. The previous Object ID is then used by the object manager,
        ///   collection, dataset provider to update the ID for the object.
        ///</summary>
        public Guid PreviousObjectID
        {
            get { return _objectID; }
        }
        ///<summary>
        /// For a given value e.g. a Guid Identifier '{......}' this will build up a primary key object that can be used to
        /// load the business object from the Data store (see Business Object loader GetBusinessObjectByValue)
        /// This can only be used for business objects that have a single property for the primary key
        /// (i.e. non composite primary keys)
        ///</summary>
        ///<param name="classDef">The Class definition of the Business Object to load</param>
        ///<param name="idValue">The value of the primary key of the business object</param>
        ///<returns>the BOPrimaryKey if this can be constructed else returns null</returns>
        public static BOPrimaryKey CreateWithValue(ClassDef classDef, object idValue)
        {
            PrimaryKeyDef primaryKeyDef = (PrimaryKeyDef) classDef.PrimaryKeyDef;
            if (primaryKeyDef.IsCompositeKey) return null;

            IBOPropCol boPropCol = classDef.CreateBOPropertyCol(true);
            BOPrimaryKey boPrimaryKey = primaryKeyDef.CreateBOKey(boPropCol) as BOPrimaryKey;
            if (boPrimaryKey != null)
            {
                boPrimaryKey[0].Value = idValue;
            }
            return boPrimaryKey;
        }

        ///<summary>
        /// For a given value e.g. a Guid Identifier '{......}' this will build up a primary key object that can be used to
        /// load the business object from the Data store <see cref="BusinessObjectLoaderBase.GetBusinessObjectByValue(Type,object)"/>
        /// This can only be used for business objects that have a single property for the primary key
        /// (i.e. non composite primary keys)
        ///</summary>
        ///<param name="type">The type of business object to be loaded</param>
        ///<param name="idValue">The value of the primary key of the business object</param>
        ///<returns>the BOPrimaryKey if this can be constructed else returns null</returns>
        public static BOPrimaryKey CreateWithValue(Type type, object idValue)
        {
            ClassDef classDef = (ClassDef) ClassDef.ClassDefs[type];
            return CreateWithValue(classDef, idValue);
        }

        /// <summary>
        /// Returns a string containing all the properties and their values
        /// </summary>
        /// <returns>Returns a string</returns>
        public override string AsString_CurrentValue()
        {
            if (AllPropValuesAreNonNull()) return base.AsString_CurrentValue();
            if (IsObjectNew && (_objectID != Guid.Empty))
            {
                return _objectID.ToString();
            } 
            return base.AsString_CurrentValue();
        }

        /// <summary>
        /// Returns a string containing all the properties and their values,
        /// but using the values held before the last time they were edited.  This
        /// method differs from AsString_LastPersistedValue in that the properties may have
        /// been edited several times since their last persistence.
        /// </summary>
        public override string AsString_PreviousValue()
        {
            if (string.IsNullOrEmpty(_previousValue))
            {
                if (AllPropPreviousValuesAreNonNull()) return base.AsString_PreviousValue();
                if (IsObjectNew && (_objectID != Guid.Empty))
                {
                    return _objectID.ToString();
                }
                return base.AsString_PreviousValue();
            }
            return _previousValue;
        }

        private bool AllPropValuesAreNonNull()
        {
            foreach (IBOProp prop in this)
            {
                if (prop.Value == null) return false;
            }
            return true;
        }

        private bool AllPropPreviousValuesAreNonNull()
        {
            foreach (IBOProp prop in this)
            {
                if (prop.ValueBeforeLastEdit == null) return false;
            }
            return true;
        }

        ///<summary>
        /// Returns the previous objects Id
        ///</summary>
        ///<returns></returns>
        public string GetPreviousObjectID()
        {
            return "";
        }
    }
}
