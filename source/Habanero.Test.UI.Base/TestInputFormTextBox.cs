using System.Windows.Forms;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;
using ScrollBars=Habanero.UI.Base.ScrollBars;

namespace Habanero.Test.UI.Base
{
    public abstract class TestInputFormTextBox
    {
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

        protected abstract IControlFactory GetControlFactory();

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

        [Test]
        public void TestLayout()
        {
            //---------------Set up test pack-------------------
            const string message = "testMessage";
            const int numLines = 1;
            InputFormTextBox inputFormTextBox = new InputFormTextBox(GetControlFactory(), message, numLines);
            //---------------Execute Test ----------------------
            IPanel panel = inputFormTextBox.createControlPanel();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, panel.Controls.Count);
            Assert.IsInstanceOfType(typeof (ILabel), panel.Controls[0]);
            Assert.IsInstanceOfType(typeof (ITextBox), panel.Controls[1]);
            Assert.Greater(panel.Controls[0].Top, panel.Top);
            Assert.IsFalse(panel.Controls[0].Font.Bold);
            Assert.AreEqual(panel.Width, panel.Controls[1].Width + 30);
            int width = GetControlFactory().CreateLabel(message, true).PreferredWidth + 20;
            Assert.AreEqual(panel.Width, width);
            //---------------Tear Down -------------------------
        }
    }



}