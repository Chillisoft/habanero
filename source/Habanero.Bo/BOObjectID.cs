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
    /// Manages a business object primary key, where the key is the object's ID.
    /// Unlike composite primary keys that are typically composed of properties
    /// that are used in real-world scenarios and can be changed, an ObjectID
    /// is a primary key that won't be used by end users.  It typically acts
    /// discretely behind the user interface.
    /// </summary>
    public class BOObjectID : BOPrimaryKey
    {
        protected IBOProp _objectIDProp;

        /// <summary>
        /// Constructor to initialise a new ObjectID
        /// </summary>
        /// <param name="lPrimaryKeyDef">The primary key definition</param>
        internal BOObjectID(PrimaryKeyDef lPrimaryKeyDef) : base(lPrimaryKeyDef)
        {
            if (lPrimaryKeyDef.Count != 1 || !lPrimaryKeyDef.IsGuidObjectID)
            {
                throw new InvalidObjectIdException(
                    "The BOOBjectID must have a key def that defines exactly one property and that is an ObjectID");
            }
        }

        /// <summary>
        /// Adds a property to the key
        /// </summary>
        /// <param name="BOProp">The property to add</param>
        internal override void Add(IBOProp BOProp)
        {
            if (Count > 0)
            {
                throw new InvalidObjectIdException("A BOObjectID cannot have " +
                    "more than one property.");
            }
            if (BOProp.PropertyType != typeof (Guid))
            {
                throw new InvalidObjectIdException("A BOObjectID cannot have " +
                    "a property of type other than Guid.");
            }

            base.Add(BOProp);
        }

        /// <summary>
        /// Returns the objectID's property
        /// </summary>
        protected IBOProp ObjectIDProp
        {
            get
            {
                if (_objectIDProp == null)
                {
                    _objectIDProp = base[KeyDef.KeyName];
                }
                if (_objectIDProp == null)
                {
                    throw new InvalidObjectIdException("Unable to located the objectIDProp.");
                }
                return _objectIDProp;
            }
        }

        /// <summary>
        /// Sets the objectID
        /// </summary>
        /// <param name="id">The Guid ID to set to</param>
        public override void SetObjectGuidID(Guid id)
        {
            //If the object id is not already set then set it.
            if (ObjectIDProp == null)
            {
                throw new InvalidObjectIdException("The property for ObjectGuidID cannot be null.");
            }
            if (ObjectIDProp.Value == null ||
                (Guid) ObjectIDProp.Value == Guid.Empty)
            {
                ObjectIDProp.Value = id;
            }
            else if ((Guid) ObjectIDProp.Value != id)
            {
                throw new InvalidObjectIdException("The ObjectGuidID has already been set for this object.");
            }
        }

        /// <summary>
        /// Returns the ObjectID as "ID=ObjectIDValue"
        /// </summary>
        /// <returns>Returns a string</returns>
        public override String GetObjectId()
        {
            return "ID=" + ObjectIDProp.PropertyValueString;
        }

        /// <summary>
        /// Indicates whether to check for duplicates. This will always
        /// return false, since the objectID is guaranteed to be unique.
        /// </summary>
        /// <returns>Returns false</returns>
        internal override bool IsDirtyOrNew()
        {
            return false;
        }

        /// <summary>
        /// Returns the ID as a Guid
        /// </summary>
        /// <returns>Returns a Guid</returns>
        public override Guid GetAsGuid()
        {
            return (Guid)ObjectIDProp.Value;
        }

        #region Operator == Overloads

        /// <summary>
        /// Indicates if a BOObjectID has the same value as a given Guid
        /// </summary>
        /// <param name="lhs">The ObjectID to compare</param>
        /// <param name="rhs">The Guid to compare</param>
        /// <returns>Returns true if the arguments are equal</returns>
        public static bool operator ==(BOObjectID lhs, Guid rhs)
        {
            return ((Guid) lhs.ObjectIDProp.Value == rhs);
        }

        /// <summary>
        /// Indicates if a BOObjectID has a different value to a given Guid
        /// </summary>
        /// <param name="lhs">The ObjectID to compare</param>
        /// <param name="rhs">The Guid to compare</param>
        /// <returns>Returns true if the arguments differ</returns>
        public static bool operator !=(BOObjectID lhs, Guid rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Indicates if two BOObjectID objects have the same value
        /// </summary>
        /// <param name="lhs">The first ObjectID to compare</param>
        /// <param name="rhs">The second ObjectID to compare</param>
        /// <returns>Returns true if the ObjectID's are equal</returns>
        public static bool operator ==(BOObjectID lhs, BOObjectID rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// Indicates if two BOObjectID objects have different values
        /// </summary>
        /// <param name="lhs">The first ObjectID to compare</param>
        /// <param name="rhs">The second ObjectID to compare</param>
        /// <returns>Returns true if the ObjectID's differ</returns>
        public static bool operator !=(BOObjectID lhs, BOObjectID rhs)
        {
            return !(lhs == rhs);
        }

        #endregion //Operator Overloads

        #region Object method overloads

        /// <summary>
        /// Indicates if a specified BOObjectID has the same value as this one
        /// </summary>
        /// <param name="obj">The BOObjectID to compare with</param>
        /// <returns>Returns true if equal</returns>
        public override bool Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is BOObjectID)
            {
                return (this.GetObjectId() == ((BOObjectID) obj).GetObjectId());
            }

            return false;
        }

        /// <summary>
        /// Returns a hashcode of the ObjectID
        /// </summary>
        /// <returns>Returns a hashcode integer</returns>
        public override int GetHashCode()
        {
            return this.GetObjectId().GetHashCode();
        }

        #endregion //Object method overloads
                
    }
}