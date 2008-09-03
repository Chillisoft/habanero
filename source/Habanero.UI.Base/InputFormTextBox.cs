namespace Habanero.UI.Base
{
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

        public InputFormTextBox(IControlFactory controlFactory, string message, int numLines)
            : this(controlFactory, message, numLines, (char)0)
        {
        }

        public InputFormTextBox(IControlFactory factory, string message)
            : this(factory, message, 1)
        {
        }

        public ITextBox TextBox
        {
            get { return _textBox; }
        }

        public string Message
        {
            get { return _message; }
        }

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
        public DialogResult ShowDialog()
        {
            IPanel panel = createControlPanel();
            IOKCancelDialogFactory okCancelDialogFactory = _controlFactory.CreateOKCancelDialogFactory();
            IFormChilli form = okCancelDialogFactory.CreateOKCancelForm(panel, "");
            return form.ShowDialog();
        }
    }
}