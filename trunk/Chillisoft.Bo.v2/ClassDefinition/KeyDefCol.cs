using System.Collections;
using Chillisoft.Bo.v2;

namespace Chillisoft.Bo.ClassDefinition.v2
{
    /// <summary>
    /// Maintains a collection of key definitions (KeyDef objects)
    /// </summary>
    public class KeyDefCol : DictionaryBase
    {
        /// <summary>
        /// A basic constructor that sets up an empty collection
        /// </summary>
        public KeyDefCol() : base()
        {
        }

        /// <summary>
        /// Adds a key definition to the collection
        /// </summary>
        /// <param name="lKeyDef"></param>
        public void Add(KeyDef lKeyDef)
        {
            //TODO_Err: Add sensible error handling if prop already exists etc
            base.Dictionary.Add(lKeyDef.KeyName, lKeyDef);
        }

        /// <summary>
        /// Provides an indexing facility for the collection so that items
        /// in the collection can be accessed like an array 
        /// (e.g. collection["surname"])
        /// </summary>
        /// <param name="keyName">The name of the key definition</param>
        /// <returns>Returns the definition matching the name
        /// provided or null if none are found</returns>
        internal KeyDef this[string keyName]
        {
            get
            {
                //TODOErr: put appropriate err handling
                return ((KeyDef) Dictionary[keyName]);
            }
        }

        /// <summary>
        /// Creates a new collection of business object keys (BOKey)
        /// using the key definitions in this collection.
        /// </summary>
        /// <param name="lBOPropCol">The collection of properties</param>
        /// <returns>Returns a new BOKey collection object containing a mirror
        /// of this key definition collection</returns>
        public BOKeyCol CreateBOKeyCol(BOPropCol lBOPropCol)
        {
            BOKeyCol lBOKeyCol = new BOKeyCol();
            KeyDef lKeyDef;
            foreach (DictionaryEntry item in this)
            {
                lKeyDef = (KeyDef) item.Value;
                lBOKeyCol.Add(lKeyDef.CreateBOKey(lBOPropCol));
            }
            return lBOKeyCol;
        }
    }
}