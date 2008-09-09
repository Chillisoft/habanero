using System;
using System.Collections.Generic;
using System.Text;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestTextBox
    {
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestTextBoxWin : TestTextBox
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }

            [Test]
            public void TestScrollBars_Vertical()
            {
                //---------------Set up test pack-------------------
                ITextBox textBox = GetControlFactory().CreateTextBox();
                //---------------Execute Test ----------------------
                textBox.ScrollBars = ScrollBars.Vertical;
                //---------------Test Result -----------------------
                Assert.AreEqual((int)System.Windows.Forms.ScrollBars.Vertical, (int)textBox.ScrollBars);
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
                Assert.AreEqual((int)System.Windows.Forms.ScrollBars.Horizontal, (int)textBox.ScrollBars);
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
                Assert.AreEqual((int)System.Windows.Forms.ScrollBars.None, (int)textBox.ScrollBars);
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
                Assert.AreEqual((int)System.Windows.Forms.ScrollBars.Both, (int)textBox.ScrollBars);
                Assert.AreEqual(ScrollBars.Both, textBox.ScrollBars);
                //---------------Tear Down -------------------------
            }

        }

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

        }

   

    }
}
