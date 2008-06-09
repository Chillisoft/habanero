//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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

using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.BO.ClassDefinition
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
        public override void Add(IPropDef propDef)
        {
            if (Count > 0 && mIsObjectID)
            {
                throw new InvalidPropertyException("You cannot have more than one " +
                    "property for a primary key that represents an object's ID");
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
        /// Returns true if the primary key is also the object's ID, that is,
        /// the primary key is a single discrete property that serves as the ID
        /// </summary>
        public bool IsObjectID
        {
            get { return mIsObjectID; }
            set { mIsObjectID = value; }
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
            foreach (PropDef lPropDef in this)
            {
                lBOKey.Add(lBOPropCol[lPropDef.PropertyName]);
            }
            return lBOKey;
        }
    }
}