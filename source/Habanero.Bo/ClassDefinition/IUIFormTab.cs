using System;
using System.Collections;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Provides an interface for a Form tab - that is, a tab that contains one or more columns of fields on it.
    /// </summary>
    public interface IUIFormTab : ICollection
    {
        /// <summary>
        /// Adds a column definition to the collection of definitions
        /// </summary>
        /// <param name="column">The UIFormColumn object</param>
        void Add(IUIFormColumn column);

        /// <summary>
        /// Removes a column definition from the collection of definitions
        /// </summary>
        /// <param name="column">The UIFormColumn object</param>
        void Remove(IUIFormColumn column);

        /// <summary>
        /// Checks if a column definition is in the collection of definitions
        /// </summary>
        /// <param name="column">The UIFormColumn object</param>
        bool Contains(IUIFormColumn column);

        /// <summary>
        /// Provides an indexing facility so that the contents of the definition
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to access</param>
        /// <returns>Returns the property definition at the index position
        /// specified</returns>
        IUIFormColumn this[int index] { get; }

        /// <summary>
        /// Gets and sets the tab name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets and sets the UIFormGrid definition
        /// </summary>
        IUIFormGrid UIFormGrid { set; get; }

        /// <summary>
        /// Returns the <see cref="UIForm"/> that this <see cref="UIFormTab"/> is defined for.
        /// </summary>
        IUIForm UIForm { get; set; }
    }

}