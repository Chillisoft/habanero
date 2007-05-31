using System.Collections.Generic;
using System.Windows.Forms;
using Chillisoft.UI.Generic.v2;

namespace Chillisoft.UI.Misc.v2
{
    /// <summary>
    /// Provides a form in which a user can select from a combo box,
    /// </summary>
    public class InputBoxComboBox
    {
        private readonly string itsMessage;
        protected ComboBox itsComboBox;

        /// <summary>
        /// Initialises the form with a message to display to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="options">The List of options to display</param>
        public InputBoxComboBox(string message, List<object> options)
        {
            itsMessage = message;
            itsComboBox = ControlFactory.CreateComboBox();
            options.ForEach(delegate(object obj) { itsComboBox.Items.Add(obj); });
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
            messagePanelManager.AddControl(ControlFactory.CreateLabel(itsMessage, false));
            messagePanelManager.AddControl(itsComboBox );
            messagePanel.Height = itsComboBox.Height + 100;
            messagePanel.Width = ControlFactory.CreateLabel(itsMessage, true).PreferredWidth + 20;
            itsComboBox.Width = messagePanel.Width - 30;
            return new OKCancelDialog(messagePanel).ShowDialog();
        }

        /// <summary>
        /// Gets or sets the selecteditem
        /// </summary>
        public object SelectedItem
        {
            get { return itsComboBox.SelectedItem; }
            set { itsComboBox.SelectedItem = value; }
        }
    }
}