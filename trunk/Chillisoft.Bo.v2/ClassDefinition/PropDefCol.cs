using System;
using System.Collections;
using Chillisoft.Bo.v2;

namespace Chillisoft.Bo.ClassDefinition.v2
{
    /// <summary>
    /// Provides a collection of property definitions.
    /// </summary>
    public class PropDefCol : DictionaryBase
    {
        /// <summary>
        /// A constructor to create a new empty collection
        /// </summary>
        public PropDefCol() : base()
        {
        }

        /// <summary>
        /// Provides an indexing facility for the collection so that items
        /// in the collection can be accessed like an array 
        /// (e.g. collection["surname"])
        /// </summary>
        public PropDef this[string key]
        {
            get
            {
                //TODOErr: put appropriate err handling
                //if (this.Contains(key))
                return ((PropDef) Dictionary[key.ToUpper()]);

                //else

                //return new PropDef("","",key,;
                //	Throw (New Exception( obj.PropertyName + " is already in this BOProperty Collection",   "obj", e));
            }
        }

        /// <summary>
        /// Returns a collection of the key names being stored
        /// </summary>
        public ICollection Keys
        {
            get { return (Dictionary.Keys); }
        }

        /// <summary>
        /// Returns a collection of the values being stored
        /// </summary>
        public ICollection Values
        {
            get { return (Dictionary.Values); }
        }

        /// <summary>
        /// Add an existing property definition to the collection
        /// </summary>
        /// <param name="propDef">The existing property definition</param>
        public void Add(PropDef propDef)
        {
            Dictionary.Add(propDef.PropertyName.ToUpper(), propDef);
        }

        /// <summary>
        /// Create a new property definition and add it to the collection
        /// </summary>
        /// <param name="propName">The name of the property, e.g. surname</param>
        /// <param name="propType">The type of the property, e.g. string</param>
        /// <param name="propRWStatus">Rules for how a property can be
        /// accessed. See PropReadWriteRule enumeration for more detail.</param>
        /// <param name="databaseFieldName">The database field name - this
        /// allows you to have a database field name that is different to the
        /// property name, which is useful for migrating systems where
        /// the database has already been set up</param>
        /// <param name="defaultValue">The default value that a property 
        /// of a new object will be set to</param>
        /// <returns>Returns the new definition created, after it has
        /// been added to the collection</returns>
        public PropDef Add(string propName,
                           Type propType,
                           PropReadWriteRule propRWStatus,
                           string databaseFieldName,
                           object defaultValue)
        {
            PropDef lPropDef = new PropDef(propName, propType, propRWStatus,
                                           databaseFieldName, defaultValue);
            Dictionary.Add(lPropDef.PropertyName.ToUpper(), lPropDef);
            return lPropDef;
        }

        /// <summary>
        /// Creates and adds a new property definition as before, but 
        /// assumes the database field name is the same as the property name.
        /// </summary>
        public PropDef Add(string propName,
                           Type propType,
                           PropReadWriteRule propRWStatus,
                           object defaultValue)
        {
            PropDef lPropDef = new PropDef(propName, propType, propRWStatus,
                                           defaultValue);
            Dictionary.Add(lPropDef.PropertyName.ToUpper(), lPropDef);
            return lPropDef;
        }

        /// <summary>
        /// Indicates if a property definition with the given key exists
        /// in the collection.
        /// </summary>
        /// <param name="key">The key to match</param>
        /// <returns>Returns true if found, false if not</returns>
        public bool Contains(string key)
        {
            return (Dictionary.Contains(key.ToUpper()));
        }

        /// <summary>
        /// Creates a business object property collection that mirrors
        /// this one.  The new collection will contain a BOProp object for
        /// each PropDef object in this collection, with that BOProp object
        /// storing an instance of the PropDef object.
        /// </summary>
        /// <param name="newObject">Whether the new BOProps in the
        /// collection will be new objects. See PropDef.CreateBOProp
        /// for more info.</param>
        /// <returns>Returns the new BOPropCol object</returns>
        public BOPropCol CreateBOPropertyCol(bool newObject)
        {
            BOPropCol lBOPropertyCol = new BOPropCol();
            PropDef lPropDef;
            foreach (DictionaryEntry item in this)
            {
                lPropDef = (PropDef) item.Value;
                lBOPropertyCol.Add(lPropDef.CreateBOProp(newObject));
            }
            return lBOPropertyCol;
        }
    }
}