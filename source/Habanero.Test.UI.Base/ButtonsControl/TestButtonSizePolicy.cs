using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.ButtonsControl
{
    public abstract class TestButtonSizePolicy
    {
        [SetUp]
        public void SetupTest()
        {
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

        protected abstract IButtonSizePolicy CreateButtonSizePolicy();

        [Test]
        public void TestButtonWidth_ResizingAccordingToButtonText()
        {
            //---------------Set up test pack-------------------

            IButtonSizePolicy buttonSizePolicy = CreateButtonSizePolicy();
            const string buttonText = "TestMustBeLongEnoughToBeGreaterThanTwelthOfScreen";
            IControlCollection buttonCollection = GetControlFactory().CreatePanel().Controls;
            IButton btnTest = GetControlFactory().CreateButton(buttonText);
            buttonCollection.Add(btnTest);

            //---------------Execute Test ----------------------
            buttonSizePolicy.RecalcButtonSizes(buttonCollection);

            ////---------------Test Result -----------------------
            ILabel lbl = GetControlFactory().CreateLabel(buttonText);
            Assert.AreEqual(lbl.PreferredWidth + 15, btnTest.Width, "Button width is incorrect.");
        }


        [Test]
        public void TestButtonWidthTwoButtons()
        {
            //---------------Set up test pack-------------------
            IButtonSizePolicy buttonSizePolicy = CreateButtonSizePolicy();
            const string buttonText = "TestMustBeLongEnoughToBeGreaterThanTwelthOfScreen";
            IControlCollection buttonCollection = GetControlFactory().CreatePanel().Controls;
            IButton btnTest1 = GetControlFactory().CreateButton("Test");
            buttonCollection.Add(btnTest1);
            IButton btnTest2 = GetControlFactory().CreateButton(buttonText);
            buttonCollection.Add(btnTest2);

            //---------------Execute Test ----------------------
            buttonSizePolicy.RecalcButtonSizes(buttonCollection);
            ////---------------Test Result -----------------------

            ILabel lbl = GetControlFactory().CreateLabel(buttonText);
            Assert.AreEqual(lbl.PreferredWidth + 15, btnTest1.Width, "Button width is incorrect.");

            Assert.AreEqual(btnTest2.Width, btnTest1.Width, "Button width is incorrect.");
        }
    }


    [TestFixture]
    public class TestButtonSizePolicyWin : TestButtonSizePolicy
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        protected override IButtonSizePolicy CreateButtonSizePolicy()
        {
            return new ButtonSizePolicyWin(GetControlFactory());
        }

        [Test]
        public void TestButtonWidthOneSmallButton_WinOnly()
        {
            //---------------Set up test pack-------------------
            IButtonSizePolicy buttonSizePolicy = CreateButtonSizePolicy();
            IControlCollection buttonCollection = GetControlFactory().CreatePanel().Controls;
            IButton btnTest = GetControlFactory().CreateButton("A");
            buttonCollection.Add(btnTest);

            //---------------Execute Test ----------------------
            buttonSizePolicy.RecalcButtonSizes(buttonCollection);

            ////---------------Test Result -----------------------

            Assert.AreEqual(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 20, btnTest.Width,
                            "Button width is incorrect - when buttons are very small they should instead be 1 18th of screen width.");
        }
    }

}
