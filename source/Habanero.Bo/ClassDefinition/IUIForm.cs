using System.Collections;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// An interface describing a form consisting of one or more <see cref="IUIFormTab"/> objects.  
    /// Implemented by <see cref="UIForm"/>.
    /// </summary>
    public interface IUIForm : ICollection
    {
        /// <summary>
        /// Adds a tab to the form
        /// </summary>
        /// <param name="tab">A UIFormTab object</param>
        void Add(IUIFormTab tab);

        /// <summary>
        /// Removes a tab from the form
        /// </summary>
        /// <param name="tab">A UIFormTab object</param>
        void Remove(IUIFormTab tab);

        /// <summary>
        /// Checks if the form contains the specified tab
        /// </summary>
        /// <param name="tab">A UIFormTab object</param>
        bool Contains(IUIFormTab tab);

        /// <summary>
        /// Provides an indexing facility so that the contents of the definition
        /// collection can be accessed with square brackets like an array
        /// </summary>
        /// <param name="index">The index position to access</param>
        /// <returns>Returns the property definition at the index position
        /// specified</returns>
        IUIFormTab this[int index] { get; }

        /// <summary>
        /// Gets and sets the width
        /// </summary>
        int Width { set; get; }

        /// <summary>
        /// Gets and sets the height
        /// </summary>
        int Height { set; get; }

        /// <summary>
        /// Gets and sets the heading
        /// </summary>
        string Title { set; get; }

        ///<summary>
        /// The UI Def that this UIForm is related to.
        ///</summary>
        IUIDef UIDef { get; set; }

    }
}