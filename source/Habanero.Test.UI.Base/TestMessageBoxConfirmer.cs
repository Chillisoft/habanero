using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using Is=Rhino.Mocks.Constraints.Is;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestMessageBoxConfirmer
    {
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
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
            //runs every time any testmethod is complete
        }

        [Test]
        public void Test_Constructor()
        {
            //---------------Set up test pack-------------------
            IControlFactory controlFactory = new ControlFactoryWin();
            string title = TestUtil.GetRandomString();
            MessageBoxIcon messageBoxIcon = MessageBoxIcon.None;
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            MessageBoxConfirmer messageBoxConfirmer = new MessageBoxConfirmer(controlFactory, title, messageBoxIcon);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(IConfirmer), messageBoxConfirmer);
            Assert.AreSame(controlFactory, messageBoxConfirmer.ControlFactory);
            Assert.AreEqual(title, messageBoxConfirmer.Title);
            Assert.AreEqual(messageBoxIcon, messageBoxConfirmer.MessageBoxIcon);
        }

        [Test]
        public void Test_Confirm_True()
        {
            //---------------Set up test pack-------------------
            const string message = "Confirmer message";
            const string title = "MessageBoxTitle";
            const DialogResult dialogResultToReturn = DialogResult.Yes;
            MessageBoxIcon messageBoxIcon = TestUtil.GetRandomEnum<MessageBoxIcon>();

            MockRepository mockRepository = new MockRepository();
            IControlFactory controlFactory = SetupControlFactoryMockWithExpectation(mockRepository, message, title, messageBoxIcon, dialogResultToReturn);

            MessageBoxConfirmer messageBoxConfirmer = new MessageBoxConfirmer(controlFactory, title, messageBoxIcon);
            mockRepository.ReplayAll();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool confirmResult = messageBoxConfirmer.Confirm(message);
            //---------------Test Result -----------------------
            Assert.IsTrue(confirmResult);
            mockRepository.VerifyAll();
        }

        [Test]
        public void Test_Confirm_False()
        {
            //---------------Set up test pack-------------------
            const string message = "Confirmer message";
            const string title = "MessageBoxTitle";
            const DialogResult dialogResultToReturn = DialogResult.No;
            MessageBoxIcon messageBoxIcon = TestUtil.GetRandomEnum<MessageBoxIcon>();

            MockRepository mockRepository = new MockRepository();
            IControlFactory controlFactory = SetupControlFactoryMockWithExpectation(mockRepository, message, title, messageBoxIcon, dialogResultToReturn);

            MessageBoxConfirmer messageBoxConfirmer = new MessageBoxConfirmer(controlFactory, title, messageBoxIcon);
            mockRepository.ReplayAll();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool confirmResult = messageBoxConfirmer.Confirm(message);
            //---------------Test Result -----------------------
            Assert.IsFalse(confirmResult);
            mockRepository.VerifyAll();
        }

        [Test]
        public void Test_Confirm_True_WithDelegate()
        {
            //---------------Set up test pack-------------------
            const string message = "Confirmer message";
            const string title = "MessageBoxTitle";
            const DialogResult dialogResultToReturn = DialogResult.Yes;
            MessageBoxIcon messageBoxIcon = TestUtil.GetRandomEnum<MessageBoxIcon>();

            bool delegateWasCalled = false;
            bool confirmedParamInDelegate = false;
            ConfirmationDelegate confirmationDelegate = delegate(bool confirmed)
            {
                delegateWasCalled = true;
                confirmedParamInDelegate = confirmed;
            };

            MockRepository mockRepository = new MockRepository();
            IControlFactory controlFactory = SetupControlFactoryMockWithExpectationWithDelegate(
                mockRepository, message, title, messageBoxIcon, dialogResultToReturn, confirmationDelegate);

            MessageBoxConfirmer messageBoxConfirmer = new MessageBoxConfirmer(controlFactory, title, messageBoxIcon);

            mockRepository.ReplayAll();
            //---------------Assert Precondition----------------
            Assert.IsFalse(delegateWasCalled);
            Assert.IsFalse(confirmedParamInDelegate);
            //---------------Execute Test ----------------------
            messageBoxConfirmer.Confirm(message, confirmationDelegate);
            //---------------Test Result -----------------------
            Assert.IsTrue(delegateWasCalled);
            Assert.IsTrue(confirmedParamInDelegate);
            mockRepository.VerifyAll();
        }

        [Test]
        public void Test_Confirm_False_WithDelegate()
        {
            //---------------Set up test pack-------------------
            const string message = "Confirmer message";
            const string title = "MessageBoxTitle";
            const DialogResult dialogResultToReturn = DialogResult.No;
            MessageBoxIcon messageBoxIcon = TestUtil.GetRandomEnum<MessageBoxIcon>();

            bool delegateWasCalled = false;
            bool confirmedParamInDelegate = true;
            ConfirmationDelegate confirmationDelegate = delegate(bool confirmed)
            {
                delegateWasCalled = true;
                confirmedParamInDelegate = confirmed;
            };

            MockRepository mockRepository = new MockRepository();
            IControlFactory controlFactory = SetupControlFactoryMockWithExpectationWithDelegate(
                mockRepository, message, title, messageBoxIcon, dialogResultToReturn, confirmationDelegate);

            MessageBoxConfirmer messageBoxConfirmer = new MessageBoxConfirmer(controlFactory, title, messageBoxIcon);

            mockRepository.ReplayAll();
            //---------------Assert Precondition----------------
            Assert.IsFalse(delegateWasCalled);
            Assert.IsTrue(confirmedParamInDelegate);
            //---------------Execute Test ----------------------
            messageBoxConfirmer.Confirm(message, confirmationDelegate);
            //---------------Test Result -----------------------
            Assert.IsTrue(delegateWasCalled);
            Assert.IsFalse(confirmedParamInDelegate);
            mockRepository.VerifyAll();
        }

        private static IControlFactory SetupControlFactoryMockWithExpectation(MockRepository mockRepository, string message, string title, MessageBoxIcon messageBoxIcon, DialogResult dialogResultToReturn)
        {
            IControlFactory controlFactory = mockRepository.StrictMock<IControlFactory>();
            controlFactory.Expect(
                factory => factory.ShowMessageBox(message, title, MessageBoxButtons.YesNo, messageBoxIcon))
                .Return(dialogResultToReturn);
            return controlFactory;
        }

        private static IControlFactory SetupControlFactoryMockWithExpectationWithDelegate(
            MockRepository mockRepository, string message, string title, 
            MessageBoxIcon messageBoxIcon, DialogResult dialogResultToReturn, 
            ConfirmationDelegate confirmationDelegate)
        {
            IControlFactory controlFactory = mockRepository.StrictMock<IControlFactory>();
            controlFactory.Expect(
                factory => factory.ShowMessageBox(null, null, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Asterisk, null))
                .Return(dialogResultToReturn).Constraints(
                  Is.Equal(message), Is.Equal(title), Is.Equal(MessageBoxButtons.YesNo), 
                  Is.Equal(messageBoxIcon), Is.Anything())
                .WhenCalled(invocation => confirmationDelegate(dialogResultToReturn == DialogResult.Yes));
            return controlFactory;
        }
    }
}