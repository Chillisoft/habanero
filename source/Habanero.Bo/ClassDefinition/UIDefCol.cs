using System;
using System.Collections;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Manages a collection of user interface definitions
    /// </summary>
    public class UIDefCol : IEnumerable
    {
        private Hashtable _defs;

        /// <summary>
        /// Constructor to initialise a new empty collection
        /// </summary>
        public UIDefCol()
        {
            _defs = new Hashtable();
        }

        /// <summary>
        /// Adds a UI definition to the collection
        /// </summary>
        /// <param name="def">The UI definition to add</param>
        public void Add(UIDef def)
        {
            if (_defs.Contains(def.Name))
            {
                throw new InvalidXmlDefinitionException(String.Format(
                                                            "A 'ui' definition with the name '{0}' is being added to " +
                                                            "the collection of ui definitions for the class, but a " +
                                                            "definition with that name already exists.  (Note: " +
                                                            "'default' is the name given to a 'ui' element without " +
                                                            "a 'name' attribute.)", def.Name));
            }
            _defs.Add(def.Name, def);
        }

        /// <summary>
        /// Indicates whether the given ui definition is contained in the
        /// collection
        /// </summary>
        /// <param name="def">The ui definition</param>
        /// <returns>Returns true if contained</returns>
        public bool Contains(UIDef def)
        {
            return _defs.ContainsKey(def.Name);
        }

        /// <summary>
        /// Removes the specified ui definition from the collection
        /// </summary>
        /// <param name="def">The ui definition to remove</param>
        public void Remove(UIDef def)
        {
            _defs.Remove(def.Name);
        }

        /// <summary>
        /// Provides an indexing facility so that the contents of the
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="name">The name of the definition to access</param>
        /// <returns>Returns the definition with the name specified</returns>
        public UIDef this[string name]
        {
            get
            {
                if (!_defs.Contains(name))
                {
                    if (name == "default")
                    {
                        throw new HabaneroApplicationException(
                            "No default 'ui' definition exists (a definition with " +
                            "no name attribute).  Check that you have at least one " +
                            "set of 'ui' definitions for the class, or check that " +
                            "you have a default 'ui' definition, or ensure that " +
                            "you have correctly indicated the name of the ui " +
                            "definition you are intending to use.");
                    }
                    else
                    {
                        throw new HabaneroApplicationException(String.Format(
                                                                   "The ui definition with the name '{0}' does not " +
                                                                   "exist in the collection of definitions for the " +
                                                                   "class.", name));
                    }
                }
                return (UIDef) this._defs[name];
            }
        }

        /// <summary>
        /// Returns the collection's enumerator
        /// </summary>
        /// <returns>Returns an object of type IEnumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return _defs.GetEnumerator();
        }
    }
}