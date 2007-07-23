using System.Collections.Generic;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Provides a form in which a user can select from a combo box,
    /// </summary>
    public class InputFormComboBox
    {
        private readonly string _message;
        protected ComboBox _comboBox;

        /// <summary>
        /// Initialises the form with a message to display to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="options">The List of options to display</param>
        public InputFormComboBox(string message, List<object> options)
        {
            Permission.Check(this);
            _message = message;
            _comboBox = ControlFactory.CreateComboBox();
            options.ForEach(delegate(object obj) { _comboBox.Items.Add(obj); });
        }

        /// <summary>
        /// Sets up the form and makes it visible to the user
        /// </summary>
        /// <returns>Returns a DialogResult object which indicates the user's 
        /// response to the dialog. See System.Windows.Forms.DialogResult for 
        /// more detail.</returns>
        public DialogResult ShowDialog()
        {
            Panel messagePanel = new Panel();
            FlowLayoutManager messagePanelManager = new FlowLayoutManager(messagePanel);
            messagePanelManager.AddControl(ControlFactory.CreateLabel(_message, false));
            messagePanelManager.AddControl(_comboBox );
            messagePanel.Height = _comboBox.Height + 100;
            messagePanel.Width = ControlFactory.CreateLabel(_message, true).PreferredWidth + 20;
            _comboBox.Width = messagePanel.Width - 30;
            return new OKCancelDialog(messagePanel).ShowDialog();
        }

        /// <summary>
        /// Gets or sets the selecteditem
        /// </summary>
        public object SelectedItem
        {
            get { return _comboBox.SelectedItem; }
            set { _comboBox.SelectedItem = value; }
        }
    }
}