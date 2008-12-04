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
        //		BusinessObjectBase mBO;
        private Guid _newObjectID = Guid.Empty;

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
            if (_newObjectID == Guid.Empty)
            {
                _newObjectID = id;
            }
            else if (_newObjectID != id)
            {
                throw new InvalidObjectIdException("The ObjectGuidID has already been set for this object.");
            }
        }

        /// <summary>
        /// Returns the object's ID as a string.
        /// </summary>
        /// <returns>Returns a string representation of the object id</returns>
        public virtual string GetObjectId()
        {
            if (IsObjectNew && (_newObjectID != Guid.Empty))
            {
                return NewObjectID();
            }
            return IsObjectNew ? "" : PersistedDatabaseWhereClause(null);
        }

        /// <summary>
        /// Returns a hashcode of the ID
        /// </summary>
        /// <returns>Returns a hashcode integer</returns>
        public override int GetHashCode()
        {
            if (_newObjectID != Guid.Empty) return NewObjectID().GetHashCode();
            return GetObjectId().GetHashCode();
        }

        private string NewObjectID()
        {
            return "ID=" + _newObjectID;
        }

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
        public static BOKey GetSuperClassKey(ClassDef subClassDef, BusinessObject subClassObj)
        {
            while (subClassDef.SuperClassClassDef.PrimaryKeyDef == null)
            {
                if (subClassDef.SuperClassClassDef == null) return null;

                subClassDef = subClassDef.SuperClassClassDef;
            }
            return subClassDef.SuperClassClassDef.PrimaryKeyDef.CreateBOKey(subClassObj.Props);
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
    }
}