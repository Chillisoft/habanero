using System.Collections.Generic;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Provides a form containing a ComboBox in order to get a single
    /// input value back from a user
    /// </summary>
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

        /// <summary>
        /// Gets the control factory used to create the controls
        /// </summary>
        public IControlFactory ControlFactory
        {
            get { return _controlFactory; }
        }

        /// <summary>
        /// Gets the message to display to the user
        /// </summary>
        public string Message
        {
            get { return _message; }
        }

        /// <summary>
        /// Gets the ComboBox control on the form
        /// </summary>
        public IComboBox ComboBox
        {
            get { return _comboBox; }
        }

        /// <summary>
        /// Gets or sets the selected item in the ComboBox
        /// </summary>
        public object SelectedItem
        {
            get { return _comboBox.SelectedItem; }
            set { _comboBox.SelectedItem = value; }
        }

        /// <summary>
        /// Creates the panel on the form
        /// </summary>
        /// <returns>Returns the panel created</returns>
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

        //this is Currently untestable, the layout has been tested in the createControlPanel method.
        /// <summary>
        /// Shows the form to the user
        /// </summary>
        public DialogResult ShowDialog()
        {
            IPanel panel = createControlPanel();
            IOKCancelDialogFactory okCancelDialogFactory = _controlFactory.CreateOKCancelDialogFactory();
            IFormChilli form = okCancelDialogFactory.CreateOKCancelForm(panel, "");
            return form.ShowDialog();
        }
    }
}