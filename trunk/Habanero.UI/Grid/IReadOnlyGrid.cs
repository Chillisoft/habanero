using System;
using Habanero.Bo;

namespace Habanero.Ui.Grid
{
    /// <summary>
    /// Handles the event of a user double-clicking on a row
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
    }
}