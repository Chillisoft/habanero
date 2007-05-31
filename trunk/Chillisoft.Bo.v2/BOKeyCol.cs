using System.Collections;

namespace Chillisoft.Bo.v2
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
            //TODO_Err: Add sensible error handling if prop already exists etc
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
                //TODOErr: put appropriate err handling
                return ((BOKey) Dictionary[boKeyName]);
            }
        }
    }
}