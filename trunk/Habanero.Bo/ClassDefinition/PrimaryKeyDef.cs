using System;
using System.Collections;
using Habanero.Bo;

namespace Habanero.Bo.ClassDefinition
{
    /// <summary>
    /// Manages the definition of the primary key in a data set
    /// </summary>
    public class PrimaryKeyDef : KeyDef
    {
        private bool mIsObjectID = true;

        /// <summary>
        /// Constructor to create a new primary key definition
        /// </summary>
        public PrimaryKeyDef() : base()
        {
            _ignoreIfNull = false; //you cannot ingnore nulls for a primary key.
        }

        /// <summary>
        /// Adds a property definition to this key
        /// </summary>
        /// <param name="propDef">The property definition to add</param>
        public override void Add(PropDef propDef)
        {
            if (Count > 0 && mIsObjectID)
            {
                throw new InvalidPropertyException("You cannot have more than one " +
                    "property for a primary key that represents an object id");
            }
            base.Add(propDef);
        }

		///// <summary>
		///// Removes a Property definition from the key
		///// </summary>
		///// <param name="propDef">The Property Definition to remove</param>
		//protected void Remove(PropDef propDef)
		//{
		//    if (Dictionary.Contains(propDef.PropertyName))
		//    {
		//        base.Dictionary.Remove(propDef.PropertyName);
		//    }
		//}

		#region Properties

        /// <summary>
        /// Returns true if the Primary Key is also the object ID
        /// </summary>
        public bool IsObjectID
        {
            get { return mIsObjectID; }
            set { mIsObjectID = value; }
        }

        /// <summary>
        /// A method used by BOKey to determine whether to check for
        /// duplicate keys.  It will always check if either
        /// IgnoreIfNull is set to false or if it encounters null
        /// properties.<br/>
        /// NOTE: Because this is a primary key, a warning will be sent to
        /// the console if you try to set this to true
        /// </summary>
        public override bool IgnoreIfNull
        {
            get { return false; }
            set
            {
                if (value)
                {
                    throw new InvalidKeyException("Error occured since you " +
                        "cannot set primary key to ignore nulls");
                }
            }
		}

        #endregion Properties


		/// <summary>
        /// Creates a new business object key (BOKey) using this key
        /// definition and its property definitions. Creates either a new
        /// BOObjectID object (if the primary key is the object ID) 
        /// or a BOPrimaryKey object.
        /// </summary>
        /// <param name="lBOPropCol">The master property collection</param>
        /// <returns>Returns a new BOKey object that mirrors this
        /// key definition</returns>
        public override BOKey CreateBOKey(BOPropCol lBOPropCol)
        {
            BOPrimaryKey lBOKey;
            if (mIsObjectID)
            {
                lBOKey = new BOObjectID(this);
            }
            else
            {
                lBOKey = new BOPrimaryKey(this);
            }
            foreach (DictionaryEntry item in this)
            {
                PropDef lPropDef = (PropDef) item.Value;
                lBOKey.Add(lBOPropCol[lPropDef.PropertyName]);
            }
            return lBOKey;
        }
    }
}