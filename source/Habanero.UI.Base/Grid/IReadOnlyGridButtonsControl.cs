using System;

namespace Habanero.UI.Base
{
    public interface IReadOnlyGridButtonsControl:IControlChilli
    {
        /// <summary>
        /// Returns the button with buttonName
        /// </summary>
        IButton this[string buttonName] { get; }
        event EventHandler DeleteClicked;
        /// <summary>
        /// Fires when the delete button is clicked
        /// </summary>
        event EventHandler AddClicked;
        /// <summary>
        /// Fires when the Add button is clicked
        /// </summary>
        event EventHandler EditClicked;
        /// <summary>
        /// Fires when the Edit button is clicked
        /// </summary>
        bool ShowDefaultDeleteButton { get; set; }
    }
}