using System.Collections;

namespace Habanero.UI.Base
{
    public interface IControlCollection : IEnumerable
    {
        /// <summary>
        /// An indexing facility for the collection so that it can be
        /// accessed like an array with square brackets
        /// </summary>
        /// <param name="index">The numerical index position</param>
        /// <returns>Returns the control at the position specified</returns>
        IControlChilli this[int index] { get;  }

        int Count { get; }

        /// <summary>
        /// Adds a control to the collection
        /// </summary>
        /// <param name="value">The control to add</param>
        /// <returns>Returns the position at which the control was added</returns>
        void Add(IControlChilli value);

        /// <summary>
        /// Provides the index position of the control specified
        /// </summary>
        /// <param name="value">The control to search for</param>
        /// <returns>Returns the index position if found, or -1</returns>
        int IndexOf(IControlChilli value);

        /// <summary>
        /// Insert a control at a specified index position
        /// </summary>
        /// <param name="index">The index position at which to insert</param>
        /// <param name="value">The control to insert</param>
        void Insert(int index, IControlChilli value);

        /// <summary>
        /// Removes the specified control from the collection
        /// </summary>
        /// <param name="value">The control to remove</param>
        void Remove(IControlChilli value);

        /// <summary>
        /// Indicates whether the collection contains the specified control
        /// </summary>
        /// <param name="value">The control to search for</param>
        /// <returns>Returns a boolean indicating whether that control is 
        /// found in the collection</returns>
        bool Contains(IControlChilli value);

        void Clear();
    }
}