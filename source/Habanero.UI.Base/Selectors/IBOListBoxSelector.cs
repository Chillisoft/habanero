namespace Habanero.UI.Base
{
    /// <summary>
    /// Provides an interface that is specialised for showing a collection of 
    /// Business Objects in a <see cref="IListBox"/> and allowing the user to select one.
    /// </summary>
    public interface IBOListBoxSelector : IBOColSelectorControl, IListBox
    {
        ///<summary>
        /// Returns the Underlying <see cref="IListBox"/> that is used by this selector
        ///</summary>
        IListBox ListBox { get; }
    }
}