using System;
using Habanero.Bo;
using Habanero.UI.Base;

namespace Habanero.UI.Grid
{
    /// <summary>
    /// Handles the event of a user double-clicking on a row in the grid
    /// </summary>
    /// <param name="sender">The object that notified of the event</param>
    /// <param name="e">Attached arguments regarding the event</param>
    public delegate void RowDoubleClickedHandler(Object sender, BOEventArgs e);

    /// <summary>
    /// An interface to model a grid that cannot be edited directly
    /// </summary>
    public interface IReadOnlyGrid
    {
        /// <summary>
        /// Gets and sets the currently selected business object
        /// </summary>
        BusinessObject SelectedBusinessObject { set; get; }
        
        /// <summary>
        /// Adds a business object to the collection being represented
        /// </summary>
        /// <param name="bo">The business object to add</param>
        void AddBusinessObject(BusinessObject bo);

        /// <summary>
        /// The event of a row being double-clicked
        /// </summary>
        event RowDoubleClickedHandler RowDoubleClicked;

        /// <summary>
        /// Returns the name of the ui definition used, as specified in the
        /// 'name' attribute of the 'ui' element in the class definitions.
        /// By default, no 'name' attribute is specified and the ui name of
        /// "default" is used.  Having a name attribute allows you to choose
        /// between a multiple visual representations of a business object
        /// collection.
        /// </summary>
        /// <returns>Returns the name of the ui definition this grid is using
        /// </returns>
        string UIName { get; }
    }
}