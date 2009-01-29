using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    ///<summary>
    /// A <see cref="ComboBox"/> with a <see cref="Button"/> next to it on the right with a '...' displayed as the text.
    ///</summary>
    public class ExtendedComboBoxWin : UserControlWin, IExtendedComboBox
    {
        private readonly IControlFactory _controlFactory;
        private readonly IComboBox _comboBox;
        private readonly IButton _button;

        ///<summary>
        /// Constructs the <see cref="ExtendedComboBoxWin"/> with the default <see cref="IControlFactory"/>.
        ///</summary>
        public ExtendedComboBoxWin() : this(GlobalUIRegistry.ControlFactory) { }

        ///<summary>
        /// Constructs the <see cref="ExtendedComboBoxWin"/> with the specified <see cref="IControlFactory"/>.
        ///</summary>
        public ExtendedComboBoxWin(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            IUserControlHabanero userControlHabanero = this;
            _comboBox = _controlFactory.CreateComboBox();
            _button = _controlFactory.CreateButton("...");
            BorderLayoutManager borderLayoutManager = controlFactory.CreateBorderLayoutManager(userControlHabanero);
            borderLayoutManager.AddControl(_comboBox, BorderLayoutManager.Position.Centre);
            borderLayoutManager.AddControl(_button, BorderLayoutManager.Position.East);
        }


        ///<summary>
        /// Returns the <see cref="IExtendedComboBox.ComboBox"/> in the control
        ///</summary>
        public IComboBox ComboBox
        {
            get { return _comboBox; }
        }

        ///<summary>
        /// Returns the <see cref="IExtendedComboBox.Button"/> in the control
        ///</summary>
        public IButton Button
        {
            get { return _button; }
        }
    }
}
