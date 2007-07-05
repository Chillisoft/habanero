using System;
using Habanero.Base;
using Habanero.Bo.ClassDefinition;

namespace Habanero.Bo
{
    /// <summary>
    /// Manages a primary key
    /// </summary>
    public class BOPrimaryKey : BOKey
    {
        //		BusinessObjectBase mBO;
        private Guid _newObjectID = Guid.Empty;
        //		bool _isObjectNew;

        /// <summary>
        /// Constructor to initialise a new primary key
        /// </summary>
        /// <param name="lKeyDef">The primary key definition</param>
        internal BOPrimaryKey(PrimaryKeyDef lKeyDef) : base(lKeyDef)
        {
        }

        /// <summary>
        /// Sets the object's ID
        /// </summary>
        /// <param name="id">The ID to set to</param>
        internal virtual void SetObjectID(Guid id)
        {
            //TODO_Err:	check that id is not empty (Eric: is this done below?)

            //If the Business object is not new then you cannot set the objectID
            if (!IsObjectNew)
            {
                throw new InvalidObjectIdException("The property for objectID cannot be null.");
            }
            //If the object id is not already set then set it.
            if (_newObjectID == Guid.Empty)
            {
                _newObjectID = id;
            }
            else if (_newObjectID != id)
            {
                throw new InvalidObjectIdException("The property for objectID cannot be null.");
            }
        }


        //		internal bool IsObjectNew
        //		{
        //			get{return _isObjectNew;}
        //			set{_isObjectNew = value;}
        //		}


        /// <summary>
        /// Returns the object's ID
        /// </summary>
        /// <returns>Returns a string</returns>
        public virtual string GetObjectId()
        {
            if (IsObjectNew && (_newObjectID != Guid.Empty))
            {
                return "ID=" + _newObjectID.ToString();
            }
            else if (!IsObjectNew)
            {
                return PersistedDatabaseWhereClause(null);
            }
            else
            {
                //TODO: This exception breaks tests. Review.
                //throw new InvalidObjectIdException("Error: _isObjectNew = true but the _newObjectID is not set");
                return "";
            }
        }

        /// <summary>
        /// Returns the object ID as if the object had been persisted 
        /// to the database, regardless of whether the object is new or not
        /// </summary>
        /// <returns>Returns a string</returns>
        public virtual string GetObjectNewID()
        {
            return PersistedDatabaseWhereClause(null);
        }

        /// <summary>
        /// Get the original ObjectID
        /// </summary>
        /// <returns>Returns a string</returns>
        internal virtual string GetOrigObjectID()
        {
            if (_newObjectID != Guid.Empty)
            {
                return "ID=" + _newObjectID.ToString();
            }
            else if (IsDirty)
            {
                return PersistedDatabaseWhereClause(null);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Returns a hashcode of the ID
        /// </summary>
        /// <returns>Returns a hashcode integer</returns>
        public override int GetHashCode()
        {
            return GetObjectId().GetHashCode();
        }

        /// <summary>
        /// Indicates whether to check for duplicates.  This is true when the
        /// object is no longer new or when the primary key has not been
        /// changed.
        /// </summary>
        /// <returns>Returns true if duplicates must be checked</returns>
        internal override bool MustCheckKey()
        {
            // if the properties have not been edited then ignore them since
            // they could not now cause a duplicate.

            return IsDirty || IsObjectNew;
        }

        /// <summary>
        /// Returns the key of the super-class
        /// </summary>
        /// <param name="subClassDef">The sub-class definition</param>
        /// <param name="subClassObj">The sub-class</param>
        /// <returns>Returns a BOKey object</returns>
        public static BOKey GetSuperClassKey(ClassDef subClassDef, BusinessObject subClassObj)
        {
            BOKey superKey = subClassDef.SuperClassClassDef.PrimaryKeyDef.CreateBOKey(subClassObj.GetBOPropCol());
            return superKey;
        }

        /// <summary>
        /// Returns the ID as a Guid
        /// </summary>
        /// <returns>Returns a Guid</returns>
        public Guid GetGuid()
        {
            return new Guid(this.GetObjectId().Substring(3, 38));
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
        /// Returns a string containing all the properties and their values
        /// </summary>
        /// <returns>Returns a string</returns>
        public override string ToString()
        {
            return this.KeyName + ":" + base.ToString();
        }

    }
}