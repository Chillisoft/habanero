using System;
using System.Collections.Generic;

namespace Habanero.Base
{
    public interface IBOKey : IEnumerable<IBOProp>
    {
        /// <summary>
        /// Indicates that the value held by one of the properties in the
        /// key has been changed
        /// </summary>
        event EventHandler<BOKeyEventArgs> Updated;

        /// <summary>
        /// Returns true if a property with this name is part of this key
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns true if it is contained</returns>
        bool Contains(string propName);

        /// <summary>
        /// Returns the number of BOProps in this key.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the key definition for this key
        /// </summary>
        IKeyDef KeyDef
        {
            get;
        }

        /// <summary>
        /// Returns a string containing all the properties and their values,
        /// but using the values at last persistence rather than any dirty values
        /// </summary>
        /// <returns>Returns a string</returns>
        string PersistedValueString();

        /// <summary>
        /// Returns a string containing all the properties and their values,
        /// but using the values held before the last time they were edited.  This
        /// method differs from PersistedValueString in that the properties may have
        /// been edited several times since their last persistence.
        /// </summary>
        string PropertyValueStringBeforeLastEdit();

        /// <summary>
        /// Provides an indexing facility so the properties can be accessed
        /// with square brackets like an array
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns the matching BOProp object or null if not found
        /// </returns>
        IBOProp this[string propName] { get;  }

        
        /// <summary>
        /// Provides an indexing facility so the properties can be accessed
        /// with square brackets like an array
        /// </summary>
        /// <param name="index">The index position of the item to retrieve</param>
        /// <returns>Returns the matching BOProp object or null if not found
        /// </returns>
        IBOProp this[int index] { get;  }

        /// <summary>
        /// Creates a "where" clause from the persisted properties held
        /// </summary>
        /// <param name="sql">The sql statement used to generate and track
        /// parameters</param>
        /// <returns>Returns a string</returns>
        string PersistedDatabaseWhereClause(ISqlStatement sql);

        /// <summary>
        /// Returns a copy of the collection of properties in the key
        /// </summary>
        /// <returns>Returns a new BOProp collection</returns>
        BOPropCol GetBOPropCol();
    }
}