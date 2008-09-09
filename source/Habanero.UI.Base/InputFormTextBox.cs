namespace Habanero.UI.Base
{
    /// <summary>
    /// Provides a form containing a TextBox in order to get a single
    /// string value back from a user
    /// </summary>
    public class InputFormTextBox
    {
        private readonly IControlFactory _controlFactory;
        private readonly string _message;
        private readonly ITextBox _textBox;

        /// <summary>
        /// Initialises the form with a message to display to the user.
        /// </summary>
        /// <param name="controlFactory">The <see cref="IControlFactory"/> to use to create the form</param>
        /// <param name="message">The message to display</param>
        /// <param name="numLines">The number of lines to make available</param>
        /// <param name="passwordChar">The Char to use if the Textbox is to be used as a password field</param>
        public InputFormTextBox(IControlFactory controlFactory, string message, int numLines, char passwordChar)
        {
            _controlFactory = controlFactory;
            _message = message;
            _textBox = _controlFactory.CreateTextBox();
            _textBox.PasswordChar = passwordChar;
            if (numLines > 1)
            {
                _textBox.Multiline = true;
                _textBox.Height = _textBox.Height * numLines;
                _textBox.ScrollBars = ScrollBars.Vertical;
            }
        }

        /// <summary>
        /// Initialises the form with a message to display to the user.
        /// </summary>
        /// <param name="controlFactory">The <see cref="IControlFactory"/> to use to create the form</param>
        /// <param name="message">The message to display</param>
        /// <param name="numLines">The number of lines to make available</param>
        public InputFormTextBox(IControlFactory controlFactory, string message, int numLines)
            : this(controlFactory, message, numLines, (char)0)
        {
        }

        /// <summary>
        /// Initialises the form with a message to display to the user.
        /// </summary>
        /// <param name="controlFactory">The <see cref="IControlFactory"/> to use to create the form</param>
        /// <param name="message">The message to display</param>
        public InputFormTextBox(IControlFactory controlFactory, string message)
            : this(controlFactory, message, 1)
        {
        }

        /// <summary>
        /// Gets the TextBox control
        /// </summary>
        public ITextBox TextBox
        {
            get { return _textBox; }
        }

        /// <summary>
        /// Gets the message to display to the user
        /// </summary>
        public string Message
        {
            get { return _message; }
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
            flowLayoutManager.AddControl(_textBox);
            panel.Height = _textBox.Height + label.Height;
            panel.Width = _controlFactory.CreateLabel(_message, true).PreferredWidth + 20;
            _textBox.Width = panel.Width - 30;
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
            IFormHabanero form = okCancelDialogFactory.CreateOKCancelForm(panel, "");
            return form.ShowDialog();
        }
    }
}