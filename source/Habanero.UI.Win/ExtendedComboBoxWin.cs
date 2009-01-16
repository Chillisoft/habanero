using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    class ExtendedComboBoxWin : UserControlWin, IExtendedComboBox
    {
        private readonly IControlFactory _controlFactory;
        private readonly IComboBox _comboBox;
        private readonly IButton _button;

        public ExtendedComboBoxWin() : this(GlobalUIRegistry.ControlFactory)
        {
        }

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



        public IComboBox ComboBox
        {
            get { return _comboBox; }
        }

        public IButton Button
        {
            get { return _button; }
        }
    }
}
