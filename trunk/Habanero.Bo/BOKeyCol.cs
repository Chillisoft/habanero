using System;
using System.Collections;
using Habanero.Base.Exceptions;

namespace Habanero.Bo
{
    /// <summary>
    /// Manages a collection of BOKey objects
    /// </summary>
    public class BOKeyCol : DictionaryBase
    {
        /// <summary>
        /// Constructor to initialise a new empty collection
        /// </summary>
        internal BOKeyCol() : base()
        {
        }

        /// <summary>
        /// Adds a key to the collection
        /// </summary>
        /// <param name="lBOKey">The BO key</param>
        internal void Add(BOKey lBOKey)
        {
            if (Dictionary.Contains(lBOKey.KeyName))
            {
                throw new InvalidKeyException(String.Format(
                    "A key with the name '{0}' is being added to a key " +
                    "collection but already exists in the collection.",
                    lBOKey.KeyName));
            }
            base.Dictionary.Add(lBOKey.KeyName, lBOKey);
        }

        /// <summary>
        /// Copies all the keys held in another collection into this collection
        /// </summary>
        /// <param name="keyCol">The other collection</param>
        internal void Add(BOKeyCol keyCol)
        {
            foreach (DictionaryEntry entry in keyCol)
            {
                this.Add((BOKey) (entry.Value));
            }
        }

        //		private bool Contains( string key )  
        //		{
        //			return( Dictionary.Contains( key ) );
        //		}

        /// <summary>
        /// Provides an indexing facility so that this collection can
        /// be accessed with square brackets like an array
        /// </summary>
        /// <param name="boKeyName">The key name</param>
        /// <returns>Returns the BOKey object found with that name, or null
        /// if nothing of that name is matched</returns>
        internal BOKey this[string boKeyName]
        {
            get
            {
                if (!Dictionary.Contains(boKeyName))
                {
                    throw new InvalidKeyException(String.Format(
                        "The key with the name '{0}' does not exist in the " +
                        "collection of keys.", boKeyName));
                }
                return ((BOKey) Dictionary[boKeyName]);
            }
        }
    }
}