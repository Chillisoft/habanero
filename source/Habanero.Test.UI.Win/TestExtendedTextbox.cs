using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win
{
    [TestFixture]
    public class TestExtendedTextBox
    {

        private static IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }


        [Test]
        public void Test_Constructor()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            ExtendedTextBoxWin extendedTextBox = new ExtendedTextBoxWin(GlobalUIRegistry.ControlFactory);
            //---------------Test Result -----------------------
            ITextBox textBox = extendedTextBox.TextBox;
            Assert.IsNotNull(textBox);
            IButton button = extendedTextBox.Button;
            Assert.IsNotNull(button);
            Assert.AreEqual("...", button.Text);
            Assert.IsFalse(textBox.Enabled);
            Assert.AreEqual(extendedTextBox.Height, textBox.Height);
            Assert.Greater(button.Left, textBox.Left);
        }

        #region testSelectItem Control

        [Test]
        public void Test_GetTextBox()
        {
            //--------------- Set up test pack ------------------
            IControlFactory controlFactory = GetControlFactory();
            ExtendedTextBoxWin extendedComboBox = new ExtendedTextBoxWin(controlFactory);
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            ITextBox comboBox = extendedComboBox.TextBox;
            //--------------- Test Result -----------------------
            Assert.IsNotNull(comboBox);
        }

        [Test]
        public void Test_GetButton()
        {
            //--------------- Set up test pack ------------------
            IControlFactory controlFactory = GetControlFactory();
            ExtendedTextBoxWin extendedTextBoxWin = new ExtendedTextBoxWin(controlFactory);
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            IButton button = extendedTextBoxWin.Button;
            //--------------- Test Result -----------------------
            Assert.IsNotNull(button);
        }

        #endregion //testSelectItem Control
    }
}