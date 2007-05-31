using System;
using System.Collections;
using Chillisoft.Bo.v2;

namespace Chillisoft.Bo.ClassDefinition.v2
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
            mIgnoreNulls = false; //you cannot ingnore nulls for a primary key.
        }

        /// <summary>
        /// Adds a property definition to this key
        /// </summary>
        /// <param name="lPropDef">The property definition to add</param>
        public override void Add(PropDef lPropDef)
        {
            if (Count > 0 && mIsObjectID)
                //TODO_Err: Raise appropriate Error
            {
                Console.WriteLine("You cannot have more than one " +
                                  "property for a primary key that represents and object id");
            }
            base.Add(lPropDef);
        }

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
        /// IgnoreNulls is set to false or if it encounters null
        /// properties.<br/>
        /// NOTE: Because this is a primary key, a warning will be sent to
        /// the console if you try to set this to true
        /// </summary>
        public override bool IgnoreNulls
        {
            get { return false; }
            set
            {
                if (value)
                    //TODO:Raise appropriate error.
                {
                    Console.WriteLine("Error occured since you cannot set primary key to ignore nulls");
                }
            }
        }

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