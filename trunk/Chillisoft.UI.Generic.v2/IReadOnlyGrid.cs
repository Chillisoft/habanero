using System;
using Chillisoft.Bo.v2;

namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// Handles the event of a user double-clicking on a row
    /// </summary>
    /// <param name="sender">The object that notified of the event</param>
    /// <param name="e">Attached arguments regarding the event</param>
    public delegate void RowDoubleClickedHandler(Object sender, BusinessObjectEventArgs e);

    /// <summary>
    /// An interface to model a grid that cannot be edited directly
    /// </summary>
    public interface IReadOnlyGrid
    {
        /// <summary>
        /// Gets and sets the currently selected business object
        /// </summary>
        BusinessObjectBase SelectedBusinessObject { set; get; }
        
        /// <summary>
        /// Adds a business object to the collection being represented
        /// </summary>
        /// <param name="bo">The business object to add</param>
        void AddBusinessObject(BusinessObjectBase bo);

        /// <summary>
        /// The event of a row being double-clicked
        /// </summary>
        event RowDoubleClickedHandler RowDoubleClicked;
    }
}