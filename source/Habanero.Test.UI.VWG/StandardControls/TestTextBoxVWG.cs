using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.StandardControls
{
    [TestFixture]
    public class TestTextBoxVWG : TestTextBox
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }
        [Test]
        public void TestScrollBars_Vertical()
        {
            //---------------Set up test pack-------------------
            ITextBox textBox = GetControlFactory().CreateTextBox();
            //---------------Execute Test ----------------------
            textBox.ScrollBars = ScrollBars.Vertical;
            //---------------Test Result -----------------------
            Assert.AreEqual((int)Gizmox.WebGUI.Forms.ScrollBars.Vertical, (int)textBox.ScrollBars);
            Assert.AreEqual(ScrollBars.Vertical, textBox.ScrollBars);
            //---------------Tear Down -------------------------
        }
        [Test]
        public void TestScrollBars_Horizontal()
        {
            //---------------Set up test pack-------------------
            ITextBox textBox = GetControlFactory().CreateTextBox();
            //---------------Execute Test ----------------------
            textBox.ScrollBars = ScrollBars.Horizontal;
            //---------------Test Result -----------------------
            Assert.AreEqual((int)Gizmox.WebGUI.Forms.ScrollBars.Horizontal, (int)textBox.ScrollBars);
            Assert.AreEqual(ScrollBars.Horizontal, textBox.ScrollBars);
            //---------------Tear Down -------------------------
        }
        [Test]
        public void TestScrollBars_None()
        {
            //---------------Set up test pack-------------------
            ITextBox textBox = GetControlFactory().CreateTextBox();
            //---------------Execute Test ----------------------
            textBox.ScrollBars = ScrollBars.None;
            //---------------Test Result -----------------------
            Assert.AreEqual((int)Gizmox.WebGUI.Forms.ScrollBars.None, (int)textBox.ScrollBars);
            Assert.AreEqual(ScrollBars.None, textBox.ScrollBars);
            //---------------Tear Down -------------------------
        }
        [Test]
        public void TestScrollBars_Both()
        {
            //---------------Set up test pack-------------------
            ITextBox textBox = GetControlFactory().CreateTextBox();
            //---------------Execute Test ----------------------
            textBox.ScrollBars = ScrollBars.Both;
            //---------------Test Result -----------------------
            Assert.AreEqual((int)Gizmox.WebGUI.Forms.ScrollBars.Both, (int)textBox.ScrollBars);
            Assert.AreEqual(ScrollBars.Both, textBox.ScrollBars);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void Test_defaultTextAlignment()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            ITextBox textBox = GetControlFactory().CreateTextBox();
            //---------------Test Result -----------------------
            Assert.AreEqual(HorizontalAlignment.Left, textBox.TextAlign);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void Test_setTextAlignment_Left()
        {
            //---------------Set up test pack-------------------
            ITextBox textBox = GetControlFactory().CreateTextBox();
            //---------------Execute Test ----------------------
            textBox.TextAlign = HorizontalAlignment.Left;
            //---------------Test Result -----------------------
            Assert.AreEqual(HorizontalAlignment.Left, textBox.TextAlign);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void Test_setTextAlignment_Center()
        {
            //---------------Set up test pack-------------------
            ITextBox textBox = GetControlFactory().CreateTextBox();
            //---------------Execute Test ----------------------
            textBox.TextAlign = HorizontalAlignment.Center;
            //---------------Test Result -----------------------
            Assert.AreEqual(HorizontalAlignment.Center, textBox.TextAlign);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void Test_setTextAlignment_Right()
        {
            //---------------Set up test pack-------------------
            ITextBox textBox = GetControlFactory().CreateTextBox();
            //---------------Execute Test ----------------------
            textBox.TextAlign = HorizontalAlignment.Right;
            //---------------Test Result -----------------------
            Assert.AreEqual(HorizontalAlignment.Right, textBox.TextAlign);
            //---------------Tear Down -------------------------
        }
    }
}