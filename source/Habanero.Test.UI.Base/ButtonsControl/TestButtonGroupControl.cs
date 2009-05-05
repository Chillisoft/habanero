//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestButtonGroupControl
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        protected abstract IControlFactory GetControlFactory();
        protected abstract void AddControlToForm(IControlHabanero cntrl);

        [TestFixture]
        public class TestButtonControlWin : TestButtonGroupControl
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }

            protected override void AddControlToForm(IControlHabanero cntrl)
            {
                System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
                frm.Controls.Add((System.Windows.Forms.Control) cntrl);
            }

            //    //protected override IReadOnlyGridControl CreateReadOnlyGridControl()
            //    //{
            //    //    ReadOnlyGridControlWin readOnlyGridControlWin = new ReadOnlyGridControlWin();
            //    //    System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            //    //    frm.Controls.Add(readOnlyGridControlWin);
            //    //    return readOnlyGridControlWin;
            //    //}
            [Test]
            public void TestSetDefaultButton_WinOnly()
            {
                //---------------Set up test pack-------------------
                IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
                IButton btn = buttons.AddButton("Test");
                System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
                frm.Controls.Add((System.Windows.Forms.Control)buttons);
                //---------------Execute Test ----------------------
                buttons.SetDefaultButton("Test");
                //---------------Test Result -----------------------
                Assert.AreSame(btn, frm.AcceptButton);
            }

            [Test]
            public void TestUseMnemonic_WinOnly()
            {
                //---------------Set up test pack-------------------
                IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();

                //---------------Execute Test ----------------------
                System.Windows.Forms.Button btn = (System.Windows.Forms.Button)buttons.AddButton("Test", delegate { });
                //---------------Test Result -----------------------
                Assert.IsTrue(btn.UseMnemonic);
            }

            [Test]
            public void TestButtonWidthOneSmallButton_WinOnly()
            {
                //---------------Set up test pack-------------------

                IButtonGroupControl buttonGroupControl = GetControlFactory().CreateButtonGroupControl();
                //---------------Execute Test ----------------------
                IButton btnTest = buttonGroupControl.AddButton("A");
                ////---------------Test Result -----------------------

                Assert.AreEqual(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 20, btnTest.Width,
                                "Button width is incorrect - when buttons are very small they should instead be 1 18th of screen width.");
            }
            //}

            [Test]
            public void TestButtonIndexer_WithASpecialCharactersInTheName_Failing()
            {
                //---------------Set up test pack-------------------
                IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
                //---------------Execute Test ----------------------
                const string buttonText = "T est@";
                IButton btn = buttons.AddButton(buttonText);
                //---------------Test Result -----------------------
                Assert.AreSame(btn, buttons["T est@"]);
            }
        }

        [TestFixture]
        public class TestButtonControlVWG : TestButtonGroupControl
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryVWG();
            }

            protected override void AddControlToForm(IControlHabanero cntrl)
            {
                Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
                frm.Controls.Add((Gizmox.WebGUI.Forms.Control) cntrl);
            }

            //TODO_: Note_ this code is not expected to pass in gizmox 
            // we need to learn how to set this up and the change test assert
            // to commented out assert.
            [Test]
            public void TestSetDefaultButton()
            {
                //---------------Set up test pack-------------------
                IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
                buttons.AddButton("Test");
                Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
                frm.Controls.Add((Gizmox.WebGUI.Forms.Control) buttons);
                //---------------Execute Test ----------------------
                buttons.SetDefaultButton("Test");
                //---------------Test Result -----------------------
                Assert.AreSame(null, frm.AcceptButton);
                //Assert.AreSame(btn, frm.AcceptButton);
            }
            [Test]
            public void TestButtonIndexer_WithASpecialCharactersInTheName_Failing()
            {
                //---------------Set up test pack-------------------
                IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
                //---------------Execute Test ----------------------
                const string buttonText = "T est@";
                IButton btn = buttons.AddButton(buttonText);
                //---------------Test Result -----------------------
                Assert.AreSame(btn, buttons["T est"]);
            }
        }

        [Test]
        public void TestCreateButtonGroupControl()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IControlHabanero buttons = GetControlFactory().CreateButtonGroupControl();
            ////---------------Test Result -----------------------
            Assert.IsNotNull(buttons);
            Assert.IsTrue(buttons is IButtonGroupControl);
            IButton btn = GetControlFactory().CreateButton();
            Assert.AreEqual(10 + btn.Height, buttons.Height);
        }

        [Test]
        public void TestAddButton()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            //---------------Execute Test ----------------------
            const string buttonText = "Test";
            IButton btnTest = buttons.AddButton(buttonText);
            ////---------------Test Result -----------------------
            Assert.IsNotNull(btnTest);
            Assert.AreEqual(buttonText, btnTest.Text);
            Assert.AreEqual(1, buttons.Controls.Count);
            Assert.AreEqual(buttonText, btnTest.Name);
        }

        [Test]
        public void TestAddButton_TextDifferentThanName()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            //---------------Execute Test ----------------------
            const string buttonText = "Test";
            const string buttonname = "buttonName";
            bool clicked = false;
            IButton btnTest = buttons.AddButton(buttonname, buttonText, delegate { clicked = true; });

            btnTest.PerformClick();

            ////---------------Test Result -----------------------
            Assert.IsNotNull(btnTest);
            Assert.AreEqual(buttonText, btnTest.Text);
            Assert.AreEqual(1, buttons.Controls.Count);
            Assert.AreEqual(buttonname, btnTest.Name);

            Assert.IsTrue(clicked);
        }

        [Test]
        public void TestButtonsIndexer()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            //---------------Execute Test ----------------------
            IButton btn = buttons.AddButton("Test");
            //---------------Test Result -----------------------
            Assert.AreSame(btn, buttons["Test"]);
        }

        [Test]
        public void TestButtons_ControlsMethod()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            AddControlToForm(buttons);
            //---------------Execute Test ----------------------
            IButton btn = buttons.AddButton("Test");
            //---------------Test Result -----------------------
            Assert.AreEqual(1, buttons.Controls.Count);
            Assert.AreSame(btn, buttons.Controls[0]);
        }

        [Test]
        public void TestAdd_2Buttons_ControlsMethod()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            AddControlToForm(buttons);
            //---------------Execute Test ----------------------
            IButton btn = buttons.AddButton("Test");
            buttons.AddButton("Test2");

            //---------------Test Result -----------------------
            Assert.AreEqual(2, buttons.Controls.Count);
            Assert.AreSame(btn, buttons.Controls[0]);
        }

        [Test]
        public void TestButtonIndexer_WithASpecialCharactersInTheName()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            //---------------Execute Test ----------------------
            const string buttonText = "T est%_&^ #$�<>()!:;.,?[]+-=*/'";
            IButton btn = buttons.AddButton(buttonText);
            //---------------Test Result -----------------------
            Assert.AreSame(btn, buttons[buttonText]);
        }


        [Test]
        public void TestHideButton()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            //---------------Execute Test ----------------------
            IButton btn = buttons.AddButton("Test");
            //---------------Test Result -----------------------
            Assert.IsTrue(btn.Visible);
            buttons["Test"].Visible = false;
            Assert.IsFalse(btn.Visible);
        }

        [Test]
        public void TestAddButton_CustomButtonEventHandler()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            //---------------Execute Test ----------------------
            const string buttonName = "Test";
            IButton btnTest = buttons.AddButton(buttonName, delegate {  });
            //---------------Test Result -----------------------
            Assert.IsNotNull(btnTest);
            Assert.AreEqual(buttonName, btnTest.Text);
            Assert.AreEqual(1, buttons.Controls.Count);
            Assert.AreEqual(buttonName, btnTest.Name);
        }

        [Test]
        public void TestCustomButtonEventHandler()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            bool clickEventFired = false;
            IButton btn = buttons.AddButton("Test", delegate { clickEventFired = true; });

            //---------------Execute Test ----------------------
            btn.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsTrue(clickEventFired, "The click event did not fire");
        }

        [Test]
        public void TestAddButton_RightAlignment()
        {
            //---------------Set up test pack-------------------

            IButtonGroupControl buttonGroupControl = GetControlFactory().CreateButtonGroupControl();
            buttonGroupControl.Width = 200;
            //---------------Execute Test ----------------------
            IButton btnTest = buttonGroupControl.AddButton("Test");
            ////---------------Test Result -----------------------

            Assert.AreEqual(buttonGroupControl.Width - 5 - btnTest.Width, btnTest.Left,
                "Button should be right aligned.");
        }

        [Test]
        public void TestAddButton_RightAlignment_ButtonBiggerthanGroup()
        {
            //---------------Set up test pack-------------------

            IButtonGroupControl buttonGroupControl = GetControlFactory().CreateButtonGroupControl();
            buttonGroupControl.Width = 50;
            //---------------Execute Test ----------------------
            IButton btnTest = buttonGroupControl.AddButton("Test");
            ////---------------Test Result -----------------------
            //Wierd the button gonna be off the screen to the left maybe flow layout manager should do something else maybe not who knows
            Assert.AreEqual(buttonGroupControl.Width - 5 - btnTest.Width, btnTest.Left,
                "Button should be right aligned.");
        }

        [Test]
        public void TestButtonWidth_ResizingAccordingToButtonText()
        {
            //---------------Set up test pack-------------------

            IButtonGroupControl buttonGroupControl = GetControlFactory().CreateButtonGroupControl();
            const string buttonText = "TestMustBeLongEnoughToBeGreaterThanTwelthOfScreen";
            //---------------Execute Test ----------------------
            IButton btnTest = buttonGroupControl.AddButton(buttonText);
            ////---------------Test Result -----------------------

            ILabel lbl = GetControlFactory().CreateLabel(buttonText);
            Assert.AreEqual(lbl.PreferredWidth + 10, btnTest.Width, "Button width is incorrect.");
        }


        [Test]
        public void TestButtonWidthTwoButtons()
        {
            //---------------Set up test pack-------------------

            IButtonGroupControl buttonGroupControl = GetControlFactory().CreateButtonGroupControl();
            const string buttonText = "TestMustBeLongEnoughToBeGreaterThanTwelthOfScreen";
            //---------------Execute Test ----------------------
            IButton btnTest1 = buttonGroupControl.AddButton("Test");
            IButton btnTest2 = buttonGroupControl.AddButton(buttonText);
            ////---------------Test Result -----------------------

            ILabel lbl = GetControlFactory().CreateLabel(buttonText);
            Assert.AreEqual(lbl.PreferredWidth + 10, btnTest1.Width, "Button width is incorrect.");

            Assert.AreEqual(btnTest2.Width, btnTest1.Width, "Button width is incorrect.");
        }
    }
}