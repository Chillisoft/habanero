using System.Collections;
using Habanero.Base.Exceptions;
using Habanero.Bo;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Bo.ClassDefinition
{
    /// <summary>
    /// The key definition that is being related to in a 
    /// relationship between objects
    /// </summary>
    /// TODO ERIC - review
    public class RelKeyDef : DictionaryBase
    {
        /// <summary>
        /// Constructor to create a new RelKeyDef object
        /// </summary>
        public RelKeyDef() : base()
        {
        }

        /// <summary>
        /// Provides an indexing facility for the property definitions
        /// in this key definition so that they can be 
        /// accessed like an array (e.g. collection["surname"])
        /// </summary>
        /// <param name="propName">The name of the property</param>
        /// <returns>Returns the corresponding RelPropDef object</returns>
        public RelPropDef this[string propName]
        {
            get
            {
                //if (this.Contains(key)) //TODO_Err: If this does not exist
                return ((RelPropDef) Dictionary[propName]);
            }
        }

        /// <summary>
        /// Adds the related property definition to this key, as long as
        /// a property by that name has not already been added.
        /// </summary>
        /// <param name="relPropDef">The RelPropDef object to be added.</param>
        /// <exception cref="HabaneroArgumentException">Thrown if the
        /// argument passed is null</exception>
        public virtual void Add(RelPropDef relPropDef)
        {
            if (relPropDef == null)
            {
				throw new HabaneroArgumentException("relPropDef",
                                                   "ClassDef-Add. You cannot add a null prop def to a classdef");
            }
            if (!Contains(relPropDef))
            {
                Dictionary.Add(relPropDef.OwnerPropertyName, relPropDef);
            }
        }

		/// <summary>
		/// Removes a Related Property definition from the key
		/// </summary>
		/// <param name="relPropDef">The Related Property Definition to remove</param>
		protected void Remove(RelPropDef relPropDef)
		{
			if (Contains(relPropDef))
			{
				base.Dictionary.Remove(relPropDef.OwnerPropertyName);
			}
		}

        /// <summary>
        /// Returns true if the specified property is found.
        /// </summary>
		/// <param name="relPropDef">The Related Property Definition to search for</param>
		/// <returns>Returns true if found, false if not</returns>
		internal protected bool Contains(RelPropDef relPropDef)
        {
			return (Dictionary.Contains(relPropDef.OwnerPropertyName));
        }

        /// <summary>
        /// Returns true if a property with this name is part of this key.
        /// </summary>
        /// <param name="propName">The property name to search by</param>
        /// <returns>Returns true if found, false if not</returns>
        internal bool Contains(string propName)
        {
            return (Dictionary.Contains(propName));
        }

        /// <summary>
        /// Create a relationship key based on this key definition and
        /// its associated property definitions
        /// </summary>
        /// <param name="lBoPropCol">The collection of properties</param>
        /// <returns>Returns the new RelKey object</returns>
        public RelKey CreateRelKey(BOPropCol lBoPropCol)
        {
            RelKey lRelKey = new RelKey(this);
            foreach (DictionaryEntry item in this)
            {
                RelPropDef relPropDef = (RelPropDef) item.Value;

                lRelKey.Add(relPropDef.CreateRelProp(lBoPropCol));
            }
            return lRelKey;
        }
    }


}