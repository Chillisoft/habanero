using System.Collections;
using System.Windows.Forms;
using Chillisoft.UI.Generic.v2;

namespace Chillisoft.UI.Misc.v2
{
    /// <summary>
    /// Provides a form in which a user can choose one from a number
    /// of radio button options
    /// </summary>
    public class InputBoxRadioButton
    {
        private IList radioButtons;

        /// <summary>
        /// Constructor to intialise a new form
        /// </summary>
        public InputBoxRadioButton()
        {
            radioButtons = new ArrayList();
        }

        /// <summary>
        /// Sets up the form and makes it visible to the user
        /// </summary>
        /// <param name="options">A set of string options for the user
        /// to choose from</param>
        /// <returns>Returns a DialogResult object which indicates the user's 
        /// response to the dialog. See System.Windows.Forms.DialogResult for 
        /// more detail.</returns>
        public DialogResult ShowDialog(string[] options)
        {
            Panel messagePanel = new Panel();
            FlowLayoutManager messagePanelManager = new FlowLayoutManager(messagePanel);
            int maxWidth = 0;
            foreach (string s in options)
            {
                RadioButton rButton = ControlFactory.CreateRadioButton(s);
                if (rButton.Width > maxWidth) maxWidth = rButton.Width;
                messagePanelManager.AddControl(rButton);
                radioButtons.Add(rButton);
            }
            messagePanel.Height = ControlFactory.CreateRadioButton("Test").Height*options.Length + 10;
            messagePanel.Width = maxWidth + 10;
            return new OKCancelDialog(messagePanel).ShowDialog();
        }

        /// <summary>
        /// Returns the currently selected radio option as a string
        /// </summary>
        public string SelectedOption
        {
            get
            {
                foreach (RadioButton radioButton in radioButtons)
                {
                    if (radioButton.Checked)
                    {
                        return radioButton.Text;
                    }
                }
                return "";
            }
        }
    }
}