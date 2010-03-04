//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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

using System;
using System.Drawing;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.UI.Base
{
    public abstract class TestButtonGroupControl
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
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
        public void Test_AddButton_ShouldCreateButtonAndAddToControl()
        {
            //---------------Set up test pack-------------------
            const string buttonText = "Test";
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, buttons.Controls.Count);
            //---------------Execute Test ----------------------
            IButton btnTest = buttons.AddButton(buttonText);
            ////---------------Test Result -----------------------
            Assert.IsNotNull(btnTest);
            Assert.AreEqual(buttonText, btnTest.Text);
            Assert.AreEqual(buttonText, btnTest.Name);
            Assert.AreEqual(1, buttons.Controls.Count);
            Assert.AreSame(btnTest, buttons.Controls[0]);
        }

        [Test]
        public void Test_AddButton_WhenTextDifferentThanName_ShouldHaveCorrectTextAndName()
        {
            //---------------Set up test pack-------------------
            const string buttonText = "Test";
            const string buttonName = "buttonName";
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, buttons.Controls.Count);
            //---------------Execute Test ----------------------
            IButton btnTest = buttons.AddButton(buttonName, buttonText, null);
            //---------------Test Result -----------------------
            Assert.IsNotNull(btnTest);
            Assert.AreEqual(buttonText, btnTest.Text);
            Assert.AreEqual(buttonName, btnTest.Name);
            Assert.AreEqual(1, buttons.Controls.Count);
        }

        [Test]
        public void Test_AddButton_ShouldLinkUpClickDelegate()
        {
            //---------------Set up test pack-------------------
            const string buttonText = "Test";
            const string buttonname = "buttonName";
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            bool clicked = false;
            //---------------Execute Test ----------------------
            IButton btnTest = buttons.AddButton(buttonname, buttonText, delegate { clicked = true; });
            ////---------------Test Result -----------------------
            btnTest.PerformClick();
            Assert.IsTrue(clicked);
        }

        [Test]
        public void Test_AddButtons_ShouldAddToControls()
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
        public void Test_AddButton_WithTwoButtons_ShouldHaveTwoControls()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            AddControlToForm(buttons);
            IButton btn = buttons.AddButton("Test");
            //---------------Execute Test ----------------------
            buttons.AddButton("Test2");
            //---------------Test Result -----------------------
            Assert.AreEqual(2, buttons.Controls.Count);
            Assert.AreSame(btn, buttons.Controls[0]);
        }

        [Test]
        public void Test_ButtonsIndexer_ShouldReturnButtonByName()
        {
            //---------------Set up test pack-------------------
            const string buttonName = "Test";
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            IButton btn = buttons.AddButton(buttonName);
            //---------------Execute Test ----------------------
            IButton returnedButton = buttons[buttonName];
            //---------------Test Result -----------------------
            Assert.AreSame(btn, returnedButton);
        }

        [Test]
        public void Test_ButtonsIndexer_WhenManyButtons_ShouldReturnButtonByName()
        {
            //---------------Set up test pack-------------------
            const string buttonName = "Test";
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            buttons.AddButton(TestUtil.GetRandomString());
            IButton btn = buttons.AddButton(buttonName);
            buttons.AddButton(TestUtil.GetRandomString());
            //---------------Execute Test ----------------------
            IButton returnedButton = buttons[buttonName];
            //---------------Test Result -----------------------
            Assert.AreSame(btn, returnedButton);
        }

        [Test]
        public void Test_ButtonsIndexer_WhenNotFound_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            const string buttonName = "Test";
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            IButton btn = buttons.AddButton(buttonName);
            //---------------Execute Test ----------------------
            IButton returnedButton = buttons["NotFound"];
            //---------------Test Result -----------------------
            Assert.IsNull(returnedButton);
        }

        [Test]
        public void Test_ButtonIndexer_WithSpecialCharactersInTheName_ShouldReturnCorrectButton()
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
        // Note: Mark- This test seems pointless. Why do we need it?
        public void Test_HideButton()
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
        public void Test_AddButton_WithCustomButtonEventHandler()
        {
            //---------------Set up test pack-------------------
            const string buttonName = "Test";
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            //---------------Execute Test ----------------------
            IButton btnTest = buttons.AddButton(buttonName, delegate {  });
            //---------------Test Result -----------------------
            Assert.IsNotNull(btnTest);
            Assert.AreEqual(buttonName, btnTest.Text);
            Assert.AreEqual(1, buttons.Controls.Count);
            Assert.AreEqual(buttonName, btnTest.Name);
        }

        [Test]
        public void Test_CustomButtonEventHandler()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            EventHandler eventHandler = MockRepository.GenerateStub<EventHandler>();
            IButton btn = buttons.AddButton("Test", eventHandler);
            //---------------Execute Test ----------------------
            btn.PerformClick();
            //---------------Test Result -----------------------
            eventHandler.AssertWasCalled(handler => handler(Arg<object>.Is.Same(btn), Arg<EventArgs>.Is.NotNull));
        }

        [Test]
        public void Test_CustomButtonEventHandler_WhenExceptionThrown_ShouldBeCaughtByUIExceptionNotifier()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttons = GetControlFactory().CreateButtonGroupControl();
            RecordingExceptionNotifier recordingExceptionNotifier = new RecordingExceptionNotifier();
            GlobalRegistry.UIExceptionNotifier = recordingExceptionNotifier;
            bool clickEventFired = false;
            Exception exception = new Exception();
            IButton btn = buttons.AddButton("Test", delegate
            {
                clickEventFired = true;
                throw exception;
            });
            //---------------Execute Test ----------------------
            btn.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsTrue(clickEventFired, "The click event should have fired");
            Assert.AreEqual(1, recordingExceptionNotifier.Exceptions.Count);
            Assert.AreSame(exception, recordingExceptionNotifier.Exceptions[0].Exception);
            Assert.AreSame("Error performing action", recordingExceptionNotifier.Exceptions[0].FurtherMessage);
            Assert.AreSame("Error", recordingExceptionNotifier.Exceptions[0].Title);
        }

        [Test]
        public void Test_AddButton_ShouldBeRightAligned()
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
        public void Test_AddButton_WhenButtonBiggerthanGroup_ShouldBeRightAligned()
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
        public void Test_ButtonSizePolicy_GetAndSet()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttonGroupControl = GetControlFactory().CreateButtonGroupControl();
            IButtonSizePolicy buttonSizePolicy = MockRepository.GenerateStub<IButtonSizePolicy>();
            //---------------Execute Test ----------------------
            buttonGroupControl.ButtonSizePolicy = buttonSizePolicy;
            //---------------Test Result -----------------------
            Assert.AreSame(buttonSizePolicy, buttonGroupControl.ButtonSizePolicy);
        }
        
        [Test]
        public void Test_ButtonSizePolicy_ShouldBeUsedByButtonGroupControl()
        {
            //---------------Set up test pack-------------------
            IButtonGroupControl buttonGroupControl = GetControlFactory().CreateButtonGroupControl();
            IButtonSizePolicy buttonSizePolicy = MockRepository.GenerateStub<IButtonSizePolicy>();
            buttonGroupControl.ButtonSizePolicy = buttonSizePolicy;
            //---------------Execute Test ----------------------
            buttonGroupControl.AddButton("");
            //---------------Test Result -----------------------
            buttonSizePolicy.AssertWasCalled(policy => policy.RecalcButtonSizes(Arg<IControlCollection>.Is.NotNull));
        }
    }
}