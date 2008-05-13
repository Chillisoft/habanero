using System;

namespace Habanero.UI.Base
{
    public interface IButtonGroupControl:IChilliControl
    {
        /// <summary>
        /// Adds a new button to the control by the name specified
        /// </summary>
        /// <param name="buttonName">The name to appear on the button</param>
        /// <returns>Returns the Button object created</returns>
        IButton AddButton(string buttonName);
        /// <summary>
        /// A facility to index the buttons in the control so that they can
        /// be accessed like an array (eg. button["name"])
        /// </summary>
        /// <param name="buttonName">The name of the button</param>
        /// <returns>Returns the button found by that name, or null if not
        /// found</returns>
        IButton this[string buttonName] { get; }
        /// <summary>
        /// Sets the default button in this control that would be chosen
        /// if the user pressed Enter without changing the focus
        /// </summary>
        /// <param name="buttonName">The name of the button</param>
        void SetDefaultButton(string buttonName);

        IButton AddButton(string buttonName, EventHandler clickHandler);
    }
}