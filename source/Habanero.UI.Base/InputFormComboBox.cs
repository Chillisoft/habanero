using System.Collections.Generic;

namespace Habanero.UI.Base
{
    public class InputFormComboBox
    {
        private readonly IControlFactory _controlFactory;
        private readonly string _message;
        private IComboBox _comboBox;

        public InputFormComboBox(IControlFactory controlFactory, string message, List<object> choices)
        {
            _controlFactory = controlFactory;
            _message = message;
            _comboBox = _controlFactory.CreateComboBox();
            choices.ForEach(delegate(object item) { _comboBox.Items.Add(item); });
        }

        public IControlFactory ControlFactory
        {
            get { return _controlFactory; }
        }

        public string Message
        {
            get { return _message; }
        }

        public IComboBox ComboBox
        {
            get { return _comboBox; }
        }

        public object SelectedItem
        {
            get { return _comboBox.SelectedItem; }
            set { _comboBox.SelectedItem = value; }
        }

        public IPanel createControlPanel()
        {
            IPanel panel = _controlFactory.CreatePanel();
            ILabel label = _controlFactory.CreateLabel(_message, false);
            FlowLayoutManager flowLayoutManager = new FlowLayoutManager(panel, _controlFactory);
            flowLayoutManager.AddControl(label);
            flowLayoutManager.AddControl(_comboBox);
            panel.Height = _comboBox.Height + label.Height;
            panel.Width = _controlFactory.CreateLabel(_message, true).PreferredWidth + 20;
            _comboBox.Width = panel.Width - 30;
            return panel;
        }
    }
}