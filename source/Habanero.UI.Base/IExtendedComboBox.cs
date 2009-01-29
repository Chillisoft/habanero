namespace Habanero.UI.Base
{
    ///<summary>
    /// A <see cref="ComboBox"/> with a <see cref="Button"/> next to it on the right with a '...' displayed as the text.
    ///</summary>
    public interface IExtendedComboBox : IControlHabanero
    {
        ///<summary>
        /// Returns the <see cref="ComboBox"/> in the control
        ///</summary>
        IComboBox ComboBox { get; }

        ///<summary>
        /// Returns the <see cref="Button"/> in the control
        ///</summary>
        IButton Button { get; }
    }
}