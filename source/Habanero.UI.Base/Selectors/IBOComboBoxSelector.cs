namespace Habanero.UI.Base
{
    /// <summary>
    /// Provides an interface that is specialised for showing a collection of 
    /// Business Objects in a <see cref="IComboBox"/> and allowing the user to select one.
    /// </summary>
    public interface IBOComboBoxSelector : IBOSelectorControl, IComboBox
    {
//        ///<summary>
//        /// Returns the control factory used by this selector
//        ///</summary>
//        IControlFactory ControlFactory { get; }
        ///<summary>
        /// Returns the Underlying ComboBoxControl that is used by this selector
        ///</summary>
        IComboBox ComboBox { get; }
    }

}