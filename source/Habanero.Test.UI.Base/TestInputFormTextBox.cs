using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestInputFormTextBox
    {
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestInputFormTextBoxGiz : TestInputFormTextBox
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }
        }


        [TestFixture]
        public class TestInputFormTextBoxWin : TestInputFormTextBox
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }
        }

        [Test]
        public void TestSimpleConstructor()
        {
            //---------------Set up test pack-------------------
            const string message = "testMessage";

            //---------------Execute Test ----------------------
            InputFormTextBox inputFormTextBox = new InputFormTextBox(GetControlFactory(), message);

            //---------------Test Result -----------------------
            Assert.AreEqual(message, inputFormTextBox.Message);
            Assert.AreEqual(false, inputFormTextBox.TextBox.Multiline);
            Assert.AreEqual((char) 0, inputFormTextBox.TextBox.PasswordChar);
            Assert.AreEqual("", inputFormTextBox.TextBox.Text);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            const string message = "testMessage";
            const int numLines = 1;
            const char passwordChar = '*';

            //---------------Execute Test ----------------------
            InputFormTextBox inputFormTextBox = new InputFormTextBox(GetControlFactory(), message, numLines, passwordChar);

            //---------------Test Result -----------------------
            Assert.AreEqual(message, inputFormTextBox.Message);
            Assert.AreEqual(false, inputFormTextBox.TextBox.Multiline);
            Assert.AreEqual(passwordChar, inputFormTextBox.TextBox.PasswordChar);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestMultiLine()
        {
            //---------------Set up test pack-------------------
            const string message = "testMessage";
            const int numLines = 5;

            //---------------Execute Test ----------------------
            InputFormTextBox inputFormTextBox = new InputFormTextBox(GetControlFactory(), message, numLines);

            //---------------Test Result -----------------------
            Assert.AreEqual(true, inputFormTextBox.TextBox.Multiline);
            Assert.AreEqual(ScrollBars.Vertical, inputFormTextBox.TextBox.ScrollBars);
            Assert.AreEqual(GetControlFactory().CreateTextBoxMultiLine(5).Height, inputFormTextBox.TextBox.Height);
            Assert.AreEqual((char) 0, inputFormTextBox.TextBox.PasswordChar);
            //---------------Tear Down -------------------------
        }


    }


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
                _textBox.Height = _textBox.Height*numLines;
                _textBox.ScrollBars = ScrollBars.Vertical;
            }
        }

        public InputFormTextBox(IControlFactory controlFactory, string message, int numLines)
            : this(controlFactory, message, numLines, (char) 0)
        {
        }

        public InputFormTextBox(IControlFactory factory, string message) : this(factory, message, 1)
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

        //TODO: ShowDialog - this needs an OKCancelDialogFactory first.
    }
}